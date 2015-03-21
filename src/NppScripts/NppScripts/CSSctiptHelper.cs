using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

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
            foreach (var dir in Directory.GetDirectories(VsDir))
            {
            }
        }

        static bool IsInUseByExternalProcess(string dir)
        {
            string pidStr = Path.GetFileName(dir);

            try
            {
                int id = int.Parse(pidStr);
                if (Process.GetProcessById(id) != null)
                    return true; //the process using this project is still active
            }
            catch { }
            return false;
        }

        static void DeleteDir(string dir)
        {
            //deletes directory recursively
            try
            {
                foreach (string file in Directory.GetFiles(dir))
                    DeleteFile(file);

                foreach (string subDir in Directory.GetDirectories(dir))
                    DeleteDir(subDir);

                if (Directory.GetFiles(dir).Length == 0 && Directory.GetDirectories(dir).Length == 0)
                    Directory.Delete(dir);
            }
            catch
            {
            }
        }

        static void DeleteFile(string file)
        {
            try
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            catch
            {
            }
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
