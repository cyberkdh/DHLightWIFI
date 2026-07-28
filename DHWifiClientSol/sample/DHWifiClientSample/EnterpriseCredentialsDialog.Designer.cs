namespace DHWifiClientSample {
	partial class EnterpriseCredentialsDialog {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lblSsid = new System.Windows.Forms.Label();
			this.txtSsid = new System.Windows.Forms.TextBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblDomain = new System.Windows.Forms.Label();
			this.txtDomain = new System.Windows.Forms.TextBox();
			this.lblTrustedRootCa = new System.Windows.Forms.Label();
			this.txtTrustedRootCa = new System.Windows.Forms.TextBox();
			this.chkNoPrompt = new System.Windows.Forms.CheckBox();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			//
			// lblSsid
			//
			this.lblSsid.AutoSize = true;
			this.lblSsid.Location = new System.Drawing.Point(12, 15);
			this.lblSsid.Name = "lblSsid";
			this.lblSsid.Size = new System.Drawing.Size(113, 13);
			this.lblSsid.TabIndex = 8;
			this.lblSsid.Text = "Network name (SSID):";
			//
			// txtSsid
			//
			this.txtSsid.Location = new System.Drawing.Point(12, 34);
			this.txtSsid.Name = "txtSsid";
			this.txtSsid.Size = new System.Drawing.Size(296, 20);
			this.txtSsid.TabIndex = 0;
			//
			// lblUsername
			//
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(12, 67);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(58, 13);
			this.lblUsername.TabIndex = 7;
			this.lblUsername.Text = "Username:";
			//
			// txtUsername
			//
			this.txtUsername.Location = new System.Drawing.Point(12, 86);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(296, 20);
			this.txtUsername.TabIndex = 1;
			//
			// lblPassword
			//
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(12, 119);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(56, 13);
			this.lblPassword.TabIndex = 6;
			this.lblPassword.Text = "Password:";
			//
			// txtPassword
			//
			this.txtPassword.Location = new System.Drawing.Point(12, 138);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(296, 20);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.UseSystemPasswordChar = true;
			//
			// lblDomain
			//
			this.lblDomain.AutoSize = true;
			this.lblDomain.Location = new System.Drawing.Point(12, 171);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(89, 13);
			this.lblDomain.TabIndex = 5;
			this.lblDomain.Text = "Domain (optional):";
			//
			// txtDomain
			//
			this.txtDomain.Location = new System.Drawing.Point(12, 190);
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.Size = new System.Drawing.Size(296, 20);
			this.txtDomain.TabIndex = 3;
			//
			// lblTrustedRootCa
			//
			this.lblTrustedRootCa.AutoSize = true;
			this.lblTrustedRootCa.Location = new System.Drawing.Point(12, 223);
			this.lblTrustedRootCa.Name = "lblTrustedRootCa";
			this.lblTrustedRootCa.Size = new System.Drawing.Size(200, 13);
			this.lblTrustedRootCa.TabIndex = 9;
			this.lblTrustedRootCa.Text = "Trusted Root CA thumbprint (optional):";
			//
			// txtTrustedRootCa
			//
			this.txtTrustedRootCa.Location = new System.Drawing.Point(12, 242);
			this.txtTrustedRootCa.Name = "txtTrustedRootCa";
			this.txtTrustedRootCa.Size = new System.Drawing.Size(296, 20);
			this.txtTrustedRootCa.TabIndex = 4;
			//
			// chkNoPrompt
			//
			this.chkNoPrompt.AutoSize = true;
			this.chkNoPrompt.Location = new System.Drawing.Point(12, 268);
			this.chkNoPrompt.Name = "chkNoPrompt";
			this.chkNoPrompt.Size = new System.Drawing.Size(240, 17);
			this.chkNoPrompt.TabIndex = 5;
			this.chkNoPrompt.Text = "Disable server-certificate trust prompt";
			this.chkNoPrompt.UseVisualStyleBackColor = true;
			//
			// btnOk
			//
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(150, 296);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 26);
			this.btnOk.TabIndex = 6;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
			//
			// btnCancel
			//
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(233, 296);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 26);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			//
			// EnterpriseCredentialsDialog
			//
			this.AcceptButton = this.btnOk;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(320, 334);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.chkNoPrompt);
			this.Controls.Add(this.txtTrustedRootCa);
			this.Controls.Add(this.lblTrustedRootCa);
			this.Controls.Add(this.txtDomain);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.lblUsername);
			this.Controls.Add(this.txtSsid);
			this.Controls.Add(this.lblSsid);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EnterpriseCredentialsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Connect to Enterprise Network (PEAP-MSCHAPv2)";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblSsid;
		private System.Windows.Forms.TextBox txtSsid;
		private System.Windows.Forms.Label lblUsername;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblDomain;
		private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.Label lblTrustedRootCa;
		private System.Windows.Forms.TextBox txtTrustedRootCa;
		private System.Windows.Forms.CheckBox chkNoPrompt;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}
