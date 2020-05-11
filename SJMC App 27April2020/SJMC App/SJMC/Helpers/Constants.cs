using SJMC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SJMC.Helpers
{
    public class Constants
    {
        public static string IsPageType = string.Empty;

        public static UserProfileModel UserProfile = null;

        public static byte[] ProfileImage = null;

        public static bool ShowNewPlayerForSplashScreen { get; set; } = false;

        public static string tempString = "";

        public static Boolean IsNotificationClickedInKillState = false;

        public static Boolean isNews = false;
        public static Boolean isReport = false;
        public static Boolean IsAppOpenFirstTime = true;


        public static string AppSettingsKey_BloodSampleNumber = "BloodSampleNumber";

        public static string AppSettingsKey_BloodSampleMessage = "BloodSampleMessage";

        //public static List<NotificationReportOpenerModel> NotificationReportOpener = null;

        public static string AndroidAppVersion = "2.0";

        public static string IOSAppVersion = "3.0";

        public static string GetAppVersion()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                return IOSAppVersion;
            }
            else
            {
                return AndroidAppVersion;
            }
        }

        public static string GetDeviceType()
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                return "iOS";
            }
            else
            {
                return "Android";
            }
        }
    }
}
