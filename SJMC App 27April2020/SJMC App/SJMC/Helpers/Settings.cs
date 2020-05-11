using System;
using System.Collections.Generic;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SJMC.Models;

namespace SJMC.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        private const string UsernameKey = "Usernamekey";
        private static readonly string UsernameDefault = string.Empty;

        private const string LabIdKey = "LabIdKey";
        private static readonly string LabIdDefault = string.Empty;

        private const string PasswordsKey = "Passwordskey";
        private static readonly string PasswordsDefault = string.Empty;

        private const string RoleKey = " RoleKey";
        private static readonly string RoleDefault = string.Empty;

        private const string FirebaseTokenKey = " FirebaseTokenKey";
        private static readonly string FirebaseTokenDefault = string.Empty;


        private const string CountriesModelKey = "CountriesModelKey";
        private static readonly string CountriesModelDefault = string.Empty;

        private const string LangModelKey = "LangModelkey";
        private static readonly string LangModelDefault = string.Empty;

        private const string ProviderKey = "ProviderKey";
        private static readonly string ProviderDefault = string.Empty;

        private const string OpenReportPageKey = "OpenReportKey";
        private static readonly bool OpenReportPageDefault = false;

        private const string IsRememberKey = "IsRememberKey";
        private static readonly bool IsRememberDefault = false;

        private const string ProfilePhotoKey = "ProfilePhotoKey";
        private static readonly string ProfilePhotoDefault = string.Empty;

        private const string NameKey = "NameKey";
        private static readonly string NameDefault = string.Empty;

        private const string UserIdKey = "UserIdKey";
        private static readonly string UserIdDefault = string.Empty;

        private const string UserPhoneKey = "UserPhoneKey";
        private static readonly string UserPhoneDefault = string.Empty;
        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault(UsernameKey, UsernameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UsernameKey, value);
            }
        }

        public static string Phone
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserPhoneKey, UserPhoneDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserPhoneKey, value);
            }
        }

        public static string LabId
        {
            get
            {
                return AppSettings.GetValueOrDefault(LabIdKey, LabIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LabIdKey, value);
            }
        }

        public static string Name
        {
            get
            {
                return AppSettings.GetValueOrDefault(NameKey, NameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(NameKey, value);
            }
        }

        public static string UserID
        {
            get
            {
                return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(UserIdKey, value);
            }
        }


        public static string Password
        {
            get
            {
                return AppSettings.GetValueOrDefault(PasswordsKey, PasswordsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(PasswordsKey, value);
            }
        }


        public static string Role
        {
            get
            {
                return AppSettings.GetValueOrDefault(RoleKey, RoleDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(RoleKey, value);
            }
        }


        public static string FirebaseToken
        {
            get
            {
                //return AppSettings.GetValueOrDefault(FirebaseTokenKey, FirebaseTokenDefault);
                string token = AppSettings.GetValueOrDefault(FirebaseTokenKey, FirebaseTokenDefault);
                if (string.IsNullOrEmpty(token))
                {
                    String authorizedEntity = "267961763454";
                    String scope = "GCM";
                    token = authorizedEntity + "&" + scope;
                }
                return token;
            }
            set
            {
                AppSettings.AddOrUpdateValue(FirebaseTokenKey, value);
            }
        }



        public static string CountriesModel
        {
            get
            {
                return AppSettings.GetValueOrDefault(CountriesModelKey, CountriesModelDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(CountriesModelKey, value);
            }
        }

        public static string LangModel
        {
            get
            {
                return AppSettings.GetValueOrDefault(LangModelKey, LangModelDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(LangModelKey, value);
            }
        }

        public static string Provider
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProviderKey, ProviderDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProviderKey, value);
            }
        }

        public static bool OpenReportPage
        {
            get
            {
                return AppSettings.GetValueOrDefault(OpenReportPageKey, OpenReportPageDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(OpenReportPageKey, value);
            }
        }

        public static bool IsRemember
        {
            get
            {
                return AppSettings.GetValueOrDefault(IsRememberKey, IsRememberDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(IsRememberKey, value);
            }
        }

        public static string ProfilePhoto
        {
            get
            {
                return AppSettings.GetValueOrDefault(ProfilePhotoKey, ProfilePhotoDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(ProfilePhotoKey, value);
            }
        }







        #region NotificationReportOpenerModel
        public static List<NotificationReportOpenerModel> NotificationList
        {
            get
            {
                return SettingsHelper<NotificationReportOpenerModel>.StringJsonToList(AppSettings.GetValueOrDefault("NotificationList",
                    SettingsHelper<NotificationReportOpenerModel>.StringJsonToListDefault()));
            }
            set
            {
                AppSettings.AddOrUpdateValue("NotificationList", SettingsHelper<NotificationReportOpenerModel>.ListToStringJson(value));
            }
        }
        public static int OpenReportNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault("OpenReportNumber", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("OpenReportNumber", value);
            }
        }
        #endregion



        #region NotificationNewsOpenerModel
        public static List<NotificationNewsOpenerModel> NewsNotificationList
        {
            get
            {
                return SettingsHelper<NotificationNewsOpenerModel>.StringJsonToList(AppSettings.GetValueOrDefault("NotificationList",
                    SettingsHelper<NotificationNewsOpenerModel>.StringJsonToListDefault()));
            }
            set
            {
                AppSettings.AddOrUpdateValue("NotificationList", SettingsHelper<NotificationNewsOpenerModel>.ListToStringJson(value));
            }
        }
        public static int OpenNewsNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault("OpenNewsNumber", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("OpenNewsNumber", value);
            }
        }
        public static bool NewsNotificationOpen
        {
            get
            {
                return AppSettings.GetValueOrDefault("NewsNotificationOpen", false);
            }
            set
            {
                AppSettings.AddOrUpdateValue("NewsNotificationOpen", value);
            }
        }
        #endregion

        public static string NotificationKind = "";


        #region Report Viewed
        //public static List<ReportViewedModel> LabortoryReportViewed
        //{
        //    get
        //    {
        //        return SettingsHelper<ReportViewedModel>.StringJsonToList(AppSettings.GetValueOrDefault("LabortoryReportViewed",
        //            SettingsHelper<ReportViewedModel>.StringJsonToListDefault()));
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("LabortoryReportViewed", SettingsHelper<ReportViewedModel>.ListToStringJson(value));
        //        //NotificationBadageCount = LabortoryReportViewed.Count + RadiologyReportViewed.Count;
        //    }
        //}

        //public static List<ReportViewedModel> RadiologyReportViewed
        //{
        //    get
        //    {
        //        return SettingsHelper<ReportViewedModel>.StringJsonToList(AppSettings.GetValueOrDefault("RadiologyReportViewed",
        //            SettingsHelper<ReportViewedModel>.StringJsonToListDefault()));
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("RadiologyReportViewed", SettingsHelper<ReportViewedModel>.ListToStringJson(value));
        //        //NotificationBadageCount = LabortoryReportViewed.Count + RadiologyReportViewed.Count;
        //    }
        //}

        public static int NotificationBadageCount
        {
            get
            {
                return AppSettings.GetValueOrDefault("NotificationBadageCount", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("NotificationBadageCount", value);
            }
        }

        //public static int TotalLabortoryReportsCount
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault("TotalLabortoryReportsCount", 0);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("TotalLabortoryReportsCount", value);
        //    }
        //}

        //public static int TotalRadiologyReportsCount
        //{
        //    get
        //    {
        //        return AppSettings.GetValueOrDefault("TotalRadiologyReportsCount", 0);
        //    }
        //    set
        //    {
        //        AppSettings.AddOrUpdateValue("TotalRadiologyReportsCount", value);
        //    }
        //}

        public static int TotalLabortoryReportsUnViewed
        {
            get
            {
                return AppSettings.GetValueOrDefault("TotalLabortoryReportsUnViewed", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("TotalLabortoryReportsUnViewed", value);
                //Plugin.Badge.CrossBadge.Current.SetBadge(TotalLabortoryReportsUnViewed + TotalRadiologyReportsUnViewed);
                //if (TotalLabortoryReportsUnViewed + TotalRadiologyReportsUnViewed <= 0)
                //{
                //    Plugin.Badge.CrossBadge.Current.ClearBadge();
                //}
            }
        }
        public static int TotalRadiologyReportsUnViewed
        {
            get
            {
                return AppSettings.GetValueOrDefault("TotalRadiologyReportsUnViewed", 0);
            }
            set
            {
                AppSettings.AddOrUpdateValue("TotalRadiologyReportsUnViewed", value);
                //Plugin.Badge.CrossBadge.Current.SetBadge(TotalLabortoryReportsUnViewed + TotalRadiologyReportsUnViewed);
                //if (TotalLabortoryReportsUnViewed + TotalRadiologyReportsUnViewed <= 0)
                //{
                //    Plugin.Badge.CrossBadge.Current.ClearBadge();
                //}
            }
        }

        public static string AppSettings_BloodSampleMessage
        {
            get
            {
                return AppSettings.GetValueOrDefault("AppSettings_BloodSampleMessage", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AppSettings_BloodSampleMessage", value);
            }
        }
        public static string AppSettings_BloodSampleNumber
        {
            get
            {
                return AppSettings.GetValueOrDefault("AppSettings_BloodSampleNumber", "");
            }
            set
            {
                AppSettings.AddOrUpdateValue("AppSettings_BloodSampleNumber", value);
            }
        }
        #endregion

    }
}
