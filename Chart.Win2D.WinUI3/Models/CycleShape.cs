namespace ChartBase.Models;
public class CycleShape : PointShape
{
    public CycleShape(int id, float ov) : base(id, ov)
    {
    }
    public float Radius { get; set; }
}