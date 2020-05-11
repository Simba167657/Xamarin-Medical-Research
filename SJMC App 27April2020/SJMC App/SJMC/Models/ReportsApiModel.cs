using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.Models
{


    public class ReportsApiModel : BaseModel
    {

        public List<ReportsModel> data { get; set; }
    }


    public class Rootobject
    {
       
    }



    public class ReportsModel : ViewModelBase
    {




        public string ASBRCH { get; set; }
        public string ASYEAR { get; set; }
        public string ASREF { get; set; }
        public string ASDEPT { get; set; }
        public string ASID { get; set; }
        public string ASPWD { get; set; }
        public string ASCDTE { get; set; }
        public string ASRDTE { get; set; }
        public string ASPATT { get; set; }
        public string ASPATL { get; set; }
        public string ASPATF { get; set; }
        public string ASSEX { get; set; }
        public string ASDOCT { get; set; }
        public string ASDOCTN { get; set; }
        public string ASGUAR1 { get; set; }
        public string ASGUAR2 { get; set; }
        public string ASCATEG { get; set; }
        public string ASAGENT { get; set; }
        public string ASAGENTN { get; set; }
        public string ASDESC { get; set; }  //
        public string ASREP { get; set; }
        public string ASUPLDTE { get; set; } //
        public string ASPCHKDTE { get; set; }
        public string ASPCHKBY { get; set; }
        public string ASDCHKDTE { get; set; }
        public string ASDCHKBY { get; set; }
        public string ASGCHKDTE { get; set; }
        public string ASGCHKBY { get; set; }
        public string ASACHKDTE { get; set; }
        public string ASACHKBY { get; set; }

        public string ASPVIEWED { get; set; }
        public string ASDVIEWED { get; set; }
        public string ASGVIEWED { get; set; }
        public string ASAVIEWED { get; set; }

        private string _checkedSource;
        public string CheckedSource
        {
            get => _checkedSource;
            set
            {
                _checkedSource = value;
                RaisePropertyChanged();
            }
        }

        private string _checkDate;
        public string CheckDate
        {
            get => _checkDate;
            set
            {
                _checkDate = value;
                RaisePropertyChanged();
            }
        }

        private bool _isViewed = false;
        public bool IsViewed
        {
            get => _isViewed;
            set
            {
                _isViewed = value;
                RaisePropertyChanged();
            }
        }

    }
}
