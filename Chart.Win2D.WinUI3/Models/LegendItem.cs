
using Windows.Foundation;
using Windows.UI;

namespace ChartBase.Models;

public class LegendItem
{

    public Color LineColor { get; set; }
    public string Text { get; set; }
    public float FontSize { get; set; } = 12;
    public int ConnectTo { get; set; }
    public bool ShowLegend { get; set; }
    public int Length { get { return Text == null ? 0 : Text.Length; } }
    public bool IsClicked { get; set; } = false;
    public bool IsOnHovoring { get; set; } = false;
    public MarkerShape Marker { get; set; }
    public float ActualWidth { get;  set; }
    public Color HoveringForgroundColor { get; set; }
    public float StrokeWidth { get; set; }
    public Rect LayoutBounds { get; set; }
}

