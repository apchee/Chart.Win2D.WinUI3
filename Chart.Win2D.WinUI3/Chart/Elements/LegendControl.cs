using ChartBase.Chart;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace Chart.Framework.Elements;

public class LegendControl : ButtonElement, IControlContainer
{
    private LegendItem legendInfo;
    private ViewWindow viewWindow;

    private Point LegendRightTop;
    private Point LegendRightButton;


    private List<IControl> controls = new List<IControl>();

    public LegendControl(LegendItem legenInfo)
    {
        this.legendInfo = legenInfo;
        //this.viewWindow = viewWindow;
        Text = legenInfo.Text;
        FontSize = legenInfo.FontSize;
        Height = Legend.LegendRowHeight;
        ForgroundColor = legenInfo.LineColor;
    }

    public LegendControl(SceneBase scene, string text = "", int fontSize = 12, uint width = 100, uint height = 50, Color foreground_color = default(Color))
    : base(text, fontSize, width, height, foreground_color)
    {
        this.Scene = scene;
        IsHoverable = true;
    }

    public SceneBase Scene { get; set; }

    public void Clear()
    {
        legendInfo = null;
        viewWindow = null;
        controls.Clear();
    }


    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        controls.Clear();
        StraightLine line = new StraightLine();
        float lineYaxis = Location.Y + Legend.LegendRowHeight / 2;
        line.Location = new Vector2(Location.X, lineYaxis);
        line.Target = new Vector2(Location.X+20, lineYaxis);
        line.ForgroundColor = ForgroundColor;
        line.LineWidth = StrokeWidth;
        line.Update(gi, ts);
        controls.Add(line);

        if(legendInfo.Marker == MarkerShape.Point)
        {
            float cycleSize = StrokeWidth + 2;
            if (legendInfo.IsClicked)
                cycleSize += 2;

            var marker = new CycleElement(new Vector2(Location.X + 10, lineYaxis), cycleSize, ForgroundColor)
            {
                ConnectTo = legendInfo.ConnectTo,
                Fill = true
            };
            marker.Update(gi, ts);
            controls.Add(marker);
        }

        if(Text!=null && Text.Length > 0 )
        {
            try
            {
                CanvasTextLayout textLayout = new CanvasTextLayout(gi.Creator, Text, new CanvasTextFormat() { FontSize = 12 }, 1000f, 50f);
                LabelElement ml = new LabelElement(Text)
                {
                    Text = Text,
                    FontSize = 12,
                    ForgroundColor = ForgroundColor,
                    BackgroundColor = Colors.Transparent
                };
                ml.Location = new Vector2(Location.X + 30, Location.Y);
                ml.Width = (float)textLayout.LayoutBounds.Width + 1;
                ml.Height = Height;
                controls.Add(ml);

                this.Width = ml.Width + 30;
            }
            catch (Exception ex) { }
        }

        LegendRightTop = new Point(Location.X-5, Location.Y+2);
        LegendRightButton = new Point(Location.X + Width+5, Location.Y + Height - 2);
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        foreach (var item in controls)
        {
            item.Draw(cds);
        }
        if (legendInfo.IsClicked)
        {
            cds.DrawRectangle(new Rect(LegendRightTop, LegendRightButton), ForgroundColor);
            if (legendInfo.IsOnHovoring && legendInfo.IsClicked)
            {
                cds.FillRectangle(new Rect(LegendRightTop, LegendRightButton), Colors.Gray);
            }
        }
    }


    public override bool OnHover(GenericInput gi)
    {
        MouseGenericInput mgi = gi as MouseGenericInput;
        legendInfo.IsOnHovoring = mgi.X > LegendRightTop.X && mgi.X < LegendRightButton.X && mgi.Y > LegendRightTop.Y && mgi.Y < LegendRightButton.Y;
        if (legendInfo.IsOnHovoring)
        {
            legendInfo.HoveringForgroundColor = Colors.Green;
            legendInfo.StrokeWidth = 2f;
        }
        return legendInfo.IsOnHovoring;
    }


}

