using System;
using System.Windows.Forms;

#pragma warning disable 1591

namespace NppScripts
{
    public partial class ScriptNameInput : Form
    {
        public ScriptNameInput()
        {
            InitializeComponent();
        }

        private void ScriptNameInput_Load(object sender, EventArgs e)
        {
            nameTextBox.SelectAll();
            nameTextBox.Focus();
        }

        public string ScriptName
        {
            get
            {
                return nameTextBox.Text;
            }
        }
    }
}