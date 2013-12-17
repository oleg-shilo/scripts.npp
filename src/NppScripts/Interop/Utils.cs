using CSScriptLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace NppScripts
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Normalizes the line breaks by replacing a single-"\n" breaks with "\r\n".
        /// </summary>
        /// <param name="text">The text to be normalized.</param>
        /// <returns></returns>
        public static string NormalizeLineBreaks(this string text)
        {
            if (text == null)
                return text;
            else
                return text.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Converts 16x16 bitmap into icon compatible with the Notepad++ toolbar buttons.
        /// <para><c>Color.Fuchsia</c> is used as a 'transparency' color. </para>
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static Icon NppBitmapToIcon(Bitmap bitmap)
        {
            using (Bitmap newBmp = new Bitmap(16, 16))
            {
                Graphics g = Graphics.FromImage(newBmp);
                ColorMap[] colorMap = new ColorMap[1];
                colorMap[0] = new ColorMap();
                colorMap[0].OldColor = Color.Fuchsia;
                colorMap[0].NewColor = Color.FromKnownColor(KnownColor.ButtonFace);
                ImageAttributes attr = new ImageAttributes();
                attr.SetRemapTable(colorMap);
                //g.DrawImage(new Bitmap(@"E:\Dev\Notepad++.Plugins\NppScripts\css_logo_16x16.png"), new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                g.DrawImage(bitmap, new Rectangle(0, 0, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel, attr);
                return Icon.FromHandle(newBmp.GetHicon());
            }
        }
    }
}
