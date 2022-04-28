using Chart.Framework;
using ChartBase.Chart;
using ChartBase.Chart.Controls;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace ChartBase.Chart.Scenes;

public abstract class SceneBase: IScene
{
    protected IControlContainer _mainContainerControl;
    protected List<IControl> GlobalControls { get; set; } = new List<IControl>();


    public SceneBase(MySceneManager mg)
    {
        this.Id = ViewWindow.Uid();
        SceneManager = mg;
    }

    public BounchOfSeries<PointShape> SeriesChartData { get; set; }

    public int Id { get; private set; }
    
    public MySceneManager SceneManager { get; set; }
   
    public SceneViewWindow SceneViewWin { get; set; }

    //public ChartDataInfo DataOverview { get { return SeriesChartData.DataOverview(); } }

    public bool IsPrimaryScene { get; set; } = false;

    public bool DataIsReady { get; set; } = false;
    

    // 如果数据最小值为负是, 需要计算数据为 0 时的 Y axis
    // 该值在 CombinationType 为 Individual 时无效
    public float ZeroYAxis { get; protected set; }


    /// <summary>
    //
    // 该值由 CalcScale() 方法计算出来
    //
    // 如果 Combination type为 Combination则该值为 该值为全局值, 应该保存在 SceneManager中
    // 如果是 Individual 则每一条线都会有独立的 scale, 需要保存在其 Container中的同一scale的组件中 <see cref="PointSeries.YAxisScale"/>
    // 如果是 Comparision 该值变化快, 需要保存在 WorldWin 对你中
    /// </summary>    
    public float YAxisScale { get; set; }

    public CombinationType Combination { get { return SceneViewWin.Combination; } }

    public virtual void CalcControlCoordinate(CanvasControl c)
    {
            _mainContainerControl.CalcControlCoordinate(c);
    }

    public virtual void Draw(CanvasDrawingSession cds)
    {
        if (!DataIsReady)
            return;
        
            _mainContainerControl.Draw(cds);

        foreach (var item in GlobalControls)
        {
            item.Draw(cds);
        }                
    }

    public virtual void Update(GenericInput gi, TimeSpan ts)
    {

        if (!DataIsReady) 
            return;

        _mainContainerControl.Update(gi, ts);       

        UpdateGlobalControls(gi.Creator);        
    }

