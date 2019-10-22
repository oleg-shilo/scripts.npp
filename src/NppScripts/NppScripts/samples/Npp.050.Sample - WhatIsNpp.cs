//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Threading.Tasks;
using System.Threading;
using System;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        WhatIsNpp();
    }

    void WhatIsNpp()
    {
        string text2display = "Notepad++ is a free (as in \"free speech\" and also as in \"free beer\") " +
                "source code editor and Notepad replacement that supports several languages.\n" +
                "Running in the MS Windows environment, its use is governed by GPL License.\n\n" +
                "Based on a powerful editing component Scintilla, Notepad++ is written in C++ and " +
                "uses pure Win32 API and STL which ensures a higher execution speed and smaller program size.\n" +
                "By optimizing as many routines as possible without losing user friendliness, Notepad++ is trying " +
                "to reduce the world carbon dioxide emissions. When using less CPU power, the PC can throttle down " +
                "and reduce power consumption, resulting in a greener environment.";

        Task.Factory.StartNew(() => TypeText(text2display));
    }

    void TypeText(string text2display)
    {
        // Open a new document
        Npp.Editor.SendMenuCommand(NppMenuCmd.IDM_FILE_NEW);

        foreach (char c in text2display)
        {
            Thread.Sleep(10);
            Npp.Document.SendMessage(SciMsg.SCI_APPENDTEXT, 1, c.ToString());
            Npp.Document.SendMessage(SciMsg.SCI_GOTOPOS, Npp.Document.GetLength(), 0);
        }
    }
}