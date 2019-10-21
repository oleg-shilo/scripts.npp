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
        Hello();
        Task.Factory.StartNew(callbackHelloFX);
    }

    void Hello()
    {
        Npp.Editor.SendMenuCommand(NppMenuCmd.IDM_FILE_NEW);
        Npp.Document.SetText("Hello, Notepad++... from .NET!");
    }

    void callbackHelloFX()
    {
        int currentZoomLevel = Npp.Document.SendMessage(SciMsg.SCI_GETZOOM, 0, 0);
        int i = currentZoomLevel;

        for (int j = 0; j < 4; j++)
        {
            for (; i >= -10; i--)
            {
                Npp.Document.SendMessage(SciMsg.SCI_SETZOOM, i, 0);
                Thread.Sleep(30);
            }

            Thread.Sleep(100);
            for (; i <= 20; i++)
            {
                Thread.Sleep(30);
                Npp.Document.SendMessage(SciMsg.SCI_SETZOOM, i, 0);
            }

            Thread.Sleep(100);
        }

        for (; i >= currentZoomLevel; i--)
        {
            Thread.Sleep(30);
            Npp.Document.SendMessage(SciMsg.SCI_SETZOOM, i, 0);
        }
    }
}