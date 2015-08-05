using LibDIV2;
using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace FNT2GPK
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
            FNT fnt = FNT.Open(src);
            String outFile = dst;

            List<String> Lfiles = new List<String>();
            String tmpFolder = Path.GetTempPath() + fi.Name + DateTime.Now.Ticks.ToString() + "\\";
            if (!Directory.Exists(tmpFolder))
                Directory.CreateDirectory(tmpFolder);

            foreach (FNT.FNTStruct f in fnt.Characters.Where(x => x.FileOffset > 0 && x.Map.Width > 0 && x.Map.Height > 0))
            {
                String name = f.Map.Code.ToString().PadLeft(3, '0');
                String pngFile = tmpFolder + name + ".png";
                f.Map.ExportToPNG(fnt.Pal, pngFile);
                PNG p = new PNG(pngFile);
                p.SetTextChunk("Description", name);
                p.SetTextChunk("OffsetPos", f.OffX + "," + f.OffY);
                p.SetTextChunk("OffsetSize", f.OffWidth + "," + f.OffHeight);
                p.Save(pngFile);
                Lfiles.Add(pngFile);
            }

            using (StreamWriter sw = new StreamWriter(tmpFolder + "\\Type.txt"))
                sw.WriteLine("FNT");
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
