using System;
using System.Windows.Forms;
using NppScripts;

/*
 '//npp_toolbar_image Shell32.dll|3' associates the script with the 16x16 icon #3 from Shell32.dll and places it on the toolbar
 '//npp_shortcut Ctrl+Alt+Shift+F12' associates the script with the shortcut Ctrl+Alt+Shift+F12
*/

public class Script : NppScript
{
    public override void Run()
    {
        string path;
        Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out path);
        MessageBox.Show(path);
    }
}