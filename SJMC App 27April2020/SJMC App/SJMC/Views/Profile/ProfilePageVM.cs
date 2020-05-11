using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.ServiceProviders;
using SJMC.ViewModels;
using SJMC.Views.LaboratoryResults;
using SJMC.Views.RadiologyResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SJMC.Views.Profile
{
    public class ProfilePageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...
        private const string _emailRegex = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

        //TODO : To Define Constructor..
        public ProfilePageVM(INavigation _Nav)
        {


            Navigation = _Nav;
            if (Constants.UserProfile != null)
            {
                MobileVisibility = (Constants.UserProfile.Role == "patient");

                Mobile = Constants.UserProfile.Phone;
                Email = Constants.UserProfile.Email;
                FullName = Constants.UserProfile.Name;
            }
            // BackCommand = new DelegateCommand(BackCommandAsync);
            LogoutCommand = new Command(LogoutCommandAsync);

            UpdateCommand = new Command(UpdateCommandAsync);

            RADIOLOGYCommand = new Command(() =>
             {
                 App.Current.MainPage.Navigation.PushAsync(new TabbedPage(1));
                 //App.HomeView.Detail.Navigation.PushAsync(new RadioResultPage());
             });

            LABORATORYCommand = new Command(() =>
             {
                 App.Current.MainPage.Navigation.PushAsync(new TabbedPage(0));

                 // App.HomeView.Detail.Navigation.PushAsync(new LabResultPage());
             });

            BloodSampleCommand = new Command(() =>
            {
                PhoneDialer.Open(Settings.AppSettings_BloodSampleNumber);
            });
        }



        #region Delegate Command

        public ICommand LogoutCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand RADIOLOGYCommand { get; }
        public ICommand LABORATORYCommand { get; }

        public ICommand BloodSampleCommand { get; }


        #endregion

        #region Properties

        private ImageSource _ProfilePhoto;
        public ImageSource ProfilePhoto
        {
            get { return _ProfilePhoto; }
            set
            {
                _ProfilePhoto = value;
                OnPropertyChanged("ProfilePhoto");
            }
        }


        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged("FullName");
            }
        }

        private string _Email;
        public string Email
        {
            get { return _Email; }
            set
            {
                if (_Email != value)
                {
                    _Email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        private string _Mobile;
        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                if (_Mobile != value)
                {
                    _Mobile = value;
                    OnPropertyChanged("Mobile");
                }
            }
        }

        private bool _mobileVisibility;
        public bool MobileVisibility
        {
            get { return _mobileVisibility; }
            set
            {
                _mobileVisibility = value;
                OnPropertyChanged("MobileVisibility");
            }
        }

        private string _bloodSampleMessage = Settings.AppSettings_BloodSampleMessage;
        public string BloodSampleMessage
        {
            get { return _bloodSampleMessage; }
            set
            {
                _bloodSampleMessage = value;
                OnPropertyChanged("MobileVisibility");
            }
        }

        private string _bloodSampleNumber = Settings.AppSettings_BloodSampleNumber;
        public string BloodSampleNumber
        {
            get { return _bloodSampleNumber; }
            set
            {
                _bloodSampleNumber = value;
                OnPropertyChanged("MobileVisibility");
            }
        }

        #endregion

        #region Method
        internal async void Initilize()
        {
            //var url = String.Format(ApiBase.GetProfileKey, Constants.UserProfile.LabID);
            //var response = await AuthServices.Download(url);
            //if (response != null && response.Success && response.ImageData != null && response.ImageData.Length > 1)
            //{
            //    ProfilePhoto = ImageSource.FromStream(() =>
            //    {
            //        return new MemoryStream(response.ImageData);
            //    });
            //}
            //else
            //{
            //    ProfilePhoto = "dot.png";
            //}

            if (Settings.ProfilePhoto != string.Empty)
            {
                ProfilePhoto = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(Convert.FromBase64String(Settings.ProfilePhoto));
                });
            }
            else
            {
                ProfilePhoto = "dot.png";
            }

            //LabCountVisibility = RadioCountVisibility = false;
            //var result = await ReportsService.GetLabReportsCount(Constants.UserProfile.Role, Constants.UserProfile.LabID);
            //if (result.Success)
            //{
            //    if (result.labCount > 0)
            //    {
            //        LabCount = result.labCount;
            //        LabCountVisibility = true;
            //    }
            //    if (result.radioCount > 0)
            //    {
            //        RadioCount = result.radioCount;
            //        RadioCountVisibility = true;
            //    }
            //}

            // Get Unseen Report Count
            var result = await ReportsService.GetLabReportsCount(Constants.UserProfile.Role, Constants.UserProfile.LabID);
            if (result.Success)
            {
                if (result.labCount > 0)
                {
                    //Settings.TotalLabortoryReportsCount = result.labCount;
                    Settings.TotalLabortoryReportsUnViewed = result.labCount;

                    LabCount = Settings.TotalLabortoryReportsUnViewed;
                    LabCountVisibility = true;
                }
                if (result.radioCount > 0)
                {
                    //Settings.TotalRadiologyReportsCount = result.radioCount;
                    Settings.TotalRadiologyReportsUnViewed = result.radioCount;

                    RadioCount = Settings.TotalRadiologyReportsUnViewed;
                    RadioCountVisibility = true;
                }
            }

        }
        /// <summary>
        /// TODO : To Define Logout Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void LogoutCommandAsync(object obj)
        {
            Settings.IsRemember = false;
            Settings.Email = String.Empty;
            Settings.Password = String.Empty;
            Settings.Role = String.Empty;

            Settings.UserID = String.Empty;

            Settings.OpenReportPage = false;
            Settings.OpenReportNumber = 0;

            Settings.ProfilePhoto = string.Empty;

            //FirebaseMessaging.Instance.UnsubscribeFromTopic("news");


            LogoutService service = new LogoutService();
            await service.LogoutTokenFromApp(Constants.UserProfile.LabID, Constants.UserProfile.Role);

            await this.Navigation.PushAsync(new Views.Login.LoginPage());
        }
        private async void UpdateCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.SignUp.SignUpPage(true));
        }


        #endregion

        #region Validate
        /// <summary>
        /// To Validate all User Input Fields...
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Validate()
        {
            if (string.IsNullOrEmpty(Email))
            {
                UserDialogs.Instance.Alert("Please enter email.");
                return false;
            }
            bool isValid = (Regex.IsMatch(Email, _emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            if (!isValid)
            {
                UserDialogs.Instance.HideLoading();
                UserDialogs.Instance.Alert("Email is not valid.");
                return false;
            }
            return true;
        }

        #endregion
    }
}
