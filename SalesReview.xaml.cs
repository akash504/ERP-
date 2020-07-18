using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
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
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    public sealed partial class SalesReview : Page
    {
        // Global Variables
        SOrder selected = new SOrder();
        List<string> pSOrder = new List<string>();
        string[] Autoitems;
        int count, flag = 0;
        Layout[] layouts;
        // Main Begins
        public SalesReview()
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
                    contentFrame.Navigate(typeof(SalesReview));
                    Debug.WriteLine("ctrl+R has been triggered");
                }
                if (e.VirtualKey == VirtualKey.Enter)
                {
                    Debug.WriteLine("Enter triggered");
                }

            };
        }
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Declaring Firebase
            try
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                    BasePath = "https://weldonetechnocrats.firebaseio.com/"

                };
                IFirebaseClient client = new FireSharp.FirebaseClient(config);
                int i = 1;
                FirebaseResponse response;
                while (true)
                {
                    response = await client.GetAsync("Sales/SalesOrder/Spinner/Pending/" + i + "/");
                    string str = response.ResultAs<string>();
                    if (str != null)
                    {
                        pSOrder.Add(str);
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
                rSOrderT.ItemsSource = pSOrder;
                // AutoSuggestion for Popup Name
                response = await client.GetAsync("Spinner/Product/count");
                int count1;
                try
                {
                    count1 = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    count1 = 0;
                }
                SalesOrderID n = new SalesOrderID();
                List<String> nameLt = new List<String>();
                for (i = 1; i <= count1; i++)
                {
                    response = await client.GetAsync("Spinner/Product/" + i + "/");
                    string nameNw = response.ResultAs<string>();
                    nameLt.Add(nameNw);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();
                if (nameLt.Count == count1)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    sr.Visibility = Visibility.Visible;

                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }

        private async void rSOrderT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox temp = (ComboBox)sender;
            rSOrderT.Text = temp.SelectedItem.ToString();
            string sel = (string)temp.SelectedItem;
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                BasePath = "https://weldonetechnocrats.firebaseio.com/"

            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Sales/SalesOrder/" + sel + "/");
            selected = response.ResultAs<SOrder>();
            cNameT.Text = selected.ClientName;
            cMailT.Text = selected.ClientMailID;
            cNoT.Text = selected.ClientContactNumber;
            cAddT.Text = selected.ClientAddress;
            DateT.Text = selected.Date;
            TimeT.Text = selected.Time;
            updateEndLayout();
        }
        public async void updateEndLayout()
        {
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                BasePath = "https://weldonetechnocrats.firebaseio.com/"

            };
            IFirebaseClient client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Sales/SalesOrder/" + selected.SalesOrderID + "/Layout/count/");
            try
            {
                count = response.ResultAs<int>();
            }
            catch (Exception)
            {
                count = 0; ;
            }
            if (count > 0)
            {
                layouts = new Layout[count];
                for (int i = 1; i <= count; i++)
                {
                    string str = "Sales/SalesOrder/" + selected.SalesOrderID + "/Layout/" + i + "/";
                    IFirebaseConfig config1 = new FirebaseConfig
                    {
                        AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                        BasePath = "https://weldonetechnocrats.firebaseio.com/"

                    };
                    IFirebaseClient client1 = new FireSharp.FirebaseClient(config);
                    response = await client1.GetAsync(str);
                    layouts[i - 1] = response.ResultAs<Layout>();

                }
                updateEndList();
            }

        }
        void updateEndList()
        {
            if (endList.Items[0] != null)
            {
                endList.Items.Clear();
            }
            for (int i = 0; i < count; i++)
            {
                ListV temp = new ListV();
                temp.SNo = i + 1;
                try
                {
                    temp.pQuantity = layouts[i].pQuantity;
                }
                catch
                {
                    count = MyList.Items.Count();
                    layouts = new Layout[count];
                    for (i = 0; i < count; i++)
                    {
                        Layout l = new Layout();
                        l = (Layout)MyList.Items[i];
                        layouts[i] = l;
                    }
                    flag = 1;
                    updateEndList();
                    continue;
                }
                temp.pName = layouts[i].pName;
                endList.Items.Add(temp);
                if (flag != 1)
                {
                    MyList.Items.Add(layouts[i]);
                }

            }
        }
        void UpdateItems_Click(object sender, RoutedEventArgs e)
        {
            Nav_Pop.IsOpen = true;
        }

        void MyList_ItemClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            MyList.Items.Remove(x);
        }

        void Finish_Click(object sender, RoutedEventArgs e)
        {
            count = MyList.Items.Count();
            layouts = new Layout[count];
            for (int i = 0; i < count; i++)
            {
                Layout l = new Layout();
                l = (Layout)MyList.Items[i];
                layouts[i] = l;
            }
            flag = 1;
            updateEndList();
            Nav_Pop.IsOpen = false;
        }

        void Add_Click(object sender, RoutedEventArgs e)
        {
            Layout l = new Layout();
            l.pName = pNameT.Text;
            l.pQuantity = Int32.Parse(pQuantityT.Text);
            MyList.Items.Add(l);
        }
        void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        internal class Layout
        {
            public string pName { get; set; }
            public int pQuantity { get; set; }
        }
        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            // Send without Layout
            if (cAddT.Text.Length > 0 && cNoT.Text.Length > 0 && cMailT.Text.Length > 0 && cNameT.Text.Length > 0 && DateT.Text.Length > 0 && TimeT.Text.Length > 0 && rSOrderT.Text.Length > 0 && MyList.Items.Count > 0)
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
                    temp.SalesOrderID = rSOrderT.Text;
                    temp.status = "Verified";
                    String loc = "Sales/SalesOrder/" + temp.SalesOrderID + "/";
                    IFirebaseConfig config = new FirebaseConfig
                    {
                        AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                        BasePath = "https://weldonetechnocrats.firebaseio.com/"

                    };
                    IFirebaseClient client = new FireSharp.FirebaseClient(config);
                    var settr = await client.SetAsync(loc, temp);
                    //Send Layout Now
                    loc = "Sales/SalesOrder/" + temp.SalesOrderID + "/Layout/";
                    int len = loc.Length;
                    client = new FireSharp.FirebaseClient(config);
                    int i = 1;
                    foreach (Layout temp1 in MyList.Items)
                    {
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
                    // Get Verified Count
                    IFirebaseClient client1 = new FireSharp.FirebaseClient(config);
                    FirebaseResponse response = await client.GetAsync("Sales/SalesOrder/Spinner/Pending/count/");
                    int pCount;
                    int vCount;
                    FirebaseResponse response1 = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/count");
                    try
                    {
                        vCount = response1.ResultAs<int>();
                    }
                    catch
                    {
                        vCount = 0;
                    }
                    try
                    {
                        pCount = response.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        pCount = 0;
                    }
                    int flag1 = 0;
                    for (i = 1; i <= vCount; i++)
                    {
                        response = await client.GetAsync("Sales/SalesOrder/Spinner/Verified/" + i + "/");
                        string SOID = response.ResultAs<string>();
                        if (SOID == rSOrderT.Text)
                        {
                            flag1 = 1;
                            break;
                        }
                    }
                    if (flag1 != 1)
                    {
                        vCount = vCount + 1;
                        loc = "Sales/SalesOrder/Spinner/Pending/count/";
                        sett = await client.SetAsync(loc, pCount - 1);
                    }

                    loc = "Sales/SalesOrder/Spinner/Verified/" + vCount + "/";
                    sett = await client.SetAsync(loc, rSOrderT.Text);
                    loc = "Sales/SalesOrder/Spinner/Verified/count/";
                    sett = await client.SetAsync(loc, vCount);
                    loc = "Sales/SalesOrder/count";
                    // Pending Count Deletion
                    //response = await client.GetAsync("Sales/SalesOrder/Spinner/Pending/count/");
                    //pCount = response.ResultAs<int>();
                    response = await client.DeleteAsync("Sales/SalesOrder/Spinner/Pending/" + pCount);
                    //loc = "Sales/SalesOrder/Spinner/Pending/count/";
                    //sett = await client.SetAsync(loc, pCount - 1);
                    MessageDialog md = new MessageDialog("Sales Order Successfully Reviewed!");
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
        private void pNameT_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            pNameT.Text = args.ChosenSuggestion.ToString();
        }

        internal class ListV
        {
            public int SNo { get; set; }
            public string pName { get; set; }
            public int pQuantity { get; set; }
        }
    }
}
