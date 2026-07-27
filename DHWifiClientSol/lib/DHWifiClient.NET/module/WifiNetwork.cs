//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNetwork
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace DHWifiClient.NET.module
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

        /// <summary>
        /// BSSID(s) (access point MAC addresses, e.g. "AA:BB:CC:DD:EE:FF") of the physical BSS(es) this
        /// entry represents. Best-effort: populated via a separate WlanGetNetworkBssList query, so it may
        /// be empty if that query fails. Since SSID text alone cannot tell apart multiple physical APs
        /// sharing the same name (or, for hidden networks, an empty name), this is the field to use for
        /// that purpose.
        /// </summary>
        public IReadOnlyList<string> Bssids { get; internal set; } = System.Array.Empty<string>();

        public override string ToString() => Ssid;
    }
}
