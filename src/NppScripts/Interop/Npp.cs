using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NppScripts
{
    /// <summary>
    /// This class contains very generic wrappers for basic Notepad++ functionality.
    /// </summary>
    public class Npp
    {
        static Output output = new Output();

        /// <summary>
        /// Gets the window handle to current Scintilla.
        /// </summary>
        /// <value>
        /// The current window handle to scintilla.
        /// </value>
        public static IntPtr CurrentScintilla { get { return Plugin.GetCurrentScintilla(); } }

        /// <summary>
        /// Gets the Notepad++ main window handle.
        /// </summary>
        /// <value>
        /// The Notepad++ main window handle.
        /// </value>
        public static IntPtr NppHandle { get { return Plugin.NppData._nppHandle; } }

        /// <summary>
        /// Gets the output window object.
        /// </summary>
        /// <value>
        /// The output window object.
        /// </value>
        static public Output Output { get { return output; } }

        [DllImport("user32")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        /// <summary>
        /// Gets all lines of the current document.
        /// </summary>
        /// <returns></returns>
        static public string[] GetAllLines()
        {
            return Npp.GetAllText().Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        /// <summary>
        /// Gets all text of the current document.
        /// </summary>
        /// <returns></returns>
        static public string GetAllText()
        {
            int fullLength = (int)Win32.SendMessage(Npp.CurrentScintilla, SciMsg.SCI_GETLENGTH, 0, 0);
            using (var tr = new Sci_TextRange(0, fullLength, fullLength + 1))
            {
                Win32.SendMessage(Npp.CurrentScintilla, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                return tr.lpstrText;
            }
        }

        /// <summary>
        /// Gets the current screen location of the caret.
        /// </summary>
        /// <returns><c>Point</c> representing the coordinates of the screen location.</returns>
        static public Point GetCaretScreenLocation()
        {
            IntPtr sci = Plugin.GetCurrentScintilla();
            int pos = (int)Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int x = (int)Win32.SendMessage(sci, SciMsg.SCI_POINTXFROMPOSITION, 0, pos);
            int y = (int)Win32.SendMessage(sci, SciMsg.SCI_POINTYFROMPOSITION, 0, pos);

            Point point = new Point(x, y);
            ClientToScreen(sci, ref point);
            return point;
        }

        /// <summary>
        /// Gets the name of the current document.
        /// </summary>
        /// <returns></returns>
        static public string GetCurrentDocument()
        {
            string path;
            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_GETFULLCURRENTPATH, 0, out path);
            return path;
        }

        /// <summary>
        /// Gets the text between two text positions.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        /// <returns></returns>
        static public string GetTextBetween(int start, int end = -1)
        {
            IntPtr sci = Plugin.GetCurrentScintilla();

            if (end == -1)
                end = (int)Win32.SendMessage(sci, SciMsg.SCI_GETLENGTH, 0, 0);

            using (var tr = new Sci_TextRange(start, end, end - start + 1)) //+1 for null termination
            {
                Win32.SendMessage(sci, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                return tr.lpstrText;
            }
        }

        /// <summary>
        /// Get text before caret.
        /// </summary>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        static public string TextBeforeCaret(int maxLength = 512)
        {
            int bufCapacity = maxLength + 1;
            IntPtr hCurrentEditView = Plugin.GetCurrentScintilla();
            int currentPos = (int)Win32.SendMessage(hCurrentEditView, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int beginPos = currentPos - maxLength;
            int startPos = (beginPos > 0) ? beginPos : 0;
            int size = currentPos - startPos;

            if (size > 0)
            {
                using (var tr = new Sci_TextRange(startPos, currentPos, bufCapacity))
                {
                    Win32.SendMessage(hCurrentEditView, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
                    return tr.lpstrText;
                }
            }
            else
                return null;
        }

        /// <summary>
        /// Gets the word at cursor.
        /// </summary>
        /// <param name="point">The point - start and end position of the word.</param>
        /// <returns></returns>
        static public string GetWordAtCursor(out Point point)
        {
            IntPtr sci = Plugin.GetCurrentScintilla();

            int currentPos = (int)Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int fullLength = (int)Win32.SendMessage(sci, SciMsg.SCI_GETLENGTH, 0, 0);

            string leftText = Npp.TextBeforeCaret(512);
            string rightText = Npp.TextAfterCaret(512);

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
        /// <param name="replacement">The replacement.</param>
        static public void ReplaceWordAtCursor(string replacement)
        {
            IntPtr sci = Npp.CurrentScintilla;

            Point p;
            string word = Npp.GetWordAtCursor(out p);

            if (!string.IsNullOrWhiteSpace(word))
                Win32.SendMessage(sci, SciMsg.SCI_SETSELECTION, p.X, p.Y);

            Win32.SendMessage(sci, SciMsg.SCI_REPLACESEL, 0, replacement);
        }

        /// <summary>
        /// Saves the current document.
        /// </summary>
        static public void SaveCurrentDocument()
        {
            Win32.SendMessage(Npp.NppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);
        }

        /// <summary>
        /// Sets the all text of the current document.
        /// </summary>
        /// <param name="text">The text.</param>
        static public void SetAllText(string text)
        {
            Win32.SendMessage(Npp.CurrentScintilla, SciMsg.SCI_SETTEXT, 0, text);
        }

        /// <summary>
        /// Returns text after the current caret position.
        /// </summary>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        static public string TextAfterCaret(int maxLength = 512)
        {
            int bufCapacity = maxLength + 1;
            IntPtr sci = Npp.CurrentScintilla;
            int currentPos = (int)Win32.SendMessage(sci, SciMsg.SCI_GETCURRENTPOS, 0, 0);
            int fullLength = (int)Win32.SendMessage(sci, SciMsg.SCI_GETLENGTH, 0, 0);
            int startPos = currentPos;
            int endPos = Math.Min(currentPos + bufCapacity, fullLength);
            int size = endPos - startPos;

            if (size > 0)
            {
                using (var tr = new Sci_TextRange(startPos, endPos, bufCapacity))
                {
                    Win32.SendMessage(sci, SciMsg.SCI_GETTEXTRANGE, 0, tr.NativePointer);
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
    public class Output
    {
        string outputName = "Automation";

        /// <summary>
        /// Clears this output panel text.
        /// </summary>
        public void Clear()
        {
            try
            {
                Plugin.GetOutputPanel()
                      .Call("OpenOrCreateOutput", outputName)
                      .Call("Clear");
            }
            catch { } //may fail if the CS-Script integration is not available
        }

        /// <summary>
        /// Writes the text line onto the output panel. Supports formatting.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="args">The arguments.</param>
        public void WriteLine(string text, params object[] args)
        {
            try
            {
                Plugin.GetOutputPanel()
                      .Call("OpenOrCreateOutput", outputName)
                      .Call("WriteLine", text, args);
            }
            catch { } //may fail if the CS-Script integration is not available
        }
    }
}