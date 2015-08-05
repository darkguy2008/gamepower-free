using LibGP.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GPIDE
{
    public partial class frmTask : Form
    {
        public enum ETask
        {
            Compile = 1,
            Debug,
            Run,
            Pack
        };

        public ETask Task;
        public frmMain frm;
        public object args;
        public bool CloseOnFinish { get; set; }

        public frmTask(frmMain parent)
        {
            CloseOnFinish = true;
            frm = parent;
            InitializeComponent();
        }

        private bool Compile()
        {
            frm.Reload();
            String gpc = Path.GetFullPath(frm.Settings["Paths"]["GPC"]);
            String args = String.Format("-i \"{0}\" -o \"{1}\"", frm.Filename, Path.ChangeExtension(frm.Filename, ".js"));
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.Arguments = args;
            pi.FileName = gpc;
            pi.UseShellExecute = false;
            pi.CreateNoWindow = true;
            Process p = new Process() { StartInfo = pi };
            p.Start();
            p.WaitForExit();            
            return true;
        }

        private void GenerateOutput()
        {
            frm.NewDebugSession();
            Compile();
            if (File.Exists(frm.DebugTempPath + "\\game.js"))
                File.Delete(frm.DebugTempPath + "\\game.js");
            File.Move(Path.ChangeExtension(frm.Filename, ".js"), frm.DebugTempPath + "\\game.js");
            if(Directory.Exists(frm.DebugTempPath + "\\pack"))
                Directory.Delete(frm.DebugTempPath + "\\pack", true);

            FileInfo prgPath = new FileInfo(frm.Filename);
            String prgDirPath = prgPath.DirectoryName;
            String[] lines = File.ReadAllLines(frm.DebugTempPath + "\\game.js");
            String resFile = String.Empty;
            bool bCopy = false;
            foreach (String line in lines)
            {
                bCopy = false;
                if (line.StartsWith("_gp.res.fnt[") && !line.Contains("default.fnt"))
                {
                    resFile = line.Substring("_gp.res.fnt['".Length);
                    resFile = resFile.Substring(0, resFile.IndexOf("'")).Replace("/", "\\");
                    bCopy = true;
                }
                else if (line.StartsWith("_gp.res.fpg["))
                {
                    resFile = line.Substring("_gp.res.fpg['".Length);
                    resFile = resFile.Substring(0, resFile.IndexOf("'")).Replace("/", "\\");
                    bCopy = true;
                }
                else if (line.StartsWith("_gp.res.song["))
                {
                    resFile = line.Substring("_gp.res.song['".Length);
                    resFile = resFile.Substring(0, resFile.IndexOf("'")).Replace("/", "\\");
                    bCopy = true;
                }
                else if (line.StartsWith("_gp.res.audio["))
                {
                    resFile = line.Substring("_gp.res.audio['".Length);
                    resFile = resFile.Substring(0, resFile.IndexOf("'")).Replace("/", "\\");
                    bCopy = true;
                }

                if (bCopy)
                {
                    FileInfo dstInfo = new FileInfo(frm.DebugTempPath + "\\" + resFile);
                    if (!Directory.Exists(dstInfo.DirectoryName))
                        Directory.CreateDirectory(dstInfo.DirectoryName);
                    File.Copy(prgDirPath + "\\" + resFile, dstInfo.FullName);
                }
            }
            frm.RefreshDebugProcess();
        }

        private void Debug()
        {
            frm.Reload();
            GenerateOutput();
            frm.RefreshDebugProcess();
            Process.Start("http://localhost:" + frm.Settings["Debug"]["Port"]);
        }

        private void Run()
        {
            frm.Reload();
            GenerateOutput();
            FolderManager.CopyFolderContents(Path.GetFullPath(frm.Settings["Paths"]["Runtime"]) + "\\pack", frm.DebugTempPath);
            frm.RefreshDebugProcess();
            Process.Start("http://localhost:" + frm.Settings["Debug"]["Port"]);
        }

        private void Pack(String dstFolder)
        {
            frm.Reload();
            GenerateOutput();
            if (!Directory.Exists(dstFolder))
                Directory.CreateDirectory(dstFolder);
            FolderManager.CopyFolderContents(frm.DebugTempPath, dstFolder);
            FolderManager.CopyFolderContents(Path.GetFullPath(frm.Settings["Paths"]["Runtime"]) + "\\pack", dstFolder);
            MessageBox.Show("Packing finished. Copy the output files into a web server and play away!");
            Process.Start(dstFolder);
        }

        private void bgWork_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                switch (Task)
                {
                    case ETask.Compile:
                        bgWork.ReportProgress(0, "Compiling...");
                        Compile();
                        bgWork.ReportProgress(100, "Compiled.");
                        File.Delete(Path.ChangeExtension(frm.Filename, ".js"));
                        break;
                    case ETask.Debug:
                        bgWork.ReportProgress(0, "Compiling...");
                        if (Compile())
                        {
                            bgWork.ReportProgress(50, "Compiled.");
                            bgWork.ReportProgress(100, "Debugging...");
                            Debug();
                        }
                        break;
                    case ETask.Run:
                        bgWork.ReportProgress(0, "Compiling...");
                        if (Compile())
                        {
                            bgWork.ReportProgress(50, "Compiled.");
                            bgWork.ReportProgress(100, "Running...");
                            Run();
                        }
                        break;
                    case ETask.Pack:
                        bgWork.ReportProgress(0, "Compiling...");
                        if (Compile())
                        {
                            bgWork.ReportProgress(50, "Compiled.");
                            bgWork.ReportProgress(50, "Packing...");
                            Pack(args.ToString());
                            bgWork.ReportProgress(100, "Packed");
                        }
                        break;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }
        }

        #region GUI Methods
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Hide();
        }

        private void bgWork_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Log.Items.Add(e.ProgressPercentage + ", " + e.UserState);
            if (e.ProgressPercentage == 100)
                if (CloseOnFinish)
                    Close();
                else
                    btnOk.Enabled = true;
        }

        private void frmTask_Load(object sender, EventArgs e)
        {
            bgWork.RunWorkerAsync();
        }
        #endregion
    }
}
