//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiRadioState
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Radio state of a WiFi adapter.</summary>
    public enum WifiRadioState
    {
        Unknown,
        On,

        /// <summary>Off if either the software switch or the hardware switch (e.g. airplane mode) is off.</summary>
        Off,
    }
}
