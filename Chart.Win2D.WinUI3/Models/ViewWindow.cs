using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Threading;

namespace ChartBase.Models;

public class ViewWindow
{
    public ViewWindow() { }
    public DispatcherQueue UIThreadDispatcherQueue { get; set; }
    public float CanvasActualWidth  { get; set; }
    public float CanvasActualHeight { get; set; }


    public float LeftBoard_X, RightBoard_X, TopBoard_Y, Board_Button_Y;
    public Thickness Margins;
    //public float LeftMargin, TopMargin, RightMargin, ButtonMargin;

    public float BoardSolid_Left_X, BoardSolid_Right_X;
    public float BoardSolid_Top_Y, BoardSolid_Buttom_Y;

    public float Board_Width, Board_Height;

    public float BoardSolid_Width, BoardSolid_Height;

    public float TickYLongWidth, TickYShortWidth, TickXLongHeight, TickXShortHeight;
    public float TickerYPeriod, TickerXPeriod;


    public float Room_Top_Y, Room_Buttom_Y, Room_Left_X, Room_Right_X;
    public float Room_Width, Room_Height;
    public Thickness Room_Paddings;
    //float LeftPadding, TopPadding, RightPadding, ButtonPadding;
    public float RoomSolid_Top_Y, RoomSolid_Buttom_Y, RoomSolid_Left_X, RoomSolid_Right_X;
    public float RoomSolid_Width { get; set; }
    public float RoomSolid_Height { get; set; }


    public float MiddleLine_Y;

    #region Legend
    //// Legend 相关坐标数据是全局数据, 保存在 ViewBoard中
    //public bool ShowLegend { get; set; } = false;
    //public float LegendHeightTotally { get; set; }
    //public float LegendWidth { get; set; }
    //public float LegendFontSize { get; set; } = 12;

    ////public LegendItem[] LegendLabels { get; set; }

    //public const float MinWidthOfLegend = 30f;
    //public const float LegendRowHeight = 30f;
    #endregion

    //#region Labels
    //// A works area can have one and only one Title, Y Label, X Label
    //public ChartLabels Labels { get; set; }
    //public CombinationType Combination { get; set; }
    //public UiBool CompressYLabel { get; set; } = UiBool.Unknow;
    //public UiBool TrimDecimal { get; set; } = UiBool.Unknow;
    //#endregion


    public float Board_Edge { get; set; } = 20;
    public float LegendBeginningYAxis { get; set; }
   

    public const int BUTTON_EDGE = 20;

    private static int count = 0;
    public static int Uid()
    {
        return Interlocked.Increment(ref count);
    }


    public static float GetRadians(float angle)
    {
        return (float)(Math.PI * angle / 180.0);
    }
}

public enum UiBool
{
    Unknow,
    Yes,
    No
}
public enum CombinationType
{
    Unknown,
    NonCombination,
    Combination,
    Individual,
    Comparision
}

public enum MarkerType
{
    UnMarkable,
    Point,
    Circle,
    Pipeline
}

