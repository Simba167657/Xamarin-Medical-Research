using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using CommonServiceLocator;
using Firebase.Iid;
using Firebase.Messaging;
using Plugin.Badge;

namespace SJMC.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                string title = message.Data["title"];
                string text = message.Data["text"];
                if (title.Contains("|-|NEWS"))
                {
                    //Handle News Notification
                    int notiId = new Random().Next(0, 1000000);

                    string removeTitle = title.Substring(title.IndexOf("|-|NEWS"));
                    string newTitle = title.Replace(removeTitle, "");
                    newTitle = newTitle.Replace(removeTitle, "");
                    string newsId = removeTitle.Split(';')[1];

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

                    SJMC.Helpers.Settings.NewsNotificationList = data;

                    SendNotification(text, newTitle, message.Data, notiId, "SjmcNewsNotification");
                }
                else
                {
                    // Handle the Report Notification
                    string description = text.Substring(0, text.IndexOf("|-|"));
                    int startIndex = text.IndexOf("|-|") + 3;
                    string parameter = text.Substring(startIndex, (text.Length - startIndex) - 1);
                    string[] parameters = parameter.Split(";");

                    int notiId = new Random().Next(0, 1000000);

                    //SJMC.Helpers.Settings.NotificationList = new List<Models.NotificationReportOpenerModel>();
                    var data = SJMC.Helpers.Settings.NotificationList;

                    data.Add(new Models.NotificationReportOpenerModel
                    {
                        ReportType = parameters[0],
                        AsBrch = parameters[1],
                        AsYear = parameters[2],
                        AsRef = parameters[3],
                        IsRead = false,
                        NotificationId = notiId
                    });

                    SJMC.Helpers.Settings.NotificationList = data;

                    //Set Notification Badage on App
                    //SJMC.Helpers.Settings.NotificationBadageCount += 1;
                    //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);

                    //var body = message.GetNotification().Body;
                    SendNotification(description, title, message.Data, notiId, "SjmcReportNotification");
                }
            }
            catch (Exception ex)
            {

            }
        }

        void SendNotification(string messageBody, string messageTitle, IDictionary<string, string> data, int notiId, string notificationAction)
        {

            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            foreach (var key in data.Keys)
            {
                intent.PutExtra(key, data[key]);
            }

            intent.SetAction(notificationAction);
            intent.PutExtra("SjmcNotificationId", Convert.ToString(notiId));

            var pendingIntent = PendingIntent.GetActivity(this,
                                                          notiId,
                                                          intent,
                                                          PendingIntentFlags.UpdateCurrent
                                                          );

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                                      .SetSmallIcon(Resource.Drawable.ic_stat_applogo_removebg_preview)
                                      .SetContentTitle(messageTitle)
                                      .SetContentText(messageBody)
                                      .SetAutoCancel(true)
                                      .SetContentIntent(pendingIntent);

            var notificationManager = NotificationManagerCompat.From(this);
            notificationManager.Notify(notiId, notificationBuilder.Build());
            notificationManager.Dispose();
        }

        public override void OnDeletedMessages()
        {
            base.OnDeletedMessages();
        }

    }
}