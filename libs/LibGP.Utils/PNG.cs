// Simple PNG chunk reader/writer library
// Copyright © 2014 DARKGuy / Alemar
// 2015-02-01

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibGP.Utils
{

    public class PNG {

        private byte[] header;
        private static uint[] crcTable;
        public List<PNGChunk> Chunks = new List<PNGChunk>();

        public class PNGChunk
        {
            public int Length { get { return Data.Length; } }
            public String Type { get; set; }
            public byte[] Data { get; set; }
            public byte[] RawCRC { get; set; }
            public byte[] Crc32
            {
                get
                {
                    byte[] crcData = Encoding.ASCII.GetBytes(Type).Concat(Data).ToArray();
                    return BitConverter.GetBytes(Crc32(crcData, 0, crcData.Length, 0)).Reverse().ToArray();
                }
            }

            public byte[] ToBytes()
            {
                byte[] rv;
                using(MemoryStream ms = new MemoryStream())
                {
                    using (BinaryWriter bw = new BinaryWriter(ms))
                    {
                        byte[] ln = BitConverter.GetBytes(Length);
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(ln);
                        bw.Write(ln);
                        bw.Write(Encoding.ASCII.GetBytes(Type));
                        bw.Write(Data);
                        bw.Write(Crc32);
                    }
                    rv = ms.ToArray();
                }
                return rv;
            }

            public override String ToString()
            {
                if (Type == "tEXt")
                {
                    int sep = Data.IndexOf(new byte[] { 0x00 }).Single();
                    String key = Encoding.ASCII.GetString(Data, 0, sep);
                    String val = Encoding.ASCII.GetString(Data, sep + 1, Data.Length - sep - 1);
                    return key + "=" + val;
                }
                else
                    return base.ToString();
            }
        }

        private static uint Crc32(byte[] stream, int offset, int length, uint crc)
        {
            uint c;
            if (crcTable == null)
            {
                crcTable = new uint[256];
                for (uint n = 0; n <= 255; n++)
                {
                    c = n;
                    for (var k = 0; k <= 7; k++)
                    {
                        if ((c & 1) == 1)
                            c = 0xEDB88320 ^ ((c >> 1) & 0x7FFFFFFF);
                        else
                            c = ((c >> 1) & 0x7FFFFFFF);
                    }
                    crcTable[n] = c;
                }
            }
            c = crc ^ 0xffffffff;
            var endOffset = offset + length;
            for (var i = offset; i < endOffset; i++)
            {
                c = crcTable[(c ^ stream[i]) & 255] ^ ((c >> 8) & 0xFFFFFF);
            }
            return c ^ 0xffffffff;
        }

        private void ReadChunk(BinaryReader b)
        {
            PNGChunk c = new PNGChunk();
            byte[] blen = b.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(blen);
            int len = BitConverter.ToInt32(blen, 0);
            c.Type = Encoding.ASCII.GetString(b.ReadBytes(4));
            c.Data = b.ReadBytes((int)len);
            c.RawCRC = b.ReadBytes(4);
            Chunks.Add(c);
        }

        public void SetTextChunk(String name, String value)
        {
            PNGChunk c = Chunks.Where(x => x.Type == "tEXt" && x.ToString().StartsWith(name + "=")).SingleOrDefault();
            if (c == null)
                Chunks.Insert(Chunks.Count - 1, new PNGChunk() { Type = "tEXt", Data = Encoding.ASCII.GetBytes(name + "\0" + value) });
            else
                c.Data = Encoding.ASCII.GetBytes(name + "\0" + value);
        }

        public PNG() { }
        public PNG(String filename) 
        {
            using (BinaryReader br = new BinaryReader(new FileStream(filename, FileMode.Open)))
            {
                header = br.ReadBytes(8);
                while (br.BaseStream.Position < br.BaseStream.Length)
                    ReadChunk(br);
            }
        }
        public PNG(MemoryStream ms)
        {
            using (BinaryReader br = new BinaryReader(ms))
            {
                header = br.ReadBytes(8);
                while (br.BaseStream.Position < br.BaseStream.Length)
                    ReadChunk(br);
            }
        }

        public void Save(String filename)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filename, FileMode.Create)))
            {
                bw.Write(header);
                foreach (PNGChunk c in Chunks)
                    bw.Write(c.ToBytes());
            }
        }
    }

}
