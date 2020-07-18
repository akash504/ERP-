using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
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
using static weldone.Basic_Tools;
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Product_Out : Page
    {
        public Product_Out()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += AcceleratorKeyActivated;

        }
        private void AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.VirtualKey == VirtualKey.R)
                {
                    contentFrame.Navigate(typeof(Product_Out));
                    Debug.WriteLine("ctrl+R has been triggered");
                }
                if (e.VirtualKey == VirtualKey.Enter)
                {
                    Debug.WriteLine("Enter triggered");
                }

            };
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };
        IFirebaseClient client;
        private async void Page_Loader(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = await client.GetAsync("Dispatch/Spinner/count/");
                int c;
                try
                {
                    c = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    c = 0;
                }
                for (int i = 1; i <= c; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Dispatch/Spinner/" + i + "/");
                    string a = response1.ResultAs<string>();
                    FirebaseResponse response3 = await client.GetAsync("Dispatch/" + a + "/count/");
                    int v;
                    try
                    {
                        v = response3.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        v = 0;
                    }
                    for (int j = 1; j <= v; j++)
                    {
                        FirebaseResponse response2 = await client.GetAsync("Dispatch/" + a + "/" + j + "/");
                        dispatch dispatch2 = new dispatch();
                        dispatch2 = response2.ResultAs<dispatch>();
                        productOut productOut = new productOut();
                        productOut.sno = MyList.Items.Count + 1;
                        productOut.salesid = dispatch2.salesordernumber;
                        FirebaseResponse response4 = await client.GetAsync("Sales/SalesOrder/" + productOut.salesid + "/ClientName/");
                        productOut.clientname = response4.ResultAs<string>();
                        productOut.productname = dispatch2.productname;
                        productOut.quantity = dispatch2.pquantity;
                        productOut.vehicleno = dispatch2.vehicleno;
                        productOut.date = dispatch2.date;
                        productOut.time = dispatch2.time;
                        MyList.Items.Add(productOut);
                    }
                    if(MyList.Items.Count > 0)
                    {
                        progress1.Visibility = Visibility.Collapsed;
                        pout.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
    }
    internal class productOut
    {
        public int sno { get; set; }
        public string salesid { get; set; }
        public string clientname { get; set; }
        public string productname { get; set; }
        public int quantity { get; set; }
        public string vehicleno { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}