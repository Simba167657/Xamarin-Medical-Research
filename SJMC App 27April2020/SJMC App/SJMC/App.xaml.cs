using Acr.UserDialogs;
using Plugin.FirebasePushNotification;
using SJMC.Helpers;
using SJMC.Models;
using SJMC.ServiceProviders;
using SJMC.Views;
using SJMC.Views.Login;
using SJMC.Views.PlayVideoResource;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC
{
    public partial class App : Application
    {


        public static bool IsAndroid { get; set; }
        public static HomeView HomeView;
        public static Views.TabbedPage TabbedPage;

        public App()
        {

            InitializeComponent();
            Material.Init(this);
            IsAndroid = Device.Android == Device.RuntimePlatform;

            MainPage = new NavigationPage(new PlayVideoResourcePage());


            //if (Constants.ShowNewPlayerForSplashScreen == true)
            //{
            //    MainPage = new NavigationPage(new SplashScreenPage());
            //    //Delay(3500);
            //}

            //if (Settings.IsRemember && Settings.Email != string.Empty && Settings.Password != string.Empty)
            //{
            //    Models.UserProfileModel result = new Models.UserProfileModel();
            //    result = new Models.UserProfileModel
            //    {
            //        Email = Settings.Email,
            //        LabID = Settings.LabId,
            //        UserID = Settings.UserID,
            //        UserPass = Settings.Password,
            //        Role = Settings.Role,
            //        Name = Settings.Name
            //    };
            //    Constants.UserProfile = result;

            //    MainPage = new NavigationPage(new HomeView());
            //}
            //else
            //{
            //    MainPage = new NavigationPage(new LoginPage());
            //}
            //return;

            //MainPage = new NavigationPage(new SplashScreenPage());

            Task.Run(async () =>
            {
                await SettingSyncFromServer();
            });

            if (SJMC.Helpers.Settings.NotificationBadageCount < 0)
            {
                SJMC.Helpers.Settings.NotificationBadageCount = 0;
            }
        }

        private async Task SettingSyncFromServer()
        {
            //BloodSampleNumber
            //BloodSampleMessage
            AppSettingsServices services = new AppSettingsServices();
            try
            {
                AppSettings setting = await services.GetAppSettingsKey(Constants.AppSettingsKey_BloodSampleNumber);
                if (setting != null)
                    Settings.AppSettings_BloodSampleNumber = setting.SettingValue;
            }
            catch (Exception)
            {
            }

            try
            {
                AppSettings setting = await services.GetAppSettingsKey(Constants.AppSettingsKey_BloodSampleMessage);
                if (setting != null)
                    Settings.AppSettings_BloodSampleMessage = setting.SettingValue;
            }
            catch (Exception)
            {
            }
        }

        private async void Delay(int miliseconds)
        {
            await Task.Delay(miliseconds);
        }

        private bool TryLogin()
        {
            return TryLoginAsync().Result;
        }

        private async Task<bool> TryLoginAsync()
        {
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Storage);
            await PermissionUtils.CheckPermissions(Plugin.Permissions.Abstractions.Permission.Location);

            string username = Settings.Email;
            string password = Settings.Password;

            var loader = await MaterialDialog.Instance.LoadingDialogAsync("Signing In");
            AuthServices auth = new AuthServices();
            var result = await auth.Login(username, password);
            if (result.Success)
            {
                Constants.UserProfile = result;

                FirebaseTokenService firebaseToken = new FirebaseTokenService();

                var tokenResult = await firebaseToken.InsertFirebaseToken(result.LabID, result.Role, Settings.FirebaseToken, App.IsAndroid, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                if (tokenResult.Success)
                {
                    return true;
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync(tokenResult.Message);
                }
            }
            else
            {
                await MaterialDialog.Instance.AlertAsync(result.Message);
            }
            await loader.DismissAsync();
            return false;
        }

        protected override void OnStart()
        {
           
        }

        //protected override void OnStart()
        //{
        //    // Handle when your app sleeps
        //}

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
