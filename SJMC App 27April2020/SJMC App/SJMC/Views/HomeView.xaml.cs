using SJMC.Helpers;
using SJMC.Popup;
using SJMC.ViewModels;
using SJMC.Views.ContactUs;
using SJMC.Views.LaboratoryResults;
using SJMC.Views.News;
using SJMC.Views.Profile;
using SJMC.Views.RadiologyResults;
using SJMC.Views.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : MasterDetailPage
    {
        //bool _isAppeared = false;

        protected HomeViewModel viewModel;
        public HomeView()
        {
            InitializeComponent();
            App.HomeView = this;
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = viewModel = new HomeViewModel();
            viewModel.NavigatePageOnNofication += () => NavigatePageOnNofication();
            Detail = new NavigationPage(new ProfilePage());

            /*if (SJMC.Helpers.Settings.NewsNotificationOpen && SJMC.Helpers.Settings.NewsNotificationList != null)
            {
                
                var SelectedNews = SJMC.Helpers.Settings.NewsNotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenNewsNumber);
                SJMC.Helpers.Settings.NewsNotificationOpen = false;

                if (SelectedNews != null && SelectedNews.IsRead == false)
                {
                    Constants.isNews = true;
                    int newsId = Convert.ToInt32(SelectedNews.NewsId);
                    App.Current.MainPage.Navigation.PushAsync(new NewsPage(newsId));
                    SelectedNews.IsRead = true;
                }
            }
            else if (SJMC.Helpers.Settings.NotificationList != null)
            {
               
                var selectedReport = SJMC.Helpers.Settings.NotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenReportNumber);

                if (selectedReport != null && selectedReport.IsRead == false)
                {
                    Constants.isReport = true;
                    if (selectedReport.ReportType == "0")
                        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(0));
                    else
                        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(1));
                }
            }*/
            

        }

        private void NavigatePageOnNofication()
        {
            if (SJMC.Helpers.Settings.NewsNotificationOpen && SJMC.Helpers.Settings.NewsNotificationList != null)
            {

                var SelectedNews = SJMC.Helpers.Settings.NewsNotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenNewsNumber);
                SJMC.Helpers.Settings.NewsNotificationOpen = false;

                if (SelectedNews != null && SelectedNews.IsRead == false)
                {
                    Constants.isNews = true;
                    int newsId = Convert.ToInt32(SelectedNews.NewsId);
                    App.Current.MainPage.Navigation.PushAsync(new NewsPage(newsId));
                    SelectedNews.IsRead = true;
                }
            }
            else if (SJMC.Helpers.Settings.NotificationList != null)
            {

                var selectedReport = SJMC.Helpers.Settings.NotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenReportNumber);

                if (selectedReport != null && selectedReport.IsRead == false)
                {
                    Constants.isReport = true;

                    if (selectedReport.ReportType == "0")
                        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(0));
                    else
                        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(1));
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Initilize();

            //Settings.IsAppeared = true;

            //if (SJMC.Helpers.Constants.NotificationReportOpener != null)
            //{
            //    var selectedReport = SJMC.Helpers.Constants.NotificationReportOpener.FirstOrDefault(x => x.MessageId == Settings.OpenReportNumber);

            //    if (selectedReport != null && selectedReport.IsRead == false)
            //    {
            //        if (selectedReport.ReportType == "0")
            //            App.Current.MainPage.Navigation.PushAsync(new TabbedPage(0));
            //        else
            //            App.Current.MainPage.Navigation.PushAsync(new TabbedPage(1));
            //    }
            //}
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //Settings.IsAppeared = false;
        }


        private void masterMenuList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var listview = (ListView)sender;
            if (listview.SelectedItem == null) return;

            if (listview.SelectedItem is ViewModels.Menu menu)
            {
                viewModel.Navigate(menu);
            }

            listview.SelectedItem = null;
        }


        #region first

        // TODO: Set the navigation menu at the runtime.

        //void NavigateTo(ViewModels.Menu menu)
        //{
        //    if (menu.TargetType == typeof(ProfilePage))
        //    {
        //        Detail = new NavigationPage(new ProfilePage());
        //    }
        //    else if (menu.TargetType == typeof(LabResultPage))
        //    {
        //        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(0));
        //    }
        //    else if (menu.TargetType == typeof(RadioResultPage))
        //    {
        //        App.Current.MainPage.Navigation.PushAsync(new TabbedPage(1));

        //    }
        //    else if (menu.TargetType == typeof(RegistrationPage))
        //    {
        //        Detail = new NavigationPage(new RegistrationPage());
        //    }
        //    else if (menu.TargetType == typeof(ContactUsPage))
        //    {
        //        App.Current.MainPage.Navigation.PushAsync(new ContactUsPage());
        //    }
        //    else if (menu.TargetType == typeof(LocationPage))
        //    {
        //        Navigation.PushAsync(new LocationPage());
        //    }
        //    else if (menu.TargetType == typeof(NewsPage))
        //    {
        //        App.Current.MainPage.Navigation.PushAsync(new NewsPage());
        //    }
        //    App.HomeView.IsPresented = false;
        //}



        #endregion


    }
}