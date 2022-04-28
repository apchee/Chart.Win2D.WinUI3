using Chart.Framework;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using System;
using System.Numerics;

namespace ChartBase.Chart;

/// <summary>
/// Define a bouch of elements
/// 一个Scene对象只处理一种原始数据类型对应的逻辑, 但一种原始数据类型可被多个场景来处理
/// 
/// 原始数据类型主要是以一个原子数据由几个数据点组成. 比如:
///     比如点只有一个数据点组成, 但可以根据这个原始数据点或一序列数据点 can draw Point, cycle, Bar chart, Pie chart and so on.
///     OHLC consists of 4 original data points
/// </summary>
/// <typeparam name="O"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IScene : IElement
{
    public void SetContainerControl(IControlContainer controlContainer);

    public MySceneManager SceneManager { get; set; }
    public SceneViewWindow SceneViewWin { get; }

    public CombinationType Combination { get; }
    //public ChartDataInfo DataOverview{get;}

    public bool IsPrimaryScene { get; }

 
    void DataTransformation(ChartDataInput syntheticalDataInput);
}

[Flags]
public enum SceneTypes
{
    None        = 0b_0000_0000_0000_0000,
    Irrelevant  = 0b_0000_0000_0000_0001,
    Ordered     = 0b_0000_0000_0000_0010,
    //KeyPressable = 0b_0000_0000_0000_0100
}
