using ChartBase.Chart.Elements;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChartBase.Chart.Controls;

public class LineControlContainer : ControlBase
{

    public PointSeries<PointShape> dataPoints { get; set; }
    private List<IControl> lists = new List<IControl>();
    //public float LineWidth { get; set; } = 1f;

    public LineControlContainer(PointSeries<PointShape> data) : base()
    {
        dataPoints = data;
    }

    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);

        PointShape start = null;
        for (int i = 0; i < dataPoints.DataPoints.Length; i++)
        {
            PointShape point = dataPoints.DataPoints[i];
            if (!point.IsValidPoint)
            {
                start = null;
                continue;
            }
            else if(start == null) {
                start = point;
                continue;
            }
            StraightLine al = new StraightLine();
            al.LineWidth = dataPoints.LineWidth;
            al.Location = start.Location;
            al.Target = point.Location;
            al.ForgroundColor = dataPoints.LineColor;
            lists.Add(al);
            start = point;

        }

        // Marker
        if (dataPoints.Marker == MarkerShape.Circle || dataPoints.Marker == MarkerShape.Point)
        {
            float cycleSize = dataPoints.LineWidth + 2;
            if (dataPoints.IsSelected)
                cycleSize += 2;
            foreach (var dataPoint in dataPoints.DataPoints)
            {
                if (dataPoint.IsValidPoint)
                {
                    bool fill = dataPoints.Marker == MarkerShape.Point ? true : false;

                    var marker = new CycleElement(dataPoint.Location, cycleSize, dataPoints.LineColor)
                    {
                        ConnectTo = dataPoints.Id,
                        Fill = fill
                    };
                    lists.Add(marker);
                }
            }
        }

        if (dataPoints.MarkValue)
        {
            // Markvalue
            foreach (var dataPoint in dataPoints.DataPoints)
            {
                try
                {
                    if (dataPoint.RealValue == null)
                        continue;
                    var unit = DataProcessUtil.FormatDecimal(dataPoint.RealValue.Value);
                    float fontSize = 12;
                    //var v = Convert.ToDecimal(dataPoint.OriginalValue / unit.Div);
                    string markableValue = $"{unit.formatedValue} {unit.unit}";
                    CanvasTextLayout textLayout = new CanvasTextLayout(gi.Creator, markableValue, new CanvasTextFormat() { FontSize = fontSize }, 200f, 50f);
                    LabelElement label = new LabelElement(markableValue, fontSize);
                    label.Width = (float)textLayout.LayoutBounds.Width + 1;
                    label.Height = (float)textLayout.LayoutBounds.Height + 1;
                    label.ForgroundColor = dataPoints.LineColor;
                    label.Location = new Vector2(dataPoint.Location.X - (float)textLayout.DrawBounds.Width / 2, dataPoint.Location.Y - (float)textLayout.DrawBounds.Height - 20);
                    lists.Add(label);
                }
                catch (Exception ex) { }
            }
        }

    }

    public override void Draw(CanvasDrawingSession cds)
    {
        foreach (var l in lists)
        {
            l.Draw(cds);
        }
    }

    public override bool OnHover(GenericInput gi)
    {
        return false;
    }

    public override void CalcViewWorldScale()
    {
        throw new NotImplementedException();
    }

    public override void CalcControlCoordinate(CanvasControl c)
    {
        throw new NotImplementedException();
    }

}
