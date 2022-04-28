using ChartBase.Chart.Controls;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace ChartBase.Chart.Elements;


public class LabelElement : SizableControl
{


    /// <summary>
    /// CGL_Label constructor.
    /// </summary>
    /// <param name="text">The text to show in the middle of the button.</param>
    /// <param name="foreground_color">The color of the text.</param>
    /// <param name="width">The default width.</param>
    /// <param name="height">the default height.</param>
    public LabelElement(string text = "", float fontSize = 18, float width = 100, float height = 50,  Color foreground_color = default(Color)) :base()
    {
        this.Text = text;
        this.ForgroundColor = foreground_color;
        this.BackgroundColor = Colors.Transparent;
        this.BorderColor = Colors.White;
        Width = width;
        Height = height;
        this.FontSize = fontSize;
    }



    /// <summary>
    /// Draw.
    /// </summary>
    /// <param name="cds">The surface to draw on.</param>
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

            cds.Transform = Matrix3x2.CreateRotation(ViewWindow.GetRadians(Angle), new Vector2(Location.X, Location.Y));
            cds.DrawText(Text, new Rect(Location.X, Location.Y, Width, Height), ForgroundColor, ctf);

            cds.Transform = ot;
        }
        else {
            cds.DrawText(Text, new Rect(Location.X, Location.Y, Width, Height), ForgroundColor, ctf);
        }

    }

    public override void CalcViewWorldScale()
    {
        
    }

    public override void CalcControlCoordinate(CanvasControl c)
    {
        
    }


    #region [Properties --------------------------------------------------]



    public string Text { get; set; }

    public float FontSize { get; set; }
    public string FontFamily { get; set; }
    public float Angle { get; internal set; }
    public bool Transform { get; internal set; }
    

    #endregion
}

