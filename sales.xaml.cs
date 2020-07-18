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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    
    public sealed partial class BlankPage2 : Page
    {
        public BlankPage2()
        {
            this.InitializeComponent();
        }
        private void NavView_BackRequested(NavigationView sender,
                                  NavigationViewBackRequestedEventArgs args)
        {
            BackButton_Click();
        }

        private void BackButton_Click()
        {
            if (contentFrame.CanGoBack)
            {
                contentFrame.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(MainPage));
            }
        }
        private void nvTopLevelNav_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in nvTopLevelNav.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "Sales Order")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(BlankPage9));
        }
        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    case "Sales Order":
                        contentFrame.Navigate(typeof(BlankPage9));
                        break;
                    case "Sales Return":
                        contentFrame.Navigate(typeof(BlankPage10));
                        break;
                    case "Sales History":
                        contentFrame.Navigate(typeof(sales_history));
                        break;
                    case "Home1":
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    case "Sales Order Review":
                        contentFrame.Navigate(typeof(SalesReview));
                        break;


                }
            }
        }
    }
}
