using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using FireSharp.Interfaces;
using Windows.Data.Json;
using FireSharp.Config;
using FireSharp.Response;

namespace weldone
{
    internal class WorkOrderNo
    {
        public DateTime sysdt;
        public string date, month, year, mnthL, PurchaseID, SL;
        public int stat = 0;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "Fld3TLREfZ47bmggol008nKkHzDAMphTdhlqi09d",
            BasePath = "https://weldonetechnocrats.firebaseio.com/"

        };

        public WorkOrderNo()
        {
            DateTime sysdt;
            sysdt = DateTime.Now;
            date = sysdt.ToString("dd", CultureInfo.InvariantCulture);
            month = sysdt.ToString("MM", CultureInfo.InvariantCulture);
            year = sysdt.ToString("yy", CultureInfo.InvariantCulture);
            int uni = Int32.Parse(month);

            switch (uni)
            {
                case 1:
                    mnthL = "A";
                    break;
                case 2:
                    mnthL = "B";
                    break;
                case 3:
                    mnthL = "C";
                    break;
                case 4:
                    mnthL = "D";
                    break;
                case 5:
                    mnthL = "E";
                    break;
                case 6:
                    mnthL = "F";
                    break;
                case 7:
                    mnthL = "G";
                    break;
                case 8:
                    mnthL = "H";
                    break;
                case 9:
                    mnthL = "I";
                    break;
                case 10:
                    mnthL = "J";
                    break;
                case 11:
                    mnthL = "K";
                    break;
                case 12:
                    mnthL = "L";
                    break;
            }
            PurchaseID = "W" + date + mnthL + year;
            SL = PurchaseID;

        }
    }

}