//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: DHWifiClient2
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DHWifiClient.NET.module;

namespace DHWifiClient.NET
{
    /// <summary>
    /// Convenience facade over <see cref="DHWifiClient"/> that automatically selects one Wi-Fi interface
    /// and exposes the most common scan/connect/profile operations through a single object.
    /// </summary>
    public class DHWifiClient2 : IDisposable
    {
        private readonly DHWifiClient m_oClient;
        private string m_strPreferredInterfaceName;
        private Guid? m_oPreferredInterfaceId;
        private WifiInterface m_oCurrentInterface;
        private bool m_bDisposed;

        /// <summary>Creates a facade that automatically selects the first available Wi-Fi interface.</summary>
        public DHWifiClient2()
        {
            m_oClient = new DHWifiClient();
            RefreshInterfaceSelection();
        }

        /// <summary>
        /// Creates a facade that selects the Wi-Fi interface with the specified display name
        /// (case-insensitive).
        /// </summary>
        public DHWifiClient2(string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
            {
                throw new ArgumentException("interfaceName is required", nameof(interfaceName));
            }

            m_strPreferredInterfaceName = interfaceName;
            m_oClient = new DHWifiClient();
            RefreshInterfaceSelection();
        }

        /// <summary>Creates a facade that selects the Wi-Fi interface with the specified GUID.</summary>
        public DHWifiClient2(Guid interfaceId)
        {
            m_oPreferredInterfaceId = interfaceId;
            m_oClient = new DHWifiClient();
            RefreshInterfaceSelection();
        }

        /// <summary>
        /// Raised on WLAN scan/connection lifecycle changes (see <see cref="WifiNotificationType"/>).
        /// Invoked on a native callback thread, not the UI thread.
        /// </summary>
        public event EventHandler<WifiNotificationEventArgs> Notification
        {
            add
            {
                ThrowIfDisposed();
                m_oClient.Notification += value;
            }
            remove
            {
                if (!m_bDisposed)
                {
                    m_oClient.Notification -= value;
                }
            }
        }

        /// <summary>The currently selected Wi-Fi interface.</summary>
        public WifiInterface CurrentInterface
        {
            get
            {
                ThrowIfDisposed();
                if (m_oCurrentInterface == null)
                {
                    RefreshInterfaceSelection();
                }

                return m_oCurrentInterface;
            }
        }

        /// <summary>Friendly name of the currently selected Wi-Fi interface.</summary>
        public string CurrentInterfaceName => CurrentInterface.Name;

        /// <summary>Guid of the currently selected Wi-Fi interface.</summary>
        public Guid CurrentInterfaceId => CurrentInterface.Id;

        /// <summary>True if the selected interface is currently connected to any visible Wi-Fi network.</summary>
        public bool IsConnected => GetCurrentConnection() != null;

        /// <summary>Returns the current Wi-Fi interface list from the underlying client.</summary>
        public IReadOnlyList<WifiInterface> GetInterfaces()
        {
            ThrowIfDisposed();
            return m_oClient.GetInterfaces();
        }

        /// <summary>Selects the first available Wi-Fi interface.</summary>
        public void SelectFirstInterface()
        {
            ThrowIfDisposed();
            m_strPreferredInterfaceName = null;
            m_oPreferredInterfaceId = null;
            m_oCurrentInterface = null;
            RefreshInterfaceSelection();
        }

        /// <summary>Re-evaluates and updates the current interface selection.</summary>
        public void RefreshInterfaceSelection()
        {
            ThrowIfDisposed();

            var listInterfaces = m_oClient.GetInterfaces();
            if (listInterfaces.Count == 0)
            {
                throw new InvalidOperationException("No WiFi interface found.");
            }

            m_oCurrentInterface = SelectInterface(listInterfaces);
            if (m_oCurrentInterface == null)
            {
                throw new InvalidOperationException("Unable to select a WiFi interface.");
            }
        }

