namespace rNascar23.Views
{
    partial class DriverPitStopView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblRunningPosition = new System.Windows.Forms.Label();
            this.lblDriver = new System.Windows.Forms.Label();
            this.lblPitStopTime = new System.Windows.Forms.Label();
            this.lblPositionIn = new System.Windows.Forms.Label();
            this.lblPositionOut = new System.Windows.Forms.Label();
            this.lblDelta = new System.Windows.Forms.Label();
            this.lblPitLap = new System.Windows.Forms.Label();
            this.picTires = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picTires)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRunningPosition
            // 
            this.lblRunningPosition.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunningPosition.Location = new System.Drawing.Point(4, 12);
            this.lblRunningPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRunningPosition.Name = "lblRunningPosition";
            this.lblRunningPosition.Size = new System.Drawing.Size(36, 29);
            this.lblRunningPosition.TabIndex = 0;
            this.lblRunningPosition.Text = "39";
            this.lblRunningPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRunningPosition.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblDriver
            // 
            this.lblDriver.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDriver.Location = new System.Drawing.Point(47, 12);
            this.lblDriver.Name = "lblDriver";
            this.lblDriver.Size = new System.Drawing.Size(200, 29);
            this.lblDriver.TabIndex = 1;
            this.lblDriver.Text = "Driver Name";
            this.lblDriver.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDriver.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblPitStopTime
            // 
            this.lblPitStopTime.Location = new System.Drawing.Point(381, 12);
            this.lblPitStopTime.Name = "lblPitStopTime";
            this.lblPitStopTime.Size = new System.Drawing.Size(69, 29);
            this.lblPitStopTime.TabIndex = 3;
            this.lblPitStopTime.Text = "123.45";
            this.lblPitStopTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPitStopTime.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblPositionIn
            // 
            this.lblPositionIn.Location = new System.Drawing.Point(456, 12);
            this.lblPositionIn.Name = "lblPositionIn";
            this.lblPositionIn.Size = new System.Drawing.Size(35, 29);
            this.lblPositionIn.TabIndex = 4;
            this.lblPositionIn.Text = "25";
            this.lblPositionIn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPositionIn.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblPositionOut
            // 
            this.lblPositionOut.Location = new System.Drawing.Point(497, 12);
            this.lblPositionOut.Name = "lblPositionOut";
            this.lblPositionOut.Size = new System.Drawing.Size(35, 29);
            this.lblPositionOut.TabIndex = 5;
            this.lblPositionOut.Text = "25";
            this.lblPositionOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPositionOut.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblDelta
            // 
            this.lblDelta.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelta.Location = new System.Drawing.Point(538, 12);
            this.lblDelta.Name = "lblDelta";
            this.lblDelta.Size = new System.Drawing.Size(35, 29);
            this.lblDelta.TabIndex = 6;
            this.lblDelta.Text = "25";
            this.lblDelta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDelta.Click += new System.EventHandler(this.View_Selected);
            // 
            // lblPitLap
            // 
            this.lblPitLap.Location = new System.Drawing.Point(309, 12);
            this.lblPitLap.Name = "lblPitLap";
            this.lblPitLap.Size = new System.Drawing.Size(66, 29);
            this.lblPitLap.TabIndex = 7;
            this.lblPitLap.Text = "125";
            this.lblPitLap.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPitLap.Click += new System.EventHandler(this.View_Selected);
            // 
            // picTires
            // 
            this.picTires.BackColor = System.Drawing.Color.DimGray;
            this.picTires.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picTires.Location = new System.Drawing.Point(259, 6);
            this.picTires.Name = "picTires";
            this.picTires.Size = new System.Drawing.Size(37, 40);
            this.picTires.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTires.TabIndex = 2;
            this.picTires.TabStop = false;
            this.picTires.Click += new System.EventHandler(this.View_Selected);
            // 
            // DriverPitStopView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblPitLap);
            this.Controls.Add(this.lblDelta);
            this.Controls.Add(this.lblPositionOut);
            this.Controls.Add(this.lblPositionIn);
            this.Controls.Add(this.lblPitStopTime);
            this.Controls.Add(this.picTires);
            this.Controls.Add(this.lblDriver);
            this.Controls.Add(this.lblRunningPosition);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.Name = "DriverPitStopView";
            this.Size = new System.Drawing.Size(586, 52);
            this.Click += new System.EventHandler(this.View_Selected);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.View_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.picTires)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRunningPosition;
        private System.Windows.Forms.Label lblDriver;
        private System.Windows.Forms.PictureBox picTires;
        private System.Windows.Forms.Label lblPitStopTime;
        private System.Windows.Forms.Label lblPositionIn;
        private System.Windows.Forms.Label lblPositionOut;
        private System.Windows.Forms.Label lblDelta;
        private System.Windows.Forms.Label lblPitLap;
    }
}
