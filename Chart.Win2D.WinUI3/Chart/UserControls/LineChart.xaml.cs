using Chart.Framework;
using ChartBase.Chart;
using ChartBase.Chart.Controls;
using ChartBase.Chart.Scenes;
using ChartBase.Chart.UserControls;
using ChartBase.utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using System;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChartBase.Chart.UserControls;

public sealed partial class LineChart : UserControlBase
{
    public LineChart()
    {
        StackFrame sf = (new StackTrace(0, true)).GetFrame(0);
        Logger.WriteLine($"{this.GetType()}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}", "");

        this.InitializeComponent();

        this.SizeChanged += BoardSize_Changed;        
    }

    private void MyCanvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), "");

        Update();
    }

    private void MyCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
        //Logger.WriteLine(GetType(), new StackTrace(0, true), "");

        PointerPoint p = e.GetCurrentPoint((UIElement)sender);

        MouseGenericInput mgi = new MouseGenericInput((float)p.Position.X, (float)p.Position.Y);
        mgi.Name = "mouse_move";
        mgi.MouseInputType = MouseGenericInputType.MouseMove;
        mgi.IsLeftButtonPress = p.Properties.IsLeftButtonPressed;
        mgi.IsMiddleButtonPress = p.Properties.IsMiddleButtonPressed;
        mgi.IsRightButtonPress = p.Properties.IsRightButtonPressed;
        mgi.MouseDown = mgi.IsLeftButtonPress | mgi.IsMiddleButtonPress | mgi.IsRightButtonPress;

        if (sender is CanvasControl)
        {
            CanvasControl creator = sender as CanvasControl;
            mgi.Creator = creator;
        }

        InputManager.AddInputItem(mgi);
    }

    //private void UserControlBase_Loaded(object sender, RoutedEventArgs e)
    //{
    //    Logger.WriteLine(GetType(), new StackTrace(0, true), "");
    //    PageLoaded = true;
    //}

    //private void UserControlBase_Unloaded(object sender, RoutedEventArgs e)
    //{
    //    Logger.WriteLine(GetType(), new StackTrace(0, true), "");
    //    PageLoaded = false;
    //}

    private void BoardSize_Changed(object sender, SizeChangedEventArgs e)
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), $"Actual Width: {(float)MyCanvas.ActualWidth}  ActualHeight: {(float)MyCanvas.ActualHeight}");

        //SceneManager.CleanElements();
        //SceneManager.CanvasSizeChanged((float)MyCanvas.ActualWidth, (float)MyCanvas.ActualHeight);
        //UpdateDataAndResource();
        Update();
    }



    private void MyCanvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), "");
        if (SceneManager.CanDraw())
        {
            CanvasDrawingSession cds = args.DrawingSession;
            SceneManager.Draw(cds);
        }
    }



    //protected override void UpdateDataAndResource()
    //{        
    //    Logger.WriteLine(GetType(), new StackTrace(0, true), $"{Environment.NewLine}{Environment.StackTrace}");
    //    CalcCoordinate(MyCanvas);
    //    UpdateCanvasData(MyCanvas, new GenericInput("Unknow") { Creator = MyCanvas });
    //    MyCanvas.Invalidate();
    //}

    protected override IControlContainer CreateMainContainerChartControl()
    {
        return new LineChartControl();
    }

    protected override bool IsCanvasValid()
    {
        return MyCanvas.ActualHeight > 0 && MyCanvas.ActualWidth > 0;
    }
    protected override void ProcessSecondaryChartData()
    {
        if (SecondaryChartData != null && SecondaryChartData.Count > 0)
        {
            var tmpScene = new CombinationScene(SceneManager) { IsPrimaryScene = false };
            SceneManager.AddScene(tmpScene);
            foreach (var chartData in SecondaryChartData)
            {
                if (chartData == null || chartData.InputDataSeries == null || chartData.InputDataSeries.Count == 0)
                    continue;

                tmpScene.SetContainerControl(new LineChartControl());
                tmpScene.DataTransformation(chartData);
            }
        }
    }

    protected override SceneBase CreatePrimaryScene()
    {
        return new CombinationScene(SceneManager);
    }

    protected override float GetActualWidth()
    {
        return (float)MyCanvas.ActualWidth;        
    }

    protected override float GetActualHeight()
    {
        return (float)MyCanvas.ActualHeight;
    }

    protected override CanvasControl GetCanvasControl()
    {
        return MyCanvas;
    }

    protected override void BeforeCanvasInvalidate()
    {

    }

    protected override void PrimarySeriesDataChanged()
    {
        
    }

    protected override void SecondarySeriesDataChanged()
    {
        
    }
}

public class ControlHovered : EventArgs
{
    public ControlBase Control { get; set; }
    public GenericInput GenericInput { get; set; }

}






