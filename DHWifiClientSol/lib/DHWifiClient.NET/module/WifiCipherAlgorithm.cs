//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiCipherAlgorithm
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
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
