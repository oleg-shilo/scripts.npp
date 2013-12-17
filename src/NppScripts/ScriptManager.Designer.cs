#pragma warning disable 1591

namespace NppScripts
{
    partial class ScriptManager
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
            this.scriptsList = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.deleteBtn = new System.Windows.Forms.Button();
            this.newBtn = new System.Windows.Forms.Button();
            this.runBtn = new System.Windows.Forms.Button();
            this.validateBtn = new System.Windows.Forms.Button();
            this.reloadBtn = new System.Windows.Forms.Button();
            this.disableBtn = new System.Windows.Forms.Button();
            this.openInVsBtn = new System.Windows.Forms.Button();
            this.aboutBtn = new System.Windows.Forms.Button();
            this.refreshBtn = new System.Windows.Forms.Button();
            this.synchBtn = new System.Windows.Forms.Button();
            this.editBtn = new System.Windows.Forms.Button();
            this.hlpBtn = new System.Windows.Forms.Button();
            this.folderOpenBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scriptsList
            // 
            this.scriptsList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scriptsList.FormattingEnabled = true;
            this.scriptsList.Location = new System.Drawing.Point(3, 25);
            this.scriptsList.Name = "scriptsList";
            this.scriptsList.Size = new System.Drawing.Size(388, 199);
            this.scriptsList.TabIndex = 1;
            this.scriptsList.SelectedIndexChanged += new System.EventHandler(this.scriptsList_SelectedIndexChanged);
            this.scriptsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.scriptsList_MouseDoubleClick);
            // 
            // deleteBtn
            // 
            this.deleteBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteBtn.Image = global::NppScripts.Resources.Resources.delete;
            this.deleteBtn.Location = new System.Drawing.Point(34, 0);
            this.deleteBtn.Name = "deleteBtn";
            this.deleteBtn.Size = new System.Drawing.Size(28, 23);
            this.deleteBtn.TabIndex = 2;
            this.deleteBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.deleteBtn, "Delete selected script");
            this.deleteBtn.UseVisualStyleBackColor = true;
            this.deleteBtn.Click += new System.EventHandler(this.deleteBtn_Click);
            // 
            // newBtn
            // 
            this.newBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newBtn.Image = global::NppScripts.Resources.Resources.add;
            this.newBtn.Location = new System.Drawing.Point(3, 0);
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(28, 23);
            this.newBtn.TabIndex = 2;
            this.newBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.newBtn, "Create new script");
            this.newBtn.UseVisualStyleBackColor = true;
            this.newBtn.Click += new System.EventHandler(this.newBtn_Click);
            // 
            // runBtn
            // 
            this.runBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.runBtn.Image = global::NppScripts.Resources.Resources.run;
            this.runBtn.Location = new System.Drawing.Point(96, 0);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(28, 23);
            this.runBtn.TabIndex = 2;
            this.runBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.runBtn, "Run selected script");
            this.runBtn.UseVisualStyleBackColor = true;
            this.runBtn.Click += new System.EventHandler(this.runBtn_Click);
            // 
            // validateBtn
            // 
            this.validateBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.validateBtn.Image = global::NppScripts.Resources.Resources.check;
            this.validateBtn.Location = new System.Drawing.Point(65, 0);
            this.validateBtn.Name = "validateBtn";
            this.validateBtn.Size = new System.Drawing.Size(28, 23);
            this.validateBtn.TabIndex = 2;
            this.validateBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.validateBtn, "Check script for errors");
            this.validateBtn.UseVisualStyleBackColor = true;
            this.validateBtn.Click += new System.EventHandler(this.validateBtn_Click);
            // 
            // reloadBtn
            // 
            this.reloadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reloadBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.reloadBtn.Image = global::NppScripts.Resources.Resources.reload;
            this.reloadBtn.Location = new System.Drawing.Point(34, 225);
            this.reloadBtn.Name = "reloadBtn";
            this.reloadBtn.Size = new System.Drawing.Size(28, 23);
            this.reloadBtn.TabIndex = 2;
            this.reloadBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.reloadBtn, "Reload Notepad++");
            this.reloadBtn.UseVisualStyleBackColor = true;
            this.reloadBtn.Click += new System.EventHandler(this.reloadBtn_Click);
            // 
            // disableBtn
            // 
            this.disableBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.disableBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.disableBtn.Image = global::NppScripts.Resources.Resources.disable;
            this.disableBtn.Location = new System.Drawing.Point(247, 228);
            this.disableBtn.Name = "disableBtn";
            this.disableBtn.Size = new System.Drawing.Size(28, 23);
            this.disableBtn.TabIndex = 2;
            this.disableBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.disableBtn, "Disable selected script");
            this.disableBtn.UseVisualStyleBackColor = true;
            this.disableBtn.Visible = false;
            // 
            // openInVsBtn
            // 
            this.openInVsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openInVsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openInVsBtn.Image = global::NppScripts.Resources.Resources.vs;
            this.openInVsBtn.Location = new System.Drawing.Point(96, 225);
            this.openInVsBtn.Name = "openInVsBtn";
            this.openInVsBtn.Size = new System.Drawing.Size(28, 23);
            this.openInVsBtn.TabIndex = 2;
            this.openInVsBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.openInVsBtn, "Open With Visual Studio");
            this.openInVsBtn.UseVisualStyleBackColor = true;
            this.openInVsBtn.Click += new System.EventHandler(this.openInVsBtn_Click);
            // 
            // aboutBtn
            // 
            this.aboutBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.aboutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.aboutBtn.Image = global::NppScripts.Resources.Resources.about;
            this.aboutBtn.Location = new System.Drawing.Point(65, 225);
            this.aboutBtn.Name = "aboutBtn";
            this.aboutBtn.Size = new System.Drawing.Size(28, 23);
            this.aboutBtn.TabIndex = 2;
            this.aboutBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.aboutBtn, "About NppScripts");
            this.aboutBtn.UseVisualStyleBackColor = true;
            this.aboutBtn.Click += new System.EventHandler(this.aboutBtn_Click);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.refreshBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refreshBtn.Image = global::NppScripts.Resources.Resources.refresh;
            this.refreshBtn.Location = new System.Drawing.Point(3, 225);
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Size = new System.Drawing.Size(28, 23);
            this.refreshBtn.TabIndex = 2;
            this.refreshBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.refreshBtn, "Refresh script list");
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // synchBtn
            // 
            this.synchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.synchBtn.Image = global::NppScripts.Resources.Resources.synch;
            this.synchBtn.Location = new System.Drawing.Point(158, 0);
            this.synchBtn.Name = "synchBtn";
            this.synchBtn.Size = new System.Drawing.Size(28, 23);
            this.synchBtn.TabIndex = 2;
            this.synchBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.synchBtn, "Synch with opened document");
            this.synchBtn.UseVisualStyleBackColor = true;
            this.synchBtn.Click += new System.EventHandler(this.synchBtn_Click);
            // 
            // editBtn
            // 
            this.editBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.editBtn.Image = global::NppScripts.Resources.Resources.edit;
            this.editBtn.Location = new System.Drawing.Point(127, 0);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(28, 23);
            this.editBtn.TabIndex = 2;
            this.editBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.editBtn, "Edit selected script");
            this.editBtn.UseVisualStyleBackColor = true;
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // hlpBtn
            // 
            this.hlpBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hlpBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.hlpBtn.Image = global::NppScripts.Resources.Resources.Help;
            this.hlpBtn.Location = new System.Drawing.Point(127, 225);
            this.hlpBtn.Name = "hlpBtn";
            this.hlpBtn.Size = new System.Drawing.Size(28, 23);
            this.hlpBtn.TabIndex = 2;
            this.hlpBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.hlpBtn, "NppScripts Documentation");
            this.hlpBtn.UseVisualStyleBackColor = true;
            this.hlpBtn.Click += new System.EventHandler(this.hlpBtn_Click);
            // 
            // folderOpenBtn
            // 
            this.folderOpenBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.folderOpenBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.folderOpenBtn.Image = global::NppScripts.Resources.Resources.folder_open;
            this.folderOpenBtn.Location = new System.Drawing.Point(158, 225);
            this.folderOpenBtn.Name = "folderOpenBtn";
            this.folderOpenBtn.Size = new System.Drawing.Size(28, 23);
            this.folderOpenBtn.TabIndex = 2;
            this.folderOpenBtn.TabStop = false;
            this.toolTip1.SetToolTip(this.folderOpenBtn, "Open Script Location");
            this.folderOpenBtn.UseVisualStyleBackColor = true;
            this.folderOpenBtn.Click += new System.EventHandler(this.folderOpenBtn_Click);
            // 
            // ScriptManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 250);
            this.Controls.Add(this.deleteBtn);
            this.Controls.Add(this.newBtn);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.validateBtn);
            this.Controls.Add(this.reloadBtn);
            this.Controls.Add(this.folderOpenBtn);
            this.Controls.Add(this.disableBtn);
            this.Controls.Add(this.openInVsBtn);
            this.Controls.Add(this.hlpBtn);
            this.Controls.Add(this.aboutBtn);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.synchBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.scriptsList);
            this.Name = "ScriptManager";
            this.Text = "ManageScripts";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox scriptsList;
        private System.Windows.Forms.Button editBtn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button synchBtn;
        private System.Windows.Forms.Button refreshBtn;
        private System.Windows.Forms.Button reloadBtn;
        private System.Windows.Forms.Button validateBtn;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Button newBtn;
        private System.Windows.Forms.Button deleteBtn;
        private System.Windows.Forms.Button aboutBtn;
        private System.Windows.Forms.Button disableBtn;
        private System.Windows.Forms.Button openInVsBtn;
        private System.Windows.Forms.Button hlpBtn;
        private System.Windows.Forms.Button folderOpenBtn;
    }
}