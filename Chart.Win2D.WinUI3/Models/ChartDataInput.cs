﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartBase.Models;

public class ChartDataInput:ICloneable
{
    public List<PointArray<float?>> InputDataSeries { get; set; }=new List<PointArray<float?>>();
    public ChartLabels Labels { get; set; } = new ChartLabels();

    public string[] HorizentalTicks { get; set; } = new string[0];

    public bool ShowLegend { get; set; } = false;

    public CombinationType Combination { get; set; } = CombinationType.Combination;

    public CHART_TYPE ChartType { get; set; } = CHART_TYPE.LINE_CHART;

    // 是否用 K, M, B压缩数字
    public UiBool CompressYLabel { get; set; } = UiBool.Unknow;

    // 去小数
    public UiBool TrimDecimal { get; set; } = UiBool.Unknow;

    public int MinXTickScale { get; set; } = 30;

    public float? ReferenceValue { get; set; } = null;
    public int ComparisingIndex { get; set; } = 0;
    public int RangeOfBeginningIndexFromLast { get; set; }


    // Please note this index is based on last data point index
    // This means the last one of index is 0, the last second one of index is 1
    public int PointSizeFromLast { get; set; } = 0;

    public int DataPointSizeOfOneSeries()
    {
        if (InputDataSeries == null || InputDataSeries.Count == 0)
            return 0;
        return InputDataSeries[0].Length;
    }

    public string DateOfLastIndexOf(int lastIndex)
    {
        if (HorizentalTicks.Length == 0)
            return null;

        int len = HorizentalTicks.Length;
        if (lastIndex >= HorizentalTicks.Length)
            return HorizentalTicks[0];
        else
        {
            return HorizentalTicks[len-lastIndex-1];
        }
    }


    public string GetLastDate()
    {
        if (HorizentalTicks.Length == 0)
            return null;
        return HorizentalTicks[HorizentalTicks.Length - 1];
    }

    public List<LegendItem> GetLegendInfo()
    {
        List<LegendItem> lg = new List<LegendItem>();
        if (InputDataSeries == null || InputDataSeries.Count == 0)
            return lg;

        foreach(var tmp in InputDataSeries) { 
            lg.Add( new LegendItem() {
                LineColor = tmp.LineColor,
                Text = tmp.Legend,
                ConnectTo = tmp.Id,
                Marker = tmp.Marker,
            });
        };
        return lg;
    }

    public ChartDataInput TrimEmptyLastPoint()
    {
        if (InputDataSeries == null || InputDataSeries.Count == 0)
            return this;

        var result = InputDataSeries.All(e=>e.DataPoints.Length>0 && e.DataPoints[e.DataPoints.Length-1] == null);
        if (result)
        {
            foreach(var point in InputDataSeries)
            {
                float?[] dps = new float?[point.DataPoints.Length - 1];
                Array.Copy(point.DataPoints, 0, dps, 0, point.DataPoints.Length - 1);
                point.DataPoints = dps;
            }
            string[] hs = new string[HorizentalTicks.Length - 1];
            Array.Copy(HorizentalTicks, 0, hs, 0, HorizentalTicks.Length - 1);
            HorizentalTicks = hs;
        }
        return this;
    }

    public void CutOffByIndex(int index)
    {
        int length = HorizentalTicks.Length - index;
        if (index > 0)
        {
            foreach (var point in InputDataSeries)
            {
                float?[] dps = new float?[length];
                Array.Copy(point.DataPoints, index, dps, 0, length);
                point.DataPoints = dps;
            }
            string[] hs = new string[length];
            Array.Copy(HorizentalTicks, index, hs, 0, length);
            HorizentalTicks = hs;
        }
    }

    public ChartDataInput CutOffByIndexValue(int beginningYear)
    {
        int index = GetSeriesIndexByIndexValue(beginningYear);
        index--;
        if (index > 0)
        {
            CutOffByIndex(index);
        }
        return this;
    }

    public int ReverseBaseComparisonIndex(int index)
    {
        if (HorizentalTicks.Length == 0)
            return 0;
        if (HorizentalTicks.Length <= index)
            return HorizentalTicks.Length;
        else
            return HorizentalTicks.Length - index;
    }
    public int GetSeriesIndexBySeriesValueFromLast(int indexedValue)
    {
        return ReverseBaseComparisonIndex(GetSeriesIndexByIndexValue(indexedValue));
    }

    public int GetSeriesIndexByIndexValue(int seriesValue)
    {
        if (HorizentalTicks.Length == 0)
            return 0;
        if (Convert.ToInt32(HorizentalTicks[0]) >= seriesValue)
            return 0;

        int index = 0;
        for (; index < HorizentalTicks.Length; index++)
        {
            int cy = Convert.ToInt32(HorizentalTicks[index]);
            if (cy >= seriesValue)
            {
                break;
            }
        }
        return index;
    }

    public ChartDataInput UpdateTitle(string title)
    {
        Labels.Title.Label = title;
        Labels.Title.ShowLabel = true;
        return this;
    }
    public ChartDataInput ShowtTitleLabel(bool show)
    {
        Labels.Title.ShowLabel = show;
        return this;
    }

    internal int GetDataPointSizeOfSeries()
    {
        if (InputDataSeries == null || InputDataSeries.Count == 0)
            return 0;
        return InputDataSeries[0].Length;
    }

    public object Clone()
    {
        var clone = (ChartDataInput) this.MemberwiseClone();
        HandleCloned(clone);
        return clone;
    }

    private void HandleCloned(ChartDataInput clone)
    {
        var series = new List<PointArray<float?>>();
        foreach (var item in InputDataSeries)
        {
            series.Add((PointArray<float?>)item.Clone());
        }
        clone.InputDataSeries = series;
    }

    public ChartDataInput UpdateCompressYLabel(UiBool no)
    {
        CompressYLabel = UiBool.No;
        return this;
    }
}

public enum CHART_TYPE
{
    LINE_CHART,
    BAR_CHART,
    PIE_CHART
}

