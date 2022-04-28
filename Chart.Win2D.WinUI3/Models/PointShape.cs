using System.Numerics;

namespace ChartBase.Models;


public class PointShape : IPoint
{
    public int Id { get; set; }
    public PointShape(int id, float? ov)
    {
        Id = id;
        RealValue = ov;
    }


    public Vector2 Location { get; set; }
    public bool Fill { get; set; } = false;

    //public float? OriginalValue { get; set; }
    public float? RealValue { get; set; }

    private float? _virtualValue;
    public float? VirtualValue {
        get { 
            if (_virtualValue == null)
                return RealValue;
            else
                return _virtualValue;
        }
        set { _virtualValue = value; } 
    }
    public bool IsValidPoint { get; set; } = true;
}
