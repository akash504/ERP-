using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class manufacturing : Page
    {
        public manufacturing()
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
                if (item is NavigationViewItem && item.Tag.ToString() == "Route Card")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(BlankPage14));
        }
        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    case "Production Entry":
                        contentFrame.Navigate(typeof(BlankPage15));
                        break;
                    case "Route Card":
                        contentFrame.Navigate(typeof(BlankPage14));
                        break;

                    case "Route Card Review":
                        contentFrame.Navigate(typeof(Routecard_Review));
                        break;
                    case "Semi Finished Goods":
                        contentFrame.Navigate(typeof(SemiFinishedGoods));
                        break;
                    case "Home1":
                        this.Frame.Navigate(typeof(MainPage));
                        break;

                }
            }
        }


    }
}
