using SJMC.Models;
using SJMC.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Views.News
{
    public class NewsDetailsPageVM : BaseViewModel
    {
        //TODO : To Declare Class Level Variables...

        //TODO : To Define Constructor..
        public NewsDetailsPageVM(INavigation _Nav, NewsModel newsModel, List<NewsModel> moreNewsModel)
        {
            Navigation = _Nav;
            BackCommand = new DelegateCommand(BackCommandAsync);
            NewsModel = newsModel;
            //moreNewsModel.Remove(newsModel);
            //MoreNewsModel = moreNewsModel;
            //NewsModel.Description = newsModel.Description + newsModel.Description + newsModel.Description + newsModel.Description;
            //LoadMoreNewsOnly2();
            LoadMoreNewsCollectionView(moreNewsModel);
        }



        #region Delegate Command

        public DelegateCommand BackCommand { get; }

        #endregion

        #region Properties
        private NewsModel _newsModel;
        public NewsModel NewsModel
        {
            get => _newsModel;
            set
            {
                _newsModel = value;
                OnPropertyChanged("NewsModel");
            }
        }

        private ObservableCollection<NewsModel> _moreNewsModel;
        public ObservableCollection<NewsModel> MoreNewsModel
        {
            get => _moreNewsModel;
            set
            {
                _moreNewsModel = value;
                OnPropertyChanged("MoreNewsModel");
            }
        }

        #endregion

        #region MoreNews Properties
        private ImageSource _imageData1;
        public ImageSource ImageData1
        {
            get => _imageData1;
            set
            {
                _imageData1 = value;
                OnPropertyChanged("ImageData1");
            }
        }
        private ImageSource _imageData2;
        public ImageSource ImageData2
        {
            get => _imageData2;
            set
            {
                _imageData2 = value;
                OnPropertyChanged("ImageData2");
            }
        }
        private string _title1;
        public string Title1
        {
            get => _title1;
            set
            {
                _title1 = value;
                OnPropertyChanged("Title1");
            }
        }
        private string _title2;
        public string Title2
        {
            get => _title2;
            set
            {
                _title2 = value;
                OnPropertyChanged("Title2");
            }
        }
        private string _issueDate1;
        public string IssueDate1
        {
            get => _issueDate1;
            set
            {
                _issueDate1 = value;
                OnPropertyChanged("IssueDate1");
            }
        }
        private string _issueDate2;
        public string IssueDate2
        {
            get => _issueDate2;
            set
            {
                _issueDate2 = value;
                OnPropertyChanged("IssueDate2");
            }
        }

        private int _id1;
        public int Id1
        {
            get => _id1;
            set
            {
                _id1 = value;
                OnPropertyChanged("Id1");
            }
        }
        private int _id2;
        public int Id2
        {
            get => _id2;
            set
            {
                _id2 = value;
                OnPropertyChanged("Id2");
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// TODO : To Define Send Button Tapped Event...
        /// </summary>
        /// <param name="obj"></param>
        private async void BackCommandAsync(object obj)
        {
            await this.Navigation.PopAsync();
        }

        private void LoadMoreNewsCollectionView(List<NewsModel> moreNewsModel)
        {
            List<NewsModel> moreNewsPresent = moreNewsModel;



            MoreNewsModel = new ObservableCollection<NewsModel>(moreNewsModel);
        }


        private void LoadMoreNewsOnly2()
        {
            List<NewsModel> moreNewsLst = new List<NewsModel>(MoreNewsModel);

            // Remove which is loaded on Detail Page
            moreNewsLst.Remove(NewsModel);

            List<NewsModel> moreNewsRandomLst = new List<NewsModel>();

            if (moreNewsLst.Count > 2)
            {
                for (int i = 0; i <= 1; i++)
                {
                    Random random = new Random();
                    int index = random.Next(moreNewsLst.Count);
                    moreNewsRandomLst.Add(moreNewsLst[index]);
                }
            }
            else
            {
                moreNewsRandomLst = moreNewsLst;
            }

            if (moreNewsRandomLst.Count > 0)
            {
                Id1 = moreNewsRandomLst[0].ID;
                ImageData1 = moreNewsRandomLst[0].ImageData;
                Title1 = moreNewsRandomLst[0].Title;
                IssueDate1 = moreNewsRandomLst[0].IssueDate;
            }
            if (moreNewsRandomLst.Count > 1)
            {
                Id2 = moreNewsRandomLst[1].ID;
                ImageData2 = moreNewsRandomLst[1].ImageData;
                Title2 = moreNewsRandomLst[1].Title;
                IssueDate2 = moreNewsRandomLst[1].IssueDate;
            }
        }
        #endregion

    }
}
