//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiProfileInfo
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Metadata for a WLAN profile saved on the current interface (name only — never contains the key material).</summary>
    public class WifiProfileInfo
    {
        /// <summary>Name of the saved profile (typically the SSID).</summary>
        public string ProfileName { get; internal set; }
        /// <summary>True if the profile was provisioned via Group Policy.</summary>
        public bool IsGroupPolicy { get; internal set; }
        /// <summary>True if the profile is scoped to the current user rather than all users of the machine.</summary>
        public bool IsPerUser { get; internal set; }

        /// <summary>Returns the profile name.</summary>
        public override string ToString() => ProfileName;
    }
}
