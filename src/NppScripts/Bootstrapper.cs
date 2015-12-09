using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NppScripts
{

    class Bootstrapper
    {
        public static void Init()
        {
            NppScripts.Plugin.CommandMenuInit(); //this will also call CSScriptIntellisense.Plugin.CommandMenuInit
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                if (args.Name.StartsWith("CSScriptLibrary,"))
                {
                    string asm = Path.Combine(rootDir, @"CSScriptNpp\CSScriptLibrary.dll");
                    if (File.Exists(asm))
                        return Assembly.LoadFrom(asm);
                    asm = Path.Combine(rootDir, @"NppScripts\CSScriptLibrary.dll");
                    if (File.Exists(asm))
                        return Assembly.LoadFrom(asm);
                }
                else if (args.Name == Assembly.GetExecutingAssembly().FullName)
                    return Assembly.GetExecutingAssembly();
            }
            catch { }
            return null;
        }
    }
}
