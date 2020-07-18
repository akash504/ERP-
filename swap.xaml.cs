using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;

using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Popups;
using Windows.UI.Core;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.System;
using System.Diagnostics;

namespace weldone
{
public sealed partial class swap : Page
    {
       
        public swap()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }

        public class FinancialStuff
        {
            public string Name { get; set; }
            public int Amount { get; set; }
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }

        private void LoadChartContents()
        {
            Random rand = new Random();
            List<FinancialStuff> financialStuffList = new List<FinancialStuff>();
            financialStuffList.Add(new FinancialStuff() { Name = "Bolero", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "Indica", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "KTM", Amount = rand.Next(0, 200) });
            financialStuffList.Add(new FinancialStuff() { Name = "Telco207", Amount = rand.Next(0, 200) });
            (PieChart.Series[0] as PieSeries).ItemsSource = financialStuffList;
            (ColumnChart.Series[0] as ColumnSeries).ItemsSource = financialStuffList;
            (LineChart.Series[0] as LineSeries).ItemsSource = financialStuffList;
            (BarChart.Series[0] as BarSeries).ItemsSource = financialStuffList;
        }

        private void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }
    }

}
