using System;
using System.Collections.Generic;
using System.Linq;

namespace ChartBase.Models;

public class PointSeries<T> : PointArray<T> where T : PointShape
{
    public T this[int index]
    {
        get
        {
            if (DataPoints != null && index < DataPoints.Length)
            {
                return DataPoints[index];
            }
            else
            {
                return null;
            }
        }
        set { }
    }

    internal void RecalculateMaxValue()
    {
        Max();
        Min();
    }

    public PointSeries<T> Transform<O>(PointArray<O> points, Func<O, T> func)
    {
        Id = points.Id;
        DataPoints = Transform(points.DataPoints, func);
        LineColor = points.LineColor;
        SupportAltLineColor = points.SupportAltLineColor;
        AltLineColor = points.AltLineColor;
        LineWidth = points.LineWidth;
        Legend = points.Legend;
        Marker = points.Marker;
        MarkValue = points.MarkValue;
        InitMaxMin();
        return this;
    }

    public T[] Transform<O>(O[] origin, Func<O, T> func)
    {
        T[] result = new T[origin.Length];
        for (int i = 0; i < origin.Length; i++)
        {
            result[i] = func(origin[i]);
        }
        return result;
    }


    // The number of SinglePoints
    public int NumberOfPoints { get { return DataPoints.Length; } }

    // The Maximun of the SinglePoint value
    public float MinWorldValue { get; set; } = float.MaxValue;
    public float MaxWorldValue { get; set; } = float.MinValue;

    public float MinVirtualValue { get; set; } = float.MaxValue;
    public float MaxVirtualValue { get; set; } = float.MinValue;

    /// <summary>
    // Can only be used with combination of CombinationType
    // 如果 Combination type为 Combination则该值为 该值为全局值, 应该保存在 Scene 中
    // <see cref="CombinationScene.YAxisScale"/>
    // 如果是 Individual 则每一条线都会有独立的 scale, 需要保存在其 Container中的同一scale的组件中
    // 如果是 Comparision 该值变化快, 需要保存在 WorldWin 对你中
    /// </summary>
    public float YAxisScale { get; set; }

    public int Id { get; internal set; }

    public bool IsSelected { get; set; } = false;

    // Comparision scale factor for Comparision model
    //public float ComparisionScaleFactor { get; internal set; } = 0;

    protected void InitMaxMin()
    {
        if (DataPoints.Length > 0)
        {
            MinWorldValue = Min();
            MaxWorldValue = Max();
            //float diff = MaxWorldValue - MinWorldValue;
            MinVirtualValue = MinWorldValue;
            MaxVirtualValue = MaxWorldValue;
        }
    }

    public float Max()
    {
        List<float> tmp = new List<float>();
        foreach(var v in DataPoints)
        {
            if (v.VirtualValue!=null)
                tmp.Add(v.VirtualValue.Value);
        }
        MaxVirtualValue = tmp.Count == 0 ? 0 : tmp.Max();
        return MaxVirtualValue;
    }
    public float Min()
    {
        List<float> tmp = new List<float>();
        foreach (var v in DataPoints)
        {
            if (v.VirtualValue != null)
                tmp.Add(v.VirtualValue.Value);
        }
        MinVirtualValue = tmp.Count==0?0:tmp.Min();
        return MinVirtualValue;
    }
}