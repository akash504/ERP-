using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp;
using FireSharp.Response;
using FireSharp.EventStreaming;
using FireSharp.Extensions;
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
    public sealed partial class Employee_Profile : Page
    {
        public Employee_Profile()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += AcceleratorKeyActivated;
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };
        IFirebaseClient client2;
        List<ClientDetails> dataList = new List<ClientDetails>();
        private void AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        {
            Window.Current.CoreWindow.KeyDown += (s, e) =>
            {
                var ctrl = Window.Current.CoreWindow.GetKeyState(VirtualKey.Control);
                if (ctrl.HasFlag(CoreVirtualKeyStates.Down) && e.VirtualKey == VirtualKey.R)
                {
                    contentFrame.Navigate(typeof(BlankPage7));
                    Debug.WriteLine("ctrl+R has been triggered");
                }
                if (e.VirtualKey == VirtualKey.Enter)
                {
                    Debug.WriteLine("Enter triggered");
                }

            };
        }
        private async void Page_Loader(object sender, RoutedEventArgs e)
        {
            try
            {
                client2 = new FireSharp.FirebaseClient(config);
                FirebaseResponse response = await client2.GetAsync("Master/Spinner/Employee/count/");
                int c;
                try
                {
                    c = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    c = 0;
                }
                for (int i = 1; i <= c; i++)
                {
                    FirebaseResponse response1 = await client2.GetAsync("Master/Spinner/Employee/" + i + "/");
                    String a = response1.ResultAs<String>();
                    FirebaseResponse response2 = await client2.GetAsync("Master/Employee/" + a + "/");
                    mClient t = response2.ResultAs<mClient>();
                    ClientDetails cd = new ClientDetails();
                    cd.cname = t.Name;
                    cd.cphone = t.ContactNo;
                    cd.cmail = t.MailId;
                    cd.caddress = t.Address;
                    cd.sno = i.ToString();
                    MyList.Items.Add(cd);
                }
                if (MyList.Items.Count == c)
                {
                    progress1.Visibility = Visibility.Collapsed;
                    emp.Visibility = Visibility.Visible;

                }
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Nav_Pop.IsOpen = true;
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (fname.Text.Length > 0 && fcontact.Text.Length > 0 && fmail.Text.Length > 0 && faddress.Text.Length > 0)
            {
                try
                {
                    client2 = new FireSharp.FirebaseClient(config);
                    var data = new mClient
                    {
                        Name = fname.Text,
                        ContactNo = fcontact.Text,
                        MailId = fmail.Text,
                        Address = faddress.Text
                    };
                    FirebaseResponse firebaseResponse = await client2.GetAsync("Master/Spinner/Employee/count/");
                    int count;
                    try
                    {
                        count = firebaseResponse.ResultAs<int>();
                    }
                    catch (Exception)
                    {
                        count = 0;
                    }
                    count++;

                    String ab;
                    if (count < 10)
                    {
                        ab = "Wd0" + count.ToString();
                        SetResponse set = await client2.SetAsync("Master/Spinner/Employee/" + count + "/", ab);
                        SetResponse set1 = await client2.SetAsync("Master/Spinner/Employee/count/", count);
                        SetResponse response = await client2.SetAsync("Master/Employee/" + ab + "/", data);
                    }
                    else if (count > 10 && count < 100)
                    {
                        ab = "Wd" + count.ToString();
                        SetResponse set = await client2.SetAsync("Master/Spinner/Employee/" + count + "/", ab);
                        SetResponse set1 = await client2.SetAsync("Master/Spinner/Employee/count/", count);
                        SetResponse response = await client2.SetAsync("Master/Employee/" + ab + "/", data);
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
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            //var Auto = (AutoSuggestBox)sender;
            //Auto.ItemsSource = th
        }
    }
}