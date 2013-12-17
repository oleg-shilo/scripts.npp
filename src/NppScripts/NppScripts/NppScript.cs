using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NppScripts
{
    /// <summary>
    /// Base class for the all Notepad++ automation scripts to be inherited from.
    /// </summary>
    public abstract class NppScript
    {
        /// <summary>
        /// Gets or sets the shortcut for the automation script to be associated with.
        /// </summary>
        /// <value>
        /// The shortcut.
        /// </value>
        public ShortcutKey Shortcut { get; set; }

        /// <summary>
        /// Gets or sets the toolbar image for the automation script.
        /// </summary>
        /// <value>
        /// The toolbar image.
        /// </value>
        public Bitmap ToolbarImage { get; set; }

        /// <summary>
        /// Gets or sets the display name if the automation script.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the generic purpose tag for the automation script object.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the associated menu item should be checked on Notepad++ initialization.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the menu item should be checked on initialize; otherwise, <c>false</c>.
        /// </value>
        public bool CheckedOnInit { get; set; }

        /// <summary>
        /// Gets or sets the dockable panel associated with the automation script.
        /// </summary>
        /// <value>
        /// The dockable panel.
        /// </value>
        public Form DockablePanel { get; set; }

        /// <summary>
        /// Gets the automation script full file name.
        /// </summary>
        /// <value>
        /// The script file.
        /// </value>
        public string ScriptFile { get; internal set; }

        /// <summary>
        /// Gets the automation script identifier.
        /// </summary>
        /// <value>
        /// The script identifier.
        /// </value>
        public int ScriptId { get; internal set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NppScript"/> class.
        /// </summary>
        public NppScript()
        {
            ToolbarImage = null;
            Shortcut = ShortcutKey.None;
            Name = "Uninitialized Script";
            OnNotification = DefaultOnNotification;
        }

        /// <summary>
        /// The custom automation script specific handler for on <see cref="T:NppScripts.SCNotification"/>
        /// </summary>
        public Action<SCNotification> OnNotification;


        /// <summary>
        /// The default handler for on <see cref="T:NppScripts.SCNotification"/>.
        /// </summary>
        /// <param name="data">The data.</param>
        public void DefaultOnNotification(SCNotification data)
        {
        }

        /// <summary>
        /// Called when automation script is loaded.
        /// </summary>
        public virtual void OnLoaded()
        {
        }

        /// <summary>
        /// Called when user triggers the automation script execution.
        /// <para>It is the place for the automation script business logic</para>.
        /// </summary>
        public abstract void Run();
    }

#pragma warning disable 1591

    public class NppScriptStub : NppScript
    {
        public override void Run()
        {
        }
    }

    public class ScriptInfo
    {
        public string File;
        public string Name;
        public int Id;
        public object Tag;
        public Bitmap ToolbarImage;
        public ShortcutKey Shortcut = ShortcutKey.None;
        public bool CheckedOnInit = false;
        public DateTime LastModified;
        public NppScript Script;

        public ScriptInfo(string file, int id = -1)
        {
            Id = id;
            File = file;
            Name = ToDisplayName(file);

            if (!IsSeparator)
            {
                using (var reader = new StreamReader(file))
                {
                    while (PorcessConfigLine(reader.ReadLine()))
                    { }
                }

                LastModified = System.IO.File.GetLastWriteTime(file);
                Script = new NppScriptStub
                {
                    ScriptFile = file,
                    ScriptId = id
                };
            }
        }

        public bool IsLoaded
        {
            get
            {
                return Script != null;
            }
        }

        public bool IsSeparator
        {
            get
            {
                return Name == "---";
            }
        }

        bool PorcessConfigLine(string data)
        {
            /*
            //npp_tag OutputPanel
            //npp_toolbar_image Shell32.dll|3
            //npp_shortcut Ctrl+Alt+Shift+F12
            */
            if (data.StartsWith("//npp_toolbar_image "))
            {
                try
                {
                    string[] parts = data.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    parts = parts.Last().Split(new[] { '|' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    string file = parts.First();
                    if (file.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) || file.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        int index = 0;
                        if (parts.Count() == 2)
                            int.TryParse(parts.Last(), out index);

                        ToolbarImage = Win32.ExtractIcon(file, index).ToBitmap();
                    }
                }
                catch
                {
                    ToolbarImage = Win32.ExtractIcon("Shell32.dll", 1).ToBitmap();
                }
                return true;
            }
            else if (data.StartsWith("//npp_tag "))
            {
                try
                {
                    string[] parts = data.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
                    Tag = parts[1];
                }
                catch
                {
                }
                return true;
            }
            else if (data.StartsWith("//npp_shortcut "))
            {
                try
                {
                    string[] parts = data.Split(new[] { ' ', '+' }, StringSplitOptions.RemoveEmptyEntries);

                    Keys key;
                    if (Enum.TryParse<Keys>(parts.Last(), true, out key))
                    {
                        this.Shortcut = new ShortcutKey(parts.Contains("Ctrl"),
                                                        parts.Contains("Alt"),
                                                        parts.Contains("Shift"),
                                                        key);
                    }
                }
                catch { }
                return true;
            }
            else
                return false;
        }

        public override string ToString()
        {
            return Name;
        }

        string ToDisplayName(string path)
        {
            try
            {
                return System.IO.Path.GetFileNameWithoutExtension(path).Split(new[] { '.' }, 3).Last();
            }
            catch { }
            return path;
        }
    }
}