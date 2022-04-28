using ChartBase.Chart;
using ChartBase.Chart.Controls;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using Windows.Graphics.Imaging;

namespace Chart.Framework.Elements;

public class ImageElement : SizableControl
{

    public ImageElement(CanvasBitmap cb):base()
    {
        Bitmap = cb;
        Width = cb.SizeInPixels.Width;
        Height = cb.SizeInPixels.Height;
    }

    public ImageElement(CanvasBitmap cb, BitmapSize size):base()
    {
        Bitmap = cb;
        Width = cb.SizeInPixels.Width;
        Height = cb.SizeInPixels.Height;
    }

    public override void Update(GenericInput gi, TimeSpan dt)
    {
        base.Update(gi, dt);

        if (IsWorkareaChanged && IsScalable) { 
            ScaleWidth = ViewWindowWidth/ Width;
            ScaleHeight = ViewWindowHeight / Height;
        }

    }



    public override void Draw(CanvasDrawingSession cds)
    {
        Transform2DEffect effect = ScaleImage();
        if (effect != null)
        {
            cds.DrawImage(effect, Location);
        }
        else
        {
            cds.DrawImage(Bitmap, Location);
        }
    }

    private Transform2DEffect ScaleImage()
    {
        if (IsScalable && ScaleWidth != 1 || ScaleHeight != 1)
        {
            Transform2DEffect effect = new Transform2DEffect() { Source = this.Bitmap };
            effect.TransformMatrix = Matrix3x2.CreateScale(ScaleWidth, ScaleHeight);
            return effect;
        }
        else
        {
            return null;
        }
    }

    public override void CalcViewWorldScale()
    {
        throw new NotImplementedException();
    }

    public override void CalcControlCoordinate(CanvasControl c)
    {
        throw new NotImplementedException();
    }

    public CanvasBitmap Bitmap { get; private set; }
    public bool IsScalable { get; set; } = true;
    public bool IsWorkareaChanged { get; set; } =  true;
    public float ScaleWidth { get; set; } = 1;
    public float ScaleHeight { get; set; } = 1;

    public bool IsMirrored { get; set; }

}

