namespace DHWifiClientSample {
	partial class FrmMain {
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
			this.btnCheckWifi = new System.Windows.Forms.Button();
			this.lblWifiCheckResult = new System.Windows.Forms.Label();
			this.lblAdapter = new System.Windows.Forms.Label();
			this.cmbAdapter = new System.Windows.Forms.ComboBox();
			this.lblRadioState = new System.Windows.Forms.Label();
			this.btnRadioToggle = new System.Windows.Forms.Button();
			this.btnScan = new System.Windows.Forms.Button();
			this.chkIncludeEmptySsids = new System.Windows.Forms.CheckBox();
			this.lvNetworks = new System.Windows.Forms.ListView();
			this.colSsid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSignal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colSecurity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colCipher = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colBssid = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnConnect = new System.Windows.Forms.Button();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnHiddenNetwork = new System.Windows.Forms.Button();
			this.btnDeleteProfile = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.chkFileLog = new System.Windows.Forms.CheckBox();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnCheckWifi
			// 
			this.btnCheckWifi.Location = new System.Drawing.Point(12, 9);
			this.btnCheckWifi.Name = "btnCheckWifi";
			this.btnCheckWifi.Size = new System.Drawing.Size(110, 26);
			this.btnCheckWifi.TabIndex = 0;
			this.btnCheckWifi.Text = "Check WiFi";
			this.btnCheckWifi.UseVisualStyleBackColor = true;
			this.btnCheckWifi.Click += new System.EventHandler(this.btnCheckWifi_Click);
			// 
			// lblWifiCheckResult
			// 
			this.lblWifiCheckResult.AutoSize = true;
			this.lblWifiCheckResult.Location = new System.Drawing.Point(132, 15);
			this.lblWifiCheckResult.Name = "lblWifiCheckResult";
			this.lblWifiCheckResult.Size = new System.Drawing.Size(77, 13);
			this.lblWifiCheckResult.TabIndex = 25;
			this.lblWifiCheckResult.Text = "WiFi Adapter: -";
			// 
			// lblAdapter
			// 
			this.lblAdapter.AutoSize = true;
			this.lblAdapter.Location = new System.Drawing.Point(12, 48);
			this.lblAdapter.Name = "lblAdapter";
			this.lblAdapter.Size = new System.Drawing.Size(47, 13);
			this.lblAdapter.TabIndex = 24;
			this.lblAdapter.Text = "Adapter:";
			// 
			// cmbAdapter
			// 
			this.cmbAdapter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbAdapter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbAdapter.FormattingEnabled = true;
			this.cmbAdapter.Location = new System.Drawing.Point(65, 45);
			this.cmbAdapter.Name = "cmbAdapter";
			this.cmbAdapter.Size = new System.Drawing.Size(338, 21);
			this.cmbAdapter.TabIndex = 1;
			this.cmbAdapter.SelectedIndexChanged += new System.EventHandler(this.cmbAdapter_SelectedIndexChanged);
			// 
			// lblRadioState
			// 
			this.lblRadioState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblRadioState.AutoSize = true;
			this.lblRadioState.Location = new System.Drawing.Point(409, 49);
			this.lblRadioState.Name = "lblRadioState";
			this.lblRadioState.Size = new System.Drawing.Size(44, 13);
			this.lblRadioState.TabIndex = 22;
			this.lblRadioState.Text = "Radio: -";
			// 
			// btnRadioToggle
			// 
			this.btnRadioToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRadioToggle.Location = new System.Drawing.Point(489, 42);
			this.btnRadioToggle.Name = "btnRadioToggle";
			this.btnRadioToggle.Size = new System.Drawing.Size(90, 26);
			this.btnRadioToggle.TabIndex = 2;
			this.btnRadioToggle.Text = "ON/OFF";
			this.btnRadioToggle.UseVisualStyleBackColor = true;
			this.btnRadioToggle.Click += new System.EventHandler(this.btnRadioToggle_Click);
			// 
			// btnScan
			// 
			this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnScan.Location = new System.Drawing.Point(585, 42);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(90, 26);
			this.btnScan.TabIndex = 3;
			this.btnScan.Text = "Scan";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			//
			// chkIncludeEmptySsids
			//
			this.chkIncludeEmptySsids.AutoSize = true;
			this.chkIncludeEmptySsids.Checked = true;
			this.chkIncludeEmptySsids.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkIncludeEmptySsids.Location = new System.Drawing.Point(12, 74);
			this.chkIncludeEmptySsids.Name = "chkIncludeEmptySsids";
			this.chkIncludeEmptySsids.Size = new System.Drawing.Size(200, 17);
			this.chkIncludeEmptySsids.TabIndex = 26;
			this.chkIncludeEmptySsids.Text = "Show hidden (blank SSID) networks";
			this.chkIncludeEmptySsids.UseVisualStyleBackColor = true;
			this.chkIncludeEmptySsids.CheckedChanged += new System.EventHandler(this.chkIncludeEmptySsids_CheckedChanged);
			//
			// lvNetworks
			//
			this.lvNetworks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvNetworks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSsid,
            this.colSignal,
            this.colSecurity,
            this.colCipher,
            this.colBssid,
            this.colStatus});
			this.lvNetworks.FullRowSelect = true;
			this.lvNetworks.GridLines = true;
			this.lvNetworks.HideSelection = false;
			this.lvNetworks.Location = new System.Drawing.Point(12, 98);
			this.lvNetworks.MultiSelect = false;
			this.lvNetworks.Name = "lvNetworks";
			this.lvNetworks.Size = new System.Drawing.Size(663, 293);
			this.lvNetworks.TabIndex = 4;
			this.lvNetworks.UseCompatibleStateImageBehavior = false;
			this.lvNetworks.View = System.Windows.Forms.View.Details;
			this.lvNetworks.DoubleClick += new System.EventHandler(this.btnConnect_Click);
			// 
			// colSsid
			// 
			this.colSsid.Text = "SSID";
			this.colSsid.Width = 180;
			// 
			// colSignal
			// 
			this.colSignal.Text = "Signal";
			this.colSignal.Width = 80;
			// 
			// colSecurity
			// 
			this.colSecurity.Text = "Security";
			this.colSecurity.Width = 110;
			// 
			// colCipher
			// 
			this.colCipher.Text = "Cipher";
			// 
			// colBssid
			// 
			this.colBssid.Text = "BSSID";
			this.colBssid.Width = 120;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			this.colStatus.Width = 110;
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Location = new System.Drawing.Point(484, 397);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(90, 26);
			this.btnConnect.TabIndex = 7;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Location = new System.Drawing.Point(584, 397);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(90, 26);
			this.btnDisconnect.TabIndex = 8;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnHiddenNetwork
			// 
			this.btnHiddenNetwork.Location = new System.Drawing.Point(12, 397);
			this.btnHiddenNetwork.Name = "btnHiddenNetwork";
			this.btnHiddenNetwork.Size = new System.Drawing.Size(150, 26);
			this.btnHiddenNetwork.TabIndex = 5;
			this.btnHiddenNetwork.Text = "Hidden Network...";
			this.btnHiddenNetwork.UseVisualStyleBackColor = true;
			this.btnHiddenNetwork.Click += new System.EventHandler(this.btnHiddenNetwork_Click);
			// 
			// btnDeleteProfile
			// 
			this.btnDeleteProfile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDeleteProfile.Location = new System.Drawing.Point(364, 397);
			this.btnDeleteProfile.Name = "btnDeleteProfile";
			this.btnDeleteProfile.Size = new System.Drawing.Size(110, 26);
			this.btnDeleteProfile.TabIndex = 6;
			this.btnDeleteProfile.Text = "Delete Profile";
			this.btnDeleteProfile.UseVisualStyleBackColor = true;
			this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.AutoSize = true;
			this.lblStatus.Location = new System.Drawing.Point(12, 435);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(46, 13);
			this.lblStatus.TabIndex = 14;
			this.lblStatus.Text = "Status: -";
			// 
			// chkFileLog
			// 
			this.chkFileLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkFileLog.AutoSize = true;
			this.chkFileLog.Checked = true;
			this.chkFileLog.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkFileLog.Location = new System.Drawing.Point(572, 432);
			this.chkFileLog.Name = "chkFileLog";
			this.chkFileLog.Size = new System.Drawing.Size(103, 17);
			this.chkFileLog.TabIndex = 9;
			this.chkFileLog.Text = "Save Log to File";
			this.chkFileLog.UseVisualStyleBackColor = true;
			this.chkFileLog.CheckedChanged += new System.EventHandler(this.chkFileLog_CheckedChanged);
			// 
			// txtLog
			// 
			this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtLog.BackColor = System.Drawing.SystemColors.Window;
			this.txtLog.Location = new System.Drawing.Point(12, 453);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtLog.Size = new System.Drawing.Size(663, 130);
			this.txtLog.TabIndex = 10;
			this.txtLog.WordWrap = false;
			// 
			// FrmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(687, 595);
			this.Controls.Add(this.txtLog);
			this.Controls.Add(this.chkFileLog);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.btnDeleteProfile);
			this.Controls.Add(this.btnHiddenNetwork);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.lvNetworks);
			this.Controls.Add(this.chkIncludeEmptySsids);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.btnRadioToggle);
			this.Controls.Add(this.lblRadioState);
			this.Controls.Add(this.cmbAdapter);
			this.Controls.Add(this.lblAdapter);
			this.Controls.Add(this.lblWifiCheckResult);
			this.Controls.Add(this.btnCheckWifi);
			this.MinimumSize = new System.Drawing.Size(560, 477);
			this.Name = "FrmMain";
			this.Text = "DHWifiClient Sample";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMain_FormClosed);
			this.Load += new System.EventHandler(this.FrmMain_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCheckWifi;
		private System.Windows.Forms.Label lblWifiCheckResult;
		private System.Windows.Forms.Label lblAdapter;
		private System.Windows.Forms.ComboBox cmbAdapter;
		private System.Windows.Forms.Label lblRadioState;
		private System.Windows.Forms.Button btnRadioToggle;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.CheckBox chkIncludeEmptySsids;
		private System.Windows.Forms.ListView lvNetworks;
		private System.Windows.Forms.ColumnHeader colSsid;
		private System.Windows.Forms.ColumnHeader colSignal;
		private System.Windows.Forms.ColumnHeader colSecurity;
		private System.Windows.Forms.ColumnHeader colCipher;
		private System.Windows.Forms.ColumnHeader colBssid;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnHiddenNetwork;
		private System.Windows.Forms.Button btnDeleteProfile;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.CheckBox chkFileLog;
		private System.Windows.Forms.TextBox txtLog;
	}
}
