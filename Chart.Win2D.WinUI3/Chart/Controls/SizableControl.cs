using Microsoft.UI;
using Windows.UI;

namespace ChartBase.Chart.Controls;

public abstract class SizableControl : ControlBase
{
    public SizableControl() : base()
    {
    }

    public override bool OnHover(GenericInput gi)
    {
        return false;
    }


    public float ViewWindowWidth { get; set; }
    public float ViewWindowHeight { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public float StrokeWidth { get; set; } = 1f;
    // Outbounding of this scene story
    //public Rect BoundingRectangle { get { return new Rect(Location.X, Location.Y, Width - 1, Height - 1); } }
    public bool IsDrawingBoundingRectangle { get; set; }

    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public Color BorderColor { get; set; }
}

