//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using System.Windows.Forms;
using NppScripts;

// Running the sample:
// run this script to activate the scripted plugin
// create and open .test file
// Press "Ctrl+."

public class Script : NppScript
{
    bool? active;

    public override void Run()
    {
        ToggleActive();
    }

    void ToggleActive()
    {
        if (!active.HasValue)
        {
            if (KeyInterceptor.Instance.IsInstalled)
            {
                // KeyInterceptor operates on the global scope so if the script is modified
                // within the same Notepad++ session KeyInterceptor will keep the reference
                // to the old version of the script until Notepad++ is restarted.
                // 
                // Thus while KeyInterceptor is perfect for the production it is not the
                // most convenient choice during the script development.
                // 
                // You may want to consider using "//npp_shortcut" as a temporary shortcut
                // and switch to KeyInterceptor in the release build.               
                MessageBox.Show("You need restart Notepad++ before you can reset KeyInterceptor",
                                "Automation Scripts");
                return;
            }
            else
            {
                KeyInterceptor.Instance.Install();

                KeyInterceptor.Instance.Add(Keys.Space);
                KeyInterceptor.Instance.KeyDown += OnKeyDown;

                MessageBox.Show("KeyInterception script is initialized\nNow 'Ctrl+Space' will be intercepted in all '.test' documents",
                                "Automation Scripts");
                active = true;
            }
        }
        else
            active = !active;
    }

    void OnKeyDown(Keys key, int repeatCount, ref bool handled)
    {
        if (key == Keys.Space)
        {
            if (Npp.Editor.GetCurrentFilePath().EndsWith(".test"))
            {
                Modifiers modifiers = KeyInterceptor.GetModifiers();

                if (modifiers.IsCtrl && !modifiers.IsShift && !modifiers.IsAlt)
                {
                    MessageBox.Show("Ctrl+Space is pressed from KeyInterception", "Automation Scripts");
                    handled = true; //prevent other handlers (if any) form further handling of the key press event
                }
            }
        }
    }
}