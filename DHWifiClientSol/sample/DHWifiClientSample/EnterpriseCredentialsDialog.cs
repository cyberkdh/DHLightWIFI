//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: EnterpriseCredentialsDialog
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Windows.Forms;

namespace DHWifiClientSample
{
    /// <summary>
    /// Modal dialog for entering the RADIUS/AD credentials needed to connect to a WPA2-Enterprise
    /// (802.1X, PEAP-MSCHAPv2) network via <see cref="DHWifiClient.NET.module.WifiInterface.ConnectEnterprise"/>.
    /// </summary>
    internal partial class EnterpriseCredentialsDialog : Form
    {
        public string Ssid => txtSsid.Text.Trim();
        public string Username => txtUsername.Text.Trim();
        public string Password => txtPassword.Text;
        public string Domain => string.IsNullOrWhiteSpace(txtDomain.Text) ? null : txtDomain.Text.Trim();
        public string TrustedRootCaThumbprint => string.IsNullOrWhiteSpace(txtTrustedRootCa.Text) ? null : txtTrustedRootCa.Text.Trim();
        public bool DisableUserPromptForServerValidation => chkNoPrompt.Checked;

        public EnterpriseCredentialsDialog(string ssid)
        {
            InitializeComponent();
            txtSsid.Text = ssid;
        }

        private void BtnOk_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSsid.Text))
            {
                MessageBox.Show(this, "Please enter the network name (SSID).", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show(this, "Please enter the username.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter the password.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
