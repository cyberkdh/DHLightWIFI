//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiAuthentication
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>
    /// Wi-Fi authentication algorithm, mirroring the native <c>DOT11_AUTH_ALGORITHM</c> values.
    /// </summary>
    public enum WifiAuthentication
    {
        /// <summary>Open System authentication (no authentication, used with Open or WEP encryption).</summary>
        Open,
        /// <summary>Shared Key authentication (used with WEP encryption).</summary>
        SharedKey,
        /// <summary>WPA authentication using an 802.1X authentication server (Enterprise).</summary>
        WPA,
        /// <summary>WPA-PSK (Pre-Shared Key / Personal) authentication.</summary>
        WPA_PSK,
        /// <summary>WPA-None authentication, used only for IBSS (ad hoc) networks.</summary>
        WPA_None,
        /// <summary>WPA2 (RSNA) authentication using an 802.1X authentication server (Enterprise).</summary>
        RSNA,
        /// <summary>WPA2-PSK (RSNA-PSK / Personal) authentication.</summary>
        RSNA_PSK,
        /// <summary>WPA3 authentication using an 802.1X authentication server (Enterprise).</summary>
        WPA3,
        /// <summary>WPA3-SAE (Simultaneous Authentication of Equals / Personal) authentication.</summary>
        WPA3_SAE,
        /// <summary>WPA3-Enterprise 192-bit authentication.</summary>
        WPA3_ENT,
    }
}
