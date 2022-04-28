using ChartBase.Chart.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Toolkit.Uwp.UI.Media.Geometry;

namespace ChartBase.Chart.Elements;

public class RectangleElement : SizableControl
{
    public RectangleElement() : base()
    {
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        if (Height == 0 || Width == 0)
            return;
        string geo = $"M {Location.X} {Location.Y} l {Width} {0}, {0} {Height}, {-Width} {0} Z";
        CanvasGeometry geometry = CanvasPathGeometry.CreateGeometry(cds, geo);
        cds.DrawGeometry(geometry, ForgroundColor, 0.5f);
        cds.FillGeometry(geometry, ForgroundColor);

        //if (Fill)
        //{
        //    string closedPathData = "M 800 200 800 300 900 300 Z";
        //    CanvasGeometry triangleGeometry1 = CanvasPathGeometry.CreateGeometry(cds, closedPathData);
        //    cds.DrawGeometry(triangleGeometry1, ForgroundColor, 1f);

        //    cds.FillRectangle(new Windows.Foundation.Rect(Location.X, Location.Y, Width-1, Height-1), ForgroundColor);
        //}
        //else
        //{
        //    cds.DrawRectangle(new Windows.Foundation.Rect(Location.X, Location.Y, Width - 1, Height - 1), ForgroundColor);
        //}
    }

    public override void Reset()
    {
        
    }

    public bool Fill { get; set; } = true;
}

