using Acr.UserDialogs;
using SJMC.Helpers;
using SJMC.Interfaces;
using SJMC.Models;
using SJMC.ServiceProviders;
using SJMC.ViewModels;
using SJMC.Views.RadiologyResults;
using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XF.Material.Forms.UI.Dialogs;

namespace SJMC.Views.LaboratoryResults
{
    public class LabResultPageVM : BaseViewModel
    {
        #region CTOR
        public LabResultPageVM(INavigation _Nav)
        {
            Navigation = _Nav;

            CheckFilterIndex = -1;

            CalendarCommand = new DelegateCommand((s) =>
            {
                CalendarVisibility = true;
            });

            CheckFilterCommand = new DelegateCommand(async (s) =>
            {
                try
                {
                    var index = await ShowActionSheet("Filter", CheckReportsFilter.ToList());// await MaterialDialog.Instance.SelectActionAsync(CheckReportsFilter);
                    if (index >= 0)
                    {
                        LabResultList.Clear();
                        PageNumber = 1;
                        CheckFilterIndex = index;
                        var list = SortAndFilter(CheckFilterIndex, SortedIndex, PageNumber, CalendarSelectedDates);
                        list.ForEach(a => LabResultList.Add(a));
                        FilterLabelText = SetFilterLabelText(CheckFilterIndex, CalendarSelectedDates);
                    }
                }
                catch(Exception ex)
                {

                }
                
            });

            SortCommand = new DelegateCommand(async (s) =>
            {
                try
                {
                    var index = await ShowActionSheet("Sort", SortDatesFilter.ToList());// await MaterialDialog.Instance.SelectActionAsync(SortDatesFilter);
                    if (index >= 0)
                    {
                        LabResultList.Clear();
                        PageNumber = 1;
                        SortedIndex = index;
                        var list = SortAndFilter(CheckFilterIndex, SortedIndex, PageNumber, CalendarSelectedDates);
                        list.ForEach(a => LabResultList.Add(a));
                    }
                }
                catch (Exception ex)
                {
                }
            });

            FilterCommand = new DelegateCommand(async (s) =>
           {
               try
               {
                   LabResultList.Clear();
                   await Initilize();
                   PageNumber = 1;
                   var list = SortAndFilter(CheckFilterIndex, SortedIndex, PageNumber, CalendarSelectedDates);
                   list.ForEach(a => LabResultList.Add(a));
                   CalendarVisibility = false;
                   FilterLabelText = SetFilterLabelText(CheckFilterIndex, CalendarSelectedDates);

               }
               catch (Exception ex)
               {
               }
           });


            //CalendarSelectedDates.StartDate = DateTime.Now.AddMonths(2);
            //CalendarSelectedDates.EndDate = DateTime.Now;
        }

        internal void ResetFilterlable()
        {
            FilterLabelText = SetFilterLabelText(CheckFilterIndex, CalendarSelectedDates);
        }

        public string SetFilterLabelText(int CheckFilterIndex, SelectionRange CalendarSelectedDates)
        {
            string result = "";
            switch (CheckFilterIndex)
            {
                case 0:
                    result = "Checked Reports";
                    break;
                case 1:
                    result = "Unchecked Reports";
                    break;
                case 2:
                    result = "All Reports";
                    break;
            }
            if (CalendarSelectedDates != null)
            {
                if(CheckFilterIndex == -1)
                {
                    result = " From " + CalendarSelectedDates.StartDate.ToString("MM/dd/yyyy") + " - " + CalendarSelectedDates.EndDate.ToString("MM/dd/yyyy");
                }
                else
                {
                    result = result + " from " + CalendarSelectedDates.StartDate.ToString("MM/dd/yyyy") + " - " + CalendarSelectedDates.EndDate.ToString("MM/dd/yyyy");
                }
                
            }
               
            return result;
        }

        #endregion

        #region Properties
        public int PageNumber { get; set; }
        public int SortedIndex { get; set; }
        public int CheckFilterIndex { get; set; }

