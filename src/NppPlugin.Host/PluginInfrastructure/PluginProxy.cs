using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NppPlugin.DllExport;

#if nppscripts

namespace NppScripts
#else
using Kbg.NppPluginNET.PluginInfrastructure;
namespace Kbg.NppPluginNET
#endif
{
    /// <summary>
    /// Class that represents plugin adapter for the plugin host. This class implements standard N++ hosting interface
    /// </summary>
    static public class PluginProxy
    {
        static IUnmanagedExports plugin;

        /// <summary>
        /// Determines whether this instance is unicode.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is unicode; otherwise, <c>false</c>.
        /// </returns>
        public static bool isUnicode()
        {
            return plugin.isUnicode();
        }

        /// <summary>
        /// Sets the information.
        /// </summary>
        /// <param name="notepadPlusData">The notepad plus data.</param>
        public static void setInfo(NppData notepadPlusData)
        {
            plugin.setInfo(notepadPlusData);
        }

        /// <summary>
        /// Gets the funcs array.
        /// </summary>
        /// <param name="nbF">The nb f.</param>
        /// <returns></returns>
        public static IntPtr getFuncsArray(ref int nbF)
        {
            return plugin.getFuncsArray(ref nbF);
        }

        /// <summary>
        /// Messages the proc.
        /// </summary>
        /// <param name="Message">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static uint messageProc(uint Message, IntPtr wParam, IntPtr lParam)
        {
            return plugin.messageProc(Message, wParam, lParam);
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns></returns>
        public static IntPtr getName()
        {
            return plugin.getName();
        }

        /// <summary>
        /// Bes the notified.
        /// </summary>
        /// <param name="notifyCode">The notify code.</param>
        public static void beNotified(IntPtr notifyCode)
        {
            plugin.beNotified(notifyCode);
        }

        static string ProbeFile(params string[] paths)
        {
            var file = Path.Combine(paths);
            if (File.Exists(file))
                return file;
            else
                return null;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public static void Init()
        {
            string thisAssembly = Assembly.GetExecutingAssembly().Location;
            string pluginName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(thisAssembly));

            string baseDir = Path.GetDirectoryName(thisAssembly);

            string pluginPath = ProbeFile(baseDir, pluginName + ".asm.dll") ??
                                ProbeFile(baseDir, pluginName, pluginName + ".dll") ??
                                ProbeFile(baseDir, pluginName, pluginName + ".asm.dll");
            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                AppDomain.CurrentDomain.TypeResolve += CurrentDomain_TypeResolve;
                Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);

                // At this stage any call to pluginAssembly.GetTypes() will throw asm probing error.
                // That's why we need to bing host and plugin first, so plugin does not have to
                // resolve/probe NppPlugin.Host assembly.

                Type binder = pluginAssembly.GetType("NppPluginBinder");
                binder.GetMethod("bind").Invoke(null, new object[] { thisAssembly });

                Type exports = Assembly.LoadFrom(pluginPath)
                                       .GetTypes()
                                       .FirstOrDefault(t => t.GetInterface("IUnmanagedExports") != null);

                // plugin = Activator.CreateInstance(exports);
                var pluginObject = Activator.CreateInstance(exports);
                var interfaces = pluginObject.GetType().GetInterfaces();
                plugin = (IUnmanagedExports)Activator.CreateInstance(exports);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, pluginName + " - Host");
                throw;
            }
        }

        private static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return null;
        }
    }
}