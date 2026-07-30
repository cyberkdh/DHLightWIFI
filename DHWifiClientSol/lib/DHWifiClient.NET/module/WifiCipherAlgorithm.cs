//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiCipherAlgorithm
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Cipher algorithm reported by a scanned network (<see cref="WifiNetwork.Cipher"/>).</summary>
    public enum WifiCipherAlgorithm
    {
        /// <summary>No cipher (unencrypted, Open network).</summary>
        None,
        /// <summary>WEP (deprecated, trivially breakable).</summary>
        WEP,
        /// <summary>TKIP (legacy WPA cipher).</summary>
        TKIP,
        /// <summary>AES-CCMP (the WPA2/WPA3 standard cipher).</summary>
        AES,
    }
}
