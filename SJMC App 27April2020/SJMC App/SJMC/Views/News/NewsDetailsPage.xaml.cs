using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace SJMC.Views.News
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsDetailsPage : ContentPage
    {
        //TODO : To Declare Class Level Variables...
        protected NewsDetailsPageVM _NewsDetailsPageVM;



        public NewsDetailsPage(NewsModel newsModel, List<NewsModel> moreNewsModels)
        {
            InitializeComponent();
            _NewsDetailsPageVM = new NewsDetailsPageVM(this.Navigation, newsModel, moreNewsModels);
            this.BindingContext = _NewsDetailsPageVM;

            // iOS Platform
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        //private async void TapGestureRecognizer_Tapped1(object sender, EventArgs e)
        //{

        //    int id = Convert.ToInt32(((Xamarin.Forms.TappedEventArgs)e).Parameter);
        //    var current = _NewsDetailsPageVM.MoreNewsModel.Where(x => x.ID == id).FirstOrDefault();
        //    if (current != null)
        //    {
        //        var previousPage = Navigation.NavigationStack.LastOrDefault();
        //        await Navigation.PushAsync(new Views.News.NewsDetailsPage(current, _NewsDetailsPageVM.MoreNewsModel), false);
        //        Navigation.RemovePage(previousPage);
        //    }
        //}

        //private async void TapGestureRecognizer_Tapped2(object sender, EventArgs e)
        //{
        //    int id = Convert.ToInt32(((Xamarin.Forms.TappedEventArgs)e).Parameter);
        //    var current = _NewsDetailsPageVM.MoreNewsModel.Where(x => x.ID == id).FirstOrDefault();
        //    if (current != null)
        //    {
        //        var previousPage = Navigation.NavigationStack.LastOrDefault();
        //        await Navigation.PushAsync(new Views.News.NewsDetailsPage(current, _NewsDetailsPageVM.MoreNewsModel), false);
        //        Navigation.RemovePage(previousPage);
        //    }
        //}

        public async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current = (e.CurrentSelection.FirstOrDefault() as NewsModel);
            if (current != null)
            {
                var previousPage = Navigation.NavigationStack.LastOrDefault();
                await Navigation.PushAsync(new Views.News.NewsDetailsPage(current, _NewsDetailsPageVM.MoreNewsModel.ToList()), false);
                Navigation.RemovePage(previousPage);

            }
        }

    }
}