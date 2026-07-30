//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: HiddenNetworkDialog
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Windows.Forms;

namespace DHWifiClient2Sample
{
    internal enum HiddenNetworkSecurityType
    {
        Open,
        Wep,
        WpaPersonalTkip,
        Wpa2PersonalAes,
    }

    internal partial class HiddenNetworkDialog : Form
    {
        public string Ssid => txtSsid.Text.Trim();
        public string Password => txtPassword.Text;
        public HiddenNetworkSecurityType SecurityType => (HiddenNetworkSecurityType)cmbSecurity.SelectedIndex;

        public HiddenNetworkDialog()
        {
            InitializeComponent();
            cmbSecurity.SelectedIndex = (int)HiddenNetworkSecurityType.Wpa2PersonalAes;
        }

        private void CmbSecurity_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtPassword.Enabled = SecurityType != HiddenNetworkSecurityType.Open;
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

            if (SecurityType != HiddenNetworkSecurityType.Open && string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show(this, "Please enter the password / key.", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.None;
                return;
            }
        }
    }
}
