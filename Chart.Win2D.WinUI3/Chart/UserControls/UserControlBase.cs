using Chart.Framework;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using ChartBase.utils;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChartBase.Chart.UserControls;

/*
 * Lifecycle Event Lists:
 * 
 * 1. UserControl.SizeChanged
 * 2. UserControl.Loaded
 * 3. CanvasControl.CreateResources
 * 4. DependencyProperty value attached
 * 5. UserControl.Unloaded
 */




/*
 * Change Size or Move Page:
 * 
 * 1. UserControl.SizeChanged
 * 2. CanvasControl.CreateResources
 * 
 */

public abstract class UserControlBase: UserControl
{
    public UserControlBase()
    {
        SceneManager = new MySceneManager();
        InputManager = new InputManager();
        SceneManager.InputManager = InputManager;
    }

    public MySceneManager SceneManager { get; private set; }
    public InputManager InputManager { get; set; }
    protected bool PageLoaded { get; set; }
    protected abstract bool IsCanvasValid();

    protected abstract IControlContainer CreateMainContainerChartControl();
    protected abstract void ProcessSecondaryChartData();
    protected abstract CanvasControl GetCanvasControl();
    protected abstract SceneBase CreatePrimaryScene();
    protected abstract float GetActualWidth();
    protected abstract float GetActualHeight();
    protected abstract void BeforeCanvasInvalidate();
    protected abstract void PrimarySeriesDataChanged();
    protected abstract void SecondarySeriesDataChanged();

    #region Dependencies


    public ChartDataInput PrimaryChartData
    {
        get { return (ChartDataInput)GetValue(SyntheticalDataInputProperty); }
        set { SetValue(SyntheticalDataInputProperty, value); }
    }

    public static readonly DependencyProperty SyntheticalDataInputProperty =
        DependencyProperty.Register(
            nameof(PrimaryChartData),
            typeof(ChartDataInput),
            typeof(UserControlBase),
            new PropertyMetadata(null, new PropertyChangedCallback(PrimarySeriesDataChanged))
        );


    public List<ChartDataInput> SecondaryChartData
    {
        get { return (List<ChartDataInput>)GetValue(SecondaryChartDataProperty); }
        set { SetValue(SecondaryChartDataProperty, value); }
    }

