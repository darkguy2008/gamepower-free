using FastColoredTextBoxNS;
using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace GPIDE
{
    public partial class frmMain : Form
    {
        public static string ApplicationPath { get { return new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName + "\\"; } }
        public enum ECompileMode
        {
            Offline = 1,
            Online = 2
        }

        public bool FirstOpen = true;
        public String originalText = String.Empty;
        public String Filename = String.Empty;
        public FCTBThemes.FCTBTheme Theme;
        public Dictionary<String, Dictionary<String, String>> Settings;
        public String DebugTempPath = String.Empty;
        public Process DebugProcess = null;
        public ECompileMode CompileMode
        {
            get { return tsCompileMode.SelectedIndex == 0 ? ECompileMode.Offline : ECompileMode.Online; }
        }

        public frmMain()
        {
            InitializeComponent();
            Icon = Properties.Resources.gpIDE;
            NewFCTB();
            Reload();
            NewDebugSession();
            tsCompileMode.Items.Add("Offline");
            tsCompileMode.Items.Add("Online");
            tsCompileMode.SelectedIndex = 0;
            this.FormClosed += FrmMain_FormClosed;
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            KillDebugProcess();
        }

        public void Reload()
        {
            Settings = INI.Load(ApplicationPath + "Config.ini");
            tsCompile.Enabled = !String.IsNullOrEmpty(Filename);
            tsDebug.Enabled = tsCompile.Enabled;
            tsRun.Enabled = tsCompile.Enabled;
            tsPack.Enabled = tsCompile.Enabled;
        }

        public void NewFCTB()
        {
            this.Controls["panel1"].Controls.Clear();

            Editor = new FastColoredTextBox();
            Editor.TextChanged += Editor_TextChanged;
            Editor.CustomAction += Editor_CustomAction;
            Editor.Dock = DockStyle.Fill;

            Theme = FCTBThemes.ThemeDIV2;
            FCTBThemes.SetTheme(Theme, Editor);

            Editor.HotkeysMapping[Keys.Control | Keys.N] = FCTBAction.CustomAction1;
            Editor.HotkeysMapping[Keys.Control | Keys.O] = FCTBAction.CustomAction2;
            Editor.HotkeysMapping[Keys.Control | Keys.S] = FCTBAction.CustomAction3;
            Editor.HotkeysMapping[Keys.F6] = FCTBAction.CustomAction4;
            Editor.HotkeysMapping[Keys.F5] = FCTBAction.CustomAction5;

            this.Controls["panel1"].Controls.Add(Editor);
        }

        public void NewDebugSession()
        {
            String tmpPath = Path.GetTempFileName();

            foreach (String dir in Directory.GetDirectories(Path.GetTempPath(), "GPDebug*"))
                try
                {
                    Directory.Delete(dir, true);
                }
                catch(Exception)
                {
                }

            DebugTempPath = new FileInfo(tmpPath).DirectoryName + "\\GPDebug_" + new FileInfo(tmpPath).Name;
            FolderManager.CopyFolderContents(Path.GetFullPath(Settings["Paths"]["Runtime"]), DebugTempPath);

            RefreshDebugProcess();
        }

        public void RefreshDebugProcess()
        {
            if (DebugProcess != null)
                if (DebugProcess.HasExited)
                    DebugProcess = null;

            if(DebugProcess == null)
            {
                ProcessStartInfo pi = new ProcessStartInfo()
                {
                    FileName = Program.ApplicationPath + "\\DebugServer.exe",
                    Arguments = "\"" + DebugTempPath + "\" " + Settings["Debug"]["Port"],
                    RedirectStandardInput = true,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false,
                    UseShellExecute = false
                };
                DebugProcess = Process.Start(pi);
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(Program.ApplicationPath + "\\refresh"))
                    sw.WriteLine(DebugTempPath);
            }
        }

        public void KillDebugProcess()
        {
            if (DebugProcess != null)
            {
                using (StreamWriter sw = new StreamWriter(Program.ApplicationPath + "\\kill"))
                    sw.WriteLine("1");

                while(!DebugProcess.HasExited)
                    Thread.Sleep(100);

                DebugProcess = null;
            }
        }

        private void Editor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            var fctb = sender as FastColoredTextBox;

            FirstOpen = false;
            if (!Text.EndsWith("*"))
                Text = Text + "*";

            e.ChangedRange.ClearStyle(Theme.StyleKeywords, Theme.StyleComments, Theme.StyleStrings, Theme.StyleNumbers);

            e.ChangedRange.SetStyle(Theme.StyleKeywords, @"\b(program|process|const|global|local|private|begin|end|if|else|end|loop|frame|while|repeat|for|from|until|break|return|type|offset)\b", RegexOptions.IgnoreCase);

            e.ChangedRange.SetStyle(Theme.StyleStrings, @"""""|@""""|''|@"".*?""|(?<!@)(?<range>"".*?[^\\]"")|'.*?[^\\]'");
            e.ChangedRange.SetStyle(Theme.StyleNumbers, @"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b");

            // TODO: Fix regex, strings don't get colored as comments
            fctb.Range.SetStyle(Theme.StyleComments, @"//.*$", RegexOptions.Multiline);
            fctb.Range.SetStyle(Theme.StyleComments, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
        }

        private void OpenFile(String filename)
        {
            Text = "GamePower IDE - " + new FileInfo(filename).Name;
            originalText = File.ReadAllText(filename);
            FirstOpen = true;
            Filename = filename;

            NewFCTB();
            Editor.OpenFile(filename);
            Editor.Focus();
            Reload();
        }

        private void tsNew_Click(object sender, EventArgs e)
        {
            NewFCTB();
            Filename = String.Empty;
            Reload();
        }

        private void tsOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "*.PRG",
                Filter = "DIV2/GamePower PRG file|*.prg",
                Multiselect = false,
                ShowReadOnly = false,
                Title = "Abrir programa"
            };
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenFile(dlg.FileName);
            Reload();
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(Filename))
            {
                SaveFileDialog dlg = new SaveFileDialog()
                {
                    AddExtension = true,
                    AutoUpgradeEnabled = true,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    DefaultExt = "*.prg",
                    FileName = "Game.prg",
                    Filter = "DIV2/GamePower PRG file|*.prg",
                    OverwritePrompt = true,
                    Title = "Guardar programa"
                };
                if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;
                Filename = dlg.FileName;
            }
            Editor.SaveToFile(Filename, Encoding.Default);
            originalText = Editor.Text;
            Text = "GamePower IDE - " + new FileInfo(Filename).Name;
            Reload();
        }

        private void Editor_CustomAction(object sender, CustomActionEventArgs e)
        {
            switch (e.Action)
            {
                // New
                case FCTBAction.CustomAction1:
                    tsNew_Click(tsNew, null);
                    break;
                // Open
                case FCTBAction.CustomAction2:
                    tsOpen_Click(tsOpen, null);
                    break;
                // Save
                case FCTBAction.CustomAction3:
                    tsSave_Click(tsSave, null);
                    break;
                // Debug
                case FCTBAction.CustomAction4:
                    tsDebug_Click(tsDebug, null);
                    break;
                // Run
                case FCTBAction.CustomAction5:
                    tsRun_Click(tsRun, null);
                    break;
            }
        }

        private void tsSettings_Click(object sender, EventArgs e)
        {
            Process p = Process.Start(ApplicationPath + "Config.ini");
            p.WaitForExit();
            Reload();
        }

        private void tsCompile_Click(object sender, EventArgs e)
        {
            tsSave_Click(sender, e);
            frmTask t = new frmTask(this) { Task = frmTask.ETask.Compile };
            t.ShowDialog(this);
            t.Close();
            Reload();
        }

        private void tsRun_Click(object sender, EventArgs e)
        {
            tsSave_Click(sender, e);
            frmTask t = new frmTask(this) { Task = frmTask.ETask.Run };
            t.ShowDialog(this);
            t.Close();
            Reload();
        }

        private void tsDebug_Click(object sender, EventArgs e)
        {
            tsSave_Click(sender, e);
            frmTask t = new frmTask(this) { Task = frmTask.ETask.Debug };
            t.ShowDialog(this);
            t.Close();
            Reload();
        }

        private void tsPack_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog()
            {
                Description = "Choose a folder where to copy the game files and runtime",
                RootFolder = Environment.SpecialFolder.Desktop,
                ShowNewFolderButton = true
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                KillDebugProcess();
                tsSave_Click(sender, e);
                frmTask t = new frmTask(this) { Task = frmTask.ETask.Pack };
                t.args = dlg.SelectedPath;
                t.ShowDialog(this);
                t.Close();
                Reload();
            }
        }
    }
}
