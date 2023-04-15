namespace rNascar23.Views
{
    partial class ScheduledEventView
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
            this.components = new System.ComponentModel.Container();
            this.picSeries = new System.Windows.Forms.PictureBox();
            this.lblEventName = new System.Windows.Forms.Label();
            this.lblTrack = new System.Windows.Forms.Label();
            this.lblEventDistance = new System.Windows.Forms.Label();
            this.picTv = new System.Windows.Forms.PictureBox();
            this.lblTv = new System.Windows.Forms.Label();
            this.lblRadio = new System.Windows.Forms.Label();
            this.picRadio = new System.Windows.Forms.PictureBox();
            this.lblSatellite = new System.Windows.Forms.Label();
            this.picSatellite = new System.Windows.Forms.PictureBox();
            this.lblEventDate = new System.Windows.Forms.Label();
            this.lblEventTime = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picSeries)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRadio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSatellite)).BeginInit();
            this.SuspendLayout();
            // 
            // picSeries
            // 
            this.picSeries.Image = global::rNascar23.Properties.Resources.NCS_Small;
            this.picSeries.Location = new System.Drawing.Point(50, 10);
            this.picSeries.Name = "picSeries";
            this.picSeries.Size = new System.Drawing.Size(84, 38);
            this.picSeries.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSeries.TabIndex = 0;
            this.picSeries.TabStop = false;
            this.picSeries.Click += new System.EventHandler(this.View_Selected);
            this.picSeries.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblEventName
            // 
            this.lblEventName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventName.Location = new System.Drawing.Point(146, 13);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(474, 23);
            this.lblEventName.TabIndex = 1;
            this.lblEventName.Text = "Toyota Owners 400";
            this.lblEventName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEventName.Click += new System.EventHandler(this.View_Selected);
            this.lblEventName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblTrack
            // 
            this.lblTrack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrack.Location = new System.Drawing.Point(146, 43);
            this.lblTrack.Name = "lblTrack";
            this.lblTrack.Size = new System.Drawing.Size(474, 23);
            this.lblTrack.TabIndex = 2;
            this.lblTrack.Text = "Richmond Raceway";
            this.lblTrack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTrack.Click += new System.EventHandler(this.View_Selected);
            this.lblTrack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblEventDistance
            // 
            this.lblEventDistance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventDistance.Location = new System.Drawing.Point(146, 72);
            this.lblEventDistance.Name = "lblEventDistance";
            this.lblEventDistance.Size = new System.Drawing.Size(474, 23);
            this.lblEventDistance.TabIndex = 3;
            this.lblEventDistance.Text = "400 Laps/300 Miles";
            this.lblEventDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEventDistance.Click += new System.EventHandler(this.View_Selected);
            this.lblEventDistance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // picTv
            // 
            this.picTv.BackColor = System.Drawing.Color.White;
            this.picTv.Image = global::rNascar23.Properties.Resources.FS1_Small;
            this.picTv.Location = new System.Drawing.Point(704, 13);
            this.picTv.Name = "picTv";
            this.picTv.Size = new System.Drawing.Size(48, 23);
            this.picTv.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTv.TabIndex = 4;
            this.picTv.TabStop = false;
            this.picTv.Click += new System.EventHandler(this.View_Selected);
            this.picTv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblTv
            // 
            this.lblTv.Location = new System.Drawing.Point(639, 15);
            this.lblTv.Name = "lblTv";
            this.lblTv.Size = new System.Drawing.Size(59, 18);
            this.lblTv.TabIndex = 5;
            this.lblTv.Text = "TV:";
            this.lblTv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTv.Click += new System.EventHandler(this.View_Selected);
            this.lblTv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblRadio
            // 
            this.lblRadio.Location = new System.Drawing.Point(639, 45);
            this.lblRadio.Name = "lblRadio";
            this.lblRadio.Size = new System.Drawing.Size(59, 18);
            this.lblRadio.TabIndex = 7;
            this.lblRadio.Text = "Radio:";
            this.lblRadio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblRadio.Click += new System.EventHandler(this.View_Selected);
            this.lblRadio.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // picRadio
            // 
            this.picRadio.BackColor = System.Drawing.Color.White;
            this.picRadio.Image = global::rNascar23.Properties.Resources.MRN_Small;
            this.picRadio.Location = new System.Drawing.Point(704, 43);
            this.picRadio.Name = "picRadio";
            this.picRadio.Size = new System.Drawing.Size(48, 23);
            this.picRadio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picRadio.TabIndex = 6;
            this.picRadio.TabStop = false;
            this.picRadio.Click += new System.EventHandler(this.View_Selected);
            this.picRadio.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblSatellite
            // 
            this.lblSatellite.Location = new System.Drawing.Point(639, 74);
            this.lblSatellite.Name = "lblSatellite";
            this.lblSatellite.Size = new System.Drawing.Size(59, 18);
            this.lblSatellite.TabIndex = 9;
            this.lblSatellite.Text = "Satellite:";
            this.lblSatellite.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSatellite.Click += new System.EventHandler(this.View_Selected);
            this.lblSatellite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // picSatellite
            // 
            this.picSatellite.BackColor = System.Drawing.Color.White;
            this.picSatellite.Image = global::rNascar23.Properties.Resources.Sirius_Small;
            this.picSatellite.Location = new System.Drawing.Point(704, 72);
            this.picSatellite.Name = "picSatellite";
            this.picSatellite.Size = new System.Drawing.Size(48, 23);
            this.picSatellite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picSatellite.TabIndex = 8;
            this.picSatellite.TabStop = false;
            this.picSatellite.Click += new System.EventHandler(this.View_Selected);
            this.picSatellite.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblEventDate
            // 
            this.lblEventDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventDate.Location = new System.Drawing.Point(14, 53);
            this.lblEventDate.Name = "lblEventDate";
            this.lblEventDate.Size = new System.Drawing.Size(157, 20);
            this.lblEventDate.TabIndex = 10;
            this.lblEventDate.Text = "Saturday, Apr 2 2023";
            this.lblEventDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEventDate.Click += new System.EventHandler(this.View_Selected);
            this.lblEventDate.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // lblEventTime
            // 
            this.lblEventTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventTime.Location = new System.Drawing.Point(14, 74);
            this.lblEventTime.Name = "lblEventTime";
            this.lblEventTime.Size = new System.Drawing.Size(157, 20);
            this.lblEventTime.TabIndex = 11;
            this.lblEventTime.Text = "3:30 PM ET";
            this.lblEventTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblEventTime.Click += new System.EventHandler(this.View_Selected);
            this.lblEventTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            // 
            // ScheduledEventView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblEventTime);
            this.Controls.Add(this.lblEventDate);
            this.Controls.Add(this.lblSatellite);
            this.Controls.Add(this.picSatellite);
            this.Controls.Add(this.lblRadio);
            this.Controls.Add(this.picRadio);
            this.Controls.Add(this.lblTv);
            this.Controls.Add(this.picTv);
            this.Controls.Add(this.lblEventDistance);
            this.Controls.Add(this.lblTrack);
            this.Controls.Add(this.lblEventName);
            this.Controls.Add(this.picSeries);
            this.DoubleBuffered = true;
            this.Name = "ScheduledEventView";
            this.Size = new System.Drawing.Size(774, 107);
            this.Load += new System.EventHandler(this.ScheduledEventView_Load);
            this.Click += new System.EventHandler(this.View_Selected);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ScheduledEventView_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ScheduledEventView_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.picSeries)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRadio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSatellite)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picSeries;
        private System.Windows.Forms.Label lblEventName;
        private System.Windows.Forms.Label lblTrack;
        private System.Windows.Forms.Label lblEventDistance;
        private System.Windows.Forms.PictureBox picTv;
        private System.Windows.Forms.Label lblTv;
        private System.Windows.Forms.Label lblRadio;
        private System.Windows.Forms.PictureBox picRadio;
        private System.Windows.Forms.Label lblSatellite;
        private System.Windows.Forms.PictureBox picSatellite;
        private System.Windows.Forms.Label lblEventDate;
        private System.Windows.Forms.Label lblEventTime;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
