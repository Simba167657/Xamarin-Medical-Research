using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.ServiceProviders;
using SJMC.ViewModels;
using SJMC.Views.Profile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views.SignUp
{
    public class SignUpPageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...
        private const string _emailRegex = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

        //TODO : To Define Constructor..
        public SignUpPageVM(INavigation _Nav)
        {
            Navigation = _Nav;
            BackCommand = new Command(BackCommandAsync);
            UpdateCommand = new Command(UpdateommandAsync);
            LoginCommand = new Command(LoginCommandAsync);
            CheckCommand = new Command(CheckCommandAsync);
            ProfilePhotoCommand = new Command(ProfilePhotoCommandAsync);
            BloodSampleCommand = new Command(BloodSampleCommandAsync);

        }

        private void BloodSampleCommandAsync(object obj)
        {
            PhoneDialer.Open(Settings.AppSettings_BloodSampleNumber);
        }

        internal async void Initilize(bool allowBack)
        {
            //var loader = await MaterialDialog.Instance.LoadingDialogAsync(,"Loading");

            //App.Current.MainPage.

            UserDialogs.Instance.ShowLoading();

            try
            {

                //  var url = $"{new HttpClientBase().BaseUrl}/Images/{Constants.UserProfile.LabID}.png";
                //  var bytes = await DownloadPDFAsync(url);
                if (Settings.ProfilePhoto != string.Empty)
                {
                    ProfilePhoto = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Settings.ProfilePhoto)));
                }
                else
                {
                    ProfilePhoto = "dot.png";
                }

                //20191204111937.png  {Constants.UserProfile.LabID}
                var url = String.Format(ApiBase.GetProfileKey, Constants.UserProfile.LabID);

                var response = await AuthServices.Download(url);

                Fullname = Constants.UserProfile.Name;
                Email = Constants.UserProfile.Email;

                if (!String.IsNullOrEmpty(Constants.UserProfile.Phone))
                {
                    CountryCode = Constants.UserProfile.Phone.Substring(0, 2);
                    PhoneNumber = Constants.UserProfile.Phone.Remove(0, 2);
                }

                ReceiveNotification = (Constants.UserProfile.Notify == null ? true : Constants.UserProfile.Notify.Value);

                PhoneVisibility = (Constants.UserProfile.Role == "patient");

                AllowBack = allowBack;
                CheckSource = "check.png";
            }
            catch (Exception ex)
            {

            }
            finally
            {
                UserDialogs.Instance.HideLoading();

                //await loader.DismissAsync();
            }
        }

        #region Delegate Command

        public ICommand BackCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand LoginCommand { get; }

        public ICommand CheckCommand { get; }
        public ICommand ProfilePhotoCommand { get; set; }

        public ICommand BloodSampleCommand { get; }

        #endregion

        #region Properties

        public bool ChangePassword { get; set; }

        private string _checkSource;
        public string CheckSource
        {
            get { return _checkSource; }
            set
            {
                _checkSource = value;
                OnPropertyChanged("CheckSource");
            }
        }

        private bool _phoneVisibility;
        public bool PhoneVisibility
        {
            get { return _phoneVisibility; }
            set
            {
                _phoneVisibility = value;
                OnPropertyChanged("PhoneVisibility");
            }
        }

        private bool _allowBack;
        public bool AllowBack
        {
            get { return _allowBack; }
            set
            {
                _allowBack = value;
                OnPropertyChanged("AllowBack");
            }
        }

        private string _Fullname;
        public string Fullname
        {
            get { return _Fullname; }
            set
            {
                if (_Fullname != value)
                {
                    _Fullname = value;
                    OnPropertyChanged("Fullname");
                }
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


        private string _countryCode;
        public string CountryCode
        {
            get { return _countryCode; }
            set
            {
                if (_countryCode != value)
                {
                    _countryCode = value;
                    OnPropertyChanged("CountryCode");
                }
            }
        }

        private string _PhoneNumber;
        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set
            {
                if (_PhoneNumber != value)
                {
                    _PhoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        private string _OldPassword;
        public string OldPassword
        {
            get { return _OldPassword; }
            set
            {
                if (_OldPassword != value)
                {
                    _OldPassword = value;
                    OnPropertyChanged("OldPassword");
                }
            }
        }

        private string _NewPassword;
        public string NewPassword
        {
            get { return _NewPassword; }
            set
            {
                if (_NewPassword != value)
                {
                    _NewPassword = value;
                    OnPropertyChanged("NewPassword");
                }
            }
        }

        private string _ConfirmPassword;
        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set
            {
                if (_ConfirmPassword != value)
                {
                    _ConfirmPassword = value;
                    OnPropertyChanged("ConfirmPassword");
                }
            }
        }
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

        private bool _receiveNotification;
        public bool ReceiveNotification
        {
            get { return _receiveNotification; }
            set
            {
                _receiveNotification = value;
                OnPropertyChanged("ReceiveNotification");
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


        private async void ProfilePhotoCommandAsync(object obj)
        {
            try
            {
                var result = await new MediaService().SelectSource();
                if (result == null)
                {
                    return;
                }

                var loader = await MaterialDialog.Instance.LoadingDialogAsync("Loading");

                //var fileResult = await AuthServices.PostFileUpload(Constants.UserProfile.LabID, result.Item3);
                //if (!fileResult.Success)
                //{
                //    await MaterialDialog.Instance.AlertAsync(fileResult.Message);
                //}
                //else
                //{
                //    ProfilePhoto = ImageSource.FromStream(() => new MemoryStream(result.Item3));

                //    //var url = $"{new HttpClientBase().BaseUrl}/Images/{Constants.UserProfile.LabID}.png";
                //    //ProfilePhoto = url;
                //}

                Settings.ProfilePhoto = Convert.ToBase64String(result.Item3);
                ProfilePhoto = ImageSource.FromStream(() => new MemoryStream(result.Item3));

                await loader.DismissAsync();
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync("Something went wrong.");
            }

        }
        private async void CheckCommandAsync(object obj)
        {

        }

        /// <summary>
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void BackCommandAsync(object obj)
        {
            await this.Navigation.PopAsync();

        }

        /// <summary>
        /// TODO : To Define Sign Up Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void UpdateommandAsync(object obj)
        {
            try
            {
                if (CheckSource != "check.png")
                {
                    await MaterialDialog.Instance.AlertAsync("Please accept our terms and conditions");
                    return;
                }

                if (String.IsNullOrEmpty(Fullname))
                {
                    await MaterialDialog.Instance.AlertAsync("Please enter your full name");
                    return;
                }

                if (String.IsNullOrEmpty(Email))
                {
                    await MaterialDialog.Instance.AlertAsync("Please enter your Email");
                    return;
                }

                bool isValid = (Regex.IsMatch(Email, _emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                if (!isValid)
                {
                    await MaterialDialog.Instance.AlertAsync("Email format is not correct");
                    return;
                }

                if (PhoneVisibility)
                {
                    if (String.IsNullOrEmpty(PhoneNumber) || String.IsNullOrEmpty(CountryCode))
                    {
                        await MaterialDialog.Instance.AlertAsync("Please enter your Phone");
                        return;
                    }

                    // string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                    var isValidPhoneNumber = (PhoneNumber.Length == 6) && (CountryCode.Length == 2);//   Regex.IsMatch(PhoneNumber, MatchPhoneNumberPattern);

                    if (!isValidPhoneNumber)
                    {
                        await MaterialDialog.Instance.AlertAsync("Phone number format is not correct");
                        return;
                    }
                }




                if (!String.IsNullOrEmpty(OldPassword) || !String.IsNullOrEmpty(NewPassword) || !String.IsNullOrEmpty(ConfirmPassword))
                {
                    if (OldPassword != Constants.UserProfile.UserPass)
                    {
                        ChangePassword = false;
                        await MaterialDialog.Instance.AlertAsync("Your old password is not correct");
                        return;
                    }
                    if (NewPassword != ConfirmPassword)
                    {
                        ChangePassword = false;

                        await MaterialDialog.Instance.AlertAsync("Your new password and confirm password don't match");
                        return;
                    }
                    ChangePassword = true;

                }
                else
                {
                    ChangePassword = false;

                }


                var loader = await MaterialDialog.Instance.LoadingDialogAsync("Updating Profile");
                // PhoneNumber = CountryCode + PhoneNumber;

                var result = await AuthServices.UpdateProfile(Constants.UserProfile.Role, Constants.UserProfile.UserID, Fullname, Email, CountryCode + PhoneNumber, ChangePassword ? NewPassword : Constants.UserProfile.UserPass, ReceiveNotification);

                await loader.DismissAsync();

                if (result.Success)
                {
                    Constants.UserProfile.Name = Fullname;
                    Constants.UserProfile.Email = Email;
                    Constants.UserProfile.Phone = CountryCode + PhoneNumber;
                    Constants.UserProfile.Notify = ReceiveNotification;
                    if (ChangePassword)
                    {
                        Constants.UserProfile.UserPass = NewPassword;

                    }


                    Settings.Email = Constants.UserProfile.Email;
                    Settings.Password = Constants.UserProfile.UserPass;

                    await MaterialDialog.Instance.AlertAsync(result.Message);

                    App.Current.MainPage = new NavigationPage(new HomeView());
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync(result.Message);
                }

                ////  await this.Navigation.PushAsync(new ProfilePage(), false);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// TODO : To Define Sign Up Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void LoginCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.Login.LoginPage(), false);
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
