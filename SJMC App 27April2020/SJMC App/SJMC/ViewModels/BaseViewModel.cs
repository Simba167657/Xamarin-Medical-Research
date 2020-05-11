using GalaSoft.MvvmLight.Command;
using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace SJMC.ViewModels
{
    public class BaseViewModel : BindableObject
    {
        private INavigation _Navigation;
        public INavigation Navigation
        {
            get { return _Navigation; }
            set
            {
                if (_Navigation != value)
                {
                    _Navigation = value;
                    OnPropertyChanged("Navigation");
                }
            }
        }

        private bool _pageVisibility;
        public bool PageVisibility
        {
            get { return _pageVisibility; }
            set
            {
                _pageVisibility = value;
                OnPropertyChanged("PageVisibility");
            }
        }

        private bool _labCountVisibility;
        public bool LabCountVisibility
        {
            get { return _labCountVisibility; }
            set
            {
                _labCountVisibility = value;
                OnPropertyChanged("LabCountVisibility");
            }
        }
        private bool _radioCountVisibility;
        public bool RadioCountVisibility
        {
            get { return _radioCountVisibility; }
            set
            {
                _radioCountVisibility = value;
                OnPropertyChanged("RadioCountVisibility");
            }
        }

        private int _radioCount;
        public int RadioCount
        {
            get { return _radioCount; }
            set
            {
                _radioCount = value;
                OnPropertyChanged("RadioCount");
            }
        }
        private int _labCount;
        public int LabCount
        {
            get { return _labCount; }
            set
            {
                _labCount = value;
                OnPropertyChanged("LabCount");
            }
        }


        public RelayCommand BackCommand => new RelayCommand(BackClick);

        private void BackClick()
        {
            App.Current.MainPage.Navigation.PopAsync();
        }

        public AuthServices AuthServices = new AuthServices();
        public ReportsService ReportsService = new ReportsService();
        public FirebaseTokenService FirebaseTokenService = new FirebaseTokenService();
        public NewsServices NewsServices = new NewsServices();

        public bool IsInitialized { get; set; }

        public BaseViewModel(INavigation navigation = null)
        {
            try
            {
                Navigation = navigation;
            }
            catch (Exception ex)
            { }
        }

        public async Task<int> ShowActionSheet(string title, List<string> actions, Color? backgroundColor = null)
        {
            var materialAlertDialog = new MaterialSimpleDialogConfiguration()
            {
                BackgroundColor = (backgroundColor == null) ? (Color)App.Current.Resources["ThemeBlueColor"] : backgroundColor.Value,
                TextColor = Color.White,
                TitleTextColor = Color.White,
                TintColor = Color.White,
                CornerRadius = 10
            };
            var result = await MaterialDialog.Instance.SelectActionAsync(title, actions, materialAlertDialog);
            return result;
        }

        public async Task PushModalAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushModalAsync(page);
        }

        public async Task PopModalAsync()
        {
            if (Navigation != null)
                await Navigation.PopModalAsync();
        }

        public async Task PushAsync(Page page)
        {
            if (Navigation != null)
                await Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            if (Navigation != null)
                await Navigation.PopAsync();
        }

        public async Task<byte[]> DownloadPDFAsync(string url)
        {
            try
            {

                //byte[] response = null;
                //await Task.Run(() =>
                //{
                //    response = new WebClient().DownloadData(new Uri(url));
                //});

                return new byte[1];
            }
            catch (Exception)
            {
                return new byte[1];
            }
        }
    }
}
