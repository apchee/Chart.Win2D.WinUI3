using ChartBase.Models;
using ChartBase.utils;
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
public sealed partial class LineChartDemo : Page, INotifyPropertyChanged
{

    public static PointArray<float?> SERIES1 = new PointArray<float?>(new float?[] { 504, 450, 430, 400, 290, 150, 125, 145, 250.5f, 295.0f, 256.75f, 366.4f, 340.7f, 200, 270, 365.7f, 359.5f, 396.0f })
    {
        LineColor = Colors.Red,
        MarkValue = true,
        Marker = MarkerType.UnMarkable,
        Legend = "Series 1"
    };

    public static PointArray<float?> SERIES2 = new PointArray<float?>(new float?[] { 10, 15, 21, 28, 37, 48, 60, 58, 54, 53, 59, 64.5f, 69f, 75f, 86.5f, 97f, 99.5f, 92f })
    {
        LineColor = Colors.Orange,
        MarkValue = true,
        Marker = MarkerType.UnMarkable,
        Legend = "Series 2"
    };

    public static PointArray<float?> SERIES3 = new PointArray<float?>(new float?[] { 50400, 45000, 43000, 40000, 29000, 15000, 12500, 14500, 25000.5f, 29500.0f, 25600.75f, 36600.4f, 34000.7f, 20000, 27000, 36500.7f, 35900.5f, 39600.0f })
    {
        LineColor = Colors.Yellow,
        MarkValue = true,
        Marker = MarkerType.UnMarkable,
        Legend = "Series 3"
    };

    public static PointArray<float?> SERIES4 = new PointArray<float?>(new float?[] { 80, 75, 60, 50, 37, 20, 0, -10, -24, -33, -59, -40f, -30f, -20f, -5f, 5f, 25f, 35f })
    {
        LineColor = Colors.White,
        MarkValue = true,
        Marker = MarkerType.UnMarkable,
        Legend = "Series 4"
    };

    public static PointArray<float?> SERIES6 = new PointArray<float?>(new float?[] { 30, 25, 21, 35, 40, 50, 65, 64, 67, 72, 78, 86f, 99f, 105f, 103f, 112f, 118f, 125f })
    {
        LineColor = Colors.Green,
        MarkValue = true,
        Marker = MarkerType.UnMarkable,
        Legend = "Series 5"
    };


    public LineChartDemo()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void Page_Loaded(object sender, RoutedEventArgs e)
    {
        Logger.WriteLine("Page_Loaded", $"- {this.GetType().ToString()}");
        LineChartComparision = new ChartDataInput()
        {
            Combination = CombinationType.Comparision,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { SERIES1,SERIES2, SERIES6 },
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Comparision Line Chart", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Horizental Label", ShowLabel = true },
                YLable = new ChartLabel() { Label = "Vertical Label", ShowLabel = true }
            },
            ComparisingIndex = 10
        };


        LineChartIndividual = new ChartDataInput()
        {
            Combination = CombinationType.Individual,
            ShowLegend = true,
            MinXTickScale = 30,
            //InputDataSeries = new List<PointArray<float?>> {
            //    new PointArray<float?>(new float?[]{ 504, 450, 430, 300, 200}){
            //        LineColor = Colors.Yellow, MarkValue =true, Marker=MarkerType.UnMarkable, Legend = "Series 2"
            //    },
            //    new PointArray<float?>(new float?[]{ -60, -54, -45, -25, -9}){
            //        LineColor = Colors.Red, MarkValue =true, Marker=MarkerType.UnMarkable, Legend = "Series 1"
            //    }
            //},
            InputDataSeries = new List<PointArray<float?>> { SERIES1, SERIES2, SERIES4 },
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Individual Line Chart", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Year", ShowLabel = true },
                YLable = new ChartLabel() { Label = "上证指数", ShowLabel = true }
            }
        };

        LineChartCombination = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { SERIES1, SERIES2, SERIES4 },
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Combination Line Chart", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Year", ShowLabel = true },
                YLable = new ChartLabel() { Label = "上证指数", ShowLabel = true }
            }
        };

        PrimaryLineChartOverlay = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { SERIES1, SERIES2 },
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Combination Line Chart with Overlay", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Year", ShowLabel = true },
                YLable = new ChartLabel() { Label = "上证指数", ShowLabel = true }
            }
        };

        var ssd = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { SERIES3 }
        };
        SecondaryLineChartOverlay = new List<ChartDataInput> { ssd};


        LineChartDataCombination = new ChartDataInput()
        {
            Combination = CombinationType.Combination,
            ShowLegend = true,
            MinXTickScale = 30,
            InputDataSeries = new List<PointArray<float?>> { SERIES1, SERIES2, SERIES3},
            Labels = new ChartLabels()
            {
                Title = new ChartLabel() { Label = "Combination Line Chart", ShowLabel = true },
                XLabel = new ChartLabel() { Label = "Year", ShowLabel = true },
                YLable = new ChartLabel() { Label = "上证指数", ShowLabel = true }
            }
        };
    }



    private ChartDataInput _LineChartDataCombination;
    public ChartDataInput LineChartDataCombination
    {
        get { return _LineChartDataCombination; }
        set
        {
            if (_LineChartDataCombination != value)
            {
                _LineChartDataCombination = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineChartDataCombination)));
            }

        }
    }


    private ChartDataInput _LineChartWithOverlay;
    public ChartDataInput PrimaryLineChartOverlay
    {
        get { return _LineChartWithOverlay; }
        set
        {
            if (_LineChartWithOverlay != value)
            {
                _LineChartWithOverlay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PrimaryLineChartOverlay)));
            }

        }
    }

    private List<ChartDataInput> _SecondaryLineChartOverlay;
    public List<ChartDataInput> SecondaryLineChartOverlay
    {
        get { return _SecondaryLineChartOverlay; }
        set
        {
            if (_SecondaryLineChartOverlay != value)
            {
                _SecondaryLineChartOverlay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SecondaryLineChartOverlay)));
            }

        }
    }



    private ChartDataInput _LineChartComparision;
    public ChartDataInput LineChartComparision
    {
        get { return _LineChartComparision; }
        set
        {
            if (_LineChartComparision != value)
            {
                _LineChartComparision = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineChartComparision)));
            }

        }
    }


    private ChartDataInput _LineChartIndividual;
    public ChartDataInput LineChartIndividual
    {
        get { return _LineChartIndividual; }
        set
        {
            if (_LineChartIndividual != value)
            {
                _LineChartIndividual = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineChartIndividual)));
            }

        }
    }


    private ChartDataInput _LineChartCombination;
    public ChartDataInput LineChartCombination
    {
        get { return _LineChartCombination; }
        set
        {
            if (_LineChartCombination != value)
            {
                _LineChartCombination = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LineChartCombination)));
            }

        }
    }



    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        Logger.WriteLine("Page_Unloaded", $"- {this.GetType().ToString()}");

    }
}
