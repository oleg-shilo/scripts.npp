//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Drawing;
using NppScripts;
using System;
using System.Windows.Forms;

public class Script : NppScript
{
    public override void Run()
    {
        TogglePanelVisibility();
    }

    void TogglePanelVisibility()
    {
        if (this.DockablePanel == null)
        {
            this.DockablePanel = new Form { BackColor = Color.Aqua };

            Plugin.DockPanel(DockablePanel, this.ScriptId, "Demo", null, NppTbMsg.DWS_DF_CONT_BOTTOM | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR);
        }
        else
        {
            Plugin.ToggleDockedPanelVisible(this.DockablePanel, this.ScriptId);
        }
    }
}