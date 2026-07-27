//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: FrmMain
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DHWifiClient.NET.log;
using DHWifiClient.NET.module;

namespace DHWifiClientSample
{
    public partial class FrmMain : Form
    {
        private DHWifiClient.NET.DHWifiClient m_oClient;
        private List<WifiInterface> m_listInterfaces = new List<WifiInterface>();
        private Timer m_oScanCompleteTimer;
        private TextBoxLogWriter m_oLogWriter;

        private WifiInterface CurrentInterface => cmbAdapter.SelectedItem as WifiInterface;

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
                    m_oClient = new DHWifiClient.NET.DHWifiClient();
                    m_oClient.Notification += Client_Notification;
                }

                LoadAdapters();
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to initialize DHWifiClient", oEx);
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

            // DataSource must be assigned before DisplayMember, otherwise the combo box can keep
            // showing blank text even though SelectedItem is bound correctly.
            cmbAdapter.DataSource = null;
            cmbAdapter.DataSource = m_listInterfaces;
            cmbAdapter.DisplayMember = "Name";

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
                    m_oScanCompleteTimer?.Stop();
                    btnScan.Enabled = true;
                    RefreshNetworkList();
                    break;

                case WifiNotificationType.ScanFailed:
                    m_oScanCompleteTimer?.Stop();
                    btnScan.Enabled = true;
                    lblStatus.Text = "Status: Scan failed";
                    break;

                case WifiNotificationType.ConnectionCompleted:
                    RefreshNetworkList(announceCount: false);
                    lblStatus.Text = "Status: Connected";
                    break;

                case WifiNotificationType.ConnectionAttemptFailed:
                    RefreshNetworkList(announceCount: false);
                    lblStatus.Text = "Status: Connect attempt failed";
                    // A status-bar text change alone is easy to miss; show an explicit dialog so the
                    // failure cannot go unnoticed. Note this can still be followed by a later
                    // ConnectionCompleted notification if the driver/AP falls back to a cached PMKSA
                    // (see doc/discuss/0028) rather than fully re-validating the current profile.
                    MessageBox.Show(this, "Connection attempt failed.", "Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            btnDisconnect.Enabled = enabled;
            btnHiddenNetwork.Enabled = enabled;
            btnDeleteProfile.Enabled = enabled;
            lvNetworks.Enabled = enabled;
            chkIncludeEmptySsids.Enabled = enabled;
        }

