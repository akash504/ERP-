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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
    public sealed partial class Product_In : Page
    {
        public Product_In()
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
                    contentFrame.Navigate(typeof(Product_In));
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
                FirebaseResponse response = await client.GetAsync("Purchase/PurchaseRegister/Spinner/finished/count");
                int c;
                try
                {
                    c = response.ResultAs<int>();
                }
                catch
                {
                    c = 0;
                }
                productIn result = new productIn();
                List<string> numberList = new List<string>();
                for (int i = 1; i <= c; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Purchase/PurchaseRegister/Spinner/finished/" + i + "/");
                    string ab = response1.ResultAs<string>();

                    pOrderFinal abc = new pOrderFinal();

                    FirebaseResponse response15 = await client.GetAsync("Purchase/PurchaseRegister/" + ab + "/");
                    abc = response15.ResultAs<pOrderFinal>();
                    result.sno = MyList.Items.Count + 1;
                    result.purchaseid = ab;
                    result.supplier = abc.supplierName;
                    result.date = abc.date;
                    result.time = abc.time;
                    result.coilno = abc.coilNo;
                    FirebaseResponse response2 = await client.GetAsync("Purchase/PurchaseRegister/" + ab + "/itemLayout/count/");
                    int count;
                    try
                    {
                        count = response2.ResultAs<int>();
                    }
                    catch
                    {
                        count = 0;
                    }

                    for (int i1 = 1; i1 <= count; i1++)
                    {
                        FirebaseResponse response3 = await client.GetAsync("Purchase/PurchaseRegister/" + ab + "/itemLayout/" + i1 + "/");
                        Product1 product1 = new Product1();
                        product1 = response3.ResultAs<Product1>();
                        productIn productIn = new productIn();
                        productIn.sno = result.sno;
                        productIn.purchaseid = result.purchaseid;
                        productIn.supplier = result.supplier;
                        productIn.date = result.date;
                        productIn.time = result.time;
                        productIn.coilno = result.coilno;
                        productIn.stripsize = product1.pName;
                        productIn.quantity = product1.pQuantity;

                        MyList.Items.Add(productIn);
                    }
                    if(MyList.Items.Count > 0)
                    {
                        progress1.Visibility = Visibility.Collapsed;
                        pin.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }

        }
        internal class productIn
        {
            public int sno { get; set; }
            public string purchaseid { get; set; }
            public string stripsize { get; set; }
            public string supplier { get; set; }
            public int quantity { get; set; }
            public string coilno { get; set; }
            public string date { get; set; }
            public string time { get; set; }
        }
    }
}
