using System;
using System.Collections.Generic;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using System.Threading.Tasks;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class QC : Page
    {
        string[] Autoitems, empList;
        public QC()
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
                    contentFrame.Navigate(typeof(QC));
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
                WorkOrderNo n = new WorkOrderNo();
                DateTime sysdt;
                sysdt = DateTime.Now;
                string Date = sysdt.ToString("dd/MM/yyyy");
                fDate.Text = Date;
                string Time1 = sysdt.ToString("hh:mm tt");
                fTime.Text = Time1;
                // AutoSuggestion for Popup Name
                FirebaseResponse response = await client.GetAsync("Manufacturing/Spinner/pending/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Manufacturing/Spinner/pending/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt.Add(nameNw);
                }
                int cnt = nameLt.Count;
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();

                FirebaseResponse response5 = await client.GetAsync("Spinner/Employee/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Employee/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt5.Add(nameNw);
                }
                int cnt5 = nameLt5.Count;
                empList = new string[cnt5];
                empList = nameLt5.ToArray();
                if (nameLt5.Count == count5)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    qc.Visibility = Visibility.Visible;

                }
            }
            catch (Exception)
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
        private void autoName(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = empList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void qOrder(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = fName.Text;
            updateInfo(cName);
        }

        private async void updateInfo(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/1/Name/");
            fProduct.Text = response1.ResultAs<string>();
            string a12 = fProduct.Text;
            FirebaseResponse response3 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/3/Name/");
            FirebaseResponse response4 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/4/Name/");
            fEQuantity.Text = response4.ResultAs<string>();
            FirebaseResponse response6 = await client.GetAsync("Products/" + a12 + "/Process/count/");
            int count2 = response6.ResultAs<int>();
            for (int i = 1; i <= count2; i++)
            {
                FirebaseResponse response7 = await client.GetAsync("Products/" + a12 + "/Process/" + i);
                string nameNw = response7.ResultAs<string>();
                if (nameNw == response3.ResultAs<string>() && i == count2)
                {
                    FirebaseResponse response = await client.GetAsync("Manufacturing/" + cName + "/process/" + nameNw + "/");
                    int a;
                    try
                    {
                        a = response.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        a = 0;
                    }
                    fProduced.Text = a.ToString();
                    break;
                }
                else if (nameNw == response3.ResultAs<string>() && i < count2)
                {
                    MessageDialog md = new MessageDialog("Not Eligible for QC");
                    await md.ShowAsync();
                    sbt.Visibility = Visibility.Collapsed;
                }

            }
        }
        private async void submit(object sender, RoutedEventArgs e)
        {
            if (fName.Text.Length > 0 && fProduct.Text.Length > 0 && fQName.Text.Length > 0 && fEQuantity.Text.Length > 0 && fProduct.Text.Length > 0 && faccepted.Text.Length > 0 && fScrap.Text.Length > 0 && fRework.Text.Length > 0 && fDate.Text.Length > 0 && fTime.Text.Length > 0)
            {
                try
                {
                    dispatchQC qc = new dispatchQC();
                    qc.workordernumber = fName.Text;
                    qc.productname = fProduct.Text;
                    qc.qcname = fQName.Text;
                    qc.equantity = Int32.Parse(fEQuantity.Text);
                    qc.pquantity = Int32.Parse(fProduced.Text);
                    qc.accepted = Int32.Parse(faccepted.Text);
                    qc.rejected = Int32.Parse(fScrap.Text);
                    qc.rework = Int32.Parse(fRework.Text);
                    qc.date = fDate.Text;
                    qc.time = fTime.Text;
                    FirebaseResponse response2 = await client.GetAsync("Manufacturing/RouteCard/" + qc.workordernumber + "/salesid/");
                    qc.salesordernumber = response2.ResultAs<string>();
                    FirebaseResponse response = await client.SetAsync("DispatchQC/" + qc.workordernumber + "/", qc);
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
        internal class dispatchQC
        {
            public string workordernumber { get; set; }
            public string salesordernumber { get; set; }
            public string productname { get; set; }
            public string qcname { get; set; }
            public int equantity { get; set; }
            public int pquantity { get; set; }
            public int accepted { get; set; }
            public int rejected { get; set; }
            public int rework { get; set; }
            public string date { get; set; }
            public string time { get; set; }
        }
    }
}