//npp_shortcut Ctrl+F11
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System;
using System.IO;
using System.Linq;
using System.Xml;
using NppScripts;

public class Script : NppScript
{
    public override void Run()
    {
        try
        {
            string text = Npp.Document.GetAllText();
            Npp.Document.SetText(FormatXML(text));
        }
        catch (Exception e)
        {
            Plugin.Output.Clear();
            Plugin.Output.WriteLine("'Format XML Document' Error:\r\n" + e.Message);
        }
    }

    string FormatXML(string data)
    {
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(data);

        using (var stringWriter = new StringWriter())
        using (var xmlWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented, Indentation = 4 })
        {
            xmlDocument.WriteTo(xmlWriter);
            return stringWriter.ToString();
        }
    }
}