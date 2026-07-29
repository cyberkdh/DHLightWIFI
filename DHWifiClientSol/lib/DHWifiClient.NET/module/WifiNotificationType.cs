//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNotificationType
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Kinds of real-time WLAN state-change notifications surfaced by <see cref="DHWifiClient.Notification"/>.</summary>
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
