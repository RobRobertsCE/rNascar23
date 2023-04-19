namespace rNascar23
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlEventInfo = new System.Windows.Forms.Panel();
            this.lblEventName = new System.Windows.Forms.Label();
            this.lblStageLaps = new System.Windows.Forms.Label();
            this.lblRaceLaps = new System.Windows.Forms.Label();
            this.pnlFlagGreenYellow = new System.Windows.Forms.Panel();
            this.picFlagStatus = new System.Windows.Forms.PictureBox();
            this.picGreenYelllowLapIndicator = new System.Windows.Forms.PictureBox();
            this.AutoUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patchNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDumpFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replayEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaySpeedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioVideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioChannelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inCarCamerasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblAutoUpdateStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblViewState = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLastUpdate = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEventReplayStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblVersion = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRaceView = new System.Windows.Forms.ToolStripButton();
            this.btnQualifyingView = new System.Windows.Forms.ToolStripButton();
            this.btnPracticeView = new System.Windows.Forms.ToolStripButton();
            this.ddbSchedules = new System.Windows.Forms.ToolStripDropDownButton();
            this.truckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xfinityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.allToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.thisWeekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nextWeekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.todayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.historicalDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbExit = new System.Windows.Forms.ToolStripButton();
            this.tsbAutoUpdate = new System.Windows.Forms.ToolStripButton();
            this.tsbFullScreen = new System.Windows.Forms.ToolStripButton();
            this.btnPitStopsView = new System.Windows.Forms.ToolStripButton();
            this.pnlSchedules = new System.Windows.Forms.Panel();
            this.timEventReplay = new System.Windows.Forms.Timer(this.components);
            this.pnlPitStops = new System.Windows.Forms.Panel();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.lblLoading = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlEventInfo.SuspendLayout();
            this.pnlFlagGreenYellow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFlagStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreenYelllowLapIndicator)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.Black;
            this.pnlHeader.Controls.Add(this.pnlEventInfo);
            this.pnlHeader.Controls.Add(this.pnlFlagGreenYellow);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 49);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1350, 81);
            this.pnlHeader.TabIndex = 2;
            // 
            // pnlEventInfo
            // 
            this.pnlEventInfo.BackColor = System.Drawing.Color.Black;
            this.pnlEventInfo.Controls.Add(this.lblEventName);
            this.pnlEventInfo.Controls.Add(this.lblStageLaps);
            this.pnlEventInfo.Controls.Add(this.lblRaceLaps);
            this.pnlEventInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlEventInfo.Name = "pnlEventInfo";
            this.pnlEventInfo.Size = new System.Drawing.Size(1350, 22);
            this.pnlEventInfo.TabIndex = 15;
            // 
            // lblEventName
            // 
            this.lblEventName.AutoSize = true;
            this.lblEventName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblEventName.Location = new System.Drawing.Point(3, 0);
            this.lblEventName.Name = "lblEventName";
            this.lblEventName.Size = new System.Drawing.Size(0, 20);
            this.lblEventName.TabIndex = 11;
            // 
            // lblStageLaps
            // 
            this.lblStageLaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStageLaps.AutoSize = true;
            this.lblStageLaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStageLaps.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblStageLaps.Location = new System.Drawing.Point(1150, 2);
            this.lblStageLaps.Name = "lblStageLaps";
            this.lblStageLaps.Size = new System.Drawing.Size(0, 16);
            this.lblStageLaps.TabIndex = 13;
            this.lblStageLaps.Visible = false;
            // 
            // lblRaceLaps
            // 
            this.lblRaceLaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRaceLaps.AutoSize = true;
            this.lblRaceLaps.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRaceLaps.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblRaceLaps.Location = new System.Drawing.Point(939, 2);
            this.lblRaceLaps.Name = "lblRaceLaps";
            this.lblRaceLaps.Size = new System.Drawing.Size(0, 16);
            this.lblRaceLaps.TabIndex = 12;
            this.lblRaceLaps.Visible = false;
            // 
            // pnlFlagGreenYellow
            // 
            this.pnlFlagGreenYellow.BackColor = System.Drawing.Color.Black;
            this.pnlFlagGreenYellow.Controls.Add(this.picFlagStatus);
            this.pnlFlagGreenYellow.Controls.Add(this.picGreenYelllowLapIndicator);
            this.pnlFlagGreenYellow.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFlagGreenYellow.Location = new System.Drawing.Point(0, 22);
            this.pnlFlagGreenYellow.Name = "pnlFlagGreenYellow";
            this.pnlFlagGreenYellow.Size = new System.Drawing.Size(1350, 59);
            this.pnlFlagGreenYellow.TabIndex = 14;
            // 
            // picFlagStatus
            // 
            this.picFlagStatus.BackColor = System.Drawing.Color.Black;
            this.picFlagStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picFlagStatus.Location = new System.Drawing.Point(0, 0);
            this.picFlagStatus.Name = "picFlagStatus";
            this.picFlagStatus.Size = new System.Drawing.Size(1350, 39);
            this.picFlagStatus.TabIndex = 8;
            this.picFlagStatus.TabStop = false;
            // 
            // picGreenYelllowLapIndicator
            // 
            this.picGreenYelllowLapIndicator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(16)))), ((int)(((byte)(16)))), ((int)(((byte)(16)))));
            this.picGreenYelllowLapIndicator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.picGreenYelllowLapIndicator.Location = new System.Drawing.Point(0, 39);
            this.picGreenYelllowLapIndicator.Name = "picGreenYelllowLapIndicator";
            this.picGreenYelllowLapIndicator.Size = new System.Drawing.Size(1350, 20);
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
            this.viewToolStripMenuItem,
            this.localDataToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.audioVideoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1350, 24);
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
            this.exitToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitToolStripMenuItem.Image")));
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logFileToolStripMenuItem,
            this.patchNotesToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // logFileToolStripMenuItem
            // 
            this.logFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logFileToolStripMenuItem.Image")));
            this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
            this.logFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logFileToolStripMenuItem.Text = "Log File";
            this.logFileToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
            // 
            // patchNotesToolStripMenuItem
            // 
            this.patchNotesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("patchNotesToolStripMenuItem.Image")));
            this.patchNotesToolStripMenuItem.Name = "patchNotesToolStripMenuItem";
            this.patchNotesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.patchNotesToolStripMenuItem.Text = "Patch Notes";
            this.patchNotesToolStripMenuItem.Click += new System.EventHandler(this.patchNotesToolStripMenuItem_Click);
            // 
            // localDataToolStripMenuItem
            // 
            this.localDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importDumpFileToolStripMenuItem,
            this.replayEventToolStripMenuItem,
            this.replaySpeedToolStripMenuItem,
            this.toolStripMenuItem5,
            this.playToolStripMenuItem,
            this.pauseToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.localDataToolStripMenuItem.Name = "localDataToolStripMenuItem";
            this.localDataToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.localDataToolStripMenuItem.Text = "&Local Data";
            // 
            // importDumpFileToolStripMenuItem
            // 
            this.importDumpFileToolStripMenuItem.Name = "importDumpFileToolStripMenuItem";
            this.importDumpFileToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.importDumpFileToolStripMenuItem.Text = "Import dump file";
            this.importDumpFileToolStripMenuItem.Click += new System.EventHandler(this.importDumpFileToolStripMenuItem_Click);
            // 
            // replayEventToolStripMenuItem
            // 
            this.replayEventToolStripMenuItem.Name = "replayEventToolStripMenuItem";
            this.replayEventToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.replayEventToolStripMenuItem.Text = "Replay event";
            this.replayEventToolStripMenuItem.Click += new System.EventHandler(this.replayEventToolStripMenuItem_Click);
            // 
            // replaySpeedToolStripMenuItem
            // 
            this.replaySpeedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.xToolStripMenuItem2});
            this.replaySpeedToolStripMenuItem.Enabled = false;
            this.replaySpeedToolStripMenuItem.Name = "replaySpeedToolStripMenuItem";
            this.replaySpeedToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.replaySpeedToolStripMenuItem.Text = "Replay Speed";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Checked = true;
            this.xToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.xToolStripMenuItem.Text = "1x";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.xToolStripMenuItem_Click);
            // 
            // xToolStripMenuItem1
            // 
            this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            this.xToolStripMenuItem1.Size = new System.Drawing.Size(92, 22);
            this.xToolStripMenuItem1.Text = "5x";
            this.xToolStripMenuItem1.Click += new System.EventHandler(this.xToolStripMenuItem1_Click);
            // 
            // xToolStripMenuItem2
            // 
            this.xToolStripMenuItem2.Name = "xToolStripMenuItem2";
            this.xToolStripMenuItem2.Size = new System.Drawing.Size(92, 22);
            this.xToolStripMenuItem2.Text = "10x";
            this.xToolStripMenuItem2.Click += new System.EventHandler(this.xToolStripMenuItem2_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(161, 6);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Enabled = false;
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.playToolStripMenuItem.Text = "Play";
            this.playToolStripMenuItem.Click += new System.EventHandler(this.playToolStripMenuItem_Click);
            // 
            // pauseToolStripMenuItem
            // 
            this.pauseToolStripMenuItem.Enabled = false;
            this.pauseToolStripMenuItem.Name = "pauseToolStripMenuItem";
            this.pauseToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.pauseToolStripMenuItem.Text = "Pause";
            this.pauseToolStripMenuItem.Click += new System.EventHandler(this.pauseToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Enabled = false;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripMenuItem.Image")));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // audioVideoToolStripMenuItem
            // 
            this.audioVideoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.audioChannelsToolStripMenuItem,
            this.inCarCamerasToolStripMenuItem});
            this.audioVideoToolStripMenuItem.Name = "audioVideoToolStripMenuItem";
            this.audioVideoToolStripMenuItem.Size = new System.Drawing.Size(86, 20);
            this.audioVideoToolStripMenuItem.Text = "&Audio/Video";
            // 
            // audioChannelsToolStripMenuItem
            // 
            this.audioChannelsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("audioChannelsToolStripMenuItem.Image")));
            this.audioChannelsToolStripMenuItem.Name = "audioChannelsToolStripMenuItem";
            this.audioChannelsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.audioChannelsToolStripMenuItem.Text = "Audio Channels";
            this.audioChannelsToolStripMenuItem.Click += new System.EventHandler(this.audioChannelsToolStripMenuItem_Click);
            // 
            // inCarCamerasToolStripMenuItem
            // 
            this.inCarCamerasToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("inCarCamerasToolStripMenuItem.Image")));
            this.inCarCamerasToolStripMenuItem.Name = "inCarCamerasToolStripMenuItem";
            this.inCarCamerasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.inCarCamerasToolStripMenuItem.Text = "In-Car Cameras";
            this.inCarCamerasToolStripMenuItem.Click += new System.EventHandler(this.inCarCamerasToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblAutoUpdateStatus,
            this.lblViewState,
            this.lblLastUpdate,
            this.lblEventReplayStatus,
            this.lblVersion});
            this.statusStrip1.Location = new System.Drawing.Point(0, 705);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1350, 24);
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
            this.lblViewState.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblViewState.Name = "lblViewState";
            this.lblViewState.Size = new System.Drawing.Size(71, 19);
            this.lblViewState.Text = "View: None";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(108, 19);
            this.lblLastUpdate.Text = "Last Update: None";
            // 
            // lblEventReplayStatus
            // 
            this.lblEventReplayStatus.AutoToolTip = true;
            this.lblEventReplayStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.lblEventReplayStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.lblEventReplayStatus.BorderStyle = System.Windows.Forms.Border3DStyle.RaisedOuter;
            this.lblEventReplayStatus.Name = "lblEventReplayStatus";
            this.lblEventReplayStatus.Size = new System.Drawing.Size(46, 19);
            this.lblEventReplayStatus.Text = "Replay";
            this.lblEventReplayStatus.Visible = false;
            // 
            // lblVersion
            // 
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(1002, 19);
            this.lblVersion.Spring = true;
            this.lblVersion.Text = "-.-.-.-";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.Color.Black;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 130);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(2);
            this.pnlMain.Size = new System.Drawing.Size(1114, 347);
            this.pnlMain.TabIndex = 6;
            this.pnlMain.Visible = false;
            // 
            // pnlBottom
            // 
            this.pnlBottom.AutoScroll = true;
            this.pnlBottom.BackColor = System.Drawing.Color.Black;
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 477);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(2);
            this.pnlBottom.Size = new System.Drawing.Size(1114, 228);
            this.pnlBottom.TabIndex = 7;
            this.pnlBottom.Visible = false;
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.BackColor = System.Drawing.Color.Black;
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(1114, 130);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(2);
            this.pnlRight.Size = new System.Drawing.Size(236, 575);
            this.pnlRight.TabIndex = 8;
            this.pnlRight.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Black;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRaceView,
            this.btnQualifyingView,
            this.btnPracticeView,
            this.ddbSchedules,
            this.tsbExit,
            this.tsbAutoUpdate,
            this.tsbFullScreen,
            this.btnPitStopsView});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1350, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnRaceView
            // 
            this.btnRaceView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRaceView.ForeColor = System.Drawing.Color.Silver;
            this.btnRaceView.Image = ((System.Drawing.Image)(resources.GetObject("btnRaceView.Image")));
            this.btnRaceView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRaceView.Name = "btnRaceView";
            this.btnRaceView.Size = new System.Drawing.Size(87, 22);
            this.btnRaceView.Text = "Race View (F1)";
            this.btnRaceView.Click += new System.EventHandler(this.btnRaceView_Click);
            // 
            // btnQualifyingView
            // 
            this.btnQualifyingView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnQualifyingView.ForeColor = System.Drawing.Color.Silver;
            this.btnQualifyingView.Image = ((System.Drawing.Image)(resources.GetObject("btnQualifyingView.Image")));
            this.btnQualifyingView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnQualifyingView.Name = "btnQualifyingView";
            this.btnQualifyingView.Size = new System.Drawing.Size(117, 22);
            this.btnQualifyingView.Text = "Qualifying View (F2)";
            this.btnQualifyingView.Click += new System.EventHandler(this.btnQualifyingView_Click);
            // 
            // btnPracticeView
            // 
            this.btnPracticeView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPracticeView.ForeColor = System.Drawing.Color.Silver;
            this.btnPracticeView.Image = ((System.Drawing.Image)(resources.GetObject("btnPracticeView.Image")));
            this.btnPracticeView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPracticeView.Name = "btnPracticeView";
            this.btnPracticeView.Size = new System.Drawing.Size(104, 22);
            this.btnPracticeView.Text = "Practice View (F3)";
            this.btnPracticeView.Click += new System.EventHandler(this.btnPracticeView_Click);
            // 
            // ddbSchedules
            // 
            this.ddbSchedules.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ddbSchedules.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.truckToolStripMenuItem,
            this.xfinityToolStripMenuItem,
            this.cupToolStripMenuItem,
            this.toolStripMenuItem8,
            this.allToolStripMenuItem1,
            this.toolStripMenuItem9,
            this.thisWeekToolStripMenuItem,
            this.nextWeekToolStripMenuItem,
            this.todayToolStripMenuItem,
            this.toolStripMenuItem6,
            this.historicalDataToolStripMenuItem});
            this.ddbSchedules.ForeColor = System.Drawing.Color.Silver;
            this.ddbSchedules.Image = ((System.Drawing.Image)(resources.GetObject("ddbSchedules.Image")));
            this.ddbSchedules.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ddbSchedules.Name = "ddbSchedules";
            this.ddbSchedules.Size = new System.Drawing.Size(96, 22);
            this.ddbSchedules.Text = "Schedules (F4)";
            // 
            // truckToolStripMenuItem
            // 
            this.truckToolStripMenuItem.Name = "truckToolStripMenuItem";
            this.truckToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.truckToolStripMenuItem.Text = "&Truck";
            this.truckToolStripMenuItem.Click += new System.EventHandler(this.truckToolStripMenuItem_Click);
            // 
            // xfinityToolStripMenuItem
            // 
            this.xfinityToolStripMenuItem.Name = "xfinityToolStripMenuItem";
            this.xfinityToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.xfinityToolStripMenuItem.Text = "&Xfinity";
            this.xfinityToolStripMenuItem.Click += new System.EventHandler(this.xfinityToolStripMenuItem_Click);
            // 
            // cupToolStripMenuItem
            // 
            this.cupToolStripMenuItem.Name = "cupToolStripMenuItem";
            this.cupToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.cupToolStripMenuItem.Text = "&Cup";
            this.cupToolStripMenuItem.Click += new System.EventHandler(this.cupToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(148, 6);
            // 
            // allToolStripMenuItem1
            // 
            this.allToolStripMenuItem1.Name = "allToolStripMenuItem1";
            this.allToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
            this.allToolStripMenuItem1.Text = "&All";
            this.allToolStripMenuItem1.Click += new System.EventHandler(this.allToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(148, 6);
            // 
            // thisWeekToolStripMenuItem
            // 
            this.thisWeekToolStripMenuItem.Name = "thisWeekToolStripMenuItem";
            this.thisWeekToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.thisWeekToolStripMenuItem.Text = "This &Week";
            this.thisWeekToolStripMenuItem.Click += new System.EventHandler(this.thisWeekToolStripMenuItem_Click);
            // 
            // nextWeekToolStripMenuItem
            // 
            this.nextWeekToolStripMenuItem.Name = "nextWeekToolStripMenuItem";
            this.nextWeekToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.nextWeekToolStripMenuItem.Text = "&Next Week";
            this.nextWeekToolStripMenuItem.Click += new System.EventHandler(this.nextWeekToolStripMenuItem_Click);
            // 
            // todayToolStripMenuItem
            // 
            this.todayToolStripMenuItem.Name = "todayToolStripMenuItem";
            this.todayToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.todayToolStripMenuItem.Text = "T&oday";
            this.todayToolStripMenuItem.Click += new System.EventHandler(this.todayToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(148, 6);
            // 
            // historicalDataToolStripMenuItem
            // 
            this.historicalDataToolStripMenuItem.Name = "historicalDataToolStripMenuItem";
            this.historicalDataToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.historicalDataToolStripMenuItem.Text = "Historical Data";
            this.historicalDataToolStripMenuItem.Click += new System.EventHandler(this.historicalDataToolStripMenuItem_Click);
            // 
            // tsbExit
            // 
            this.tsbExit.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbExit.ForeColor = System.Drawing.Color.Silver;
            this.tsbExit.Image = ((System.Drawing.Image)(resources.GetObject("tsbExit.Image")));
            this.tsbExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsbExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExit.Name = "tsbExit";
            this.tsbExit.Size = new System.Drawing.Size(98, 22);
            this.tsbExit.Text = "Exit (Alt + F4)";
            this.tsbExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tsbExit.Click += new System.EventHandler(this.tsbExit_Click);
            // 
            // tsbAutoUpdate
            // 
            this.tsbAutoUpdate.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbAutoUpdate.BackColor = System.Drawing.Color.DimGray;
            this.tsbAutoUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbAutoUpdate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tsbAutoUpdate.ForeColor = System.Drawing.Color.White;
            this.tsbAutoUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsbAutoUpdate.Image")));
            this.tsbAutoUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAutoUpdate.Name = "tsbAutoUpdate";
            this.tsbAutoUpdate.Size = new System.Drawing.Size(119, 22);
            this.tsbAutoUpdate.Text = "Auto-Update (F12)";
            this.tsbAutoUpdate.Click += new System.EventHandler(this.tsbAutoUpdate_Click);
            // 
            // tsbFullScreen
            // 
            this.tsbFullScreen.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFullScreen.ForeColor = System.Drawing.Color.Silver;
            this.tsbFullScreen.Image = ((System.Drawing.Image)(resources.GetObject("tsbFullScreen.Image")));
            this.tsbFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFullScreen.Name = "tsbFullScreen";
            this.tsbFullScreen.Size = new System.Drawing.Size(100, 22);
            this.tsbFullScreen.Text = "Full Screen  (F11)";
            this.tsbFullScreen.Click += new System.EventHandler(this.tsbFullScreen_Click);
            // 
            // btnPitStopsView
            // 
            this.btnPitStopsView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPitStopsView.ForeColor = System.Drawing.Color.Silver;
            this.btnPitStopsView.Image = ((System.Drawing.Image)(resources.GetObject("btnPitStopsView.Image")));
            this.btnPitStopsView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPitStopsView.Name = "btnPitStopsView";
            this.btnPitStopsView.Size = new System.Drawing.Size(80, 22);
            this.btnPitStopsView.Text = "Pit Stops (F5)";
            this.btnPitStopsView.Click += new System.EventHandler(this.btnPitStopsView_Click);
            // 
            // pnlSchedules
            // 
            this.pnlSchedules.BackColor = System.Drawing.Color.Black;
            this.pnlSchedules.Location = new System.Drawing.Point(176, 269);
            this.pnlSchedules.Name = "pnlSchedules";
            this.pnlSchedules.Size = new System.Drawing.Size(0, 0);
            this.pnlSchedules.TabIndex = 10;
            this.pnlSchedules.Visible = false;
            // 
            // timEventReplay
            // 
            this.timEventReplay.Interval = 1000;
            this.timEventReplay.Tick += new System.EventHandler(this.timEventReplay_Tick);
            // 
            // pnlPitStops
            // 
            this.pnlPitStops.BackColor = System.Drawing.Color.Black;
            this.pnlPitStops.Location = new System.Drawing.Point(176, 250);
            this.pnlPitStops.Name = "pnlPitStops";
            this.pnlPitStops.Size = new System.Drawing.Size(54, 44);
            this.pnlPitStops.TabIndex = 11;
            this.pnlPitStops.Visible = false;
            // 
            // pnlLoading
            // 
            this.pnlLoading.BackColor = System.Drawing.Color.Black;
            this.pnlLoading.Controls.Add(this.lblLoading);
            this.pnlLoading.Font = new System.Drawing.Font("Snyder Speed Brush", 20.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlLoading.Location = new System.Drawing.Point(0, 4);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(13, 20);
            this.pnlLoading.TabIndex = 12;
            this.pnlLoading.Visible = false;
            // 
            // lblLoading
            // 
            this.lblLoading.AutoSize = true;
            this.lblLoading.BackColor = System.Drawing.Color.Black;
            this.lblLoading.ForeColor = System.Drawing.Color.Silver;
            this.lblLoading.Location = new System.Drawing.Point(603, 292);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(133, 33);
            this.lblLoading.TabIndex = 0;
            this.lblLoading.Text = "Loading...";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.pnlLoading);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlPitStops);
            this.Controls.Add(this.pnlSchedules);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "rNASCAR23";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.pnlEventInfo.ResumeLayout(false);
            this.pnlEventInfo.PerformLayout();
            this.pnlFlagGreenYellow.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFlagStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreenYelllowLapIndicator)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Timer AutoUpdateTimer;
        private System.Windows.Forms.PictureBox picFlagStatus;
        private System.Windows.Forms.Label lblEventName;
        private System.Windows.Forms.Label lblStageLaps;
        private System.Windows.Forms.Label lblRaceLaps;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblAutoUpdateStatus;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblViewState;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlFlagGreenYellow;
        private System.Windows.Forms.PictureBox picGreenYelllowLapIndicator;
        private System.Windows.Forms.Panel pnlEventInfo;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnRaceView;
        private System.Windows.Forms.ToolStripButton btnQualifyingView;
        private System.Windows.Forms.ToolStripButton btnPracticeView;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton ddbSchedules;
        private System.Windows.Forms.ToolStripMenuItem truckToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xfinityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem thisWeekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem todayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
        private System.Windows.Forms.Panel pnlSchedules;
        private System.Windows.Forms.ToolStripButton tsbFullScreen;
        private System.Windows.Forms.ToolStripButton tsbExit;
        private System.Windows.Forms.ToolStripButton tsbAutoUpdate;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblLastUpdate;
        private System.Windows.Forms.ToolStripButton btnPitStopsView;
        private System.Windows.Forms.ToolStripMenuItem localDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDumpFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replayEventToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Timer timEventReplay;
        private System.Windows.Forms.ToolStripStatusLabel lblEventReplayStatus;
        private System.Windows.Forms.ToolStripMenuItem nextWeekToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel lblVersion;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem historicalDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaySpeedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem audioVideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem audioChannelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inCarCamerasToolStripMenuItem;
        private System.Windows.Forms.Panel pnlPitStops;
        private System.Windows.Forms.ToolStripMenuItem patchNotesToolStripMenuItem;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Label lblLoading;
    }
}

