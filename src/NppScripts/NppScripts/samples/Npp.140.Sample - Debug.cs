//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using System.Windows.Forms;
using NppScripts;

//run the script and attach the debugger when prompted

public class Script : NppScript
{
    public override void Run()
    {
        System.Diagnostics.Debug.Assert(false);

        int a = 1;
        int b = 2;
        int sum = a + b;
        MessageBox.Show(sum.ToString());
    }
}