        /// <summary>Selects the Wi-Fi interface with the specified display name (case-insensitive).</summary>
        public void SelectInterface(string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
            {
                throw new ArgumentException("interfaceName is required", nameof(interfaceName));
            }

            ThrowIfDisposed();
            m_strPreferredInterfaceName = interfaceName;
            m_oPreferredInterfaceId = null;
            RefreshInterfaceSelection();
        }

        /// <summary>Selects the Wi-Fi interface with the specified GUID.</summary>
        public void SelectInterface(Guid interfaceId)
        {
            ThrowIfDisposed();
            m_oPreferredInterfaceId = interfaceId;
            m_strPreferredInterfaceName = null;
            RefreshInterfaceSelection();
        }

        /// <summary>Attempts to select the Wi-Fi interface with the specified display name.</summary>
        public bool TrySelectInterface(string interfaceName)
        {
            if (string.IsNullOrWhiteSpace(interfaceName))
            {
                return false;
            }

            ThrowIfDisposed();
            var listInterfaces = m_oClient.GetInterfaces();
            var oMatchedInterface = listInterfaces.FirstOrDefault(oInterface =>
                string.Equals(oInterface.Name, interfaceName, StringComparison.OrdinalIgnoreCase));
            if (oMatchedInterface == null)
            {
                return false;
            }

            m_strPreferredInterfaceName = oMatchedInterface.Name;
            m_oPreferredInterfaceId = null;
            m_oCurrentInterface = oMatchedInterface;
            return true;
        }

        /// <summary>Attempts to select the Wi-Fi interface with the specified GUID.</summary>
        public bool TrySelectInterface(Guid interfaceId)
        {
            ThrowIfDisposed();
            var listInterfaces = m_oClient.GetInterfaces();
            var oMatchedInterface = listInterfaces.FirstOrDefault(oInterface => oInterface.Id == interfaceId);
            if (oMatchedInterface == null)
            {
                return false;
            }

            m_oPreferredInterfaceId = interfaceId;
            m_strPreferredInterfaceName = null;
            m_oCurrentInterface = oMatchedInterface;
            return true;
        }

        /// <summary>Requests an asynchronous scan on the selected interface.</summary>
        public void Scan()
        {
            CurrentInterface.Scan();
        }

        /// <summary>
        /// Requests a scan and waits for the matching scan-complete or scan-failed notification on the
        /// currently selected interface.
        /// </summary>
        public WifiWaitStatus ScanAndWait(int millisecondsTimeout = 15000)
        {
            return WaitForScanResult(millisecondsTimeout, startScan: true);
        }

        /// <summary>
        /// Waits for the next scan-complete or scan-failed notification on the currently selected interface.
        /// Call this immediately after a separate <see cref="Scan()"/> request if you need explicit confirmation.
        /// </summary>
        public WifiWaitStatus WaitForScanComplete(int millisecondsTimeout = 15000)
        {
            return WaitForScanResult(millisecondsTimeout, startScan: false);
        }

        /// <summary>
        /// Requests an asynchronous scan and then immediately returns the current visible Wi-Fi network list.
        /// This method does not wait for the scan-complete notification, so the returned list can still be
        /// the pre-scan snapshot if the WLAN service has not refreshed it yet.
        /// </summary>
        public IReadOnlyList<WifiNetwork> ScanAndGetAvailableNetworks(bool mergeDuplicateBssids = false)
        {
            Scan();
            return GetAvailableNetworks(mergeDuplicateBssids);
        }

        /// <summary>Returns the current visible Wi-Fi network list for the selected interface.</summary>
        public IReadOnlyList<WifiNetwork> GetAvailableNetworks(bool mergeDuplicateBssids = false)
        {
            return CurrentInterface.GetAvailableNetworks(mergeDuplicateBssids);
        }

