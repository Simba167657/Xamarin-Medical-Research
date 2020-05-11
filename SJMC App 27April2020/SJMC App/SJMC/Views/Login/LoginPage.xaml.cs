using SJMC.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...

        protected LoginVM LoginVM;

        public LoginPage()
        {
            try
            {
                InitializeComponent();
                LoginVM = new LoginVM(this.Navigation);
                this.BindingContext = LoginVM;
                LoginVM.Initilize();


                // iOS Platform
                On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (Settings.IsRemember)
            {
                LoginVM.LoginCommand.Execute(null);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new Views.HomeView());
        }

        private void CustomEntryRegular_Focused(object sender, FocusEventArgs e)
        {
            try
            {
                var scrollView = (Xamarin.Forms.ScrollView)Content;
                scrollView.Padding = new Thickness(0, 0, 0, 300);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        private void CustomEntryRegular_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                var scrollView = (Xamarin.Forms.ScrollView)Content;
                scrollView.Padding = new Thickness(0);
            }
            catch (Exception ex)
            {
                //Crashes.TrackError(ex);
            }
        }

        //private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    await this.Navigation.PushAsync(new Views.News.NewsPage());
        //}
    }
}