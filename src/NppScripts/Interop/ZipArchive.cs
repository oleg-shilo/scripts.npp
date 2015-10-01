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
    public class ZipArchiveTest
    {
        public static void Test()
        {
            using (var zip = ZipArchive.OpenOnFile(@"E:\<>\Projects\CS-Script.Npp\NppScripts\bin\Plugins\NppScripts\samples.zip"))
            {
                var arr = zip.Files.OrderBy(file => file.Name).Select(file => file.Name).ToArray();
                foreach (var name in arr)
                {
                    var data = zip.GetFile(name);
                    if (data.FolderFlag) continue;

                    //var text = new StreamReader(data.GetStream().Wr).ReadToEnd();
                    using (var fileStream = new FileStream(@"E:\<>\Projects\CS-Script.Npp\NppScripts\bin\Plugins\NppScripts\www\" + name, FileMode.Create))
                    {
                        data.GetStream().CopyTo(fileStream);
                    }
                }
            }
        }
    }

    //http://www.codeproject.com/Articles/209731/Csharp-use-Zip-archives-without-external-libraries
    class ZipArchive : IDisposable
    {
        object external;

        static Type GetExternalType(string name)
        {
            return typeof(System.IO.Packaging.Package).Assembly.GetType(name);
        }

        public enum CompressionMethodEnum { Stored, Deflated };

        public enum DeflateOptionEnum { Normal, Maximum, Fast, SuperFast };

        public static ZipArchive OpenOnFile(string path, FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read, FileShare share = FileShare.Read, bool streaming = false)
        {
            return new ZipArchive { external = GetExternalType("MS.Internal.IO.Zip.ZipArchive").CallStatic("OpenOnFile", path, mode, access, share, streaming) };
        }

        public static ZipArchive OpenOnStream(Stream stream, FileMode mode = FileMode.OpenOrCreate, FileAccess access = FileAccess.ReadWrite, bool streaming = false)
        {
            return new ZipArchive { external = GetExternalType("MS.Internal.IO.Zip.ZipArchive").CallStatic("OpenOnStream", stream, mode, access, streaming) };
        }

        public ZipFileInfo AddFile(string path, CompressionMethodEnum compmeth = CompressionMethodEnum.Deflated, DeflateOptionEnum option = DeflateOptionEnum.Normal)
        {
            var comp = GetExternalType("MS.Internal.IO.Zip.CompressionMethodEnum").GetStaticField(compmeth.ToString());
            var opti = GetExternalType("MS.Internal.IO.Zip.DeflateOptionEnum").GetStaticField(option.ToString());
            return new ZipFileInfo { external = external.Call("AddFile", path, comp, opti) };
        }

        public void DeleteFile(string name)
        {
            external.Call("DeleteFile", name);
        }

        public void Dispose()
        {
            ((IDisposable)external).Dispose();
        }

        public ZipFileInfo GetFile(string name)
        {
            return new ZipFileInfo { external = external.Call("GetFile", name) };
        }

        public IEnumerable<ZipFileInfo> Files
        {
            get
            {
                var coll = external.Call("GetFiles") as System.Collections.IEnumerable; //ZipFileInfoCollection
                foreach (var p in coll)
                    yield return new ZipFileInfo { external = p };
            }
        }

        public IEnumerable<string> FileNames
        {
            get { return Files.Select(p => p.Name).OrderBy(p => p); }
        }

        public struct ZipFileInfo
        {
            internal object external;

            public override string ToString()
            {
                return Name; ;
            }

            public string Name { get { return (string)external.GetProp("Name"); } }

            public DateTime LastModFileDateTime { get { return (DateTime)external.GetProp("LastModFileDateTime"); } }

            public bool FolderFlag { get { return (bool)external.GetProp("FolderFlag"); } }

            public bool VolumeLabelFlag { get { return (bool)external.GetProp("VolumeLabelFlag"); } }

            public object CompressionMethod { get { return external.GetProp("CompressionMethod"); } }

            public object DeflateOption { get { return external.GetProp("DeflateOption"); } }

            public Stream GetStream(FileMode mode = FileMode.Open, FileAccess access = FileAccess.Read)
            {
                return (Stream)external.Call("GetStream", mode, access);
            }
        }
    }
}
