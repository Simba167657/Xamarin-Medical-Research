using SJMC.CustomControl;
using SJMC.Helpers;
using SJMC.Interfaces;
using SJMC.Models;
using SJMC.ServiceProviders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views.RadiologyResults
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RadioResultPage : ContentView
    {
        //TODO : To Declare Class Level Variables...
        public RadioResultPageVM _RadioResultPageVM;

        public RadioResultPage()
        {
            try
            {
                InitializeComponent();
                _RadioResultPageVM = new RadioResultPageVM(this.Navigation);
                this.BindingContext = _RadioResultPageVM;
                _RadioResultPageVM.RadioResultList = new ObservableCollection<ReportsModel>();
                _RadioResultPageVM.RadioResultListComplete = new ObservableCollection<ReportsModel>();


                if (Settings.NotificationList != null && Settings.OpenReportNumber != 0)
                {
                    var selectedNotiData = Settings.NotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenReportNumber);
                    if (selectedNotiData.ReportType == "1" && selectedNotiData.IsRead == false)
                    {
                        SJMC.Helpers.Settings.OpenReportPage = false;

                        Show_Report_On_Notification_Received(selectedNotiData.AsRef,
                                                             selectedNotiData.AsBrch,
                                                             selectedNotiData.AsYear);
                        selectedNotiData.IsRead = true;
                        Settings.OpenReportNumber = 0;
                    }
                }

            }
            catch (Exception ex)
            {

            }


            // iOS Platform
            // On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        public async Task Initilize()
        {
            await _RadioResultPageVM.Initilize();
            listView.ItemAppearing += (s, e) =>
            {
                try
                {
                    var lastIndex = e.ItemIndex;
                    var lastItem = _RadioResultPageVM.RadioResultList.LastOrDefault();
                    if (lastIndex == _RadioResultPageVM.RadioResultList.IndexOf(lastItem))
                    {
                        _RadioResultPageVM.PageNumber++;
                        var newList = _RadioResultPageVM.SortAndFilter(_RadioResultPageVM.CheckFilterIndex, _RadioResultPageVM.SortedIndex, _RadioResultPageVM.PageNumber, _RadioResultPageVM.CalendarSelectedDates);
                        newList.ForEach(a => _RadioResultPageVM.RadioResultList.Add(a));
                    }
                }
                catch (Exception ex)
                {
                }
            };
        }


        private void Menu_Tapped(object sender, EventArgs e)
        {
            //App.Current.MainPage.Navigation.PopAsync();
            // App.HomeView.IsPresented = !App.HomeView.IsPresented;
            // Helpers.Constants.IsMenuListPresented = true;
            //   Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");

            var previousPage = Navigation.NavigationStack.LastOrDefault();
            App.Current.MainPage.Navigation.PushAsync(new HomeView(), true);
            Navigation.RemovePage(previousPage);
        }

        private void Report_IsChecked(object sender, EventArgs e)
        {
            ReportsModel reportsModel = null;

            if (sender is CustomLabelRegular customLabel)
                reportsModel = (ReportsModel)customLabel.BindingContext;


            if (sender is Image image)
                reportsModel = (ReportsModel)image.BindingContext;


            _RadioResultPageVM.CheckByImplementation(reportsModel);



        }

        private async void SearchText_Change(object sender, TextChangedEventArgs e)
        {
            var searchBar = (CustomEntryRegular)sender;
            await Task.Delay(100);
            if (searchBar.Text == e.NewTextValue)
            {
                _RadioResultPageVM.Search(searchBar.Text);
            }
        }

        private void Show_Report_On_Notification_Received(string asRef, string asBrch, string asYear)
        {
            ReportsModel reportsModel = new ReportsModel
            {
                ASREF = asRef,
                ASBRCH = asBrch,
                ASYEAR = asYear
            };
            _RadioResultPageVM.ShowReport(reportsModel);
        }

        private async void Show_Report(object sender, EventArgs e)
        {
            if (sender is Image image)
            {
                ReportsModel reportsModel = (ReportsModel)image.BindingContext;
                _RadioResultPageVM.ShowReport(reportsModel);


            }
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            calendar.ClearSelection();
            _RadioResultPageVM.CalendarVisibility = false;
            _RadioResultPageVM.PageNumber = 1;
            _RadioResultPageVM.RadioResultList.Clear();
            _RadioResultPageVM.CalendarSelectedDates = new Syncfusion.SfCalendar.XForms.SelectionRange(DateTime.Now.AddMonths(-2), DateTime.Now);
            var list = _RadioResultPageVM.SortAndFilter(_RadioResultPageVM.CheckFilterIndex, _RadioResultPageVM.SortedIndex, _RadioResultPageVM.PageNumber, _RadioResultPageVM.CalendarSelectedDates);
            list.ForEach(a => _RadioResultPageVM.RadioResultList.Add(a));
            FilterLbl.Text = "All Reports";
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem == null) return;

            if (sender is Xamarin.Forms.ListView selectedItem)
            {
                ReportsModel reportsModel = (ReportsModel)selectedItem.SelectedItem;
                _RadioResultPageVM.ShowReport(reportsModel);
            }

            listView.SelectedItem = null;
        }
    }
}