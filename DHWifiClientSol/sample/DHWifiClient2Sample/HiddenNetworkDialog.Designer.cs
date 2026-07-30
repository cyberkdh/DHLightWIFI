//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: HiddenNetworkDialog.Designer
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DHWifiClient2Sample {
	partial class HiddenNetworkDialog {
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		private void InitializeComponent() {
			this.lblSsid = new System.Windows.Forms.Label();
			this.txtSsid = new System.Windows.Forms.TextBox();
			this.lblSecurity = new System.Windows.Forms.Label();
			this.cmbSecurity = new System.Windows.Forms.ComboBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			this.lblSsid.AutoSize = true;
			this.lblSsid.Location = new System.Drawing.Point(12, 15);
			this.lblSsid.Name = "lblSsid";
			this.lblSsid.Size = new System.Drawing.Size(113, 13);
			this.lblSsid.TabIndex = 7;
			this.lblSsid.Text = "Network name (SSID):";
			this.txtSsid.Location = new System.Drawing.Point(12, 34);
			this.txtSsid.Name = "txtSsid";
			this.txtSsid.Size = new System.Drawing.Size(296, 20);
			this.txtSsid.TabIndex = 0;
			this.lblSecurity.AutoSize = true;
			this.lblSecurity.Location = new System.Drawing.Point(12, 67);
			this.lblSecurity.Name = "lblSecurity";
			this.lblSecurity.Size = new System.Drawing.Size(71, 13);
			this.lblSecurity.TabIndex = 5;
			this.lblSecurity.Text = "Security type:";
			this.cmbSecurity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSecurity.FormattingEnabled = true;
			this.cmbSecurity.Items.AddRange(new object[] {
			"Open (no password)",
			"WEP",
			"WPA-Personal (TKIP)",
			"WPA2-Personal (AES)"});
			this.cmbSecurity.Location = new System.Drawing.Point(12, 86);
			this.cmbSecurity.Name = "cmbSecurity";
			this.cmbSecurity.Size = new System.Drawing.Size(296, 21);
			this.cmbSecurity.TabIndex = 1;
			this.cmbSecurity.SelectedIndexChanged += new System.EventHandler(this.CmbSecurity_SelectedIndexChanged);
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(12, 119);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(84, 13);
			this.lblPassword.TabIndex = 3;
			this.lblPassword.Text = "Password / key:";
			this.txtPassword.Location = new System.Drawing.Point(12, 138);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(296, 20);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.UseSystemPasswordChar = true;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(150, 170);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 26);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(233, 170);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.AcceptButton = this.btnOk;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 208);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.cmbSecurity);
			this.Controls.Add(this.lblSecurity);
			this.Controls.Add(this.txtSsid);
			this.Controls.Add(this.lblSsid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HiddenNetworkDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect to Hidden Network";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblSsid;
		private System.Windows.Forms.TextBox txtSsid;
		private System.Windows.Forms.Label lblSecurity;
		private System.Windows.Forms.ComboBox cmbSecurity;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}
