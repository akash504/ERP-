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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage8 : Page
    {
        public BlankPage8()
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
                if (item is NavigationViewItem && item.Tag.ToString() == "Employee Profile")
                {
                    nvTopLevelNav.SelectedItem = item;
                    break;
                }
            }
            contentFrame.Navigate(typeof(Employee_Profile));
        }
        private void nvTopLevelNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TextBlock ItemContent = args.InvokedItem as TextBlock;
            if (ItemContent != null)
            {
                switch (ItemContent.Tag)
                {
                    case "Attendance":
                        contentFrame.Navigate(typeof(BlankPage7));
                        break;
                    case "Employee Profile":
                        contentFrame.Navigate(typeof(Employee_Profile));
                        break;
                    case "Home1":
                        this.Frame.Navigate(typeof(MainPage));
                        break;
                    case "Salary":
                        contentFrame.Navigate(typeof(salary));
                        break;

                }
            }
        }
        private void C_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
    

}
