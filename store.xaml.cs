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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class store : Page
    {
        public store()
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
                if (item is NavigationViewItem && item.Tag.ToString() == "Finished Goods")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(Finished_Goods));
        }
        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    
                    case "Home1":
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    case "Product In":
                        contentFrame.Navigate(typeof(Product_In));
                        break;
                    case "Product Out":
                        contentFrame.Navigate(typeof(Product_Out));
                        break;
                    case "Finished Goods":
                        contentFrame.Navigate(typeof(Finished_Goods));
                        break;
                    case "Scrap":
                        contentFrame.Navigate(typeof(Scrub));
                        break;
                    case "Dispatch":
                        contentFrame.Navigate(typeof(Basic_Tools));
                        break;
                    case "Generate Excel":
                        contentFrame.Navigate(typeof(excel1));
                        break;


                }
            }
        }
    }
}
