namespace rNascar23
{
    partial class MultiView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MultiView));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddVideo = new System.Windows.Forms.ToolStripButton();
            this.btnAddAudio = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnTileHorizontal = new System.Windows.Forms.ToolStripButton();
            this.btnTileVertical = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddVideo,
            this.btnAddAudio,
            this.toolStripSeparator1,
            this.btnTileHorizontal,
            this.btnTileVertical});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddVideo
            // 
            this.btnAddVideo.Image = global::rNascar23.Properties.Resources.Video;
            this.btnAddVideo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddVideo.Name = "btnAddVideo";
            this.btnAddVideo.Size = new System.Drawing.Size(82, 22);
            this.btnAddVideo.Text = "Add &Video";
            this.btnAddVideo.Click += new System.EventHandler(this.btnAddVideo_Click);
            // 
            // btnAddAudio
            // 
            this.btnAddAudio.Image = global::rNascar23.Properties.Resources.Audio;
            this.btnAddAudio.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddAudio.Name = "btnAddAudio";
            this.btnAddAudio.Size = new System.Drawing.Size(84, 22);
            this.btnAddAudio.Text = "Add &Audio";
            this.btnAddAudio.Click += new System.EventHandler(this.btnAddAudio_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnTileHorizontal
            // 
            this.btnTileHorizontal.Image = global::rNascar23.Properties.Resources.Horizontal;
            this.btnTileHorizontal.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTileHorizontal.Name = "btnTileHorizontal";
            this.btnTileHorizontal.Size = new System.Drawing.Size(103, 22);
            this.btnTileHorizontal.Text = "Tile &Horizontal";
            this.btnTileHorizontal.Click += new System.EventHandler(this.btnTileHorizontal_Click);
            // 
            // btnTileVertical
            // 
            this.btnTileVertical.Image = global::rNascar23.Properties.Resources.Vertical;
            this.btnTileVertical.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnTileVertical.Name = "btnTileVertical";
            this.btnTileVertical.Size = new System.Drawing.Size(86, 22);
            this.btnTileVertical.Text = "Tile &Vertical";
            this.btnTileVertical.Click += new System.EventHandler(this.btnTileVertical_Click);
            // 
            // MultiView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.Name = "MultiView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rNascar23  Viewer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MultiView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAddVideo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnTileHorizontal;
        private System.Windows.Forms.ToolStripButton btnTileVertical;
        private System.Windows.Forms.ToolStripButton btnAddAudio;
    }
}