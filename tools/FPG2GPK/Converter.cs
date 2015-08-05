using LibDIV2;
using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FPG2GPK
{
    public static class Converter
    {
        public static void BatchConvert(String[] files)
        {
            foreach (String f in files)
            {
                FileInfo fi = new FileInfo(f);
                Convert(f, fi.DirectoryName + "\\" + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".zip");
            }
        }

        public static void Convert(String src, String dst)
        {
            FileInfo fi = new FileInfo(src);
            FileInfo fiDst = new FileInfo(dst);
            FPG fpg = FPG.Open(src);
            String outFile = dst;

            List<String> Lfiles = new List<String>();
            String tmpFolder = Path.GetTempPath() + fi.Name + DateTime.Now.Ticks.ToString() + "\\";
            if (!Directory.Exists(tmpFolder))
                Directory.CreateDirectory(tmpFolder);

            foreach (MAP m in fpg.Maps)
            {
                String name = m.Code.ToString().PadLeft(3, '0') + "_" + m.Name;
                String pngFile = tmpFolder + name + ".png";
                m.ExportToPNG(fpg.Pal, pngFile);
                PNG p = new PNG(pngFile);
                p.SetTextChunk("Description", m.Name);
                if (m.ControlPoints.Count == 0)
                    m.ControlPoints.Add(0, new Point(m.Width / 2, m.Height / 2));

                String chunk = String.Empty;
                foreach (KeyValuePair<int, Point> kv in m.ControlPoints.OrderBy(x => x.Key))
                    chunk += kv.Key + "=" + kv.Value.X + "," + kv.Value.Y + "|";
                chunk = chunk.Substring(0, chunk.Length - 1);
                p.SetTextChunk("ControlPoints", chunk);

                p.Save(pngFile);
                Lfiles.Add(pngFile);
            }

            using (StreamWriter sw = new StreamWriter(tmpFolder + "\\Type.txt"))
                sw.WriteLine("GFX");
            Lfiles.Add(tmpFolder + "\\Type.txt");

            ZIP zipfile = new ZIP(outFile);
            zipfile.Files.AddRange(Lfiles);
            zipfile.Save();
            zipfile.Close();

            if (Directory.Exists(tmpFolder))
                Directory.Delete(tmpFolder, true);            
        }

    }
}
