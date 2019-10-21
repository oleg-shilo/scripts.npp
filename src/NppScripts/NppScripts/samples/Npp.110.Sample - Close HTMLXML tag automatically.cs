//npp_shortcut Alt+Q
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System;
using System.Windows.Forms;
using NppScripts;

// to activete the scripted plugin press Alt+Q or select the menu item  
// Plugins->'Automation Scripts'->Sample - Close HTMLXML tag automatically' 
public class Script : NppScript
{
    public Script()
    {
        this.OnNotification = (notification) =>
        {
            if (notification.Header.Code == (uint)SciMsg.SCN_CHARADDED)
            {
                doInsertHtmlCloseTag(notification.Character);
            }
        };
    }

    bool doCloseTag;

    void checkInsertHtmlCloseTag()
    {
        doCloseTag = !doCloseTag;
        Npp.Editor.SendMessage(NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[this.ScriptId]._cmdID, doCloseTag ? 1 : 0);
    }

    public override void Run()
    {
        checkInsertHtmlCloseTag();
    }

    void doInsertHtmlCloseTag(char newChar)
    {
        var docType = LangType.L_TEXT;
        Win32.SendMessage(Npp.Editor.Handle, NppMsg.NPPM_GETCURRENTLANGTYPE, 0, ref docType);
        bool isDocTypeHTML = (docType == LangType.L_HTML || docType == LangType.L_XML || docType == LangType.L_PHP);

        if (doCloseTag && isDocTypeHTML)
        {
            var text_on_left = Npp.Document.TextBeforeCaret();

            if (text_on_left.EndsWith(">")) // last char of the tag
            {
                var pos = Npp.Document.GetSelectionEnd();

                var lastOpeningTag = new Regex(@"<\s*\w+").Matches(text_on_left).Cast<Match>().LastOrDefault();
                var lastClosingTag = new Regex(@"</\s*\w+").Matches(text_on_left).Cast<Match>().LastOrDefault();

                if (lastOpeningTag == null)
                    return;
                if (lastClosingTag != null && lastOpeningTag.Index < lastClosingTag.Index)
                    return;

                var closingTag = lastOpeningTag.Value.Replace("<", "</") + ">";

                Npp.Document.BeginUndoAction();
                Npp.Document.ReplaceSel(closingTag);
                Npp.Document.EndUndoAction();
                Npp.Document.SetSel(pos, pos);
            }
        }
    }
}