using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SJMC.Views.ContactUs
{
    public class ContactUsPageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...

        //TODO : To Define Constructor..
        public ContactUsPageVM(INavigation _Nav)
        {
            Navigation = _Nav;
            // BackCommand = new DelegateCommand(BackCommandAsync);
            LabFormCommand = new DelegateCommand(LabFormCommandAsync);
            RadioFormCommand = new DelegateCommand(RadioFormCommandAsync);
            LaserFormCommand = new DelegateCommand(LaserFormCommandAsync);
            WhatsAppCommand = new Command<string>((s) =>
              {
                  try
                  {
                      string iosUrl = $"whatsapp://send?phone=+{s}";
                      string androidUrl = $"https://wa.me/{s}";
                      if (Device.RuntimePlatform == Device.Android)
                      {
                          Device.OpenUri(new Uri(androidUrl));
                      }
                      else
                      {
                          Device.OpenUri(new Uri(iosUrl));
                      }
                  }
                  catch (Exception ex)
                  {
                  }
              });

            //CommandParameter="9611613616" 96171111556
            RadioCommand = new Command(() =>
              {
                  try
                  {
                      string iosUrl = $"whatsapp://send?phone=+9611613616";
                      string androidUrl = $"https://wa.me/96171288986";
                      if (Device.RuntimePlatform == Device.Android)
                      {
                          Device.OpenUri(new Uri(androidUrl));
                      }
                      else
                      {
                          Device.OpenUri(new Uri(iosUrl));
                      }
                  }
                  catch (Exception ex)
                  {
                  }
              });

            LocateCommand = new Command(async() =>
              {
                  await App.Current.MainPage.Navigation.PushAsync(new LocationPage());
              });
        }

        #region Delegate Command

        public DelegateCommand BackCommand { get; }
        public DelegateCommand LabFormCommand { get; }
        public DelegateCommand RadioFormCommand { get; }
        public DelegateCommand LaserFormCommand { get; }

        public ICommand WhatsAppCommand { get; set; }

        public ICommand RadioCommand { get; set; }

        public ICommand LocateCommand { get; set; }

        #endregion

        #region Properties

        #endregion

        #region Method

        /// <summary>
        /// TODO : To Define Lab form Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void LabFormCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.LaboratoryResults.LabForm(), false);
        }

        /// <summary>
        /// TODO : To Define Radio form Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void RadioFormCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.LaboratoryResults.LabForm(), false);
        }

        /// <summary>
        /// TODO : To Define Laser form Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void LaserFormCommandAsync(object obj)
        {
            await this.Navigation.PushAsync(new Views.LaboratoryResults.LabForm(), false);
        }

        #endregion
    }
}
