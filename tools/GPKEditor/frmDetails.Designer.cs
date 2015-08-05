namespace GPKEditor
{
    partial class frmDetails
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
            this.label1 = new System.Windows.Forms.Label();
            this.nmCode = new System.Windows.Forms.NumericUpDown();
            this.txName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txW = new System.Windows.Forms.TextBox();
            this.txH = new System.Windows.Forms.TextBox();
            this.txCY = new System.Windows.Forms.TextBox();
            this.txCX = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txDescription = new System.Windows.Forms.TextBox();
            this.bOk = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.pbImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.nmCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Code:";
            // 
            // nmCode
            // 
            this.nmCode.Location = new System.Drawing.Point(15, 30);
            this.nmCode.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nmCode.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmCode.Name = "nmCode";
            this.nmCode.Size = new System.Drawing.Size(50, 20);
            this.nmCode.TabIndex = 1;
            this.nmCode.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txName
            // 
            this.txName.Location = new System.Drawing.Point(92, 30);
            this.txName.Name = "txName";
            this.txName.Size = new System.Drawing.Size(170, 20);
            this.txName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Width:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Height:";
            // 
            // txW
            // 
            this.txW.Location = new System.Drawing.Point(67, 67);
            this.txW.Name = "txW";
            this.txW.ReadOnly = true;
            this.txW.Size = new System.Drawing.Size(52, 20);
            this.txW.TabIndex = 3;
            // 
            // txH
            // 
            this.txH.Location = new System.Drawing.Point(67, 93);
            this.txH.Name = "txH";
            this.txH.ReadOnly = true;
            this.txH.Size = new System.Drawing.Size(52, 20);
            this.txH.TabIndex = 4;
            // 
            // txCY
            // 
            this.txCY.Location = new System.Drawing.Point(210, 93);
            this.txCY.Name = "txCY";
            this.txCY.Size = new System.Drawing.Size(52, 20);
            this.txCY.TabIndex = 6;
            // 
            // txCX
            // 
            this.txCX.Location = new System.Drawing.Point(210, 67);
            this.txCX.Name = "txCX";
            this.txCX.Size = new System.Drawing.Size(52, 20);
            this.txCX.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Center Y:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(155, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(51, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Center X:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 126);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Description:";
            // 
            // txDescription
            // 
            this.txDescription.Location = new System.Drawing.Point(15, 142);
            this.txDescription.Name = "txDescription";
            this.txDescription.Size = new System.Drawing.Size(382, 20);
            this.txDescription.TabIndex = 7;
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(15, 169);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(75, 23);
            this.bOk.TabIndex = 8;
            this.bOk.Text = "Ok";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // bCancel
            // 
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(322, 168);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 9;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // pbImage
            // 
            this.pbImage.Location = new System.Drawing.Point(277, 14);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(120, 120);
            this.pbImage.TabIndex = 0;
            this.pbImage.TabStop = false;
            // 
            // frmDetails
            // 
            this.AcceptButton = this.bOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(409, 201);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txDescription);
            this.Controls.Add(this.txCY);
            this.Controls.Add(this.txCX);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txH);
            this.Controls.Add(this.txW);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txName);
            this.Controls.Add(this.nmCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDetails";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Description";
            this.Load += new System.EventHandler(this.frmDetails_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nmCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbImage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nmCode;
        private System.Windows.Forms.TextBox txName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txW;
        private System.Windows.Forms.TextBox txH;
        private System.Windows.Forms.TextBox txCY;
        private System.Windows.Forms.TextBox txCX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txDescription;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.Button bCancel;
    }
}