using ChartBase.Chart.Events;

namespace ChartBase.Chart.Controls;

public interface IKeyPressable {
    #region [Events ------------------------------------------------------]

    public delegate void ElementEventHandler(object sender, KeyPressedEventArgs e);

    event ElementEventHandler KeyPresse;

    #endregion
}

