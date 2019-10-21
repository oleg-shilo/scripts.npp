//css_dir C:\Program Files\Notepad++\plugins\NppScripts
//css_ref NppScripts.dll
//css_ref NppScripts\NppScripts.asm.dll
//css_inc automation.cs
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Windows.Forms;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        Automation.MenuFileNew();

        Task.Factory.StartNew(() =>
        {
            Automation.TypeText("Hello, Notepad++... from .NET!");
            Blink(6, 7, 9, '.');
            SelectWordsAt(0, 9, 23, 29);
            SplitCharacters();
            ReportCompletion();
        });
    }

    void Blink(int count, int start, int length, char substitutionCharBase = ' ')
    {
        string original = Npp.Document.GetTextBetween(start, start + length);
        string hidden = new string(substitutionCharBase, length);

        for (int i = 0; i < count; i++)
        {
            Thread.Sleep(500);

            string replacement = (i % 2) == 0 ? hidden : original;
            Npp.Document.SetSel(start, start + length);
            Npp.Document.ReplaceSel(replacement);
            Npp.Document.SetSel(0, 0);
        }
    }

    void ReportCompletion()
    {
        Npp.Document.SetSel(0, 0);
        Npp.Document.ReplaceSel("\r\n");
        Npp.Document.ReplaceSel("\r\n");
        Npp.Document.SetSel(0, 0);
        Automation.TypeText("---------- ALL DONE ----------");
        Blink(8, 11, 8);
        Npp.Document.SetSel(0, 0);
        Thread.Sleep(500);
    }

    void SplitCharacters()
    {
        string text = Npp.Document.GetAllText();
        int pos = text.Length - 1;

        while (pos > 0)
        {
            Thread.Sleep(50);
            Npp.Document.SetSel(pos, pos);
            Npp.Document.ReplaceSel("\r\n");
            pos--;
        }

        Npp.Document.SetText("");
        Thread.Sleep(500);
    }

    void SelectWordsAt(params int[] positions)
    {
        foreach (int pos in positions)
        {
            Thread.Sleep(700);
            Npp.Document.SetSel(pos, pos);
            Npp.Document.SelectWorAtCaret();
        }

        Thread.Sleep(500);
    }

}