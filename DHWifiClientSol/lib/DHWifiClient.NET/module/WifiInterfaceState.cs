//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiInterfaceState
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>
    /// Operational state of a Wi-Fi interface, mirroring the native <c>WLAN_INTERFACE_STATE</c> values.
    /// </summary>
    public enum WifiInterfaceState
    {
        /// <summary>The interface is not ready to operate.</summary>
        NotReady,
        /// <summary>The interface is connected to a network.</summary>
        Connected,
        /// <summary>The interface has formed an ad hoc (IBSS) network.</summary>
        AdHocNetworkFormed,
        /// <summary>The interface is disconnecting from the current network.</summary>
        Disconnecting,
        /// <summary>The interface is not connected to any network.</summary>
        Disconnected,
        /// <summary>The interface is attempting to associate with a network.</summary>
        Associating,
        /// <summary>The interface is auto-configuring (discovering available networks).</summary>
        Discovering,
        /// <summary>The interface is in the process of authenticating.</summary>
        Authenticating,
    }
}
