//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiProfileInfo
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Metadata for a WLAN profile saved on the current interface (name only — never contains the key material).</summary>
    public class WifiProfileInfo
    {
        public string ProfileName { get; internal set; }
        public bool IsGroupPolicy { get; internal set; }
        public bool IsPerUser { get; internal set; }

        public override string ToString() => ProfileName;
    }
}
