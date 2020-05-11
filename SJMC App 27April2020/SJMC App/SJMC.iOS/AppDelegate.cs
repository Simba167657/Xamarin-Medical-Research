using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FFImageLoading;
using FFImageLoading.Forms.Platform;
using Firebase.CloudMessaging;
using Foundation;
using Newtonsoft.Json;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using Plugin.Badge;
using Plugin.FirebasePushNotification;
using SJMC.Models;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.XForms.iOS.TabView;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using XF.Material.iOS;

namespace SJMC.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IMessagingDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Material.Init();

            if (options != null)
            {

                if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                {
                    NSDictionary userInfo = (NSDictionary)options[UIApplication.LaunchOptionsRemoteNotificationKey];
                    if (userInfo != null)
                    {
                        RecieveNotification(userInfo);
                        //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
                    }
                    //DidReceiveRemoteNotification(app, userInfo, null);
                }
            }

            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();

            var config = new FFImageLoading.Config.Configuration()
            {
                VerboseLogging = false,
                VerbosePerformanceLogging = false,
                VerboseMemoryCacheLogging = false,
                VerboseLoadingCancelledLogging = false,
                Logger = new CustomLogger(),
            };
            ImageService.Instance.Initialize(config);
            Xamarin.Forms.Forms.SetFlags("CarouselView_Experimental");
            Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            global::Xamarin.Forms.Forms.Init();
            // Add Pop up
            Rg.Plugins.Popup.Popup.Init();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MTU2NjkwQDMxMzcyZTMzMmUzMEMydm9JYmZXaEZlQllmK0dHbnp1azc5MXVTcVhaUlp6OXBOb2xWZm9URXM9");
            Syncfusion.SfPdfViewer.XForms.iOS.SfPdfDocumentViewRenderer.Init();
            Syncfusion.SfRangeSlider.XForms.iOS.SfRangeSliderRenderer.Init();
            SfCalendarRenderer.Init();
            new SfRatingRenderer();
            Xamarin.FormsGoogleMaps.Init("AIzaSyDZJDdRcGq2OvtQHO4pjLVCOJHLCw463qw");
            Xamarin.FormsMaps.Init();
            FormsVideoPlayer.Init();
            new SfTabViewRenderer();

            //            FirebasePushNotificationManager.Initialize(options, new NotificationUserCategory[]
            //{
            //new NotificationUserCategory("message",new List<NotificationUserAction> {
            //new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground)
            //}),
            //new NotificationUserCategory("request",new List<NotificationUserAction> {
            //new NotificationUserAction("Accept","Accept"),
            //new NotificationUserAction("Reject","Reject",NotificationActionType.Destructive)
            //})

            //},true);
            //            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            //            {
            //                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
            //                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
            //                {
            //                    Console.WriteLine(granted);
            //                });
            //                UNUserNotificationCenter.Current.Delegate = new MyNotificationCenterDelegate();
            //            }
            //            else
            //            {
            //                // iOS 9 <=
            //                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
            //                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
            //                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            //            }



            InitPushNotification();
            //hamad
            //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
            LoadApplication(new App());


            

            return base.FinishedLaunching(app, options);
        }

        public override void WillEnterForeground(UIApplication application)
        {
            UNUserNotificationCenter.Current.GetDeliveredNotifications(notifications =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossBadge.Current.SetBadge(notifications.Length);
                });
            });
        }

        private void RecieveNotification(NSDictionary userInfo)
        {
            SJMC.Helpers.Constants.IsNotificationClickedInKillState = true;
            SJMC.Helpers.Constants.tempString = userInfo.ToString();
            try
            {
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
                                //App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);

                            }
                            catch (Exception ex)
                            {
                                //Toast.MakeText(this, "OnNewIntent Err: " + ex.Message, ToastLength.Long);
                            }

                        // mati bhai


                        }
                        else if (SJMC.Helpers.Settings.NotificationKind == "SjmcNewsNotification")
                        {
                            /*string kkk = intent.GetStringExtra("SjmcNotificationId");
                            SJMC.Helpers.Settings.OpenNewsNumber = Convert.ToInt32(kkk);
                            SJMC.Helpers.Settings.NewsNotificationOpen = true;

                            FirebasePushNotificationManager.ProcessIntent(this, intent);*/
                            SJMC.Helpers.Settings.NewsNotificationOpen = true;
                            //App.Current.MainPage.Navigation.PushAsync(new SJMC.Views.HomeView(), true);
                        }
                        SJMC.Helpers.Settings.NotificationKind = "";

                        //base.DidReceiveNotificationResponse(center, response, completionHandler);

                    }
                }
                catch (Exception ex)
                {

                }

            
        }

        private void InitPushNotification()
        {
            try
            {
                Firebase.Core.App.Configure();
            }
            catch(Exception ex)
            {

            }
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                //UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Badge | UIUserNotificationType.Sound, new Foundation.NSSet()));
                UIApplication.SharedApplication.RegisterUserNotificationSettings(UIUserNotificationSettings.GetSettingsForTypes( UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                       new NSSet()));
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            UIApplication.SharedApplication.SetMinimumBackgroundFetchInterval(UIApplication.BackgroundFetchIntervalMinimum);

            Messaging.SharedInstance.Delegate = this;
            Messaging.SharedInstance.ShouldEstablishDirectChannel = true;

            UNUserNotificationCenter.Current.Delegate = new iOSNotificationReceiver();
        }

        [Export("messaging:didReceiveRegistrationToken:")]
        public void DidReceiveRegistrationToken(Messaging messaging, string fcmToken)
        {
            SJMC.Helpers.Settings.FirebaseToken = fcmToken;
        }

        [Export("messaging:didReceiveMessage:")]
        public void DidReceiveMessage(Messaging messaging, RemoteMessage message)
        {
            try
            {//var data = remoteMessage.AppData;
                string title = message.AppData.ObjectForKey(new NSString("title")).ToString();
                string text = message.AppData.ObjectForKey(new NSString("text")).ToString();
                var notificationType = message.AppData.ObjectForKey(new NSString("notificationType")).ToString();
                if (notificationType.Contains("NEWS"))
                {
                    //Handle News Notification
                    int notiId = new Random().Next(0, 1000000);

                    string newTitle = title;
                    string newsJson = message.AppData.ObjectForKey(new NSString("news")).ToString(); //"123";//noti_id.ObjectForKey(new NSString("newsId")).ToString();

                    NotificationNewsPayLoad notificationNewsPayLoad = JsonConvert.DeserializeObject<NotificationNewsPayLoad>(newsJson);

                    string newsId = notificationNewsPayLoad.newsId.ToString();

                    //string removeTitle = title.Substring(title.IndexOf("|-|NEWS"));
                    //string newTitle = title.Replace(removeTitle, "");
                    //string newsId = removeTitle.Split(';')[1];

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

                    //hamad
                    //SJMC.Helpers.Settings.NotificationBadageCount += 1;
                    //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);

                    SJMC.Helpers.Settings.NotificationKind = "SjmcNewsNotification";
                    SendNotification(text, newTitle, message.AppData, notiId, "SjmcNewsNotification");
                }
                else
                {
                    if (SJMC.Helpers.Settings.Email == "")
                    {
                        return;
                    }

                    string reportJson = message.AppData.ObjectForKey(new NSString("report")).ToString(); //"123";//noti_id.ObjectForKey(new NSString("newsId")).ToString();

                    NotificationReportPayLoad notificationReportPayLoad = JsonConvert.DeserializeObject<NotificationReportPayLoad>(reportJson);

                    // Handle the Report Notification
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

                    /*data.Add(new Models.NotificationReportOpenerModel
                    {
                        ReportType = parameters[0],
                        AsBrch = parameters[1],
                        AsYear = parameters[2],
                        AsRef = parameters[3],
                        IsRead = false,
                        NotificationId = notiId
                    });*/

                    SJMC.Helpers.Settings.NotificationList = data;
                    SJMC.Helpers.Settings.OpenReportNumber = notiId;
                    //Set Notification Badage on App
                    //hamad
                    //SJMC.Helpers.Settings.NotificationBadageCount += 1;
                    //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
                    SJMC.Helpers.Settings.NotificationKind = "SjmcReportNotification";
                    //var body = message.GetNotification().Body;
                    SendNotification(text, title, message.AppData, notiId, "SjmcReportNotification");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("AppDelegate DidReceiveMessage Exception "+ ex.Message);
            }
            
        }


        [Export("application:supportedInterfaceOrientationsForWindow:")]
        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations(UIApplication application, UIWindow forWindow)
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        void SendNotification(string messageBody, string messageTitle, NSDictionary data, int notiId, string notificationAction)
        {


            /*var intent = new Intent(this, typeof(MainActivity));
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
            notificationManager.Dispose();*/

            var content = new UNMutableNotificationContent()
            {
                Title = "testing",
                Subtitle = "",
                Body = messageBody,
                Badge = SJMC.Helpers.Settings.NotificationBadageCount,
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false);
            var request = UNNotificationRequest.FromIdentifier(notiId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
            });


            UNUserNotificationCenter.Current.GetPendingNotificationRequests((result) =>
            {
                Task.Run(() =>
                {
                    // do work here that you don't want on the UI thread

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        //nint number = UIApplication.SharedApplication.ApplicationIconBadgeNumber;
                        //number += result.Length;
                        //SJMC.Helpers.Settings.NotificationBadageCount
                        //hamad
                        //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
                    });
                });
            });

            
        }

        [Export("userNotificationCenter:didReceiveNotificationResponse:withCompletionHandler:")]
        public void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        {
            completionHandler();

        }

        public class CustomLogger : FFImageLoading.Helpers.IMiniLogger
        {
            public void Debug(string message)
            {
                Console.WriteLine(message);
            }

            public void Error(string errorMessage)
            {
                Console.WriteLine(errorMessage);
            }

            public void Error(string errorMessage, Exception ex)
            {
                Error(errorMessage + System.Environment.NewLine + ex.ToString());
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //base.RegisteredForRemoteNotifications(application, deviceToken);
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //base.FailedToRegisterForRemoteNotifications(application, error);
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
            //Serial: F17VKPZJJCL7 · UDID: cd897aca037df9d0f50973a04950dcaa49423196 · Model: iPhone10,3
        }


        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            //base.DidReceiveRemoteNotification(application, userInfo, completionHandler);
            try
            {
                FirebasePushNotificationManager.DidReceiveMessage(userInfo);
                System.Console.WriteLine(userInfo);

                if(completionHandler != null)
                {
                    completionHandler(UIBackgroundFetchResult.NewData);
                }
                

                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var newtitle = alert_d["title"] as NSString;

                var title = newtitle.ToString();

                if (SJMC.Helpers.Settings.NotificationBadageCount < 0)
                {
                    SJMC.Helpers.Settings.NotificationBadageCount = 0;
                }

                //hamad
                //SJMC.Helpers.Settings.NotificationBadageCount += 1;
                //CrossBadge.Current.SetBadge(SJMC.Helpers.Settings.NotificationBadageCount);
            }
            catch(Exception ex)
            {
                Console.WriteLine("AppDelegate DidReceiveRemoteNotification Exception " + ex.Message);
            }

            UNUserNotificationCenter.Current.GetDeliveredNotifications(notifications =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CrossBadge.Current.SetBadge(notifications.Length);
                });
            });
        }


        
        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        //}

        //public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        //{
        //    FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);

        //}
        //// To receive notifications in foregroung on iOS 9 and below.
        //// To receive notifications in background in any iOS version
        //public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        //{
        //    // If you are receiving a notification message while your app is in the background,
        //    // this callback will not be fired 'till the user taps on the notification launching the application.

        //    // If you disable method swizzling, you'll need to call this method.
        //    // This lets FCM track message delivery and analytics, which is performed
        //    // automatically with method swizzling enabled.
        //    FirebasePushNotificationManager.DidReceiveMessage(userInfo);
        //    // Do your magic to handle the notification data
        //    System.Console.WriteLine(userInfo);

        //    completionHandler(UIBackgroundFetchResult.NewData);

        //    UIApplication.SharedApplication.ApplicationIconBadgeNumber = 3;


        //    if (application.ApplicationState == UIApplicationState.Active)
        //    {
        //        var aps_d = userInfo["aps"] as NSDictionary;
        //        var alert_d = aps_d["alert"] as NSString;

        //        var NotificationData = alert_d.ToString();

        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            var result = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Broom Service", NotificationData, "Ok", "Cancel"); // since we are using async, we should specify the DisplayAlert as awaiting.
        //            if (result) // if it's equal to Ok
        //            {
        //                //if (NotificationData.StartsWith("You have a new message"))
        //                //{
        //                //    Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new HomeTabPage());
        //                //    _ = Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new ChatListPage());
        //                //}
        //                //else
        //                //{
        //                //    Xamarin.Forms.Application.Current.MainPage = new NavigationPage(new HomeTabPage());
        //                //    MessagingCenter.Send("Notification_Tab", "HomeTabBar");
        //                //}
        //            }
        //            else // if it's equal to Cancel
        //            {
        //                return;
        //            }
        //        });


        //    }




        //    ////Create Alert
        //    //var okCancelAlertController = UIAlertController.Create("Broom Service", "Choose from two buttons", UIAlertControllerStyle.Alert);

        //    ////Add Actions
        //    //okCancelAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, alert => Console.WriteLine("Okay was clicked")));
        //    //okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, alert => Console.WriteLine("Cancel was clicked")));

        //    ////Present Alert
        //    //PresentViewController(okCancelAlertController, true, null);
        //}

        //// iOS 10, fire when recieve notification foreground
        //[Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
        //public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        //{
        //    var title = notification.Request.Content.Title;
        //    var body = notification.Request.Content.Body;
        //    completionHandler(UNNotificationPresentationOptions.Alert);
        //    //debugAlert(title, body);
        //}


        //public class MyNotificationCenterDelegate : UNUserNotificationCenterDelegate
        //{
        //    public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
        //    {
        //        //if (App.badgeCount > 0)
        //        //{
        //        // App.badgeCount = App.badgeCount - 1;
        //        // CrossBadge.Current.SetBadge(App.badgeCount);
        //        //}
        //        var aps = response.Notification.Request.Content.UserInfo.
        //        ObjectForKey(new NSString("aps")) as NSDictionary;

        //        var type = response.Notification.Request.Content.UserInfo.
        //        ObjectForKey(new NSString("gcm.notification.type")) as NSString;

        //        if (type == "5")
        //        {
        //            //MainTabbedPage._isAskMsg = true;
        //            //App.Current.MainPage = new NavigationPage(new MainTabbedPage());
        //        }
        //        else if (type == "6") //quiz notification
        //        {
        //            //MainTabbedPage._isAskMsg = false;
        //            //App.Current.MainPage = new NavigationPage(new MainTabbedPage());
        //        }
        //        else
        //        {
        //            //App.Current.MainPage.Navigation.PushAsync(new AlertsPage());
        //        }

        //        completionHandler();
        //    }

        //    public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        //    {
        //        completionHandler(UNNotificationPresentationOptions.Sound | UNNotificationPresentationOptions.Alert | UNNotificationPresentationOptions.Badge);
        //    }
        //}
    }
}
