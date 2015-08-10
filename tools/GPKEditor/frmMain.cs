using LibGP.Lite;
using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GPKEditor
{
    public partial class frmMain : Form
    {
        public String FilenameToOpen = String.Empty;

        GPK Gpk = null;
        private String Filename = String.Empty;

        public frmMain()
        {
            InitializeComponent();
            Icon = Properties.Resources.icnApp;
            lvContent.DragEnter += lvContent_DragEnter;
            lvContent.DragDrop += lvContent_DragDrop;
            lvContent.ItemDrag += lvContent_ItemDrag;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(FilenameToOpen))
                GPKOpen(FilenameToOpen);
            UIRefresh();
        }

        #region Folder to App
        void lvContent_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if(files != null && files.Length > 0)
            {
                if (
                    files[0].ToLowerInvariant().EndsWith("zip") ||
                    files[0].ToLowerInvariant().EndsWith("fpg") ||
                    files[0].ToLowerInvariant().EndsWith("fnt")
                )
                {
                    try
                    {
                        GPKOpen(files[0]);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Not a valid GPK file");
                    }
                }
                else
                {
                    if (Gpk != null)
                    {
                        foreach (String f in files)
                            if (f.ToLowerInvariant().EndsWith("png"))
                                GPKAdd(f);
                        FPGRefresh();
                    }
                }
            }
        }

        void lvContent_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }
        #endregion
        #region App to Folder
        private bool DropReady;
        private List<FileSystemWatcher> lFSW;
        private String DropDir;

        void lvContent_ItemDrag(object sender, ItemDragEventArgs e)
        {
            String sPath = System.IO.Path.GetTempFileName() + ".DROPTARGET";
            using (StreamWriter sw = new StreamWriter(sPath))
                sw.WriteLine("Placeholder");

            StringCollection s = new StringCollection();
            s.Add(sPath);

            DropReady = false;
            lFSW = new List<FileSystemWatcher>();

            foreach (DriveInfo d in DriveInfo.GetDrives())
            {
                if (d.DriveType == DriveType.CDRom ||
                    d.DriveType == DriveType.NoRootDirectory ||
                    d.DriveType == DriveType.Ram ||
                    d.DriveType == DriveType.Unknown ||
                    !d.IsReady)
                        continue;
                FileSystemWatcher Fsw = new FileSystemWatcher();
                Fsw.Path = d.RootDirectory.FullName;
                Fsw.IncludeSubdirectories = true;
                Fsw.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                Fsw.Filter = "*.DROPTARGET";
                Fsw.Created += Fsw_Created;
                Fsw.EnableRaisingEvents = true;
                lFSW.Add(Fsw);
            }

            DataObject obj = new DataObject();
            obj.SetFileDropList(s);
            lvContent.DoDragDrop(obj, DragDropEffects.All);

            while (!DropReady)
                Thread.Sleep(1);

            List<String> SelectedListItems = new List<String>();
            foreach (ListViewItem i in lvContent.SelectedItems)
                ((GPBitmap)i.Tag).Save(DropDir + ((GPBitmap)i.Tag).Filename);

            SHChangeNotify(HChangeNotifyEventID.SHCNE_ALLEVENTS, HChangeNotifyFlags.SHCNF_DWORD, IntPtr.Zero, IntPtr.Zero);
            SHChangeNotify(HChangeNotifyEventID.SHCNE_ASSOCCHANGED, HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
            SHChangeNotify(HChangeNotifyEventID.SHCNE_UPDATEDIR, HChangeNotifyFlags.SHCNF_IDLIST, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll")]
        static extern void SHChangeNotify(HChangeNotifyEventID wEventId,
                                           HChangeNotifyFlags uFlags,
                                           IntPtr dwItem1,
                                           IntPtr dwItem2);

        [Flags]
        enum HChangeNotifyEventID
        {
            SHCNE_ALLEVENTS = 0x7FFFFFFF,
            SHCNE_ASSOCCHANGED = 0x08000000,
            SHCNE_UPDATEDIR = 0x00001000,
        }

        [Flags]
        public enum HChangeNotifyFlags
        {
            SHCNF_DWORD = 0x0003,
            SHCNF_IDLIST = 0x0000,
        }

        void Fsw_Created(object sender, FileSystemEventArgs e)
        {
            DropDir = String.Empty;
            foreach (FileSystemWatcher fsw in lFSW)
                fsw.Dispose();
            lFSW = new List<FileSystemWatcher>();

            DirectoryInfo d = new FileInfo(e.FullPath).Directory;
            try
            {
                File.Delete(e.FullPath);
            }
            catch
            {
                GC.Collect();
                Thread.Sleep(500);
                File.Delete(e.FullPath);
            }
            d.Refresh();

            if (DropDir == String.Empty)
            {
                DropDir = new FileInfo(e.FullPath).Directory.FullName;
                if (!DropDir.EndsWith("\\"))
                    DropDir += "\\";
            }

            DropReady = true;
        }
        #endregion

        #region Utils
        public void CenterDrawImage(Bitmap target, Color background, Bitmap centerme)
        {
            Graphics g = Graphics.FromImage(target);
            g.Clear(background);
            int x = (target.Width - centerme.Width) / 2;
            int y = (target.Height - centerme.Height) / 2;
            g.DrawImage(centerme, x, y);
            g.Dispose();
        }
        #endregion
        #region GUI events
        // http://stackoverflow.com/questions/1101149/displaying-thumbnail-icons-128x128-pixels-or-larger-in-a-grid-in-listview
        // Test penultima opcion
        private void tsOpen_Click(object sender, EventArgs e)
        {
            lvContent.View = View.LargeIcon;
            OpenFileDialog dlg = new OpenFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "zip",
                Filter = "GPK File|*.zip",
                Multiselect = false,
                ShowReadOnly = false,
                SupportMultiDottedExtensions = false,
                Title = "Open GPK",
                ValidateNames = true
            };
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
                GPKOpen(dlg.FileName);
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "PNG files (*.png)|*.png",
                Multiselect = true,
                ShowReadOnly = false,
                SupportMultiDottedExtensions = true,
                Title = "Add images",
                ValidateNames = true
            };
            dlg.ShowDialog();
            if (dlg.FileNames.Length > 0)
            {
                foreach (String filename in dlg.FileNames)
                    GPKAdd(filename);
                FPGRefresh();
            }
        }

        void tsDelete_Click(object sender, EventArgs e)
        {
            if (lvContent.SelectedIndices.Count > 0)
            {
                List<ListViewItem> lvi = new List<ListViewItem>();
                foreach (ListViewItem i in lvContent.SelectedItems)
                    lvi.Add(i);
                foreach (ListViewItem i in lvContent.SelectedItems)
                    lvContent.Items.Remove(i);
                Gpk.Open();
                foreach (ListViewItem i in lvi)
                    File.Delete(Gpk.TempFolder + Gpk.Bitmaps.Single(x => x.Code == int.Parse(i.ImageKey)).Filename);
                Gpk.Close();
                FPGRefresh();
            }
        }

        private void tsNew_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog()
            {
                AddExtension = true,
                AutoUpgradeEnabled = true,
                CheckFileExists = false,
                CheckPathExists = true,
                OverwritePrompt = true,
                DefaultExt = "*.zip",
                Filter = "GPK Package|*.zip",
                SupportMultiDottedExtensions = false,
                Title = "New GPK",
                ValidateNames = true
            };
            dlg.ShowDialog();
            if (dlg.FileName.Length > 0)
            {
                Gpk = GPK.Create(GPK.GPKType.Graphics, dlg.FileName);
                Filename = dlg.FileName;
                FPGRefresh();
            }
        }

        private void lvContent_DoubleClick(object sender, EventArgs e)
        {
            if (lvContent.SelectedIndices.Count > 0)
            {
                frmDetails f = new frmDetails();
                f.Gpk = Gpk;
                f.Map = Gpk.Bitmaps[lvContent.SelectedIndices[0]];
                f.Filename = Filename;
                DialogResult r = f.ShowDialog(this);
                if (r == DialogResult.OK)
                    FPGRefresh();
            }
        }
        #endregion
        #region GUI functions
        public void FPGRefresh()
        {
            lvContent.Items.Clear();
            lvContent.LargeImageList = new ImageList()
            {
                ImageSize = new Size(50, 50),
                ColorDepth = ColorDepth.Depth32Bit
            };
            Gpk = GPK.Load(Filename);
            foreach (GPBitmap bmp in Gpk.Bitmaps)
            {
                lvContent.LargeImageList.Images.Add(bmp.Code.ToString(), bmp.Bitmap);
                lvContent.Items.Add(new ListViewItem() { Text = bmp.Code.ToString().PadLeft(3, '0'), ImageKey = bmp.Code.ToString(), Tag = bmp });
            }
            Gpk.Save(Filename);
            WinAPI.ListViewItem_SetSpacing(lvContent, 55, 70);
            UIRefresh();
        }

        public void UIRefresh() {
            statusStrip1.Items.Clear();
            statusStrip1.Items.Add(lvContent.Items.Count + ((lvContent.Items.Count > 1 || lvContent.Items.Count == 0) ? " images" : " image"));
            Text = "GPK Editor" + (String.IsNullOrEmpty(Filename) ? "" : (" - " + Filename));
            tsAdd.Enabled = !String.IsNullOrEmpty(Filename);
            tsDelete.Enabled = !String.IsNullOrEmpty(Filename);
        }
        #endregion
        #region App Actions
        private void GPKOpen(String filename)
        {
            Gpk = GPK.Load(filename);
            if (Gpk.PackageType == GPK.GPKType.Font)
                MessageBox.Show("Warning! The GPK editor is not suited for editing Font files yet. If you make changes here, the font might end up corrupted in your game. This is to be fixed soon, but it's not ready yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Filename = filename;
            FPGRefresh();
        }
        private void GPKAdd(String filename)
        {
            if (!tsAdd.Enabled)
                return;

            FileInfo fi = new FileInfo(filename);

            Image dImage = Image.FromFile(fi.FullName);
            Image mImage = new Bitmap(dImage);
            dImage.Dispose();

            Gpk = GPK.Load(Filename);
            Gpk.Open();

            // TODO: Prevent > 999
            GPBitmap bmp = new GPBitmap()
            {
                Code = Gpk.Bitmaps.Count > 0 ? Gpk.Bitmaps.Max(x => x.Code) + 1 : 1,
                Description = fi.Name,
                Filename = fi.Name,
                Size = mImage.Size,
                Center = new Point(mImage.Size.Width / 2, mImage.Size.Height / 2),
                ControlPoints = new Dictionary<int, Point>(),
                Bitmap = mImage
            };

            if(Gpk.PackageType == GPK.GPKType.Graphics)
                bmp.Save(Gpk.TempFolder + bmp.Code.ToString().PadLeft(3, '0') + "_" + bmp.Filename);
            else
                bmp.Save(Gpk.TempFolder + bmp.Code.ToString().PadLeft(3, '0'));

            Gpk.Close();
        }
        #endregion

        private void lvContent_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
                tsDelete_Click(lvContent, null);
        }
    }
}
