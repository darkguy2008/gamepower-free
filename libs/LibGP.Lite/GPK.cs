using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace LibGP.Lite
{
    public class GPK
    {
        public enum GPKType
        {
            Graphics = 1,
            Font = 2
        }

        public GPKType PackageType;
        private String _filename;
        public String TempFolder;
        public List<GPBitmap> Bitmaps = new List<GPBitmap>();

        public GPK(String filename) { _filename = filename; }

        public static GPK Load(String filename)
        {
            GPK rv = new GPK(filename);

            ZIP z = new ZIP(filename);
            z.Open();

            String type = z.ExtractFileToString("Type.txt");
            switch (type.ToUpperInvariant().Trim())
            {
                case "GFX":
                    rv.PackageType = GPKType.Graphics;
                    break;
                case "FNT":
                    rv.PackageType = GPKType.Font;
                    break;
            }

            foreach(ZipStorer.ZipFileEntry zf in z.ListFiles().Where(x => x.FilenameInZip != "Type.txt"))
            {
                MemoryStream ms = new MemoryStream();
                if(z.ExtractFileToStream(zf, ref ms))
                {
                    int code = -1;
                    String name = String.Empty;
                    if (zf.FilenameInZip.Contains("_"))
                    {
                        code = int.Parse(zf.FilenameInZip.Substring(0, zf.FilenameInZip.IndexOf('_')));
                        name = zf.FilenameInZip.Substring(zf.FilenameInZip.IndexOf('_') + 1);
                    }
                    else
                    {
                        code = int.Parse(zf.FilenameInZip.Substring(0, zf.FilenameInZip.IndexOf(".")));
                        name = zf.FilenameInZip;
                    }
                    GPBitmap bmp = new GPBitmap()
                    {
                        Code = code,
                        Name = name,
                        Filename = zf.FilenameInZip,
                        Bitmap = Image.FromStream(ms)
                    };
                    bmp.Name = bmp.Name.Substring(0, bmp.Name.LastIndexOf('.'));
                    bmp.Size = bmp.Bitmap.Size;
                    ms.Position = 0;
                    PNG p = new PNG(ms);
                    String tmp = String.Empty;
                    foreach (PNG.PNGChunk c in p.Chunks.Where(x => x.Type == "tEXt"))
                    {
                        switch (c.ToString().Split('=')[0])
                        { 
                            case "Description":
                                bmp.Description = c.ToString().Substring(c.ToString().IndexOf('=') + 1);
                                break;
                            case "ControlPoints":
                                foreach (String cp in c.ToString().Substring(c.ToString().IndexOf('=') + 1).Split('|'))
                                    bmp.ControlPoints.Add(int.Parse(cp.Split('=')[0]), new Point(int.Parse(cp.Split('=')[1].Split(',')[0]), int.Parse(cp.Split('=')[1].Split(',')[1])));
                                break;
                            case "OffsetPos":
                                tmp = c.ToString().Substring(c.ToString().IndexOf('=') + 1);
                                bmp.OffsetPosition = new Point(int.Parse(tmp.Split(',')[0]), int.Parse(tmp.Split(',')[1]));
                                break;
                            case "OffsetSize":
                                tmp = c.ToString().Substring(c.ToString().IndexOf('=') + 1);
                                bmp.OffsetSize = new Size(int.Parse(tmp.Split(',')[0]), int.Parse(tmp.Split(',')[1]));
                                break;
                        }
                    }
                    if (bmp.ControlPoints.Count == 0)
                        bmp.ControlPoints.Add(0, new Point(bmp.Bitmap.Size.Width / 2, bmp.Bitmap.Size.Height / 2));
                    bmp.Center = bmp.ControlPoints[0];
                    ms.Close(); ms.Dispose();
                    rv.Bitmaps.Add(bmp);
                }
            }

            z.Close();
            rv._filename = filename;
            return rv;
        }

        public static GPK CreateFromFolder(GPKType gpkType, String srcFolder, String outZipFile)
        {
            ZIP zipfile = new ZIP(outZipFile);
            using (StreamWriter sw = new StreamWriter(srcFolder + "\\Type.txt"))
                sw.WriteLine(gpkType == GPKType.Graphics ? "GFX" : "FNT");
            zipfile.Files.AddRange(Directory.GetFiles(srcFolder));
            zipfile.Save();
            zipfile.Close();
            File.Delete(srcFolder + "\\Type.txt");
            return GPK.Load(outZipFile);
        }

        public void Save(string Filename)
        {
            Open();
            _filename = Filename;
            Close();
        }

        public void Open()
        {
            if (!String.IsNullOrEmpty(TempFolder))
                throw new Exception("GPK is opened already");

            String tmpFilename = Path.GetTempFileName();
            TempFolder = tmpFilename + "_dir\\";
            if (!Directory.Exists(TempFolder))
                Directory.CreateDirectory(TempFolder);
            File.Delete(tmpFilename);

            foreach (GPBitmap b in Bitmaps)
                b.Save(TempFolder + b.Filename);
        }

        public void Close()
        {
            ZIP zipfile = new ZIP(_filename);
            using (StreamWriter sw = new StreamWriter(TempFolder + "\\Type.txt"))
                sw.WriteLine(PackageType == GPKType.Graphics ? "GFX" : "FNT");
            zipfile.Files.AddRange(Directory.GetFiles(TempFolder));
            zipfile.Save();
            zipfile.Close();
            Directory.Delete(TempFolder, true);
            TempFolder = null;
        }

        public static GPK Create(GPKType type, string filename)
        {
            ZIP zipfile = new ZIP(filename);

            String tmpFilename = Path.GetTempFileName();
            String tmpFolder = tmpFilename + "_dir\\";
            if (!Directory.Exists(tmpFolder))
                Directory.CreateDirectory(tmpFolder);
            File.Delete(tmpFilename);

            using (StreamWriter sw = new StreamWriter(tmpFolder + "\\Type.txt"))
                sw.WriteLine(type == GPKType.Graphics ? "GFX" : "FNT");
            zipfile.Files.AddRange(Directory.GetFiles(tmpFolder));
            zipfile.Save();
            zipfile.Close();
            Directory.Delete(tmpFolder, true);

            return GPK.Load(filename);
        }
    }
}
