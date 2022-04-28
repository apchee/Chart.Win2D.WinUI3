using ChartBase.Chart;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChartBase.Chart.Scenes;

public class CombinationScene : SceneBase
{

    //private List<IControl> _sceneLevelControls = new List<IControl>();

    public CombinationScene(MySceneManager mg) : base(mg)
    {
        SceneViewWin = new SceneViewWindow();
    }

    //public override void CleanElements()
    //{
    //    base.CleanElements();
    //    if(_sceneLevelControls.Count > 0)
    //    {
    //        _sceneLevelControls.Clear();
    //    }
    //}


    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        if (DataIsReady)
        {
            UpdateSceneLevelControls(gi.Creator);
        }
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
            //else if (WorldWindow.Combination == CombinationType.Individual)
            //{
            //    if (SelectedSeries != null)
            //    {
            //        YxtickLabels(ref tickLabels, ref coorLines, ViewWindow, SelectedSeries.MinVirtualValue, SelectedSeries.MaxVirtualValue, leftX, WorldWindow.YScale, gi.Creator);
            //    }
            //}

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
        
    }
}