    /// <summary>
    /// Y axis will be processed in primary scen or primary control becuase it depends on the Combination type and accurate data
    /// </summary>
    /// <param name="creator"></param>
    public void UpdateGlobalControls(ICanvasResourceCreator creator)
    {
        if (!IsPrimaryScene)
            return;

        // X Axis Coordinate
        GlobalControls.Add(new StraightLine(new Vector2(SceneManager.GlobalViewWindow.Room_Left_X, SceneManager.GlobalViewWindow.Room_Buttom_Y), new Vector2(SceneManager.GlobalViewWindow.Room_Right_X, SceneManager.GlobalViewWindow.Room_Buttom_Y), Colors.Gray));

        if (!DataIsReady)
        {
            return;
        }

        if (SceneViewWin.HorizentalTickCoordinators != null)
        {
            // X axis scale & tick
            string[] ticks = SceneViewWin.HorizentalTicks;
            
            if (ticks != null)
            {
                float y_coordinate_down = SceneManager.GlobalViewWindow.BoardSolid_Buttom_Y + SceneViewWindow.TickScaleHeightOrLength;

                for (int i = 0; i < SceneViewWin.HorizentalTickCoordinators.Length; i++)
                {

                    //float x_coordinate = ViewWindow.HorizentalTickCoordinators[i];
                    var x_coordinate = SceneViewWin.HorizentalTickCoordinators[i];

                    if (!x_coordinate.IsValid)
                        continue;
                    
                    // Scale
                    GlobalControls.Add(new StraightLine(new Vector2(x_coordinate.Coor, SceneManager.GlobalViewWindow.BoardSolid_Buttom_Y), new Vector2(x_coordinate.Coor, y_coordinate_down), Colors.Gray));

                    // Tick
                    if (ticks[i] != null && ticks[i].Length > 0)
                    {

                        CanvasTextLayout textLayout = new CanvasTextLayout(creator, ticks[i], new CanvasTextFormat() { FontSize = 12 }, SceneManager.GlobalViewWindow.CanvasActualWidth - 20, 50f);
                        Rect LayoutBounds = textLayout.LayoutBounds;

                        float tick_X = x_coordinate.Coor - (float)LayoutBounds.Width / 2;
                        LabelElement ml = new LabelElement(ticks[i])
                        {
                            Text = ticks[i],
                            FontSize = 12,
                            ForgroundColor = Colors.Gray
                        };
                        ml.Location = new Vector2(tick_X, y_coordinate_down + 2);
                        ml.Width = (float)LayoutBounds.Width + 1;
                        ml.Height = (float)LayoutBounds.Height + 1;
                        GlobalControls.Add(ml);
                    }
                }
            }
        }
        if (SceneViewWin.Labels != null)
        {
            // Title
            if (SceneViewWin.Labels.Title != null && SceneViewWin.Labels.Title.ShowLabel)
            {
                ChartLabel title = SceneViewWin.Labels.Title;
                CanvasTextLayout textLayout = new CanvasTextLayout(creator, title.Label, new CanvasTextFormat() { FontSize = title.TitleFontSize }, SceneManager.GlobalViewWindow.CanvasActualWidth - 20, 100f);
                Rect LayoutBounds = textLayout.LayoutBounds;

                //cds.DrawTextLayout(textLayout, 0, 0, Colors.Transparent);
                float middleOfTopMargin_y = (SceneManager.GlobalViewWindow.BoardSolid_Top_Y - SceneManager.GlobalViewWindow.TopBoard_Y) / 2 - (float)LayoutBounds.Height;
                //float middleOfTopMargin_y = (ViewWindow.BoardSolid_Top_Y - ViewWindow.TopBoard_Y) / 2;
                float middleOfWorkArea_x = SceneManager.GlobalViewWindow.BoardSolid_Left_X + (SceneManager.GlobalViewWindow.BoardSolid_Width / 2);
                float beginningOfText_x = middleOfWorkArea_x - (float)LayoutBounds.Width / 2;
                LabelElement ml = new LabelElement(title.Label)
                {
                    Text = title.Label,
                    FontSize = title.TitleFontSize,
                    ForgroundColor = title.TitleColor,
                    BackgroundColor = title.BackgroundColor
                };
                ml.Location = new Vector2(beginningOfText_x, middleOfTopMargin_y);
                ml.Width = (float)LayoutBounds.Width + 1;
                ml.Height = (float)LayoutBounds.Height + 1;
                GlobalControls.Add(ml);
            }

            // X Label

            if (SceneViewWin.Labels.XLabel != null && SceneViewWin.Labels.XLabel.ShowLabel)
            {
                ChartLabel xlabel = SceneViewWin.Labels.XLabel;

                CanvasTextLayout textLayout = new CanvasTextLayout(creator, xlabel.Label, new CanvasTextFormat() { FontSize = xlabel.TitleFontSize }, 300f, 100f);
                Rect LayoutBounds = textLayout.LayoutBounds;

                //cds.DrawTextLayout(textLayout, 0, 0, Colors.Transparent);
                float middleOfButtomMargin_y = SceneManager.GlobalViewWindow.BoardSolid_Buttom_Y + 30;//(board.ButtonBoard_Y - board.SolidButtomBoard_Y) /2 ;
                float middleOfWorkArea_x = SceneManager.GlobalViewWindow.BoardSolid_Left_X + (SceneManager.GlobalViewWindow.BoardSolid_Width / 2);
                float beginningOfText_x = middleOfWorkArea_x - (float)LayoutBounds.Width / 2;

                LabelElement ml = new LabelElement(xlabel.Label)
                {
                    Text = xlabel.Label,
                    FontSize = xlabel.TitleFontSize,
                    ForgroundColor = xlabel.TitleColor,
                    BackgroundColor = xlabel.BackgroundColor
                };
                ml.Location = new Vector2(beginningOfText_x, middleOfButtomMargin_y);
                ml.Width = (float)LayoutBounds.Width + 1;
                ml.Height = (float)LayoutBounds.Height + 1;


                GlobalControls.Add(ml);

            }

            // Y Label
            if (SceneViewWin.Labels.YLable != null && SceneViewWin.Labels.YLable.ShowLabel)
            {
                ChartLabel ylabel = SceneViewWin.Labels.YLable;
                CanvasTextLayout textLayout = new CanvasTextLayout(creator, ylabel.Label, new CanvasTextFormat() { FontSize = ylabel.TitleFontSize }, 300f, 100f);
                //cds.DrawTextLayout(textLayout, 0, 0, Colors.Transparent);

                float middleLeftBoardMargin_x = (float)SceneManager.GlobalViewWindow.Margins.Left / 2 - 30;
                float labelOfCoordinate_y = SceneManager.GlobalViewWindow.BoardSolid_Height / 2 + SceneManager.GlobalViewWindow.BoardSolid_Top_Y + (float)textLayout.DrawBounds.Width / 2;

                LabelElement ml = new LabelElement(ylabel.Label)
                {
                    Text = ylabel.Label,
                    FontSize = ylabel.TitleFontSize,
                    ForgroundColor = ylabel.TitleColor,
                    BackgroundColor = ylabel.BackgroundColor
                };
                ml.Location = new Vector2(middleLeftBoardMargin_x, labelOfCoordinate_y);
                ml.Width = (float)textLayout.LayoutBounds.Width + 1;
                ml.Height = (float)textLayout.LayoutBounds.Height + 1;

                ml.Angle = -90f;
                ml.Transform = true;
                GlobalControls.Add(ml);
            }
        }

    }


