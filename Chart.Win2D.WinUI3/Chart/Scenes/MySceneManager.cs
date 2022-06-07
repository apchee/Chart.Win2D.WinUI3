using ChartBase.Chart.Controls;
using ChartBase.Models;
using ChartBase.utils;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChartBase.Chart.Scenes;

public class MySceneManager : IElement
{
    public List<IScene> Scenes { get; set; } = new List<IScene>();
    public SceneBase PrimaryScene { get; set; }

    private List<IControl> GenericControls { get; } = new List<IControl>();

    public int Id { get; set; }

    public InputManager InputManager { get; set; }

    public ViewWindow GlobalViewWindow { get; set; }

    public Legend GlobalLegend { get; set; }

    public MySceneManager()
    {
        //ViewWindow = new ViewWindow();
    }

    internal void UpdatePrimarySceneData(ChartDataInput syntheticalDataInput)
    {
        PrimaryScene.DataTransformation(syntheticalDataInput);
    }

    internal bool CanUpdate()
    {
        return Scenes.Count > 0;
    }

    internal bool CanDraw()
    {
        return Scenes.Count > 0;
    }


    public void AddScene(IScene scene)
    {
        Scenes.Add(scene);
    }
    public void AddFirstScene(IScene scene)
    {
        Scenes.Insert(0, scene);
    }
    public void SetPrimaryScene(SceneBase scene)
    {
        AddScene(scene);
        PrimaryScene = scene;
    }

    /*
     * Re-initialize ViewWindow
     * 
     */
    public void CanvasSizeChanged(float actualWidth, float actualHeight)
    {
        GlobalViewWindow = new ViewWindow();
        GlobalViewWindow.CanvasActualWidth = actualWidth;
        GlobalViewWindow.CanvasActualHeight = actualHeight;
        GlobalViewWindow.TopBoard_Y = 0;
        GlobalViewWindow.LeftBoard_X = 0;

        GlobalViewWindow.Board_Height = GlobalViewWindow.CanvasActualHeight;
        GlobalViewWindow.Board_Button_Y = GlobalViewWindow.CanvasActualHeight;
        GlobalViewWindow.RightBoard_X = GlobalViewWindow.CanvasActualWidth;
        GlobalViewWindow.Board_Width = GlobalViewWindow.CanvasActualWidth;
    }


    internal void CalcEachLegendSpace(CanvasControl myCanvas)
    {
        foreach (var item in GlobalLegend.LegendList)
        {
            CanvasTextLayout textLayout = new CanvasTextLayout(myCanvas, item.Text, new CanvasTextFormat() { FontSize = item.FontSize }, 200f, 50f);
            item.LayoutBounds = textLayout.LayoutBounds;
        }
    }

