using System.Numerics;

namespace ChartBase.Models;
public interface IPoint
{
    public bool IsValidPoint { get; set; }
    //public float? OriginalValue { get; set; }
    public float? RealValue { get; set; }
    public Vector2 Location { get; set; }
}
