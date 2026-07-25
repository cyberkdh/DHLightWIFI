//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: PasswordDialog
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH@HOTMAIL.COM. All Rights Reserved.
//////////////////////////////////////////////////////////////////////////////////////////////////
using System.Windows.Forms;

namespace DHWifiClientSample
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
