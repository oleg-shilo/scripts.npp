using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

#pragma warning disable 1591

namespace HtmlView
{
    public partial class WebPanel : Form
    {
        public WebPanel()
        {
            InitializeComponent();

            //SetIeVersion(90000, "notepad++.exe");
        }

        public bool AutoRefresh
        {
            get { return autoRefreshCheckBox.Checked; }
            set { autoRefreshCheckBox.Checked = value; }
        }

        string url;

        public string Url
        {
            get { return url; }
            set { url = value; Refresh(true); }
        }

        /// <summary>
        /// Sets the IE version for the host process.
        /// </summary>
        /// <param name="version">The version of Internet Explorer. Use '10000' for IE10</param>
        /// <param name="appName">Name of the application (host processes). </param>
        /// <para>The name should include the file extension (e.g. "notepad++.exe").</para>
        /// <para>If not specified then the name of the current process will be used.</para>
        /// <returns><c>true</c> if the version has been changed, otherwise <c>false</c>.</returns>
        public bool SetIeVersion(int version, string appName = null)
        {
            string processName = appName ?? Process.GetCurrentProcess().ProcessName+".exe";
            //IE10 -> 10000
            var currentVersion = (int)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, 0);
            if (currentVersion != version)
            {
                Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", appName, version);
                return true;
            }
            else
                return false;
        }

        void Minitor(bool enable)
        {
            timer1.Enabled = enable;
        }

        void settingsBtn_Click(object sender, EventArgs e)
        {
        }

        void autoRefreshCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            refreshBtn.Enabled = !autoRefreshCheckBox.Checked;
            Minitor(autoRefreshCheckBox.Checked);
        }

        DateTime fileUrlTimestamp;
        void Refresh(bool force = false)
        {
            if (!string.IsNullOrEmpty(Url))
            {
                if (!File.Exists(Url))
                {
                    webBrowser1.Url = new Uri(Url);
                }
                else
                {
                    if (force || fileUrlTimestamp != File.GetLastWriteTimeUtc(Url))
                    {
                        fileUrlTimestamp = File.GetLastWriteTimeUtc(Url);
                        webBrowser1.Url = new Uri(Url);
                    }
                }
            }
        }

        void refreshBtn_Click(object sender, EventArgs e)
        {
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void WebPanel_VisibleChanged(object sender, EventArgs e)
        {
            timer1.Enabled = this.Visible && this.AutoRefresh;
        }
    }
}