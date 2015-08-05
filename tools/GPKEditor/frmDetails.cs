using LibGP.Lite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GPKEditor
{
    public partial class frmDetails : Form
    {
        public GPK Gpk { get; set; }
        public GPBitmap Map { get; set; }
        public String Filename { get; set; }

        public frmDetails()
        {
            InitializeComponent();
            Icon = Properties.Resources.icnApp;
        }

        private void frmDetails_Load(object sender, EventArgs e)
        {
            nmCode.Focus();

            pbImage.BackColor = Color.Black;
            pbImage.BackgroundImage = Map.Bitmap;
            if (Map.Bitmap.Width > pbImage.Width || Map.Bitmap.Height > pbImage.Height)
                pbImage.BackgroundImageLayout = ImageLayout.Stretch;
            else
                pbImage.BackgroundImageLayout = ImageLayout.Center;

            nmCode.Value = Map.Code;
            txName.Text = Map.Name;
            txDescription.Text = Map.Description;
            txW.Text = Map.Bitmap.Width.ToString();
            txH.Text = Map.Bitmap.Height.ToString();
            txCX.Text = Map.Center.X.ToString();
            txCY.Text = Map.Center.Y.ToString();
        }

        private void bOk_Click(object sender, EventArgs e)
        {
            GPBitmap m = Gpk.Bitmaps.Where(x =>
                x.Code != Map.Code &&
                (
                    x.Code == (int)nmCode.Value || 
                    (x.Code == (int)nmCode.Value && x.Name.ToUpperInvariant().Trim() == txName.Text.ToUpperInvariant().Trim())
                )
            ).FirstOrDefault();
            if (m == null)
            {
                String nCode = ((int)nmCode.Value).ToString();
                Gpk.Open();
                File.Delete(Gpk.TempFolder + Map.Filename);

                Map.Code = (int)nmCode.Value;
                if (Map.ControlPoints == null || Map.ControlPoints.Count == 0)
                    Map.ControlPoints = new Dictionary<int, Point>();
                Map.ControlPoints[0] = new Point(int.Parse(txCX.Text), int.Parse(txCY.Text));
                Map.Center = Map.ControlPoints[0];
                Map.Name = txName.Text;
                Map.Filename = nCode.PadLeft(3, '0') + "_" + txName.Text + ".png";
                Map.Description = txDescription.Text;
                Map.Save(Gpk.TempFolder + Map.Filename);

                Gpk.Close();

                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else
            {
                MessageBox.Show("There's already a map with the same name and code");
                nmCode.Focus();
            }
        }
    }
}
