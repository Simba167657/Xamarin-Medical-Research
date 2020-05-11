using SJMC.ViewModels;
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
    public partial class AboutThisAppPage : ContentPage
    {
        AboutThisAppPageViewModel viewModel;
        public AboutThisAppPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new AboutThisAppPageViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}