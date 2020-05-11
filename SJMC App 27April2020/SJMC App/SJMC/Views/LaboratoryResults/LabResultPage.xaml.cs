using SJMC.CustomControl;
using SJMC.Helpers;
using SJMC.Interfaces;
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
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views.LaboratoryResults
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LabResultPage : ContentView
    {
        //TODO : To Declare Class Level Variables...
        public LabResultPageVM _LabResultPageVM;

        public LabResultPage()
        {

            try
            {
                InitializeComponent();
                _LabResultPageVM = new LabResultPageVM(this.Navigation);
                this.BindingContext = _LabResultPageVM;
                _LabResultPageVM.LabResultList = new ObservableCollection<ReportsModel>();
                _LabResultPageVM.LabResultListComplete = new ObservableCollection<ReportsModel>();

                if (Settings.NotificationList != null && Settings.OpenReportNumber != 0)
                {
                    var selectedNotiData = Settings.NotificationList.FirstOrDefault(x => x.NotificationId == Settings.OpenReportNumber);
                    if (selectedNotiData.ReportType == "0" && selectedNotiData.IsRead == false)
                    {
                        SJMC.Helpers.Settings.OpenReportPage = false;

                        Show_Report_On_Notification_Received(selectedNotiData.AsRef,
                                                             selectedNotiData.AsBrch,
                                                             selectedNotiData.AsYear);
                        selectedNotiData.IsRead = true;
                        Settings.OpenReportNumber = 0;
                    }
                }
                else
                {
                    Settings.OpenReportNumber = 0;
                }

            }
            catch (Exception ex)
            {

            }
        }


        public async Task Initilize()
        {
            await _LabResultPageVM.Initilize();
            listView.ItemAppearing += (s, e) =>
            {
                try
                {
                    var lastIndex = e.ItemIndex;
                    var lastItem = _LabResultPageVM.LabResultList.LastOrDefault();
                    if (lastIndex == _LabResultPageVM.LabResultList.IndexOf(lastItem))
                    {
                        _LabResultPageVM.PageNumber++;
                        var newList = _LabResultPageVM.SortAndFilter(_LabResultPageVM.CheckFilterIndex, _LabResultPageVM.SortedIndex, _LabResultPageVM.PageNumber, _LabResultPageVM.CalendarSelectedDates);
                        newList.ForEach(a => _LabResultPageVM.LabResultList.Add(a));
                      
                        
                    }
                }
                catch (Exception ex)
                {
                }
            };

        }


        private void Menu_Tapped(object sender, EventArgs e)
        {
            // Helpers.Constants.IsMenuListPresented = true;
            //   Xamarin.Forms.MessagingCenter.Send<string>("", "IsMenuListPresented");
            //    App.HomeView.IsPresented = !App.HomeView.IsPresented;
            // App.Current.MainPage.Navigation.PopAsync();

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


            _LabResultPageVM.CheckByImplementation(reportsModel);



        }

        private async void SearchText_Change(object sender, TextChangedEventArgs e)
        {
            var searchBar = (CustomEntryRegular)sender;
            await Task.Delay(100);
            if (searchBar.Text == e.NewTextValue)
            {
                _LabResultPageVM.Search(searchBar.Text);
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
            _LabResultPageVM.ShowReport(reportsModel);
        }

        private void Show_Report(object sender, EventArgs e)
        {

            if (sender is Image image)
            {
                ReportsModel reportsModel = (ReportsModel)image.BindingContext;
                _LabResultPageVM.ShowReport(reportsModel);
                //if (!String.IsNullOrEmpty(reportsModel.ASREF))
                //{

                //    App.Current.MainPage.Navigation.PushAsync(new PdfPage(reportsModel));
                //}
                //else
                //{
                //    MaterialDialog.Instance.AlertAsync("Report not available");
                //}

            }


        }
        private void Reset_Clicked(object sender, EventArgs e)
        {
            calendar.ClearSelection();
            _LabResultPageVM.CalendarVisibility = false;
            _LabResultPageVM.PageNumber = 1;
            _LabResultPageVM.LabResultList.Clear();
            _LabResultPageVM.CalendarSelectedDates = null;
            _LabResultPageVM.CheckFilterIndex = 2;
            _LabResultPageVM.SortedIndex = 4;
            _LabResultPageVM.CalendarSelectedDates = new Syncfusion.SfCalendar.XForms.SelectionRange(DateTime.Now.AddMonths(-2), DateTime.Now);
            //var list = _LabResultPageVM.SortAndFilter(_LabResultPageVM.CheckFilterIndex, _LabResultPageVM.SortedIndex, _LabResultPageVM.PageNumber, _LabResultPageVM.CalendarSelectedDates);
            var list = _LabResultPageVM.SortAndFilter(2, 4, _LabResultPageVM.PageNumber, _LabResultPageVM.CalendarSelectedDates);

            list.ForEach(a => _LabResultPageVM.LabResultList.Add(a));
            _LabResultPageVM.ResetFilterlable();
        }
        private void Calendar_Tapped(object sender, EventArgs e)
        {
            var xx = calendar.SelectedRange;
            _LabResultPageVM.CalendarVisibility = !_LabResultPageVM.CalendarVisibility;
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (listView.SelectedItem == null) return;

            if (sender is Xamarin.Forms.ListView selectedItem)
            {
                ReportsModel reportsModel = (ReportsModel)selectedItem.SelectedItem;
                _LabResultPageVM.ShowReport(reportsModel);
            }

            listView.SelectedItem = null;
        }
    }
}