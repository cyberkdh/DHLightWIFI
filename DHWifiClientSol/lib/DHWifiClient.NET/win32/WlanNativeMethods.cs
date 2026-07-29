//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WlanNativeMethods
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.InteropServices;

namespace DHWifiClient.NET.win32
{
    /// <summary>
    /// P/Invoke declarations for the Windows Native Wifi API (wlanapi.dll).
    /// </summary>
    /// <summary>Native callback signature for WlanRegisterNotification. The delegate instance must be kept alive by the caller for the lifetime of the registration.</summary>
    internal delegate void WlanNotificationCallback(ref WlanNotificationData notificationData, IntPtr context);

    internal static class WlanNativeMethods
    {
        [DllImport("wlanapi.dll")]
        public static extern int WlanOpenHandle(
            uint dwClientVersion,
            IntPtr pReserved,
            out uint pdwNegotiatedVersion,
            out IntPtr phClientHandle);

        [DllImport("wlanapi.dll")]
        public static extern int WlanCloseHandle(
            IntPtr hClientHandle,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanEnumInterfaces(
            IntPtr hClientHandle,
            IntPtr pReserved,
            out IntPtr ppInterfaceList);

        [DllImport("wlanapi.dll")]
        public static extern int WlanGetAvailableNetworkList(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            uint dwFlags,
            IntPtr pReserved,
            out IntPtr ppAvailableNetworkList);

        /// <summary>
        /// Raw per-BSS (physical access point) scan cache query. Unlike WlanGetAvailableNetworkList, each
        /// returned entry carries its own BSSID (MAC address), which is the only way to tell apart multiple
        /// physical APs that share the same (or, for hidden networks, an empty) SSID.
        /// </summary>
        [DllImport("wlanapi.dll")]
        public static extern int WlanGetNetworkBssList(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            IntPtr pDot11Ssid,
            Dot11BssType dot11BssType,
            [MarshalAs(UnmanagedType.Bool)] bool bSecurityEnabled,
            IntPtr pReserved,
            out IntPtr ppWlanBssList);

        [DllImport("wlanapi.dll")]
        public static extern int WlanScan(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            IntPtr pDot11Ssid,
            IntPtr pIeData,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanConnect(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            [In] ref WlanConnectionParameters pConnectionParameters,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanDisconnect(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanSetProfile(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            uint dwFlags,
            [MarshalAs(UnmanagedType.LPWStr)] string strProfileXml,
            [MarshalAs(UnmanagedType.LPWStr)] string strAllUserProfileSecurity,
            [MarshalAs(UnmanagedType.Bool)] bool bOverwrite,
            IntPtr pReserved,
            out uint pdwReasonCode);

        [DllImport("wlanapi.dll")]
        public static extern int WlanDeleteProfile(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            [MarshalAs(UnmanagedType.LPWStr)] string strProfileName,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanGetProfileList(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            IntPtr pReserved,
            out IntPtr ppProfileList);

        [DllImport("wlanapi.dll", CharSet = CharSet.Unicode)]
        public static extern int WlanSetProfileEapXmlUserData(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            [MarshalAs(UnmanagedType.LPWStr)] string strProfileName,
            uint dwFlags,
            [MarshalAs(UnmanagedType.LPWStr)] string strEapXmlUserData,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern int WlanQueryInterface(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            WlanIntfOpcode opCode,
            IntPtr pReserved,
            out uint pdwDataSize,
            out IntPtr ppData,
            IntPtr pWlanOpcodeValueType);

        [DllImport("wlanapi.dll")]
        public static extern int WlanSetInterface(
            IntPtr hClientHandle,
            [In] ref Guid pInterfaceGuid,
            WlanIntfOpcode opCode,
            uint dwDataSize,
            IntPtr pData,
            IntPtr pReserved);

        [DllImport("wlanapi.dll")]
        public static extern void WlanFreeMemory(IntPtr pMemory);

        [DllImport("wlanapi.dll")]
        public static extern int WlanRegisterNotification(
            IntPtr hClientHandle,
            uint dwNotifSource,
            [MarshalAs(UnmanagedType.Bool)] bool bIgnoreDuplicate,
            WlanNotificationCallback funcCallback,
            IntPtr pCallbackContext,
            IntPtr pReserved,
            out uint pdwPrevNotifSource);
    }
}
