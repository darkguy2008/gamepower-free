using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace LibDIV2
{
    public class MAP
    {
        public int Code { get; set; }
        public int RegSize { get; set; }
        public String RawName { get; set; }
        public String RawFilename { get; set; }
        public String Name { get; set; }
        public String Filename { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Dictionary<int, Point> ControlPoints = new Dictionary<int, Point>();
        public Byte[] Data1D { get; set; }
        public Byte[,] Data2D { get; set; }

        public MAP ReadFromFPG(BinaryReader br)
        {
            Code = br.ReadInt32();
            RegSize = br.ReadInt32();
            RawName = Encoding.ASCII.GetString(br.ReadBytes(32));
            RawFilename = Encoding.ASCII.GetString(br.ReadBytes(12));
            Name = Utils.SanitizeString(RawName);
            Filename = Utils.SanitizeString(RawFilename);
            Width = br.ReadInt32();
            Height = br.ReadInt32();
            int cpCount = br.ReadInt32();
            if (cpCount > 0)
                for (int i = 0; i < cpCount; i++)
                    ControlPoints.Add(i, new Point(br.ReadInt16(), br.ReadInt16()));
            Data1D = br.ReadBytes(Width * Height);
            Data2D = new byte[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Data2D[y, x] = Data1D[(y * Width) + x];
            return this;
        }

        public MAP ReadFromFNT(BinaryReader br, int id, int offset, int width, int height)
        {
            br.BaseStream.Seek(offset, SeekOrigin.Begin);
            Code = id;
            RegSize = 0;
            RawName = id.ToString();
            RawFilename = RawName;
            Name = RawName;
            Filename = RawName;
            Width = width;
            Height = height;
            Data1D = br.ReadBytes(Width * Height);
            Data2D = new byte[Height, Width];
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Data2D[y, x] = Data1D[(y * Width) + x];
            return this;
        }

        public void ExportToMAP(PAL pal, String filename)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create)))
            {
                bw.Write(Encoding.ASCII.GetBytes("map"));
                bw.Write(new byte[] { 0x1A, 0x0D, 0x0A, 0x00, 0x00 });
                bw.Write((short)Width);
                bw.Write((short)Height);
                bw.Write(Code);
                bw.Write(Encoding.ASCII.GetBytes(Name));
                bw.Write(pal.RawData());
                bw.Write(new byte[] { 0x00, 0x00 });
                if (ControlPoints.Count > 0)
                {
                    bw.Write((short)ControlPoints.Count);
                    foreach (KeyValuePair<int, Point> kv in ControlPoints)
                    {
                        bw.Write((short)kv.Value.X);
                        bw.Write((short)kv.Value.Y);
                    }
                }
                bw.Write(Data1D);
            }
        }

        public void ExportToPNG(PAL pal, String filename)
        {
            if(Width > 0 && Height > 0)
                using (Bitmap bmp = new Bitmap(Width, Height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        for (int y = 0; y < Height; y++)
                            for (int x = 0; x < Width; x++)
                                g.FillRectangle(new SolidBrush(pal.Palette[Data2D[y, x]]), x, y, 1, 1);
                    }
                    bmp.Save(filename);
                }
        }
    }
}