    public void CalcCanvasBoardSize()
    {
        double left = 60;
        double top = 10;
        double right = 30;
        double buttom = ViewWindow.BUTTON_EDGE;
        buttom += 30; // for scale and tick
        if (PrimaryScene.SceneViewWin.Labels != null && PrimaryScene.SceneViewWin.Labels.YLable != null && PrimaryScene.SceneViewWin.Labels.YLable.ShowLabel)
            left = 100;
        if (PrimaryScene.SceneViewWin.Labels != null && PrimaryScene.SceneViewWin.Labels.Title != null && PrimaryScene.SceneViewWin.Labels.Title.ShowLabel)
            top = 80;
        if (PrimaryScene.SceneViewWin.Labels != null && PrimaryScene.SceneViewWin.Labels.XLabel != null && PrimaryScene.SceneViewWin.Labels.XLabel.ShowLabel)
            buttom += 30; // X Label
        if (PrimaryScene.SceneViewWin.Combination == CombinationType.Individual || PrimaryScene.SceneViewWin.Combination == CombinationType.Comparision)
            right = 60;
        // Default Margins
        GlobalViewWindow.Margins = new Thickness(left, top, right, buttom);

        // Default Romm Paddings
        GlobalViewWindow.Room_Paddings = new Thickness(10, 0, 0, 20);


        GlobalViewWindow.BoardSolid_Width = GlobalViewWindow.Board_Width - (float)(GlobalViewWindow.Margins.Left + GlobalViewWindow.Margins.Right);
        GlobalViewWindow.BoardSolid_Left_X = (float)GlobalViewWindow.Margins.Left;
        GlobalViewWindow.BoardSolid_Right_X = GlobalViewWindow.Board_Width - (float)GlobalViewWindow.Margins.Right;

        GlobalViewWindow.Room_Left_X = GlobalViewWindow.BoardSolid_Left_X;
        GlobalViewWindow.Room_Right_X = GlobalViewWindow.BoardSolid_Right_X;
        GlobalViewWindow.Room_Width = GlobalViewWindow.BoardSolid_Width;

        GlobalViewWindow.RoomSolid_Left_X = GlobalViewWindow.Room_Left_X + (float)GlobalViewWindow.Room_Paddings.Left;
        GlobalViewWindow.RoomSolid_Right_X = GlobalViewWindow.Room_Right_X - (float)GlobalViewWindow.Room_Paddings.Right;
        GlobalViewWindow.RoomSolid_Width = GlobalViewWindow.RoomSolid_Right_X - GlobalViewWindow.RoomSolid_Left_X;

        if (GlobalLegend.ShowLegend && GlobalLegend.LegendList != null && GlobalLegend.LegendList.Count > 0)
        {
            CalcLegendHeight();
            GlobalViewWindow.Margins = new Thickness(GlobalViewWindow.Margins.Left, GlobalViewWindow.Margins.Top, GlobalViewWindow.Margins.Right, GlobalViewWindow.Margins.Bottom + GlobalLegend.LegendHeightTotally);
        }

        GlobalViewWindow.BoardSolid_Height = GlobalViewWindow.Board_Height - (float)(GlobalViewWindow.Margins.Top + GlobalViewWindow.Margins.Bottom);

        GlobalViewWindow.BoardSolid_Top_Y = (float)GlobalViewWindow.Margins.Top;
        GlobalViewWindow.BoardSolid_Buttom_Y = GlobalViewWindow.Board_Height - (float)GlobalViewWindow.Margins.Bottom;
        if (GlobalViewWindow.BoardSolid_Buttom_Y < 10)
            return;

        GlobalViewWindow.Room_Height = GlobalViewWindow.BoardSolid_Height;

        GlobalViewWindow.Room_Top_Y = GlobalViewWindow.BoardSolid_Top_Y;
        GlobalViewWindow.Room_Buttom_Y = GlobalViewWindow.BoardSolid_Buttom_Y;


        GlobalViewWindow.RoomSolid_Top_Y = GlobalViewWindow.Room_Top_Y + (float)GlobalViewWindow.Room_Paddings.Top;
        GlobalViewWindow.RoomSolid_Buttom_Y = GlobalViewWindow.Room_Buttom_Y - (float)GlobalViewWindow.Room_Paddings.Bottom;

        GlobalViewWindow.RoomSolid_Height = GlobalViewWindow.RoomSolid_Buttom_Y - GlobalViewWindow.RoomSolid_Top_Y;

        GlobalViewWindow.MiddleLine_Y = GlobalViewWindow.RoomSolid_Top_Y + (GlobalViewWindow.RoomSolid_Buttom_Y - GlobalViewWindow.RoomSolid_Top_Y) / 2;
        if (GlobalViewWindow.RoomSolid_Width <= 10 || GlobalViewWindow.RoomSolid_Height <= 10)
        {
            Console.WriteLine("Warning");
        }
    }


    public virtual void CalcViewWorldScale()
    {
        StackFrame sf = (new StackTrace(0, true)).GetFrame(0);
        Logger.WriteLine($"{this.GetType()}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}", $"{Environment.NewLine}{Environment.StackTrace}");

        if (Scenes.Count > 0)
        {
            foreach (var item in Scenes)
            {
                item.CalcViewWorldScale();
            }
        }
    }

