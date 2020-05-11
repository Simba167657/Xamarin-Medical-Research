using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.ContactUs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactUsPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected ContactUsPageVM _ContactUsPageVM;

        public ContactUsPage()
        {
            InitializeComponent();
            _ContactUsPageVM = new ContactUsPageVM(this.Navigation);
            this.BindingContext = _ContactUsPageVM;

            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        private void Menu_Tapped(object sender, EventArgs e)
        {
          //  App.HomeView.IsPresented = !App.HomeView.IsPresented;
            App.Current.MainPage.Navigation.PopAsync();

            // App.HomeView.IsPresented = !App.HomeView.IsPresented;
            //  Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");
        }

        private void WhatsAppNaviagte(object sender, EventArgs e)
        {
            var stack = (StackLayout)sender;
            
        }
    }
}