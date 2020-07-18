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

    public sealed partial class BlankPage13 : Page
    {
        string[] Autoitems, productList1, pID;
        List<Product1> itemList = new List<Product1>();
        List<Sales1> salesList = new List<Sales1>();
        public BlankPage13()
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
                    contentFrame.Navigate(typeof(BlankPage13));
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

                FirebaseResponse response3 = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt3.Add(nameNw);
                }
                int cnt3 = nameLt3.Count;
                pID = new string[cnt3];
                pID = nameLt3.ToArray();

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

        private void AutoSuggestBox_TextChanged2(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = productList1.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void autoOrder(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = pID.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void qOrder(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = pOrderID.Text;
            updateInfo(cName);
        }

        private async void updateInfo(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response1 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/supplierName/");
            pSupplier.Text = response1.ResultAs<string>();

            FirebaseResponse response4 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/itemLayout/count/");
            int c2;
            try
            {
                c2 = response4.ResultAs<int>();
            }
            catch (Exception)
            {
                c2 = 0;
            }
            for (int i = 1; i <= c2; i++)
            {
                FirebaseResponse response3 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/itemLayout/" + i + "/");
                Product1 sales2 = new Product1();
                sales2 = response3.ResultAs<Product1>();
                endList.Items.Add(sales2);
                MyList2.Items.Add(sales2);
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

        private void MyList_ItemClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            MyList2.Items.Remove(x);
            changeSno();
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
            if (pOrderID.Text.Length > 0 && pSupplier.Text.Length > 0 && DateT.Text.Length > 0 && TimeT.Text.Length > 0 && endList.Items.Count > 0)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);
                    pOrderFinal a = new pOrderFinal();
                    a.purchaseNumber = pOrderID.Text;
                    a.supplierName = pSupplier.Text;
                    a.date = DateT.Text;
                    a.time = TimeT.Text;
                    FirebaseResponse response2 = await client.GetAsync("Purchase/PurchaseReturn/Spinner/pending/count/");
                    int cnt;
                    try
                    {
                        cnt = response2.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        cnt = 0;
                    }
                    cnt++;
                    FirebaseResponse firebaseResponse3 = await client.SetAsync("Purchase/PurchaseReturn/Spinner/pending/count/", cnt);
                    FirebaseResponse firebaseResponse = await client.SetAsync("Purchase/PurchaseReturn/Spinner/pending/" + cnt, a.purchaseNumber);
                    FirebaseResponse ab = await client.SetAsync("Purchase/PurchaseReturn/" + a.purchaseNumber + "/", a);

                    foreach (Product1 temp in endList.Items)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response = await client.SetAsync("Purchase/PurchaseReturn/" + a.purchaseNumber + "/itemLayout/" + temp.SNo + "/", temp);
                        FirebaseResponse response1 = await client.SetAsync("Purchase/PurchaseReturn/" + a.purchaseNumber + "/itemLayout/" + temp.SNo + "/count/", temp.SNo);
                    }

                    MessageDialog md = new MessageDialog("Purchase Return Successfully Added!");
                    await md.ShowAsync();
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


    }

}
