//npp_toolbar_image Shell32.dll|3
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        System.Diagnostics.Process.Start("explorer.exe", "/select," + Npp.Editor.GetCurrentFilePath());
    }
}