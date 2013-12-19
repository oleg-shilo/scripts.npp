using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NppScripts
{
    partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.Text = "About Notepad++ Automation";
            this.label3.Text = "Version: " + AssemblyVersion;
            this.label5.Text = AssemblyCopyright;
            this.textBoxDescription.Text = AssemblyDescription;

            if (downloadingMsi)
                SetUpdateStatus("Downloading...");
        }

        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        #endregion Assembly Attribute Accessors

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://csscript.net/npp/license.txt");
            }
            catch { }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("http://csscript.net/npp/");
            }
            catch { }
        }

        void updateCheckBtn_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            SetUpdateStatus("Checking...");
            Task.Factory.StartNew(CheckForUpdates);
        }

        static bool downloadingMsi = false;

        void SetUpdateStatus(string status = null)
        {
            if (status == null)
            {
                updateCheckBtn.Enabled = true;
                updateCheckBtn.Text = "Check for Updates...";
            }
            else
            {
                updateCheckBtn.Enabled = true;
                updateCheckBtn.Text = status;
            }
        }


        void CheckForUpdates()
        {
            string version = Product.GetLatestAvailableVersion();

            Invoke((Action)delegate
            {
                SetUpdateStatus();
                Cursor = Cursors.Default;
            });

            if (version == null)
            {
                MessageBox.Show("Cannot check for updates. The latest release Web page will be opened instead.", "Notepad++ Automation");
                try
                {
                    Process.Start(Product.HomeUrl);
                }
                catch { }
            }
            else
            {
                var latestVersion = new Version(version);
                var nppVersion = Assembly.GetExecutingAssembly().GetName().Version;

                if (nppVersion == latestVersion)
                {
                    MessageBox.Show("You are already running the latest version - v" + version, "Notepad++ Automation");
                }
                else if (nppVersion > latestVersion)
                {
                    MessageBox.Show("Wow... your version is even newer than the latest one - v" + version + ".", "Notepad++ Automation");
                }
                else if (nppVersion < latestVersion)
                {
                    //if (DialogResult.Yes == MessageBox.Show("The newer version v" + version + " is available.\nDo you want to download and install it?\n\nWARNING: If you coose 'Yes' Notepad++ will be closed and all unsaved data may be lost.", "CS-Script", MessageBoxButtons.YesNo))
                    {
                        Invoke((Action)delegate
                        {
                            SetUpdateStatus("Downloading");
                        });

                        downloadingMsi = true;
                        string msiFile = Product.GetLatestAvailableMsi(version);
                        downloadingMsi = false;

                        if (msiFile != null)
                        {
                            try
                            {
                                Process.Start("msiexec.exe", "/i \"" + msiFile + "\" /qb");

                                //close notepad++
                                //Win32.SendMenuCmd(Npp.NppHandle, NppMenuCmd.IDM_FILE_EXIT, 0);
                                //string file;
                                //Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out file);

                            }
                            catch
                            {
                                MessageBox.Show("Cannot execute setup file: " + msiFile, "Notepad++ Automation");
                            }

                        }
                        else
                        {
                            MessageBox.Show("Cannot download the binaries. The latest release Web page will be opened instead.", "Notepad++ Automation");
                            try
                            {
                                Process.Start(Product.HomeUrl);
                            }
                            catch { }
                        }

                        Invoke((Action)delegate
                        {
                            SetUpdateStatus();
                        });
                    }
                }
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("mailto:csscript.support@gmail.com?subject=CS-Script_NppScripts");
            }
            catch { }
        }

        int blinkingCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (updateCheckBtn.Text.StartsWith("Downloading"))
            {
                blinkingCount++;
                if (blinkingCount > 3)
                    blinkingCount = 0;

                updateCheckBtn.Text = "Downloading" + new string('.', blinkingCount);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string samplesBackup = Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), @"NppScripts\samples.zip");

            if (File.Exists(samplesBackup))
            {
                try
                {
                    using (var zip = ZipArchive.OpenOnFile(samplesBackup))
                    {
                        var backupFiles = zip.Files.OrderBy(file => file.Name).Select(file => file.Name).ToArray();
                        var existingFiles = Directory.GetFiles(Plugin.ScriptsDir).Select(file => Path.GetFileName(file)).ToArray();

                        if (existingFiles.Intersect(backupFiles).Any())
                            if (DialogResult.Yes != MessageBox.Show("Some existing sample scripts will be overwritten.\nDo you want to proceed?", "Notepad++ Automation", MessageBoxButtons.YesNo))
                                return;

                        foreach (var name in backupFiles)
                        {
                            var data = zip.GetFile(name);
                            if (data.FolderFlag)
                                continue;

                            string destFile = Path.Combine(Plugin.ScriptsDir, name);
                            string destDir = Path.GetDirectoryName(destFile);

                            if (!Directory.Exists(destDir))
                                Directory.CreateDirectory(destDir);

                            using (var fileStream = new FileStream(destFile, FileMode.Create))
                            {
                                data.GetStream().CopyTo(fileStream);
                            }
                        }
                    }

                    MessageBox.Show("Sample scripts have been restored.", "Notepad++ Automation");
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Samples backup cannot be found. Please reinstall plugin to restore the samples.\nOr download samples from the plugin home page", "Notepad++ Automation");
            }
        }
    }
}