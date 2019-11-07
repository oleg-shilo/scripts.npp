using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NppScripts
{
    /// <summary>
    /// This class contains very generic wrappers for basic Notepad++ functionality.
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