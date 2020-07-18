using System;
using System.Collections.Generic;
using System.IO;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InwardQC : Page
    {
        string[] Autoitems, supplierList, empList, productList;
        public InwardQC()
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
                    contentFrame.Navigate(typeof(InwardQC));
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
                // AutoSuggestion for Popup Name
                FirebaseResponse response = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/count/");
                int count = response.ResultAs<int>();
                List<String> nameLt = new List<String>();
                for (int i = 1; i <= count; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Purchase/PurchaseOrder/Spinner/pending/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt.Add(nameNw);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();



                FirebaseResponse response6 = await client.GetAsync("Spinner/Employee/count/");
                int count6 = response6.ResultAs<int>();
                List<String> nameLt6 = new List<String>();
                for (int i = 1; i <= count6; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Employee/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt6.Add(nameNw);
                }
                int cnt6 = nameLt6.Count;
                empList = new string[cnt6];
                empList = nameLt6.ToArray();
                if(nameLt6.Count == count6)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    iqc.Visibility = Visibility.Visible;
                }
            }
            catch(Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
            }
        private void autoOrder(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void qOrder(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = fOrder.Text;
            updateInfo(cName);
        }
        private void qProduct(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = fProduct.Text;
            updateQuantity(cName);
        }

        private async void updateQuantity(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response5 = await client.GetAsync("Purchase/PurchaseOrder/" + fOrder.Text + "/itemLayout/count/");
            int count5 = response5.ResultAs<int>();
            for (int i = 1; i <= count5; i++)
            {
                FirebaseResponse response3 = await client.GetAsync("Purchase/PurchaseOrder/" + fOrder.Text + "/itemLayout/" + i + "/pName/");
                string nameNw = response3.ResultAs<string>();
                if(nameNw == cName)
                {
                    FirebaseResponse response4 = await client.GetAsync("Purchase/PurchaseOrder/" + fOrder.Text + "/itemLayout/" + i + "/pQuantity/");
                    string nameNw2 = response4.ResultAs<string>();
                    fRQuantity.Text = nameNw2;
                    break;
                }
                
            }
        }

        private async void updateInfo(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response1 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/itemLayout/count/");
            List<String> nameLt7 = new List<String>();
            int a = response1.ResultAs<int>();
            for (int i=1;i<=a;i++)
            {
                FirebaseResponse response2 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/itemLayout/"+i+"/pName/");
                string nameNw = response2.ResultAs<string>();
                nameLt7.Add(nameNw);
            }
            int cnt7 = nameLt7.Count;
            productList = new string[cnt7];
            productList = nameLt7.ToArray();
            FirebaseResponse response = await client.GetAsync("Purchase/PurchaseOrder/"+cName+"/supplierName/");
            fSupplier.Text = response.ResultAs<string>();
        }

        private void autoQC(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = empList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
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
        private async void submit(object sender, RoutedEventArgs e)
        {
            client = new FireSharp.FirebaseClient(config);
            Inwardqc a = new Inwardqc();
            a.purchaseid = fOrder.Text;
            a.productname = fProduct.Text;
            a.qcname = fQC.Text;
            a.supplier = fSupplier.Text;
            a.Equantity = Int32.Parse(fRQuantity.Text);
            a.Rquantity = Int32.Parse(fQRecieved.Text);
            a.accepted = Int32.Parse(fAccepted.Text);
            a.replacement = Int32.Parse(fRejected.Text);
            a.date = fDate.Text;
            a.time = fTime.Text;
            FirebaseResponse response1 = await client.SetAsync("QC/PurchaseInward/"+a.purchaseid+"/"+a.productname+"/Accepted/",a);


        }
    }
    internal class Inwardqc
    {
        public string purchaseid { get; set; }
        public string productname { get; set; }
        public string qcname { get; set; }
        public string supplier { get; set; }
        public int Equantity{ get; set; }
        public int Rquantity{ get; set; }
        public int accepted { get; set; }
        public int replacement { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}
