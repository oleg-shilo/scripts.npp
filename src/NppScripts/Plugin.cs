using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using CSScriptLibrary;

#pragma warning disable 1591

/*
 TODO:
 * + check version of CSScriptNpp.dll during the integration stage 
 * + On script errors output into CSScriptNpp output panel if available
 * XML Documentation for Npp.Members
 * + Sample scripts
 */
namespace NppScripts
{

    public partial class Plugin
    {
        static internal void CommandMenuInit()
        {
            LoadScripts();
        }

        static Dictionary<int, ScriptInfo> configuredScripts = new Dictionary<int, ScriptInfo>();

        static public NppScript GetScriptByTag(object tag)
        {
            var info = Plugin.AllConfiguredScripts
                         .Where(s => tag.Equals(s.Tag))
                         .FirstOrDefault();

            if (info == null)
                return null;
            else
                return EnsureScriptLoaded(info);
        }

        static public NppScript GetScriptByFileName(string file)
        {
            var info = Plugin.AllConfiguredScripts
                        .Where(s => string.Compare(Path.GetFullPath(s.File), Path.GetFullPath(file), StringComparison.OrdinalIgnoreCase) == 0)
                        .FirstOrDefault();

            if (info == null)
                return null;
            else
                return EnsureScriptLoaded(info);
        }

        static public bool ExecuteScript(NppScript script)
        {
            var scriptInfo = configuredScripts[script.ScriptId];
            return ExecuteScript(scriptInfo);
        }

        static bool ExecuteScript(ScriptInfo script)
        {
            if (script == null || !script.IsLoaded)
            {
                return false;
            }
            else
            {
                try
                {
                    var run = FuncItems.Items[script.Id]._pFunc;
                    run();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Notepad++ Scripts");
                }
                return true;
            }
        }

        static public NppScript[] AllLoadedScripts
        {
            get
            {
                return configuredScripts.Values.Where(x => x.IsLoaded).Select(x => x.Script).ToArray();
            }
        }

        static public ScriptInfo[] AllConfiguredScripts
        {
            get
            {
                return configuredScripts.Values.Where(x => !x.IsSeparator).ToArray();
            }
        }

        static string sectionName = "ScriptManager";
        static string showScriptManagerKey = "initialShow";
        public const string PluginName = "Automation Scripts";
        public static int scriptManagerId = -1;

        static string iniFile;

        static string IniFile
        {
            get
            {
                if (iniFile == null)
                {
                    var configDirBuf = new StringBuilder(Win32.MAX_PATH);
                    Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETPLUGINSCONFIGDIR, Win32.MAX_PATH, configDirBuf);
                    string configDir = configDirBuf.ToString();

                    if (!Directory.Exists(configDir))
                        Directory.CreateDirectory(configDir);

                    iniFile = Path.Combine(configDir, "NppScripts.ini");
                }
                return iniFile;
            }
        }

        static public string ScriptsDir
        {
            get
            {
                string rootDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string scriptDir = Path.Combine(rootDir, "NppScripts");

                if (!Directory.Exists(scriptDir))
                    Directory.CreateDirectory(scriptDir);

                return scriptDir;
            }
        }

        static public object GetOutputPanel()
        {
            try
            {
                return CSScriptIntegration.GetOutputPanel();
            }
            catch
            {
                MessageBox.Show("CS-Script Output Panel cannot be found.\nPlease ensure you have CS-Script Plugin installed.", "Notepad++ Scripts");
            }
            return null;
        }

        static public string NppPluginsDir
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        static public string PluginDir
        {
            get
            {
                string rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                return Path.Combine(rootDir, "NppScripts");
            }
        }

        public static bool ExecuteScriptByFileName(string scriptFile)
        {
            var script = configuredScripts.Where(x => x.Value != null && x.Value.File == scriptFile).Select(x => x.Value).FirstOrDefault();
            return ExecuteScript(script);
        }

        static NppScript EnsureScriptLoaded(ScriptInfo scriptInfo)
        {
            if (File.GetLastWriteTime(scriptInfo.File) != scriptInfo.LastModified || scriptInfo.Script is NppScriptStub)
            {
                scriptInfo.Script = LoadScript(scriptInfo.File, scriptInfo.Id);

                scriptInfo.Script.ScriptFile = scriptInfo.File;
                scriptInfo.Script.ScriptId = scriptInfo.Id;
                scriptInfo.Script.Name = scriptInfo.Name;
                scriptInfo.Script.Tag = scriptInfo.Tag;

                scriptInfo.LastModified = File.GetLastWriteTime(scriptInfo.File);
            }
            return scriptInfo.Script;
        }

