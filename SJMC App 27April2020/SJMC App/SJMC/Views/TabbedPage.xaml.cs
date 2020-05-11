using SJMC.ViewModels;
using SJMC.Views.LaboratoryResults;
using SJMC.Views.RadiologyResults;
using Syncfusion.XForms.TabView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage : ContentPage
    {
        public TabbedPageViewModel viewModel;
        public bool IsNewInstance { get; set; }
        public int _index { get; set; }

        LabResultPage page1 = new LabResultPage();

        RadioResultPage page2 = new RadioResultPage();

        public TabbedPage(int index)
        {
            InitializeComponent();
            BindingContext = viewModel = new TabbedPageViewModel();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            IsNewInstance = true;
            _index = index;
            App.TabbedPage = this;

            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (IsNewInstance)
            {
                page1 = new LabResultPage();
                page2 = new RadioResultPage();

                //var loader = await MaterialDialog.Instance.LoadingDialogAsync("loading...");
                await page1.Initilize();
                await page2.Initilize();
                //await loader.DismissAsync();

                //var customTab = new SJMC.CustomControl.CustomTabItem();
                //customTab.Content = page1;
                //var customTab2 = new SJMC.CustomControl.CustomTabItem();
                //customTab.Content = page2;

                var tabItems = new TabItemCollection
                {
                     new SfTabItem()
                {
                    Title = "Calls",
                    Content = page1
                },
                    new SfTabItem()
                {
                    Title = "Favorites",
                    Content = page2
                },

                  };
                MyTab.Items = tabItems;
                viewModel.Initilize(_index);

                IsNewInstance = false;
            }


        }

        private void MyTab_TabItemTapped(object sender, Syncfusion.XForms.TabView.TabItemTappedEventArgs e)
        {

        }

        protected override bool OnBackButtonPressed()
        {
            //if (_browser.CanGoBack)
            //{
            //    _browser.GoBack();
            //    return true;
            //}
            //else
            //{
            //    //await Navigation.PopAsync(true);
            //    base.OnBackButtonPressed();
            //    return true;
            //}

            //Set the calender control's visibility to false
            page1._LabResultPageVM.CalendarVisibility = false;
            page2._RadioResultPageVM.CalendarVisibility = false;

            return true;
        }
    }
}