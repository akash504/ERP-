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
    public sealed partial class purchase : Page
    {
        public purchase()
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
                if (item is NavigationViewItem && item.Tag.ToString() == "Purchase Order")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(BlankPage11));
        }
        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    case "Purchase Order":
                        contentFrame.Navigate(typeof(BlankPage11));
                        break;
                    case "Home1":
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    case "Purchase Return":
                        contentFrame.Navigate(typeof(BlankPage13));
                        break;
                    case "Purchase Register":
                        contentFrame.Navigate(typeof(BlankPage12));
                        break;

                }
            }
        }
        public static DependencyProperty PaneCustomContentProperty { get; }
    }
}
