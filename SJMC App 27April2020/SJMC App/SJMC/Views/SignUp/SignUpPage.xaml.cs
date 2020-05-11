using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.SignUp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected SignUpPageVM _SignUpPageVM;

        bool _allowBack;

        public SignUpPage(bool allowBack = false)
        {
            InitializeComponent();
            _allowBack = allowBack;

            _SignUpPageVM = new SignUpPageVM(this.Navigation);
            this.BindingContext = _SignUpPageVM;
            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        protected override bool OnBackButtonPressed()
        {
            if (_SignUpPageVM.AllowBack)
            {
                return base.OnBackButtonPressed();
            }
            else
            {
                return true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _SignUpPageVM.Initilize(_allowBack);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                swtReceiveNotification.ThumbColor = Color.FromHex("#1e428f");
            }
            else
            {
                swtReceiveNotification.ThumbColor = Color.Gray;
            }
        }
    }
}