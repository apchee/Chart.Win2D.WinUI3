using ChartBase.Chart;
using ChartBase.Chart.Scenes;
using ChartBase.Chart.UserControls;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;
using ChartBase.utils;
using System.Diagnostics;
using Chart.Framework;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ChartBase.Chart.UserControls;
public sealed partial class BarChart : UserControlBase
{
    public CombinationScene PrimaryScene { get; set; }

    public BarChart()
    {
        this.InitializeComponent();
        this.SizeChanged += BoardSize_Changed;
    }

    private void BoardSize_Changed(object sender, SizeChangedEventArgs e)
    {        
        SceneManager.CanvasSizeChanged((float)MyCanvas.ActualWidth, (float)MyCanvas.ActualHeight);
        Update();
    }
    private void MyCanvas_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
    {
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

    //protected override void UpdateDataAndResource()
    //{
    //    Logger.WriteLine(GetType(), new StackTrace(0, true), $"{Environment.NewLine}{Environment.StackTrace}");
    //    CalcCoordinate(MyCanvas);
    //    UpdateCanvasData(MyCanvas, new GenericInput("Unknow") { Creator = MyCanvas });
    //    MyCanvas.Invalidate();
    //}

   
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

    protected override IControlContainer CreateMainContainerChartControl()
    {
        return new BarChartControl();
    }

    protected override bool IsCanvasValid()
    {
        return MyCanvas.ActualHeight > 0 && MyCanvas.ActualWidth > 0;
    }

    private void BarChartCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
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

    private void BarChartCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
    {

    }

    private void BarChartCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
    {

    }

    private void UserControlBase_Loaded_1(object sender, RoutedEventArgs e)
    {

    }

    private void UserControlBase_Unloaded_1(object sender, RoutedEventArgs e)
    {

    }

    protected override void ProcessSecondaryChartData()
    {
       
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