    public void SetContainerControl(IControlContainer controlContainer)
    {
        controlContainer.Scene = this;
        _mainContainerControl = controlContainer;   
    }

    public virtual void CalcViewWorldScale() {

        _mainContainerControl.CalcViewWorldScale();
    }

    public static float CalcViewYAxisCoordinate(float worldYAxisValue, float worldMinYAxisValue, float viewMinYAxisValue, float wordViewYAxisScale, float yAxisMidValue)
    {     
        // calculating the point on viewport
        float viewYAxisValue = viewMinYAxisValue + (worldYAxisValue - worldMinYAxisValue) * wordViewYAxisScale;

        if (viewYAxisValue > yAxisMidValue)
            return yAxisMidValue - Math.Abs(viewYAxisValue - yAxisMidValue);
        else if ((viewYAxisValue < yAxisMidValue))
            return yAxisMidValue + Math.Abs(viewYAxisValue - yAxisMidValue);
        else return viewYAxisValue;
    }

    public List<IControl> YAxisScaleAndTicks(SceneViewWindow board, float minWorldValue, float maxWorldValue, float xCoordinate, float YScale, ICanvasResourceCreator cds, Color color, bool reverse=false)
    {
        ViewWindow globalViewWin = this.SceneManager.GlobalViewWindow;
        List<IControl> controls = new List<IControl>();

        float fixedStepScale = 50;
        var verticalScaleSteps = (int)globalViewWin.RoomSolid_Height / fixedStepScale;
        float stepScale = (maxWorldValue - minWorldValue) / verticalScaleSteps;

        float beginningValue = minWorldValue + stepScale / 2;
        if (minWorldValue < 0)
        {
            CalcYAxisDataPointCoordinate(controls, 0, maxWorldValue, stepScale, SceneViewWin, xCoordinate, cds, color, reverse,
                 (o) => { return CalcViewYAxisCoordinate(o, minWorldValue, globalViewWin.RoomSolid_Top_Y, YScale, globalViewWin.MiddleLine_Y); });

            CalcYAxisDataPointCoordinate(controls, 0, beginningValue, stepScale, SceneViewWin, xCoordinate, cds, color, reverse,
                  (o) => { return CalcViewYAxisCoordinate(o, minWorldValue, globalViewWin.RoomSolid_Top_Y, YScale, globalViewWin.MiddleLine_Y); });
        }
        else
        {
            CalcYAxisDataPointCoordinate(controls, beginningValue, maxWorldValue, stepScale, SceneViewWin, xCoordinate, cds, color, reverse,
                (o) => { return CalcViewYAxisCoordinate(o, minWorldValue, globalViewWin.RoomSolid_Top_Y, YScale, globalViewWin.MiddleLine_Y); });
        }
        return controls;
    }


