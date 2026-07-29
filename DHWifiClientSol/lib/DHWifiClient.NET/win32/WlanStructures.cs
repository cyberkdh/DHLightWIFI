//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WlanStructures
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Runtime.InteropServices;

namespace DHWifiClient.NET.win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Dot11Ssid
    {
        public uint SsidLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] SsidBytes;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WlanInterfaceInfo
    {
        public Guid InterfaceGuid;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string InterfaceDescription;

        public WlanInterfaceState State;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanInterfaceInfoListHeader
    {
        public uint NumberOfItems;
        public uint Index;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WlanAvailableNetwork
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ProfileName;

        public Dot11Ssid Dot11Ssid;
        public Dot11BssType Dot11BssType;
        public uint NumberOfBssids;
        [MarshalAs(UnmanagedType.Bool)]
        public bool NetworkConnectable;
        public uint NotConnectableReason;
        public uint NumberOfPhyTypes;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public uint[] Dot11PhyTypes;

        [MarshalAs(UnmanagedType.Bool)]
        public bool MorePhyTypes;
        public uint SignalQuality;
        [MarshalAs(UnmanagedType.Bool)]
        public bool SecurityEnabled;
        public Dot11AuthAlgorithm Dot11DefaultAuthAlgorithm;
        public Dot11CipherAlgorithm Dot11DefaultCipherAlgorithm;
        public WlanAvailableNetworkFlags Flags;
        public uint Reserved;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanAvailableNetworkListHeader
    {
        public uint NumberOfItems;
        public uint Index;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanPhyRadioState
    {
        public uint PhyIndex;
        public Dot11RadioState SoftwareRadioState;
        public Dot11RadioState HardwareRadioState;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanRadioStateHeader
    {
        public uint NumberOfPhys;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanConnectionParameters
    {
        public WlanConnectionMode WlanConnectionMode;

        [MarshalAs(UnmanagedType.LPWStr)]
        public string ProfileNameOrXml;

        // Native WLAN_CONNECTION_PARAMETERS declares pDot11Ssid here, between strProfile and
        // pDesiredBssidList. Omitting it shifts every field after it to the wrong byte offset,
        // which made WlanConnect fail with ERROR_INVALID_PARAMETER (87).
        public IntPtr Dot11SsidPtr;

        public IntPtr DesiredBssidListPtr;
        public Dot11BssType Dot11BssType;
        public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanProfileInfoListHeader
    {
        public uint NumberOfItems;
        public uint Index;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WlanProfileInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string ProfileName;

        public uint Flags;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanNotificationData
    {
        public uint NotificationSource;
        public uint NotificationCode;
        public Guid InterfaceGuid;
        public uint DataSize;
        public IntPtr DataPtr;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanRateSet
    {
        public uint RateSetLength;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 126)]
        public ushort[] RateSet;
    }

    /// <summary>
    /// Native WLAN_BSS_ENTRY. Field order/types must match exactly - in particular, bInRegDomain is
    /// a 1-byte Win32 BOOLEAN (not the 4-byte BOOL used elsewhere in this API), so it is marshaled as
    /// UnmanagedType.U1; getting this wrong shifts every field after it to the wrong byte offset.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanBssEntry
    {
        public Dot11Ssid Dot11Ssid;
        public uint PhyId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Dot11Bssid;

        public Dot11BssType Dot11BssType;
        public uint Dot11BssPhyType;
        public int Rssi;
        public uint LinkQuality;

        [MarshalAs(UnmanagedType.U1)]
        public bool InRegDomain;

        public ushort BeaconPeriod;
        public ulong Timestamp;
        public ulong HostTimestamp;
        public ushort CapabilityInformation;
        public uint ChCenterFrequency;
        public WlanRateSet WlanRateSet;
        public uint IeOffset;
        public uint IeSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct WlanBssListHeader
    {
        public uint TotalSize;
        public uint NumberOfItems;
    }
}
