using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

#pragma warning disable 1591

namespace NppScripts
{
    public class CSSctiptHelper
    {
        static public string GenerateVSProjectFor(string script)
        {
            var searchDirs = new List<string> { Plugin.ScriptsDir, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };

            var parser = new ScriptParser(script, searchDirs.ToArray(), false);
            searchDirs.AddRange(parser.SearchDirs);        //search dirs could be also defined n the script

            IList<string> sourceFiles = parser.SaveImportedScripts().ToList(); //this will also generate auto-scripts and save them
            sourceFiles.Add(script);

            //some assemblies are referenced from code and some will need to be resolved from the namespaces
            var refAsms = parser.ReferencedNamespaces
                                .Where(name => !parser.IgnoreNamespaces.Contains(name))
                                .SelectMany(name => AssemblyResolver.FindAssembly(name, searchDirs.ToArray()))
                                .Union(parser.ReferencedAssemblies
                                             .SelectMany(asm => AssemblyResolver.FindAssembly(asm, searchDirs.ToArray())))
                                .Distinct()
                                .ToArray();

            string dir = Path.Combine(VsDir, Process.GetCurrentProcess().Id.ToString());
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string projectFile = Path.Combine(dir, Path.GetFileNameWithoutExtension(script) + ".csproj");

            string sourceFilesXml = string.Join(Environment.NewLine,
                                                sourceFiles.Select(file => "<Compile Include=\"" + file + "\" />").ToArray());

            string refAsmsXml = string.Join(Environment.NewLine,
                                            refAsms.Select(file => "<Reference Include=\"" + Path.GetFileNameWithoutExtension(file) + "\"><HintPath>" + file + "</HintPath></Reference>").ToArray());


            File.WriteAllText(projectFile,
                              Resources.Resources.VS2010ProjectTemplate
                                                  .Replace("{$SOURCES}", sourceFilesXml)
                                                  .Replace("{$REFERENCES}", refAsmsXml));
            return projectFile;
        }

        static public void ClearVSDir()
        {
        }

        static string vsDir;

        public static string VsDir
        {
            get
            {
                if (vsDir == null)
                    vsDir = Path.Combine(CSScript.GetScriptTempDir(), "NppScripts");
                return vsDir;
            }
        }
    }
}
