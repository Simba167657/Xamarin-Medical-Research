using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.Interfaces;
using SJMC.Models;
using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfPage : ContentPage
    {
        private static bool? _isHtmlSet;
        public static bool? IsHtmlSet
        {
            get => _isHtmlSet;
            set
            {
                _isHtmlSet = value;
            }
        }

        public bool IsLoaded { get; set; }
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        ReportsModel _model;
        byte[] _bytes;

        public PdfPage(ReportsModel model)
        {


            InitializeComponent();
            _model = model;
            NavigationPage.SetHasNavigationBar(this, false);

        }
        public PdfPage(byte[] bytes)
        {


            InitializeComponent();
            _bytes = bytes;
            NavigationPage.SetHasNavigationBar(this, false);
            if (!IsLoaded)
            {
                if (_bytes != null)
                {
                    pdfViewerControl.InputFileStream = new MemoryStream(_bytes);
                    pdfViewerControl.ViewMode = Syncfusion.SfPdfViewer.XForms.ViewMode.FitWidth;
                }
                IsLoaded = true;
            }
            IsLoaded = false;
        }



        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //if (!IsLoaded)
            //{

            //    if (_bytes != null)
            //    {
            //        pdfViewerControl.InputFileStream = new MemoryStream(_bytes);
            //        pdfViewerControl.ViewMode = Syncfusion.SfPdfViewer.XForms.ViewMode.FitWidth;
            //     //   var result2 = await new ReportsService().SetSeenReports(Constants.UserProfile.Role, _model.ASBRCH, _model.ASYEAR, _model.ASREF, Constants.UserProfile.LabID, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));

            //    }
            //    //else
            //    //{
            //    //    var loader = await MaterialDialog.Instance.LoadingDialogAsync("");

            //    //    byte[] result = await new ReportsService().DownloadReport(_model.ASREF);
            //    //    await loader.DismissAsync();

            //    //    if (result == null)
            //    //    {
            //    //        await MaterialDialog.Instance.AlertAsync("Pdf could not be loaded");
            //    //        return;
            //    //    }


            //    //    var name = DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";
            //    //    DependencyService.Get<INativeFileHandler>().SaveFile(name, result);

            //    //    var newBytes = DependencyService.Get<INativeFileHandler>().GetFile(name);
            //    //    pdfViewerControl.InputFileStream = new MemoryStream(newBytes);
            //    //    pdfViewerControl.ViewMode = Syncfusion.SfPdfViewer.XForms.ViewMode.FitWidth;
            //    //   // var result1 = await new ReportsService().SetSeenReports(Constants.UserProfile.Role, _model.ASBRCH, _model.ASYEAR, _model.ASREF, Constants.UserProfile.LabID, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
            //    //}

            //    IsLoaded = true;
            //}
            //IsLoaded = false;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //var stream = pdfViewerControl.SaveDocument();
            //var bytess = ReadFully(stream);
            //DependencyService.Get<INativeFileHandler>().SaveFile("xyz.pdf", bytess);

            App.Current.MainPage.Navigation.PopAsync();
        }

        async void stkWhatsApp_Attachment_Tapped(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            try
            {
                var fn = Settings.Name.ToUpperInvariant() + "-Report.pdf";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(file, _bytes);

                UserDialogs.Instance.HideLoading();

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty,
                    File = new ShareFile(file)
                });
            }
            catch(Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                ShowErrorMessageDialog();
            }

            
        }

        async void stkEmail_Attachment_Tapped(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            var fn = Settings.Name + "-Report.pdf";
            var file = Path.Combine(FileSystem.CacheDirectory, fn);
            File.WriteAllBytes(file, _bytes);

            UserDialogs.Instance.HideLoading();

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = Title,
                PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty,
                File = new ShareFile(file)
            });
        }

        async private void ShowErrorMessageDialog()
        {
            var dialogResponse = await App.Current.MainPage.DisplayAlert("Error", "Error encountered while converting report to pdf", null, "ok");
        }
    }
}