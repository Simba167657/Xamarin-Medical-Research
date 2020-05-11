using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected ProfilePageVM _ProfilePageVM;

        public ProfilePage()
        {
            InitializeComponent();
            _ProfilePageVM = new ProfilePageVM(this.Navigation);
            this.BindingContext = _ProfilePageVM;
            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ProfilePageVM.Initilize();

        }
        private void Menu_Tapped(object sender, EventArgs e)
        {
            App.HomeView.IsPresented = !App.HomeView.IsPresented;

            // Helpers.Constants.IsMenuListPresented = true;
          //  Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");
        }
    }
}