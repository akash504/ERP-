using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
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
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Drawing;
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage9 : Page
    {
        List<Product1> dataList = new List<Product1>();
        string[] Autoitems, cNameitem;
        public BlankPage9()
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
                    contentFrame.Navigate(typeof(BlankPage9));
                    Debug.WriteLine("ctrl+R has been triggered");
                }
                if (e.VirtualKey == VirtualKey.Enter)
                {
                    Debug.WriteLine("Enter triggered");
                }

            };
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };
        IFirebaseClient client;
        public string pName, pQuantity;

        private async void Page_Loader(object sender, RoutedEventArgs e)
        {
            try
            {
                SalesOrderID n = new SalesOrderID();
                client = new FireSharp.FirebaseClient(config);
                DateTime sysdt;
                sysdt = DateTime.Now;
                string Date = sysdt.ToString("dd/MM/yyyy");
                DateT.Text = Date;
                string Time1 = sysdt.ToString("hh:mm tt");
                TimeT.Text = Time1;
                // AutoSuggestion for Popup Name
                FirebaseResponse response = await client.GetAsync("Spinner/Product/count");
                int count;
                try
                {
                    count = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    count = 0;
                }
                List<String> nameLt = new List<String>();
                for (int i = 1; i <= count; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Product/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt.Add(nameNw);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();
                orderT.Text = n.saleID;
                // AutoSuggestion for Client Name
                response = await client.GetAsync("Master/Spinner/Client/count");
                count = response.ResultAs<int>();
                List<String> cnameLt = new List<String>();
                for (int i = 1; i <= count; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Master/Spinner/Client/" + i);
                    string nameNw = response1.ResultAs<string>();
                    cnameLt.Add(nameNw);
                }
                cnt = cnameLt.Count;
                cNameitem = new string[cnt];
                cNameitem = cnameLt.ToArray();
                cNameT.IsHitTestVisible = true;
                if (cnameLt.Count == count)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    so.Visibility = Visibility.Visible;

                }
            }

            catch (FireSharp.Exceptions.FirebaseException)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Nav_Pop.IsOpen = true;
        }

        public void Popup_Submit(object sender, RoutedEventArgs e)
        {
            Product1 c1 = new Product1();
            c1.pName = pNameT.Text;
            c1.pQuantity = Int32.Parse(pQuantityT.Text);

            if (MyList.Items.Count > 0)
            {
                c1.SNo = MyList.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            dataList.Add(c1);
            MyList.Items.Add(c1);
        }
        private void Button_Click5(object sender, RoutedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private async void pListT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageDialog mg = new MessageDialog("Changed!");
            await mg.ShowAsync();
        }

        private void MyList_ItemClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            MyList.Items.Remove(x);
            changeSno();
        }
        public void updateEnd()
        {
            while (endList.Items.Count > 0)
            {
                int ii = endList.Items.Count - 1;
                endList.Items.RemoveAt(ii);
                ii++;
            }
            foreach (Product1 s in MyList.Items)
            {
                endList.Items.Add(s);
            }
        }
        public void changeSno()
        {
            ListView sample = new ListView();
            foreach (Product1 s in MyList.Items)
            {
                sample.Items.Add(s);
            }
            ListView temp = new ListView();
            int i = 1;
            foreach (Product1 s in sample.Items)
            {
                s.SNo = i;
                i++;
                temp.Items.Add(s);
            }
            while (MyList.Items.Count > 0)
            {
                int ii = MyList.Items.Count - 1;
                MyList.Items.RemoveAt(ii);
                ii++;
            }

            foreach (Product1 s in sample.Items)
            {
                MyList.Items.Add(s);
            }
        }

        private void cNameT_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = cNameitem.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
            else
            {

            }
        }
        private async void updateClientInfo(string Name)
        {
            IFirebaseConfig config1 = new FirebaseConfig
            {
                AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                BasePath = "https://weldonetechnocrats.firebaseio.com/"

            };
            var client1 = new FireSharp.FirebaseClient(config1);
            Name = cNameT.Text;
            FirebaseResponse response1 = await client1.GetAsync("Master/Client/" + Name);
            Client var = response1.ResultAs<Client>();
            try
            {
                cAddT.Text = var.Address;
                cNoT.Text = var.ContactNo;
                cMailT.Text = var.MailId;
            }
            catch (Exception)
            {
                updateClientInfo(Name);
            }
        }

        private void cNameT_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = cNameT.Text;
            updateClientInfo(cName);
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if ((cNameT.Text.Length > 0) && (cMailT.Text.Length > 0) && (cMailT.Text.Length > 0) && (cNoT.Text.Length > 0) && (cAddT.Text.Length > 0) && (DateT.Text.Length > 0) && (TimeT.Text.Length > 0) && (MyList.Items.Count > 0) == true)
            {
                try
                {
                    SOrder temp = new SOrder();
                    temp.ClientAddress = cAddT.Text;
                    temp.ClientContactNumber = cNoT.Text;
                    temp.ClientMailID = cMailT.Text;
                    temp.ClientName = cNameT.Text;
                    temp.Date = DateT.Text;
                    temp.Time = TimeT.Text;
                    temp.SalesOrderID = orderT.Text;
                    temp.status = "Pending";
                    String loc = "Sales/SalesOrder/" + temp.SalesOrderID + "/";
                    client = new FireSharp.FirebaseClient(config);
                    var settr = await client.SetAsync(loc, temp);
                    loc = "Sales/SalesOrder/" + temp.SalesOrderID + "/Layout/";
                    int len = loc.Length;
                    client = new FireSharp.FirebaseClient(config);
                    int i = 1;
                    foreach (Product1 tempP in MyList.Items)
                    {
                        Product temp1 = new Product();
                        temp1.pName = tempP.pName;
                        temp1.pQuantity = tempP.pQuantity;
                        loc = loc + i + "/";
                        var settr1 = await client.SetAsync(loc, temp1);
                        loc = loc.Remove(len);
                        i++;
                    }
                    loc = loc + "count/";
                    var sett = await client.SetAsync(loc, i - 1);
                    loc = loc.Remove(len);
                    loc = "Sales/SalesOrder/count";
                    sett = await client.SetAsync(loc, temp.getCount());
                    // Get Pending Count
                    IFirebaseClient client1 = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = await client.GetAsync("Sales/SalesOrder/Spinner/Pending/count/");
                    int pCount, flag1 = 0;
                    try
                    {
                        pCount = response.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        pCount = 0;
                    }
                    for (i = 1; i <= pCount; i++)
                    {
                        response = await client.GetAsync("Sales/SalesOrder/Spinner/Pending/" + i + "/");
                        string SOID = response.ResultAs<string>();
                        if (SOID == orderT.Text)
                        {
                            flag1 = 1;
                            break;
                        }
                    }
                    if (flag1 != 1)
                    {
                        pCount = pCount + 1;

                    }
                    loc = "Sales/SalesOrder/Spinner/Pending/" + pCount + "/";
                    sett = await client.SetAsync(loc, temp.SalesOrderID);
                    loc = "Sales/SalesOrder/Spinner/Pending/count/";
                    sett = await client.SetAsync(loc, pCount);
                    loc = "Sales/SalesOrder/count";
                    MessageDialog md = new MessageDialog("SalesOrder Successfully Updated!");
                    await md.ShowAsync();
                }
                catch (FireSharp.Exceptions.FirebaseException)
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
        private void Popup_Finish(object sender, RoutedEventArgs e)
        {
            Nav_Pop.IsOpen = false;
            updateEnd();
        }

    }
    internal class SOrder
    {
        public string SalesOrderID, ClientName, ClientMailID, ClientContactNumber, ClientAddress, Date, Time, ListView, status;
        public int getCount()
        {
            int count = Int32.Parse(SalesOrderID.Substring(7));
            return count;
        }
    }
    internal class Product1
    {
        public int SNo { get; set; }
        public string pName { get; set; }
        public int pQuantity { get; set; }
    }
}
