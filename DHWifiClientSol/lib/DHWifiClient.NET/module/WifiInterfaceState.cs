//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiInterfaceState
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
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
