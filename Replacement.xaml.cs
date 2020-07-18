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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace weldone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Replacement : Page
    {
        string[] Autoitems, supplierList, empList, productList;
        public Replacement()
        {
            this.InitializeComponent();
        }
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };
        IFirebaseClient client;
        private async void Page_Loader(object sender, RoutedEventArgs e)
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
                if (nameNw == cName)
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
            for (int i = 1; i <= a; i++)
            {
                FirebaseResponse response2 = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/itemLayout/" + i + "/pName/");
                string nameNw = response2.ResultAs<string>();
                nameLt7.Add(nameNw);
            }
            int cnt7 = nameLt7.Count;
            productList = new string[cnt7];
            productList = nameLt7.ToArray();
            FirebaseResponse response = await client.GetAsync("Purchase/PurchaseOrder/" + cName + "/supplierName/");
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
    }
}
