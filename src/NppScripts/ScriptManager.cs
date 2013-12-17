using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

#pragma warning disable 1591

namespace NppScripts
{
    public partial class ScriptManager : Form
    {
        public ScriptManager()
        {
            InitializeComponent();
            RefreshScriptsList();
            RefreshControls();
        }

        //refresh available scripts
        //synch ->
        //synch <-
        //validate selected script
        //disable script
        //create script
        //delete script
        //about
        //help
        //reload scripts or Notepad
        //execute script
        //script status
        //open script Dir
        //Links

        List<ScriptInfo> scripts = new List<ScriptInfo>();

        void RefreshScriptsList()
        {
            scriptsList.Items.Clear();

            foreach (string file in Directory.GetFiles(Plugin.ScriptsDir, "Npp.???.*.cs").OrderBy(x => x))
            {
                if (!file.EndsWith(".disabled.cs", StringComparison.OrdinalIgnoreCase))
                {
                    scriptsList.Items.Add(new ScriptInfo(file));
                }
            }
        }

        private void refreshBtn_Click(object sender, EventArgs e)
        {
            RefreshScriptsList();
        }

        ScriptInfo SelectedScript
        {
            get
            {
                return (scriptsList.SelectedItem as ScriptInfo);
            }
        }

        private void runBtn_Click(object sender, EventArgs e)
        {
            Run();
        }

        public void Run()
        {
            if (SelectedScript != null)
            {
                if (SelectedScript.IsSeparator)
                {
                    MessageBox.Show("Selected item is not a script.", "Notepad++ Scripts");
                }
                else
                    try
                    {
                        Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);

                        if (!Plugin.ExecuteScriptByFileName(SelectedScript.File))
                        {
                            if (DialogResult.Yes == MessageBox.Show("The selected script is not loaded yet. You need to restart Notepad++ in order the changes to take affect.\nDo you want to restart it now?", "Notepad++ Scripts", MessageBoxButtons.YesNo))
                            {
                                Application.DoEvents();
                                reloadBtn.PerformClick();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex, "Notepad++ Scripts");
                    }
            }
            else
                MessageBox.Show("Please select the script to be executed.", "Notepad++ Scripts");
        }

        void scriptsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            editBtn.PerformClick();
        }

        void editBtn_Click(object sender, EventArgs e)
        {
            if (SelectedScript != null && !SelectedScript.IsSeparator)
                EditScript(SelectedScript.File);
        }

