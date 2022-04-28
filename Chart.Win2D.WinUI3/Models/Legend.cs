using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartBase.Models;

public class Legend
{
    public List<LegendItem> LegendList { get; set; }

    #region Legend
    // Legend 相关坐标数据是全局数据, 保存在 ViewBoard中
    public bool ShowLegend { get; set; } = false;
    public float LegendHeightTotally { get; set; }
    public float LegendWidth { get; set; }
    public float LegendHeight { get; set; }

    public float LegendFontSize { get; set; } = 12;

    //public LegendItem[] LegendLabels { get; set; }

    public const float MinWidthOfLegend = 30f;
    public const float LegendRowHeight = 30f;
    #endregion
}
