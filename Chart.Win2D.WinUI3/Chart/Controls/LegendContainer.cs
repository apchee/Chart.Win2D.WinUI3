using Chart.Framework.Elements;
using ChartBase.Chart.Scenes;
using ChartBase.Models;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace ChartBase.Chart.Controls;

public class LegendContainer : ControlContainerBase
{

    private List<LegendControl> legendElements = new List<LegendControl>();

    //public LegendItem[] Legends { get; set; }

    public Legend LegendInfo { get; set; }

    public LegendContainer(SceneBase scene, Legend legendInfo)
    {
        this.Scene = scene;
        this.LegendInfo = legendInfo;
    }

    public override void CalcControlCoordinate(CanvasControl creator)
    {
        base.CalcControlCoordinate(creator);        
    }

    public override void Update(GenericInput gi, TimeSpan ts)
    {
        var vm = Scene.SceneManager.GlobalViewWindow;
        base.Update(gi, ts);
        legendElements.Clear();

        float starty = vm.LegendBeginningYAxis;
        float startx = vm.Room_Left_X;
        foreach (var legendElement in LegendInfo.LegendList)
        {            
            LegendControl le = new LegendControl(legendElement)
            {
                Location = new Vector2(startx, starty),               
                StrokeWidth = 1f,
                IsHoverable = true,
            };
            if (legendElement.IsClicked)
            {
                le.StrokeWidth += 1f;
            }
            startx += Scene.SceneManager.GlobalLegend.LegendWidth;
            if (startx + Scene.SceneManager.GlobalLegend.LegendWidth > vm.Room_Right_X)
            {
                startx = vm.Room_Left_X;
                starty += Legend.LegendRowHeight;
            }
            le.Update(gi, ts);  
            legendElements.Add(le);
        }
    }
    //public override void Clear()
    //{
    //    base.Reset();
    //    legendElements.Clear();
    //}
    //public override void Reset()
    //{
    //    base.Reset();
    //    legendElements.Clear();
    //}
    public override void Draw(CanvasDrawingSession cds)
    {
        foreach (var item in legendElements)
        {
            item.Draw(cds);
        }
        
    }

}

