using ChartBase.Chart.Controls;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using ChartBase.utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;

namespace ChartBase.Chart.UserControls;


/// <summary>
/// LineChar 的主控件
/// </summary>
public class LineChartControl : ControlContainerBase
{
    private List<IControl> otherControls = new List<IControl>();

    public LineChartControl()
    {
    }

    //public CombinationType Combination { get; set; }
    //public BounchOfSeries<PointShape> Serieses { get; set; }


    public override void CalcControlCoordinate(CanvasControl creator)
    {
        BounchOfSeries<PointShape> Serieses;
        if (Scene.SeriesChartData is BounchOfSeries<PointShape>)
        {
            Serieses = Scene.SeriesChartData as BounchOfSeries<PointShape>;
        }
        else
        {
            return;
        }

        base.CalcControlCoordinate(creator);

        if (SceneViewWin == null || Serieses == null || Serieses.Length == 0)
            return;

        // tick scale on X axis
        float[] HorizentalTickCoordinators = new float[Serieses.MaxPointSize()];
        ViewWindow globalViewWin = Scene.SceneManager.GlobalViewWindow;
        SceneViewWin.X_Step = (globalViewWin.RoomSolid_Right_X - globalViewWin.RoomSolid_Left_X) / Serieses.MaxPointSize();
        float start = globalViewWin.RoomSolid_Left_X + SceneViewWin.X_Step / 2;
        for (int i = 0; i < HorizentalTickCoordinators.Length; i++)
        {
            HorizentalTickCoordinators[i] = start;
            start += SceneViewWin.X_Step;
        }
        
        // 计算每一个数据点的Y 坐标
        #region Calculate Line Coordinate
        for (int i = 0; i < Serieses.Serieses.Length; i++)
        {
            PointSeries<PointShape> series = Serieses.Serieses[i];
            Color color = series.LineColor;
            if (color == Colors.Transparent)
            {
                series.LineColor = ChartColors.GetColor(i);
            }

            for (int j = 0; j < series.DataPoints.Length; j++)
            {
                var dataPoint = series.DataPoints[j];
                if (dataPoint.VirtualValue == null)
                {
                    dataPoint.IsValidPoint= false;
                    continue;
                }

                float value = dataPoint.VirtualValue.Value;
                float y_coordinate = 0;
                if (Scene.Combination == CombinationType.Combination || Scene.Combination == CombinationType.Comparision)
                {
                    y_coordinate = SceneBase.CalcViewYAxisCoordinate(value, Serieses.MinVirtualValue, globalViewWin.RoomSolid_Top_Y, Scene.YAxisScale, globalViewWin.MiddleLine_Y);
                }
                else if (Scene.Combination == CombinationType.Individual || Scene.Combination == CombinationType.Comparision)
                {
                    y_coordinate = SceneBase.CalcViewYAxisCoordinate(value, series.MinVirtualValue, globalViewWin.RoomSolid_Top_Y, series.YAxisScale, globalViewWin.MiddleLine_Y);
                }
                dataPoint.Location = new Vector2(HorizentalTickCoordinators[j], y_coordinate);

            }
        }

        List<(bool, float)> xcoor = new List<(bool, float)>();
        float startXcorr = HorizentalTickCoordinators[0];
        if (SceneViewWin.MinXTickScale > 30)
        {
            Console.Error.WriteLine("Testing");
        }
        xcoor.Add((true,startXcorr));
        for (int i = 1; i < HorizentalTickCoordinators.Length; i++)
        {
            if (HorizentalTickCoordinators[i] > startXcorr + SceneViewWin.MinXTickScale)
            {
                startXcorr = HorizentalTickCoordinators[i];
                xcoor.Add((true, HorizentalTickCoordinators[i]));
            }
            else {
                xcoor.Add((false, HorizentalTickCoordinators[i]));
            }
        }
        SceneViewWin.HorizentalTickCoordinators = xcoor.ToArray();
        #endregion

    }