        public ObservableCollection<string> CheckReportsFilter { get; set; } = new ObservableCollection<string>() { "Checked", "UnChecked", "All" };
        public ObservableCollection<string> SortDatesFilter { get; set; } = new ObservableCollection<string>() { "Collection Date", "Report Date", "Upload Date", "Checked Date", "Reset" };

        private SelectionRange _calendarSelectedDates = new SelectionRange(DateTime.Now.AddMonths(-2), DateTime.Now);
        public SelectionRange CalendarSelectedDates
        {
            get
            {
                return _calendarSelectedDates;
            }
            set
            {
                _calendarSelectedDates = value;
                FilterLabelText = SetFilterLabelText(CheckFilterIndex, CalendarSelectedDates);
                OnPropertyChanged("CalendarSelectedDates");
            }
        }


        private ObservableCollection<ReportsModel> _LabResultList;
        public ObservableCollection<ReportsModel> LabResultList
        {
            get
            {
                return _LabResultList;
            }
            set
            {
                _LabResultList = value;
                OnPropertyChanged("LabResultList");
            }
        }

        private ObservableCollection<ReportsModel> _LabResultListComplete;
        public ObservableCollection<ReportsModel> LabResultListComplete
        {
            get
            {
                return _LabResultListComplete;
            }
            set
            {
                _LabResultListComplete = value;
                OnPropertyChanged("LabResultList");
            }
        }
        private bool _calendarVisibility;
        public bool CalendarVisibility
        {
            get
            {
                return _calendarVisibility;
            }
            set
            {
                _calendarVisibility = value;
                OnPropertyChanged("CalendarVisibility");
            }
        }
        public string _FilterLabelText = "All Reports";
        public string FilterLabelText
        {
            get
            {
                return _FilterLabelText;
            }
            set
            {
                _FilterLabelText = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region Delegate Command

        public DelegateCommand BackCommand { get; set; }
        //public DelegateCommand RadioCommand { get; set; }
        //public DelegateCommand LabCommand { get; set; }
        public DelegateCommand CalendarCommand { get; set; }
        public DelegateCommand CheckFilterCommand { get; set; }
        public DelegateCommand SortCommand { get; set; }

        public DelegateCommand FilterCommand { get; set; }
        #endregion

        #region Events

        internal async void ShowReport(ReportsModel reportsModel)
        {
            try
            {
                var res = LabResultListComplete.Where(x => x.ASREF == reportsModel.ASREF &&
                                            x.ASYEAR == reportsModel.ASYEAR).FirstOrDefault();
                if (res != null)
                {
                    res.IsViewed = true;
                }

                App.TabbedPage.viewModel.LabCount--;
                App.TabbedPage.viewModel.LabCountVisibility = App.TabbedPage.viewModel.LabCount >= 0;

                Task.Run(async () =>
                {
                    var result = await ReportsService.SetSeenReports(Constants.UserProfile.Role, reportsModel.ASBRCH, reportsModel.ASYEAR,
                    reportsModel.ASREF, "Y");
                });
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (!String.IsNullOrEmpty(reportsModel.ASREF))
                {
                    var name = $"{reportsModel.ASBRCH}_{reportsModel.ASYEAR}_{reportsModel.ASREF}";
                    var loader = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                    var result = await ReportsService.GetAttachmentsList(name);
                    await loader.DismissAsync();

                    if (result.Success)
                    {
                        var list = result.data.ToList();
                        list.Insert(0, name + ".pdf");

                        var newList = new List<string>();
                        foreach (var item in list)
                        {
                            var newIndex = list.IndexOf(item);
                            if (newIndex == 0)
                            {
                                newList.Add("Main Report");
                            }
                            else
                            {
                                newList.Add($"Attachment {newIndex}");
                            }
                        }


                        var index = await ShowActionSheet("Select", newList);
                        if (index >= 0)
                        {
                            if (index >= 1)
                            {
                                var nativeFileHandler = DependencyService.Get<INativeFileHandler>();
                                //var url = $"{ApiBase.HttpClientBase.BaseUrl}/Attachments/{list[index]}";
                                var loader1 = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                                //var bytes = await ReportsService.Download($"/Attachments/{list[index]}");// DownloadPDFAsync(url);
                                //var response = await ReportsService.DownloadFileBytes(ApiBase.HttpClientBase.BaseUrl + $"api/Reports/GetReportAttachment?filename=X_19_19804_01.PDF");
                                var response = await ReportsService.DownloadFileBytes(ApiBase.HttpClientBase.BaseUrl + $"api/Reports/GetReportAttachment?filename={list[index]}");
                                await loader1.DismissAsync();
                                if (response.FileContents != null && response.FileContents.Length > 1)
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new PdfPage(response.FileContents));
                                    nativeFileHandler.SaveFile(list[index], response.FileContents);
                                    //   await MaterialDialog.Instance.AlertAsync("Report downloaded in your local storage");
                                }
                                else
                                {
                                    await MaterialDialog.Instance.AlertAsync("Cannot download report due to server issue");
                                }
                            }
                            else
                            {
                                //var url = $"{ApiBase.HttpClientBase.BaseUrl}/api/Reports/DownloadReport?refernce={reportsModel.ASREF}";
                                var loader1 = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                                var pdfModel = await ReportsService.DownloadBytes($"api/Reports/DownloadReportBytes?brch={reportsModel.ASBRCH}&year={reportsModel.ASYEAR}&refernce={reportsModel.ASREF}");
                                await loader1.DismissAsync();
                                if (pdfModel.FileContents != null && pdfModel.FileContents.Length > 1)
                                {
                                    //var pdfByte = HtmlByteToPdfByte(pdfModel.FileContents);
                                    //await App.Current.MainPage.Navigation.PushAsync(new PdfPage(pdfByte));
                                    await App.Current.MainPage.Navigation.PushAsync(new BrowserPage(pdfModel.FileContents));
                                }
                                else
                                {
                                    await MaterialDialog.Instance.AlertAsync("Report not available");
                                }
                            }
                        }
                    }
                    else
                    {
                        var url = $"api/Reports/DownloadReportBytes?brch={reportsModel.ASBRCH}&year={reportsModel.ASYEAR}&refernce={reportsModel.ASREF}";
                        var loader1 = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                        var pdfModel = await ReportsService.DownloadBytes(url);
                        await loader1.DismissAsync();
                        if (pdfModel.FileContents != null && pdfModel.FileContents.Length > 1)
                        {
                            //var pdfByte = HtmlByteToPdfByte(pdfModel.FileContents);
                            //await App.Current.MainPage.Navigation.PushAsync(new PdfPage(pdfByte));
                            await App.Current.MainPage.Navigation.PushAsync(new BrowserPage(pdfModel.FileContents));
                        }
                        else
                        {
                            await MaterialDialog.Instance.AlertAsync("Report not available");
                        }
                    }
                }
                else
                {
                    await MaterialDialog.Instance.AlertAsync("Report not available");
                }
            }
            catch (Exception ex)
            {
                await MaterialDialog.Instance.AlertAsync(ex.Message);

            }

        }


