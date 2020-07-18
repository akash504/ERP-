using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class excel1 : Page
    {
        public excel1()
        {
            this.InitializeComponent();
        }
        private async void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            //Create an instance of ExcelEngine.
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                excelEngine.Excel.DefaultVersion = ExcelVersion.Xlsx;
                IWorkbook workbook = excelEngine.Excel.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
               
                worksheet.Range["A1"].Text = "Hello World";
                worksheet.Range["A2"].Text = "Yeah sure";
                worksheet.Range["A1"].RowHeight = 100;
                worksheet.Range["A1"].ColumnWidth = 200;
                
                StorageFile storageFile;
                if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
                {
                    FileSavePicker savePicker = new FileSavePicker();
                    savePicker.SuggestedStartLocation = PickerLocationId.Desktop;
                    savePicker.SuggestedFileName = "Sample";
                    savePicker.FileTypeChoices.Add("Excel Files", new List<string>() { ".xlsx" });
                    storageFile = await savePicker.PickSaveFileAsync();
                }
                else
                {
                    StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                    storageFile = await local.CreateFileAsync("Sample.xlsx", CreationCollisionOption.ReplaceExisting);
                }
                //Saving the workbook
                await workbook.SaveAsAsync(storageFile);

                // Launch the saved file
                await Windows.System.Launcher.LaunchFileAsync(storageFile);
            }
        }
    }
}
