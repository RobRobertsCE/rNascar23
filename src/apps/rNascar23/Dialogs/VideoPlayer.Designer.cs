namespace rNascar23.Dialogs
{
    partial class VideoPlayer
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
            this.cboChannels = new System.Windows.Forms.ComboBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.pnlSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select the channel to watch:";
            // 
            // cboChannels
            // 
            this.cboChannels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboChannels.FormattingEnabled = true;
            this.cboChannels.IntegralHeight = false;
            this.cboChannels.Location = new System.Drawing.Point(12, 32);
            this.cboChannels.Name = "cboChannels";
            this.cboChannels.Size = new System.Drawing.Size(310, 29);
            this.cboChannels.TabIndex = 4;
            this.cboChannels.SelectedIndexChanged += new System.EventHandler(this.cboChannels_SelectedIndexChanged);
            // 
            // btnPlay
            // 
            this.btnPlay.Enabled = false;
            this.btnPlay.Location = new System.Drawing.Point(328, 32);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(133, 29);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.Text = "Open Channel";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pnlSelection
            // 
            this.pnlSelection.Controls.Add(this.cboChannels);
            this.pnlSelection.Controls.Add(this.label1);
            this.pnlSelection.Controls.Add(this.btnPlay);
            this.pnlSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(676, 69);
            this.pnlSelection.TabIndex = 8;
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Location = new System.Drawing.Point(8, 8);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(660, 380);
            this.webView.TabIndex = 10;
            this.webView.Visible = false;
            this.webView.ZoomFactor = 1D;
            // 
            // VideoPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 398);
            this.Controls.Add(this.pnlSelection);
            this.Controls.Add(this.webView);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "VideoPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "In-Car Video";
            this.Load += new System.EventHandler(this.VideoSelectionDialog_Load);
            this.pnlSelection.ResumeLayout(false);
            this.pnlSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboChannels;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Panel pnlSelection;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
    }
}