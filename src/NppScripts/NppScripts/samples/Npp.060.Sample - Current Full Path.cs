//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        // WIN32
        // string path;
        // Win32.SendMessage(Npp.Editor.Handle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out path);

        // Npp API
        var path = Npp.Editor.GetCurrentFilePath();

        Npp.Document.ReplaceSel(path);
    }
}