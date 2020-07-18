using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using static weldone.QC;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Basic_Tools : Page
    {
        string[] Autoitems, productList;

        public Basic_Tools()
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
                    contentFrame.Navigate(typeof(Basic_Tools));
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
                DateTime sysdt;
                sysdt = DateTime.Now;
                string Date = sysdt.ToString("dd/MM/yyyy");
                fDate.Text = Date;
                string Time1 = sysdt.ToString("hh:mm tt");
                fTime.Text = Time1;
                FirebaseResponse response = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/count/");
                int c;
                try
                {
                    c = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    c = 0;
                }
                List<String> nameLt = new List<String>();
                for (int i = 1; i <= c; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/" + i + "/");
                    string ab = response1.ResultAs<string>();
                    nameLt.Add(ab);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();
                if (nameLt.Count == c)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    dis.Visibility = Visibility.Visible;

                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
        private void autoSales(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void autoProduct(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = productList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void qSales(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = fSales.Text;
            updateInfo(cName);
        }

        private async void updateInfo(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response5 = await client.GetAsync("Sales/SalesOrder/" + cName + "/Layout/count/");
            int count5;
            try
            {
                count5 = response5.ResultAs<int>();
            }
            catch (Exception)
            {
                count5 = 0;
            }
            List<String> nameLt5 = new List<String>();
            for (int i = 1; i <= count5; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Sales/SalesOrder/" + cName + "/Layout/" + i);
                Product list1 = new Product();
                list1 = response1.ResultAs<Product>();
                string nameNw = list1.pName;
                nameLt5.Add(nameNw);
            }
            int cnt5 = nameLt5.Count;
            productList = new string[cnt5];
            productList = nameLt5.ToArray();
        }

        private void qProduct(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = fProduct.Text;
            updateDetails(cName);
        }

        private async void updateDetails(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response5 = await client.GetAsync("Sales/SalesOrder/" + fSales.Text + "/Layout/count/");
            int count5;
            try
            {
                count5 = response5.ResultAs<int>();
            }
            catch (Exception)
            {
                count5 = 0;
            }
            List<String> nameLt5 = new List<String>();
            for (int i = 1; i <= count5; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Sales/SalesOrder/" + fSales.Text + "/Layout/" + i);
                Product list1 = new Product();
                list1 = response1.ResultAs<Product>();
                string nameNw = list1.pName;
                if (nameNw == cName)
                {
                    fEQuantity.Text = list1.pQuantity.ToString();
                }
            }
            FirebaseResponse response = await client.GetAsync("DispatchQC/Spinner/Verified/count/");
            int n;
            try
            {
                n = response.ResultAs<int>();
            }
            catch (Exception)
            {
                n = 0;
            }
            int finished = 0;
            for (int i = 1; i <= n; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("DispatchQC/Spinner/Verified/" + i + "/");
                string a = response1.ResultAs<string>();
                FirebaseResponse response2 = await client.GetAsync("DispatchQC/" + a + "/");
                dispatchQC dispatch = new dispatchQC();
                dispatch = response2.ResultAs<dispatchQC>();
                string ab = fSales.Text;
                if (dispatch.salesordernumber == ab && dispatch.productname == cName)
                {
                    finished = finished + dispatch.pquantity;
                }
            }
            fPQuantity.Text = finished.ToString();
            if (fPQuantity.Text == fEQuantity.Text)
            {
                fStatus.Text = "Fully Completed";
            }
            else if (finished < Int32.Parse(fEQuantity.Text))
            {
                fStatus.Text = "Partially Completed";
            }
        }
        private async void submit(object sender, RoutedEventArgs e)
        {
            if (fDate.Text.Length > 0 && fTime.Text.Length > 0 && fDriverContact.Text.Length > 0 && fDriverName.Text.Length > 0 && fEQuantity.Text.Length > 0 && fPQuantity.Text.Length > 0 && fSales.Text.Length > 0 && fProduct.Text.Length > 0 && fStatus.Text.Length > 0 && fVehicle.Text.Length > 0)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);
                    dispatch dispatch1 = new dispatch();
                    dispatch1.date = fDate.Text;
                    dispatch1.time = fTime.Text;
                    dispatch1.drivercontactno = fDriverContact.Text;
                    dispatch1.drivername = fDriverName.Text;
                    dispatch1.equantity = Int32.Parse(fEQuantity.Text);
                    dispatch1.pquantity = Int32.Parse(fPQuantity.Text);
                    dispatch1.salesordernumber = fSales.Text;
                    dispatch1.productname = fProduct.Text;
                    dispatch1.vehicleno = fVehicle.Text;
                    dispatch1.status = fStatus.Text;
                    FirebaseResponse firebase = await client.GetAsync("Dispatch/" + dispatch1.salesordernumber + "/count/");
                    int c;
                    try
                    {
                        c = firebase.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        c = 0;
                    }
                    c++;
                    FirebaseResponse firebaseResponse = await client.SetAsync("Dispatch/" + dispatch1.salesordernumber + "/count/", c);
                    FirebaseResponse response = await client.SetAsync("Dispatch/" + dispatch1.salesordernumber + "/" + c + "/", dispatch1);
                }
                catch (Exception)
                {
                    MessageDialog error = new MessageDialog("Failed to connect to database!");
                    this.Frame.Navigate(typeof(BlankPage6));
                }

            }
            else
            {
                MessageDialog md = new MessageDialog("Check the inputs!");
                await md.ShowAsync();
            }
        }
        internal class dispatch
        {
            public string salesordernumber { get; set; }
            public string productname { get; set; }
            public string status { get; set; }
            public int equantity { get; set; }
            public int pquantity { get; set; }
            public string vehicleno { get; set; }
            public string drivername { get; set; }
            public string drivercontactno { get; set; }
            public string date { get; set; }
            public string time { get; set; }
        }
    }
}
