//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiNetwork
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;

namespace DHWifiClient.NET.module
{
    /// <summary>
    /// Represents a Wi-Fi network (BSS/SSID entry) discovered by a scan, as returned by
    /// <see cref="WifiInterface.GetAvailableNetworks(bool)"/>.
    /// </summary>
    public class WifiNetwork
    {
        /// <summary>Network name (SSID). Empty for hidden (non-broadcast) networks unless already known via a saved profile.</summary>
        public string Ssid { get; internal set; }
        /// <summary>Name of the matching saved connection profile, if one exists for this network.</summary>
        public string ProfileName { get; internal set; }
        /// <summary>True if the network requires authentication/encryption (i.e. is not Open).</summary>
        public bool SecurityEnabled { get; internal set; }
        /// <summary>Authentication algorithm used by the network.</summary>
        public WifiAuthentication Authentication { get; internal set; }
        /// <summary>Cipher algorithm used by the network.</summary>
        public WifiCipherAlgorithm Cipher { get; internal set; }
        /// <summary>True if this network is the one the adapter is currently connected to.</summary>
        public bool IsConnected { get; internal set; }
        /// <summary>True if a saved connection profile already exists for this network.</summary>
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

        /// <summary>Returns the SSID.</summary>
        public override string ToString() => Ssid;
    }
}
