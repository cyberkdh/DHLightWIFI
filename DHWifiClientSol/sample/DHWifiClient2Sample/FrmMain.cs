//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: FrmMain
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHWifiClient.NET;
using DHWifiClient.NET.log;
using DHWifiClient.NET.module;

namespace DHWifiClient2Sample
{
    public partial class FrmMain : Form
    {
        private DHWifiClient2 m_oClient;
        private List<WifiInterface> m_listInterfaces = new List<WifiInterface>();
        private TextBoxLogWriter m_oLogWriter;
        private string m_strLastConnectAttemptSsid;
        private bool m_bSuppressOperationNotifications;

        private WifiInterface CurrentInterface => m_oClient?.CurrentInterface;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            string strLogDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            m_oLogWriter = new TextBoxLogWriter(txtLog, strLogDirectory) { FileLoggingEnabled = chkFileLog.Checked };
            Logger.SetWriter(m_oLogWriter);
            Logger.MinimumLevel = LogLevel.Debug;

            SetControlsEnabled(false);
            lblWifiCheckResult.Text = "WiFi Adapter: -";
            lblStatus.Text = "Status: Click 'Check WiFi' to begin.";
        }

        private void chkFileLog_CheckedChanged(object sender, EventArgs e)
        {
            if (m_oLogWriter != null)
            {
                m_oLogWriter.FileLoggingEnabled = chkFileLog.Checked;
                Logger.Info($"File logging {(chkFileLog.Checked ? "enabled" : "disabled")}");
            }
        }

