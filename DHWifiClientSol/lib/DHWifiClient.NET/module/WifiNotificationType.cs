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
        /// <summary>An unrecognized or unmapped native notification code.</summary>
        Unknown,
        /// <summary>A requested scan has finished.</summary>
        ScanComplete,
        /// <summary>A requested scan failed.</summary>
        ScanFailed,
        /// <summary>A connection attempt has started.</summary>
        ConnectionStarted,
        /// <summary>A connection attempt finished (success or failure; check the resulting interface state).</summary>
        ConnectionCompleted,
        /// <summary>A connection attempt failed.</summary>
        ConnectionAttemptFailed,
        /// <summary>The interface is disconnecting from its current network.</summary>
        Disconnecting,
        /// <summary>The interface has disconnected from its network.</summary>
        Disconnected,
    }
}
