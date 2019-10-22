// NPP plugin platform for .Net v0.93.96 by Kasper B. Graversen etc.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using NppPluginNET.PluginInfrastructure;

using hosting = NppPluginNET.PluginInfrastructure;

#if nppscripts

namespace NppScripts
#else
namespace Kbg.NppPluginNET.PluginInfrastructure
#endif
{
    /// <summary>
    ///
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="scintilla">The scintilla.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static int SendMessage(this ScintillaGateway scintilla, SciMsg Msg, int wParam, int lParam)
            => (int)Win32.SendMessage(scintilla.Handle, Msg, wParam, lParam);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="scintilla">The scintilla.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static int SendMessage(this ScintillaGateway scintilla, SciMsg Msg, int wParam, IntPtr lParam)
            => (int)Win32.SendMessage(scintilla.Handle, Msg, wParam, lParam);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="scintilla">The scintilla.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static int SendMessage(this ScintillaGateway scintilla, SciMsg Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam)
            => (int)Win32.SendMessage(scintilla.Handle, Msg, wParam, lParam);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <param name="expectedSize">The expected size.</param>
        public static void SendMessage(this NotepadPPGateway editor, NppMsg Msg, int wParam, out string lParam, int expectedSize = 2000)
        {
            var path = new StringBuilder(expectedSize);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETFULLCURRENTPATH, path.Capacity, path);
            lParam = path.ToString();
        }

        /// <summary>
        /// Gets all text of the current document.
        /// </summary>
        static public string GetAllText(this ScintillaGateway scintilla)
            => scintilla.GetText(scintilla.GetTextLength() + 1);

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="Msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns></returns>
        public static int SendMessage(this NotepadPPGateway editor, NppMsg Msg, int wParam, int lParam)
            => (int)Win32.SendMessage(editor.Handle, Msg, wParam, lParam);

        /// <summary>
        /// Sends the menu command.
        /// </summary>
        /// <param name="editor">The editor.</param>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        public static IntPtr SendMenuCommand(this NotepadPPGateway editor, NppMenuCmd command)
            => Win32.SendMessage(editor.Handle, (uint)NppMsg.NPPM_MENUCOMMAND, 0, NppMenuCmd.IDM_FILE_NEW);

        /// <summary>
        /// Selects all.
        /// </summary>
        /// <param name="document">The document.</param>
        static public void SelectAll(this ScintillaGateway document)
            => document.SetSel(0, document.GetLength());

        /// <summary>
        /// Gets the text between two text positions.
        /// </summary>
        /// <param name="sci">The scintilla object.</param>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        /// <returns></returns>
        static public string GetTextBetween(this ScintillaGateway sci, int start, int end = -1)
        {
            if (end == -1)
                end = sci.SendMessage(SciMsg.SCI_GETLENGTH, 0, 0);

            using (var tr = new TextRange(start, end, end - start + 1)) //+1 for null termination
            {
                sci.SendMessage(SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                return tr.lpstrText;
            }
        }

        /// <summary>
        /// Get text before caret.
        /// </summary>
        /// <param name="document">The scintilla object.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        static public string TextBeforeCaret(this ScintillaGateway document, int maxLength = 512)
        {
            int bufCapacity = maxLength + 1;
            int currentPos = document.SendMessage(SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int beginPos = currentPos - maxLength;
            int startPos = (beginPos > 0) ? beginPos : 0;
            int size = currentPos - startPos;

            if (size > 0)
            {
                using (var tr = new TextRange(startPos, currentPos, bufCapacity))
                {
                    document.SendMessage(SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                    return tr.lpstrText;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the word at cursor.
        /// </summary>
        /// <param name="document">The scintilla object.</param>
        /// <param name="point">The point - start and end position of the word.</param>
        /// <returns></returns>
        static public string GetWordAtCursor(this ScintillaGateway document, out Point point)
        {
            int currentPos = document.SendMessage(SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int fullLength = document.SendMessage(SciMsg.SCI_GETLENGTH, 0, 0);

            string leftText = document.TextBeforeCaret(512);
            string rightText = document.TextAfterCaret(512);

            var delimiters = "\t\n\r .,:;'\"[]{}()".ToCharArray();

            string wordLeftPart = "";
            int startPos = currentPos;

            if (leftText != null)
            {
                startPos = leftText.LastIndexOfAny(delimiters);
                wordLeftPart = (startPos != -1) ? leftText.Substring(startPos + 1) : "";
                int relativeStartPos = leftText.Length - startPos;
                startPos = (startPos != -1) ? (currentPos - relativeStartPos) + 1 : 0;
            }

            string wordRightPart = "";
            int endPos = currentPos;
            if (rightText != null)
            {
                endPos = rightText.IndexOfAny(delimiters);
                wordRightPart = (endPos != -1) ? rightText.Substring(0, endPos) : "";
                endPos = (endPos != -1) ? currentPos + endPos : fullLength;
            }

            point = new Point(startPos, endPos);
            return wordLeftPart + wordRightPart;
        }

        /// <summary>
        /// Replaces the word at cursor.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="replacement">The replacement.</param>
        static public void ReplaceWordAtCursor(this ScintillaGateway document, string replacement)
        {
            Point p;
            string word = Npp.Document.GetWordAtCursor(out p);

            if (!string.IsNullOrWhiteSpace(word))
                document.SendMessage(SciMsg.SCI_SETSELECTION, p.X, p.Y);

            document.SendMessage(SciMsg.SCI_REPLACESEL, 0, replacement);
        }

        /// <summary>
        /// Selects the wor at caret.
        /// </summary>
        /// <param name="document">The document.</param>
        static public void SelectWorAtCaret(this ScintillaGateway document)
        {
            Point p;
            document.GetWordAtCursor(out p);
            document.SendMessage(SciMsg.SCI_SETSELECTION, p.X, p.Y);
        }

        /// <summary>
        /// Returns text after the current caret position.
        /// </summary>
        /// <param name="document">The scintilla object.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        static public string TextAfterCaret(this ScintillaGateway document, int maxLength = 512)
        {
            int bufCapacity = maxLength + 1;
            int currentPos = document.SendMessage(SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int fullLength = document.SendMessage(SciMsg.SCI_GETLENGTH, 0, 0);
            int startPos = currentPos;
            int endPos = Math.Min(currentPos + bufCapacity, fullLength);
            int size = endPos - startPos;

            if (size > 0)
            {
                using (var tr = new TextRange(startPos, endPos, bufCapacity))
                {
                    document.SendMessage(SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                    return tr.lpstrText;
                }
            }
            else
                return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public static class Npp
    {
        /// <summary>
        /// Gets the editor object representing Notepad++ environment.
        /// </summary>
        /// <value>
        /// The editor.
        /// </value>
        public static NotepadPPGateway Editor { get { return PluginBase.Editor; } }

        /// <summary>
        /// Gets the Scintilla document object.
        /// </summary>
        /// <value>
        /// The document.
        /// </value>
        public static ScintillaGateway Document
            => (ScintillaGateway)PluginBase.GetCurrentDocument();

        [DllImport("user32")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        /// <summary>
        /// Gets the current screen location of the caret.
        /// </summary>
        /// <returns><c>Point</c> representing the coordinates of the screen location.</returns>
        static public Point GetCaretScreenLocation()
        {
            IntPtr sci = Npp.Document.Handle;
            int pos = (int)Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int x = (int)Win32.SendMessage(sci, SciMsg.SCI_POINTXFROMPOSITION, 0, pos);
            int y = (int)Win32.SendMessage(sci, SciMsg.SCI_POINTYFROMPOSITION, 0, pos);

            Point point = new Point(x, y);
            ClientToScreen(sci, ref point);
            return point;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface INotepadPPGateway
    {
        /// <summary>
        /// Invokes "New File" menu item.
        /// </summary>
        /// <returns></returns>
        NotepadPPGateway FileNew();

        /// <summary>
        /// Gets the current file path.
        /// </summary>
        /// <returns></returns>
        string GetCurrentFilePath();

        /// <summary>
        /// Gets the file path of a specific document buffer (Notepad++ tab).
        /// </summary>
        /// <param name="bufferId">The buffer identifier.</param>
        /// <returns></returns>
        unsafe string GetFilePath(int bufferId);

        /// <summary>
        /// Sets the current language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <returns></returns>
        NotepadPPGateway SetCurrentLanguage(LangType language);
    }

#pragma warning disable 1591

    /// <summary>
    /// This class holds helpers for sending messages defined in the Msgs_h.cs file. It is at the moment
    /// incomplete. Please help fill in the blanks.
    /// </summary>
    public class NotepadPPGateway : INotepadPPGateway
    {
        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>
        /// The handle.
        /// </value>
        public IntPtr Handle { get { return PluginBase.nppData._nppHandle; } }

        private const int Unused = 0;

        IntPtr send(NppMsg command, int wParam, NppMenuCmd lParam)
        {
            return Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)command, wParam, lParam);
        }

        IntPtr send(NppMsg command, IntPtr wParam, IntPtr lParam)
        {
            return Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)command, wParam, lParam);
        }

        public NotepadPPGateway FileNew()
        {
            send(NppMsg.NPPM_MENUCOMMAND, Unused, NppMenuCmd.IDM_FILE_NEW);
            return this;
        }

        public NotepadPPGateway ReloadFile(string file, bool showAlert)
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_RELOADFILE, showAlert ? 1 : 0, file);
            return this;
        }

        public NotepadPPGateway SaveCurrentFile()
        {
            send(NppMsg.NPPM_SAVECURRENTFILE, Unused, Unused);
            return this;
        }

        public NotepadPPGateway FileExit()
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_MENUCOMMAND, Unused, NppMenuCmd.IDM_FILE_EXIT);
            return this;
        }

        public NotepadPPGateway Open(string file)
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_DOOPEN, Unused, file);
            return this;
        }

        public string[] GetOpenFilesRaw()
        {
            var count = send(NppMsg.NPPM_GETNBOPENFILES, Unused, Unused);

            using (var cStrArray = new ClikeStringArray(count.ToInt32(), Win32.MAX_PATH))
            {
                if (send(NppMsg.NPPM_GETOPENFILENAMES, cStrArray.NativePointer, count) != IntPtr.Zero)
                    return cStrArray.ManagedStringsUnicode.ToArray();
                else
                    return new string[0];
            }
        }

        public IntPtr GetTabCount()
        {
            return send(NppMsg.NPPM_GETNBOPENFILES, Unused, Unused);
        }

        public IntPtr GetBufferIdFromTab(int tabIndex)
        {
            return send(NppMsg.NPPM_GETBUFFERIDFROMPOS, tabIndex, 0);
        }

        public string GetTabFileFromPosition(int tabIndex)
        {
            var id = this.GetBufferIdFromTab(tabIndex);
            return this.GetTabFile(id);
        }

        public string[] GetOpenFilesSafe()
        {
            var count = this.GetTabCount().ToInt32();

            var files = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var file = this.GetTabFileFromPosition(i);

                if (!string.IsNullOrEmpty(file))
                    files.Add(file);
            }
            return files.ToArray();
        }

        public string GetTabFile(IntPtr index)
        {
            var path = new StringBuilder(2000);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETFULLPATHFROMBUFFERID, index, path);
            return path.ToString();
        }

        /// <summary>
        /// Gets the path of the current document.
        /// </summary>
        public string GetPluginsConfigDir()
        {
            var path = new StringBuilder(2000);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETPLUGINSCONFIGDIR, path.Capacity, path);
            return path.ToString();
        }

        /// <summary>
        /// Gets the path of the current document.
        /// </summary>
        public string GetCurrentFilePath()
        {
            var path = new StringBuilder(2000);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETFULLCURRENTPATH, path.Capacity, path);
            return path.ToString();
        }

        /// <summary>
        /// Gets the path of the current document.
        /// </summary>
        public unsafe string GetFilePath(int bufferId)
        {
            var path = new StringBuilder(2000);
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufferId, path);
            return path.ToString();
        }

        public NotepadPPGateway SetCurrentLanguage(LangType language)
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, Unused, (int)language);
            return this;
        }
    }

    /// <summary>
    /// This class holds helpers for sending messages defined in the Resource_h.cs file. It is at the moment
    /// incomplete. Please help fill in the blanks.
    /// </summary>
    class NppResource
    {
        private const int Unused = 0;

        public void ClearIndicator()
        {
            Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)Resource.NPPM_INTERNAL_CLEARINDICATOR, Unused, Unused);
        }
    }
}