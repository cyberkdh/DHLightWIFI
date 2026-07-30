//////////////////////////////////////////////////////////////////////////////////////////////////
//	Projects		: DHWifiClient
//	Author			: CYBERKDH
//	Module			: EnterpriseCredentialsDialog.Designer
//	History			:
//	Copyrights		: Copyright (C)CYBERKDH. Licensed under the MIT License.
//////////////////////////////////////////////////////////////////////////////////////////////////

namespace DHWifiClient2Sample {
	partial class EnterpriseCredentialsDialog {
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
			this.lblEapMethod = new System.Windows.Forms.Label();
			this.rbPeap = new System.Windows.Forms.RadioButton();
			this.rbEapTls = new System.Windows.Forms.RadioButton();
			this.lblUsername = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblDomain = new System.Windows.Forms.Label();
			this.txtDomain = new System.Windows.Forms.TextBox();
			this.lblClientCertThumbprint = new System.Windows.Forms.Label();
			this.txtClientCertThumbprint = new System.Windows.Forms.TextBox();
			this.lblTrustedRootCa = new System.Windows.Forms.Label();
			this.txtTrustedRootCa = new System.Windows.Forms.TextBox();
			this.chkNoPrompt = new System.Windows.Forms.CheckBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			this.lblSsid.AutoSize = true;
			this.lblSsid.Location = new System.Drawing.Point(12, 15);
			this.lblSsid.Name = "lblSsid";
			this.lblSsid.Size = new System.Drawing.Size(113, 13);
			this.lblSsid.TabIndex = 0;
			this.lblSsid.Text = "Network name (SSID):";
			this.txtSsid.Location = new System.Drawing.Point(12, 34);
			this.txtSsid.Name = "txtSsid";
			this.txtSsid.Size = new System.Drawing.Size(376, 20);
			this.txtSsid.TabIndex = 0;
			this.lblEapMethod.AutoSize = true;
			this.lblEapMethod.Location = new System.Drawing.Point(12, 64);
			this.lblEapMethod.Name = "lblEapMethod";
			this.lblEapMethod.Size = new System.Drawing.Size(70, 13);
			this.lblEapMethod.TabIndex = 0;
			this.lblEapMethod.Text = "EAP Method:";
			this.rbPeap.AutoSize = true;
			this.rbPeap.Checked = true;
			this.rbPeap.Location = new System.Drawing.Point(12, 83);
			this.rbPeap.Name = "rbPeap";
			this.rbPeap.Size = new System.Drawing.Size(140, 17);
			this.rbPeap.TabIndex = 1;
			this.rbPeap.TabStop = true;
			this.rbPeap.Text = "PEAP-MSCHAPv2 (id/pw)";
			this.rbPeap.UseVisualStyleBackColor = true;
			this.rbPeap.CheckedChanged += new System.EventHandler(this.EapMethod_CheckedChanged);
			this.rbEapTls.AutoSize = true;
			this.rbEapTls.Location = new System.Drawing.Point(190, 83);
			this.rbEapTls.Name = "rbEapTls";
			this.rbEapTls.Size = new System.Drawing.Size(120, 17);
			this.rbEapTls.TabIndex = 2;
			this.rbEapTls.Text = "EAP-TLS (certificate)";
			this.rbEapTls.UseVisualStyleBackColor = true;
			this.rbEapTls.CheckedChanged += new System.EventHandler(this.EapMethod_CheckedChanged);
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(12, 113);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(58, 13);
			this.lblUsername.TabIndex = 0;
			this.lblUsername.Text = "Username:";
			this.txtUsername.Location = new System.Drawing.Point(12, 132);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(376, 20);
			this.txtUsername.TabIndex = 3;
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(12, 165);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 0;
			this.lblPassword.Text = "Password:";
			this.txtPassword.Location = new System.Drawing.Point(12, 184);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(376, 20);
			this.txtPassword.TabIndex = 4;
			this.txtPassword.UseSystemPasswordChar = true;
			this.lblDomain.AutoSize = true;
			this.lblDomain.Location = new System.Drawing.Point(12, 217);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(89, 13);
			this.lblDomain.TabIndex = 0;
			this.lblDomain.Text = "Domain (optional):";
			this.txtDomain.Location = new System.Drawing.Point(12, 236);
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.Size = new System.Drawing.Size(376, 20);
			this.txtDomain.TabIndex = 5;
			this.lblClientCertThumbprint.AutoSize = true;
			this.lblClientCertThumbprint.Location = new System.Drawing.Point(12, 217);
			this.lblClientCertThumbprint.Name = "lblClientCertThumbprint";
			this.lblClientCertThumbprint.Size = new System.Drawing.Size(300, 13);
			this.lblClientCertThumbprint.TabIndex = 0;
			this.lblClientCertThumbprint.Text = "Client certificate SHA1 thumbprint (optional = auto-select):";
			this.txtClientCertThumbprint.Location = new System.Drawing.Point(12, 236);
			this.txtClientCertThumbprint.Name = "txtClientCertThumbprint";
			this.txtClientCertThumbprint.Size = new System.Drawing.Size(376, 20);
			this.txtClientCertThumbprint.TabIndex = 6;
			this.lblTrustedRootCa.AutoSize = true;
			this.lblTrustedRootCa.Location = new System.Drawing.Point(12, 269);
			this.lblTrustedRootCa.Name = "lblTrustedRootCa";
			this.lblTrustedRootCa.Size = new System.Drawing.Size(200, 13);
			this.lblTrustedRootCa.TabIndex = 0;
			this.lblTrustedRootCa.Text = "Trusted Root CA thumbprint (optional):";
			this.txtTrustedRootCa.Location = new System.Drawing.Point(12, 288);
			this.txtTrustedRootCa.Name = "txtTrustedRootCa";
			this.txtTrustedRootCa.Size = new System.Drawing.Size(376, 20);
			this.txtTrustedRootCa.TabIndex = 7;
			this.chkNoPrompt.AutoSize = true;
			this.chkNoPrompt.Location = new System.Drawing.Point(12, 314);
			this.chkNoPrompt.Name = "chkNoPrompt";
			this.chkNoPrompt.Size = new System.Drawing.Size(240, 17);
			this.chkNoPrompt.TabIndex = 8;
			this.chkNoPrompt.Text = "Disable server-certificate trust prompt";
			this.chkNoPrompt.UseVisualStyleBackColor = true;
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(230, 342);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 26);
			this.btnOk.TabIndex = 9;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(313, 342);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.AcceptButton = this.btnOk;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(400, 380);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.chkNoPrompt);
			this.Controls.Add(this.txtTrustedRootCa);
			this.Controls.Add(this.lblTrustedRootCa);
			this.Controls.Add(this.txtClientCertThumbprint);
			this.Controls.Add(this.lblClientCertThumbprint);
			this.Controls.Add(this.txtDomain);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.lblUsername);
			this.Controls.Add(this.rbEapTls);
			this.Controls.Add(this.rbPeap);
			this.Controls.Add(this.lblEapMethod);
			this.Controls.Add(this.txtSsid);
			this.Controls.Add(this.lblSsid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EnterpriseCredentialsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect to Enterprise Network (802.1X)";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblSsid;
		private System.Windows.Forms.TextBox txtSsid;
		private System.Windows.Forms.Label lblEapMethod;
		private System.Windows.Forms.RadioButton rbPeap;
		private System.Windows.Forms.RadioButton rbEapTls;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblDomain;
		private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.Label lblClientCertThumbprint;
		private System.Windows.Forms.TextBox txtClientCertThumbprint;
		private System.Windows.Forms.Label lblTrustedRootCa;
		private System.Windows.Forms.TextBox txtTrustedRootCa;
		private System.Windows.Forms.CheckBox chkNoPrompt;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}
