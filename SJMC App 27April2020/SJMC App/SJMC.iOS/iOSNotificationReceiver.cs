using System;
using Foundation;
using Newtonsoft.Json;
using Plugin.Badge;
using SJMC.Models;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

namespace SJMC.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
            //base.WillPresentNotification(center, notification, completionHandler);
        }


        public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response,  Action completionHandler)
        {

            try
            {
                var userInfo = response.Notification.Request.Content.UserInfo as NSDictionary;

                if (userInfo["title"] != null)
                {
                    Console.WriteLine("Title is :" + userInfo["title"]);
                }

                if (userInfo["aps"] != null)
                {

                    var aps_d = userInfo["aps"] as NSDictionary;

                    //rootNotificationPayload = JsonConvert.DeserializeObject<RootNotificationPayload>(userInfo.ToString());

                    var alert_d = aps_d["alert"] as NSDictionary;
                    var text = alert_d.ObjectForKey(new NSString("body")).ToString();
                    var newTitles = alert_d["title"] as NSString;
                    var notificationType = userInfo.ObjectForKey(new NSString("notificationType")).ToString();

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        // do work here that you don't want on the UI thread

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            //hamad
                            //SJMC.Helpers.Settings.NotificationBadageCount -= 1;
                            //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
                        });
                    });

                    if (SJMC.Helpers.Settings.NotificationBadageCount < 0)
                    {
                        SJMC.Helpers.Settings.NotificationBadageCount = 0;
                    }

                    if (notificationType.Contains("NEWS"))
                    {
                        int notiId = new Random().Next(0, 1000000);
                        string newTitle = newTitles;
                        string newsJson = userInfo.ObjectForKey(new NSString("news")).ToString(); //"123";//noti_id.ObjectForKey(new NSString("newsId")).ToString();

                        NotificationNewsPayLoad notificationNewsPayLoad = JsonConvert.DeserializeObject<NotificationNewsPayLoad>(newsJson);

                        string newsId = notificationNewsPayLoad.newsId.ToString();

                        SJMC.Helpers.Settings.NewsNotificationOpen = true;

                        var data = SJMC.Helpers.Settings.NewsNotificationList;

                        data.Add(new Models.NotificationNewsOpenerModel
                        {
                            NewsId = newsId,
                            NewsTitle = newTitle,
                            NewsText = text,
                            IsRead = false,
                            NotificationId = notiId
                        });
                        SJMC.Helpers.Settings.OpenNewsNumber = notiId;
                        SJMC.Helpers.Settings.NewsNotificationList = data;
                        SJMC.Helpers.Settings.NotificationKind = "SjmcNewsNotification";



                    }
                    else
                    {

                        string reportJson = userInfo.ObjectForKey(new NSString("report")).ToString(); //"123";//noti_id.ObjectForKey(new NSString("newsId")).ToString();

                        NotificationReportPayLoad notificationReportPayLoad = JsonConvert.DeserializeObject<NotificationReportPayLoad>(reportJson);

                        //string description = text.Substring(0, text.IndexOf("|-|"));
                        //int startIndex = text.IndexOf("|-|") + 3;
                        //string parameter = text.Substring(startIndex, (text.Length - startIndex) - 1);
                        //string[] parameters = parameter.Split(";");

                        int notiId = new Random().Next(0, 1000000);

                        //SJMC.Helpers.Settings.NotificationList = new List<Models.NotificationReportOpenerModel>();
                        var data = SJMC.Helpers.Settings.NotificationList;

                        NotificationReportOpenerModel notificationReportOpenerModel = new NotificationReportOpenerModel()
                        {
                            ReportType = notificationReportPayLoad.ReportType,//parameters[0],
                            AsBrch = notificationReportPayLoad.AsBrch,//parameters[1],
                            AsYear = notificationReportPayLoad.AsYear,//parameters[2],
                            AsRef = notificationReportPayLoad.AsRef,//parameters[3],
                            IsRead = false,
                            NotificationId = notiId
                        };


                        data.Add(new Models.NotificationReportOpenerModel
                        {
                            ReportType = notificationReportPayLoad.ReportType,//parameters[0],
                            AsBrch = notificationReportPayLoad.AsBrch,//parameters[1],
                            AsYear = notificationReportPayLoad.AsYear,//parameters[2],
                            AsRef = notificationReportPayLoad.AsRef,//parameters[3],
                            IsRead = false,
                            NotificationId = notiId
                        });

                        SJMC.Helpers.Settings.NotificationList = data;
                        SJMC.Helpers.Settings.OpenReportNumber = notiId;
                        SJMC.Helpers.Settings.NotificationKind = "SjmcReportNotification";
                    }

                    //base.DidReceiveNotificationResponse(center, response, completionHandler);
                    if (SJMC.Helpers.Settings.NotificationKind == "SjmcReportNotification")
                    {
                        try
                        {

                            /// Do something now that you know the user clicked on the notification...
                            //CrossBadge.Current.ClearBadge();

                            //SJMC.Helpers.Settings.OpenReportPage = true;

                            /*string kkk = intent.GetStringExtra("SjmcNotificationId");
                            SJMC.Helpers.Settings.OpenReportNumber = Convert.ToInt32(kkk);

                            var winnerToast = Toast.MakeText(this, $"Opening your report...", ToastLength.Long);
                            winnerToast.SetGravity(Android.Views.GravityFlags.Bottom, 0, 0);
                            winnerToast.Show();

                            // App.Current.MainPage = new SJMC.Views.HomeView();
                            // App.Current.MainPage = new NavigationPage(new HomeView());

                            //Toast.MakeText(this, "kkk: " + kkk, ToastLength.Long);

                            FirebasePushNotificationManager.ProcessIntent(this, intent);
                            //App.Current.MainPage = new SJMC.Views.HomeView();*/
                            App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);

                        }
                        catch (Exception ex)
                        {
                            //Toast.MakeText(this, "OnNewIntent Err: " + ex.Message, ToastLength.Long);
                        }

                    }
                    else if (SJMC.Helpers.Settings.NotificationKind == "SjmcNewsNotification")
                    {
                        /*string kkk = intent.GetStringExtra("SjmcNotificationId");
                        SJMC.Helpers.Settings.OpenNewsNumber = Convert.ToInt32(kkk);
                        SJMC.Helpers.Settings.NewsNotificationOpen = true;

                        FirebasePushNotificationManager.ProcessIntent(this, intent);*/
                        SJMC.Helpers.Settings.NewsNotificationOpen = true;
                        App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);
                    }
                    SJMC.Helpers.Settings.NotificationKind = "";

                    //base.DidReceiveNotificationResponse(center, response, completionHandler);

                }
            }
            catch(Exception ex)
            {

            }

        }
    }
}
