using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using static weldone.QC;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Finished_Goods : Page
    {
        public Finished_Goods()
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
                    contentFrame.Navigate(typeof(Finished_Goods));
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
                FirebaseResponse response = await client.GetAsync("DispatchQC/Spinner/Verified/count/");
                int c;
                try
                {
                    c = response.ResultAs<int>();
                }
                catch (Exception)
                { c = 0; }


                for (int i = 1; i <= c; i++)
                {
                    FinishedGoods finished = new FinishedGoods();
                    FirebaseResponse response1 = await client.GetAsync("DispatchQC/Spinner/Verified/" + i + "/");
                    string ab = response1.ResultAs<string>();
                    FirebaseResponse response2 = await client.GetAsync("DispatchQC/" + ab + "/");
                    dispatchQC a = new dispatchQC();
                    a = response2.ResultAs<dispatchQC>();
                    finished.sno = MyList.Items.Count + 1;
                    finished.workid = a.workordernumber;
                    finished.salesid = a.salesordernumber;
                    finished.productname = a.productname;
                    finished.eoutput = a.equantity;
                    finished.produced = a.pquantity;
                    finished.date = a.date;
                    finished.time = a.time;
                    MyList.Items.Add(finished);
                }
                if(MyList.Items.Count  > 0)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    fg.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }

        }

    }
    internal class FinishedGoods
    {
        public int sno { get; set; }
        public string workid { get; set; }
        public string salesid { get; set; }
        public string productname { get; set; }
        public int eoutput { get; set; }
        public int produced { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}
