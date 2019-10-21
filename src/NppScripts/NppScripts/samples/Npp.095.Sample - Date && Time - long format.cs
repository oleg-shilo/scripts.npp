//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        string dateTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToLongDateString();
        Npp.Document.ReplaceSel(dateTime);
    }
}