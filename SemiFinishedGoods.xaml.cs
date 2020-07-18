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

namespace weldone
{
    public sealed partial class SemiFinishedGoods : Page
    {
        string[] Autoitems, process;
        public SemiFinishedGoods()
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
                    contentFrame.Navigate(typeof(SemiFinishedGoods));
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
                if (Autoitems.Length == cnt)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    sfg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
        private void tName(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
            }
        }

        private async void qName(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            client = new FireSharp.FirebaseClient(config);
            progress1.Visibility = Visibility.Visible;
            sfg.Visibility = Visibility.Collapsed;
            sender.Text = args.QueryText;
            string cName = fName.Text;

            FirebaseResponse response1 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/1/Name/");
            fProduct.Text = response1.ResultAs<string>();
            string a12 = fProduct.Text;
            FirebaseResponse response2 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/2/Name/");
            string ab = response2.ResultAs<string>() + " to ";
            FirebaseResponse response3 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/3/Name/");
            fCycle.Text = ab + response3.ResultAs<string>();
            FirebaseResponse response4 = await client.GetAsync("Manufacturing/RouteCard/" + cName + "/processLayout/4/Name/");
            fEQuantity.Text = response4.ResultAs<string>();
            FirebaseResponse response5 = await client.GetAsync("Manufacturing/" + cName + "/StripsCount/");
            fStrips.Text = response5.ResultAs<int>().ToString();
            FirebaseResponse response6 = await client.GetAsync("Products/" + a12 + "/Process/count/");
            int count2 = response6.ResultAs<int>();
            for (int i = 1; i <= count2; i++)
            {
                FirebaseResponse response7 = await client.GetAsync("Products/" + a12 + "/Process/" + i);
                string nameNw = response7.ResultAs<string>();

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
                sfg ab2 = new sfg();
                ab2.Name = nameNw;
                ab2.Quantity = a;
                endList.Items.Add(ab2);
                if (nameNw == response3.ResultAs<string>())
                {
                    progress1.Visibility = Visibility.Collapsed;
                    sfg.Visibility = Visibility.Visible;
                    break;
                }

            }
        }
    }
    internal class sfg
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
