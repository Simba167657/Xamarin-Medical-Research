using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
        private void Menu_Tapped(object sender, EventArgs e)
        {
            App.HomeView.IsPresented = !App.HomeView.IsPresented;

            // Helpers.Constants.IsMenuListPresented = true;
        //    Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");
        }
    }
}