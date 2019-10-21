//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        ClearOutput();
        WriteLine("Current file is {0}", Npp.Editor.GetCurrentFilePath());
        WriteLine("Current time is " + DateTime.Now);
        Plugin.Output.WriteLine("Test"); // default output stream (id: "Automation") of the plugin output panel
    }

    void WriteLine(string text, params object[] args)
    {
        Plugin.GetOutputPanel()
              .Call("OpenOrCreateOutput", "Automation")
              .Call("WriteLine", text, args);
    }

    void ClearOutput()
    {
        Plugin.GetOutputPanel()
              .Call("OpenOrCreateOutput", "Automation")
              .Call("Clear");
    }
}