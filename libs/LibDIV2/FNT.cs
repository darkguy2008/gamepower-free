using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using LibGP.Utils;

namespace LibDIV2
{
    public class FNT
    {
        public PAL Pal = new PAL();

        public class FNTStruct
        {
            public int Width { get; set; }
            public int Height { get; set; }
            public byte OffWidth { get; set; }
            public byte OffHeight { get; set; }
            public byte OffX { get; set; }
            public byte OffY { get; set; }
            public int FileOffset { get; set; }
            public MAP Map;
        }
        public List<FNTStruct> Characters = new List<FNTStruct>();

        public static FNT Open(String filename)
        {
            FNT rv = new FNT();

            using (BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                byte[] sig = br.ReadBytes(3);
                byte[] sig2 = br.ReadBytes(3);
                if (sig2.Compare(new byte[] { 0x1A, 0x0D, 0x0A }))
                {
                    String sg = Encoding.ASCII.GetString(sig);
                    if (sg == "fnt")
                    {
                        br.BaseStream.Seek(8, SeekOrigin.Begin);

                        // TODO: Check if needs fix like FPG.cs where a color could be
                        // index #255 and it bugs out, if it does, add +3 to pal and
                        // substract -3 to padding
                        byte[] pal = br.ReadBytes(765);
                        byte[] padding = br.ReadBytes(567 + 16);

                        // #############################################
                        // Read color palette
                        // #############################################
                        int ii = 1;
                        rv.Pal.Palette[0] = Color.Transparent;
                        for (int i = 3; i < pal.Length; i += 3)
                        {
                            rv.Pal.Palette[ii] = Color.FromArgb(255,
                                pal[i] * 255 / 64,
                                pal[i + 1] * 255 / 64,
                                pal[i + 2] * 255 / 64
                            );
                            ii++;
                        }

                        for (int i = 0; i < 256; i++)
                        {
                            FNTStruct f = new FNTStruct()
                            {
                                Width = br.ReadInt32(),
                                Height = br.ReadInt32(),
                                OffHeight = br.ReadByte(),
                                OffWidth = br.ReadByte(),
                                OffX = br.ReadByte(),
                                OffY = br.ReadByte(),
                                FileOffset = br.ReadInt32(),
                            };
                            rv.Characters.Add(f);
                        }

                        for (int i = 0; i < rv.Characters.Count; i++)
                        {
                            FNTStruct f = rv.Characters[i];
                            if (f.FileOffset > 0)
                                f.Map = new MAP().ReadFromFNT(br, i, f.FileOffset, f.Width, f.Height);
                        }
                    }
                }
            }

            return rv;
        }
    }
}