        private void btnCheckWifi_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_oClient == null)
                {
                    m_oClient = new DHWifiClient2();
                    m_oClient.Notification += Client_Notification;
                }

                LoadAdapters();
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to initialize DHWifiClient2", oEx);
                lblWifiCheckResult.Text = "WiFi Adapter: Check failed";
                MessageBox.Show(this, "Unable to check WiFi adapter presence: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetControlsEnabled(false);
            }
        }

        private void LoadAdapters()
        {
            m_listInterfaces = m_oClient.GetInterfaces().ToList();
            Logger.Debug($"Adapter list: [{string.Join(", ", m_listInterfaces.Select(i => i.Name))}]");

            cmbAdapter.DataSource = null;
            cmbAdapter.DataSource = m_listInterfaces;
            cmbAdapter.DisplayMember = "Name";

            if (m_oClient.CurrentInterface != null)
            {
                for (int ni = 0; ni < m_listInterfaces.Count; ni++)
                {
                    if (m_listInterfaces[ni].Id == m_oClient.CurrentInterface.Id)
                    {
                        cmbAdapter.SelectedIndex = ni;
                        break;
                    }
                }
            }

            bool bHasAdapter = m_listInterfaces.Count > 0;
            SetControlsEnabled(bHasAdapter);

            lblWifiCheckResult.Text = bHasAdapter
                ? $"WiFi Adapter: Found ({m_listInterfaces.Count})"
                : "WiFi Adapter: Not found";

            lblStatus.Text = bHasAdapter ? "Status: Ready" : "Status: No WiFi adapter found.";
        }

        /// <summary>Fires on a native callback thread; marshal to the UI thread before touching controls.</summary>
        private void Client_Notification(object sender, WifiNotificationEventArgs e)
        {
            if (IsDisposed)
            {
                return;
            }

            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => HandleNotification(e)));
            }
            else
            {
                HandleNotification(e);
            }
        }

        private void HandleNotification(WifiNotificationEventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null || e.InterfaceId != oIface.Id)
            {
                return;
            }

            Logger.Debug($"[{oIface.Name}] Notification received: {e.Type}");

            switch (e.Type)
            {
                case WifiNotificationType.ScanComplete:
                    if (!m_bSuppressOperationNotifications)
                    {
                        RefreshNetworkList(announceCount: false);
                        lblStatus.Text = "Status: Scan completed";
                    }
                    break;

                case WifiNotificationType.ScanFailed:
                    if (!m_bSuppressOperationNotifications)
                    {
                        lblStatus.Text = "Status: Scan failed";
                    }
                    break;

                case WifiNotificationType.ConnectionCompleted:
                    if (!m_bSuppressOperationNotifications)
                    {
                        bool? bConnected = RefreshNetworkList(announceCount: false);
                        if (bConnected == true)
                        {
                            lblStatus.Text = "Status: Connected";
                        }
                        else if (bConnected == false)
                        {
                            lblStatus.Text = "Status: Connect attempt failed";
                        }
                    }
                    break;

                case WifiNotificationType.ConnectionAttemptFailed:
                    if (!m_bSuppressOperationNotifications)
                    {
                        RefreshNetworkList(announceCount: false);
                        lblStatus.Text = "Status: Connect attempt failed";
                        MessageBox.Show(this, "Connection attempt failed.", "Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    break;

                case WifiNotificationType.Disconnected:
                    RefreshNetworkList(announceCount: false);
                    lblStatus.Text = "Status: Disconnected";
                    break;
            }
        }

        private void SetControlsEnabled(bool enabled)
        {
            cmbAdapter.Enabled = enabled;
            btnRadioToggle.Enabled = enabled;
            btnScan.Enabled = enabled;
            btnConnect.Enabled = enabled;
            btnReconnectSaved.Enabled = enabled;
            btnDisconnect.Enabled = enabled;
            btnHiddenNetwork.Enabled = enabled;
            btnConnectEnterprise.Enabled = enabled;
            btnDeleteProfile.Enabled = enabled;
            lvNetworks.Enabled = enabled;
            chkMergeDuplicateBssids.Enabled = enabled;
            chkDeleteProfileOnAuthFailure.Enabled = enabled;
        }

        private void cmbAdapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var oIface = cmbAdapter.SelectedItem as WifiInterface;
            if (m_oClient != null && oIface != null)
            {
                m_oClient.TrySelectInterface(oIface.Id);
            }

            RefreshRadioState();
            RefreshNetworkList();
        }

        private void chkMergeDuplicateBssids_CheckedChanged(object sender, EventArgs e)
        {
            RefreshNetworkList(announceCount: false);
        }

        private void RefreshRadioState()
        {
            var oIface = CurrentInterface;
            if (oIface == null)
            {
                lblRadioState.Text = "Radio: -";
                return;
            }

            try
            {
                var eState = m_oClient.GetRadioState();
                lblRadioState.Text = "Radio: " + DescribeRadioState(eState);
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to query radio state", oEx);
                lblRadioState.Text = "Radio: Unknown";
            }
        }

        private static string DescribeRadioState(WifiRadioState state)
        {
            switch (state)
            {
                case WifiRadioState.On: return "ON";
                case WifiRadioState.Off: return "OFF";
                default: return "Unknown";
            }
        }

        private bool? RefreshNetworkList(bool announceCount = true)
        {
            var oIface = CurrentInterface;

            if (oIface == null)
            {
                lvNetworks.Items.Clear();
                Logger.Debug("RefreshNetworkList skipped: no adapter is currently selected");
                return null;
            }

            try
            {
                var listNetworks = m_oClient.GetAvailableNetworks(
                    mergeDuplicateBssids: chkMergeDuplicateBssids.Checked);
                Logger.Debug($"[{oIface.Name}] Network list: [{string.Join("\r\n", listNetworks.Select(n => $"{n.Ssid}({n.Authentication}/{n.Cipher}) Bssids=[{string.Join(",", n.Bssids)}]"))}]");

                var setSavedProfileNames = new HashSet<string>(m_oClient.GetSavedProfiles().Select(p => p.ProfileName));
                var dictNetworksBySsid = listNetworks
                    .GroupBy(n => n.Ssid)
                    .ToDictionary(
                        g => g.Key,
                        g => new Queue<WifiNetwork>(
                            !string.IsNullOrEmpty(g.Key) && g.Any(n => n.IsConnected)
                                ? g.Where(n => n.IsConnected)
                                : g));

                for (int ni = lvNetworks.Items.Count - 1; ni >= 0; ni--)
                {
                    var oExisting = (WifiNetwork)lvNetworks.Items[ni].Tag;
                    if (!dictNetworksBySsid.TryGetValue(oExisting.Ssid, out var queueMatches) || queueMatches.Count == 0)
                    {
                        lvNetworks.Items.RemoveAt(ni);
                    }
                    else
                    {
                        ApplyNetworkToItem(lvNetworks.Items[ni], queueMatches.Dequeue(), setSavedProfileNames);
                    }
                }

                foreach (var queueEntries in dictNetworksBySsid.Values)
                {
                    while (queueEntries.Count > 0)
                    {
                        var oItem = new ListViewItem();
                        ApplyNetworkToItem(oItem, queueEntries.Dequeue(), setSavedProfileNames);
                        lvNetworks.Items.Add(oItem);
                    }
                }

                if (lvNetworks.ListViewItemSorter == null)
                {
                    lvNetworks.ListViewItemSorter = new NetworkSignalComparer();
                }
                lvNetworks.Sort();

                Logger.Debug($"lvNetworks populated with {lvNetworks.Items.Count} row(s)");
                if (announceCount)
                {
                    lblStatus.Text = $"Status: Found {listNetworks.Count} network(s)";
                }

                return listNetworks.Any(n => n.IsConnected);
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to query network list", oEx);
                lvNetworks.Items.Clear();
                lblStatus.Text = "Status: Failed to query network list";
                return null;
            }
        }

        private static void ApplyNetworkToItem(ListViewItem item, WifiNetwork network, HashSet<string> savedProfileNames)
        {
            bool bHasProfile = !string.IsNullOrEmpty(network.ProfileName) && savedProfileNames.Contains(network.ProfileName);
            string strSecurity = network.SecurityEnabled ? network.Authentication.ToString() : "Open";
            string strCipher = network.SecurityEnabled ? network.Cipher.ToString() : "-";
            string strBssid = network.Bssids.Count > 0 ? string.Join(", ", network.Bssids) : "-";
            string strStatus = network.IsConnected ? "Connected" : (bHasProfile ? "Profile saved" : "-");

            string[] arrValues = { network.Ssid, network.SignalQuality + "%", strSecurity, strCipher, strBssid, strStatus };

            if (item.SubItems.Count != arrValues.Length)
            {
                item.SubItems.Clear();
                item.Text = arrValues[0];
                for (int ni = 1; ni < arrValues.Length; ni++)
                {
                    item.SubItems.Add(arrValues[ni]);
                }
            }
            else
            {
                item.Text = arrValues[0];
                for (int ni = 1; ni < arrValues.Length; ni++)
                {
                    item.SubItems[ni].Text = arrValues[ni];
                }
            }

            item.Tag = network;
        }

        private sealed class NetworkSignalComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                var oLeft = (WifiNetwork)((ListViewItem)x).Tag;
                var oRight = (WifiNetwork)((ListViewItem)y).Tag;

                int nConnectedCompare = oRight.IsConnected.CompareTo(oLeft.IsConnected);
                if (nConnectedCompare != 0)
                {
                    return nConnectedCompare;
                }

                return oRight.SignalQuality.CompareTo(oLeft.SignalQuality);
            }
        }

        private void btnRadioToggle_Click(object sender, EventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null)
            {
                return;
            }

            try
            {
                var eCurrent = m_oClient.GetRadioState();
                if (eCurrent == WifiRadioState.On)
                {
                    m_oClient.TurnRadioOff();
                }
                else
                {
                    m_oClient.TurnRadioOn();
                }
                RefreshRadioState();
                RefreshNetworkList(announceCount: false);
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to change radio state", oEx);
                MessageBox.Show(this, "Unable to change the radio state: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null)
            {
                return;
            }

            try
            {
                btnScan.Enabled = false;
                lblStatus.Text = "Status: Scanning...";
                m_bSuppressOperationNotifications = true;
                var eStatus = await Task.Run(() => m_oClient.ScanAndWait(10000));
                RefreshNetworkList(announceCount: true);

                switch (eStatus)
                {
                    case WifiWaitStatus.Success:
                        lblStatus.Text = $"Status: Found {lvNetworks.Items.Count} network(s)";
                        break;

                    case WifiWaitStatus.Failed:
                        lblStatus.Text = "Status: Scan failed";
                        break;

                    default:
                        lblStatus.Text = $"Status: Found {lvNetworks.Items.Count} network(s) (scan wait timed out)";
                        break;
                }
            }
            catch (Exception oEx)
            {
                Logger.Error("Scan request failed", oEx);
                lblStatus.Text = "Status: Scan request failed";
            }
            finally
            {
                btnScan.Enabled = true;
                m_bSuppressOperationNotifications = false;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null || lvNetworks.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select a network from the list to connect to.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oNetwork = (WifiNetwork)lvNetworks.SelectedItems[0].Tag;
            string strPassword = null;

            if (oNetwork.SecurityEnabled)
            {
                using (var oDlg = new PasswordDialog(oNetwork.Ssid))
                {
                    if (oDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }

                    strPassword = oDlg.Password;
                }
            }

            try
            {
                lblStatus.Text = $"Status: Connecting to {oNetwork.Ssid}...";
                m_strLastConnectAttemptSsid = oNetwork.Ssid;
                m_bSuppressOperationNotifications = true;
                var oResult = await Task.Run(() => m_oClient.ConnectAndWait(
                    oNetwork,
                    strPassword,
                    millisecondsTimeout: 15000,
                    mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                ApplyConnectionResult(oResult, oNetwork.Ssid);
            }
            catch (Exception oEx)
            {
                Logger.Error($"Connect failed: SSID={oNetwork.Ssid}", oEx);
                lblStatus.Text = "Status: Connect failed";
                MessageBox.Show(this, "Unable to connect to the network: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_bSuppressOperationNotifications = false;
            }
        }

        private async void btnReconnectSaved_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null || lvNetworks.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select a network from the list to connect to.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oNetwork = (WifiNetwork)lvNetworks.SelectedItems[0].Tag;
            if (!m_oClient.HasProfile(oNetwork.Ssid))
            {
                MessageBox.Show(this, "There is no saved profile for this network yet. Use Connect first.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                lblStatus.Text = $"Status: Connecting to {oNetwork.Ssid} (saved profile)...";
                m_strLastConnectAttemptSsid = oNetwork.Ssid;
                m_bSuppressOperationNotifications = true;
                var oResult = await Task.Run(() => m_oClient.ConnectSavedProfileAndWait(
                    oNetwork.Ssid,
                    millisecondsTimeout: 15000,
                    mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                ApplyConnectionResult(oResult, oNetwork.Ssid);
            }
            catch (Exception oEx)
            {
                Logger.Error($"Connect (saved profile) failed: SSID={oNetwork.Ssid}", oEx);
                lblStatus.Text = "Status: Connect failed";
                MessageBox.Show(this, "Unable to connect to the network: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                m_bSuppressOperationNotifications = false;
            }
        }

        private async void btnHiddenNetwork_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null)
            {
                return;
            }

            using (var oDlg = new HiddenNetworkDialog())
            {
                if (oDlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    lblStatus.Text = $"Status: Connecting to {oDlg.Ssid}...";
                    m_strLastConnectAttemptSsid = oDlg.Ssid;
                    Logger.Info($"[{CurrentInterface.Name}] Hidden network connect attempt: SSID={oDlg.Ssid}, Security={oDlg.SecurityType}");
                    m_bSuppressOperationNotifications = true;

                    WifiConnectionResult oResult;

                    switch (oDlg.SecurityType)
                    {
                        case HiddenNetworkSecurityType.Open:
                            oResult = await Task.Run(() => m_oClient.ConnectHiddenOpenAndWait(
                                oDlg.Ssid,
                                millisecondsTimeout: 15000,
                                mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                            break;

                        case HiddenNetworkSecurityType.Wep:
                            oResult = await Task.Run(() => m_oClient.ConnectHiddenWepAndWait(
                                oDlg.Ssid,
                                oDlg.Password,
                                millisecondsTimeout: 15000,
                                mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                            break;

                        case HiddenNetworkSecurityType.WpaPersonalTkip:
                            oResult = await Task.Run(() => m_oClient.ConnectHiddenPersonalAndWait(
                                oDlg.Ssid,
                                oDlg.Password,
                                WifiPskProtocol.WPA,
                                WifiCipher.TKIP,
                                millisecondsTimeout: 15000,
                                mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                            break;

                        case HiddenNetworkSecurityType.Wpa2PersonalAes:
                        default:
                            oResult = await Task.Run(() => m_oClient.ConnectHiddenPersonalAndWait(
                                oDlg.Ssid,
                                oDlg.Password,
                                WifiPskProtocol.WPA2,
                                WifiCipher.AES,
                                millisecondsTimeout: 15000,
                                mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                            break;
                    }

                    ApplyConnectionResult(oResult, oDlg.Ssid);
                }
                catch (Exception oEx)
                {
                    Logger.Error($"Hidden network connect failed: SSID={oDlg.Ssid}", oEx);
                    lblStatus.Text = "Status: Connect failed";
                    MessageBox.Show(this, "Unable to connect to the hidden network: " + oEx.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    m_bSuppressOperationNotifications = false;
                }
            }
        }

        private async void btnConnectEnterprise_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null)
            {
                return;
            }

            string strPreselectedSsid = lvNetworks.SelectedItems.Count > 0
                ? ((WifiNetwork)lvNetworks.SelectedItems[0].Tag).Ssid
                : string.Empty;

            using (var oDlg = new EnterpriseCredentialsDialog(strPreselectedSsid))
            {
                if (oDlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    lblStatus.Text = $"Status: Connecting to {oDlg.Ssid} (Enterprise)...";
                    m_strLastConnectAttemptSsid = oDlg.Ssid;
                    m_bSuppressOperationNotifications = true;

                    WifiConnectionResult oResult;

                    if (oDlg.IsEapTls)
                    {
                        oResult = await Task.Run(() => m_oClient.ConnectEnterpriseEapTlsAndWait(
                            oDlg.Ssid,
                            oDlg.ClientCertThumbprint,
                            trustedRootCaThumbprint: oDlg.TrustedRootCaThumbprint,
                            disableUserPromptForServerValidation: oDlg.DisableUserPromptForServerValidation,
                            millisecondsTimeout: 15000,
                            mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                    }
                    else
                    {
                        oResult = await Task.Run(() => m_oClient.ConnectEnterpriseAndWait(
                            oDlg.Ssid,
                            oDlg.Username,
                            oDlg.Password,
                            oDlg.Domain,
                            trustedRootCaThumbprint: oDlg.TrustedRootCaThumbprint,
                            disableUserPromptForServerValidation: oDlg.DisableUserPromptForServerValidation,
                            millisecondsTimeout: 15000,
                            mergeDuplicateBssids: chkMergeDuplicateBssids.Checked));
                    }

                    ApplyConnectionResult(oResult, oDlg.Ssid);
                }
                catch (Exception oEx)
                {
                    Logger.Error($"Enterprise connect failed: SSID={oDlg.Ssid}", oEx);
                    lblStatus.Text = "Status: Connect failed";
                    MessageBox.Show(this, "Unable to connect to the Enterprise network: " + oEx.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    m_bSuppressOperationNotifications = false;
                }
            }
        }

        private void ApplyConnectionResult(WifiConnectionResult result, string ssid)
        {
            RefreshNetworkList(announceCount: false);

            if (result == null)
            {
                lblStatus.Text = "Status: Connect result was not available";
                return;
            }

            if (result.IsSuccess)
            {
                lblStatus.Text = $"Status: Connected to {ssid}";
                return;
            }

            lblStatus.Text = "Status: Connect attempt failed";

            if (chkDeleteProfileOnAuthFailure.Checked && !string.IsNullOrEmpty(m_strLastConnectAttemptSsid))
            {
                TryDeleteProfileAfterAuthFailure(m_strLastConnectAttemptSsid);
            }

            MessageBox.Show(this, result.Message, "Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void TryDeleteProfileAfterAuthFailure(string ssid)
        {
            try
            {
                m_oClient.DeleteSavedProfile(ssid);
                Logger.Info($"[{CurrentInterface.Name}] Deleted profile after auth failure: SSID={ssid}");
                RefreshNetworkList(announceCount: false);
                lblStatus.Text = $"Status: Connect attempt failed (profile for {ssid} deleted)";
            }
            catch (Exception oEx)
            {
                Logger.Error($"Delete profile after auth failure failed: SSID={ssid}", oEx);
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null || lvNetworks.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select a network from the list.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oNetwork = (WifiNetwork)lvNetworks.SelectedItems[0].Tag;
            if (!m_oClient.HasProfile(oNetwork.Ssid))
            {
                MessageBox.Show(this, $"\"{oNetwork.Ssid}\" has no saved profile to delete.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var eConfirm = MessageBox.Show(this, $"Delete the saved profile for \"{oNetwork.Ssid}\"?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (eConfirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                m_oClient.DeleteSavedProfile(oNetwork.ProfileName);
                RefreshNetworkList(announceCount: false);
                lblStatus.Text = $"Status: Deleted profile for {oNetwork.Ssid}";
            }
            catch (Exception oEx)
            {
                Logger.Error($"Delete profile failed: SSID={oNetwork.Ssid}", oEx);
                lblStatus.Text = "Status: Delete profile failed";
                MessageBox.Show(this, "Unable to delete the profile: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (CurrentInterface == null)
            {
                return;
            }

            try
            {
                m_oClient.Disconnect();
                lblStatus.Text = "Status: Disconnected";
            }
            catch (Exception oEx)
            {
                Logger.Error("Disconnect failed", oEx);
                MessageBox.Show(this, "Unable to disconnect: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (m_oClient != null)
            {
                m_oClient.Notification -= Client_Notification;
                m_oClient.Dispose();
            }
        }
    }
}
