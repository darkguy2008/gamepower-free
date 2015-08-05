using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace LibDIV2
{
    public class PAL
    {
        public Dictionary<int, Color> Palette = new Dictionary<int, Color>();

        public byte[] RawData()
        {
            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(new byte[] { 0x00, 0x00, 0x00 });
                foreach (KeyValuePair<int, Color> kv in Palette.OrderBy(x => x.Key))
                    bw.Write(new byte[] { 
                        Convert.ToByte(kv.Value.R * 64 / 255, CultureInfo.InvariantCulture),
                        Convert.ToByte(kv.Value.G * 64 / 255, CultureInfo.InvariantCulture),
                        Convert.ToByte(kv.Value.B * 64 / 255, CultureInfo.InvariantCulture)
                    });
                bw.Write(Encoding.ASCII.GetBytes("".PadRight(579, '\xFF')));
            }
            return ms.ToArray();
        }

        public void ExportToPAL(String filename)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create)))
            {
                bw.Write(Encoding.ASCII.GetBytes("pal"));
                bw.Write(new byte[] { 0x1A, 0x0D, 0x0A, 0x00, 0x00, 0x00 });
                bw.Write(RawData());
            }
        }
    }
}
