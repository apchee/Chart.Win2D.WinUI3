namespace ChartBase.Models;

internal class RectangleShape: PointShape
{
    public RectangleShape(int id, float? ov, PointShape pointShape):base(id, ov)
    {
        base.RealValue = pointShape.RealValue;
        Location = pointShape.Location;
        Fill = pointShape.Fill;
        VirtualValue = pointShape.VirtualValue;
        IsValidPoint = pointShape.IsValidPoint;
    }


}
