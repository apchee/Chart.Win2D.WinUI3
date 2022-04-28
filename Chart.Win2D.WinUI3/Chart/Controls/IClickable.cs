using ChartBase.Chart.Events;

namespace ChartBase.Chart.Controls;

public interface IClickable
{

    #region [Events ------------------------------------------------------]

    public delegate void ElementEventHandler(object sender, MouseClickEventArgs e);

    event ElementEventHandler ButtonClick;

    #endregion
}

