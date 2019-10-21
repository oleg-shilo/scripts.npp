//npp_shortcut Alt+OemPeriod
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll

// Ctrl+Space and Ctrl+OemPeriod are already taken so let's use "Alt+."
// if "Ctrl+Space" is really needed then KeyInterception should be used (see KeyInterception.cs code sample for details).

// Running the sample:
// create and open .dsl file
// start typing (e.g. character 'A') and press "Alt+."
// observe auto-completion suggestions (e.g. "Aaron, Adam, ...");
// use arrow keys and to select the suggestion
// press 'Enter' to accept the selected suggestion

using System.Collections.Generic;
using NppScripts;
using System.Drawing;
using System.Linq;
using System;
using System.Windows.Forms;

public class Script : NppScript
{
    public override void Run()
    {
        if (Npp.Editor.GetCurrentFilePath().EndsWith(".dsl"))
        {
            string hint = Editor.GetSuggestionHint();
            string[] suggestions = Parser.GetSuggestions(hint);
            Point caretPoint = Npp.GetCaretScreenLocation();

            Form form;

            if (suggestions.Length > 0)
                form = new SuggestionList(suggestions, caretPoint, Editor.ApplySuggestion);
            else
                form = new SuggestionList(new[] { "< no suggestions >" }, caretPoint);

            form.Show();
        }
    }

    class Editor
    {
        public static string GetSuggestionHint()
        {
            //cannot use Npp.GetWordAtCursor() as we need only the left part of the word
            string hint = "";

            string text = Npp.Document.TextBeforeCaret();

            if (!string.IsNullOrWhiteSpace(text))
            {
                int pos = text.LastIndexOf(" ");

                if (pos != -1)
                {
                    hint = text.Substring(pos + 1);
                }
            }

            return hint;
        }

        public static void ApplySuggestion(string suggestion)
        {
            Npp.Document.ReplaceWordAtCursor(suggestion);
        }
    }

    class Parser
    {
        public static string[] GetSuggestions(string hint)
        {
            var namesData = "Dennis,Beverly,Christina,Marilyn,Joan,Nicholas,Ryan,Sara,Teresa,Charles,Frank,Janet,Kelly,Chris,Ruth,Steven,Eugene,Katherine,Sharon,Diane,Russell,Keith,Louise,Marie,Anthony,Rachel,Kenneth,Kathy,Thomas,Virginia,Diana,Jean,Jimmy,Ruby,Kathryn,Gary,Benjamin,Amanda,Peter,Jeremy,Lori,Jack,Phillip,Douglas,Richard,Philip,William,Brandon,Adam,Emily,Patricia,Doris,Gerald,Julie,Stephen,Elizabeth,Michelle,Lisa,Jane,Albert,Earl,Linda,Billy,Roy,Carlos,Shawn,Mark,Amy,Jennifer,Henry,Carolyn,Anna,Walter,Sean,Terry,Fred,Jose,Betty,Heather,Donald,Michael,Raymond,Tammy,Rose,Ann,Larry,Helen,Cheryl,Harry,Ralph,Carol,Cynthia,Patrick,Phyllis,Todd,Clarence,Aaron,Mildred,Justin,Dennis,Lois,Beverly,Christina,Marilyn,Joan,Nicholas,Ryan,Sara,Teresa,Charles,Frank,Janet,Kelly,Chris,Ruth,Steven,Eugene,Katherine,Sharon,Diane,Russell,Keith,Louise,Marie,Anthony,Rachel,Kenneth,Kathy,Thomas,Virginia,Diana,Jean,Jimmy,Ruby,Kathryn,Gary,Benjamin,Peter,Jeremy,Lori,Jack,Phillip,Douglas,Richard,Philip,William,Brandon,Emily,Patricia,Doris,Gerald,Julie,Stephen,Elizabeth,Michelle,Lisa,Jane,Earl,Linda,Billy,Roy,Carlos,Shawn,Mark,Jennifer,Henry,Carolyn,Walter,Sean,Terry,Fred,Jose,Betty,Heather,Donald,Michael,Raymond,Tammy,Rose,Larry,Helen,Cheryl,Harry,Ralph,Carol,Cynthia,Patrick,Phyllis,Todd,Clarence,Mildred,Justin";

            var allSuggestions = namesData.Split(',').OrderBy(x => x);

            if (string.IsNullOrWhiteSpace(hint))
                return allSuggestions.ToArray();
            else
                return allSuggestions.Where(x => x.StartsWith(hint, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }
    }

    class SuggestionList : Form
    {
        public SuggestionList(string[] items, Point point, Action<string> onSuggestionAccepted = null)
        {
            if (onSuggestionAccepted == null)
                onSuggestionAccepted = x => { };

            var listBox = new ListBox();
            listBox.Dock = DockStyle.Fill;
            listBox.Items.AddRange(items);
            listBox.TabIndex = 0;
            listBox.SelectedIndex = 0;

            this.Size = new Size(30, 100);
            this.Top = point.Y + 18;
            this.Left = point.X;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.KeyPreview = true;
            this.ShowInTaskbar = false;

            this.KeyDown += (sender, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
                else if (e.KeyCode == Keys.Return)
                {
                    onSuggestionAccepted(listBox.SelectedItem as string);
                    Close();
                }
            };

            listBox.DoubleClick += (s, e) =>
            {
                onSuggestionAccepted(listBox.SelectedItem as string);
                Close();
            };

            this.Controls.Add(listBox);
        }
    }
}