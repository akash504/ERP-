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
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scrub : Page
    {
        public Scrub()
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
                    contentFrame.Navigate(typeof(Scrub));
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
                DateTime sysdt;
                sysdt = DateTime.Now;
                string Date = sysdt.ToString("dd/MM/yyyy");
                fDate.Text = Date;
                string Time1 = sysdt.ToString("hh:mm tt");
                fTime.Text = Time1;
                progress1.Visibility = Visibility.Collapsed;
                grid1.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }
        private async void bt(object sender, RoutedEventArgs e)
        {

            if (fWeight.Text.Length > 0 && fAmount.Text.Length > 0 && fVendor.Text.Length > 0 && fDate.Text.Length > 0 && fTime.Text.Length > 0)
            {
                try
                {
                    client = new FireSharp.FirebaseClient(config);



                    scrap scrap1 = new scrap();
                    scrap1.weight = double.Parse(fWeight.Text);
                    scrap1.amount = Int32.Parse(fAmount.Text);
                    scrap1.vendor = fVendor.Text;
                    scrap1.date = fDate.Text;
                    scrap1.time = fTime.Text;
                    FirebaseResponse response = await client.SetAsync("Scrap/" + scrap1.date + "/", scrap1);
                    FirebaseResponse response1 = await client.GetAsync("Scrap/date/count/");
                    int c;
                    try
                    {
                        c = response1.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        c = 0;
                    }
                    c++;
                    FirebaseResponse response2 = await client.SetAsync("Scrap/date/" + c + "/", scrap1.date);
                    FirebaseResponse response3 = await client.SetAsync("Scrap/date/count/", c);
                    MessageDialog mg = new MessageDialog("Successfully Marked In ERP");
                    await mg.ShowAsync();
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
        internal class scrap
        {
            public double weight { get; set; }
            public int amount { get; set; }
            public string vendor { get; set; }
            public string date { get; set; }
            public string time { get; set; }
        }
    }
}
