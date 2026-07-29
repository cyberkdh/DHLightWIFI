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
        None,
        WEP,
        TKIP,
        AES,
    }
}