        /// <summary>Returns the first visible Wi-Fi network whose SSID matches the specified value.</summary>
        public WifiNetwork GetAvailableNetwork(string ssid, bool mergeDuplicateBssids = false,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            return GetAvailableNetworks(mergeDuplicateBssids).FirstOrDefault(oNetwork =>
                string.Equals(oNetwork.Ssid, ssid, comparison));
        }

        /// <summary>Returns true if a visible Wi-Fi network with the specified SSID exists.</summary>
        public bool HasAvailableNetwork(string ssid, bool mergeDuplicateBssids = false,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return GetAvailableNetwork(ssid, mergeDuplicateBssids, comparison) != null;
        }

        /// <summary>Returns the currently connected network entry, or null when not connected.</summary>
        public WifiNetwork GetCurrentConnection(bool mergeDuplicateBssids = false)
        {
            return GetAvailableNetworks(mergeDuplicateBssids).FirstOrDefault(oNetwork => oNetwork.IsConnected);
        }

        /// <summary>Connects to an open network.</summary>
        public void ConnectOpen(string ssid, bool isHidden = false)
        {
            CurrentInterface.Connect(ssid, password: null, isHidden: isHidden);
        }

        /// <summary>Connects to an open network and waits for the resulting connection outcome.</summary>
        public WifiConnectionResult ConnectOpenAndWait(string ssid, bool isHidden = false,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.Connect(ssid, password: null, isHidden: isHidden),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Connects to a WPA/WPA2-Personal network, auto-selecting the default PSK profile shape.</summary>
        public void ConnectPersonal(string ssid, string password, bool isHidden = false)
        {
            CurrentInterface.Connect(ssid, password, isHidden);
        }

        /// <summary>
        /// Connects to a WPA/WPA2-Personal network and waits for the resulting connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectPersonalAndWait(string ssid, string password, bool isHidden = false,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.Connect(ssid, password, isHidden),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Connects to a WPA/WPA2-Personal network with an explicit protocol/cipher combination.</summary>
        public void ConnectPersonal(string ssid, string password, WifiPskProtocol protocol, WifiCipher cipher, bool isHidden = false)
        {
            CurrentInterface.Connect(ssid, password, protocol, cipher, isHidden);
        }

        /// <summary>
        /// Connects to a WPA/WPA2-Personal network with an explicit protocol/cipher combination and waits
        /// for the resulting connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectPersonalAndWait(string ssid, string password,
            WifiPskProtocol protocol, WifiCipher cipher, bool isHidden = false,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.Connect(ssid, password, protocol, cipher, isHidden),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Connects to a legacy WEP network.</summary>
        public void ConnectWep(string ssid, string wepKey, WifiWepAuthentication authentication = WifiWepAuthentication.Open, int keyIndex = 0, bool isHidden = false)
        {
            CurrentInterface.ConnectWep(ssid, wepKey, authentication, keyIndex, isHidden);
        }

        /// <summary>Connects to a legacy WEP network and waits for the resulting connection outcome.</summary>
        public WifiConnectionResult ConnectWepAndWait(string ssid, string wepKey,
            WifiWepAuthentication authentication = WifiWepAuthentication.Open, int keyIndex = 0,
            bool isHidden = false, int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.ConnectWep(ssid, wepKey, authentication, keyIndex, isHidden),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Connects to a hidden open network.</summary>
        public void ConnectHiddenOpen(string ssid)
        {
            ConnectOpen(ssid, isHidden: true);
        }

        /// <summary>Connects to a hidden open network and waits for the resulting connection outcome.</summary>
        public WifiConnectionResult ConnectHiddenOpenAndWait(string ssid, int millisecondsTimeout = 15000,
            bool mergeDuplicateBssids = false)
        {
            return ConnectOpenAndWait(
                ssid,
                isHidden: true,
                millisecondsTimeout: millisecondsTimeout,
                mergeDuplicateBssids: mergeDuplicateBssids);
        }

        /// <summary>Connects to a hidden WPA/WPA2-Personal network using the default PSK profile shape.</summary>
        public void ConnectHiddenPersonal(string ssid, string password)
        {
            ConnectPersonal(ssid, password, isHidden: true);
        }

        /// <summary>
        /// Connects to a hidden WPA/WPA2-Personal network using the default PSK profile shape and waits
        /// for the resulting connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectHiddenPersonalAndWait(string ssid, string password,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            return ConnectPersonalAndWait(
                ssid,
                password,
                isHidden: true,
                millisecondsTimeout: millisecondsTimeout,
                mergeDuplicateBssids: mergeDuplicateBssids);
        }

