using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;

namespace ChartBase.Models;


public class BounchOfSeries<T>  where T : PointShape
{
    public bool ShowLegend { get; set; } = true;

    public BounchOfSeries(PointSeries<T>[] serieses)
    {
        Serieses = serieses;
        CalculateMaxValue();
        //CalcLegends();
    }

    private void CalculateMaxValue()
    {
        foreach (var serie in Serieses)
        {
            serie.RecalculateMaxValue();
        }
        MaxVirtualValue = MaxVirtual();
        MinVirtualValue = MinVirtual();
        MaxWorldValue = MaxWorld();
        MinWorldValue = MinWorld();
    }

    public PointSeries<T> this[int index]{
        get { if (Serieses != null && index < Serieses.Length)
            {
                return Serieses[index];
            }
            else {
                return null;
            }
        }
        set { }
    }

    public PointSeries<T>[] Serieses { get; private set; }

    //public ChartDataInfo DataInfo { get; set; }

    public float MinWorldValue { get; private set; }

    public float MaxWorldValue { get; private set; }

    public float MinVirtualValue { get; private set; }

    public float MaxVirtualValue { get; private set; }
    public int ComparingPointIndex { get; set; } = 0;

    //public LegendItem[] Legends{ get; set; }

    private int maxLength = 0;
    public int MaxPointSize()
    {
        if (maxLength > 0)
            return maxLength;

        foreach (var item in Serieses)
        {
            if(item.Length > maxLength)
                maxLength = item.Length;    
        }
        if(maxLength != Serieses[0].Length)
        {
            throw new ArgumentException("Data point size are not aligned.");
        }
        return maxLength;
    }


    public int SeriesLength()
    {
        if (Serieses != null)
        {
            return Serieses.Length;
        }
        return 0;
    }

    public int Length
    {
        get
        {
            if (Serieses != null)
            {
                return Serieses.Length;
            }
            return 0;
        }
        set { }
    }

    //public ChartDataInfo DataOverview()
    //{
    //    if (DataInfo == null)
    //    {
    //        DataInfo = new ChartDataInfo();
    //        //DataInfo.Legends = Legends;
    //        DataInfo.LineColors = new Color[Serieses.Length];
    //    }
    //    for (int i = 0; i < Serieses.Length; i++)
    //    {
    //        DataInfo.LineColors[i] = Serieses[i].LineColor;
    //    }
    //    return DataInfo;
    //}



    private float MaxVirtual()
    {
        List<float> vs = new List<float>();
        foreach (PointSeries<T> p in Serieses)
        {
            p.Max();
            p.Min();
            vs.Add(p.MaxVirtualValue);
        }
        return vs.Count == 0?0:vs.Max();
    }
    private float MinVirtual()
    {
        List<float> vs = new List<float>();
        foreach (PointSeries<T> p in Serieses)
        {
            vs.Add(p.MinVirtualValue);
        }
        return vs.Count == 0 ? 0 : vs.Min();
    }

    private float MaxWorld()
    {
        List<float> vs = new List<float>();
        foreach (PointSeries<T> p in Serieses)
        {
            vs.Add(p.MaxWorldValue);
        }
        return vs.Count == 0 ? 0 : vs.Max();
    }
    private float MinWorld()
    {
        List<float> vs = new List<float>();
        foreach (PointSeries<T> p in Serieses)
        {
            vs.Add(p.MinWorldValue);
        }
        return vs.Count == 0 ? 0 : vs.Min();
    }


    //
    // Summary:
    //  Calculate comparision virtual value
    //
    // Parameters:
    //  baseIndex:      the index of data series which is the point for calculating scale values and virtual values
    internal void ClaculateComparisionVirtualValue()
    {
        // do nothing if it has only one series
        if (Serieses == null || Serieses.Length ==0)
            return;

        int maxPointSzie = MaxPointSize();
        // baseIndex must be between start index and last index
        if (ComparingPointIndex < 0 || ComparingPointIndex >= maxPointSzie)
            return;

        // calculate scale value
        //int midSeries = Length / 2; // determine the base series. the scale value should be 1 for this base series

        int baseIndex = ComparingPointIndex;
        // base value
        bool fulledValues = false;
        for (int i = baseIndex; i < maxPointSzie; i++)
        {
            fulledValues = Serieses.All(item=>item[i].RealValue!=null);
            if (fulledValues)
            {
                baseIndex = i;
                break;
            }
        }
        if (!fulledValues)
            return;

        ComparingPointIndex = baseIndex;

        // Calculate virtual value
        foreach(var ser in Serieses)
        {
            var baseValue = ser.DataPoints[ComparingPointIndex].RealValue.Value;
            ser.DataPoints[ComparingPointIndex].VirtualValue = 0;

            for (int i = ComparingPointIndex; i < ser.DataPoints.Length; i++)
            {

                var tmpItem = ser.DataPoints[i];
                var tv = tmpItem.RealValue;
                if (tv == null)
                    continue;
                var value = tv.Value;
                tmpItem.VirtualValue = (value - baseValue) / baseValue;                
            }

            for (int i = ComparingPointIndex-1; i >=0; i--)
            {

                var tmpItem = ser.DataPoints[i];
                var tv = tmpItem.RealValue;
                if (tv == null)
                    continue;
                var value = tv.Value;
                tmpItem.VirtualValue = (value - baseValue) / baseValue;
            }
        }

        // re-calculate Maxminal value
        CalculateMaxValue();
    }
}




//public enum CombinationType
//{
//    NonCombination,
//    Combination,
//    Individual,
//    Comparision 
//}

//public enum MarkerType
//{
//    UnMarkable,
//    Point,
//    Circle,
//    Pipeline
//}



