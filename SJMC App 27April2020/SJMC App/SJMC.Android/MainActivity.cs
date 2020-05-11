using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Acr.UserDialogs;
using XF.Material.Droid;
using Plugin.Permissions;
using Xamarin.Forms.GoogleMaps.Android;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using System.Collections.Concurrent;
using Android.Gms.Maps.Model;
using Xamarin.Forms.GoogleMaps;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Plugin.FirebasePushNotification;
using Android.Content;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Rg.Plugins.Popup.Services;
using Firebase;
using Firebase.Iid;
using Android.Content.Res;
using static Android.Media.MediaPlayer;
using Android.Media;
using System.Threading.Tasks;
using Plugin.Badge;
using System.Linq;
using SJMC.ViewModels;

namespace SJMC.Droid
{
    [Activity(Label = "SJMC", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        static readonly string TAG = "MainActivity";

        internal static readonly string CHANNEL_ID = "my_notification_channel";
        internal static readonly int NOTIFICATION_ID = 100;

        // public const  string FilePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/SJMC/";

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            //CrossBadge.Current.ClearBadge();

            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");

            var platformConfig = new PlatformConfig
            {
                BitmapDescriptorFactory = new CachingNativeBitmapDescriptorFactory()
            };

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };
            ImageService.Instance.Initialize(config);

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Material.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig);
            global::Xamarin.FormsMaps.Init(this, savedInstanceState);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU2NjkwQDMxMzcyZTMzMmUzMEMydm9JYmZXaEZlQllmK0dHbnp1azc5MXVTcVhaUlp6OXBOb2xWZm9URXM9");
            FormsVideoPlayer.Init();



            //FirebasePushNotificationManager.ProcessIntent(this, Intent);




            CrossFirebasePushNotification.Current.RegisterForPushNotifications();
            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();
            //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            // await  new ServerCommunication().GetFromServerAsync("https://sjmc.stjosephlab-results.com/api/Auth/Login?userInfo=D1&password=D1");

            CreateNotificationChannel();

            //var intent = new Intent(this, typeof(MainActivity));
            //if (intent != null)
            //{
            //    Toast.MakeText(this, "Opening report from main", ToastLength.Short);
            //    OnNewIntent(intent);
            //}
            //else
            //{
            //    Toast.MakeText(this, "Elese Opening report from main" + intent, ToastLength.Short);
            //}
            //Xamarin.Forms.MessagingCenter.Instance.Subscribe<string, string>("Hello", "DeviceLog", (obj, args) =>
            //   {
            //       Android.Util.Log.Error("SJMC", args);
            //   });
            LoadApplication(new App());

            OnNewIntent(Intent);

        }

        // Field, properties, and method for Video Picker
        public static MainActivity Current { private set; get; }

        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<string> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    // Set the filename as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(data.DataString);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }


        protected override void OnNewIntent(Intent intent)
        {
            //Toast.MakeText(this, "OnNewIntent", ToastLength.Long);
            if (intent == null)
                return;

            if (intent.Action == "SjmcReportNotification" && intent.HasExtra("SjmcNotificationId"))
            {
                try
                {

                    /// Do something now that you know the user clicked on the notification...
                    //CrossBadge.Current.ClearBadge();

                    //SJMC.Helpers.Settings.OpenReportPage = true;

                    string kkk = intent.GetStringExtra("SjmcNotificationId");
                    SJMC.Helpers.Settings.OpenReportNumber = Convert.ToInt32(kkk);

                    var winnerToast = Toast.MakeText(this, $"Opening your report...", ToastLength.Long);
                    winnerToast.SetGravity(Android.Views.GravityFlags.Bottom, 0, 0);
                    winnerToast.Show();

                    // App.Current.MainPage = new SJMC.Views.HomeView();
                    // App.Current.MainPage = new NavigationPage(new HomeView());

                    //Toast.MakeText(this, "kkk: " + kkk, ToastLength.Long);

                    FirebasePushNotificationManager.ProcessIntent(this, intent);
                    //App.Current.MainPage = new SJMC.Views.HomeView();
                    if (App.Current.MainPage.Navigation.NavigationStack.Any(x => x is SJMC.Views.HomeView))
                    {
                        var homeView = (SJMC.Views.HomeView)App.Current.MainPage.Navigation.NavigationStack.FirstOrDefault(x => x is SJMC.Views.HomeView);
                        var vm = homeView.BindingContext as HomeViewModel;
                        vm.Initilize();
                    }
                    //App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);

                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, "OnNewIntent Err: " + ex.Message, ToastLength.Long);
                }

            }
            else if (intent.Action == "SjmcNewsNotification" && intent.HasExtra("SjmcNotificationId"))
            {
                string kkk = intent.GetStringExtra("SjmcNotificationId");
                SJMC.Helpers.Settings.OpenNewsNumber = Convert.ToInt32(kkk);
                SJMC.Helpers.Settings.NewsNotificationOpen = true;

                FirebasePushNotificationManager.ProcessIntent(this, intent);
                //App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);
                if (App.Current.MainPage.Navigation.NavigationStack.Any(x => x is SJMC.Views.HomeView))
                {
                    var homeView = (SJMC.Views.HomeView)App.Current.MainPage.Navigation.NavigationStack.FirstOrDefault(x => x is SJMC.Views.HomeView);
                    var vm = homeView.BindingContext as HomeViewModel;
                    vm.Initilize();
                }
            }

            base.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public void GetNewToken()
        //{
        //    FirebaseApp app = new FirebaseApp();
        //    var firId = FirebaseInstanceId.GetInstance(app).GetToken("ABC", "");
        //}

        public override async void OnBackPressed()
        {



            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }


        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID,
                                                  "FCM Notifications",
                                                  NotificationImportance.Default)
            {

                Description = "Firebase Cloud Messages appear in this channel"
            };

            var notificationManager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        protected override void AttachBaseContext(Context @base)
        {
            var configuration = new Configuration(@base.Resources.Configuration);

            configuration.FontScale = 1f;
            var config = Application.Context.CreateConfigurationContext(configuration);

            base.AttachBaseContext(config);
        }


    }

    public sealed class CachingNativeBitmapDescriptorFactory : IBitmapDescriptorFactory
    {
        private readonly ConcurrentDictionary<string, AndroidBitmapDescriptor> _cache
            = new ConcurrentDictionary<string, AndroidBitmapDescriptor>();

        public AndroidBitmapDescriptor ToNative(Xamarin.Forms.GoogleMaps.BitmapDescriptor descriptor)
        {
            var defaultFactory = DefaultBitmapDescriptorFactory.Instance;

            if (!string.IsNullOrEmpty(descriptor.Id))
            {
                var cacheEntry = _cache.GetOrAdd(descriptor.Id, _ => defaultFactory.ToNative(descriptor));
                return cacheEntry;
            }

            return defaultFactory.ToNative(descriptor);
        }

    }

    public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }

        public void Error(string errorMessage)
        {
            Console.WriteLine(errorMessage);
        }

        public void Error(string errorMessage, Exception ex)
        {
            Error(errorMessage + System.Environment.NewLine + ex.ToString());
        }
    }



}