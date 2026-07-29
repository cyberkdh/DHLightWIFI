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
