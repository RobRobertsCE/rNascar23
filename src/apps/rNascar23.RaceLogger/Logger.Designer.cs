namespace rNascar23.RaceLogger
{
    partial class Logger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Logger));
            this.dataRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.picOnOff = new System.Windows.Forms.PictureBox();
            this.picCheckForUpdate = new System.Windows.Forms.PictureBox();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblLastUpdate = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEvent = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picOnOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckForUpdate)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataRefreshTimer
            // 
            this.dataRefreshTimer.Interval = 5000;
            this.dataRefreshTimer.Tick += new System.EventHandler(this.dataRefreshTimer_Tick);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnLogFile);
            this.pnlTop.Controls.Add(this.picOnOff);
            this.pnlTop.Controls.Add(this.picCheckForUpdate);
            this.pnlTop.Controls.Add(this.btnStartStop);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(766, 74);
            this.pnlTop.TabIndex = 0;
            // 
            // picOnOff
            // 
            this.picOnOff.BackColor = System.Drawing.Color.White;
            this.picOnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picOnOff.Location = new System.Drawing.Point(137, 21);
            this.picOnOff.Name = "picOnOff";
            this.picOnOff.Size = new System.Drawing.Size(37, 33);
            this.picOnOff.TabIndex = 3;
            this.picOnOff.TabStop = false;
            // 
            // picCheckForUpdate
            // 
            this.picCheckForUpdate.BackColor = System.Drawing.Color.White;
            this.picCheckForUpdate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCheckForUpdate.Location = new System.Drawing.Point(180, 21);
            this.picCheckForUpdate.Name = "picCheckForUpdate";
            this.picCheckForUpdate.Size = new System.Drawing.Size(37, 33);
            this.picCheckForUpdate.TabIndex = 1;
            this.picCheckForUpdate.TabStop = false;
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(18, 19);
            this.btnStartStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(112, 37);
            this.btnStartStop.TabIndex = 0;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessages.Location = new System.Drawing.Point(0, 74);
            this.txtMessages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessages.Size = new System.Drawing.Size(766, 299);
            this.txtMessages.TabIndex = 1;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblLastUpdate,
            this.lblEvent});
            this.statusStrip1.Location = new System.Drawing.Point(0, 373);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(766, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = false;
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(150, 17);
            this.lblLastUpdate.Text = "-";
            this.lblLastUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEvent
            // 
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(12, 17);
            this.lblEvent.Text = "-";
            // 
            // btnLogFile
            // 
            this.btnLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFile.Location = new System.Drawing.Point(641, 21);
            this.btnLogFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(112, 37);
            this.btnLogFile.TabIndex = 4;
            this.btnLogFile.Text = "Log File";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // Logger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 395);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Logger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rNascar23 Event Logger";
            this.Load += new System.EventHandler(this.Logger_Load);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picOnOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckForUpdate)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer dataRefreshTimer;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.TextBox txtMessages;
        private System.Windows.Forms.PictureBox picCheckForUpdate;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdate;
        private System.Windows.Forms.ToolStripStatusLabel lblEvent;
        private System.Windows.Forms.PictureBox picOnOff;
        private System.Windows.Forms.Button btnLogFile;
    }
}

