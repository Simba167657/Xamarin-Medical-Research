using Acr.UserDialogs;
using SJMC.Models;
using SJMC.ServiceProviders;
using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SJMC.Views.News
{
    public class NewsPageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...

        //TODO : To Define Constructor..
        public NewsPageVM(INavigation _Nav)
        {
            Navigation = _Nav;
            BackCommand = new DelegateCommand(BackCommandAsync);

            //TODO : To Define Dummy List ...
            //NewsList = new ObservableCollection<NewsModel>
            //{
            //    new NewsModel() { Id = "1",NewsImage="image.png", NewsTitle = "SAMPLE NEWS TITLE", NewsDate = "10 APR 2019 ", NewsDescription = "Here goes your short description"},
            //    new NewsModel() { Id = "2",NewsImage="image.png", NewsTitle = "SAMPLE NEWS TITLE", NewsDate = "10 APR 2019 ", NewsDescription = "Here goes your short description"},

            //};                             
        }



        #region Properties

        private ObservableCollection<NewsModel> _NewsList;
        public ObservableCollection<NewsModel> NewsList
        {
            get
            {
                return _NewsList;
            }
            set
            {
                _NewsList = value;
                OnPropertyChanged("NewsList");
            }
        }


        #endregion

        #region Delegate Command

        public DelegateCommand BackCommand { get; }

        #endregion

        #region Method

        internal async Task Initilize()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();
                var result = await NewsServices.GetAllNews();
                var list = new List<NewsModel>();
                if (result.Success)
                {
                    int i = 2;
                    foreach (var item in result.data)
                    {
                        item.IssueDate = String.IsNullOrEmpty(item.IssueDate) ? "" : Convert.ToDateTime(item.IssueDate).ToString("dd MMM, yyyy");
                        //item.ImageURL = (ApiBase.HttpClientBase.BaseUrl + "NewsImages/" + item.ImageURL);

                        //var response = await ReportsService.Download(ApiBase.HttpClientBase.BaseUrl + "NewsImages/" + item.ImageURL);
                        var response = await ReportsService.DownloadFileBytes(ApiBase.HttpClientBase.BaseUrl + "api/News/GetNewsImage?newsimage=" + item.ImageURL);
                        if (response.FileContents.Length > 1)
                        {
                            item.ImageData = ImageSource.FromStream(() => new MemoryStream(response.FileContents));
                        }

                        item.IssueDate = "Published on " + item.IssueDate;

                        if (item.IsApproved)
                        {
                            //Background color scheme - even odd.
                            if ((i % 2) == 0)
                                item.BackgroundColor = "#6CAAD3";
                            else
                                item.BackgroundColor = "#D3C1A0";
                            i++;

                            list.Add(item);
                        }
                    }

                    NewsList = new ObservableCollection<NewsModel>(list);
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync(result.Message);
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
            }
            UserDialogs.Instance.HideLoading();

        }
        private async void BackCommandAsync(object obj)
        {
            await this.Navigation.PopAsync();
        }




        #endregion
    }

    //public class NewsModel
    //{
    //    public string Id { get; set; }
    //    public string NewsImage { get; set; }
    //    public string NewsTitle { get; set; }
    //    public string NewsDate { get; set; }
    //    public string NewsDescription { get; set; }
    //}
}
