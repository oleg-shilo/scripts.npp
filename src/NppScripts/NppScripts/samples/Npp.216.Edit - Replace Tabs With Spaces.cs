//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Text;
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        Plugin.Output.Clear();
        try
        {
            string text = Npp.Document.GetAllText();
            Npp.Document.SetText(text.Replace("\t", "    "));

            Plugin.Output.WriteLine("'Replacing Tabs'...");
        }
        catch (Exception e)
        {
            Plugin.Output.WriteLine("'Replace Tabs' Error:\r\n" + e.Message);
        }
    }
}