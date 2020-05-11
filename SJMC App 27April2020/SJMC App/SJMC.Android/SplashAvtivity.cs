using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using static Android.Media.MediaPlayer;

namespace SJMC.Droid
{
    [Activity(Theme = "@style/Theme.splash", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class SplashAvtivity : Activity
    {
        MediaPlayer player;
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.splashView);

            var manufacturer = DeviceInfo.Manufacturer;
            var deviceName = DeviceInfo.Name;

            if (Android.OS.Build.VERSION.SdkInt == Android.OS.BuildVersionCodes.M)
            {
                SJMC.Helpers.Constants.ShowNewPlayerForSplashScreen = true;
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
            else if (manufacturer.ToLower().Contains("xiaomi") || deviceName.ToLower().Equals("redmi"))
            {
                SJMC.Helpers.Constants.ShowNewPlayerForSplashScreen = true;
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            }
            else
            {
                SJMC.Helpers.Constants.ShowNewPlayerForSplashScreen = false;
                Task startupWork = new Task(() => { SimulateStartup(); });
                startupWork.Start();
            }

            //if (Android.OS.Build.VERSION.SdkInt == Android.OS.BuildVersionCodes.M)
            //{
            //   StartActivity(new Intent(Application.Context, typeof(MainActivity)));
            //}


        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            //Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
            //await Task.Delay(8000); // Simulate a bit of startup work.
            //Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
            //StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            try
            {
                Xamarin.Essentials.MainThread.BeginInvokeOnMainThread(async () =>
                {

                    var videoView = FindViewById<Android.Widget.VideoView>(Resource.Id.videoViewID);
                    //CrossMediaManager.Current.MediaPlayer.VideoView = videoView;
                    //Assembly assembly = GetType().GetTypeInfo().Assembly;
                    //var media_item = CrossMediaManager.Current.PlayFromAssembly("Assembly.Resource.raw.sample.mp4", assembly);

                    //await CrossMediaManager.Current.Play(media_item);

                    //media.MetadataUpdated += (sender, args) =>
                    //{
                    //   StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    //};

                    MediaController mediaController = new MediaController(this);
                    mediaController.SetAnchorView(videoView);
                    videoView.SetMediaController(null);
                    videoView.SetVideoURI(Android.Net.Uri.Parse("android.resource://" + PackageName + "/" +
                    Resource.Raw.sample));

                    videoView.Completion += (s, e) =>
                    {
                        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
                    };

                    videoView.Start();
                });
            }
            catch (Exception ex)
            {
                // Handle this catch to anyway load MainPage, for HTC devices.
                //StartActivity(typeof(MainActivity));
                StartActivity(new Intent(Application.Context, typeof(MainActivity)));

            }
        }

    }

}