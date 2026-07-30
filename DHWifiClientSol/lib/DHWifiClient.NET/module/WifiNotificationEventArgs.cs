//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNotificationEventArgs
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace DHWifiClient.NET.module
{
    /// <summary>Event data for <see cref="DHWifiClient.Notification"/>.</summary>
    public class WifiNotificationEventArgs : EventArgs
    {
        /// <summary>GUID of the interface the notification pertains to.</summary>
        public Guid InterfaceId { get; }
        /// <summary>Kind of notification that occurred.</summary>
        public WifiNotificationType Type { get; }

        internal WifiNotificationEventArgs(Guid interfaceId, WifiNotificationType type)
        {
            InterfaceId = interfaceId;
            Type = type;
        }
    }
}
