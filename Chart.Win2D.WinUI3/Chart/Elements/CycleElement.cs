using ChartBase.Chart.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.UI;

namespace ChartBase.Chart.Elements;

public class CycleElement : ControlBase
{
    public bool Fill { get; set; } =true;
    public int ConnectTo { get; set; }
    public float Radius { get; set; }
    public CycleElement(Vector2 location, float radius, Color forgroundColor) : base()
    {
        Location = location;
        Radius = radius;
        ForgroundColor = forgroundColor;
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        if (Fill)
        {
            cds.FillCircle(Location, Radius, ForgroundColor);
        }
        else
        {
            cds.DrawCircle(Location, Radius, ForgroundColor);
        }
    }

    public override bool OnHover(GenericInput gi)
    {
        if (gi is MouseGenericInput)
        {
            MouseGenericInput mgi = gi as MouseGenericInput;
            double distance = Math.Sqrt(Math.Abs(Location.X - mgi.X) * Math.Abs(Location.X - mgi.X) + Math.Abs(Location.Y - mgi.Y) * Math.Abs(Location.Y - mgi.Y));
            return distance < Radius;
        }
        else
        {
            return false;
        }

    }

    public override void CalcViewWorldScale()
    {
        
    }

    public override void CalcControlCoordinate(CanvasControl c)
    {
        
    }
}

