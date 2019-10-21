using System.Text;
using System;
using System.Drawing;
using System.Threading;

namespace NppScripts
{
    public class Automation
    {
        static public void MenuFileNew()
        {
            Npp.Editor.SendMessage(NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FILE_NEW);
        }

        static public void TypeText(string text, int delay = 20)
        {
            int currentPos = Npp.Document.GetCurrentPos();

            Npp.Document.SetSel(currentPos, currentPos);

            foreach (char c in text)
            {
                Thread.Sleep(delay);
                Npp.Document.ReplaceSel(c.ToString());
                Npp.Document.SetCurrentPos(++currentPos);
            }
        }
    }
}