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
    /// Modal dialog for entering the credentials needed to connect to a WPA2-Enterprise (802.1X)
    /// network, either PEAP-MSCHAPv2 (username/password) via
    /// <see cref="DHWifiClient.NET.module.WifiInterface.ConnectEnterprise"/> or EAP-TLS (client
    /// certificate) via <see cref="DHWifiClient.NET.module.WifiInterface.ConnectEnterpriseEapTls"/>.
    /// </summary>
    internal partial class EnterpriseCredentialsDialog : Form
    {
        public string Ssid => txtSsid.Text.Trim();
        public bool IsEapTls => rbEapTls.Checked;
        public string Username => txtUsername.Text.Trim();
        public string Password => txtPassword.Text;
        public string Domain => string.IsNullOrWhiteSpace(txtDomain.Text) ? null : txtDomain.Text.Trim();
        public string ClientCertThumbprint => string.IsNullOrWhiteSpace(txtClientCertThumbprint.Text) ? null : txtClientCertThumbprint.Text.Trim();
        public string TrustedRootCaThumbprint => string.IsNullOrWhiteSpace(txtTrustedRootCa.Text) ? null : txtTrustedRootCa.Text.Trim();
        public bool DisableUserPromptForServerValidation => chkNoPrompt.Checked;

        public EnterpriseCredentialsDialog(string ssid)
        {
            InitializeComponent();
            txtSsid.Text = ssid;
            UpdateFieldsForEapMethod();
        }

        private void EapMethod_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateFieldsForEapMethod();
        }

        private void UpdateFieldsForEapMethod()
        {
            bool bEapTls = rbEapTls.Checked;

            lblUsername.Visible = !bEapTls;
            txtUsername.Visible = !bEapTls;
            lblPassword.Visible = !bEapTls;
            txtPassword.Visible = !bEapTls;
            lblDomain.Visible = !bEapTls;
            txtDomain.Visible = !bEapTls;

            lblClientCertThumbprint.Visible = bEapTls;
            txtClientCertThumbprint.Visible = bEapTls;
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

            if (!IsEapTls)
            {
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
}
