namespace rNascar23TestApp
{
    partial class MainForm
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblEventName = new System.Windows.Forms.Label();
            this.lblStageLaps = new System.Windows.Forms.Label();
            this.lblRaceLaps = new System.Windows.Forms.Label();
            this.pnlFlagGreenYellow = new System.Windows.Forms.Panel();
            this.picStatus = new System.Windows.Forms.PictureBox();
            this.picGreenYelllowLapIndicator = new System.Windows.Forms.PictureBox();
            this.AutoUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveFeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vehicleListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cupRacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xfinityRacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.truckRacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.driverStatisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.forrmattedLiveFeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.autoUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.practiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qualifyingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.raceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblAutoUpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewState = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlFlagGreenYellow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreenYelllowLapIndicator)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.panel1);
            this.pnlHeader.Controls.Add(this.pnlFlagGreenYellow);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 24);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1292, 97);
            this.pnlHeader.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblEventName);
            this.panel1.Controls.Add(this.lblStageLaps);
            this.panel1.Controls.Add(this.lblRaceLaps);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1292, 42);
            this.panel1.TabIndex = 15;
            // 
            // lblEventName
            // 
            this.lblEventName.AutoSize = true;
            this.lblEventName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventName.Location = new System.Drawing.Point(3, 10);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(15, 20);
            this.lblEventName.TabIndex = 11;
            this.lblEventName.Text = "-";
            // 
            // lblStageLaps
            // 
            this.lblStageLaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStageLaps.AutoSize = true;
            this.lblStageLaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStageLaps.Location = new System.Drawing.Point(1090, 12);
            this.lblStageLaps.Name = "lblStageLaps";
            this.lblStageLaps.Size = new System.Drawing.Size(102, 16);
            this.lblStageLaps.TabIndex = 13;
            this.lblStageLaps.Text = "<Stage Laps>";
            this.lblStageLaps.Visible = false;
            // 
            // lblRaceLaps
            // 
            this.lblRaceLaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRaceLaps.AutoSize = true;
            this.lblRaceLaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRaceLaps.Location = new System.Drawing.Point(879, 12);
            this.lblRaceLaps.Name = "lblRaceLaps";
            this.lblRaceLaps.Size = new System.Drawing.Size(98, 16);
            this.lblRaceLaps.TabIndex = 12;
            this.lblRaceLaps.Text = "<Race Laps>";
            this.lblRaceLaps.Visible = false;
            // 
            // pnlFlagGreenYellow
            // 
            this.pnlFlagGreenYellow.BackColor = System.Drawing.Color.DimGray;
            this.pnlFlagGreenYellow.Controls.Add(this.picStatus);
            this.pnlFlagGreenYellow.Controls.Add(this.picGreenYelllowLapIndicator);
            this.pnlFlagGreenYellow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFlagGreenYellow.Location = new System.Drawing.Point(0, 38);
            this.pnlFlagGreenYellow.Name = "pnlFlagGreenYellow";
            this.pnlFlagGreenYellow.Padding = new System.Windows.Forms.Padding(2);
            this.pnlFlagGreenYellow.Size = new System.Drawing.Size(1292, 59);
            this.pnlFlagGreenYellow.TabIndex = 14;
            // 
            // picStatus
            // 
            this.picStatus.BackColor = System.Drawing.Color.Black;
            this.picStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picStatus.Location = new System.Drawing.Point(2, 2);
            this.picStatus.Name = "picStatus";
            this.picStatus.Size = new System.Drawing.Size(1288, 29);
            this.picStatus.TabIndex = 8;
            this.picStatus.TabStop = false;
            // 
            // picGreenYelllowLapIndicator
            // 
            this.picGreenYelllowLapIndicator.BackColor = System.Drawing.Color.DimGray;
            this.picGreenYelllowLapIndicator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picGreenYelllowLapIndicator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picGreenYelllowLapIndicator.Location = new System.Drawing.Point(2, 31);
            this.picGreenYelllowLapIndicator.Name = "picGreenYelllowLapIndicator";
            this.picGreenYelllowLapIndicator.Size = new System.Drawing.Size(1288, 26);
            this.picGreenYelllowLapIndicator.TabIndex = 9;
            this.picGreenYelllowLapIndicator.TabStop = false;
            this.picGreenYelllowLapIndicator.Paint += new System.Windows.Forms.PaintEventHandler(this.picGreenYelllowLapIndicator_Paint);
            // 
            // AutoUpdateTimer
            // 
            this.AutoUpdateTimer.Interval = 5000;
            this.AutoUpdateTimer.Tick += new System.EventHandler(this.AutoUpdateTimer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.viewToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1292, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.liveFeedToolStripMenuItem,
            this.vehicleListToolStripMenuItem,
            this.eventsToolStripMenuItem,
            this.driverStatisticsToolStripMenuItem,
            this.toolStripMenuItem3,
            this.forrmattedLiveFeedToolStripMenuItem,
            this.toolStripMenuItem1,
            this.autoUpdateToolStripMenuItem,
            this.toolStripMenuItem4});
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.refreshToolStripMenuItem.Text = "&Refresh";
            // 
            // liveFeedToolStripMenuItem
            // 
            this.liveFeedToolStripMenuItem.Name = "liveFeedToolStripMenuItem";
            this.liveFeedToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.liveFeedToolStripMenuItem.Text = "Raw &Live Feed";
            this.liveFeedToolStripMenuItem.Click += new System.EventHandler(this.liveFeedToolStripMenuItem_Click);
            // 
            // vehicleListToolStripMenuItem
            // 
            this.vehicleListToolStripMenuItem.Name = "vehicleListToolStripMenuItem";
            this.vehicleListToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.vehicleListToolStripMenuItem.Text = "Raw &Vehicle List";
            this.vehicleListToolStripMenuItem.Click += new System.EventHandler(this.vehicleListToolStripMenuItem_Click);
            // 
            // eventsToolStripMenuItem
            // 
            this.eventsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cupRacesToolStripMenuItem,
            this.xfinityRacesToolStripMenuItem,
            this.truckRacesToolStripMenuItem,
            this.allToolStripMenuItem});
            this.eventsToolStripMenuItem.Name = "eventsToolStripMenuItem";
            this.eventsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.eventsToolStripMenuItem.Text = "&Series Schedules";
            // 
            // cupRacesToolStripMenuItem
            // 
            this.cupRacesToolStripMenuItem.Name = "cupRacesToolStripMenuItem";
            this.cupRacesToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.cupRacesToolStripMenuItem.Text = "&Cup";
            this.cupRacesToolStripMenuItem.Click += new System.EventHandler(this.cupRacesToolStripMenuItem_Click);
            // 
            // xfinityRacesToolStripMenuItem
            // 
            this.xfinityRacesToolStripMenuItem.Name = "xfinityRacesToolStripMenuItem";
            this.xfinityRacesToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.xfinityRacesToolStripMenuItem.Text = "&Xfinity";
            this.xfinityRacesToolStripMenuItem.Click += new System.EventHandler(this.xfinityRacesToolStripMenuItem_Click);
            // 
            // truckRacesToolStripMenuItem
            // 
            this.truckRacesToolStripMenuItem.Name = "truckRacesToolStripMenuItem";
            this.truckRacesToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.truckRacesToolStripMenuItem.Text = "&Truck";
            this.truckRacesToolStripMenuItem.Click += new System.EventHandler(this.truckRacesToolStripMenuItem_Click);
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.allToolStripMenuItem.Text = "&All";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // driverStatisticsToolStripMenuItem
            // 
            this.driverStatisticsToolStripMenuItem.Name = "driverStatisticsToolStripMenuItem";
            this.driverStatisticsToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.driverStatisticsToolStripMenuItem.Text = "&Driver Statistics";
            this.driverStatisticsToolStripMenuItem.Click += new System.EventHandler(this.driverStatisticsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(192, 6);
            // 
            // forrmattedLiveFeedToolStripMenuItem
            // 
            this.forrmattedLiveFeedToolStripMenuItem.Name = "forrmattedLiveFeedToolStripMenuItem";
            this.forrmattedLiveFeedToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.forrmattedLiveFeedToolStripMenuItem.Text = "&Forrmatted Live Feed";
            this.forrmattedLiveFeedToolStripMenuItem.Click += new System.EventHandler(this.formattedLiveFeedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(192, 6);
            // 
            // autoUpdateToolStripMenuItem
            // 
            this.autoUpdateToolStripMenuItem.CheckOnClick = true;
            this.autoUpdateToolStripMenuItem.Name = "autoUpdateToolStripMenuItem";
            this.autoUpdateToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.autoUpdateToolStripMenuItem.Text = "&Auto-Update Live Feed";
            this.autoUpdateToolStripMenuItem.Click += new System.EventHandler(this.autoUpdateToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(192, 6);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.practiceToolStripMenuItem,
            this.qualifyingToolStripMenuItem,
            this.raceToolStripMenuItem,
            this.toolStripMenuItem2,
            this.infoToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // noneToolStripMenuItem
            // 
            this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
            this.noneToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.noneToolStripMenuItem.Text = "None";
            this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
            // 
            // practiceToolStripMenuItem
            // 
            this.practiceToolStripMenuItem.Name = "practiceToolStripMenuItem";
            this.practiceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.practiceToolStripMenuItem.Text = "Practice";
            this.practiceToolStripMenuItem.Click += new System.EventHandler(this.practiceToolStripMenuItem_Click);
            // 
            // qualifyingToolStripMenuItem
            // 
            this.qualifyingToolStripMenuItem.Name = "qualifyingToolStripMenuItem";
            this.qualifyingToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.qualifyingToolStripMenuItem.Text = "Qualifying";
            this.qualifyingToolStripMenuItem.Click += new System.EventHandler(this.qualifyingToolStripMenuItem_Click);
            // 
            // raceToolStripMenuItem
            // 
            this.raceToolStripMenuItem.Name = "raceToolStripMenuItem";
            this.raceToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.raceToolStripMenuItem.Text = "Race";
            this.raceToolStripMenuItem.Click += new System.EventHandler(this.raceToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(126, 6);
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAutoUpdateStatus,
            this.lblViewState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 845);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1292, 24);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblAutoUpdateStatus
            // 
            this.lblAutoUpdateStatus.AutoSize = false;
            this.lblAutoUpdateStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblAutoUpdateStatus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.lblAutoUpdateStatus.Name = "lblAutoUpdateStatus";
            this.lblAutoUpdateStatus.Size = new System.Drawing.Size(150, 19);
            this.lblAutoUpdateStatus.Text = "Auto-Update Status: Off";
            // 
            // lblViewState
            // 
            this.lblViewState.AutoSize = false;
            this.lblViewState.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblViewState.Name = "lblViewState";
            this.lblViewState.Size = new System.Drawing.Size(125, 19);
            this.lblViewState.Text = "View: None";
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.DimGray;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 121);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(2);
            this.pnlMain.Size = new System.Drawing.Size(1032, 465);
            this.pnlMain.TabIndex = 6;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.DimGray;
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 586);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBottom.Size = new System.Drawing.Size(1292, 259);
            this.pnlBottom.TabIndex = 7;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.DimGray;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1032, 121);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(2);
            this.pnlRight.Size = new System.Drawing.Size(260, 465);
            this.pnlRight.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1292, 869);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rNASCAR23";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlFlagGreenYellow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreenYelllowLapIndicator)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Timer AutoUpdateTimer;
        private System.Windows.Forms.PictureBox picStatus;
        private System.Windows.Forms.Label lblEventName;
        private System.Windows.Forms.Label lblStageLaps;
        private System.Windows.Forms.Label lblRaceLaps;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveFeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem vehicleListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cupRacesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xfinityRacesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem truckRacesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem autoUpdateToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblAutoUpdateStatus;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem practiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qualifyingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem raceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblViewState;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlFlagGreenYellow;
        private System.Windows.Forms.PictureBox picGreenYelllowLapIndicator;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem forrmattedLiveFeedToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem driverStatisticsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

