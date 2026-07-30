//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: PasswordDialog
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Windows.Forms;

namespace DHWifiClient2Sample
{
    /// <summary>Simple modal dialog for entering a password when connecting to a secured network such as WPA2-PSK.</summary>
    internal partial class PasswordDialog : Form
    {
        public string Password => txtPassword.Text;

        public PasswordDialog(string ssid)
        {
            InitializeComponent();
            lblPassword.Text = $"Password for \"{ssid}\":";
        }
    }
}