        static void LoadScripts()
        {
            var sw = new Stopwatch();
            sw.Start();

            int index = 0;

            configuredScripts.Clear();

            foreach (string file in Directory.GetFiles(ScriptsDir, "Npp.???.*.cs").OrderBy(x => x))
            {
                if (file.EndsWith(".disabled.cs", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (file.EndsWith(".---.cs", StringComparison.OrdinalIgnoreCase))
                {
                    SetCommand(index++, "---", null);
                    continue;
                }

                var scriptInfo = new ScriptInfo(file, index++);

                configuredScripts.Add(scriptInfo.Id, scriptInfo);

                SetCommand(scriptInfo.Id,
                           scriptInfo.Name,
                           () =>
                           {
                               EnsureScriptLoaded(scriptInfo);
                               scriptInfo.Script.Run();
                           },
                           scriptInfo.Shortcut,
                           scriptInfo.CheckedOnInit);
            }

            SetCommand(index++, "---", null);

            initailShowScriptManager = (Win32.GetPrivateProfileInt(sectionName, showScriptManagerKey, 0, IniFile) != 0);
            scriptManagerId = index++;
            SetCommand(scriptManagerId, "Script Manager", ManageScripts, initailShowScriptManager);
            sw.Stop();
        }

        static bool initailShowScriptManager = false;

        static NppScript LoadScript(string file, int id)
        {
            try
            {
                CSScriptIntegration.ClearBuildError();

                CSScript.CacheEnabled = true;
                bool debugScript = true;

                string asmFile = CSScript.Compile(file, null, debugScript, Assembly.GetExecutingAssembly().Location);
                string debugSymbols = Path.ChangeExtension(asmFile, ".pdb");

                Assembly asm;
                if (File.Exists(debugSymbols) && debugScript)
                    asm = Assembly.Load(File.ReadAllBytes(asmFile), File.ReadAllBytes(debugSymbols));
                else
                    asm = Assembly.Load(File.ReadAllBytes(asmFile));

                object script = asm.CreateObject("Script");

                var retval = (NppScript)script;
                retval.ScriptFile = file;
                retval.ScriptId = id;
                retval.OnLoaded();

                return retval;
            }
            catch (UnauthorizedAccessException)
            {
                if (DialogResult.Yes == MessageBox.Show("The Script is locked. Restarting Notepad++ will release it.\nDo you want to restart it now?", "Notepad++ Scripts", MessageBoxButtons.YesNo))
                {
                    var proc = System.Diagnostics.Process.GetProcessById(55);
                    ScriptManager.RestartNpp();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show("Script '" + file + "' is invalid.\n" + e.Message, "Notepad++ Scripts");
                CSScriptIntegration.ShowBuildError("Script '" + file + "' is invalid.\n" + e.Message);
            }

            return new NppScriptStub
                {
                    ScriptFile = file,
                    ScriptId = id
                };
        }

        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            if (args.LoadedAssembly.Location.Contains(".compiled"))
                Debug.Assert(false, "Script Assembly Get Locked:\n" + args.LoadedAssembly.Location + "\n\n");
        }

        static public void SetToolbarImage(Bitmap image, int pluginId)
        {
            var tbIcons = new toolbarIcons();
            tbIcons.hToolbarBmp = image.GetHbitmap();
            IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf(tbIcons));
            Marshal.StructureToPtr(tbIcons, pTbIcons, false);
            Win32.SendMessage(NppData._nppHandle, NppMsg.NPPM_ADDTOOLBARICON, FuncItems.Items[pluginId]._cmdID, pTbIcons);
            Marshal.FreeHGlobal(pTbIcons);
        }

        static internal void InitView()
        {
            if (initailShowScriptManager)
            {
                initailShowScriptManager = false;
                ManageScripts();
            }
        }

        static internal void CleanUp()
        {
            Win32.WritePrivateProfileString(sectionName, showScriptManagerKey, (scriptManager != null && scriptManager.Visible) ? "1" : "0", IniFile);
        }

        public static bool IsNppScript(string script)
        {
            if (string.Compare(Path.GetDirectoryName(script), Plugin.ScriptsDir, true) != 0)
                return false;
            else
                return Regex.IsMatch(Path.GetFileName(script), @"Npp.\d{3}..*.cs", RegexOptions.IgnoreCase);
        }

        public static void OnNotification(SCNotification data)
        {
            foreach (ScriptInfo info in configuredScripts.Values)
                if (info.Script != null && !(info.Script is NppScriptStub))
                    try { info.Script.OnNotification(data); }
                    catch { } //does not matter what but we should not interrupt the SCNotification handling
        }

        public static void RefreshToolbarImages()
        {
            foreach (int scriptId in configuredScripts.Keys)
            {
                Bitmap toolbarImage = configuredScripts[scriptId].ToolbarImage;

                if (toolbarImage != null)
                    SetToolbarImage(toolbarImage, scriptId);
            }
            SetToolbarImage(Resources.Resources.css_logo_16x16_tb, scriptManagerId);
        }

        internal static ScriptManager scriptManager;

        public static void ToggleDockedPanelVisible(Form panel, int scriptId)
        {
            SetDockedPanelVisible(panel, scriptId, !panel.Visible);
        }

        public static void SetDockedPanelVisible(Form panel, int scriptId, bool visible)
        {
            if (visible)
            {
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMSHOW, 0, panel.Handle);
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, Plugin.FuncItems.Items[scriptId]._cmdID, 1);
            }
            else
            {
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMHIDE, 0, panel.Handle);
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, Plugin.FuncItems.Items[scriptId]._cmdID, 0);
            }
        }

        static Dictionary<int, IntPtr> dockedManagedPanels = new Dictionary<int, IntPtr>();

        public static void DockPanel(Form panel, int scriptId, string name, Icon tollbarIcon, NppTbMsg tbMsg, bool initiallyVisible = true)
        {
            var tbIcon = tollbarIcon ?? Utils.NppBitmapToIcon(Resources.Resources.css_logo_16x16);

            //tbIcon = Utils.NppBitmapToIcon(Properties.Resources.css_logo_16x16);

            NppTbData _nppTbData = new NppTbData();
            _nppTbData.hClient = panel.Handle;
            _nppTbData.pszName = name;
            // the dlgDlg should be the index of funcItem where the current function pointer is,
            //in this case is 15. so the initial value of funcItem[15]._cmdID - not the updated internal one !
            _nppTbData.dlgID = scriptId;
            // define the default docking behaviour
            _nppTbData.uMask = tbMsg;
            _nppTbData.hIconTab = (uint)tbIcon.Handle;
            _nppTbData.pszModuleName = PluginName;
            IntPtr _ptrNppTbData = Marshal.AllocHGlobal(Marshal.SizeOf(_nppTbData));
            Marshal.StructureToPtr(_nppTbData, _ptrNppTbData, false);

            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMREGASDCKDLG, 0, _ptrNppTbData);

            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, Plugin.FuncItems.Items[scriptId]._cmdID, 1); //from this moment the panel is visible

            if (!initiallyVisible)
                SetDockedPanelVisible(panel, scriptId, initiallyVisible);

            if (dockedManagedPanels.ContainsKey(scriptId))
            {
                //there is already another panel
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMHIDE, 0, dockedManagedPanels[scriptId]);
                dockedManagedPanels[scriptId] = panel.Handle;
            }
            else
                dockedManagedPanels.Add(scriptId, panel.Handle);
        }

        static void ManageScripts()
        {
            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                DockPanel(scriptManager, scriptManagerId, "Automation", null, NppTbMsg.DWS_DF_CONT_LEFT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR);
                //DockPanel(scriptManager, scriptManagerId, "Script Manager", null, NppTbMsg.DWS_DF_CONT_LEFT | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR);

                // Following message will toogle both menu item state and toolbar button
                Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, FuncItems.Items[scriptManagerId]._cmdID, 1);
            }
            else
            {
                if (!scriptManager.Visible)
                {
                    Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMSHOW, 0, scriptManager.Handle);
                    Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, FuncItems.Items[scriptManagerId]._cmdID, 1);
                }
                else
                {
                    Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_DMMHIDE, 0, scriptManager.Handle);
                    Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, FuncItems.Items[scriptManagerId]._cmdID, 0);
                }
            }
            scriptManager.scriptsList.Focus();
        }
    }
}