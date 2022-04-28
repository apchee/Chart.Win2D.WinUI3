using ChartBase.Chart.Controls;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChartBase.Chart.UserControls;

public class BarChartControl : ControlContainerBase
{
    private List<IControl> otherControls = new List<IControl>();
    public BarChartControl()
    {        
    }

    //// 该变量保存传入的数据序列(经转换后的数据)
    //// 该变量保存在 Scene 中
    //public BounchOfSeries<PointShape> Serieses { get; set; }
    ////public CombinationType Combination { get; internal set; }

    public override void CalcControlCoordinate(CanvasControl creator)
    {
        base.CalcControlCoordinate(creator);
    }

    public override void Clear()
    {
        base.Clear();
        otherControls.Clear();
    }

    public override void Update(GenericInput gi, TimeSpan ts)
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

        base.Update(gi, ts);
        otherControls.Clear();

        if (SceneViewWin == null)
            return;

        SceneViewWin.HorizentalTickCoordinators = new (bool, float)[Serieses.MaxPointSize()];

        ViewWindow globalVewWin = Scene.SceneManager.GlobalViewWindow;

        // how may groups of charts(即一个数据序列的点的个数)
        int groupSize = Serieses.MaxPointSize();

        SceneViewWin.X_Step = (globalVewWin.RoomSolid_Right_X - globalVewWin.RoomSolid_Left_X) / Serieses.MaxPointSize();

        // bar chart 两边留空及每组 bar charts之间的留空
        float emptySpace = SceneViewWin.X_Step / (Serieses.Length+1);
        emptySpace *= 1.5f;

        // 两边各有一个留空, 所以留空的个数要比数据点个数多一个
        float totalEmptySpaces = emptySpace * (groupSize + 1);

        // 每一组 bar charts 的实际宽度 = (Room 的有效宽度 - 全部留空宽度之和) / 数据点数
        float groupBarWidth = (globalVewWin.RoomSolid_Width - totalEmptySpaces) / groupSize;
        float halfGroupBarChartWidth = groupBarWidth / 2;

        // 每一个 bar chart 的宽度
        float barWidth = groupBarWidth / Serieses.Length;

        float beginningX = globalVewWin.RoomSolid_Left_X;
        for (int i = 0; i < Serieses.MaxPointSize(); i++)
        {
            beginningX += emptySpace;
            // X tick scale
            SceneViewWin.HorizentalTickCoordinators[i] = (true, beginningX + halfGroupBarChartWidth);

            // Coordinate of each group of bar charts
            for (int j = 0; j < Serieses.Length; j++)
            {
                PointSeries<PointShape> ps = Serieses[j];
                RectangleShape dataPoint = new RectangleShape(ViewWindow.Uid(), ps[i].RealValue, ps[i]);

                float value = 0;
                if (dataPoint.VirtualValue == null)
                {
                    dataPoint.IsValidPoint = false;
                    beginningX += barWidth;
                    continue;
                }

                value = dataPoint.VirtualValue.Value;

                float y_coordinate = SceneBase.CalcViewYAxisCoordinate(value,
                    Serieses.MinVirtualValue, globalVewWin.RoomSolid_Top_Y, Scene.YAxisScale, globalVewWin.MiddleLine_Y);

                RectangleElement rc = new RectangleElement()
                {
                    Location = new Vector2(beginningX + 0.5f, y_coordinate)
                };
                float middleXCoor = beginningX + barWidth / 2;
                beginningX += barWidth;
                Vector2 RightButton = new Vector2(beginningX - 0.5f, Scene.ZeroYAxis);
                rc.Width = RightButton.X - rc.Location.X;
                rc.Height = RightButton.Y - rc.Location.Y;
                rc.ForgroundColor = ps.LineColor;
                rc.Fill = true;
                GenericControls.Add(rc);

                // Markvalue
                if (ps.MarkValue)
                {
                    float valueMakerYcoor = y_coordinate;
                    if(valueMakerYcoor > Scene.ZeroYAxis)
                    {
                        valueMakerYcoor += 20;
                    } else if(valueMakerYcoor < Scene.ZeroYAxis)
                    {
                        valueMakerYcoor -= 20;
                    }
                    if (dataPoint.RealValue.Value < 10000) {
                        Console.WriteLine("");
                    }
                    var unit = DataProcessUtil.FormatDecimal(dataPoint.RealValue.Value);
                    TextElement le = new TextElement($"{unit.formatedValue}{unit.unit}")
                    {
                        Location = new Vector2(middleXCoor, valueMakerYcoor),
                        FontSize = 12,
                        ForgroundColor = ps.LineColor,
                        HAlign = HorizentalAlignment.Center
                    };

                    if (valueMakerYcoor > Scene.ZeroYAxis)
                    {
                        le.VAlign = VerticalAlignment.Top;
                    }
                    else if (valueMakerYcoor < Scene.ZeroYAxis)
                    {
                        le.VAlign = VerticalAlignment.Bottom;
                    }
                    le.Update(gi, ts);
                    GenericControls.Add(le);
                }
            }

        }

        // Draw Legends
        //if (Serieses.ShowLegend && Serieses.Legends.Length > 0)
        //{
        //    var col = new LegendContainer(Scene, Serieses.Legends);
        //    col.Update(gi, ts);
        //    GenericControls.Add(col);
        //}

    }

    public override void Draw(CanvasDrawingSession cds)
    {
        base.Draw(cds);
        foreach (var item in otherControls)
        {
            item.Draw(cds);
        }
    }
}

