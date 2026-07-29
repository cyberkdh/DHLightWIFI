//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: WifiInterface
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using DHWifiClient.NET.log;
using DHWifiClient.NET.win32;

namespace DHWifiClient.NET.module
{
    public class WifiInterface
    {
        private readonly IntPtr m_pClientHandle;

        public Guid Id { get; }
        public string Name { get; }
        public WifiInterfaceState State { get; internal set; }

        internal WifiInterface(IntPtr clientHandle, Guid id, string name, WifiInterfaceState state)
        {
            m_pClientHandle = clientHandle;
            Id = id;
            Name = name;
            State = state;
        }

        /// <summary>Requests a rescan of nearby WiFi networks. Asynchronous; returns immediately.</summary>
        public void Scan()
        {
            Logger.Debug($"[{Name}] Scan requested");
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanScan(m_pClientHandle, ref gGuid, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            WifiException.ThrowIfError(nResult);
        }

        /// <summary>
        /// Gets the list of currently visible WiFi networks. Each entry's <see cref="WifiNetwork.Bssids"/>
        /// is always resolved via a separate WlanGetNetworkBssList query per entry, since callers need it
        /// to tell apart multiple physical APs that share the same (or, for hidden networks, an empty)
        /// SSID.
        /// </summary>
        /// <param name="mergeDuplicateBssids">
        /// When true, collapses entries that share the same SSID and resolved BSSID(s) down to a single
        /// row. This works around a driver/scan-cache quirk where WlanGetAvailableNetworkList can report
        /// the same physical AP more than once in a single call even when neither duplicate is the
        /// currently connected network. Entries whose BSSID could not be resolved are left untouched
        /// (never merged with each other), since an empty BSSID list is not evidence they are the same
        /// AP. Off by default.
        /// </param>
        public IReadOnlyList<WifiNetwork> GetAvailableNetworks(bool mergeDuplicateBssids = false)
        {
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanGetAvailableNetworkList(
                m_pClientHandle, ref gGuid, 0, IntPtr.Zero, out IntPtr pListPtr);
            WifiException.ThrowIfError(nResult);

            var listNetworks = new List<WifiNetwork>();
            try
            {
                var oHeader = Marshal.PtrToStructure<WlanAvailableNetworkListHeader>(pListPtr);
                int nHeaderSize = Marshal.SizeOf<WlanAvailableNetworkListHeader>();
                int nItemSize = Marshal.SizeOf<WlanAvailableNetwork>();

                for (int ni = 0; ni < oHeader.NumberOfItems; ni++)
                {
                    IntPtr pItemPtr = IntPtr.Add(pListPtr, nHeaderSize + ni * nItemSize);
                    var oItem = Marshal.PtrToStructure<WlanAvailableNetwork>(pItemPtr);

                    string strSsid = DecodeSsid(oItem.Dot11Ssid);

                    listNetworks.Add(new WifiNetwork
                    {
                        Ssid = strSsid,
                        ProfileName = oItem.ProfileName,
                        SecurityEnabled = oItem.SecurityEnabled,
                        Authentication = MapAuthentication(oItem.Dot11DefaultAuthAlgorithm),
                        Cipher = MapCipher(oItem.Dot11DefaultCipherAlgorithm),
                        IsConnected = (oItem.Flags & WlanAvailableNetworkFlags.Connected) != 0,
                        HasProfile = (oItem.Flags & WlanAvailableNetworkFlags.HasProfile) != 0,
                        SignalQuality = oItem.SignalQuality,
                        Bssids = QueryBssids(oItem.Dot11Ssid, oItem.Dot11BssType, oItem.SecurityEnabled,
                            Math.Max(1, (int)oItem.NumberOfBssids)),
                    });
                }
            }
            finally
            {
                WlanNativeMethods.WlanFreeMemory(pListPtr);
            }

            var listResult = mergeDuplicateBssids ? MergeDuplicateBssids(listNetworks) : listNetworks;

            Logger.Debug($"[{Name}] Found {listResult.Count} available network(s)");
            return listResult;
        }

        /// <summary>
        /// Collapses entries within the same SSID that resolved to the exact same set of BSSIDs down to
        /// one entry, keeping the first occurrence. Entries with no resolved BSSID are passed through
        /// unmerged, since an empty BSSID list is not evidence two entries are the same physical AP.
        /// </summary>
        private static List<WifiNetwork> MergeDuplicateBssids(List<WifiNetwork> networks)
        {
            var listResult = new List<WifiNetwork>(networks.Count);
            var dictSeenBssidKeysBySsid = new Dictionary<string, HashSet<string>>();

            foreach (var oNetwork in networks)
            {
                if (oNetwork.Bssids.Count == 0)
                {
                    listResult.Add(oNetwork);
                    continue;
                }

                if (!dictSeenBssidKeysBySsid.TryGetValue(oNetwork.Ssid, out var setSeenBssidKeys))
                {
                    setSeenBssidKeys = new HashSet<string>();
                    dictSeenBssidKeysBySsid[oNetwork.Ssid] = setSeenBssidKeys;
                }

                string strBssidKey = string.Join(",", oNetwork.Bssids);
                if (setSeenBssidKeys.Add(strBssidKey))
                {
                    listResult.Add(oNetwork);
                }
            }

            return listResult;
        }

        /// <summary>Connects to the specified SSID. If <paramref name="password"/> is null, the network is treated as open.</summary>
        public void Connect(string ssid, string password = null, bool isHidden = false)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            Logger.Info($"[{Name}] Connect attempt: SSID={ssid}, Hidden={isHidden}"); // Never log the password

            string strProfileXml;
            if (string.IsNullOrEmpty(password))
            {
                strProfileXml = WlanProfileXml.CreateOpen(ssid, isHidden);
            }
            else
            {
                ValidatePskPassphrase(password);
                strProfileXml = WlanProfileXml.CreateWpa2Psk(ssid, password, isHidden);
            }

            SetProfile(strProfileXml);
            ConnectProfile(ssid);
            Logger.Info($"[{Name}] Connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>
        /// Connects to a previously scanned network, automatically choosing the profile type (open, WEP, or
        /// WPA/WPA2-PSK) based on <see cref="WifiNetwork.Authentication"/> and <see cref="WifiNetwork.Cipher"/>.
        /// 802.1X Enterprise networks are not supported here; use <see cref="ConnectEnterprise"/> instead.
        /// </summary>
        public void Connect(WifiNetwork network, string password = null)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            Logger.Debug($"[{Name}] Auto-detect connect: SSID={network.Ssid}, Authentication={network.Authentication}, Cipher={network.Cipher}");

            switch (network.Authentication)
            {
                case WifiAuthentication.Open:
                    if (network.Cipher == WifiCipherAlgorithm.WEP)
                    {
                        ConnectWep(network.Ssid, password, WifiWepAuthentication.Open);
                        return;
                    }

                    Connect(network.Ssid);
                    return;

                case WifiAuthentication.SharedKey:
                    ConnectWep(network.Ssid, password, WifiWepAuthentication.Shared);
                    return;

                case WifiAuthentication.WPA_PSK:
                    Connect(network.Ssid, password, WifiPskProtocol.WPA, ResolveCipher(network.Cipher));
                    return;

                case WifiAuthentication.RSNA_PSK:
                    Connect(network.Ssid, password, WifiPskProtocol.WPA2, ResolveCipher(network.Cipher));
                    return;

                case WifiAuthentication.WPA:
                case WifiAuthentication.RSNA:
                case WifiAuthentication.WPA3_ENT:
                    Logger.Warn($"[{Name}] Auto-detect connect rejected: SSID={network.Ssid} is an 802.1X Enterprise network");
                    throw new InvalidOperationException(
                        $"[{Name}] {network.Ssid} is an 802.1X Enterprise network. Use ConnectEnterprise(ssid, username, password) instead.");

                case WifiAuthentication.WPA3_SAE:
                    Logger.Warn($"[{Name}] Auto-detect connect rejected: SSID={network.Ssid} uses unsupported WPA3-SAE");
                    throw new NotSupportedException(
                        $"[{Name}] {network.Ssid} uses WPA3-SAE, which is not yet supported by this library.");

                case WifiAuthentication.WPA_None:
                    Logger.Warn($"[{Name}] Auto-detect connect rejected: SSID={network.Ssid} is an unsupported ad-hoc (IBSS) network");
                    throw new NotSupportedException(
                        $"[{Name}] {network.Ssid} is an ad-hoc (IBSS) network, which is not supported by this library.");

                default:
                    Logger.Warn($"[{Name}] Auto-detect connect rejected: SSID={network.Ssid} uses unsupported authentication {network.Authentication}");
                    throw new NotSupportedException(
                        $"[{Name}] {network.Ssid} uses an unsupported authentication method: {network.Authentication}");
            }
        }

        /// <summary>
        /// Reconnects using an already-saved profile by name (see <see cref="WifiNetwork.HasProfile"/>),
        /// without rebuilding or overwriting the profile's XML. Use this for a reconnect where the caller
        /// has no new password to supply; calling <see cref="Connect(WifiNetwork, string)"/> instead would
        /// require the password again and would overwrite the existing saved profile with one derived from
        /// the current scan result.
        /// </summary>
        public void ConnectSavedProfile(string ssid)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            Logger.Info($"[{Name}] Connect attempt using saved profile: SSID={ssid}");
            ConnectProfile(ssid);
            Logger.Info($"[{Name}] Connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>Connects to a WPA/WPA2-Personal (PSK) network with an explicit protocol and cipher combination.</summary>
        public void Connect(string ssid, string password, WifiPskProtocol protocol, WifiCipher cipher, bool isHidden = false)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password is required", nameof(password));
            }

            ValidatePskPassphrase(password);

            Logger.Info($"[{Name}] Connect attempt: SSID={ssid}, Protocol={protocol}, Cipher={cipher}, Hidden={isHidden}"); // Never log the password

            string strAuthentication = protocol == WifiPskProtocol.WPA2 ? "WPA2PSK" : "WPAPSK";
            string strEncryption = cipher == WifiCipher.TKIP ? "TKIP" : "AES";
            string strProfileXml = WlanProfileXml.CreatePsk(ssid, password, strAuthentication, strEncryption, isHidden);

            SetProfile(strProfileXml);
            ConnectProfile(ssid);
            Logger.Info($"[{Name}] Connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>
        /// Connects to a legacy WEP-secured network. <paramref name="wepKey"/> may be a 5/13-character ASCII passphrase
        /// or a 10/26-character hex key. WEP is deprecated and trivially breakable; use only for compatibility with
        /// legacy hardware that cannot support WPA2/WPA3.
        /// </summary>
        public void ConnectWep(string ssid, string wepKey, WifiWepAuthentication authentication = WifiWepAuthentication.Open, int keyIndex = 0, bool isHidden = false)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            if (keyIndex < 0 || keyIndex > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(keyIndex), "keyIndex must be between 0 and 3");
            }

            Logger.Info($"[{Name}] WEP connect attempt (legacy, deprecated): SSID={ssid}, Hidden={isHidden}"); // Never log the key

            string strKeyMaterialHex = NormalizeWepKeyToHex(wepKey);
            string strAuthValue = authentication == WifiWepAuthentication.Shared ? "shared" : "open";
            string strProfileXml = WlanProfileXml.CreateWep(ssid, strKeyMaterialHex, strAuthValue, keyIndex, isHidden);

            SetProfile(strProfileXml);
            ConnectProfile(ssid);
            Logger.Info($"[{Name}] WEP connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>
        /// Connects to a WPA2-Enterprise (802.1X) network using PEAP-MSCHAPv2 (username/password against the RADIUS/AD backend).
        /// The server certificate is validated using the system trust store; no client certificate is required.
        /// </summary>
        /// <param name="serverNames">Optional semicolon-separated list of expected RADIUS server certificate subject names.</param>
        /// <param name="trustedRootCaThumbprint">Optional SHA1 thumbprint (hex, no separators) of the trusted Root CA that issued the RADIUS server certificate. Providing this lets Windows validate the server certificate without an interactive trust prompt.</param>
        /// <param name="disableUserPromptForServerValidation">When true, suppresses the "Continue connecting?" prompt. Only meaningful together with <paramref name="trustedRootCaThumbprint"/>.</param>
        public void ConnectEnterprise(string ssid, string username, string password, string domain = null,
            string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("username is required", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password is required", nameof(password));
            }

            Logger.Info($"[{Name}] Enterprise connect attempt (PEAP-MSCHAPv2): SSID={ssid}, User={username}"); // Never log the password

            string strProfileXml = WlanProfileXml.CreateWpa2EnterprisePeap(ssid, serverNames, trustedRootCaThumbprint, disableUserPromptForServerValidation);
            Logger.Debug($"[{Name}] Enterprise profile XML:{System.Environment.NewLine}{strProfileXml}");
            SetProfile(strProfileXml);

            var gGuid = Id;
            string strEapUserDataXml = WlanProfileXml.CreatePeapMsChapV2UserData(username, password, domain);
            int nEapResult = WlanNativeMethods.WlanSetProfileEapXmlUserData(
                m_pClientHandle, ref gGuid, ssid, 0, strEapUserDataXml, IntPtr.Zero);
            if (nEapResult != 0)
            {
                string strRedactedEapXml = WlanProfileXml.CreatePeapMsChapV2UserData(username, "********", domain);
                Logger.Debug($"[{Name}] Enterprise EAP user-data XML (password redacted):{System.Environment.NewLine}{strRedactedEapXml}");

                // WlanSetProfileEapXmlUserData can fail (e.g. ERROR_BAD_PROFILE) if a stale profile
                // with the same name already exists from a previous attempt. Falling through to
                // ConnectProfile lets Windows fall back to its own interactive credential prompt
                // instead of failing outright.
                Logger.Info($"[{Name}] Enterprise EAP user-data pre-injection failed (code={nEapResult}); proceeding to connect without it, Windows may prompt for credentials: SSID={ssid}");
            }

            ConnectProfile(ssid);
            Logger.Info($"[{Name}] Enterprise connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>
        /// Connects to a WPA2-Enterprise (802.1X) network using EAP-TLS, authenticating with a client
        /// certificate instead of a username/password. The client certificate must already be installed
        /// in the Windows certificate store (Personal) with its private key.
        /// </summary>
        /// <param name="clientCertThumbprint">
        /// Optional SHA1 thumbprint (hex, no separators) of the client certificate to use. When omitted,
        /// Windows automatically selects a matching certificate from the store (SimpleCertSelection);
        /// it may also fall back to an interactive certificate chooser.
        /// </param>
        /// <param name="serverNames">Optional semicolon-separated list of expected RADIUS server certificate subject names.</param>
        /// <param name="trustedRootCaThumbprint">Optional SHA1 thumbprint (hex, no separators) of the trusted Root CA that issued the RADIUS server certificate. Providing this lets Windows validate the server certificate without an interactive trust prompt.</param>
        /// <param name="disableUserPromptForServerValidation">When true, suppresses the "Continue connecting?" prompt. Only meaningful together with <paramref name="trustedRootCaThumbprint"/>.</param>
        public void ConnectEnterpriseEapTls(string ssid, string clientCertThumbprint = null,
            string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            if (string.IsNullOrEmpty(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            Logger.Info($"[{Name}] Enterprise connect attempt (EAP-TLS): SSID={ssid}, ClientCertThumbprint={clientCertThumbprint ?? "(auto)"}");

            string strProfileXml = WlanProfileXml.CreateWpa2EnterpriseEapTls(ssid, serverNames, trustedRootCaThumbprint, disableUserPromptForServerValidation);
            Logger.Debug($"[{Name}] Enterprise profile XML:{System.Environment.NewLine}{strProfileXml}");
            SetProfile(strProfileXml);

            if (!string.IsNullOrEmpty(clientCertThumbprint))
            {
                var gGuid = Id;
                string strEapUserDataXml = WlanProfileXml.CreateEapTlsUserData(clientCertThumbprint);
                Logger.Debug($"[{Name}] Enterprise EAP user-data XML:{System.Environment.NewLine}{strEapUserDataXml}");
                int nEapResult = WlanNativeMethods.WlanSetProfileEapXmlUserData(
                    m_pClientHandle, ref gGuid, ssid, 0, strEapUserDataXml, IntPtr.Zero);
                if (nEapResult != 0)
                {
                    // WlanSetProfileEapXmlUserData can fail (e.g. ERROR_BAD_PROFILE) if a stale profile
                    // with the same name already exists from a previous attempt. Falling through lets
                    // Windows fall back to automatic certificate selection or its own interactive chooser.
                    Logger.Info($"[{Name}] Enterprise EAP-TLS client certificate thumbprint apply failed (code={nEapResult}); Windows may fall back to certificate chooser: SSID={ssid}");
                }
            }

            ConnectProfile(ssid);
            Logger.Info($"[{Name}] Enterprise connect request succeeded (whether the connection actually completes must be confirmed via a state change): SSID={ssid}");
        }

        /// <summary>Disconnects the current connection.</summary>
        public void Disconnect()
        {
            Logger.Info($"[{Name}] Disconnect requested");
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanDisconnect(m_pClientHandle, ref gGuid, IntPtr.Zero);
            WifiException.ThrowIfError(nResult);
        }

        /// <summary>Gets the list of WLAN profiles saved on this interface.</summary>
        public IReadOnlyList<WifiProfileInfo> GetProfiles()
        {
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanGetProfileList(m_pClientHandle, ref gGuid, IntPtr.Zero, out IntPtr pListPtr);
            WifiException.ThrowIfError(nResult);

            var listProfiles = new List<WifiProfileInfo>();
            try
            {
                var oHeader = Marshal.PtrToStructure<WlanProfileInfoListHeader>(pListPtr);
                int nHeaderSize = Marshal.SizeOf<WlanProfileInfoListHeader>();
                int nItemSize = Marshal.SizeOf<WlanProfileInfo>();

                for (int ni = 0; ni < oHeader.NumberOfItems; ni++)
                {
                    IntPtr pItemPtr = IntPtr.Add(pListPtr, nHeaderSize + ni * nItemSize);
                    var oItem = Marshal.PtrToStructure<WlanProfileInfo>(pItemPtr);
                    var eFlags = (WlanProfileFlags)oItem.Flags;

                    listProfiles.Add(new WifiProfileInfo
                    {
                        ProfileName = oItem.ProfileName,
                        IsGroupPolicy = (eFlags & WlanProfileFlags.GroupPolicy) != 0,
                        IsPerUser = (eFlags & WlanProfileFlags.User) != 0,
                    });
                }
            }
            finally
            {
                WlanNativeMethods.WlanFreeMemory(pListPtr);
            }

            Logger.Debug($"[{Name}] Found {listProfiles.Count} saved profile(s)");
            return listProfiles;
        }

        /// <summary>Deletes the saved profile with the specified name.</summary>
        public void DeleteProfile(string profileName)
        {
            Logger.Info($"[{Name}] DeleteProfile: {profileName}");

            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanDeleteProfile(m_pClientHandle, ref gGuid, profileName, IntPtr.Zero);
            WifiException.ThrowIfError(nResult);
        }

        /// <summary>Gets the adapter's radio on/off state. Based on the software switch; does not require administrator privileges.</summary>
        public WifiRadioState GetRadioState()
        {
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanQueryInterface(
                m_pClientHandle, ref gGuid, WlanIntfOpcode.RadioState, IntPtr.Zero,
                out _, out IntPtr pDataPtr, IntPtr.Zero);
            WifiException.ThrowIfError(nResult);

            try
            {
                var oHeader = Marshal.PtrToStructure<WlanRadioStateHeader>(pDataPtr);
                if (oHeader.NumberOfPhys == 0)
                {
                    return WifiRadioState.Unknown;
                }

                var oPhyState = Marshal.PtrToStructure<WlanPhyRadioState>(
                    IntPtr.Add(pDataPtr, Marshal.SizeOf<WlanRadioStateHeader>()));

                WifiRadioState eState;
                if (oPhyState.SoftwareRadioState == Dot11RadioState.Off ||
                    oPhyState.HardwareRadioState == Dot11RadioState.Off)
                {
                    eState = WifiRadioState.Off;
                }
                else if (oPhyState.SoftwareRadioState == Dot11RadioState.On &&
                    oPhyState.HardwareRadioState == Dot11RadioState.On)
                {
                    eState = WifiRadioState.On;
                }
                else
                {
                    eState = WifiRadioState.Unknown;
                }

                Logger.Debug($"[{Name}] Radio state queried: {eState}");
                return eState;
            }
            finally
            {
                WlanNativeMethods.WlanFreeMemory(pDataPtr);
            }
        }

        /// <summary>
        /// Turns the adapter's software radio on or off. Does not require administrator privileges (based on the default WLAN service ACL).
        /// If the physical hardware switch (e.g. airplane mode) is off, this call alone cannot turn the radio on.
        /// </summary>
        public void SetRadioState(bool turnOn)
        {
            var gGuid = Id;
            var oPhyState = new WlanPhyRadioState
            {
                PhyIndex = 0,
                SoftwareRadioState = turnOn ? Dot11RadioState.On : Dot11RadioState.Off,
                HardwareRadioState = Dot11RadioState.Unknown,
            };

            IntPtr pDataPtr = Marshal.AllocHGlobal(Marshal.SizeOf<WlanPhyRadioState>());
            try
            {
                Marshal.StructureToPtr(oPhyState, pDataPtr, false);
                int nResult = WlanNativeMethods.WlanSetInterface(
                    m_pClientHandle, ref gGuid, WlanIntfOpcode.RadioState,
                    (uint)Marshal.SizeOf<WlanPhyRadioState>(), pDataPtr, IntPtr.Zero);
                WifiException.ThrowIfError(nResult);
                Logger.Info($"[{Name}] Radio state changed: {(turnOn ? "ON" : "OFF")}");
            }
            finally
            {
                Marshal.FreeHGlobal(pDataPtr);
            }
        }

        private void SetProfile(string profileXml)
        {
            var gGuid = Id;
            int nResult = WlanNativeMethods.WlanSetProfile(
                m_pClientHandle, ref gGuid, 0, profileXml, null, true, IntPtr.Zero, out _);
            WifiException.ThrowIfError(nResult);
        }

        private void ConnectProfile(string ssid)
        {
            var gGuid = Id;
            var oConnectionParameters = new WlanConnectionParameters
            {
                WlanConnectionMode = WlanConnectionMode.Profile,
                ProfileNameOrXml = ssid,
                Dot11SsidPtr = IntPtr.Zero,
                DesiredBssidListPtr = IntPtr.Zero,
                Dot11BssType = Dot11BssType.Infrastructure,
                Flags = 0,
            };

            int nResult = WlanNativeMethods.WlanConnect(m_pClientHandle, ref gGuid, ref oConnectionParameters, IntPtr.Zero);
            WifiException.ThrowIfError(nResult);
        }

        /// <summary>
        /// Validates that a WPA/WPA2-Personal passphrase meets the IEEE 802.11i length requirement (8-63 ASCII
        /// characters). Windows rejects out-of-range passphrases at <c>WlanSetProfile</c> time with the generic,
        /// unhelpful ERROR_BAD_PROFILE (1206, "The network connection profile is corrupted") - this check surfaces
        /// the real cause up front instead.
        /// </summary>
        internal static void ValidatePskPassphrase(string passphrase)
        {
            if (passphrase.Length < 8 || passphrase.Length > 63)
            {
                throw new ArgumentException(
                    $"WPA/WPA2-Personal passphrase must be 8-63 characters long (was {passphrase.Length})",
                    nameof(passphrase));
            }
        }

        private static string NormalizeWepKeyToHex(string wepKey)
        {
            if (string.IsNullOrEmpty(wepKey))
            {
                throw new ArgumentException("wepKey is required", nameof(wepKey));
            }

            if ((wepKey.Length == 10 || wepKey.Length == 26) && IsHex(wepKey))
            {
                return wepKey;
            }

            if (wepKey.Length == 5 || wepKey.Length == 13)
            {
                var arrBytes = Encoding.ASCII.GetBytes(wepKey);
                var oSb = new StringBuilder(arrBytes.Length * 2);
                foreach (byte byValue in arrBytes)
                {
                    oSb.Append(byValue.ToString("X2"));
                }
                return oSb.ToString();
            }

            throw new ArgumentException(
                "wepKey must be a 5/13-character ASCII passphrase or a 10/26-character hex key", nameof(wepKey));
        }

        private static bool IsHex(string value)
        {
            foreach (char c in value)
            {
                bool bIsHexDigit = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F');
                if (!bIsHexDigit)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Best-effort BSSID lookup for one WlanGetAvailableNetworkList entry, via a separate
        /// WlanGetNetworkBssList query filtered to the same SSID/BSS type/security setting. Returns an
        /// empty list rather than throwing if the query fails, since a missing BSSID must not break the
        /// primary network list. <paramref name="maxCount"/> caps the result at the entry's own
        /// NumberOfBssids, since a wildcard (empty-SSID) filter can otherwise also match unrelated hidden
        /// networks elsewhere in range.
        /// </summary>
        private List<string> QueryBssids(Dot11Ssid ssid, Dot11BssType bssType, bool securityEnabled, int maxCount)
        {
            var listBssids = new List<string>();
            var gGuid = Id;
            IntPtr pSsidPtr = IntPtr.Zero;
            IntPtr pListPtr = IntPtr.Zero;
            try
            {
                pSsidPtr = Marshal.AllocHGlobal(Marshal.SizeOf<Dot11Ssid>());
                Marshal.StructureToPtr(ssid, pSsidPtr, false);

                int nResult = WlanNativeMethods.WlanGetNetworkBssList(
                    m_pClientHandle, ref gGuid, pSsidPtr, bssType, securityEnabled, IntPtr.Zero, out pListPtr);
                if (nResult != 0)
                {
                    Logger.Debug($"[{Name}] WlanGetNetworkBssList failed (code={nResult}); Bssids left empty for this entry");
                    return listBssids;
                }

                var oHeader = Marshal.PtrToStructure<WlanBssListHeader>(pListPtr);
                int nHeaderSize = Marshal.SizeOf<WlanBssListHeader>();
                int nItemSize = Marshal.SizeOf<WlanBssEntry>();

                for (int ni = 0; ni < oHeader.NumberOfItems && listBssids.Count < maxCount; ni++)
                {
                    IntPtr pItemPtr = IntPtr.Add(pListPtr, nHeaderSize + ni * nItemSize);
                    var oItem = Marshal.PtrToStructure<WlanBssEntry>(pItemPtr);
                    string strBssid = FormatBssid(oItem.Dot11Bssid);
                    if (strBssid != null)
                    {
                        listBssids.Add(strBssid);
                    }
                }
            }
            catch (Exception oEx)
            {
                Logger.Debug($"[{Name}] BSSID lookup failed; Bssids left empty for this entry: {oEx.Message}");
            }
            finally
            {
                if (pListPtr != IntPtr.Zero)
                {
                    WlanNativeMethods.WlanFreeMemory(pListPtr);
                }
                if (pSsidPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(pSsidPtr);
                }
            }
            return listBssids;
        }

        internal static string FormatBssid(byte[] bssidBytes)
        {
            if (bssidBytes == null || bssidBytes.Length != 6)
            {
                return null;
            }

            var oSb = new StringBuilder(17);
            for (int ni = 0; ni < bssidBytes.Length; ni++)
            {
                if (ni > 0)
                {
                    oSb.Append(':');
                }
                oSb.Append(bssidBytes[ni].ToString("X2"));
            }
            return oSb.ToString();
        }

        internal static string DecodeSsid(Dot11Ssid ssid)
        {
            if (ssid.SsidBytes == null || ssid.SsidLength == 0)
            {
                return string.Empty;
            }

            // Defensive clamp: the driver has been observed to report a SsidLength larger than the
            // 32-byte SsidBytes buffer during rapid radio/connection state transitions.
            int nLength = (int)Math.Min(ssid.SsidLength, (uint)ssid.SsidBytes.Length);
            if (nLength != ssid.SsidLength)
            {
                Logger.Warn($"SSID length reported by driver ({ssid.SsidLength}) exceeds buffer size ({ssid.SsidBytes.Length}); clamped");
            }

            return Encoding.UTF8.GetString(ssid.SsidBytes, 0, nLength);
        }

        private static WifiAuthentication MapAuthentication(Dot11AuthAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case Dot11AuthAlgorithm.IEEE80211_Open: return WifiAuthentication.Open;
                case Dot11AuthAlgorithm.IEEE80211_SharedKey: return WifiAuthentication.SharedKey;
                case Dot11AuthAlgorithm.WPA: return WifiAuthentication.WPA;
                case Dot11AuthAlgorithm.WPA_PSK: return WifiAuthentication.WPA_PSK;
                case Dot11AuthAlgorithm.WPA_None: return WifiAuthentication.WPA_None;
                case Dot11AuthAlgorithm.RSNA: return WifiAuthentication.RSNA;
                case Dot11AuthAlgorithm.RSNA_PSK: return WifiAuthentication.RSNA_PSK;
                case Dot11AuthAlgorithm.WPA3: return WifiAuthentication.WPA3;
                case Dot11AuthAlgorithm.WPA3_SAE: return WifiAuthentication.WPA3_SAE;
                case Dot11AuthAlgorithm.WPA3_ENT: return WifiAuthentication.WPA3_ENT;
                default: return WifiAuthentication.Open;
            }
        }

        private static WifiCipherAlgorithm MapCipher(Dot11CipherAlgorithm algorithm)
        {
            switch (algorithm)
            {
                case Dot11CipherAlgorithm.None: return WifiCipherAlgorithm.None;
                case Dot11CipherAlgorithm.WEP40: return WifiCipherAlgorithm.WEP;
                case Dot11CipherAlgorithm.WEP104: return WifiCipherAlgorithm.WEP;
                case Dot11CipherAlgorithm.WEP: return WifiCipherAlgorithm.WEP;
                case Dot11CipherAlgorithm.TKIP: return WifiCipherAlgorithm.TKIP;
                case Dot11CipherAlgorithm.CCMP: return WifiCipherAlgorithm.AES;
                default: return WifiCipherAlgorithm.AES;
            }
        }

        private static WifiCipher ResolveCipher(WifiCipherAlgorithm cipher)
        {
            return cipher == WifiCipherAlgorithm.TKIP ? WifiCipher.TKIP : WifiCipher.AES;
        }
    }
}
