//npp_shortcut Ctrl+Shift+F11
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Text;
using System;
using System.Windows.Forms;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        string text = Npp.Document.GetAllText();
        Npp.Document.SetText(FormatCSharp(text));
    }

    string FormatCSharp(string text)
    {
        var buffer = new StringBuilder();

        bool prevLineIsEmpty = false;

        foreach (string line in text.Split(new[] { Environment.NewLine }, System.StringSplitOptions.None))
        {
            bool isEmpty = string.IsNullOrWhiteSpace(line);

            if (isEmpty)
            {
                if (!prevLineIsEmpty)
                    buffer.AppendLine("");
            }
            else
                buffer.AppendLine(line);

            prevLineIsEmpty = isEmpty;
        }
        return buffer.ToString();
    }
}