    public static readonly DependencyProperty SecondaryChartDataProperty =
        DependencyProperty.Register(
            nameof(SecondaryChartData),
            typeof(List<ChartDataInput>),
            typeof(UserControlBase),
            new PropertyMetadata(null, new PropertyChangedCallback(SecondarySeriesDataChanged))
        );
    private static void SecondarySeriesDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Logger.WriteLine(typeof(UserControlBase), new StackTrace(0, true), "");
        UserControlBase ucb = d as UserControlBase;
        ucb.SecondarySeriesDataChanged();
        ucb.Update();
    }

    private static void PrimarySeriesDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        Logger.WriteLine(typeof(UserControlBase), new StackTrace(0, true), "");
        UserControlBase ucb = d as UserControlBase;
        ucb.PrimarySeriesDataChanged();
        ucb.Update();
    }
        

    public static readonly DependencyProperty CombinationProperty = DependencyProperty.Register(nameof(Combination), typeof(CombinationType), typeof(UserControlBase),
    new PropertyMetadata(CombinationType.Combination, new PropertyChangedCallback(Combination_Changed)));
    private static void Combination_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var value = e.NewValue;
    }

    public CombinationType Combination
    {
        get { return (CombinationType)GetValue(CombinationProperty); }
        set { SetValue(CombinationProperty, value); }
    }


    public UiBool CompressYLabel
    {
        get { return (UiBool)GetValue(CompressYLabelProperty); }
        set { SetValue(CompressYLabelProperty, value); }
    }

    public static readonly DependencyProperty CompressYLabelProperty =
        DependencyProperty.Register(
            nameof(CompressYLabel),
            typeof(UiBool),
            typeof(UserControlBase),
            new PropertyMetadata(UiBool.Unknow)
        );


    public UiBool TrimDecimal
    {
        get { return (UiBool)GetValue(TrimDecimalProperty); }
        set { SetValue(TrimDecimalProperty, value); }
    }

    public static readonly DependencyProperty TrimDecimalProperty =
        DependencyProperty.Register(
            nameof(TrimDecimal),
            typeof(UiBool),
            typeof(UserControlBase),
            new PropertyMetadata(default(UiBool))
        );

    #endregion

    #region Event

    protected virtual void UserControlBase_Loaded(object sender, RoutedEventArgs e)
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), "");
        PageLoaded = true;
    }

    protected virtual void UserControlBase_Unloaded(object sender, RoutedEventArgs e)
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), "");
        PageLoaded = false;
    }

    #endregion


    #region Methods

    protected void Update()
    {        
        if (!PageLoaded)
            return;
        if (!IsCanvasValid())
            return;

        CanvasControl canvas = GetCanvasControl();

        SceneManager.Reset();
        SceneManager.CanvasSizeChanged(GetActualWidth(), GetActualHeight());

        // 3.1 If PrimaryChartData is null, return;
        //if (PrimaryChartData != null && PrimaryChartData.InputDataSeries != null && PrimaryChartData.InputDataSeries.Count > 0)
        if (PrimaryChartData != null)
        {
            SeriesDataChanged();
        }
        canvas.Invalidate();
    }

    /*
     * Initialize Canvas status
     * 
     */
    protected virtual void SeriesDataChanged()
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), "");


        // 1. Clear Scenes
        //SceneManager.Reset();
        

        BeforeSetupCanvasAndUpdateData();
        // 4. Process Primary Chart data
        ProcessPrimaryChartData();

        // 5. Secondary chart data
        ProcessSecondaryChartData();

        // 6. Update data and coordinate
        //UpdateDataAndResource();
        CanvasControl canvas = GetCanvasControl();

        CalcCoordinate(canvas);

        UpdateElementCoordinate(canvas, new GenericInput("Unknow") { Creator = canvas });
        BeforeCanvasInvalidate();       

    }

    /*
     * Do preprocessing about input arguments before it to be processed
     */
    protected virtual void BeforeSetupCanvasAndUpdateData()
    {
        VerifyDataValidation();
        
    }

    /*
     * Verify id input data is valid. If data is not valid, a Exception will be raised.
     * 
     * Exception:
     *  InvalidArgumentsException
     */
    protected virtual bool VerifyDataValidation() {
        if (PrimaryChartData == null || PrimaryChartData.InputDataSeries == null || PrimaryChartData.InputDataSeries.Count == 0)
            return false;
        var dataPointSize = new HashSet<int>();
        foreach (var series in PrimaryChartData.InputDataSeries)
        {
            if (series.DataPoints == null)
                continue;
            if(series.DataPoints.Length > 0)
                dataPointSize.Add(series.DataPoints.Length);
        }
        if(PrimaryChartData.HorizentalTicks != null && PrimaryChartData.HorizentalTicks.Length > 0)
            dataPointSize.Add(PrimaryChartData.HorizentalTicks.Length);

        if (dataPointSize.Count > 1)
            throw new ArgumentException("Length of data point in serieses is not consistent");

        if (PrimaryChartData.HorizentalTicks == null || PrimaryChartData.HorizentalTicks.Length == 0)
        {
            string[] ticks = new string[PrimaryChartData.GetDataPointSizeOfSeries()];
            for (int i = 0; i < ticks.Length; i++)
            {
                ticks[i] = $"{i}";
            }
            PrimaryChartData.HorizentalTicks = ticks;
        }
        return true;
    }

    protected virtual void ProcessPrimaryChartData()
    {
        // Create Primary Scene
        SceneBase primaryScene = CreatePrimaryScene();
        // Set property IsPrimaryScene to true
        primaryScene.IsPrimaryScene = true;

        // Create Main ChartControl
        IControlContainer mainChartControl = CreateMainContainerChartControl();

        // Set ChartControl to PrimaryScene
        primaryScene.SetContainerControl(mainChartControl);

        // Add Primary Scene to SceneManager
        SceneManager.SetPrimaryScene(primaryScene);



        // 4. Get global Legend info Axises information
        var legend = this.GetLegendInfo();
        SceneManager.GlobalLegend = legend;
        if (PrimaryChartData.ShowLegend && SceneManager.GlobalLegend.ShowLegend)
        {
            SceneManager.GlobalLegend.ShowLegend = true;
        }
        else
        {
            SceneManager.GlobalLegend.ShowLegend = false;
        }

        if (CompressYLabel != UiBool.Unknow)
        {
            PrimaryChartData.CompressYLabel = CompressYLabel;
        }

        if (TrimDecimal != UiBool.Unknow)
        {
            PrimaryChartData.TrimDecimal = TrimDecimal;
        }

        if (Combination != CombinationType.Unknown)
        {
            PrimaryChartData.Combination = Combination;
        }

        // 5. Update input arguments to primary scene
        SceneManager.UpdatePrimarySceneData(PrimaryChartData);
    }


    protected void CalcCoordinate(CanvasControl MyCanvas)
    {

        if (SceneManager.CanUpdate())
        {
            SceneManager.CalcEachLegendSpace(MyCanvas);

            SceneManager.CalcCanvasBoardSize();

            SceneManager.CalcViewWorldScale();

            SceneManager.CalcControlCoordinate(MyCanvas);
        }
    }

    protected void UpdateElementCoordinate(CanvasControl MyCanvas, GenericInput genericInput)
    {
        if (SceneManager.CanUpdate() && MyCanvas.ActualHeight > 0 && MyCanvas.ActualWidth > 0)
        {     
            SceneManager.Update(genericInput, TimeSpan.Zero);
        }
    }


    protected Legend GetLegendInfo()
    {
        Legend legendInfo = new Legend();
        List<LegendItem> legends = new List<LegendItem>();
        if (PrimaryChartData != null && PrimaryChartData.ShowLegend)
        {
            legends.AddRange(PrimaryChartData.GetLegendInfo());
        }
        if (SecondaryChartData != null && SecondaryChartData.Count > 0)
        {
            foreach (var legend in SecondaryChartData)
            {
                if (legend.ShowLegend)
                    legends.AddRange(legend.GetLegendInfo());
            }
        }
        legendInfo.LegendList = legends;
        if(legends.Count > 0)
            legendInfo.ShowLegend = true;
        return legendInfo;
    }

    #endregion

}

