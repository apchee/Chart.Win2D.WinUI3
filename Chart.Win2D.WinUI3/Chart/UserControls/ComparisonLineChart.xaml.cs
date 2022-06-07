using Chart.Framework;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using ChartBase.utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChartBase.Chart.UserControls;

public sealed partial class ComparisonLineChart : UserControlBase, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ComparisonLineChart()
    {
        this.InitializeComponent();
    }

    private void BoardSize_Changed(object sender, SizeChangedEventArgs e)
    {
        StackFrame sf = (new StackTrace(0, true)).GetFrame(0);
        Logger.WriteLine($"{this.GetType()}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}", "");
        Update();
    }

    #region Override methods

    protected override void BeforeSetupCanvasAndUpdateData()
    {
        base.BeforeSetupCanvasAndUpdateData();
        this.Combination = Models.CombinationType.Comparision;
        PrimaryChartData.Combination = Models.CombinationType.Comparision;

        //MaxSeriesIndexSliderSteps = PrimaryChartData.GetDataPointSizeOfSeries();
    }

    protected override IControlContainer CreateMainContainerChartControl()
    {
        return new ComparisonLineChartControl();
    }

    protected override bool IsCanvasValid()
    {
        return MyCanvas.ActualHeight > 0 && MyCanvas.ActualWidth > 0;
    }


    protected override void ProcessSecondaryChartData()
    {
        // TODO

    }


    protected override SceneBase CreatePrimaryScene()
    {
        return new ComparisonLineScene(SceneManager);
    }

    protected override float GetActualWidth()
    {
        return (float)MyCanvas.ActualWidth;        
    }

    protected override float GetActualHeight()
    {
        return (float)MyCanvas.ActualHeight;
    }


    private List<PointArray<float?>> inputDataSeries;
    private string[] originalInputHorizentalTicks;
    private int dataPointSizeOfSeries;


    protected override void PrimarySeriesDataChanged()
    {
        inputDataSeries = new List<PointArray<float?>>();
        originalInputHorizentalTicks = null;
        dataPointSizeOfSeries = 0;

        isCanvasReady = false;

        if (!VerifyDataValidation())
            return;

        // Cache input series data somewhere
        foreach (var item in PrimaryChartData.InputDataSeries)
        {
            var clone = (PointArray<float?>)item.Clone();
            //float?[] dps = new float?[item.DataPoints.Length];
            //Array.Copy(item.DataPoints, 0, dps, 0, item.DataPoints.Length);
            //clone.DataPoints = dps;
            inputDataSeries.Add(clone);
        }

        originalInputHorizentalTicks = PrimaryChartData.HorizentalTicks;
        dataPointSizeOfSeries = PrimaryChartData.GetDataPointSizeOfSeries();
        MaxSeriesIndexSliderSteps = originalInputHorizentalTicks.Length - 1;

        int comparisonBaseIndexFromLast = originalInputHorizentalTicks.Length - PrimaryChartData.ComparisingIndex;
        
        if (PrimaryChartData.RangeOfBeginningIndexFromLast > 0 && originalInputHorizentalTicks.Length > PrimaryChartData.RangeOfBeginningIndexFromLast)
        {
            SeriesIndexSliderValue = originalInputHorizentalTicks.Length - PrimaryChartData.RangeOfBeginningIndexFromLast;

            RerangeSeriesScope(PrimaryChartData.RangeOfBeginningIndexFromLast);
            //PrimaryChartData.ComparisingIndex = PrimaryChartData.HorizentalTicks.Length - comparisonBaseIndexFromLast;
            CurrentValue = PrimaryChartData.ComparisingIndex;
        }


        if (PrimaryChartData.ComparisingIndex < MaxSeriesIndexSliderSteps - 1)
        {
            /*
            int originalComparisingIndex = MaxSeriesIndexSliderSteps - PrimaryChartData.ComparisingIndex;
            int showDataPointSeizeFromLast = originalComparisingIndex;
            if (showDataPointSeizeFromLast < 10)
                showDataPointSeizeFromLast = 10;
            if (showDataPointSeizeFromLast < 20)
                showDataPointSeizeFromLast = 20;
            int more = (int)(showDataPointSeizeFromLast * 0.2);
            if (more < 5)
                more = 5;
            int beginningIndex = MaxSeriesIndexSliderSteps - (showDataPointSeizeFromLast + more);
            if (beginningIndex < 0)
                beginningIndex = 0;
            int seriesIndexValue = int.Parse( PrimaryChartData.HorizentalTicks[beginningIndex]);
            PrimaryChartData.CutOffBySeriesValue(seriesIndexValue);
            SeriesIndexSliderValue = beginningIndex;

            PrimaryChartData.ComparisingIndex = PrimaryChartData.DataPointSizeOfOneSeries() - originalComparisingIndex;
         */
        }
    }

    protected override void SecondarySeriesDataChanged()
    {

    }
    #endregion

    #region Events 
    private void ComparingBasingIndexChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if (PrimaryChartData.ComparisingIndex != (int)e.NewValue)
        {
            PrimaryChartData.ComparisingIndex = (int)e.NewValue;
            BasicComparisonIndex = PrimaryChartData.ComparisingIndex;
            BasicIndexSliderViewValue.Text = $"{BasicComparisonIndex}";
            Update();
        }
    }

    private void SeriesIndexScopeSliderStepChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
        if (!isCanvasReady)
            return;
        var currentIndexFromLast = MaximumIndexOfView - CurrentValue;
        int showPointSize = dataPointSizeOfSeries - (int)e.NewValue;
        if(showPointSize < 20)
            showPointSize = 20;
        if(showPointSize > dataPointSizeOfSeries)
            showPointSize = dataPointSizeOfSeries;
        var value = ((Slider)sender).Value;
        MaximumIndexOfView = showPointSize - 1;
        CurrentValue = MaximumIndexOfView -currentIndexFromLast;
        RerangeSeriesScope(showPointSize);
        PrimaryChartData.ComparisingIndex = CurrentValue;

        Update();
    }

    private void SliberValueTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            int sliderValue = 0;
            Int32.TryParse(((TextBox)sender).Text, out sliderValue);
            if(Math.Abs(sliderValue) < SceneManager.PrimaryScene.SceneViewWin.HorizentalTickCoordinators.Length - 1 && Math.Abs(sliderValue) != PrimaryChartData.ComparisingIndex)
            {
                int tmpValue = sliderValue;
                if(sliderValue < 0)
                {
                    tmpValue = SceneManager.PrimaryScene.SceneViewWin.HorizentalTickCoordinators.Length - 1 + sliderValue;
                }
                PrimaryChartData.ComparisingIndex = tmpValue;
                BasicComparisonIndex = tmpValue;
                Update();
            }
        }
    }

    private void MyCanvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
    {        
        //SeriesDataChanged();
        Update();
    }

    private void MyCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        if (SceneManager.CanDraw())
        {
            CanvasDrawingSession cds = args.DrawingSession;
            SceneManager.Draw(cds);
        }
    }

    protected override CanvasControl GetCanvasControl()
    {
        return MyCanvas;
    }

    private bool isCanvasReady = false;

    protected override void BeforeCanvasInvalidate()
    {
        if (SceneManager.PrimaryScene.SceneViewWin.HorizentalTickCoordinators == null)
            return;
        // var baseIndexFromLast = MaximumIndexOfRange - BasicComparisonIndex;
        MaximumIndexOfView = SceneManager.PrimaryScene.SceneViewWin.HorizentalTickCoordinators.Length-1;
        //MiniumIndexOfView = PrimaryChartData.RangeOfBeginningIndexFromLast;
        CurrentValue = PrimaryChartData.ComparisingIndex;
        if (CurrentValue > MaximumIndexOfView)
        {
            CurrentValue = MaximumIndexOfView;
            PrimaryChartData.ComparisingIndex = CurrentValue;
        }
        //SliberValueInputBox.Text = $"{CurrentValue}";
        BasicComparisonIndex = CurrentValue;
        isCanvasReady = true;
    }

    protected override void UserControlBase_Unloaded(object sender, RoutedEventArgs e)
    {
        base.UserControlBase_Unloaded(sender, e);
    }

    protected override void UserControlBase_Loaded(object sender, RoutedEventArgs e)
    {
        base.UserControlBase_Loaded(sender, e);
    }

    #endregion

    private void RerangeSeriesScope(int showPointLength)
    {
        if (inputDataSeries == null || originalInputHorizentalTicks == null || dataPointSizeOfSeries == 0)
            return;

        int beginningIndex = dataPointSizeOfSeries - showPointLength;

        var tmpSeries = new List<PointArray<float?>>();

        foreach (var series in inputDataSeries)
        {
            var pointSeries = (PointArray<float?>)series.Clone();
            if(beginningIndex > 0)
            {
                float?[] space = new float?[showPointLength];
                Array.Copy(pointSeries.DataPoints, beginningIndex, space, 0, showPointLength);
                pointSeries.DataPoints = space;

            }
            tmpSeries.Add(pointSeries);
        }
        PrimaryChartData.InputDataSeries = tmpSeries;
        if (beginningIndex > 0)
        {
            string[] space = new string[showPointLength];
            Array.Copy(originalInputHorizentalTicks, beginningIndex, space, 0, showPointLength);
            PrimaryChartData.HorizentalTicks = space;
        }
        else
        {
            PrimaryChartData.HorizentalTicks = originalInputHorizentalTicks;
        }        
    }

    private int _MiniumSteps;
    public int MiniumIndexOfView
    {
        get { return _MiniumSteps; }
        set
        {
            if (_MiniumSteps != value)
            {
                _MiniumSteps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MiniumIndexOfView)));
            }

        }
    }

    private int _MaximumSteps;
    public int MaximumIndexOfView
    {
        get { return _MaximumSteps; }
        set
        {
            if (_MaximumSteps != value)
            {
                _MaximumSteps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaximumIndexOfView)));
            }

        }
    }

    private int _CurrentValue;
    public int CurrentValue
    {
        get { return _CurrentValue; }
        set
        {
            if (_CurrentValue != value)
            {
                _CurrentValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentValue)));
            }

        }
    }

    private int _MinSeriesIndexSliderSteps;
    public int MinSeriesIndexSliderSteps
    {
        get { return _MinSeriesIndexSliderSteps; }
        set
        {
            if (_MinSeriesIndexSliderSteps != value)
            {
                _MinSeriesIndexSliderSteps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MinSeriesIndexSliderSteps)));
            }

        }
    }

    private int _MaxSeriesIndexSliderSteps;
    public int MaxSeriesIndexSliderSteps
    {
        get { return _MaxSeriesIndexSliderSteps; }
        set
        {
            if (_MaxSeriesIndexSliderSteps != value)
            {
                _MaxSeriesIndexSliderSteps = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxSeriesIndexSliderSteps)));
            }

        }
    }

    private int _SeriesIndexSliderValue;
    public int SeriesIndexSliderValue
    {
        get { return _SeriesIndexSliderValue; }
        set
        {
            if (_SeriesIndexSliderValue != value)
            {
                _SeriesIndexSliderValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SeriesIndexSliderValue)));
            }

        }
    }


    public int BasicComparisonIndex
    {
        get { return (int)GetValue(BasicComparisonIndexProperty); }
        set { SetValue(BasicComparisonIndexProperty, value); }
    }

    public static readonly DependencyProperty BasicComparisonIndexProperty =
        DependencyProperty.Register(
            nameof(BasicComparisonIndex),
            typeof(int),
            typeof(ComparisonLineChart),
            new PropertyMetadata(default(int))
        );


}
