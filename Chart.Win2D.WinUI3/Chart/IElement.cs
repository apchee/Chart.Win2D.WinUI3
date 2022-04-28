using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;

namespace ChartBase.Chart;

/// <summary>
/// 2. 定义每一个组件都具有的接口
/// 1. Define APIs for Elements which reacte Mouse Or Keyboard inputs
/// </summary>
public interface IElement
{
    // Draw on screen
    public void Draw(CanvasDrawingSession cds);

    // Reacte Mouse or Keyboard event
    public void Update(GenericInput gi, TimeSpan ts);

    // Reacte Size changed Event
    public void CalcViewWorldScale();

    // 计算各控件在屏幕上的坐标位置
    public void CalcControlCoordinate(CanvasControl c);

    // Each element should have an ID
    public int Id { get; }
}

