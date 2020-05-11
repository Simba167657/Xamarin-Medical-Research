using System;
using SJMC.Interfaces;
using UIKit;

namespace SJMC.iOS.Implementation
{
    public class NotificationCountService : INotificationCountService
    {
        public NotificationCountService()
        {
        }

        public void SetCount(int count)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = count;
        }
    }
}