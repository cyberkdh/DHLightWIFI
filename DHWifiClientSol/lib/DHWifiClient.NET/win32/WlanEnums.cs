//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WlanEnums
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;

namespace DHWifiClient.NET.win32
{
    internal enum WlanInterfaceState
    {
        NotReady = 0,
        Connected = 1,
        AdHocNetworkFormed = 2,
        Disconnecting = 3,
        Disconnected = 4,
        Associating = 5,
        Discovering = 6,
        Authenticating = 7,
    }

    [Flags]
    internal enum WlanAvailableNetworkFlags
    {
        None = 0x0,
        Connected = 0x1,
        HasProfile = 0x2,
    }

    internal enum Dot11BssType
    {
        Infrastructure = 1,
        Independent = 2,
        Any = 3,
    }

    internal enum Dot11AuthAlgorithm : uint
    {
        IEEE80211_Open = 1,
        IEEE80211_SharedKey = 2,
        WPA = 3,
        WPA_PSK = 4,
        WPA_None = 5,
        RSNA = 6,
        RSNA_PSK = 7,
        WPA3 = 8,
        WPA3_SAE = 9,
        WPA3_ENT = 10,
    }

    internal enum Dot11CipherAlgorithm : uint
    {
        None = 0x00,
        WEP40 = 0x01,
        TKIP = 0x02,
        CCMP = 0x04,
        WEP104 = 0x05,
        WPA_UseGroup = 0x100,
        RSN_UseGroup = 0x100,
        WEP = 0x101,
    }

    internal enum WlanConnectionMode
    {
        Profile = 0,
        TemporaryProfile = 1,
        DiscoverySecure = 2,
        DiscoveryUnsecure = 3,
        Auto = 4,
        Invalid = 5,
    }

    internal enum Dot11RadioState : uint
    {
        Unknown = 0,
        On = 1,
        Off = 2,
    }

    internal enum WlanIntfOpcode
    {
        AutoconfEnabled = 1,
        BackgroundScanEnabled,
        MediaStreamingMode,
        RadioState,
        BssType,
        InterfaceState,
        CurrentConnection,
        ChannelNumber,
        SupportedInfrastructureAuthCipherPairs,
        SupportedAdhocAuthCipherPairs,
        SupportedCountryOrRegionStringList,
        CurrentOperationMode,
        SupportedSafeMode,
        CertifiedSafeMode,
    }

    [Flags]
    internal enum WlanNotificationSource : uint
    {
        None = 0x00000000,
        OneX = 0x00000004,
        Acm = 0x00000008,
        Msm = 0x00000010,
        Security = 0x00000020,
        Ihv = 0x00000040,
        Hnwk = 0x00000080,
        All = 0x0000FFFF,
    }

    [Flags]
    internal enum WlanProfileFlags : uint
    {
        None = 0,
        GroupPolicy = 0x1,
        User = 0x2,
        GetPlaintextKey = 0x4,
    }

    /// <summary>
    /// WLAN_NOTIFICATION_ACM subset used by this library (scan and connection lifecycle notifications).
    /// Values must match the native wlan_notification_acm_enum exactly (wlanapi.h): scan_complete=7,
    /// scan_fail=8, connection_start=9, connection_complete=10, connection_attempt_fail=11. A previous
    /// off-by-one here silently mislabeled every ACM notification (e.g. a real ConnectionAttemptFail
    /// was reported to callers as ConnectionComplete).
    /// </summary>
    internal enum WlanNotificationCodeAcm : uint
    {
        ScanComplete = 7,
        ScanFail = 8,
        ConnectionStart = 9,
        ConnectionComplete = 10,
        ConnectionAttemptFail = 11,
        Disconnecting = 20,
        Disconnected = 21,
    }
}
