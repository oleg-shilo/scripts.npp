using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// using Kbg.NppPluginNET;
// using Kbg.NppPluginNET.PluginInfrastructure;
using NppPlugin.DllExport;
using NppScripts;

#pragma warning disable 1591

public class NppPluginBinder
{
    static public void bind(string hostAssemblyFile)
    {
        // Debug.Assert(false);
        Assembly.LoadFrom(hostAssemblyFile); // to avoid complicated probing scenarios
    }
}

namespace NppScripts
{
    public class UnmanagedExports : IUnmanagedExports
    {
        public bool isUnicode()
        {
            return true;
        }

        public void setInfo(NppData notepadPlusData)
        {
            try
            {
                PluginBase.nppData = notepadPlusData;
                Bootstrapper.Init();
            }
            catch
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Notepad++\plugins\logs\CSScriptNpp");

                MessageBox.Show("Cannot load the plugin.\nThe error information has been logged into '" + dir + "' directory", "CS-Script");
                throw;
            }
        }

        public IntPtr getFuncsArray(ref int nbF)
        {
            nbF = PluginBase._funcItems.Items.Count;
            return PluginBase._funcItems.NativePointer;
        }

        public uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return 1;
        }

        static IntPtr _ptrPluginName = IntPtr.Zero;

        public IntPtr getName()
        {
            if (_ptrPluginName == IntPtr.Zero)
                _ptrPluginName = Marshal.StringToHGlobalUni(Plugin.PluginName);
            return _ptrPluginName;
        }

        public void beNotified(IntPtr notifyCode)
        {
            lock (typeof(UnmanagedExports))
            {
                try
                {
                    ScNotification nc = (ScNotification)Marshal.PtrToStructure(notifyCode, typeof(ScNotification));
                    string contentFile = Npp.Editor.GetTabFile(nc.Header.IdFrom);

                    if (nc.Header.Code == (uint)NppMsg.NPPN_TBMODIFICATION)
                    {
                        PluginBase._funcItems.RefreshItems();
                        Plugin.RefreshToolbarImages();
                    }
                    else if (nc.Header.Code == (uint)SciMsg.SCN_CHARADDED)
                    {
                    }
                    else if (nc.Header.Code == (uint)NppMsg.NPPN_READY)
                    {
                        CSScriptIntegration.Initialize();
                        Plugin.InitView();
                    }
                    else if (nc.Header.Code == (uint)NppMsg.NPPN_SHUTDOWN)
                    {
                        Marshal.FreeHGlobal(_ptrPluginName);
                        Plugin.CleanUp();
                    }

                    Plugin.OnNotification(nc);
                }
                catch { }//this is indeed the last line of defense as all CS-S calls have the error handling inside
            }
        }
    }
}