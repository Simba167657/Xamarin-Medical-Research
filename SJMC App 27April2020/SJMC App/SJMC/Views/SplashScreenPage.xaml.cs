using Octane.Xamarin.Forms.VideoPlayer;
using SJMC.Views.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashScreenPage : ContentPage
    {
        public SplashScreenPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void VideoPlayer_Completed(object sender, Octane.Xamarin.Forms.VideoPlayer.Events.VideoPlayerEventArgs e)
        {
            //Navigation.PushAsync(new LoginPage());

        }

        private void VideoPlayer_Playing(object sender, Octane.Xamarin.Forms.VideoPlayer.Events.VideoPlayerEventArgs e)
        {

        }

        private void VideoPlayer_OnFailed(object sender, Octane.Xamarin.Forms.VideoPlayer.Events.VideoPlayerEventArgs e)
        {
        }
    }
}