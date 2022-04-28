using Chart.Framework;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;

namespace ChartBase.Chart.Controls;

public abstract class ControlContainerBase : IControlContainer
{
    public ControlContainerBase()
    {
        Id = ViewWindow.Uid();
    }

    public List<IControl> GenericControls { get; set; } = new List<IControl>();

    public SceneBase Scene { get; set; }

    public int Id { get; private set; }

    public SceneViewWindow SceneViewWin { get { return Scene.SceneViewWin; } }

    //public abstract void AttachData<T>(T data);
    

    public virtual void Update(GenericInput gi, TimeSpan ts)
    {        
        //GenericControls.Clear();
    }

    public virtual void  Draw(CanvasDrawingSession cds)
    {
        foreach (var item in GenericControls)
        {
            item.Draw(cds);
        }
    }

    public virtual void CalcControlCoordinate(CanvasControl creator) {

    }

    public virtual void CalcViewWorldScale()
    {        
    }

    //public virtual void Reset()
    //{
    //    GenericControls.Clear();
    //}

    public virtual void Clear() {
        GenericControls.Clear();
    }
}
