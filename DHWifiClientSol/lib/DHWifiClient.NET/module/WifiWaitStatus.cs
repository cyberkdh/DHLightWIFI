//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiWaitStatus
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
namespace DHWifiClient.NET.module
{
    /// <summary>Result of waiting for an asynchronous WLAN operation notification.</summary>
    public enum WifiWaitStatus
    {
        /// <summary>The expected completion notification arrived and the operation succeeded.</summary>
        Success,
        /// <summary>The operation reported a failure notification or could not be confirmed.</summary>
        Failed,
        /// <summary>No matching completion notification arrived before the timeout elapsed.</summary>
        TimedOut,
    }
}
