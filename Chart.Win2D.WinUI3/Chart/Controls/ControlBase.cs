using ChartBase.Chart.Events;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.UI;

namespace ChartBase.Chart.Controls;

public abstract class ControlBase : IControl, IElement, IKeyPressable, IClickable
{
    public ControlBase()
    {
        this.Id = ViewWindow.Uid();
    }

    public abstract void Draw(CanvasDrawingSession cds);

    #region Events
    public event IKeyPressable.ElementEventHandler KeyPresse;
    public event IClickable.ElementEventHandler ButtonClick;


    public abstract bool OnHover(GenericInput gi);
    public virtual void CalcViewWorldScale() { }
    public virtual void CalcControlCoordinate(CanvasControl c) { }

    public virtual void Reset() { }


    #endregion


    #region ---- Properties ----



    public int Id { get; private set; }

    public Color ForgroundColor { get; set; }

    public bool IsHover { get; set; }

    public int ZIndex { get; set; }


    // Starting point on the screen for this scene story
    public Vector2 Location { get; set; } = new Vector2 { X = 0, Y = 0 };

    // Size of Canvas
    //public Vector2 WorkareaSize { get; set; }


    public virtual bool IsHoverable { get; set; } = false;


    public virtual void ButtonClicked(object sender, MouseClickEventArgs e)
    {
        if (IsHover)
            ButtonClick?.Invoke(sender, e);
    }

    public virtual void OnKeyPressed(object sender, KeyPressedEventArgs e)
    {
        KeyPresse?.Invoke(sender, e);
    }

    public virtual void Update(GenericInput gi, TimeSpan ts)
    {
        if (IsHoverable && gi is MouseGenericInput && (gi as MouseGenericInput).MouseInputType == MouseGenericInputType.MouseMove)
        {
            IsHover = OnHover(gi);
        }
    }

    public int X
    {
        get { return (int)Location.X; }
        set
        {
            if (Location.X != value)
            {
                Location = new Vector2(value, Location.Y);
            }
        }
    }

    public int Y
    {
        get { return (int)Location.Y; }
        set
        {
            if (Location.Y != value)
            {
                Location = new Vector2(Location.X, value);
            }
        }
    }

    #endregion
}

