namespace HtmlView
{
    partial class WebPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebPanel));
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.autoRefreshCheckBox = new System.Windows.Forms.CheckBox();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.settingsBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(0, -1);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(284, 234);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // autoRefreshCheckBox
            // 
            this.autoRefreshCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.autoRefreshCheckBox.AutoSize = true;
            this.autoRefreshCheckBox.Location = new System.Drawing.Point(37, 239);
            this.autoRefreshCheckBox.Name = "autoRefreshCheckBox";
            this.autoRefreshCheckBox.Size = new System.Drawing.Size(88, 17);
            this.autoRefreshCheckBox.TabIndex = 3;
            this.autoRefreshCheckBox.Text = "Auto Refresh";
            this.toolTip1.SetToolTip(this.autoRefreshCheckBox, "Auto Refresh HTML Preview ");
            this.autoRefreshCheckBox.UseVisualStyleBackColor = true;
            this.autoRefreshCheckBox.CheckedChanged += new System.EventHandler(this.autoRefreshCheckBox_CheckedChanged);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshBtn.Image = ((System.Drawing.Image)(resources.GetObject("refreshBtn.Image")));
            this.refreshBtn.Location = new System.Drawing.Point(3, 235);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(28, 23);
            this.refreshBtn.TabIndex = 4;
            this.refreshBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.refreshBtn, "Refresh HTML Preview");
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // settingsBtn
            // 
            this.settingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsBtn.Image = ((System.Drawing.Image)(resources.GetObject("settingsBtn.Image")));
            this.settingsBtn.Location = new System.Drawing.Point(3, 235);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(28, 23);
            this.settingsBtn.TabIndex = 4;
            this.settingsBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.settingsBtn, "Show Settings Dialog");
            this.settingsBtn.UseVisualStyleBackColor = true;
            this.settingsBtn.Visible = false;
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // WebPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.autoRefreshCheckBox);
            this.Controls.Add(this.settingsBtn);
            this.Controls.Add(this.webBrowser1);
            this.Name = "WebPanel";
            this.VisibleChanged += new System.EventHandler(this.WebPanel_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.CheckBox autoRefreshCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button settingsBtn;
    }
}

