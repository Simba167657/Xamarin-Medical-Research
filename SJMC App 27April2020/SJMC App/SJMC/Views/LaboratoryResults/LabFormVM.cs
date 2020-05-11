using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SJMC.Views.LaboratoryResults
{
    public class LabFormVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...
        private const string _emailRegex = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";

        //TODO : To Define Constructor..
        public LabFormVM(INavigation _Nav)
        {
            Navigation = _Nav;
             BackCommand = new DelegateCommand(BackCommandAsync);
             SendCommand = new DelegateCommand(SendCommandAsync);
            Fullname = Constants.UserProfile.Name;
            Email = Constants.UserProfile.Email;
        
        }

        #region Delegate Command
       
        public DelegateCommand BackCommand { get; }
        public DelegateCommand SendCommand { get; }

        #endregion

        #region Properties

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

        private string _Subject;
        public string Subject
        {
            get { return _Subject; }
            set
            {
                if (_Subject != value)
                {
                    _Subject = value;
                    OnPropertyChanged("Subject");
                }
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    OnPropertyChanged("Message");
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
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void SendCommandAsync(object obj)
        {
            await this.PopAsync();
            //await this.Navigation.PushAsync(new Views.ContactUs.ContactUsPage(), false);
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
