using System;
namespace SJMC.Models
{
    public class RootNotificationPayload
    {
        public string to { get; set; }
        public string priority { get; set; }
        public bool content_available { get; set; }
        public NotificationPayload notification { get; set; }
        public NotificationDataPayLoad data { get; set; }
    }

    public class NotificationPayload
    {
        public string body { get; set; }
        public string title { get; set; }
        public string sound { get; set; }
        public int badge { get; set; }
    }

    public class NotificationDataPayLoad
    {
        public string notifacationType { get; set; }
        public NotificationNewsPayLoad news { get; set; }
        public NotificationReportPayLoad report { get; set; }
    }

    public class NotificationNewsPayLoad
    {
        public int newsId { get; set; }
    }

    public class NotificationReportPayLoad
    {
        public string ReportType { get; set; }
        public string AsBrch { get; set; }
        public string AsYear { get; set; }
        public string AsRef { get; set; }
    }
}