        /// <summary>Connects to a hidden WPA/WPA2-Personal network with an explicit protocol/cipher combination.</summary>
        public void ConnectHiddenPersonal(string ssid, string password, WifiPskProtocol protocol, WifiCipher cipher)
        {
            ConnectPersonal(ssid, password, protocol, cipher, isHidden: true);
        }

        /// <summary>
        /// Connects to a hidden WPA/WPA2-Personal network with an explicit protocol/cipher combination and
        /// waits for the resulting connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectHiddenPersonalAndWait(string ssid, string password,
            WifiPskProtocol protocol, WifiCipher cipher, int millisecondsTimeout = 15000,
            bool mergeDuplicateBssids = false)
        {
            return ConnectPersonalAndWait(
                ssid,
                password,
                protocol,
                cipher,
                isHidden: true,
                millisecondsTimeout: millisecondsTimeout,
                mergeDuplicateBssids: mergeDuplicateBssids);
        }

        /// <summary>Connects to a hidden WEP network.</summary>
        public void ConnectHiddenWep(string ssid, string wepKey, WifiWepAuthentication authentication = WifiWepAuthentication.Open, int keyIndex = 0)
        {
            ConnectWep(ssid, wepKey, authentication, keyIndex, isHidden: true);
        }

        /// <summary>Connects to a hidden WEP network and waits for the resulting connection outcome.</summary>
        public WifiConnectionResult ConnectHiddenWepAndWait(string ssid, string wepKey,
            WifiWepAuthentication authentication = WifiWepAuthentication.Open, int keyIndex = 0,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            return ConnectWepAndWait(
                ssid,
                wepKey,
                authentication,
                keyIndex,
                isHidden: true,
                millisecondsTimeout: millisecondsTimeout,
                mergeDuplicateBssids: mergeDuplicateBssids);
        }

        /// <summary>Connects using the supplied scanned network entry.</summary>
        public void Connect(WifiNetwork network, string password = null)
        {
            CurrentInterface.Connect(network, password);
        }

        /// <summary>
        /// Connects using the supplied scanned network entry and waits for the resulting connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectAndWait(WifiNetwork network, string password = null,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            if (network == null)
            {
                throw new ArgumentNullException(nameof(network));
            }

