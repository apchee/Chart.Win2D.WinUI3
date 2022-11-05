using ChartBase.Chart.Controls;
using ChartBase.utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System.Numerics;
using Windows.UI;

namespace ChartBase.Chart.Elements;

public class StraightLine : ControlBase
{
    public float LineWidth { get; set; } = 1f;
    public CanvasStrokeStyle StrokeStyle { get; set; }


    public StraightLine() : base()
    {
        ForgroundColor = ChartColors.GetColor(Id);
    }

    public StraightLine(Vector2 loc, Vector2 tar, Color cor) : base()
    {
        ForgroundColor = cor;
        Location = loc;
        Target = tar;
    }


    public override void Draw(CanvasDrawingSession cds)
    {
        Color forgColor = ForgroundColor;
        if (SupportAltLineColor && Target.Y > Location.Y)
            forgColor = AltLineColor;
        if (StrokeStyle==null)
            cds.DrawLine(Location, Target, forgColor, LineWidth);
        else
            cds.DrawLine(Location, Target, forgColor, LineWidth, StrokeStyle);
    }

    public override bool OnHover(GenericInput gi)
    {
        return true;
    }

    public override void CalcViewWorldScale()
    {

    }

    public override void CalcControlCoordinate(CanvasControl c)
    {

    }

    public Vector2 Target { get; set; }
    public bool SupportAltLineColor { get; set; } = false;
    public Color AltLineColor { get; set; } = Colors.Green;
}

