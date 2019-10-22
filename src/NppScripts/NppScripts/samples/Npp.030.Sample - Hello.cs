//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        // Open a new document
        Npp.Editor.SendMenuCommand(NppMenuCmd.IDM_FILE_NEW);
        // you can also do it with Win API SendMessage
        //  Win32.SendMessage(Npp.Editor.Handle, (uint)NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);

        // Say hello now :
        Npp.Document.SetText("Hello, Notepad++... from .NET!");
        Npp.Document.SetSel(0, Npp.Document.GetLength() - 1);
    }
}