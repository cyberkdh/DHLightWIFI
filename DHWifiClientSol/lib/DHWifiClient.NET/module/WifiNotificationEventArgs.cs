//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNotificationEventArgs
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace DHWifiClient.NET
{
    public class WifiNotificationEventArgs : EventArgs
    {
        public Guid InterfaceId { get; }
        public WifiNotificationType Type { get; }

        internal WifiNotificationEventArgs(Guid interfaceId, WifiNotificationType type)
        {
            InterfaceId = interfaceId;
            Type = type;
        }
    }
}
