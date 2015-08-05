using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using LibGP.Utils;

namespace LibDIV2
{
    public class FPG
    {
        public PAL Pal = new PAL();
        public List<MAP> Maps = new List<MAP>();

        public static FPG Open(String filename)
        {
            FPG rv = new FPG();
            using (BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                byte[] sig = br.ReadBytes(3);
                byte[] sig2 = br.ReadBytes(3);
                if (sig2.Compare(new byte[] { 0x1A, 0x0D, 0x0A }))
                {
                    String sg = Encoding.ASCII.GetString(sig);
                    if (sg == "fpg")
                    {
                        br.BaseStream.Seek(8, SeekOrigin.Begin);
                        byte[] pal = br.ReadBytes(768);
                        byte[] padding = br.ReadBytes(576);

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

                        // #############################################
                        // Read maps
                        // #############################################
                        while (br.BaseStream.Position < br.BaseStream.Length)
                            rv.Maps.Add(new MAP().ReadFromFPG(br));
                    }
                }
            }
            return rv;
        }
    }
}
