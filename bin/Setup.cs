//css_dir %WIXSHARP_DIR%;
//css_dir %WIXSHARP_DIR%\Wix_bin\bin;
//css_ref Microsoft.Deployment.WindowsInstaller.dll;
using IO = System.IO;
using System;
using WixSharp;

class Script
{
    [STAThread]
    static public void Main(string[] args)
    {
        string pluginFile = IO.Path.GetFullPath(@"Plugins\NppScripts.dll");
        Version version = System.Reflection.Assembly.ReflectionOnlyLoadFrom(pluginFile).GetName().Version;

        Project project =
            new Project("Notepad++ Automation",
                new Dir(@"%ProgramFiles%\Notepad++\Plugins",
                    new File(@"Plugins\NppScripts.dll"),
                    new Dir("NppScripts",
                        new File(@"Plugins\NppScripts\CSScriptLibrary.dll"),
                        new File(@"Plugins\NppScripts\samples.zip")))
                    );

        project.Actions = new WixSharp.Action[]
        {
            new PathFileAction("%ProgramFiles%\\Notepad++\\notepad++.exe", "", "INSTALLDIR", Return.asyncNoWait, When.After, Step.InstallInitialize, Condition.NOT_Installed)
            {
                Name = "Action_StartNPP" //need to give custom name as "Action1_notepad++.exe" is illegal because of '++'
            }
        };
        
        project.GUID = new Guid("6f930b47-2373-431d-9095-18614525889b");
        project.Version = version;
        project.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
        project.LicenceFile = "license.rtf";

        Compiler.ClientAssembly = System.Reflection.Assembly.GetExecutingAssembly().Location;

        Compiler.BuildMsi(project, "NppScripts."+version+".msi");
    }
}
