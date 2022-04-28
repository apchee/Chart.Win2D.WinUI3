using Microsoft.UI;
using System;
using Windows.UI;

namespace ChartBase.Models;

public class ChartLabels
{
    public ChartLabel Title { get; set; } = new ChartLabel();
    public ChartLabel XLabel { get; set; } = new ChartLabel();
    public ChartLabel YLable { get; set; } = new ChartLabel();

}

public class ChartLabel
{
    public bool ShowLabel { get; set; } = false;
    public string Label { get; set; }
    public Color TitleColor { get; set; } = Colors.Yellow;
    public Color BackgroundColor { get; set; } = Colors.Transparent;
    public int TitleFontSize { get; set; } = 20;
    public string FontFamily { get; set; }

    public TextDirection Direction { get; set; } = TextDirection.LEFT_TO_RIGHT;
}
public enum TextDirection
{
    LEFT_TO_RIGHT = 0,
    RIGHT_TO_LEFT = 1,
    UP_TO_DOWN = 2,
    DOWN_TO_UP = 3
}

[Flags]
public enum ChartTypes
{
    LineChart = 0b_0000_0000_0001,
    BarChart = 0b_0000_0000_0010,
}
