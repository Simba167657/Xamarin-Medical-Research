using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        private NewsPageVM _NewsPageVM;


        public NewsPage()
        {
            InitializeComponent();
            _NewsPageVM = new NewsPageVM(this.Navigation);
            this.BindingContext = _NewsPageVM;
            _NewsPageVM.Initilize();
            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }


        public NewsPage(int newsIdAutoOpen)
        {
            InitializeComponent();
            _NewsPageVM = new NewsPageVM(this.Navigation);
            this.BindingContext = _NewsPageVM;
            //Initalise();
            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);

            NewsDetail_AutoOpen(newsIdAutoOpen);
        }

        private async void Initalise()
        {
            await _NewsPageVM.Initilize();
        }

        private async void Menu_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            // Helpers.Constants.IsMenuListPresented = true;
            //   Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");
        }

        /// <summary>
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void NewsDetail_Tapped(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            if (grid.BindingContext is NewsModel newsModel)
            {
                var allNews = _NewsPageVM.NewsList.ToList();
                await this.Navigation.PushAsync(new Views.News.NewsDetailsPage(newsModel, allNews), false);
            }
        }

        private async void NewsDetail_AutoOpen(int newsIdAutoOpen)
        {
            try
            {
                if (newsIdAutoOpen != 0)
                {
                    await _NewsPageVM.Initilize();

                    var newsDetail = _NewsPageVM.NewsList.Where(x => x.ID == newsIdAutoOpen).FirstOrDefault();
                    await this.Navigation.PushAsync(new Views.News.NewsDetailsPage(newsDetail, _NewsPageVM.NewsList.ToList()), false);
                }
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.Alert("Something went wrong, please try again. Error: " + ex.Message.ToString());
            }
        }
    }
}