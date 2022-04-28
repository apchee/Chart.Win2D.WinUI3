using ChartBase.Chart;
using ChartBase.Chart.Scenes;

namespace Chart.Framework;

/// <summary>
/// 处理一组相关联的控件
/// </summary>
public interface IControlContainer:IControl, IElement
{
    public SceneBase Scene { get; set; }

    void Clear();
}

