//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts.dll
//css_ref C:\Program Files\Notepad++\plugins\NppScripts\NppScripts\NppScripts.asm.dll
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System;
using System.Windows.Forms;
using NppScripts;
using System.Drawing;

public class Script : NppScript
{
    public Script()
    {
        this.OnNotification = (notification) =>
        {
            if (notification.Header.Code == (uint)NppMsg.NPPN_BUFFERACTIVATED)
            {
                string file = Npp.Editor.GetCurrentFilePath();

                if (File.Exists(file) && this.DockablePanel is Form1)
                {
                    string dir = Path.GetDirectoryName(file);
                    (this.DockablePanel as Form1).SetWorkingDirectory(dir);
                }
            }
        };
    }

    public override void Run()
    {
        TogglePanelVisibility();
    }

    void TogglePanelVisibility()
    {
        if (this.DockablePanel == null)
        {
            this.DockablePanel = new Form1();
            Plugin.DockPanel(this.DockablePanel, this.ScriptId, "Document Cmd Prompt", null, NppTbMsg.DWS_DF_CONT_BOTTOM | NppTbMsg.DWS_ICONTAB | NppTbMsg.DWS_ICONBAR);
        }
        else
            Plugin.ToggleDockedPanelVisible(this.DockablePanel, this.ScriptId);
    }

    public class Form1 : Form
    {
        TextBox textBox1;
        TextBox textBox2;
        Button button1;

        public Form1()
        {
            //Debug.Assert(false);

            textBox1 = new TextBox();
            textBox2 = new TextBox();
            button1 = new Button();

            textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
            textBox1.Location = new System.Drawing.Point(3, 3);
            textBox1.Size = new System.Drawing.Size(233, 20);
            textBox1.TabIndex = 0;
            textBox1.KeyDown += textBox1_KeyDown;

            textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                            | System.Windows.Forms.AnchorStyles.Left)
                            | System.Windows.Forms.AnchorStyles.Right)));
            textBox2.Location = new System.Drawing.Point(0, 27);
            textBox2.Multiline = true;
            textBox2.ReadOnly = true;
            textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            textBox2.Size = new System.Drawing.Size(284, 234);
            textBox2.TabIndex = 0;

            textBox1.Font =
            textBox2.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            button1.Location = new System.Drawing.Point(237, 1);
            button1.Size = new System.Drawing.Size(44, 23);
            button1.Text = "Break";
            button1.Click += button1_Click;

            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(button1);
            this.Controls.Add(textBox2);
            this.Controls.Add(textBox1);

            Prompt();
        }

        void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                string cmd = textBox1.Text;
                textBox1.Text = "";
                Run(cmd);
            }
        }

        Process proc = new Process();

        void Prompt()
        {
            if (textBox2.Text.Length != 0)
                textBox2.Text += Environment.NewLine;
            textBox2.Text += WorkingDirectory + ">";
            ScrollToEnt();
            textBox1.Select();
        }

        void Run(string cmd)
        {
            if (string.Compare(cmd.Trim(), "cls", true) == 0)
            {
                textBox2.Text = "";
                Prompt();
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    WriteLine(cmd);
                    try { proc.Kill(); } catch { }

                    lock (proc)
                    {
                        proc.StartInfo.FileName = "cmd.exe";
                        proc.StartInfo.Arguments = "/C " + cmd;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.WorkingDirectory = WorkingDirectory;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.StartInfo.RedirectStandardInput = true;
                        proc.StartInfo.RedirectStandardError = true;
                        proc.StartInfo.CreateNoWindow = true;

                        proc.Start();
                    }

                    InUIThread(() =>
                    {
                        button1.Enabled = true;
                        textBox1.Enabled = false;
                    });

                    Action<StreamReader> receive = (reader) =>
                    {
                        Task.Factory.StartNew((Action)delegate
                        {
                            string line = null;

                            while (null != (line = reader.ReadLine()))
                                WriteLine(line);
                        });
                    };

                    receive(proc.StandardOutput);
                    receive(proc.StandardError);
                    proc.WaitForExit();

                    InUIThread(() =>
                    {
                        button1.Enabled = false;
                        textBox1.Enabled = true;
                        textBox1.Select();
                        Prompt();
                    });

                    Debug.WriteLine("Done.......");
                });
            }
        }

        void WriteLine(string text)
        {
            InUIThread(() =>
            {
                textBox2.Text += text + Environment.NewLine;
                ScrollToEnt();
            });
        }

        void ScrollToEnt()
        {
            textBox2.SelectionLength = 0;
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.ScrollToCaret();
        }

        void InUIThread(Action action)
        {
            lock (this)
            {
                this.Invoke(action);
            }
        }

        public string WorkingDirectory = Environment.CurrentDirectory;

        public void SetWorkingDirectory(string path)
        {
            WorkingDirectory = path;
            Prompt();
        }

        void button1_Click(object sender, EventArgs e)
        {
            try
            {
                proc.Kill();
            }
            catch { }
        }
    }
}