        private void cmbAdapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshRadioState();
            RefreshNetworkList();
        }

        private void chkIncludeEmptySsids_CheckedChanged(object sender, EventArgs e)
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
                var eState = oIface.GetRadioState();
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

        /// <summary>
        /// Updates lvNetworks in place: removes rows for networks no longer in range, updates rows
        /// whose signal/status changed, and adds rows for newly seen networks. Avoids Items.Clear(),
        /// which would drop the user's current selection and cause the whole list to flicker/reset
        /// on every scan even when most rows are unchanged.
        /// </summary>
        /// <param name="announceCount">
        /// When true, overwrites lblStatus with a generic "Found N network(s)" summary. Pass false
        /// when the caller already set a more specific status message (e.g. a connect/delete result)
        /// that this refresh must not clobber.
        /// </param>
        private void RefreshNetworkList(bool announceCount = true)
        {
            var oIface = CurrentInterface;

            if (oIface == null)
            {
                lvNetworks.Items.Clear();
                Logger.Debug("RefreshNetworkList skipped: no adapter is currently selected");
                return;
            }

            try
            {
                // BSSID resolution is enabled here so that duplicate/blank-SSID rows (e.g. hidden
                // networks) can be told apart by their actual AP MAC address; see the Bssids= entries
                // in the debug log below.
                var listNetworks = oIface.GetAvailableNetworks(includeBssids: true, includeEmptySsids: chkIncludeEmptySsids.Checked);
                Logger.Debug($"[{oIface.Name}] Network list: [{string.Join("\r\n", listNetworks.Select(n => $"{n.Ssid}({n.Authentication}/{n.Cipher}) Bssids=[{string.Join(",", n.Bssids)}]"))}]");

                // WlanGetAvailableNetworkList's HasProfile flag reflects wlansvc's cached scan
                // snapshot, which can briefly still report a profile as present right after
                // WlanDeleteProfile succeeds. WlanGetProfileList reads the profile store directly, so
                // it is used as the authoritative source for whether a profile is actually saved.
                var setSavedProfileNames = new HashSet<string>(oIface.GetProfiles().Select(p => p.ProfileName));

                // The same SSID can legitimately appear more than once (multiple BSSes/mesh nodes
                // broadcasting the same name, or multiple hidden networks all showing an empty SSID),
                // so networks are grouped into per-SSID queues rather than keyed 1:1 by SSID.
                //
                // Exception: for a named (non-hidden) SSID, WlanGetAvailableNetworkList can report a
                // non-broadcast network with a saved profile as two entries (one profile-matched/
                // connected, one raw scan hit) even though there is only one physical AP. When one of
                // the duplicates is Connected, the other is a phantom of that same quirk, so it is
                // dropped. Blank (hidden) SSIDs are left untouched since those duplicates are usually
                // genuinely different APs.
                var dictNetworksBySsid = listNetworks
                    .GroupBy(n => n.Ssid)
                    .ToDictionary(
                        g => g.Key,
                        g => new Queue<WifiNetwork>(
                            !string.IsNullOrEmpty(g.Key) && g.Any(n => n.IsConnected)
                                ? g.Where(n => n.IsConnected)
                                : g));

                // Remove rows for networks that dropped out of range.
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

                // Add rows for any networks left over (newly seen SSIDs, or extra duplicates of an
                // SSID that now has more entries than before).
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
            }
            catch (Exception oEx)
            {
                Logger.Error("Failed to query network list", oEx);
                // The most common cause is the radio being off (whether turned off via the button
                // above or externally, e.g. a hardware switch), in which case a stale network list
                // would be misleading, so clear it rather than leaving old rows on screen.
                lvNetworks.Items.Clear();
                lblStatus.Text = "Status: Failed to query network list";
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
                var eCurrent = oIface.GetRadioState();
                bool bTurnOn = eCurrent != WifiRadioState.On;
                oIface.SetRadioState(bTurnOn);
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

        private void btnScan_Click(object sender, EventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null)
            {
                return;
            }

            try
            {
                btnScan.Enabled = false;
                lblStatus.Text = "Status: Scanning...";
                oIface.Scan();
            }
            catch (Exception oEx)
            {
                Logger.Error("Scan request failed", oEx);
                btnScan.Enabled = true;
                lblStatus.Text = "Status: Scan request failed";
                return;
            }

            // Safety-net fallback in case the wlan_notification_acm_scan_complete notification never arrives.
            m_oScanCompleteTimer?.Stop();
            m_oScanCompleteTimer = new Timer { Interval = 6000 };
            m_oScanCompleteTimer.Tick += (s, args) =>
            {
                m_oScanCompleteTimer.Stop();
                btnScan.Enabled = true;
                RefreshNetworkList();
            };
            m_oScanCompleteTimer.Start();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null || lvNetworks.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select a network from the list to connect to.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oNetwork = (WifiNetwork)lvNetworks.SelectedItems[0].Tag;
            string strPassword = null;

            if (oNetwork.SecurityEnabled && !oNetwork.HasProfile)
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
                oIface.Connect(oNetwork.Ssid, strPassword);
                lblStatus.Text = $"Status: Connect request for {oNetwork.Ssid} completed";
            }
            catch (Exception oEx)
            {
                Logger.Error($"Connect failed: SSID={oNetwork.Ssid}", oEx);
                lblStatus.Text = "Status: Connect failed";
                MessageBox.Show(this, "Unable to connect to the network: " + oEx.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHiddenNetwork_Click(object sender, EventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null)
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
                    Logger.Info($"[{oIface.Name}] Hidden network connect attempt: SSID={oDlg.Ssid}, Security={oDlg.SecurityType}"); // Never log the password

                    switch (oDlg.SecurityType)
                    {
                        case HiddenNetworkSecurityType.Open:
                            oIface.Connect(oDlg.Ssid, isHidden: true);
                            break;

                        case HiddenNetworkSecurityType.Wep:
                            oIface.ConnectWep(oDlg.Ssid, oDlg.Password, isHidden: true);
                            break;

                        case HiddenNetworkSecurityType.WpaPersonalTkip:
                            oIface.Connect(oDlg.Ssid, oDlg.Password, WifiPskProtocol.WPA, WifiCipher.TKIP, isHidden: true);
                            break;

                        case HiddenNetworkSecurityType.Wpa2PersonalAes:
                        default:
                            oIface.Connect(oDlg.Ssid, oDlg.Password, WifiPskProtocol.WPA2, WifiCipher.AES, isHidden: true);
                            break;
                    }

                    lblStatus.Text = $"Status: Connect request for {oDlg.Ssid} completed";
                }
                catch (Exception oEx)
                {
                    Logger.Error($"Hidden network connect failed: SSID={oDlg.Ssid}", oEx);
                    lblStatus.Text = "Status: Connect failed";
                    MessageBox.Show(this, "Unable to connect to the hidden network: " + oEx.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {
            var oIface = CurrentInterface;
            if (oIface == null || lvNetworks.SelectedItems.Count == 0)
            {
                MessageBox.Show(this, "Please select a network from the list.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var oNetwork = (WifiNetwork)lvNetworks.SelectedItems[0].Tag;
            if (!oNetwork.HasProfile)
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
                oIface.DeleteProfile(oNetwork.ProfileName);
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
            var oIface = CurrentInterface;
            if (oIface == null)
            {
                return;
            }

            try
            {
                oIface.Disconnect();
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
            m_oScanCompleteTimer?.Stop();
            if (m_oClient != null)
            {
                m_oClient.Notification -= Client_Notification;
                m_oClient.Dispose();
            }
        }
    }
}
