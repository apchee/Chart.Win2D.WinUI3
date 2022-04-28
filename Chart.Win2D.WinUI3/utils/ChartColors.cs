using Microsoft.UI;
using Windows.UI;

namespace ChartBase.utils;

public static class ChartColors
{
    private static Color[] _allColors = new Color[] {
        Colors.Black,   Colors.Red,                 Colors.Yellow,      Colors.Orange,    Colors.Green,         Colors.Blue,
        Colors.Purple,  Colors.PaleVioletRed,       Colors.LightYellow, Colors.Pink,      Colors.LightGreen,    Colors.LightBlue,
        Colors.Cyan,    Colors.MediumVioletRed,     Colors.DarkMagenta, Colors.DeepPink,  Colors.ForestGreen,   Colors.DarkBlue,   
        Colors.Gray,     Colors.DarkGoldenrod,                                               Colors.SkyBlue,
        Colors.White };


    private static Color[] _allBackgroundColors = new Color[] {
        Colors.Black,   
        Colors.Red,   Colors.DarkRed, Colors.IndianRed, Colors.MediumVioletRed, Colors.OrangeRed, Colors.PaleVioletRed,
        Colors.Orange,    Colors.Green,         Colors.Blue,
        Colors.Purple,  Colors.PaleVioletRed,       Colors.Pink,      Colors.LightGreen,    Colors.LightBlue,
        Colors.Cyan,    Colors.MediumVioletRed,     Colors.DeepPink,  Colors.ForestGreen,   Colors.DarkBlue,
        Colors.Gray,     Colors.DarkGoldenrod,                                               Colors.SkyBlue,
        Colors.White };

    public static Color GetBackgroundColor(int index)
    {
        return _allBackgroundColors[index % _allBackgroundColors.Length];
    }

    public static Color GetColor(int index)
    {
        return _allColors[ index % _allColors.Length ];
    }
}

