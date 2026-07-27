//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiException
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.ComponentModel;
using DHWifiClient.NET.log;

namespace DHWifiClient.NET.module
{
    public class WifiException : Exception
    {
        public int ErrorCode { get; }

        public WifiException(int win32ErrorCode)
            : base(new Win32Exception(win32ErrorCode).Message)
        {
            ErrorCode = win32ErrorCode;
        }

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
