//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiCipher
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Cipher used together with a PSK protocol for a WPA-Personal connection profile.</summary>
    public enum WifiCipher
    {
        /// <summary>AES-CCMP (the WPA2/WPA3 standard cipher).</summary>
        AES,
        /// <summary>TKIP (legacy WPA cipher).</summary>
        TKIP,
    }
}
