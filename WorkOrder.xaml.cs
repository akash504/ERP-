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
    public sealed partial class BlankPage15 : Page
    {
        string[] Autoitems, empList, machineList, process;
        public BlankPage15()
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
                    contentFrame.Navigate(typeof(BlankPage15));
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

                if (nameLt.Count == count)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    wo.Visibility = Visibility.Visible;
                }
            }
            catch
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
        private void qWork(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = wOrderID.Text;
            updateInfo(cName);
        }
        private async void updateInfo(string Name)
        {
            IFirebaseConfig config1 = new FirebaseConfig
            {
                AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
                BasePath = "https://weldonetechnocrats.firebaseio.com/"

            };
            var client1 = new FireSharp.FirebaseClient(config1);
            Name = wOrderID.Text;

            FirebaseResponse response1 = await client1.GetAsync("Manufacturing/RouteCard/" + Name + "/");
            routeCard var = response1.ResultAs<routeCard>();
            updateMaster(Name);

            try
            {
                FirebaseResponse response = await client1.GetAsync("Manufacturing/RouteCard/" + Name + "/processLayout/1/Name/");
                string a = response.ResultAs<string>();
                fProduct.Text = a;
                fBatch.Text = var.batchNo;
                updateProcess(a);

            }
            catch (Exception)
            {
                updateInfo(Name);
            }
        }

        private void aOperator(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = empList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }



        private void aOrder(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void aMachine(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = machineList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void aProcess(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = process.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }


        private async void updateProcess(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response2 = await client.GetAsync("Products/" + cName + "/Process/count/");
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
                FirebaseResponse response1 = await client.GetAsync("Products/" + cName + "/Process/" + i);
                string nameNw = response1.ResultAs<string>();
                nameLt2.Add(nameNw);
            }
            int cnt2 = nameLt2.Count;
            process = new string[cnt2];
            process = nameLt2.ToArray();
        }

        private async void updateMaster(string cName)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response5 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/employeeLayout/count");
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
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/employeeLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                string nameNw = list1.Name;
                nameLt5.Add(nameNw);
            }
            int cnt5 = nameLt5.Count;
            empList = new string[cnt5];
            empList = nameLt5.ToArray();



            FirebaseResponse response6 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/machineLayout/count");
            int count6;
            try
            {
                count6 = response6.ResultAs<int>();
            }
            catch (Exception)
            {
                count6 = 0;
            }
            List<String> nameLt6 = new List<String>();
            for (int i = 1; i <= count6; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/machineLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                string nameNw = list1.Name;
                nameLt6.Add(nameNw);
            }
            int cnt6 = nameLt6.Count;
            machineList = new string[cnt6];
            machineList = nameLt6.ToArray();
        }

        private async void submit(object sender, RoutedEventArgs e)
        {
            if (wOrderID.Text.Length > 0 && fBatch.Text.Length > 0 && fProduct.Text.Length > 0 && fOperator.Text.Length > 0 && fMachine.Text.Length > 0 && fProcess.Text.Length > 0 && fInput.Text.Length > 0 && fProduced.Text.Length > 0 && fDate.Text.Length > 0 && fTime.Text.Length > 0)
            {
                try
                {
                    work1 card = new work1();
                    card.workid = wOrderID.Text;
                    card.batch = fBatch.Text;
                    card.productname = fProduct.Text;
                    card.operatorname = fOperator.Text;
                    card.machine = fMachine.Text;
                    card.process = fProcess.Text;
                    card.input = Int32.Parse(fInput.Text);
                    card.produced = Int32.Parse(fProduced.Text);
                    card.date = fDate.Text;
                    card.time = fTime.Text;

                    int iCount;
                    FirebaseResponse firebaseResponse1 = await client.GetAsync("Manufacturing/ProductionEntry/" + card.workid + "/count/");
                    int pCount;
                    try
                    {
                        pCount = firebaseResponse1.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        pCount = 0;
                    }
                    pCount++;
                    FirebaseResponse firebaseResponse2 = await client.SetAsync("Manufacturing/ProductionEntry/" + card.workid + "/count/", pCount);
                    FirebaseResponse response = await client.SetAsync("Manufacturing/ProductionEntry/" + card.workid + "/" + pCount + "/", card);

                    FirebaseResponse response5 = await client.GetAsync("Manufacturing/" + card.workid + "/process/" + card.process + "/");
                    int pCount2;
                    try
                    {
                        pCount2 = response5.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        pCount2 = 0;
                    }
                    pCount2 = pCount2 + card.produced;

                    try
                    {
                        FirebaseResponse firebaseResponse = await client.GetAsync("Inventory/SemifinishedGoods/" + card.productname + "/" + card.process + "/");
                        iCount = firebaseResponse.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        iCount = 0;
                    }
                    iCount = iCount + card.produced;
                    int len = process.Length;
                    for (int i = 0; i < len; i++)
                    {
                        if (card.process == process[i])
                        {
                            if (i == 0)
                            {
                                FirebaseResponse response2 = await client.GetAsync("Manufacturing/ProductionEntry/Spinner/finished/count");
                                int fCount;
                                try
                                {
                                    fCount = response2.ResultAs<int>();
                                }
                                catch (Exception)
                                {
                                    fCount = 0;
                                }
                                fCount++;
                                FirebaseResponse response3 = await client.SetAsync("Manufacturing/ProductionEntry/Spinner/finished/count", fCount);
                                FirebaseResponse response4 = await client.SetAsync("Manufacturing/ProductionEntry/Spinner/finished/" + fCount + "/", card.workid);


                                FirebaseResponse response6 = await client.SetAsync("Manufacturing/" + card.workid + "/process/" + card.process + "/", pCount2);
                                FirebaseResponse firebase = await client.GetAsync("Manufacturing/" + card.workid + "/StripsCount/");
                                int c;
                                c = firebase.ResultAs<int>();
                                c = c - card.input;
                                FirebaseResponse firebaseResponse = await client.SetAsync("Manufacturing/" + card.workid + "/StripsCount/", c);
                                FirebaseResponse response1 = await client.SetAsync("Inventory/SemifinishedGoods/" + card.productname + "/" + card.process + "/", iCount);
                            }
                            else if (i > 0)
                            {
                                FirebaseResponse response6 = await client.GetAsync("Manufacturing/" + card.workid + "/process/" + process[i - 1] + "/");
                                int d = response6.ResultAs<int>();
                                d = d - card.input;
                                FirebaseResponse response7 = await client.SetAsync("Manufacturing/" + card.workid + "/process/" + process[i - 1] + "/", d);
                                FirebaseResponse response61 = await client.SetAsync("Manufacturing/" + card.workid + "/process/" + card.process + "/", pCount2);

                                FirebaseResponse firebase = await client.GetAsync("Inventory/SemifinishedGoods/" + card.productname + "/" + process[i - 1] + "/");
                                int c;
                                c = firebase.ResultAs<int>();
                                c = c - card.input;
                                FirebaseResponse response21 = await client.SetAsync("Inventory/SemifinishedGoods/" + card.productname + "/" + process[i - 1] + "/", c);
                                FirebaseResponse response1 = await client.SetAsync("Inventory/SemifinishedGoods/" + card.productname + "/" + card.process + "/", iCount);
                            }
                        }
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
        internal class work1
        {
            public string workid { get; set; }
            public string batch { get; set; }
            public string productname { get; set; }
            public string operatorname { get; set; }
            public string machine { get; set; }
            public string process { get; set; }
            public int input { get; set; }
            public int produced { get; set; }
            public string date { get; set; }
            public string time { get; set; }
        }
    }
}