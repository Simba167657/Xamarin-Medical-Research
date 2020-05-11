using Acr.UserDialogs;
using GalaSoft.MvvmLight;
using Plugin.Badge;
using SJMC.Helpers;
using SJMC.Models;
using SJMC.Popup;
using SJMC.ServiceProviders;
using SJMC.Views;
using SJMC.Views.ContactUs;
using SJMC.Views.LaboratoryResults;
using SJMC.Views.News;
using SJMC.Views.Profile;
using SJMC.Views.RadiologyResults;
using SJMC.Views.Registration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public Action NavigatePageOnNofication;

        public string newversiondescription = "";
        public string newversion = "";

        #region CTOR
        public HomeViewModel()
        {
            ////  IsMenuListPresented = false;

            

        //  _menuItemList = new List<Menu>()
        //  {
        //    new Menu {Title = "Homepage", TargetType = typeof(ProfilePage)},
        //    new Menu {Title = "Laboratory results", TargetType = typeof(LabResultPage)},
        //    new Menu {Title = "Radiology results", TargetType = typeof(RadioResultPage)},
        //    new Menu {Title = "Contact Us", TargetType = typeof(ContactUsPage)},
        //    new Menu {Title = "News", TargetType = typeof(NewsPage)},
        //    new Menu {Title = "Locate Us", TargetType = typeof(LocationPage)},

        //    //new Menu {Title = "Billing and payment", TargetType = typeof(RegistrationPage)},
        // //   new Menu {Title = "Setting", TargetType = typeof(NewsDetailsPage)},
        //  };
        //  OnPropertyChanged("MenuItemList");

        //  MessagingCenter.Subscribe<string>(this, "IsMenuListPresented", async (sender) =>
        //  {
        //     // IsMenuListPresented = true;
        //  });


        RateCommand = new DelegateCommand(async (s) =>
            {
                RatingBarView ratingView = new RatingBarView();
                bool? dialogResult = await MaterialDialog.Instance.ShowCustomContentAsync(ratingView, "Rate");
                if (dialogResult != null && dialogResult.Value)
                {
                    UserDialogs.Instance.ShowLoading();
                    var result = await AuthServices.UpdateRating(Constants.UserProfile.Role, Constants.UserProfile.UserID, ratingView.Rating.ToString());
                    UserDialogs.Instance.HideLoading();

                    if (result.Success)
                    {
                        UserDialogs.Instance.Alert("Your rating is saved, Thank You!");
                    }
                    else
                    {
                        UserDialogs.Instance.Alert("Your rating is not saved, Try again later!");
                    }
                }
            });
            CloseCommand = new DelegateCommand((s) =>
            {
                App.HomeView.IsPresented = false;
            });
            GoogleCommand = new DelegateCommand((s) =>
            {
                Device.OpenUri(new Uri("https://business.google.com/u/1/dashboard/l/03066549321158524192"));
                App.HomeView.IsPresented = false;
            });
            FacebookCommand = new DelegateCommand((s) =>
            {
                Device.OpenUri(new Uri("https://www.facebook.com/Saint-Joseph-Medical-Center-603207859770837/?ref=bookmarks"));
                App.HomeView.IsPresented = false;
            });
            AboutCommand = new DelegateCommand(async (s) =>
            {
                await App.Current.MainPage.Navigation.PushAsync(new AboutThisAppPage());
                App.HomeView.IsPresented = false;
            });
            LogoutCommand = new DelegateCommand(async (s) =>
            {
                Settings.IsRemember = false;
                Settings.Email = String.Empty;
                Settings.Password = String.Empty;
                Settings.Role = String.Empty;
                Settings.UserID = String.Empty;
                Settings.ProfilePhoto = string.Empty;

                Settings.OpenReportNumber = 0;
                Settings.OpenReportPage = false;


                LogoutService service = new LogoutService();
                await service.LogoutTokenFromApp(Constants.UserProfile.LabID, Constants.UserProfile.Role);

                App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());

              

                //await this.Navigation.PushAsync(new Views.Login.LoginPage());
            });

        }

        public async void Initilize()
        {
           // MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Initilize Start");
            // Get Unseen Report Count
            var result = await ReportsService.GetLabReportsCount(Constants.UserProfile.Role, Constants.UserProfile.LabID);
            if (result.Success)
            {
                //MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Success");
                if (result.labCount > 0)
                {
                    //MessagingCenter.Send<string, string>("Hello", "DeviceLog", "First Iff");
                    //Settings.TotalLabortoryReportsCount = result.labCount;
                    Settings.TotalLabortoryReportsUnViewed = result.labCount;

                    MenuItemList[0].Count = Settings.TotalLabortoryReportsUnViewed;
                    MenuItemList[0].CountVisibility = true;
                }
                if (result.radioCount > 0)
                {
                   // MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Second Iff");
                    //Settings.TotalRadiologyReportsCount = result.labCount;
                    Settings.TotalRadiologyReportsUnViewed = result.radioCount;

                    MenuItemList[1].Count = Settings.TotalRadiologyReportsUnViewed;
                    MenuItemList[1].CountVisibility = true;
                }
               // MessagingCenter.Send<string, string>("Hello", "DeviceLog", "End If");
            }
           // MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Constant");
            if (Constants.IsAppOpenFirstTime)
            {
                //MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Here true");
                Constants.IsAppOpenFirstTime = false;
                UserDialogs.Instance.ShowLoading("Loading");

                AppLatestVersionResponseModel appLatestVersionResponseModel = new AppLatestVersionResponseModel();
                AppLatestVersion appLatestVersion = new AppLatestVersion();
                appLatestVersionResponseModel = await appLatestVersion.CheckAppVersion(Constants.GetDeviceType(), Constants.GetAppVersion());

                UserDialogs.Instance.HideLoading();
                //MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Hide Loading");
                if (appLatestVersionResponseModel.IsLatestAvailable)
                {
                    newversiondescription = appLatestVersionResponseModel.VersionDescription;
                    newversion = appLatestVersionResponseModel.NewVersion;
                    showStoppingAlert();

                }
                else
                {
                    //MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Notification");
                    //AppBadgeClearService service = new AppBadgeClearService();
                    //await service.ClearBadgeForApp(Constants.UserProfile.LabID, Constants.UserProfile.Role);
                    //CrossBadge.Current.SetBadge(-1);
                    NavigatePageOnNofication();
                }

            }
            else
            {
               // MessagingCenter.Send<string, string>("Hello", "DeviceLog", "Here false");
                //AppBadgeClearService service = new AppBadgeClearService();
                //await service.ClearBadgeForApp(Constants.UserProfile.LabID, Constants.UserProfile.Role);
                //CrossBadge.Current.SetBadge(-1);
                NavigatePageOnNofication();
            }


        }

        private string GetDeviceType()
        {
            if(Device.Android.Equals("Android"))
            {
                return "Android";
            }
            else {
                return "iOS";
            }
        }

        async private void showStoppingAlert()
        {
            var dialogResponse = await App.Current.MainPage.DisplayAlert(newversiondescription, newversion, null, "ok");

            if(dialogResponse || !dialogResponse)
            {
                showStoppingAlert();
            }
        }

        #endregion

        #region Set Properties
        private ObservableCollection<Menu> _menuItemList = new ObservableCollection<Menu>()
        {
              //new Menu {Title = "Homepage", TargetType = typeof(ProfilePage)},
              new Menu {Title = "Laboratory results", TargetType = typeof(LabResultPage)},
              new Menu {Title = "Radiology results", TargetType = typeof(RadioResultPage)},
              new Menu {Title = "Contact Us", TargetType = typeof(ContactUsPage)},
              new Menu {Title = "News", TargetType = typeof(NewsPage)},
              //new Menu {Title = "Logout", TargetType = typeof()},
              // new Menu {Title = "Locate Us", TargetType = typeof(LocationPage)},
        };
        public ObservableCollection<Menu> MenuItemList
        {
            get { return _menuItemList; }
            set
            {
                _menuItemList = value;
                OnPropertyChanged("MenuItemList");
            }
        }
        #endregion

        #region Commands

        public DelegateCommand RateCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand GoogleCommand { get; set; }
        public DelegateCommand FacebookCommand { get; set; }
        public DelegateCommand AboutCommand { get; set; }
        public DelegateCommand LogoutCommand { get; set; }

        internal void Navigate(Menu menu)
        {
            if (menu.TargetType == typeof(ProfilePage))
            {
                App.HomeView.Detail = new NavigationPage(new ProfilePage());
            }
            else if (menu.TargetType == typeof(LabResultPage))
            {
                App.Current.MainPage.Navigation.PushAsync(new Views.TabbedPage(0));
            }
            else if (menu.TargetType == typeof(RadioResultPage))
            {
                App.Current.MainPage.Navigation.PushAsync(new Views.TabbedPage(1));
            }
            else if (menu.TargetType == typeof(RegistrationPage))
            {
                App.HomeView.Detail = new NavigationPage(new RegistrationPage());
            }
            else if (menu.TargetType == typeof(ContactUsPage))
            {
                App.Current.MainPage.Navigation.PushAsync(new ContactUsPage());
            }
            else if (menu.TargetType == typeof(LocationPage))
            {
                App.Current.MainPage.Navigation.PushAsync(new LocationPage());
            }
            else if (menu.TargetType == typeof(NewsPage))
            {
                App.Current.MainPage.Navigation.PushAsync(new NewsPage());
            }

            App.HomeView.IsPresented = false;
        }


        #endregion
    }

    public class Menu : ViewModelBase
    {
        public string Title { get; set; }
        public Type TargetType { get; set; }

        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                RaisePropertyChanged();
            }
        }

        private bool _countVisibility;
        public bool CountVisibility
        {
            get => _countVisibility;
            set
            {
                _countVisibility = value;
                RaisePropertyChanged();
            }
        }
    }

}