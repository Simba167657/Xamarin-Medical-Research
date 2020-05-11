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
    public partial class MenuView : ContentPage
    {
       public ListView menuList;


        public MenuView()
        {
            InitializeComponent();


            this.menuList = masterMenuList;
           // App.MenuView = this;
        }

        private void masterMenuList_ItemTapped(object sender, ItemTappedEventArgs e)
        => ((ListView)sender).SelectedItem = null;

        private void close_Tapped(object sender, EventArgs e)
        {
            App.HomeView.IsPresented = !App.HomeView.IsPresented;
        }

        private void Facebook_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://www.facebook.com/Saint-Joseph-Medical-Center-603207859770837/?ref=bookmarks"));
            App.HomeView.IsPresented = false;
        }

        private void Google_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://business.google.com/u/1/dashboard/l/03066549321158524192"));
            App.HomeView.IsPresented = false;
        }

        private void AboutThis_Tapped(object sender, EventArgs e)
        {
            App.Current.MainPage.Navigation.PushAsync(new AboutThisAppPage());
            App.HomeView.IsPresented = false;
        }
    }
}