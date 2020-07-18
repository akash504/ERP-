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
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.System;
using System.Diagnostics;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class salary : Page
    {
        List<String> empName = new List<String>();
        string[] Autoitems;
        Employee emp = new Employee();
        public salary()
        {
            this.InitializeComponent();
        }

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };
        IFirebaseClient client;
        private async void e_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String loc = "Attendance/" + emp.eName;
                MessageDialog mg = new MessageDialog("Name: " + emp.eName +  "\n\n" + "Successful!");

                client = new FireSharp.FirebaseClient(config);
                var settr = await client.SetAsync(loc, emp);
                await mg.ShowAsync();

            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));

            }
        }
        private async void Page_Loader(object sender, RoutedEventArgs e)
        {

            try
            {
                client = new FireSharp.FirebaseClient(config);
                List<String> nameLt = new List<String>();
                FirebaseResponse response = await client.GetAsync("Spinner/Employee/count");
                int count;
                try
                {
                    count = response.ResultAs<int>();
                }
                catch (Exception)
                {
                    count = 0;
                }
                for (int i = 1; i <= count; i++)
                {
                    FirebaseResponse response1 = await client.GetAsync("Spinner/Employee/" + i);
                    string nameNw = response1.ResultAs<string>();
                    nameLt.Add(nameNw);

                }
                int cnt = nameLt.Count;
                Employee rt = new Employee();
                rt.eName = "";
                if (cnt > 0)
                {
                    progress1.IsActive = false;
                    progress1.Visibility = Visibility.Collapsed;
                    grid1.Visibility = Visibility.Visible;
                }
                Autoitems = new string[cnt];
                Autoitems = nameLt.ToArray();
            }
            catch (Exception)
            {
                MessageDialog error = new MessageDialog("Failed to connect to database!");
                this.Frame.Navigate(typeof(BlankPage6));
            }


        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var Auto = (AutoSuggestBox)sender;
                var Suggestion = Autoitems.Where(p => p.Contains(Auto.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                Auto.ItemsSource = Suggestion;
                emp.eName = sender.Text;

            }
            else
                emp.eName = sender.Text;

        }
        internal class Employee
        {
            public string  eName;
        }

    }
}
