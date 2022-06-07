using ChartBase.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chart.Demo.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class BarChartDemo : Page, INotifyPropertyChanged
{
    public BarChartDemo()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    public event PropertyChangedEventHandler PropertyChanged;


    private void Page_Loaded(object sender, RoutedEventArgs e)
    {

        MainChart = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { LineChartDemo.SERIES1 },
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Combination Line Chart with Overlay", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Year", ShowLabel = true },
                YLable = new ChartLabel() { Label = "Vertical Label", ShowLabel = true }
            }
        };

        var overlayBarChart = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ChartType = CHART_TYPE.BAR_CHART,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { LineChartDemo.SERIES2 }
        };
        OverlayChart = new List<ChartDataInput> { overlayBarChart };
    }



    private ChartDataInput _MainChart;
    public ChartDataInput MainChart
    {
        get { return _MainChart; }
        set
        {
            if (_MainChart != value)
            {
                _MainChart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MainChart)));
            }

        }
    }


    private List<ChartDataInput> _OverlayChart;
    public List<ChartDataInput> OverlayChart
    {
        get { return _OverlayChart; }
        set
        {
            if (_OverlayChart != value)
            {
                _OverlayChart = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverlayChart)));
            }

        }
    }

}
