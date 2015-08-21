namespace GPIDE
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
            this.components = new System.ComponentModel.Container();
            this.Toolbar = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsCompile = new System.Windows.Forms.ToolStripButton();
            this.tsDebug = new System.Windows.Forms.ToolStripButton();
            this.tsRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsPack = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsSettings = new System.Windows.Forms.ToolStripButton();
            this.tsCompileMode = new System.Windows.Forms.ToolStripComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Editor = new FastColoredTextBoxNS.FastColoredTextBox();
            this.Toolbar.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).BeginInit();
            this.SuspendLayout();
            // 
            // Toolbar
            // 
            this.Toolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsOpen,
            this.tsSave,
            this.toolStripSeparator1,
            this.tsCompile,
            this.tsDebug,
            this.tsRun,
            this.tsCompileMode,
            this.toolStripSeparator2,
            this.tsPack,
            this.toolStripSeparator3,
            this.tsSettings});
            this.Toolbar.Location = new System.Drawing.Point(0, 0);
            this.Toolbar.Name = "Toolbar";
            this.Toolbar.Size = new System.Drawing.Size(624, 25);
            this.Toolbar.TabIndex = 0;
            this.Toolbar.Text = "Toolbar";
            // 
            // tsNew
            // 
            this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNew.Image = global::GPIDE.Properties.Resources.icnNew;
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(23, 22);
            this.tsNew.Text = "New program";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = global::GPIDE.Properties.Resources.icnOpen;
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.Text = "Open program";
            this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Image = global::GPIDE.Properties.Resources.icnSave;
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.Text = "Save program";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsCompile
            // 
            this.tsCompile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCompile.Image = global::GPIDE.Properties.Resources.icnBricks;
            this.tsCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsCompile.Name = "tsCompile";
            this.tsCompile.Size = new System.Drawing.Size(23, 22);
            this.tsCompile.Text = "Compile";
            this.tsCompile.Click += new System.EventHandler(this.tsCompile_Click);
            // 
            // tsDebug
            // 
            this.tsDebug.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsDebug.Image = global::GPIDE.Properties.Resources.icnCogGo;
            this.tsDebug.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDebug.Name = "tsDebug";
            this.tsDebug.Size = new System.Drawing.Size(23, 22);
            this.tsDebug.Text = "Compile & Debug";
            this.tsDebug.Click += new System.EventHandler(this.tsDebug_Click);
            // 
            // tsRun
            // 
            this.tsRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRun.Image = global::GPIDE.Properties.Resources.icnPlay;
            this.tsRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRun.Name = "tsRun";
            this.tsRun.Size = new System.Drawing.Size(23, 22);
            this.tsRun.Text = "Compile & Run";
            this.tsRun.Click += new System.EventHandler(this.tsRun_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsPack
            // 
            this.tsPack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPack.Image = global::GPIDE.Properties.Resources.icnPack;
            this.tsPack.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPack.Name = "tsPack";
            this.tsPack.Size = new System.Drawing.Size(23, 22);
            this.tsPack.Text = "Pack game";
            this.tsPack.Click += new System.EventHandler(this.tsPack_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsSettings
            // 
            this.tsSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSettings.Image = global::GPIDE.Properties.Resources.icnWrench;
            this.tsSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSettings.Name = "tsSettings";
            this.tsSettings.Size = new System.Drawing.Size(23, 22);
            this.tsSettings.Text = "Settings";
            this.tsSettings.Click += new System.EventHandler(this.tsSettings_Click);
            // 
            // tsCompileMode
            // 
            this.tsCompileMode.Name = "tsCompileMode";
            this.tsCompileMode.Size = new System.Drawing.Size(121, 25);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Editor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 416);
            this.panel1.TabIndex = 1;
            // 
            // Editor
            // 
            this.Editor.AutoCompleteBrackets = true;
            this.Editor.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '[',
        ']',
        '\"',
        '\"'};
            this.Editor.AutoScrollMinSize = new System.Drawing.Size(158, 15);
            this.Editor.BackBrush = null;
            this.Editor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(121)))));
            this.Editor.CaretColor = System.Drawing.Color.White;
            this.Editor.CharHeight = 15;
            this.Editor.CharWidth = 7;
            this.Editor.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.Editor.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold);
            this.Editor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(154)))), ((int)(((byte)(170)))));
            this.Editor.HighlightFoldingIndicator = false;
            this.Editor.IndentBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(20)))), ((int)(((byte)(121)))));
            this.Editor.IsReplaceMode = false;
            this.Editor.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(89)))), ((int)(((byte)(178)))));
            this.Editor.Location = new System.Drawing.Point(0, 0);
            this.Editor.Name = "Editor";
            this.Editor.Paddings = new System.Windows.Forms.Padding(0);
            this.Editor.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.Editor.ServiceColors = null;
            this.Editor.Size = new System.Drawing.Size(624, 416);
            this.Editor.TabIndex = 3;
            this.Editor.Text = "fastColoredTextBox1";
            this.Editor.Zoom = 100;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.Toolbar);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GamePower IDE";
            this.Toolbar.ResumeLayout(false);
            this.Toolbar.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Editor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip Toolbar;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsCompile;
        private System.Windows.Forms.ToolStripButton tsRun;
        private System.Windows.Forms.Panel panel1;
        private FastColoredTextBoxNS.FastColoredTextBox Editor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsSettings;
        private System.Windows.Forms.ToolStripButton tsDebug;
        private System.Windows.Forms.ToolStripButton tsPack;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripComboBox tsCompileMode;
    }
}