    private static void CalcYAxisDataPointCoordinate(List<IControl> controls, float beginningValue, float endingValue, float fixedValueScale, 
        SceneViewWindow sceneViewWin, float xCoordinate, ICanvasResourceCreator cds, Color color, bool reverse,  Func<float, float> func)
    {
        float shortTick = SceneViewWindow.TickScaleHeightOrLength;
        if (reverse) shortTick = -shortTick;
        string printableValue;
        float forward = fixedValueScale;
        if (endingValue < 0)
            forward = -Math.Abs(fixedValueScale);

        for (float step = beginningValue; Math.Abs(step) < Math.Abs(endingValue); step += forward)
        {
            try
            {
                float scale = step;               

                if (sceneViewWin.CompressYLabel == UiBool.No && Math.Abs(scale) > 0.01)
                {
                    if (sceneViewWin.TrimDecimal == UiBool.No && Math.Abs(scale) > 1)
                    {
                        scale = (int)scale;
                        printableValue = $"{scale}";
                    }
                    else
                    {
                        int tempScale = (int)(scale * 100);
                        scale = (float)tempScale / 100;
                        printableValue = $"{scale}";
                    }
                }
                else
                {
                    var formatted = DataProcessUtil.FormatDecimal(scale);
                    printableValue = $"{formatted.formatedValue}{formatted.unit}";
                }

                //}
                //float y_coordinate = Calc_V_Y_Coordinate(scale, minWorldValue, board.RoomSolid_Top_Y, YScale, board.MiddleLine_Y);
                float y_coordinate = func(scale);
                controls.Add(new StraightLine(new Vector2(xCoordinate, y_coordinate), new Vector2(xCoordinate - shortTick, y_coordinate), color));

                string text = printableValue;
                CanvasTextLayout textLayout = new CanvasTextLayout(cds, text, new CanvasTextFormat() { FontSize = 12 }, 100f, 50f);
                //cds.DrawTextLayout(textLayout, 10, 50, Colors.Transparent);

                float tick_X = xCoordinate - (float)textLayout.LayoutBounds.Width - shortTick - 5;
                if (reverse)
                {
                    tick_X = xCoordinate + Math.Abs(shortTick) + 5;
                }
                LabelElement ml = new LabelElement(text)
                {
                    Text = text,
                    FontSize = 12,
                    ForgroundColor = color
                };
                ml.Location = new Vector2(tick_X, y_coordinate - (float)textLayout.LayoutBounds.Height / 2);
                ml.Width = (float)textLayout.LayoutBounds.Width + 1;
                ml.Height = (float)textLayout.LayoutBounds.Height + 1;
                controls.Add(ml);
            }catch (Exception ex)
            {
                Debug.WriteLine(ex.Message+"\r\n"+ex.StackTrace);
            }
        }
    }


    public void DataTransformation(ChartDataInput chartData)
    {
        BounchOfSeries<PointShape> data = ConvertPointSeriesToBounchOfSeries(chartData.InputDataSeries);
        data.ComparingPointIndex = chartData.ComparisingIndex;
        // data.ShowLegend = chartData.ShowLegend;

        // Get Combination type from XAML prioritily
        SceneViewWin.Combination = chartData.Combination;

        //// Following is only for Line chart
        //// 
        ///*
        // * TODO This logic will be removed after ComparisonLineScene being ready
        // * 
        // */
        ////========================================================================
        //if (SceneViewWin.Combination == CombinationType.Comparision)
        //{
        //    data.ClaculateComparisionVirtualValue();
        //    SceneViewWin.ComparingPointIndex = data.ComparingPointIndex;
        //}
        ////========================================================================

        HandleCombinationSpecialLogic(data);

        SceneViewWin.Labels = chartData.Labels;
        SceneViewWin.HorizentalTicks = chartData.HorizentalTicks;
        //if (SceneViewWin.HorizentalTicks == null || SceneViewWin.HorizentalTicks.Length == 0)
        //{
        //    string[] ticks = new string[chartData.GetDataPointSizeOfEachSeries()];
        //    for (int i = 0; i < ticks.Length; i++)
        //    {
        //        ticks[i] = $"{i}";
        //    }
        //    SceneViewWin.HorizentalTicks = ticks;
        //}

        SceneViewWin.CompressYLabel = chartData.CompressYLabel;
        SceneViewWin.TrimDecimal = chartData.TrimDecimal;            

        
        //ViewWindow.LegendLabels = data.Legends;
        SceneViewWin.MinXTickScale = chartData.MinXTickScale;
                        
        SeriesChartData = data;
        DataIsReady = true;
    }

    protected abstract void HandleCombinationSpecialLogic(BounchOfSeries<PointShape> data);

    public static BounchOfSeries<PointShape> ConvertPointSeriesToBounchOfSeries(List<PointArray<float?>> multiDataPointSerieses)
    {
        if (multiDataPointSerieses == null)
            return null;
        List<PointSeries<PointShape>> sd = new List<PointSeries<PointShape>>();
        foreach (PointArray<float?> point in multiDataPointSerieses)
        {
            if (point != null && point.Length > 0)
                sd.Add(new PointSeries<PointShape>().Transform(point, (o) => { return new PointShape(point.Id, o); }));
        }

        var re = new BounchOfSeries<PointShape>(sd.ToArray());

        return re;
    }

}