        public List<ReportsModel> SortAndFilter(int checkFilterIndex, int sortFilterIndex, int pageNumber, SelectionRange calendarSelectedDates)
        {
            int skipCount = 0;
            if (pageNumber == 1)
            {
                skipCount = 0;
            }
            else
            {
                var newPageNumber = pageNumber - 1;
                skipCount = newPageNumber * 20;
            }

            List<ReportsModel> tempCompleteList = new List<ReportsModel>();


            if (calendarSelectedDates != null && (calendarSelectedDates.StartDate != calendarSelectedDates.EndDate))
            {
                for (var date = calendarSelectedDates.StartDate; date <= calendarSelectedDates.EndDate; date = date.AddDays(1))
                {
                    foreach (var item in LabResultListComplete)
                    {
                        if (Convert.ToDateTime(item.ASUPLDTE).Date == date.Date)
                        {
                            tempCompleteList.Add(item);
                        }
                    }
                }
            }
            else
            {
                tempCompleteList = LabResultListComplete.ToList();
            }

            var totalOutputList = tempCompleteList.OrderBy(s => s.ASREF).Skip(skipCount).Take(20).ToList();

            List<ReportsModel> checkFilterdOutputList = new List<ReportsModel>();

            if (checkFilterIndex == 0)
            {
                CheckFilterIndex = checkFilterIndex;
                switch (Constants.UserProfile.Role)
                {
                    case "patient":
                        checkFilterdOutputList = totalOutputList.Where(ss => !String.IsNullOrEmpty(ss.ASPCHKBY)).ToList();
                        break;

                    case "doctor":
                        checkFilterdOutputList = totalOutputList.Where(ss => !String.IsNullOrEmpty(ss.ASDCHKBY)).ToList();
                        break;

                    case "agent":
                        checkFilterdOutputList = totalOutputList.Where(ss => !String.IsNullOrEmpty(ss.ASACHKBY)).ToList();
                        break;

                    case "guarantor":
                        checkFilterdOutputList = totalOutputList.Where(ss => !String.IsNullOrEmpty(ss.ASGCHKBY)).ToList();
                        break;
                }

            }
            else if (checkFilterIndex == 1)
            {
                CheckFilterIndex = checkFilterIndex;
                switch (Constants.UserProfile.Role)
                {
                    case "patient":
                        checkFilterdOutputList = totalOutputList.Where(ss => String.IsNullOrEmpty(ss.ASPCHKBY)).ToList();
                        break;

                    case "doctor":
                        checkFilterdOutputList = totalOutputList.Where(ss => String.IsNullOrEmpty(ss.ASDCHKBY)).ToList();
                        break;

                    case "agent":
                        checkFilterdOutputList = totalOutputList.Where(ss => String.IsNullOrEmpty(ss.ASACHKBY)).ToList();
                        break;

                    case "guarantor":
                        checkFilterdOutputList = totalOutputList.Where(ss => String.IsNullOrEmpty(ss.ASGCHKBY)).ToList();
                        break;
                }
            }
            else if (checkFilterIndex == 2)
            {
                CheckFilterIndex = checkFilterIndex;
                checkFilterdOutputList = totalOutputList.ToList();
            }


            List<ReportsModel> sortedOutputList = new List<ReportsModel>();
            if (sortFilterIndex == 0)
            {
                SortedIndex = sortFilterIndex;
                sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASCDTE)).ToList();
            }
            else if (sortFilterIndex == 1)
            {
                SortedIndex = sortFilterIndex;
                sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASRDTE)).ToList();

            }
            else if (sortFilterIndex == 2)
            {
                SortedIndex = sortFilterIndex;
                sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASUPLDTE)).ToList();

            }
            else if (sortFilterIndex == 3)
            {
                SortedIndex = sortFilterIndex;
                switch (Constants.UserProfile.Role)
                {
                    case "patient":
                        sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASPCHKDTE)).ToList();
                        break;

                    case "doctor":
                        sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASDCHKDTE)).ToList();
                        break;

                    case "agent":
                        sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASACHKDTE)).ToList();
                        break;

                    case "guarantor":
                        sortedOutputList = checkFilterdOutputList.OrderBy(x => Convert.ToDateTime(x.ASGCHKDTE)).ToList();
                        break;
                }

            }
            else if (sortFilterIndex == 4)
            {
                SortedIndex = sortFilterIndex;
                sortedOutputList = checkFilterdOutputList.ToList();
            }
            // var
            return sortedOutputList;
        }

        internal async void CheckByImplementation(ReportsModel reportsModel)
        {
            bool isChecked = false;

            if (reportsModel != null)
            {

                switch (Constants.UserProfile.Role)
                {
                    case "patient":
                        isChecked = !String.IsNullOrEmpty(reportsModel.ASPCHKBY);
                        break;

                    case "doctor":
                        isChecked = !String.IsNullOrEmpty(reportsModel.ASDCHKBY);
                        break;

                    case "agent":
                        isChecked = !String.IsNullOrEmpty(reportsModel.ASACHKBY);
                        break;

                    case "guarantor":
                        isChecked = !String.IsNullOrEmpty(reportsModel.ASGCHKBY);
                        break;
                }

                if (isChecked)
                {
                    var loader = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                    var result = await ReportsService.CheckReports(Constants.UserProfile.Role, reportsModel.ASBRCH, reportsModel.ASYEAR,
                        reportsModel.ASREF, "", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                    await loader.DismissAsync();

                    if (result.Success)
                    {
                        reportsModel.CheckedSource = "checkbox.png";
                        //  reportsModel.CheckDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                        //App.TabbedPage.viewModel.LabCount++;
                        //App.TabbedPage.viewModel.LabCountVisibility = App.TabbedPage.viewModel.LabCount != 0;
                        switch (Constants.UserProfile.Role)
                        {
                            case "patient":
                                reportsModel.ASPCHKBY = reportsModel.CheckDate = String.Empty;
                                break;

                            case "doctor":
                                reportsModel.ASDCHKBY = reportsModel.CheckDate = String.Empty;
                                break;

                            case "agent":
                                reportsModel.ASACHKBY = reportsModel.CheckDate = String.Empty;
                                break;

                            case "guarantor":
                                reportsModel.ASGCHKBY = reportsModel.CheckDate = String.Empty;
                                break;
                        }



                    }
                    else
                    {
                        await MaterialDialog.Instance.AlertAsync(result.Message);
                    }
                }
                else
                {
                    var loader = await MaterialDialog.Instance.LoadingDialogAsync("Loading");
                    var result = await ReportsService.CheckReports(Constants.UserProfile.Role, reportsModel.ASBRCH, reportsModel.ASYEAR,
                        reportsModel.ASREF, Constants.UserProfile.LabID, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                    await loader.DismissAsync();

                    if (result.Success)
                    {
                        reportsModel.CheckedSource = "check.png";
                        reportsModel.CheckDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");


                        //App.TabbedPage.viewModel.LabCount--;
                        //App.TabbedPage.viewModel.LabCountVisibility = App.TabbedPage.viewModel.LabCount != 0;

                        switch (Constants.UserProfile.Role)
                        {
                            case "patient":
                                reportsModel.ASPCHKBY = Constants.UserProfile.LabID;
                                break;

                            case "doctor":
                                reportsModel.ASDCHKBY = Constants.UserProfile.LabID;
                                break;

                            case "agent":
                                reportsModel.ASACHKBY = Constants.UserProfile.LabID;
                                break;

                            case "guarantor":
                                reportsModel.ASGCHKBY = Constants.UserProfile.LabID;
                                break;
                        }



                    }
                    else
                    {
                        await MaterialDialog.Instance.AlertAsync(result.Message);
                    }
                }



            }
        }
        internal async void Search(string text)
        {
            text = text.ToLower();

            LabResultList.Clear();
            if (String.IsNullOrEmpty(text))
            {
                var loader = await MaterialDialog.Instance.LoadingDialogAsync("Loading ..");
                await Initilize();
                await loader.DismissAsync();
            }
            else
            {
                PageNumber = 1;
                var mainList = SortAndFilter(CheckFilterIndex, SortedIndex, PageNumber, CalendarSelectedDates);
                var list = mainList.Where(s => s.ASREF.ToLower().Contains(text) || s.ASBRCH.ToLower().Contains(text) || s.ASYEAR.ToLower().Contains(text) ||
                                                            s.ASPATT.ToLower().Contains(text) || s.ASPATF.ToLower().Contains(text) || s.ASPATL.ToLower().Contains(text));
                list.ForEach(s => LabResultList.Add(s));
            };

        }
        public async Task Initilize()
        {
            try
            {
                UserDialogs.Instance.ShowLoading();

                PageNumber = 1;
                CheckFilterIndex = 2;
                SortedIndex = 4;
                //var result = await ReportsService.GetLabReports(Constants.UserProfile.Role, Constants.UserProfile.LabID, "L", 1);
                var result = await ReportsService.GetAllLabReports(Constants.UserProfile.Role, Constants.UserProfile.LabID, "L", CalendarSelectedDates.StartDate, CalendarSelectedDates.EndDate);
                if (result.Success)
                {
                    int unViewed = 0;
                    foreach (var reportsModel in result.data)
                    {
                        //ReportViewedModel viewedModel = new ReportViewedModel
                        //{
                        //    ReportBrch = reportsModel.ASBRCH,
                        //    ReportRef = reportsModel.ASREF,
                        //    ReportYear = reportsModel.ASYEAR
                        //};

                        //var data = Settings.LabortoryReportViewed;
                        //if (data.Any(x => x.ReportBrch == reportsModel.ASBRCH &&
                        //                  x.ReportRef == reportsModel.ASREF &&
                        //                  x.ReportYear == reportsModel.ASYEAR))
                        //{
                        //    reportsModel.IsViewed = true;
                        //}

                        bool isChecked = false;
                        string checkedOn = String.Empty;

                        switch (Constants.UserProfile.Role)
                        {
                            case "patient":
                                isChecked = !String.IsNullOrEmpty(reportsModel.ASPCHKBY);
                                checkedOn = reportsModel.ASPCHKDTE;
                                reportsModel.IsViewed = !String.IsNullOrEmpty(reportsModel.ASPVIEWED);
                                break;

                            case "doctor":
                                isChecked = !String.IsNullOrEmpty(reportsModel.ASDCHKBY);
                                checkedOn = reportsModel.ASDCHKDTE;
                                reportsModel.IsViewed = !String.IsNullOrEmpty(reportsModel.ASDVIEWED);
                                break;

                            case "agent":
                                isChecked = !String.IsNullOrEmpty(reportsModel.ASACHKBY);
                                checkedOn = reportsModel.ASACHKDTE;
                                reportsModel.IsViewed = !String.IsNullOrEmpty(reportsModel.ASAVIEWED);
                                break;

                            case "guarantor":
                                isChecked = !String.IsNullOrEmpty(reportsModel.ASGCHKBY);
                                checkedOn = reportsModel.ASGCHKDTE;
                                reportsModel.IsViewed = !String.IsNullOrEmpty(reportsModel.ASGVIEWED);
                                break;

                        }

                        reportsModel.CheckedSource = isChecked ? "check.png" : "checkbox.png";
                        reportsModel.CheckDate = isChecked ? checkedOn : String.Empty;
                    }

                    LabResultListComplete = new ObservableCollection<ReportsModel>(result.data);
                    var newList = SortAndFilter(CheckFilterIndex, SortedIndex, PageNumber, CalendarSelectedDates);
                    newList.ForEach(s => LabResultList.Add(s));
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                UserDialogs.Instance.HideLoading();
            }
        }

        byte[] HtmlByteToPdfByte(byte[] htmlByte)
        {
            string html = Encoding.ASCII.GetString(htmlByte);

            //SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
            //SelectPdf.PdfDocument doc = converter.ConvertHtmlString(html);

            Syncfusion.HtmlConverter.HtmlToPdfConverter con = new Syncfusion.HtmlConverter.HtmlToPdfConverter();
            Syncfusion.Pdf.PdfDocument pdfData = con.Convert(html);

            MemoryStream stream = new MemoryStream();

            pdfData.Save(stream);
            pdfData.Close(true);
            // Converts the PdfDocument object to byte form.
            byte[] docBytes = stream.ToArray();

            return docBytes;
        }

        #endregion

    }
}