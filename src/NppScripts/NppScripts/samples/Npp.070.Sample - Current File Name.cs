//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        string path;
        Npp.Editor.SendMessage(NppMsg.NPPM_GETFILENAME, 0, out path);
        Npp.Document.ReplaceSel(path);
    }
}