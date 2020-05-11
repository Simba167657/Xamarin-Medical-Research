using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.PasswordReset
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordResetPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected PasswordResetPageVM _PasswordResetPageVM;

        public PasswordResetPage()
        {
            InitializeComponent();
            _PasswordResetPageVM = new PasswordResetPageVM(this.Navigation);
            this.BindingContext = _PasswordResetPageVM;

            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }
    }
}