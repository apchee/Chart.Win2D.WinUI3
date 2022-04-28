using Chart.Demo.Views;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Chart.Demo
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            MyFrame.Navigate(typeof(LineChartDemo));
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FrameworkElement ele = e.AddedItems[0] as FrameworkElement;
            var tag = ele.Tag.ToString();
            if(tag == "LineChart")
            {
                MyFrame.Navigate(typeof(LineChartDemo));
            }
            else if (tag == "BarChart")
            {
                MyFrame.Navigate(typeof(BarChartDemo));
            }
        }

        private void ListBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            PInvoke.User32.ShowWindow(hwnd, PInvoke.User32.WindowShowStyle.SW_MAXIMIZE);
        }
    }
}
