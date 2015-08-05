using LibGP.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibGP.Lite
{
    public class GPBitmap
    {
        public int Code { get; set; }
        public String Name { get; set; }
        public String Filename { get; set; }
        public String Description { get; set; }
        public Dictionary<int, Point> ControlPoints { get; set; }
        public Point Center { get; set; }
        public Size Size { get; set; }
        public Point OffsetPosition { get; set; }
        public Size OffsetSize { get; set; }
        public Image Bitmap { get; set; }

        public GPBitmap()
        {
            ControlPoints = new Dictionary<int, Point>();
        }

        public void Save(string filename)
        {
            Bitmap.Save(filename);
            PNG p = new PNG(filename);
            p.SetTextChunk("Description", Name);
            if (ControlPoints.Count == 0)
                ControlPoints.Add(0, new Point(Bitmap.Width / 2, Bitmap.Height / 2));
            String chunk = String.Empty;
            foreach (KeyValuePair<int, Point> kv in ControlPoints.OrderBy(x => x.Key))
                chunk += kv.Key + "=" + kv.Value.X + "," + kv.Value.Y + "|";
            chunk = chunk.Substring(0, chunk.Length - 1);
            p.SetTextChunk("ControlPoints", chunk);

            if(OffsetPosition != null)
                p.SetTextChunk("OffsetPos", OffsetPosition.X + "," + OffsetPosition.Y);
            if(OffsetSize != null)
                p.SetTextChunk("OffsetSize", OffsetSize.Width + "," + OffsetSize.Height);

            p.Save(filename);
        }
    }
}
