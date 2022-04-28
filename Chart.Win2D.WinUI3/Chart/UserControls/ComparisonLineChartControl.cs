using ChartBase.Chart.Elements;
using ChartBase.Models;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ChartBase.Chart.UserControls;

public class ComparisonLineChartControl : LineChartControl
{
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
        if (Serieses.Serieses == null || Serieses.Serieses.Length == 0)
            return;
        foreach(var serie in Serieses.Serieses)
        {
            var linePoints = serie.DataPoints;
            if (linePoints == null || linePoints.Length ==0)
                continue;
            var last = linePoints.Last();
            var location = last.Location;
            float? virtualValue = last.VirtualValue;
            if(virtualValue != null)
            {
                double double_val = (double)virtualValue.Value * 100;
                decimal good_decimal_val = (decimal)double_val;

                decimal d = decimal.Round(good_decimal_val, 2);
                //var chgPctInt = (int)((currentValue - preValue) / preValue * 10000);
                float actualValue = Convert.ToSingle(d);

                string test = $"{actualValue}";
                CanvasTextLayout textLayout = new CanvasTextLayout(gi.Creator, test, new CanvasTextFormat() { FontSize = 12 }, 1000f, 50f);
                LabelElement ml = new LabelElement(test)
                {
                    Text = test,
                    FontSize = 12,
                    ForgroundColor = serie.LineColor,
                };
                ml.Location = new Vector2(location.X + 5, location.Y - 10);
                ml.Width = (float)textLayout.LayoutBounds.Width + 1;
                ml.Height = (float)textLayout.LayoutBounds.Height;
                GenericControls.Add(ml);
            }
        }
    }
}
