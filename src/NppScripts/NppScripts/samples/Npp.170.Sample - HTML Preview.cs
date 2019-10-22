//npp_toolbar_image Shell32.dll|13
//css_ref %userprofile%\Documents\NppScripts\HtmlView.exe;
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Reflection;
using HtmlView;
using System.IO;
using System;
using System.Drawing;
using NppScripts;
using System;
using System.Windows.Forms;

public class Script : NppScript
{
    public override void OnLoaded()
    {
        //Notepad++ does not know the location of the script dependency assembly so pre-load it manually
        string panelAssembly = Path.Combine(Path.GetDirectoryName(this.ScriptFile), "HtmlView.exe");
        Assembly.LoadFrom(panelAssembly);

        this.OnNotification = (notification) =>
        {
            if (notification.Header.Code == (uint)NppMsg.NPPN_BUFFERACTIVATED) //new document tab is selected 
                Rebind();
        };
    }

    void Rebind()
    {
        if (this.DockablePanel is WebPanel)
        {
            var panel = (this.DockablePanel as WebPanel);
            string file = Npp.Editor.GetCurrentFilePath();

            if (file != panel.Url && file.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase))
                panel.Url = file;
        }
    }

    public override void Run()
    {
        if (this.DockablePanel == null)
        {
            var panel = new WebPanel();
            var needReboot = panel.SetIeVersion(10000, "notepad++.exe"); //IE10 

            if (needReboot)
                MessageBox.Show("Please restart Notepad++ in order for the IE version change to take effect.");

            Plugin.DockPanel(panel, this.ScriptId, "Html Preview", null, NppTbMsg.DWS_DF_FLOATING | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR);
            this.DockablePanel = panel;
            Rebind();
        }
        else
        {
            Plugin.ToggleDockedPanelVisible(this.DockablePanel, this.ScriptId);
        }
    }
}