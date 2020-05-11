using Acr.UserDialogs;
using Rg.Plugins.Popup.Extensions;
using SJMC.Popup;
using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SJMC.Views.PasswordReset
{
    public class PasswordResetPageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...
        private const string _emailRegex = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
        public PasswordPopupPage passwordPopup;

        //TODO : To Define Constructor..
        public PasswordResetPageVM(INavigation _Nav)
        {
            Navigation = _Nav;
            BackCommand = new DelegateCommand(BackCommandAsync);
          //  ResendCommand = new DelegateCommand(ResendCommandAsync);
            ConfirmCommand = new DelegateCommand(ConfirmCommandAsync);
        }

        #region Delegate Command

        public DelegateCommand BackCommand { get; }
        public DelegateCommand ResendCommand { get; }
        public DelegateCommand ConfirmCommand { get; }

        #endregion

        #region Properties

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

        #endregion

        #region Method

        /// <summary>
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void BackCommandAsync(object obj)
        {
            await this.Navigation.PopAsync();
        }

        /// <summary>
        /// TODO : To Define Confirm Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void ConfirmCommandAsync(object obj)
        {
            passwordPopup = new PasswordPopupPage(this);
            await Navigation.PushPopupAsync(passwordPopup, true);

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
