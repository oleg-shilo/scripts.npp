using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace NppScripts
{
    class CSScriptIntegration
    {
        static Action originalRunScript;
        static Action originalRunScriptAsExternal;
        static Action originalDebugScript;

        public static void Initialize()
        {
            //Debug.Assert(false);
            try
            {
                Assembly csscriptAsm = AppDomain.CurrentDomain
                                                .GetAssemblies()
                                                .Where(asm => asm.FullName.StartsWith("CSScriptNpp,"))
                                                .FirstOrDefault();

                if (csscriptAsm == null || csscriptAsm.GetName().Version < new Version(1, 0, 7, 2))
                    return;

                Type CSScriptNppPlugin = csscriptAsm.GetType("CSScriptNpp.Plugin");
                if (CSScriptNppPlugin != null)
                {
                    //ensure local Notepad++ scripts are executed within Notepad++ host app
                    originalRunScript = Substitute(CSScriptNppPlugin, "RunScript", RunScript);
                    originalRunScriptAsExternal = Substitute(CSScriptNppPlugin, "RunScriptAsExternal", RunScriptAsExternal);
                    originalDebugScript = Substitute(CSScriptNppPlugin, "DebugScript", DebugScript);

                    LoadScript = (script) =>
                        {
                            try
                            {
                                CSScriptNppPlugin.GetStaticField("ProjectPanel")
                                                 .Call("LoadScript", script);
                            }
                            catch { }
                        };

                    BuildOutputWriteLine = (line) =>
                        {
                            try
                            {
                                CSScriptNppPlugin.CallStatic("ShowOutputPanel")
                                                 .Call("ShowBuildOutput")
                                                 .Call("Clear")
                                                 .Call("WriteLine", line.NormalizeLineBreaks(), new object[0]);
                            }
                            catch { }
                        };

                    GetOutputPanel = () =>
                        {
                            return CSScriptNppPlugin.CallStatic("ShowOutputPanel");
                        };
                }
            }
            catch { }
        }

        static Action Substitute(Type type, string name, Action newAction)
        {
            Action originalAction = null;
            FieldInfo routine = type.GetField(name);
            if (routine != null)
            {
                originalAction = (Action)routine.GetValue(null);
                routine.SetValue(null, newAction);
            }
            return originalAction;
        }

        static public void ShowBuildError(string error)
        {
            if (BuildOutputWriteLine != null)
                CSScriptIntegration.BuildOutputWriteLine(error);
            else
                MessageBox.Show(error, "Notepad++ Automation");
        }

        static public void ClearBuildError()
        {
            if (BuildOutputWriteLine != null)
                CSScriptIntegration.BuildOutputWriteLine("");
        }

        static Action<string> LoadScript;
        static Action<string> BuildOutputWriteLine;
        public static Func<object> GetOutputPanel;

        static void RunScript()
        {
            string script = Npp.Editor.GetCurrentFilePath();

            if (Plugin.IsNppScript(script))
            {
                // Npp.GetCurrentDocument();
                LoadScript(script);
                ExecuteScriptLocally(script); //execute with NppScripts
            }
            else
            {
                originalRunScript(); //execute with CSScriptNpp
            }
        }

        static void RunScriptAsExternal()
        {
            string script = Npp.Editor.GetCurrentFilePath();

            if (Plugin.IsNppScript(script))
            {
                // Npp.CurrentDocument;
                ExecuteScriptLocally(script); //execute with NppScripts
            }
            else
            {
                originalRunScriptAsExternal(); //execute with CSScriptNpp
            }
        }

        static void DebugScript()
        {
            string script = Npp.Editor.GetCurrentFilePath();

            if (Plugin.IsNppScript(script))
            {
                // Npp.CurrentDocument;
                MessageBox.Show("Notepad++ scripts cannot be debugged from Notepad++.\nTry to attach Visual Studio or inject Debug.Assert in the script code.", "Notepad++ Automation");
            }
            else
            {
                originalDebugScript(); //execute with CSScriptNpp
            }
        }

        static void ExecuteScriptLocally(string script)
        {
            if (Plugin.scriptManager != null)
            {
                Plugin.scriptManager.SelectScript(script);
                Plugin.scriptManager.Run();
            }
            else
                try
                {
                    if (!Plugin.ExecuteScriptByFileName(script))
                    {
                        MessageBox.Show("The selected script is not loaded yet. You need to restart Notepad++ in order the changes to take affect.", "Notepad++ Automation");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Notepad++ Automation");
                }
        }
    }
}