            return WaitForConnectionResultCore(
                () => CurrentInterface.Connect(network, password),
                network.Ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Reconnects using an already-saved profile by SSID/profile name.</summary>
        public void ConnectSavedProfile(string ssid)
        {
            CurrentInterface.ConnectSavedProfile(ssid);
        }

        /// <summary>
        /// Reconnects using an already-saved profile by SSID/profile name and waits for the resulting
        /// connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectSavedProfileAndWait(string ssid, int millisecondsTimeout = 15000,
            bool mergeDuplicateBssids = false)
        {
            if (string.IsNullOrWhiteSpace(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }

            return WaitForConnectionResultCore(
                () => CurrentInterface.ConnectSavedProfile(ssid),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>
        /// Waits for the next connection outcome notification on the currently selected interface.
        /// Call this immediately after a separate connect request if you are not using one of the
        /// `...AndWait(...)` helpers. For new code, prefer the `...AndWait(...)` helpers because they
        /// subscribe before issuing the connect request and therefore avoid missing a fast notification.
        /// </summary>
        public WifiConnectionResult WaitForConnectionResult(string expectedSsid = null,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            return WaitForConnectionResultCore(
                startConnectAction: null,
                expectedSsid: expectedSsid,
                millisecondsTimeout: millisecondsTimeout,
                mergeDuplicateBssids: mergeDuplicateBssids);
        }

        /// <summary>Connects to a WPA2-Enterprise (802.1X) network using PEAP-MSCHAPv2.</summary>
        public void ConnectEnterprise(string ssid, string username, string password, string domain = null,
            string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            CurrentInterface.ConnectEnterprise(
                ssid, username, password, domain, serverNames, trustedRootCaThumbprint,
                disableUserPromptForServerValidation);
        }

        /// <summary>
        /// Connects to a WPA2-Enterprise (802.1X) network using PEAP-MSCHAPv2 and waits for the resulting
        /// connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectEnterpriseAndWait(string ssid, string username, string password,
            string domain = null, string serverNames = "", string trustedRootCaThumbprint = null,
            bool disableUserPromptForServerValidation = false, int millisecondsTimeout = 15000,
            bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.ConnectEnterprise(
                    ssid,
                    username,
                    password,
                    domain,
                    serverNames,
                    trustedRootCaThumbprint,
                    disableUserPromptForServerValidation),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Connects to a WPA2-Enterprise (802.1X) network using EAP-TLS.</summary>
        public void ConnectEnterpriseEapTls(string ssid, string clientCertThumbprint = null,
            string serverNames = "", string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false)
        {
            CurrentInterface.ConnectEnterpriseEapTls(
                ssid, clientCertThumbprint, serverNames, trustedRootCaThumbprint,
                disableUserPromptForServerValidation);
        }

        /// <summary>
        /// Connects to a WPA2-Enterprise (802.1X) network using EAP-TLS and waits for the resulting
        /// connection outcome.
        /// </summary>
        public WifiConnectionResult ConnectEnterpriseEapTlsAndWait(string ssid,
            string clientCertThumbprint = null, string serverNames = "",
            string trustedRootCaThumbprint = null, bool disableUserPromptForServerValidation = false,
            int millisecondsTimeout = 15000, bool mergeDuplicateBssids = false)
        {
            ValidateSsid(ssid);
            return WaitForConnectionResultCore(
                () => CurrentInterface.ConnectEnterpriseEapTls(
                    ssid,
                    clientCertThumbprint,
                    serverNames,
                    trustedRootCaThumbprint,
                    disableUserPromptForServerValidation),
                ssid,
                millisecondsTimeout,
                mergeDuplicateBssids);
        }

        /// <summary>Disconnects the selected interface.</summary>
        public void Disconnect()
        {
            CurrentInterface.Disconnect();
        }

        /// <summary>
        /// Returns the saved profiles on the selected interface.
        /// For new code, prefer <see cref="GetSavedProfiles()"/> to make the intent explicit.
        /// </summary>
        public IReadOnlyList<WifiProfileInfo> GetProfiles()
        {
            return CurrentInterface.GetProfiles();
        }

        /// <summary>
        /// Returns the saved profiles on the selected interface.
        /// This is the preferred name for new code because it makes the profile source explicit.
        /// </summary>
        public IReadOnlyList<WifiProfileInfo> GetSavedProfiles()
        {
            return GetProfiles();
        }

        /// <summary>
        /// Returns true if the selected interface currently has a saved profile with the specified name.
        /// For new code, prefer <see cref="HasSavedProfile(string)"/> to make the intent explicit.
        /// </summary>
        public bool HasProfile(string profileName)
        {
            return HasSavedProfile(profileName);
        }

        /// <summary>
        /// Returns true if the selected interface currently has a saved profile with the specified name.
        /// This is the preferred name for new code because it makes the profile source explicit.
        /// </summary>
        public bool HasSavedProfile(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                throw new ArgumentException("profileName is required", nameof(profileName));
            }

            return GetProfiles().Any(oProfile =>
                string.Equals(oProfile.ProfileName, profileName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Deletes a saved profile from the selected interface.
        /// For new code, prefer <see cref="DeleteSavedProfile(string)"/> to make the intent explicit.
        /// </summary>
        public void DeleteProfile(string profileName)
        {
            CurrentInterface.DeleteProfile(profileName);
        }

        /// <summary>
        /// Deletes a saved profile from the selected interface.
        /// This is the preferred name for new code because it makes the profile source explicit.
        /// </summary>
        public void DeleteSavedProfile(string profileName)
        {
            DeleteProfile(profileName);
        }

        /// <summary>Returns the selected interface radio state.</summary>
        public WifiRadioState GetRadioState()
        {
            return CurrentInterface.GetRadioState();
        }

        /// <summary>Toggles the selected interface software radio state.</summary>
        public void SetRadioState(bool turnOn)
        {
            CurrentInterface.SetRadioState(turnOn);
        }

        /// <summary>Turns the selected interface software radio on.</summary>
        public void TurnRadioOn()
        {
            SetRadioState(true);
        }

        /// <summary>Turns the selected interface software radio off.</summary>
        public void TurnRadioOff()
        {
            SetRadioState(false);
        }

        /// <summary>Releases the underlying native Wi-Fi client handle.</summary>
        public void Dispose()
        {
            if (m_bDisposed)
            {
                return;
            }

            m_oClient.Dispose();
            m_bDisposed = true;
        }

        private WifiInterface SelectInterface(IReadOnlyList<WifiInterface> interfaces)
        {
            if (m_oPreferredInterfaceId.HasValue)
            {
                return interfaces.FirstOrDefault(oInterface => oInterface.Id == m_oPreferredInterfaceId.Value)
                    ?? throw new InvalidOperationException(
                        $"WiFi interface not found for id '{m_oPreferredInterfaceId.Value}'.");
            }

            if (!string.IsNullOrEmpty(m_strPreferredInterfaceName))
            {
                return interfaces.FirstOrDefault(oInterface =>
                    string.Equals(oInterface.Name, m_strPreferredInterfaceName, StringComparison.OrdinalIgnoreCase))
                    ?? throw new InvalidOperationException(
                        $"WiFi interface not found for name '{m_strPreferredInterfaceName}'.");
            }

            if (m_oCurrentInterface != null)
            {
                var oMatchedInterface = interfaces.FirstOrDefault(oInterface => oInterface.Id == m_oCurrentInterface.Id);
                if (oMatchedInterface != null)
                {
                    return oMatchedInterface;
                }
            }

            return interfaces[0];
        }

        private WifiWaitStatus WaitForScanResult(int millisecondsTimeout, bool startScan)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            ThrowIfDisposed();

            var oInterfaceId = CurrentInterfaceId;
            var eResult = WifiWaitStatus.TimedOut;

            using (var oCompletedEvent = new ManualResetEventSlim(false))
            {
                EventHandler<WifiNotificationEventArgs> handler = (sender, args) =>
                {
                    if (args.InterfaceId != oInterfaceId)
                    {
                        return;
                    }

                    switch (args.Type)
                    {
                        case WifiNotificationType.ScanComplete:
                            eResult = WifiWaitStatus.Success;
                            oCompletedEvent.Set();
                            break;

                        case WifiNotificationType.ScanFailed:
                            eResult = WifiWaitStatus.Failed;
                            oCompletedEvent.Set();
                            break;
                    }
                };

                Notification += handler;
                try
                {
                    if (startScan)
                    {
                        Scan();
                    }

                    if (!oCompletedEvent.Wait(millisecondsTimeout))
                    {
                        return WifiWaitStatus.TimedOut;
                    }

                    return eResult;
                }
                finally
                {
                    Notification -= handler;
                }
            }
        }

        private WifiConnectionResult WaitForConnectionResultCore(Action startConnectAction,
            string expectedSsid, int millisecondsTimeout, bool mergeDuplicateBssids)
        {
            ValidateMillisecondsTimeout(millisecondsTimeout);
            ThrowIfDisposed();

            var oInterfaceId = CurrentInterfaceId;
            var oResult = WifiConnectionResult.CreateTimedOut(expectedSsid);

            using (var oCompletedEvent = new ManualResetEventSlim(false))
            {
                EventHandler<WifiNotificationEventArgs> handler = (sender, args) =>
                {
                    if (args.InterfaceId != oInterfaceId)
                    {
                        return;
                    }

                    switch (args.Type)
                    {
                        case WifiNotificationType.ConnectionCompleted:
                            var oCurrentConnection = GetCurrentConnection(mergeDuplicateBssids);
                            if (oCurrentConnection != null && IsExpectedSsid(oCurrentConnection.Ssid, expectedSsid))
                            {
                                oResult = WifiConnectionResult.CreateSuccess(
                                    args.Type,
                                    expectedSsid,
                                    oCurrentConnection,
                                    "Connected successfully.");
                            }
                            else if (oCurrentConnection != null)
                            {
                                oResult = WifiConnectionResult.CreateFailed(
                                    args.Type,
                                    expectedSsid,
                                    oCurrentConnection,
                                    "Connection completed notification arrived, but another network is currently connected: "
                                        + oCurrentConnection.Ssid);
                            }
                            else
                            {
                                oResult = WifiConnectionResult.CreateFailed(
                                    args.Type,
                                    expectedSsid,
                                    connectedNetwork: null,
                                    "Connection completed notification arrived, but the current connection could not be confirmed.");
                            }

                            oCompletedEvent.Set();
                            break;

                        case WifiNotificationType.ConnectionAttemptFailed:
                            oResult = WifiConnectionResult.CreateFailed(
                                args.Type,
                                expectedSsid,
                                connectedNetwork: null,
                                "Connection attempt failed.");
                            oCompletedEvent.Set();
                            break;

                        case WifiNotificationType.Disconnected:
                            if (!oCompletedEvent.IsSet)
                            {
                                oResult = WifiConnectionResult.CreateFailed(
                                    args.Type,
                                    expectedSsid,
                                    connectedNetwork: null,
                                    "Disconnected before the connection could be confirmed.");
                                oCompletedEvent.Set();
                            }
                            break;
                    }
                };

                Notification += handler;
                try
                {
                    startConnectAction?.Invoke();

                    if (!oCompletedEvent.Wait(millisecondsTimeout))
                    {
                        return WifiConnectionResult.CreateTimedOut(expectedSsid);
                    }

                    return oResult;
                }
                finally
                {
                    Notification -= handler;
                }
            }
        }

        private static bool IsExpectedSsid(string actualSsid, string expectedSsid)
        {
            if (string.IsNullOrWhiteSpace(expectedSsid))
            {
                return !string.IsNullOrWhiteSpace(actualSsid);
            }

            return string.Equals(actualSsid ?? string.Empty, expectedSsid, StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateSsid(string ssid)
        {
            if (string.IsNullOrWhiteSpace(ssid))
            {
                throw new ArgumentException("ssid is required", nameof(ssid));
            }
        }

        private static void ValidateMillisecondsTimeout(int millisecondsTimeout)
        {
            if (millisecondsTimeout < Timeout.Infinite)
            {
                throw new ArgumentOutOfRangeException(nameof(millisecondsTimeout));
            }
        }

        private void ThrowIfDisposed()
        {
            if (m_bDisposed)
            {
                throw new ObjectDisposedException(nameof(DHWifiClient2));
            }
        }
    }
}
