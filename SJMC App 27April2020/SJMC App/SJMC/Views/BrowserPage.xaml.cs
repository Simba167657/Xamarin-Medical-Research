using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.ServiceProviders;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BrowserPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected BrowserDetailPageVM _BrowserDetailPageVM;

        public byte[] _data { get; set; }
        string _htmlData;

        public BrowserPage(byte[] data)
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            _BrowserDetailPageVM = new BrowserDetailPageVM(this.Navigation);
            this.BindingContext = _BrowserDetailPageVM;

            _data = data;
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            setWebSource();
        }

        private void setWebSource()
        {
            string htmlData = Encoding.UTF8.GetString(_data);

            _htmlData = htmlData;

            if (Device.RuntimePlatform == Device.iOS)
            {
                string resourceRelative = "ReplogoR.jpg";
                htmlData = htmlData.Replace("replogoR.jpg", resourceRelative);

                resourceRelative = "RepLogoL.jpg";
                htmlData = htmlData.Replace("replogoL.jpg", resourceRelative);

                resourceRelative = "replogo.jpg";
                htmlData = htmlData.Replace("replogo.jpg", resourceRelative);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                string resourceRelative = "file:///android_asset/ReplogoR.jpg";
                htmlData = htmlData.Replace("replogoR.jpg", resourceRelative);

                resourceRelative = "file:///android_asset/RepLogoL.jpg";
                htmlData = htmlData.Replace("replogoL.jpg", resourceRelative);

                resourceRelative = "file:///android_asset/replogo.jpg";
                htmlData = htmlData.Replace("replogo.jpg", resourceRelative);
            }

            //htmlData = htmlData.Replace("replogoR.jpg", resourceRelative.ToString());
            //htmlData = htmlData.Replace("https://sjmc.stjosephlab-results.com/ReportImages/ReportLogo.png", resourceRelative.ToString());

            var html = new HtmlWebViewSource
            {
                Html = htmlData
            };

            webViewReport.Source = html;
        }

        async void stkWhatsApp_Attachment_Tapped(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            PdfAttachmentService service = new PdfAttachmentService();
            byte[] data1 = await service.PostFileUploadAndGetPDF(_htmlData);

            if(data1!=null)
            {
                var fn = Settings.Name.ToUpperInvariant() + "-Report.pdf";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(file, data1);



                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    PresentationSourceBounds = DeviceInfo.Platform == DevicePlatform.iOS && DeviceInfo.Idiom == DeviceIdiom.Tablet
                            ? new System.Drawing.Rectangle(0, 20, 0, 0)
                            : System.Drawing.Rectangle.Empty,
                    File = new ShareFile(file)
                });

               
            }
            else
            {
                UserDialogs.Instance.HideLoading();
                ShowErrorMessageDialog();
            }
        }

        async void stkEmail_Attachment_Tapped(System.Object sender, System.EventArgs e)
        {
            UserDialogs.Instance.ShowLoading();

            PdfAttachmentService service = new PdfAttachmentService();
            byte[] data1 = await service.PostFileUploadAndGetPDF(_htmlData);

            if(data1 != null)
            {
                var fn = Settings.Name + "-Report.pdf";
                var file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllBytes(file, data1);

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
            else
            {
                UserDialogs.Instance.HideLoading();
                ShowErrorMessageDialog();
            }

        }

        async private void ShowErrorMessageDialog()
        {
            var dialogResponse = await App.Current.MainPage.DisplayAlert("Error", "Error encountered while converting report to pdf", null, "ok");
        }


    }
}