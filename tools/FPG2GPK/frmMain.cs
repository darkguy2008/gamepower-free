using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FPG2GPK
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            Icon = Properties.Resources.icnApp;
            lbDrop.DragEnter += lbDrop_DragEnter;
            lbDrop.DragDrop += lbDrop_DragDrop;
        }

        void lbDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        void lbDrop_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            if (files != null && files.Length > 0)
            {
                try
                {
                    Converter.BatchConvert(files);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error converting files: " + ex.Message);
                }
            }
            MessageBox.Show("Conversion finished");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by DARKGuy / Alemar");
        }
    }
}
