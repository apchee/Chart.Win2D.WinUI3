using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartBase.Models;

public class SceneViewWindow
{

    #region X coordinate and Ticks attaching X coordinate
    // X 坐标 tick 为全局值应该保存在 SceneManager 中
    // X_Step 为全局值, 因为X轴只会有一个
    // 该变量应该保存在 ViewBoard 对象中
    // 该值应该在其主控件中计算
    public float X_StepSpace { get; set; }

    // The legth of this array should be the same as HorizentalTickCoordinators its value is X_Step
    public string[] HorizentalTicks { get; set; }

    public int ComparingPointIndex { get; set; }

    public bool ShowLegend { get; set; }

    // X 轴上坐标值, 每两个点之间最小值为30
    //public float[] HorizentalTickCoordinators { get; set; }
    public (bool IsValid, float Coor)[] HorizentalTickCoordinators { get; set; }


    private const float _MinHorizentalTickCoordinatorPeriod = 30;
    public static float MinHorizentalTickCoordinatorPeriod
    {
        get { return _MinHorizentalTickCoordinatorPeriod; }
        set { throw new Exception("Cannot change const value"); }
    }

    public int MinXTickScale { get; set; } = 30;


    public const float TickScaleHeightOrLength = 5;

    // Y 坐标只有在Combination type 为Combination时才会成为全局坐标
    // 如果该为全局坐标时应该保存在 SceneManager对你中,
    // 如果是 Individual 要保存在Container中
    // 如果是 Comparision 要保存在 WorldWin 对你中
    public float[] VerticalTicks { get; set; }

    #endregion


    #region Labels
    // A works area can have one and only one Title, Y Label, X Label
    public ChartLabels Labels { get; set; }
    public CombinationType Combination { get; set; }
    public UiBool CompressYLabel { get; set; } = UiBool.Unknow;
    public UiBool TrimDecimal { get; set; } = UiBool.Unknow;
    #endregion
}
