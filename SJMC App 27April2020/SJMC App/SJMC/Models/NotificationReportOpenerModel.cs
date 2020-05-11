using System;
namespace SJMC.Models
{
    public class NotificationReportOpenerModel
    {
        public string ReportType { get; set; }
        public string AsRef { get; set; }
        public string AsBrch { get; set; }
        public string AsYear { get; set; }
        public bool IsRead { get; set; }
        public int NotificationId { get; set; }
    }

    public class NotificationNewsOpenerModel
    {
        public string NewsTitle { get; set; }
        public string NewsText { get; set; }
        public string NewsId { get; set; }
        public bool IsRead { get; set; }
        public int NotificationId { get; set; }
    }
    
}
