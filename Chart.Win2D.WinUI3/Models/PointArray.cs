using Microsoft.UI;
using System;
using Windows.UI;

namespace ChartBase.Models;

public class PointArray<T>:ICloneable
{

    public PointArray() {
        Id = ViewWindow.Uid();
    }
    public PointArray(T[] points):this() 
    {
        DataPoints = points;
    }

    public int Id { get; private set; }

    public T[] DataPoints { get; set; } = new T[0];
    public Color LineColor { get; set; } = Colors.Transparent;
    public float LineWidth { get; set; } = 1f;

    public MarkerType Marker { get; set; } = MarkerType.Point;

    public bool MarkValue { get; set; } = true;
    public int ValueUnit { get; set; } = 1;

    public int Length { get { return DataPoints==null?0:DataPoints.Length; } }

    public int MinXScale { get; set; } = 30;

    public string Legend { get; set; }
    //public bool ShowLabel { get; set; } = false;
    //public int LabelFontSize { get; set; } = 18;

    public object Tag { get; set; }
    public string Identifier { get; set; }

    public object Clone()
    {
        return this.MemberwiseClone();
    }
}


