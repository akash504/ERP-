using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage8));
        }

        private void salesn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage2));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BlankPage1));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(purchase));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(manufacturing));
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(store));
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {

        }
        private void logoClick(object sender, RoutedEventArgs e)
        {
            Nav_Company.IsOpen = true;
        }
        private void web(object sender, RoutedEventArgs e)
        {
            Nav_Company.IsOpen = false;
            this.Frame.Navigate(typeof(website));
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(QC1));
        }
    }
}
