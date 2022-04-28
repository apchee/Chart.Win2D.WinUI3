using ChartBase.Chart.Elements;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChartBase.Chart.Scenes;

public class ComparisonLineScene : SceneBase
{


    //private List<IControl> _sceneLevelControls = new List<IControl>();

    public ComparisonLineScene(MySceneManager mg) : base(mg)
    {
        SceneViewWin = new SceneViewWindow();
    }


    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        if (DataIsReady)
        {
            UpdateSceneLevelControls(gi.Creator);
        }

        UpdateComparisonDataPoint(gi.Creator);
    }

    private void UpdateComparisonDataPoint(CanvasControl cc)
    {
        if (SceneViewWin.HorizentalTickCoordinators.Length <= SceneViewWin.ComparingPointIndex)
            return;
        var ht = SceneViewWin.HorizentalTicks[SceneViewWin.ComparingPointIndex];
        var x_coordinate = SceneViewWin.HorizentalTickCoordinators[SceneViewWin.ComparingPointIndex];
        CanvasStrokeStyle strockeStyle = new CanvasStrokeStyle() { DashStyle = CanvasDashStyle.Dot };
        var l = new StraightLine(new Vector2(x_coordinate.Coor, SceneManager.GlobalViewWindow.BoardSolid_Buttom_Y), new Vector2(x_coordinate.Coor, SceneManager.GlobalViewWindow.BoardSolid_Top_Y), Colors.Gray) { LineWidth = 0.5f };
        l.LineWidth = 0.5f;
        l.StrokeStyle = strockeStyle;
        GlobalControls.Add(l);

        CanvasTextLayout textLayout = new CanvasTextLayout(cc, ht, new CanvasTextFormat() { FontSize = 14 }, 200f, 50f);
        LabelElement label = new LabelElement(ht, 14);
        label.Width = (float)textLayout.LayoutBounds.Width + 1;
        label.Height = (float)textLayout.LayoutBounds.Height + 1;
        label.ForgroundColor = Colors.Gray;
        label.Location = new Vector2(x_coordinate.Coor, SceneManager.GlobalViewWindow.BoardSolid_Top_Y);
        GlobalControls.Add(label);

        string indexOfLast = $"Last: {SceneViewWin.HorizentalTickCoordinators.Length - (SceneViewWin.ComparingPointIndex) -1 }";
        textLayout = new CanvasTextLayout(cc, indexOfLast, new CanvasTextFormat() { FontSize = 14 }, 200f, 50f);
        label = new LabelElement(indexOfLast, 14);
        label.Width = (float)textLayout.LayoutBounds.Width + 1;
        label.Height = (float)textLayout.LayoutBounds.Height + 1;
        label.ForgroundColor = Colors.Gray;
        label.Location = new Vector2(x_coordinate.Coor, SceneManager.GlobalViewWindow.BoardSolid_Buttom_Y - (float)textLayout.LayoutBounds.Height);
        GlobalControls.Add(label);
    }

        /// <summary>
        /// Process Y axis scale and ticks if CombinationType is combination
        /// For indicidual CombinationType, Y axis scale and ticks will be process in primary control
        /// </summary>
        /// <param name="creator"></param>
    private void UpdateSceneLevelControls(ICanvasResourceCreator creator)
    {
        ViewWindow globalViewWin = this.SceneManager.GlobalViewWindow;
        if (IsPrimaryScene && Combination == CombinationType.Combination)
        {
            // Y axis coordinate
            GlobalControls.Add(new StraightLine(new Vector2(globalViewWin.Room_Left_X, globalViewWin.Room_Top_Y), new Vector2(globalViewWin.Room_Left_X, globalViewWin.Room_Buttom_Y), Colors.Gray));

            // Y axis scale & ticks
            var re = YAxisScaleAndTicks(SceneViewWin, SeriesChartData.MinVirtualValue, SeriesChartData.MaxVirtualValue, globalViewWin.BoardSolid_Left_X, YAxisScale, creator, Colors.Gray);
            if (re.Count > 0)
            {
                GlobalControls.AddRange(re);
            }
            // Zero axis coordinate
            if (ZeroYAxis != 0)
            {
                GlobalControls.Add(new StraightLine(new Vector2(globalViewWin.RoomSolid_Left_X, ZeroYAxis), new Vector2(globalViewWin.Room_Right_X, ZeroYAxis), Colors.Gray));
            }
        }
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        base.Draw(cds);
    }

    public override void CalcViewWorldScale()
    {
        ViewWindow globalViewWin = this.SceneManager.GlobalViewWindow;

        base.CalcViewWorldScale();
        if (SeriesChartData == null)
            return;
        if (Combination == CombinationType.Combination || Combination == CombinationType.Comparision)
        {
            YAxisScale = (globalViewWin.RoomSolid_Buttom_Y - globalViewWin.RoomSolid_Top_Y) / (SeriesChartData.MaxVirtualValue - SeriesChartData.MinVirtualValue);

            if (SeriesChartData.MinWorldValue >= 0)
            {
                ZeroYAxis = globalViewWin.BoardSolid_Buttom_Y;
            }
            else
            {

                ZeroYAxis = CalcViewYAxisCoordinate(0, SeriesChartData.MinVirtualValue, globalViewWin.RoomSolid_Top_Y, YAxisScale, globalViewWin.MiddleLine_Y);
            }
        }
    }

    public override void CalcControlCoordinate(CanvasControl c)
    {
        base.CalcControlCoordinate(c);
    }

    protected override void HandleCombinationSpecialLogic(BounchOfSeries<PointShape> data)
    {
        /*
        * TODO This logic will be removed after ComparisonLineScene being ready
        * 
        */
        //========================================================================
        if (SceneViewWin.Combination == CombinationType.Comparision)
        {
            data.ClaculateComparisionVirtualValue();
            SceneViewWin.ComparingPointIndex = data.ComparingPointIndex;
        }
        //========================================================================
    }

}
