//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Windows.Forms;
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        string path;
        Npp.Editor.SendMessage(NppMsg.NPPM_GETCURRENTDIRECTORY, 0, out path);
        MessageBox.Show(path);
    }
}