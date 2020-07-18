using System;
using System.Collections.Generic;
using System.IO;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
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
using Windows.UI.Popups;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{

    public sealed partial class BlankPage11 : Page
    {
        string[] Autoitems, productList1, sID;
        List<Product1> itemList = new List<Product1>();
        List<Sales1> salesList = new List<Sales1>();
        public BlankPage11()
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
                    contentFrame.Navigate(typeof(BlankPage11));
                    Debug.WriteLine("ctrl+R has been triggered");
                }
                if (e.VirtualKey == VirtualKey.Enter)
                {
                    Debug.WriteLine("Enter triggered");
                }

            };
        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void salesn_Click(object sender, RoutedEventArgs e)
        {

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
                PurchaseOrderId n = new PurchaseOrderId();
                client = new FireSharp.FirebaseClient(config);
                DateTime sysdt;
                sysdt = DateTime.Now;
                string Date = sysdt.ToString("dd/MM/yyyy");
                DateT.Text = Date;
                string Time1 = sysdt.ToString("hh:mm tt");
                TimeT.Text = Time1;
                // AutoSuggestion for Popup Name
                FirebaseResponse response = await client.GetAsync("Master/Spinner/Supplier/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Master/Spinner/Supplier/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt.Add(nameNw);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();
                FirebaseResponse response2 = await client.GetAsync("Spinner/StripSize/count/");
                int count2;
                try
                {
                    count2 = response2.ResultAs<int>();
                }
                catch (Exception)
                {
                    count2 = 0;
                }
                List<String> nameLt2 = new List<String>();
                for (int i = 1; i <= count2; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/StripSize/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt2.Add(nameNw);
                }
                int cnt2 = nameLt.Count;
                productList1 = new string[cnt2];
                productList1 = nameLt2.ToArray();

                FirebaseResponse response3 = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/count/");
                int count3;
                try
                {
                    count3 = response3.ResultAs<int>();
                }
                catch (Exception)
                {
                    count3 = 0;
                }
                List<String> nameLt3 = new List<String>();
                for (int i = 1; i <= count3; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt3.Add(nameNw);
                }
                int cnt3 = nameLt3.Count;
                sID = new string[cnt3];
                sID = nameLt3.ToArray();
                string av = n.PurchaseID;
                int inc = 0;
                FirebaseResponse respons = await client.GetAsync("Purchase/PurchaseOrder/count/");
                try
                {
                    inc = respons.ResultAs<int>();
                }
                catch (Exception)
                {
                    inc = 0;
                }
                inc++;
                av = av + inc.ToString("00");
                pOrderID.Text = av;
                if (nameLt3.Count == cnt3)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    po.Visibility = Visibility.Visible;

                }
            }
            catch (Exception)
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
        private void AutoSuggestBox_TextChanged2(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = productList1.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void AutoSuggestBox_TextChanged3(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = productList1.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void AutoSuggestBox_TextChanged4(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = sID.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Nav_Pop.IsOpen = true;
        }
        private void Popup_Finish(object sender, RoutedEventArgs e)
        {
            updateEnd();
            Nav_Pop.IsOpen = false;
        }
        public void updateEnd()
        {
            while (endList.Items.Count > 0)
            {
                int ii = endList.Items.Count - 1;
                endList.Items.RemoveAt(ii);
                ii++;
            }
            foreach (Product1 s in MyList2.Items)
            {
                endList.Items.Add(s);
            }
        }
        public void updateEnd2()
        {
            while (endList2.Items.Count > 0)
            {
                int ii = endList2.Items.Count - 1;
                endList2.Items.RemoveAt(ii);
                ii++;
            }
            foreach (Sales1 s in MyList.Items)
            {
                endList2.Items.Add(s);
            }
        }
        private void Popup_Submit(object sender, RoutedEventArgs e)
        {
            Product1 c1 = new Product1();
            c1.pName = pNameT.Text;
            c1.pQuantity = Int32.Parse(pQuantityT.Text);

            if (MyList2.Items.Count > 0)
            {
                c1.SNo = MyList2.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            itemList.Add(c1);
            MyList2.Items.Add(c1);
        }
        private void Popup_Submit2(object sender, RoutedEventArgs e)
        {
            Sales1 c1 = new Sales1();
            c1.salesID = pSalesID.Text;

            if (MyList.Items.Count > 0)
            {
                c1.SNo = MyList.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            salesList.Add(c1);
            MyList.Items.Add(c1);
        }
        private void Popup_Finish2(object sender, RoutedEventArgs e)
        {
            updateEnd2();
            Nav_Pop2.IsOpen = false;
        }
        private void MyList_ItemClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            MyList2.Items.Remove(x);
            changeSno();
        }
        private void MyList_ItemClick2(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            MyList.Items.Remove(x);
            changeSno2();
        }
        public void changeSno()
        {
            ListView sample = new ListView();
            foreach (Product1 s in MyList2.Items)
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
            while (MyList2.Items.Count > 0)
            {
                int ii = MyList2.Items.Count - 1;
                MyList2.Items.RemoveAt(ii);
                ii++;
            }

            foreach (Product1 s in sample.Items)
            {
                MyList2.Items.Add(s);
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (pOrderID.Text.Length > 0 && pSupplier.Text.Length > 0 && DateT.Text.Length > 0 && TimeT.Text.Length > 0 && endList.Items.Count > 0 && endList2.Items.Count > 0)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);
                    pOrderFinal a = new pOrderFinal();
                    a.purchaseNumber = pOrderID.Text;
                    a.supplierName = pSupplier.Text;
                    a.date = DateT.Text;
                    a.time = TimeT.Text;
                    FirebaseResponse response2 = await client.GetAsync("Purchase/PurchaseOrder/count/");
                    int c;
                    try
                    {
                        c = response2.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        c = 0;
                    }
                    c++;
                    FirebaseResponse firebaseResponse1 = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/count/");
                    int cnt;
                    try
                    {
                        cnt = firebaseResponse1.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        cnt = 0;
                    }
                    List<string> list2 = new List<string>();
                    for (int i = 1; i <= cnt; i++)
                    {
                        FirebaseResponse firebase = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/");
                        string abv = firebase.ResultAs<string>();
                        list2.Add(abv);
                    }
                    int booll = 0;
                    foreach (string y in list2)
                    {
                        if (y == a.purchaseNumber)
                        {
                            booll++;
                            break;
                        }
                    }
                    if (booll == 0)
                    {
                        cnt++;
                        FirebaseResponse firebaseResponse3 = await client.SetAsync("Purchase/PurchaseOrder/Spinner/pending/count/", cnt);
                        FirebaseResponse firebaseResponse = await client.SetAsync("Purchase/PurchaseOrder/Spinner/pending/" + cnt, a.purchaseNumber);
                        FirebaseResponse response3 = await client.SetAsync("Purchase/PurchaseOrder/count/", c);
                        FirebaseResponse ab = await client.SetAsync("Purchase/PurchaseOrder/" + a.purchaseNumber + "/", a);
                        foreach (Sales1 tempP in endList2.Items)
                        {
                            client = new FireSharp.FirebaseClient(config);
                            FirebaseResponse response = await client.SetAsync("Purchase/PurchaseOrder/" + a.purchaseNumber + "/salesLayout/" + tempP.SNo + "/", tempP);
                            FirebaseResponse response1 = await client.SetAsync("Purchase/PurchaseOrder/" + a.purchaseNumber + "/salesLayout/count/", tempP.SNo);
                        }
                        foreach (Product1 temp in endList.Items)
                        {
                            client = new FireSharp.FirebaseClient(config);
                            FirebaseResponse response = await client.SetAsync("Purchase/PurchaseOrder/" + a.purchaseNumber + "/itemLayout/" + temp.SNo + "/", temp);
                            FirebaseResponse response1 = await client.SetAsync("Purchase/PurchaseOrder/" + a.purchaseNumber + "/itemLayout/count/", temp.SNo);
                        }

                        MessageDialog md = new MessageDialog("SalesOrder Successfully Added!");
                        await md.ShowAsync();
                    }
                    else if (booll > 0)
                    {
                        MessageDialog md = new MessageDialog("SalesOrder Already Raised");
                        await md.ShowAsync();
                    }
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

        public void changeSno2()
        {
            ListView sample = new ListView();
            foreach (Sales1 s in MyList.Items)
            {
                sample.Items.Add(s);
            }
            ListView temp = new ListView();
            int i = 1;
            foreach (Sales1 s in sample.Items)
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
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Nav_Pop2.IsOpen = true;
        }
    }
    internal class Sales1
    {
        public int SNo { get; set; }
        public string salesID { get; set; }
    }
    internal class pOrderFinal
    {
        public string purchaseNumber { get; set; }
        public string supplierName { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string coilNo { get; set; }
    }
}
