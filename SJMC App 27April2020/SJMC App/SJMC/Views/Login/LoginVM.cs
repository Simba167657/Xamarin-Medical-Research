using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.Interfaces;
using SJMC.ViewModels;
using SJMC.Views.News;
using SJMC.Views.SignUp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.Dialogs;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace SJMC.Views.Login
{
    public class LoginVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...

        public LoginVM(INavigation _Nav)
        {


            Navigation = _Nav;
            ForgetCommand = new Command(ForgetCommandAsync);
            SignUpCommand = new Command(SignUpCommandAsync);
            ShowCommand = new Command(ShowCommandAsync);
            LoginCommand = new Command(LoginCommandAsync);

            UserTypeRoleCommand = new Command(UserTypeRoleCommandAsync);
            RememberMeCommand = new Command(RememberMeCommandAsync);
            LatestNewsCommand = new Command(LatestNewsCommandAsync);

            LocateUsCommand = new DelegateCommand((s) =>
            {
                App.Current.MainPage.Navigation.PushAsync(new ContactUs.ContactUsPage());
            });
        }



        #region Delegate Command 
        public ICommand ForgetCommand { get; }
        public ICommand SignUpCommand { get; }
        public ICommand ShowCommand { get; }
        public ICommand LoginCommand { get; }

        public ICommand UserTypeRoleCommand { get; }
        public ICommand RememberMeCommand { get; }

        public DelegateCommand LocateUsCommand { get; set; }
        public ICommand LatestNewsCommand { get; }

        #endregion

        #region Properties

        public ObservableCollection<string> UserTypesList => new ObservableCollection<string>() { "patient", "doctor", "agent", "guarantor" };


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
        private string _selectedUserType;
        public string SelectedUserType
        {
            get { return _selectedUserType; }
            set
            {
                _selectedUserType = value;
                OnPropertyChanged("SelectedUserType");
            }
        }

        private string _Username;
        public string Username
        {
            get { return _Username; }
            set
            {
                if (_Username != value)
                {
                    _Username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                if (_Password != value)
                {
                    _Password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        private bool _IsPassword = true;
        public bool IsPassword
        {
            get { return _IsPassword; }
            set
            {
                if (_IsPassword != value)
                {
                    _IsPassword = value;
                    OnPropertyChanged("IsPassword");
                }
            }
        }

        public bool RememberChecked { get; set; } = false;

        #endregion

        #region Method


        private async void LatestNewsCommandAsync(object obj)
        {
            await PushAsync(new NewsPage());
        }


        private void RememberMeCommandAsync(object obj)
        {
            if (Settings.IsRemember)
            {
                RememberChecked = false;
                CheckSource = "checkbox.png";
            }
            else
            {
                RememberChecked = true;
                CheckSource = "check.png";
            }

        }

        private async void UserTypeRoleCommandAsync(object obj)
        {


            var result = await MaterialDialog.Instance.SelectActionAsync("Select Role", UserTypesList, configuration: new MaterialSimpleDialogConfiguration()
            {
                BackgroundColor = Color.FromHex("#33405D"),
                CornerRadius = 10,
                TextColor = Color.White,
                TitleTextColor = Color.Yellow,

            });

            if (result >= 0)
                SelectedUserType = UserTypesList[result];
        }
        private async void LoginCommandAsync(object obj)
        {
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Storage);
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Location);
            var loader = await MaterialDialog.Instance.LoadingDialogAsync("Signing In");

            Models.UserProfileModel result = new Models.UserProfileModel();

            if (Settings.IsRemember && Settings.Email != string.Empty && Settings.Password != string.Empty)
            {
                //result = await AuthServices.Login(Settings.Email, Settings.Password);
                result = new Models.UserProfileModel
                {
                    Email = Settings.Email,
                    LabID = Settings.LabId,
                    UserID = Settings.UserID,
                    UserPass = Settings.Password,
                    Role = Settings.Role,
                    Name = Settings.Name,
                    Phone = Settings.Phone
                };
                Constants.UserProfile = result;
                App.Current.MainPage = new NavigationPage(new HomeView());
            }
            else
            {
                if (String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password))
                {
                    await MaterialDialog.Instance.AlertAsync("Please fill both the fields to continue");
                    return;
                }
                result = await AuthServices.Login(Username, Password);
            }

            if (result.Success)
            {
                Constants.UserProfile = result;
                if (RememberChecked)
                {
                    Settings.Email = !String.IsNullOrEmpty(Constants.UserProfile.Email) ? Constants.UserProfile.Email : Constants.UserProfile.UserID;
                    Settings.Password = Constants.UserProfile.UserPass;
                    Settings.Role = result.Role;
                    Settings.LabId = result.LabID;
                    Settings.Name = result.Name;
                    Settings.Phone = result.Phone;
                    Settings.UserID = result.UserID;
                    Settings.IsRemember = true;
                }

                var tokenResult = await FirebaseTokenService.InsertFirebaseToken(result.LabID, result.Role, Settings.FirebaseToken, App.IsAndroid, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                if (tokenResult.Success)
                {
                    if (String.IsNullOrEmpty(Constants.UserProfile.Email))
                    {
                        //await PushAsync(new SignUpPage());
                        await this.Navigation.PushAsync(new SignUpPage(), false);
                        //App.Current.MainPage = new NavigationPage(new SignUpPage());
                    }
                    else
                    {
                        App.Current.MainPage = new NavigationPage(new HomeView());
                    }

                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(tokenResult.Message);
                }
            }
            else
            {
                await MaterialDialog.Instance.AlertAsync(result.Message);
            }
            await loader.DismissAsync();
        }

        /// <summary>
        /// TODO : To Define Forget Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void ForgetCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.PasswordReset.PasswordResetPage(), false);
        }

        /// <summary>
        /// TODO : To Define Password show Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void ShowCommandAsync(object obj)
        {
            if (IsPassword == false)
            {
                IsPassword = true;
            }
            else
            {
                IsPassword = false;
            }
        }

        /// <summary>
        /// TODO : To Define Sign Up Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void SignUpCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new SignUpPage(), false);
        }
        internal void Initilize()
        {
            PageVisibility = true;



            if (Settings.IsRemember)
            {
                Username = Settings.Email;
                Password = Settings.Password;
                SelectedUserType = Settings.Role;
                CheckSource = "check.png";

                //LoginCommandAsync(null);
            }
            else
            {
                Username = String.Empty;
                Password = String.Empty;
                SelectedUserType = UserTypesList[0];
                CheckSource = "checkbox.png";
                //Settings.IsRemember = true;
            }
            //#if DEBUG



            //            //doc
            //            Username = "GBDW";
            //            Password = "9";

            //            //pat
            //            Username = "m@m.com";
            //            Password = "9";

            //#endif


        }
        #endregion
    }
}
