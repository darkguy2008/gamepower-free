namespace DIV2Help2HTML
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbDrop = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.cbMSDOS = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbMSDOS);
            this.groupBox1.Controls.Add(this.lbDrop);
            this.groupBox1.Location = new System.Drawing.Point(12, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 166);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // lbDrop
            // 
            this.lbDrop.AllowDrop = true;
            this.lbDrop.Location = new System.Drawing.Point(6, 18);
            this.lbDrop.Name = "lbDrop";
            this.lbDrop.Size = new System.Drawing.Size(289, 101);
            this.lbDrop.TabIndex = 0;
            this.lbDrop.Text = "Drag a DIV Games Studio 2 HELP file (Ex. HELP.DIV) here to convert it to a naviga" +
    "table HTML file.\r\n\r\nThe output file will end up in the same location as the orig" +
    "inal file.";
            this.lbDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(238, 178);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(12, 178);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "&About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // cbMSDOS
            // 
            this.cbMSDOS.Location = new System.Drawing.Point(9, 122);
            this.cbMSDOS.Name = "cbMSDOS";
            this.cbMSDOS.Size = new System.Drawing.Size(286, 39);
            this.cbMSDOS.TabIndex = 1;
            this.cbMSDOS.Text = "The file is in MS-DOS format (check if the output file ends up with weird charact" +
    "ers)";
            this.cbMSDOS.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(325, 213);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DIV 2 HELP file to HTML converter";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbDrop;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.CheckBox cbMSDOS;
    }
}

