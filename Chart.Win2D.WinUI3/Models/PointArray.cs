using Microsoft.UI;
using System;
using System.Collections.Generic;
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

    public MarkerShape Marker { get; set; } = MarkerShape.Point;

    public bool MarkValue { get; set; } = true;
    public int ValueUnit { get; set; } = 1;

    public int Length { get { return DataPoints==null?0:DataPoints.Length; } }

    public int MinXScale { get; set; } = 30;

    public string Legend { get; set; }
    //public bool ShowLabel { get; set; } = false;
    //public int LabelFontSize { get; set; } = 18;

    public object Tag { get; set; }
    public string Identifier { get; set; }

    public bool SupportAltLineColor { get; set; } = false;
    public Color AltLineColor { get; set; } = Colors.Green;

    public object Clone()
    {
        var clone = (PointArray<T>)this.MemberwiseClone();
        HandleCloned(clone);
        return clone;
    }

    private void HandleCloned(PointArray<T> clone)
    {
        var series = new List<T>(this.DataPoints);
        clone.DataPoints = series.ToArray();
        clone.Id = ViewWindow.Uid();
    }
}