        void EditScript(string scriptFile)
        {
            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DOOPEN, 0, scriptFile);
            Win32.SendMessage(Npp.CurrentScintilla, SciMsg.SCI_GRABFOCUS, 0, 0);
        }

        void newBtn_Click(object sender, EventArgs e)
        {
            using (var input = new ScriptNameInput())
            {
                if (input.ShowDialog() != DialogResult.OK)
                    return;

                var lastScriptFile = Directory.GetFiles(Plugin.ScriptsDir, "Npp.???.*.cs")
                                              .OrderBy(x => x)
                                              .LastOrDefault();
                int index = -1;
                if (lastScriptFile != null)
                    int.TryParse(Path.GetFileName(lastScriptFile).Substring(4, 3), out index);

                index++;

                string newScript = Path.Combine(Plugin.ScriptsDir, string.Format("Npp.{0:000}.{1}.cs", index, NormalizeScriptName(input.ScriptName)));
                File.WriteAllText(newScript, defaultScriptCode.Replace("{$ScriptName}", input.ScriptName));

                RefreshScriptsList();
                SelectScript(newScript);
                EditScript(newScript);
            }
        }

        string NormalizeScriptName(string text)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + "_";

            foreach (char c in invalid)
            {
                text = text.Replace(c.ToString(), "");
            }

            return text;
        }

        public void SelectScript(string scriptFile)
        {
            var item = scriptsList.Items.Cast<ScriptInfo>().Where(x => x.File == scriptFile).FirstOrDefault();
            scriptsList.SelectedItem = item;
        }

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            if (SelectedScript != null)
            {
                string scriptFile = SelectedScript.File;
                try
                {
                    if (DialogResult.Yes == MessageBox.Show("You are about to delete '" + scriptFile + "'\nDo you want to proceed?", "Notepad++ Scripts", MessageBoxButtons.YesNo))
                    {
                        File.Delete(scriptFile);
                        refreshBtn.PerformClick();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                MessageBox.Show("Please select the script to be executed.", "Notepad++ Scripts");
        }

        const string defaultScriptCode =
@"using System;
using System.Windows.Forms;
using NppScripts;

/*
 '//npp_toolbar_image Shell32.dll|3' associates the script with the 16x16 icon #3 from Shell32.dll and places it on the toolbar
 '//npp_shortcut Ctrl+Alt+Shift+F12' associates the script with the shortcut Ctrl+Alt+Shift+F12
*/

public class Script : NppScript
{
    public override void Run()
    {
        string path;
        Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out path);
        MessageBox.Show(path);
    }
}";

        private void synchBtn_Click(object sender, EventArgs e)
        {
            string path;
            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out path);
            SelectScript(path);
        }

        private void validateBtn_Click(object sender, EventArgs e)
        {
            if (SelectedScript != null)
            {
                if (SelectedScript.IsSeparator)
                {
                    MessageBox.Show("Selected item is not a script.", "Notepad++ Scripts");
                }
                else
                    try
                    {
                        EditScript(SelectedScript.File);
                        Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);

                        CSScript.Compile(SelectedScript.File, null, false, typeof(NppScript).Assembly.Location);

                        MessageBox.Show("Success", "Notepad++ Scripts");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Notepad++ Scripts");
                    }
            }
        }

        static public void RestartNpp()
        {
            Win32.SendMenuCmd(Npp.NppHandle, NppMenuCmd.IDM_FILE_EXIT, 0);

            string file;
            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out file);

            if (string.IsNullOrEmpty(file)) //the Exit request has been processed and user did not cancel it
            {
                Application.DoEvents();

                int processIdToWaitForExit = Process.GetCurrentProcess().Id;
                string appToStart = Process.GetCurrentProcess().MainModule.FileName;

                bool lessReliableButLessIntrusive = true;

                if (lessReliableButLessIntrusive)
                {
                    var proc = new Process();
                    proc.StartInfo.FileName = appToStart;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                }
                else
                {
                    string restarter = Path.Combine(Plugin.ScriptsDir, "restartApp.exe");
                    if (!File.Exists(restarter))
                        File.WriteAllBytes(restarter, Resources.Resources.restartApp_exe);

                    //the restarter will also wait for this process to exit
                    Process.Start(restarter, string.Format("{0} \"{1}\"", processIdToWaitForExit, appToStart));
                }
            }
        }

        private void reloadBtn_Click(object sender, EventArgs e)
        {
            RestartNpp();
        }

        private void scriptsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshControls();
        }

        void RefreshControls()
        {
            editBtn.Enabled =
            validateBtn.Enabled =
            runBtn.Enabled =
            deleteBtn.Enabled =
            disableBtn.Enabled = (scriptsList.SelectedItem != null && !(scriptsList.SelectedItem as ScriptInfo).IsSeparator);
            deleteBtn.Enabled = (scriptsList.SelectedItem != null);
        }

        private void openInVsBtn_Click(object sender, EventArgs e)
        {
            if (SelectedScript != null && !SelectedScript.IsSeparator)
            {
                string projectFile = CSSctiptHelper.GenerateVSProjectFor(SelectedScript.File);
                try
                {
                    Process.Start(projectFile);
                    CSSctiptHelper.ClearVSDir();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Notepad++ Scripts");
                }
            }
            else
                MessageBox.Show("Please select the script to be opened in Visual Studio.", "Notepad++ Scripts");
        }

        private void aboutBtn_Click(object sender, EventArgs e)
        {
        }

        private void hlpBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://dl.dropboxusercontent.com/u/2192462/NPP/NppScriptsHelp.html");
            }
            catch { }
        }

        private void folderOpenBtn_Click(object sender, EventArgs e)
        {
            //ftp://ftp.sm.ifes.edu.br/professores/EduardoSilva/Curso%20Ginga/Composer/include/composer/Qsci/qsciscintillabase.h
            //http://sourceforge.net/apps/mediawiki/notepad-plus/index.php?title=Plugin_Development#How_to_develop_a_plugin
            //http://sourceforge.net/apps/mediawiki/notepad-plus/index.php?title=Messages_And_Notifications
            //http://www.scintilla.org/ScintillaDoc.html

            try
            {
                if (SelectedScript != null)
                    Process.Start("explorer.exe", "/select," + SelectedScript.File);
                else
                    Process.Start("explorer.exe", Plugin.ScriptsDir);
            }
            catch { }
        }
    }

}