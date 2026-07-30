//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiException
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using DHWifiClient.NET.log;

namespace DHWifiClient.NET.module
{
    /// <summary>Exception thrown when a native <c>wlanapi.dll</c> call fails, carrying the underlying Win32 error code.</summary>
    public class WifiException : Exception
    {
        /// <summary>The Win32 error code returned by the failing native call.</summary>
        public int ErrorCode { get; }

        /// <summary>Creates an instance whose message is derived from <paramref name="win32ErrorCode"/> via <see cref="Win32Exception"/>.</summary>
        public WifiException(int win32ErrorCode)
            : base(new Win32Exception(win32ErrorCode).Message)
        {
            ErrorCode = win32ErrorCode;
        }

        /// <summary>Creates an instance with an explicit message.</summary>
        public WifiException(int win32ErrorCode, string message)
            : base(message)
        {
            ErrorCode = win32ErrorCode;
        }

        internal static void ThrowIfError(int win32ErrorCode)
        {
            if (win32ErrorCode != 0)
            {
                var oEx = new WifiException(win32ErrorCode);
                Logger.Error($"WLAN API call failed (code={win32ErrorCode})", oEx);
                throw oEx;
            }
        }
    }
}
