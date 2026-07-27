//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiInterfaceState
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    public enum WifiInterfaceState
    {
        NotReady,
        Connected,
        AdHocNetworkFormed,
        Disconnecting,
        Disconnected,
        Associating,
        Discovering,
        Authenticating,
    }
}
