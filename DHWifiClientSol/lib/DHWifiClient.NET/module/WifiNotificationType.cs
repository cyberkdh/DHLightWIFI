//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNotificationType
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET
{
    /// <summary>Kinds of real-time WLAN state-change notifications surfaced by <see cref="WifiClient.Notification"/>.</summary>
    public enum WifiNotificationType
    {
        Unknown,
        ScanComplete,
        ScanFailed,
        ConnectionStarted,
        ConnectionCompleted,
        ConnectionAttemptFailed,
        Disconnecting,
        Disconnected,
    }
}
