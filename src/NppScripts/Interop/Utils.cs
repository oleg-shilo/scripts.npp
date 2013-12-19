using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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

    public static class Product
    {
        internal static string HomeUrl = "http://csscript.net/npp/csscript.html";

        static public string GetLatestAvailableVersion()
        {
            try
            {
                return DownloadText("http://csscript.net/npp/latest_npps_version.txt");
            }
            catch
            {
                return null;
            }
        }

        static public string GetLatestAvailableMsi(string version)
        {
            try
            {
                string downloadDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

                string destFile = Path.Combine(downloadDir, "NppScripts.msi");

                int numOfAlreadyDownloaded = Directory.GetFiles(downloadDir, "NppScripts*.msi").Count();
                if (numOfAlreadyDownloaded > 0)
                    destFile = Path.Combine(downloadDir, "NppScripts (" + (numOfAlreadyDownloaded + 1) + ").msi");

                DownloadBinary("http://csscript.net/npp/NppScripts." + version + ".msi", destFile);

                return destFile;
            }
            catch
            {
                return null;
            }
        }

        static void DownloadBinary(string url, string destinationPath, string proxyUser = null, string proxyPw = null)
        {
            var sb = new StringBuilder();
            byte[] buf = new byte[1024 * 4];

            if (proxyUser != null)
                WebRequest.DefaultWebProxy.Credentials = new NetworkCredential(proxyUser, proxyPw);

            var request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (var destStream = new FileStream(destinationPath, FileMode.CreateNew))
            using (var resStream = response.GetResponseStream())
            {
                int count = 0;
                while (0 < (count = resStream.Read(buf, 0, buf.Length)))
                {
                    destStream.Write(buf, 0, count);
                }
            }
        }

        static string DownloadText(string url, string proxyUser = null, string proxyPw = null)
        {
            var sb = new StringBuilder();
            byte[] buf = new byte[1024 * 4];

            if (proxyUser != null)
                WebRequest.DefaultWebProxy.Credentials = new NetworkCredential(proxyUser, proxyPw);

            var request = WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (var resStream = response.GetResponseStream())
            {
                string tempString = null;
                int count = 0;

                while (0 < (count = resStream.Read(buf, 0, buf.Length)))
                {
                    tempString = Encoding.ASCII.GetString(buf, 0, count);
                    sb.Append(tempString);
                }
                return sb.ToString();
            }
        }
    }
}
