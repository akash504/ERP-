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
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

namespace weldone
{

    public sealed partial class Routecard_Review : Page
    {
        string[] sID, Autoitems, productList1, prodName, empList, machineList, toolList, process, orderList;
        List<employeeList1> eList1 = new List<employeeList1>();
        List<employeeList1> mList1 = new List<employeeList1>();
        List<employeeList1> tList1 = new List<employeeList1>();
        public Routecard_Review()
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
                    contentFrame.Navigate(typeof(Routecard_Review));
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void Page_Loader(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = await client.GetAsync("Master/Spinner/CoilNo/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Master/Spinner/CoilNo/" + i);
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
                int cnt2 = nameLt2.Count;
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
                int cnt3 = nameLt.Count;
                sID = new string[cnt3];
                sID = nameLt3.ToArray();

                FirebaseResponse response4 = await client.GetAsync("Spinner/Product/count/");
                int count4;
                try
                {
                    count4 = response4.ResultAs<int>();
                }
                catch (Exception)
                {
                    count4 = 0;
                }
                List<String> nameLt4 = new List<String>();
                for (int i = 1; i <= count4; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Product/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt4.Add(nameNw);
                }
                int cnt4 = nameLt4.Count;
                prodName = new string[cnt4];
                prodName = nameLt4.ToArray();

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



                FirebaseResponse response6 = await client.GetAsync("Spinner/MachineId/count/");
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
                    FirebaseResponse response1 = await client.GetAsync("Spinner/MachineId/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt6.Add(nameNw);
                }
                int cnt6 = nameLt6.Count;
                machineList = new string[cnt6];
                machineList = nameLt6.ToArray();

                FirebaseResponse response7 = await client.GetAsync("Spinner/Tools/count/");
                int count7;
                try
                {
                    count7 = response7.ResultAs<int>();
                }
                catch (Exception)
                {
                    count7 = 0;
                }
                List<String> nameLt7 = new List<String>();
                for (int i = 1; i <= count7; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Tools/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt7.Add(nameNw);
                }
                int cnt7 = nameLt7.Count;
                toolList = new string[cnt7];
                toolList = nameLt7.ToArray();

                FirebaseResponse response8 = await client.GetAsync("Manufacturing/Spinner/pending/count/");
                int count8;
                try
                {
                    count8 = response8.ResultAs<int>();
                }
                catch
                {
                    count8 = 0;
                }
                List<String> nameLt8 = new List<String>();
                for (int i = 1; i <= count8; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Manufacturing/Spinner/pending/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt8.Add(nameNw);
                }
                int cnt8 = nameLt8.Count;
                orderList = new string[cnt8];
                orderList = nameLt8.ToArray();

                if (nameLt8.Count == count8)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    rc.Visibility = Visibility.Visible;
                }


            }
            catch
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
                var Suggestion = sID.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void autoStripSize(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = productList1.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void autoCoil(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
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
                var Suggestion = prodName.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void autoEmployee(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = empList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private void autoMachine(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = machineList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void autoFrom(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = process.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void autoTo(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = process.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void autoOrder(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = orderList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void autoTool(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = toolList.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }
        private void pQuery(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            sender.Text = args.QueryText;
            string cName = pName.Text;
            Row2.Visibility = Visibility.Collapsed;
            progress1.Visibility = Visibility.Visible;
            updateMaster(cName);
        }
        private async void orderQuery(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            rc.Visibility = Visibility.Collapsed;
            progress1.Visibility = Visibility.Visible;
            sender.Text = args.QueryText;
            string cName = fOrderNo.Text;
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/");
            routeCard ab = new routeCard();
            ab = response.ResultAs<routeCard>();
            fSalesNo.Text = ab.salesid;
            fStripSize.Text = ab.stripsize;
            fDate.Text = ab.date;
            fTime.Text = ab.time;
            fQuantityStrip.Text = ab.quantityS.ToString();
            fCoilNo.Text = ab.coilno;
            fBatchNo.Text = ab.batchNo;

            FirebaseResponse response5 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/employeeLayout/count/");
            int count5;
            try
            {
                count5 = response5.ResultAs<int>();
            }
            catch
            {
                count5 = 0;
            }
            for (int i = 1; i <= count5; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/employeeLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                endList1.Items.Add(list1);
                lEmployee.Items.Add(list1);
            }

            FirebaseResponse response6 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/machineLayout/count/");
            int count6;
            try
            {
                count6 = response6.ResultAs<int>();
            }
            catch (Exception)
            {
                count6 = 0;
            }
            for (int i = 1; i <= count6; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/machineLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                endList2.Items.Add(list1);
                lMachine.Items.Add(list1);
            }

            FirebaseResponse response7 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/toolLayout/count/");
            int count7;
            try
            {
                count7 = response7.ResultAs<int>();
            }
            catch (Exception)
            {
                count7 = 0;
            }
            for (int i = 1; i <= count7; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/toolLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                endList3.Items.Add(list1);
                lTool.Items.Add(list1);
            }

            FirebaseResponse response8 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/count/");
            int count8;
            try
            {
                count8 = response8.ResultAs<int>();
            }
            catch (Exception)
            {
                count8 = 0;
            }
            for (int i = 1; i <= count8; i++)
            {
                FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/" + i);
                employeeList1 list1 = new employeeList1();
                list1 = response1.ResultAs<employeeList1>();
                endList4.Items.Add(list1);
            }
            rc.Visibility = Visibility.Visible;
            progress1.Visibility = Visibility.Collapsed;
        }
        private async void updateMaster(string cName)
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
            progress1.Visibility = Visibility.Collapsed;
            Row2.Visibility = Visibility.Visible;

        }

        private void ssubmit(object sender, RoutedEventArgs e)
        {
            endList4.Items.Clear();
            employeeList1 Name = new employeeList1();
            employeeList1 Name1 = new employeeList1();
            employeeList1 Name2 = new employeeList1();
            employeeList1 Name3 = new employeeList1();
            Name.Name = pName.Text;
            Name.SNo = 1;
            endList4.Items.Add(Name);
            Name1.Name = pFrom.Text;
            Name1.SNo = 2;
            endList4.Items.Add(Name1);
            Name2.Name = pTo.Text;
            Name2.SNo = 3;
            endList4.Items.Add(Name2);
            Name3.Name = pQuantity.Text;
            Name3.SNo = 4;
            endList4.Items.Add(Name3);
            Nav_Prod.IsOpen = false;
        }
        private void empAdd(object sender, RoutedEventArgs e)
        {
            Nav_Emp.IsOpen = true;
        }
        private void prodAdd(object sender, RoutedEventArgs e)
        {
            Nav_Prod.IsOpen = true;
        }
        private void machineAdd(object sender, RoutedEventArgs e)
        {
            Nav_Machine.IsOpen = true;
        }

        private void toolAdd(object sender, RoutedEventArgs e)
        {
            Nav_Tool.IsOpen = true;
        }

        private void eFinish(object sender, RoutedEventArgs e)
        {
            e_updateEnd();
            Nav_Emp.IsOpen = false;
        }

        public void e_updateEnd()
        {
            while (endList1.Items.Count > 0)
            {
                int ii = endList1.Items.Count - 1;
                endList1.Items.RemoveAt(ii);
                ii++;
            }
            foreach (employeeList1 s in lEmployee.Items)
            {
                endList1.Items.Add(s);
            }
        }
        public void e_changeSno()
        {
            ListView sample = new ListView();
            foreach (employeeList1 s in lEmployee.Items)
            {
                sample.Items.Add(s);
            }
            ListView temp = new ListView();
            int i = 1;
            foreach (employeeList1 s in sample.Items)
            {
                s.SNo = i;
                i++;
                temp.Items.Add(s);
            }
            while (lEmployee.Items.Count > 0)
            {
                int ii = lEmployee.Items.Count - 1;
                lEmployee.Items.RemoveAt(ii);
                ii++;
            }

            foreach (employeeList1 s in sample.Items)
            {
                lEmployee.Items.Add(s);
            }
        }

        private void eClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            lEmployee.Items.Remove(x);
            e_changeSno();
        }
        private void eSubmit(object sender, RoutedEventArgs e)
        {
            employeeList1 c1 = new employeeList1();
            c1.Name = pEmployeeList.Text;

            if (lEmployee.Items.Count > 0)
            {
                c1.SNo = lEmployee.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            eList1.Add(c1);
            lEmployee.Items.Add(c1);
        }



        private void mFinish(object sender, RoutedEventArgs e)
        {
            m_updateEnd();
            Nav_Machine.IsOpen = false;
        }

        public void m_updateEnd()
        {
            while (endList2.Items.Count > 0)
            {
                int ii = endList2.Items.Count - 1;
                endList2.Items.RemoveAt(ii);
                ii++;
            }
            foreach (employeeList1 s in lMachine.Items)
            {
                endList2.Items.Add(s);
            }
        }
        public void m_changeSno()
        {
            ListView sample = new ListView();
            foreach (employeeList1 s in lMachine.Items)
            {
                sample.Items.Add(s);
            }
            ListView temp = new ListView();
            int i = 1;
            foreach (employeeList1 s in sample.Items)
            {
                s.SNo = i;
                i++;
                temp.Items.Add(s);
            }
            while (lMachine.Items.Count > 0)
            {
                int ii = lMachine.Items.Count - 1;
                lMachine.Items.RemoveAt(ii);
                ii++;
            }

            foreach (employeeList1 s in sample.Items)
            {
                lMachine.Items.Add(s);
            }
        }

        private void mClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            lMachine.Items.Remove(x);
            m_changeSno();
        }



        private void mSubmit(object sender, RoutedEventArgs e)
        {
            employeeList1 c1 = new employeeList1();
            c1.Name = pMachineList.Text;

            if (lMachine.Items.Count > 0)
            {
                c1.SNo = lMachine.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            mList1.Add(c1);
            lMachine.Items.Add(c1);
        }



        private void tFinish(object sender, RoutedEventArgs e)
        {
            t_updateEnd();
            Nav_Tool.IsOpen = false;
        }

        public void t_updateEnd()
        {
            while (endList3.Items.Count > 0)
            {
                int ii = endList3.Items.Count - 1;
                endList3.Items.RemoveAt(ii);
                ii++;
            }
            foreach (employeeList1 s in lTool.Items)
            {
                endList3.Items.Add(s);
            }
        }
        public void t_changeSno()
        {
            ListView sample = new ListView();
            foreach (employeeList1 s in lTool.Items)
            {
                sample.Items.Add(s);
            }
            ListView temp = new ListView();
            int i = 1;
            foreach (employeeList1 s in sample.Items)
            {
                s.SNo = i;
                i++;
                temp.Items.Add(s);
            }
            while (lTool.Items.Count > 0)
            {
                int ii = lTool.Items.Count - 1;
                lTool.Items.RemoveAt(ii);
                ii++;
            }

            foreach (employeeList1 s in sample.Items)
            {
                lTool.Items.Add(s);
            }
        }

        private void tClick(object sender, ItemClickEventArgs e)
        {
            object x = e.ClickedItem;
            lTool.Items.Remove(x);
            t_changeSno();
        }
        private void tSubmit(object sender, RoutedEventArgs e)
        {
            employeeList1 c1 = new employeeList1();
            c1.Name = pToolList.Text;

            if (lTool.Items.Count > 0)
            {
                c1.SNo = lTool.Items.Count + 1;
            }
            else
                c1.SNo = 1;
            tList1.Add(c1);
            lTool.Items.Add(c1);
        }
        private async void submit(object sender, RoutedEventArgs e)
        {
            if (fOrderNo.Text.Length > 0 && fSalesNo.Text.Length > 0 && fQuantityStrip.Text.Length > 0 && fStripSize.Text.Length > 0 && fCoilNo.Text.Length > 0 && fBatchNo.Text.Length > 0 && fDate.Text.Length > 0 && fTime.Text.Length > 0)
            {
                try
                {
                    routeCard card = new routeCard();
                    card.workid = fOrderNo.Text;
                    card.salesid = fSalesNo.Text;
                    card.quantityS = Int32.Parse(fQuantityStrip.Text);
                    card.stripsize = fStripSize.Text;
                    card.coilno = fCoilNo.Text;
                    card.batchNo = fBatchNo.Text;
                    card.date = fDate.Text;
                    card.time = fTime.Text;

                    FirebaseResponse response = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/", card);

                    foreach (employeeList1 tempP in endList1.Items)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response12 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/employeeLayout/" + tempP.SNo + "/", tempP);
                        FirebaseResponse response13 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/employeeLayout/count/", tempP.SNo);
                    }

                    foreach (employeeList1 tempP in endList2.Items)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response12 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/machineLayout/" + tempP.SNo + "/", tempP);
                        FirebaseResponse response13 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/machineLayout/count/", tempP.SNo);
                    }

                    foreach (employeeList1 tempP in endList3.Items)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response12 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/toolLayout/" + tempP.SNo + "/", tempP);
                        FirebaseResponse response13 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/toolLayout/count/", tempP.SNo);
                    }

                    foreach (employeeList1 tempP in endList4.Items)
                    {
                        client = new FireSharp.FirebaseClient(config);
                        FirebaseResponse response12 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/processLayout/" + tempP.SNo + "/", tempP);
                        FirebaseResponse response13 = await client.SetAsync("Manufacturing/RouteCard/" + card.workid + "/processLayout/count/", tempP.SNo);
                    }

                    FirebaseResponse firebase = await client.SetAsync("Manufacturing/" + card.workid + "/StripsCount/", Int32.Parse(fQuantityStrip.Text));

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
