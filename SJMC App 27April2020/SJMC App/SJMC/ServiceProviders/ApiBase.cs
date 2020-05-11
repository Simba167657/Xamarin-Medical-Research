using System;
using System.Collections.Generic;
using System.Text;

namespace SJMC.ServiceProviders
{
    public class ApiBase
    {

        public static HttpClientBase HttpClientBase = new HttpClientBase();

        public const string UpdateProfileKey = "api/Profile/UpdateProfile?role={0}&userId={1}&name={2}&email={3}&phoneNumber={4}&password={5}&notify={6}";

        public const string GetProfileKey = "api/Profile/GetProfilePicture?labId={0}";

        public const string LoginKey = "api/Auth/Login?userInfo={0}&password={1}";

        public const string GetReportsKey = "api/Reports/GetReports?role={0}&id={1}&labType={2}&pageNumber={3}";

        public const string GetAllReportsKey = "api/Reports/GetAllReports?role={0}&id={1}&labType={2}&startDate={3}&endDate={4}";

        public const string CheckReportsKey = "api/Reports/CheckReports?role={0}&branch={1}&year={2}&refernce={3}&id={4}&date={5}";

        //   public const string DownloadReportKey = "v1/generate?apiKey=4959eb261dd4dc5c9c443a5d2a4957462f21d387e1b4618d090841ac8b2c4c72";

        public const string GetFirebaseTokenKey = "api/FirebaseTokens/GetFirebaseToken?labId={0}&type={1}";

        public const string InsertFirebaseTokenKey = "api/FirebaseTokens/InsertFirebaseToken?labId={0}&type={1}&token={2}&isAndroid={3}&dateUpdated={4}";

        public const string GetAllNewsKey = "api/News/GetAllNews";

        public const string AppSettingsKey = "api/AppSettings/GetAppSettings?settingKey={0}";

        public const string AppLogout = "api/FirebaseTokens/Logout?labId={0}&type={1}";

        public const string GetLabReportsCountKey = "api/Reports/GetLabReportsCount?role={0}&id={1}";

        public const string SetSeenReportsKey = "api/Reports/SetSeenReports?role={0}&branch={1}&year={2}&refernce={3}&isViewed={4}";

        public const string DownloadReportKey = "api/Reports/DownloadReportBytes?refernce={0}";

        public const string GetAttachmentsListKey = "api/Reports/GetAttachmentsList?name={0}";

        public const string GetAttachmentKey = "api/Reports/GetAttachment?name={0}";

        public const string PostFileUploadKey = "api/Profile/PostFileUpload";

        public const string UpdateAppRatingKey = "api/Profile/UpdateAppRating?role={0}&userId={1}&rating={2}";

        public const string AppBadgeClear = "api/FirebaseTokens/ClearCount?labId={0}&type={1}";

        public const string AppVersion = "api/AppVersion/CheckLatest?platform={0}&version={1}";

    }
}
