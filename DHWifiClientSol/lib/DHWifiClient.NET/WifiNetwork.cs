//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNetwork
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET
{
    public class WifiNetwork
    {
        public string Ssid { get; internal set; }
        public string ProfileName { get; internal set; }
        public bool SecurityEnabled { get; internal set; }
        public WifiAuthentication Authentication { get; internal set; }
        public WifiCipherAlgorithm Cipher { get; internal set; }
        public bool IsConnected { get; internal set; }
        public bool HasProfile { get; internal set; }

        /// <summary>Signal quality, ranging from 0 to 100.</summary>
        public uint SignalQuality { get; internal set; }

        public override string ToString() => Ssid;
    }
}
