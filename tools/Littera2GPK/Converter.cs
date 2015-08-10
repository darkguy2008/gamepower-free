using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Littera2GPK
{
    public static class Converter
    {
        public static void BatchConvert(String[] files)
        {
            foreach (String f in files)
            {
                FileInfo fi = new FileInfo(f);
                Convert(f, fi.DirectoryName + "\\" + "converted_" + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".zip");
            }
        }

        public static void Convert(String src, String dst)
        {
            FileInfo fi = new FileInfo(src);
            FileInfo fiDst = new FileInfo(dst);

            List<String> Lfiles = new List<String>();
            String tmpFolder = Path.GetTempPath() + fi.Name + DateTime.Now.Ticks.ToString() + "\\";
            String outFolder = tmpFolder + "output\\";
            if (!Directory.Exists(outFolder))
                Directory.CreateDirectory(outFolder);

            ZIP z = new ZIP();
            try
            {
                z = new ZIP(fi.FullName);
                z.Open();

                font f = Xml.Deserialize<font>("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + z.ExtractFileToString("font.fnt"));

                foreach (fontPage p in f.pages)
                    z.ExtractFile(p.file, tmpFolder + p.file);

                foreach (fontCharsChar c in f.chars.@char)
                {
                    if (c.width == 0 && c.height == 0)
                        continue;

                    String outFile = outFolder + c.id.ToString().PadLeft(3, '0') + ".png";
                    Rectangle rCrop = new Rectangle(c.x, c.y, c.width, c.height);
                    using (Bitmap bSrc = Image.FromFile(tmpFolder + f.pages[c.page].file) as Bitmap)
                    {
                        using (Bitmap bDst = new Bitmap(c.width, c.height))
                        {
                            using (Graphics g = Graphics.FromImage(bDst))
                                g.DrawImage(bSrc, new Rectangle(0, 0, bDst.Width, bDst.Height), rCrop, GraphicsUnit.Pixel);
                            bDst.Save(outFile);
                        }
                    }
                    PNG p = new PNG(outFile);
                    p.SetTextChunk("Description", c.id.ToString());
                    p.SetTextChunk("OffsetPos", "0,0");
                    p.SetTextChunk("OffsetSize", c.xoffset + "," + c.yoffset);
                    p.Save(outFile);
                    Lfiles.Add(outFile);
                }
                z.Close();
            }
            catch (Exception)
            {
                z.Close();
            }

            using (StreamWriter sw = new StreamWriter(outFolder + "\\Type.txt"))
                sw.WriteLine("FNT");
            Lfiles.Add(outFolder + "\\Type.txt");

            ZIP fnt = new ZIP(dst);
            fnt.Files.AddRange(Lfiles);
            fnt.Save();
            fnt.Close();

            if (Directory.Exists(tmpFolder))
                Directory.Delete(tmpFolder, true);
        }

    }
}
