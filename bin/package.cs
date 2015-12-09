//css_args /ac
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System;

void main(string[] args)
{
    string name = "NppScripts";
    var version = Directory.GetFiles(".", name +".*.msi").Select(x =>Path.GetFileNameWithoutExtension(x)).First().Replace(name+".", "");

    Console.WriteLine("Injecting version into file names: " + version);

    var zipFile = Directory.GetFiles(".", name +".zip").FirstOrDefault();
    if(zipFile != null)
    {
        string distro = Path.Combine(Path.GetDirectoryName(zipFile), Path.GetFileNameWithoutExtension(zipFile) + "." + version + ".zip");
        if (File.Exists(distro)) File.Delete(distro);
        File.Move(zipFile, distro);
        //MessageBox.Show(distro);
        File.Copy(distro, @"E:\cs-script.web\csscript\npp\" + Path.GetFileName(distro), true);
    }

    var sevenZFile = Directory.GetFiles(".", name +".7z").FirstOrDefault();
    if (sevenZFile != null)
    {
        Console.WriteLine("----------");
        var distro = Path.Combine(Path.GetDirectoryName(sevenZFile), Path.GetFileNameWithoutExtension(sevenZFile) + "." + version + ".7z");
        if (File.Exists(distro)) File.Delete(distro);
        File.Move(sevenZFile, distro);
    }

    File.WriteAllText(@"E:\cs-script\cs-scriptWEB\npp\latest_npps_version.txt", version);

    var html = File.ReadAllText(@"E:\cs-script\cs-scriptWEB\npp\csscript.html");
    html = html.Replace("https://dl.dropboxusercontent.com/u/2192462/CS-S_NPP/NppScripts.zip", "https://dl.dropboxusercontent.com/u/2192462/CS-S_NPP/NppScripts."+version+".zip");
    File.WriteAllText(@"E:\cs-script\cs-scriptWEB\npp\csscript.html", html);
}