    public override void CalcViewWorldScale()
    {
        ViewWindow globalViewWin = Scene.SceneManager.GlobalViewWindow;
        BounchOfSeries<PointShape> Serieses;
        if (Scene.SeriesChartData is BounchOfSeries<PointShape>)
        {
            Serieses = Scene.SeriesChartData as BounchOfSeries<PointShape>;
        }
        else
        {
            return;
        }

        base.CalcViewWorldScale();
        if ((Scene.Combination == CombinationType.Individual || Scene.Combination == CombinationType.Comparision) 
            && Serieses != null && Serieses.Serieses != null)
        {
            for (uint i = 0; i < Serieses.Serieses.Length; i++)
            {
                PointSeries<PointShape> series = Serieses.Serieses[i];
                series.YAxisScale = (globalViewWin.RoomSolid_Buttom_Y - globalViewWin.RoomSolid_Top_Y) / (series.MaxVirtualValue - series.MinVirtualValue);
            }
        }
    }


    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        BounchOfSeries<PointShape> Serieses;
        if (Scene.SeriesChartData is BounchOfSeries<PointShape>)
        {
            Serieses = Scene.SeriesChartData as BounchOfSeries<PointShape>;
        }
        else
        {
            return;
        }

        //Reset();
        if (Serieses != null)
        {
            foreach (var dataPoints in Serieses.Serieses)
            {
                // Line
                var tmpLine = new LineControlContainer(dataPoints);
                tmpLine.Update(gi, ts);
                GenericControls.Add(tmpLine);
            }
        }

        foreach (var item in GenericControls)
        {
            item.Update(gi, ts);
        }

        // Y Axis tick for Individual CombinationType
        if (Scene.Combination == CombinationType.Individual)
        {
            UpdateIndividualYAxisTicks(gi.Creator);
        }

        // TODO: Y Axis tick for Comparision of CombinationType
        if (Scene.Combination == CombinationType.Comparision)
        {
            // TODO
        }
    }

    private void UpdateIndividualYAxisTicks(ICanvasResourceCreator creator)
    {
        ViewWindow globalViewWin = Scene.SceneManager.GlobalViewWindow;
        BounchOfSeries<PointShape> Serieses;
        if (Scene.SeriesChartData is BounchOfSeries<PointShape>)
        {
            Serieses = Scene.SeriesChartData as BounchOfSeries<PointShape>;
        }
        else
        {
            return;
        }

        // First Y axis scale and ticks
        if (Serieses.SeriesLength()>0)
        {            
            PointSeries<PointShape> series = Serieses[0];
            //float yAxisScale = (ViewWindow.RoomSolid_Buttom_Y - ViewWindow.RoomSolid_Top_Y) / (series.MaxVirtualValue - series.MinVirtualValue);
            var re = this.Scene.YAxisScaleAndTicks(SceneViewWin, series.MinVirtualValue, series.MaxVirtualValue, globalViewWin.BoardSolid_Left_X, series.YAxisScale, creator, series.LineColor);
            if (re.Count > 0)
            {
                otherControls.AddRange(re);
            }
            otherControls.Add(new StraightLine() { 
                Location = new Vector2(globalViewWin.BoardSolid_Left_X, globalViewWin.BoardSolid_Top_Y), 
                Target = new Vector2(globalViewWin.BoardSolid_Left_X, globalViewWin.BoardSolid_Buttom_Y),
                ForgroundColor = series.LineColor,
                LineWidth = 1f,
            });
        }
        if (Serieses.SeriesLength() > 1)
        {
            PointSeries<PointShape> series = Serieses[1];
            //float yAxisScale = (ViewWindow.RoomSolid_Buttom_Y - ViewWindow.RoomSolid_Top_Y) / (series.MaxVirtualValue - series.MinVirtualValue);
            var re = this.Scene.YAxisScaleAndTicks(SceneViewWin, series.MinVirtualValue, series.MaxVirtualValue, globalViewWin.BoardSolid_Right_X, series.YAxisScale, creator, series.LineColor, true);
            if (re.Count > 0)
            {
                otherControls.AddRange(re);
            }
            otherControls.Add(new StraightLine()
            {
                Location = new Vector2(globalViewWin.BoardSolid_Right_X, globalViewWin.BoardSolid_Top_Y),
                Target = new Vector2(globalViewWin.BoardSolid_Right_X, globalViewWin.BoardSolid_Buttom_Y),
                ForgroundColor = series.LineColor,
                LineWidth = 1f,
            });
        }
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        base.Draw(cds);
        foreach (var item in otherControls)
        {
            item.Draw(cds); 
        }
    }

    //public override void Reset()
    //{
    //    otherControls.Clear();
    //}

    public override void Clear()
    {
        base.Clear();
        otherControls.Clear();
    }

}
