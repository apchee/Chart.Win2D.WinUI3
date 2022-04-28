using ChartBase.Chart.Controls;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Numerics;
using Windows.Foundation;

namespace ChartBase.Chart.Elements;

public class TextElement : SizableControl
{

    public HorizentalAlignment HAlign { get; set; } = HorizentalAlignment.Left;
    public VerticalAlignment VAlign { get; set; } = VerticalAlignment.Undefined;
    public string Text { get; set; }
    public float FontSize { get; set; } = 12;
    public string FontFamily { get; set; }
    public float Angle { get; set; }
    public bool Transform { get; set; }
    public float RequiredHeight { get; set; } = 1000f;
    public float RequiredWidth { get; set; } = 100f;
    float BeginningX;
    float BeginningY;

    public TextElement(string text)
    {
        this.Text = text;
    }

    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        CanvasTextLayout textLayout = new CanvasTextLayout(gi.Creator, Text, new CanvasTextFormat() { FontSize = FontSize }, RequiredHeight, RequiredWidth);
        BeginningX = Location.X;
        BeginningY = Location.Y;
        if(VAlign == VerticalAlignment.Top)
        {
            BeginningY -= (float)textLayout.LayoutBounds.Height;
        }else if (VAlign == VerticalAlignment.Center)
        {
            BeginningY -= (float)textLayout.LayoutBounds.Height/2;
        }
        if(HAlign == HorizentalAlignment.Center)
        {
            BeginningX -= (float)textLayout.LayoutBounds.Width / 2;
        }else if(HAlign == HorizentalAlignment.Right)
        {
            BeginningX -= (float)textLayout.LayoutBounds.Width;
        }
        Width = (float)textLayout.LayoutBounds.Width;
        Height = (float)textLayout.LayoutBounds.Height;
    }

    public override void Draw(CanvasDrawingSession cds)
    {
        CanvasTextFormat ctf = new CanvasTextFormat();
        ctf.VerticalAlignment = CanvasVerticalAlignment.Center;
        ctf.HorizontalAlignment = CanvasHorizontalAlignment.Center;
        ctf.FontSize = FontSize;

        var oldTransform = cds.Transform;


        if (Transform && Angle != 0)
        {
            var ot = cds.Transform;

            cds.Transform = Matrix3x2.CreateRotation(ViewWindow.GetRadians(Angle), new Vector2(BeginningX, BeginningY));
            cds.DrawText(Text, new Rect(BeginningX, BeginningY, Width, Height), ForgroundColor, ctf);

            cds.Transform = ot;
        }
        else
        {
            cds.DrawText(Text, new Rect(BeginningX, BeginningY, Width, Height), ForgroundColor, ctf);
        }
    }
}

public enum HorizentalAlignment
{
    Left,
    Center,
    Right
}

public enum VerticalAlignment
{
    Undefined,
    Top,
    Center,
    Bottom
}