    private void CalcLegendHeight()
    {
        float maxLengthOfOneLegend = Legend.MinWidthOfLegend;
        foreach (LegendItem legendItem in GlobalLegend.LegendList)
        {
            if (legendItem.Text != null && legendItem.Text.Length > 0)
            {
                if (legendItem.LayoutBounds.Width + 30 > maxLengthOfOneLegend)
                {
                    legendItem.ActualWidth = (float)legendItem.LayoutBounds.Width + 30;
                    if (legendItem.ActualWidth > GlobalViewWindow.RoomSolid_Width)
                        legendItem.ActualWidth = GlobalViewWindow.RoomSolid_Width;
                    maxLengthOfOneLegend = legendItem.ActualWidth;
                }
            }
        }
        maxLengthOfOneLegend += 10;
        int RowNumbers = (int)Math.Ceiling((maxLengthOfOneLegend * GlobalLegend.LegendList.Count / (int)GlobalViewWindow.Room_Width));

        float height = Legend.LegendRowHeight * RowNumbers;

        if (RowNumbers == 1)
        {
            GlobalLegend.LegendWidth = (GlobalViewWindow.RoomSolid_Width) / GlobalLegend.LegendList.Count;
        }
        else if (RowNumbers == 2)
        {
            var numEachRow = (int)Math.Ceiling((double)GlobalLegend.LegendList.Count / 2);
            GlobalLegend.LegendWidth = (int)GlobalViewWindow.Room_Width / numEachRow;
        }
        else
        {
            GlobalLegend.LegendWidth = maxLengthOfOneLegend;
        }

        GlobalLegend.LegendHeightTotally = height;
        var beginningYAxis = GlobalViewWindow.Board_Height - (GlobalLegend.LegendHeightTotally + ViewWindow.BUTTON_EDGE);
        GlobalViewWindow.LegendBeginningYAxis = beginningYAxis;
        GlobalLegend.LegendHeight = height;
    }


    public void CalcControlCoordinate(CanvasControl c)
    {
        StackFrame sf = (new StackTrace(0, true)).GetFrame(0);
        Logger.WriteLine($"{this.GetType().ToString()}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}", $"{Environment.NewLine}{Environment.StackTrace}");

        foreach (var item in Scenes)
        {
            item.CalcControlCoordinate(c);
        }
    }


    /*
     * Clean up all Controlers includint Container Control and Scene
     * MySceneManager go back to original status
     * 
     */
    public void Reset()
    {
        Logger.WriteLine(GetType(), new StackTrace(0, true), $"{Environment.NewLine}{Environment.StackTrace}");

        Scenes.Clear();
        PrimaryScene = null;
        GenericControls.Clear();
    }

    public void Draw(CanvasDrawingSession cds)
    {
        if (Scenes.Count > 0)
        {
            foreach (var item in Scenes)
            {
                item.Draw(cds);
            }
        }
        foreach (var item in GenericControls)
        {
            item.Draw(cds);
        }
    }

    public void Update(GenericInput gi, TimeSpan ts)
    {
        StackFrame sf = (new StackTrace(0, true)).GetFrame(0);
        Logger.WriteLine($"{this.GetType().ToString()}#{sf?.GetMethod().Name}:{sf?.GetFileLineNumber()}", $"{Environment.NewLine}{Environment.StackTrace}");

        if (Scenes.Count > 0)
        {
            foreach (var item in Scenes)
            {
                item.Update(gi, ts);                
            }
        }
        UpdateLegendChart(gi, ts);
        //UpdateGlobalControls(gi.Creator);
    }

    public void UpdateLegendChart(GenericInput gi, TimeSpan ts)
    {

        if (GlobalLegend!=  null && GlobalLegend.ShowLegend && GlobalLegend.LegendList.Count > 0)
        {
            var col = new LegendContainer(PrimaryScene, GlobalLegend);
            col.Update(gi, ts);
            GenericControls.Add(col);
        }
    }
    public static void ShowDialog(string title)
    {
        ContentDialog dialog = new ContentDialog();
        dialog.Title = title;
        dialog.PrimaryButtonText = "Save";
        dialog.SecondaryButtonText = "Don't Save";
        dialog.CloseButtonText = "Cancel";
        dialog.DefaultButton = ContentDialogButton.Primary;
        //dialog.Content = new ContentDialogContent();

        var result = dialog.ShowAsync();
    }

   
    // 该变量移到 CombinationScene 中
    //public float YAxisScale { get; set; }

}

