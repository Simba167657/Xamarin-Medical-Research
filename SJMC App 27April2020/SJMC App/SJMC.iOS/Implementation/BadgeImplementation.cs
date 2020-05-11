using System;
using Plugin.Badge.Abstractions;
using UIKit;

namespace SJMC.iOS.Implementation
{
    /// <summary>
    /// Implementation of Badge for iOS
    /// </summary>
    public class BadgeImplementation : IBadge
    {
        /// <summary>
        /// Clears the badge.
        /// </summary>
        public void ClearBadge()
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
        }

        /// <summary>
        /// Sets the badge.
        /// </summary>
        /// <param name="badgeNumber">The badge number.</param>
        /// <param name="title">The title. Used only by Android</param>
        public void SetBadge(int badgeNumber)
        {
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = badgeNumber;
        }
    }
}
