using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SJMC.Helpers;
using SJMC.Views.Login;
using Xamarin.Forms;

namespace SJMC.Views.PlayVideoResource
{
    public partial class PlayVideoResourcePage : ContentPage
    {
        public PlayVideoResourcePage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    Constants.ShowNewPlayerForSplashScreen = true;
                }
                if (Constants.ShowNewPlayerForSplashScreen == true)
                {
                    await Task.Delay(new TimeSpan(0, 0, 4));
                }

                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (Settings.IsRemember && Settings.Email != string.Empty && Settings.Password != string.Empty)
                    {
                        Models.UserProfileModel result = new Models.UserProfileModel();
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
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    return;
                });
            });
        }
    }
}
