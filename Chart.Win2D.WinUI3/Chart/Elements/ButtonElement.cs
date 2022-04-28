using ChartBase.Chart;
using ChartBase.Chart.Elements;
using ChartBase.Chart.Events;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.UI;
using System;
using Windows.UI;

namespace Chart.Framework.Elements;

public class ButtonElement:LabelElement
{

    /// <summary>
    /// CGL_Button constructor.
    /// </summary>
    /// <param name="text">The text to show in the middle of the button.</param>
    /// <param name="foreground_color">The color of the text.</param>
    /// <param name="width">The default width.</param>
    /// <param name="height">the default height.</param>
    public ButtonElement(string text = "", int fontSize=12, uint width = 100, uint height = 50, Color foreground_color = default(Color))
        : base(text, fontSize, width, height, foreground_color)
    {
        this.HighlightBorderColor = Colors.Lime;
        ButtonClick += OnCuttonClicked;
    }

    private void OnCuttonClicked(object sender, MouseClickEventArgs e)
    {
        
    }

    /// <summary>
    /// Response to hover event item
    /// Response to mouse click event
    /// </summary>
    /// <param name="dt">The delta time.</param>
    /// <param name="gi">The input item if any.</param>
    public override void Update(GenericInput gi, TimeSpan ts)
    {
        base.Update(gi, ts);
        if (gi is MouseGenericInput)
        {
            MouseGenericInput mgi = (MouseGenericInput)gi;
            switch (mgi.MouseInputType)
            {              

                case MouseGenericInputType.MouseClick:
                    {
                        if (IsHover)
                        {
                            // raise the event
                            ButtonClicked(this, new MouseClickEventArgs());
                        }
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Draw.
    /// </summary>
    /// <param name="cds">The surface to draw on.</param>
    public override void Draw(Microsoft.Graphics.Canvas.CanvasDrawingSession cds)
    {
        // the bounding rectangle
        Windows.Foundation.Rect outbound = new Windows.Foundation.Rect(Location.X, Location.Y, Width, Height);

        // draw the background
        if (BackgroundColor != Colors.Transparent)
        {
            cds.FillRectangle(new Windows.Foundation.Rect(outbound.X + 1, outbound.Y + 1, outbound.Width - 2, outbound.Height - 2), this.BackgroundColor);
        }

        // draw the border
        if (IsHover)
        {
            cds.DrawRectangle(outbound, HighlightBorderColor);
        }
        else
        {
            cds.DrawRectangle(outbound, BorderColor);
        }

        // create the text description
        CanvasTextFormat ctf = new CanvasTextFormat();
        ctf.VerticalAlignment = CanvasVerticalAlignment.Center;
        ctf.HorizontalAlignment = CanvasHorizontalAlignment.Center;
        ctf.FontSize = this.FontSize;

        // draw the text
        cds.DrawText(Text, outbound, ForgroundColor, ctf);
    }

    public override bool OnHover(GenericInput gi)
    {
        MouseGenericInput mgi = gi as MouseGenericInput;
        return mgi.X > X && mgi.X < X + Width - 1 && mgi.Y > Y && mgi.Y < Y + Height - 1;
    }


    #region [Properties --------------------------------------------------]

    public Color HighlightBorderColor { get; set; }

    #endregion


    #region [Events ------------------------------------------------------]

    #endregion
}

