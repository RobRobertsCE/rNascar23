namespace rNascar23TestApp.Dialogs
{
    partial class UserSettingsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettingsDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDataDirectory = new System.Windows.Forms.TextBox();
            this.txtBackupDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLogDirectory = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDataDirectory = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnBackupDirectory = new System.Windows.Forms.Button();
            this.btnLogDirectory = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbFastestLapsSpeed = new System.Windows.Forms.RadioButton();
            this.rbFastestLapsTime = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbLastNLapsTime = new System.Windows.Forms.RadioButton();
            this.rbLastNLapsSpeed = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbBestNLapsTime = new System.Windows.Forms.RadioButton();
            this.rbBestNLapsSpeed = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbLeaderboardLastLapTime = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.rbLeaderboardLastLapSpeed = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbLeaderboardBestLapTime = new System.Windows.Forms.RadioButton();
            this.rbLeaderboardBestLapSpeed = new System.Windows.Forms.RadioButton();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SteelBlue;
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 461);
            this.panel1.Margin = new System.Windows.Forms.Padding(6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(677, 68);
            this.panel1.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(565, 11);
            this.btnSave.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 45);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(23, 11);
            this.button1.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 45);
            this.button1.TabIndex = 5;
            this.button1.Text = "Cancel";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.Location = new System.Drawing.Point(2257, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(255, 64);
            this.button2.TabIndex = 4;
            this.button2.Text = "Save and Close";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(-912, 19);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(279, 64);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Discard Changes";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(1338, 19);
            this.btnClose.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(255, 64);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Save and Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Data Directory:";
            // 
            // txtDataDirectory
            // 
            this.txtDataDirectory.Location = new System.Drawing.Point(18, 32);
            this.txtDataDirectory.Name = "txtDataDirectory";
            this.txtDataDirectory.Size = new System.Drawing.Size(587, 27);
            this.txtDataDirectory.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtDataDirectory, "Directory where your json data files will be stored.");
            // 
            // txtBackupDirectory
            // 
            this.txtBackupDirectory.Location = new System.Drawing.Point(18, 85);
            this.txtBackupDirectory.Name = "txtBackupDirectory";
            this.txtBackupDirectory.Size = new System.Drawing.Size(587, 27);
            this.txtBackupDirectory.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtBackupDirectory, "Directory where your backups will be written to.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Backups Directory:";
            // 
            // txtLogDirectory
            // 
            this.txtLogDirectory.Location = new System.Drawing.Point(18, 138);
            this.txtLogDirectory.Name = "txtLogDirectory";
            this.txtLogDirectory.Size = new System.Drawing.Size(587, 27);
            this.txtLogDirectory.TabIndex = 7;
            this.toolTip1.SetToolTip(this.txtLogDirectory, "Directory where your log files and debug dump files will be written to.");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 115);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Log File Directory:";
            // 
            // btnDataDirectory
            // 
            this.btnDataDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnDataDirectory.Image")));
            this.btnDataDirectory.Location = new System.Drawing.Point(617, 32);
            this.btnDataDirectory.Name = "btnDataDirectory";
            this.btnDataDirectory.Size = new System.Drawing.Size(43, 27);
            this.btnDataDirectory.TabIndex = 8;
            this.toolTip1.SetToolTip(this.btnDataDirectory, "Select your data file directory");
            this.btnDataDirectory.UseVisualStyleBackColor = true;
            this.btnDataDirectory.Click += new System.EventHandler(this.btnDataDirectory_Click);
            // 
            // btnBackupDirectory
            // 
            this.btnBackupDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnBackupDirectory.Image")));
            this.btnBackupDirectory.Location = new System.Drawing.Point(617, 85);
            this.btnBackupDirectory.Name = "btnBackupDirectory";
            this.btnBackupDirectory.Size = new System.Drawing.Size(43, 27);
            this.btnBackupDirectory.TabIndex = 9;
            this.toolTip1.SetToolTip(this.btnBackupDirectory, "Select your backup file directory");
            this.btnBackupDirectory.UseVisualStyleBackColor = true;
            this.btnBackupDirectory.Click += new System.EventHandler(this.btnBackupDirectory_Click);
            // 
            // btnLogDirectory
            // 
            this.btnLogDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnLogDirectory.Image")));
            this.btnLogDirectory.Location = new System.Drawing.Point(617, 138);
            this.btnLogDirectory.Name = "btnLogDirectory";
            this.btnLogDirectory.Size = new System.Drawing.Size(43, 27);
            this.btnLogDirectory.TabIndex = 10;
            this.toolTip1.SetToolTip(this.btnLogDirectory, "Select your log file directory");
            this.btnLogDirectory.UseVisualStyleBackColor = true;
            this.btnLogDirectory.Click += new System.EventHandler(this.btnLogDirectory_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(19, 191);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 20);
            this.label4.TabIndex = 11;
            this.label4.Text = "Fastest Laps displays:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbFastestLapsTime);
            this.groupBox1.Controls.Add(this.rbFastestLapsSpeed);
            this.groupBox1.Location = new System.Drawing.Point(295, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 48);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // rbFastestLapsSpeed
            // 
            this.rbFastestLapsSpeed.AutoSize = true;
            this.rbFastestLapsSpeed.Checked = true;
            this.rbFastestLapsSpeed.Location = new System.Drawing.Point(6, 16);
            this.rbFastestLapsSpeed.Name = "rbFastestLapsSpeed";
            this.rbFastestLapsSpeed.Size = new System.Drawing.Size(130, 24);
            this.rbFastestLapsSpeed.TabIndex = 0;
            this.rbFastestLapsSpeed.TabStop = true;
            this.rbFastestLapsSpeed.Text = "Speed in M.P.H.";
            this.rbFastestLapsSpeed.UseVisualStyleBackColor = true;
            // 
            // rbFastestLapsTime
            // 
            this.rbFastestLapsTime.AutoSize = true;
            this.rbFastestLapsTime.Location = new System.Drawing.Point(166, 16);
            this.rbFastestLapsTime.Name = "rbFastestLapsTime";
            this.rbFastestLapsTime.Size = new System.Drawing.Size(135, 24);
            this.rbFastestLapsTime.TabIndex = 1;
            this.rbFastestLapsTime.Text = "Time in Seconds";
            this.rbFastestLapsTime.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbLastNLapsTime);
            this.groupBox2.Controls.Add(this.rbLastNLapsSpeed);
            this.groupBox2.Location = new System.Drawing.Point(295, 225);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(310, 48);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            // 
            // rbLastNLapsTime
            // 
            this.rbLastNLapsTime.AutoSize = true;
            this.rbLastNLapsTime.Location = new System.Drawing.Point(166, 16);
            this.rbLastNLapsTime.Name = "rbLastNLapsTime";
            this.rbLastNLapsTime.Size = new System.Drawing.Size(135, 24);
            this.rbLastNLapsTime.TabIndex = 1;
            this.rbLastNLapsTime.Text = "Time in Seconds";
            this.rbLastNLapsTime.UseVisualStyleBackColor = true;
            // 
            // rbLastNLapsSpeed
            // 
            this.rbLastNLapsSpeed.AutoSize = true;
            this.rbLastNLapsSpeed.Checked = true;
            this.rbLastNLapsSpeed.Location = new System.Drawing.Point(6, 16);
            this.rbLastNLapsSpeed.Name = "rbLastNLapsSpeed";
            this.rbLastNLapsSpeed.Size = new System.Drawing.Size(130, 24);
            this.rbLastNLapsSpeed.TabIndex = 0;
            this.rbLastNLapsSpeed.TabStop = true;
            this.rbLastNLapsSpeed.Text = "Speed in M.P.H.";
            this.rbLastNLapsSpeed.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(19, 245);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 20);
            this.label5.TabIndex = 13;
            this.label5.Text = "Last N Laps displays:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbBestNLapsTime);
            this.groupBox3.Controls.Add(this.rbBestNLapsSpeed);
            this.groupBox3.Location = new System.Drawing.Point(295, 279);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(310, 48);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            // 
            // rbBestNLapsTime
            // 
            this.rbBestNLapsTime.AutoSize = true;
            this.rbBestNLapsTime.Location = new System.Drawing.Point(166, 16);
            this.rbBestNLapsTime.Name = "rbBestNLapsTime";
            this.rbBestNLapsTime.Size = new System.Drawing.Size(135, 24);
            this.rbBestNLapsTime.TabIndex = 1;
            this.rbBestNLapsTime.Text = "Time in Seconds";
            this.rbBestNLapsTime.UseVisualStyleBackColor = true;
            // 
            // rbBestNLapsSpeed
            // 
            this.rbBestNLapsSpeed.AutoSize = true;
            this.rbBestNLapsSpeed.Checked = true;
            this.rbBestNLapsSpeed.Location = new System.Drawing.Point(6, 16);
            this.rbBestNLapsSpeed.Name = "rbBestNLapsSpeed";
            this.rbBestNLapsSpeed.Size = new System.Drawing.Size(130, 24);
            this.rbBestNLapsSpeed.TabIndex = 0;
            this.rbBestNLapsSpeed.TabStop = true;
            this.rbBestNLapsSpeed.Text = "Speed in M.P.H.";
            this.rbBestNLapsSpeed.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(19, 299);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(156, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Best N Laps displays:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbLeaderboardLastLapTime);
            this.groupBox4.Controls.Add(this.radioButton6);
            this.groupBox4.Location = new System.Drawing.Point(295, 333);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(310, 48);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            // 
            // rbLeaderboardLastLapTime
            // 
            this.rbLeaderboardLastLapTime.AutoSize = true;
            this.rbLeaderboardLastLapTime.Location = new System.Drawing.Point(166, 16);
            this.rbLeaderboardLastLapTime.Name = "rbLeaderboardLastLapTime";
            this.rbLeaderboardLastLapTime.Size = new System.Drawing.Size(135, 24);
            this.rbLeaderboardLastLapTime.TabIndex = 1;
            this.rbLeaderboardLastLapTime.Text = "Time in Seconds";
            this.rbLeaderboardLastLapTime.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Checked = true;
            this.radioButton6.Location = new System.Drawing.Point(6, 16);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(130, 24);
            this.radioButton6.TabIndex = 0;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Speed in M.P.H.";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // rbLeaderboardLastLapSpeed
            // 
            this.rbLeaderboardLastLapSpeed.AutoSize = true;
            this.rbLeaderboardLastLapSpeed.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbLeaderboardLastLapSpeed.Location = new System.Drawing.Point(19, 353);
            this.rbLeaderboardLastLapSpeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.rbLeaderboardLastLapSpeed.Name = "rbLeaderboardLastLapSpeed";
            this.rbLeaderboardLastLapSpeed.Size = new System.Drawing.Size(223, 20);
            this.rbLeaderboardLastLapSpeed.TabIndex = 15;
            this.rbLeaderboardLastLapSpeed.Text = "Leaderboard Last Lap displays:";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbLeaderboardBestLapTime);
            this.groupBox5.Controls.Add(this.rbLeaderboardBestLapSpeed);
            this.groupBox5.Location = new System.Drawing.Point(295, 387);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(310, 48);
            this.groupBox5.TabIndex = 14;
            this.groupBox5.TabStop = false;
            // 
            // rbLeaderboardBestLapTime
            // 
            this.rbLeaderboardBestLapTime.AutoSize = true;
            this.rbLeaderboardBestLapTime.Location = new System.Drawing.Point(166, 16);
            this.rbLeaderboardBestLapTime.Name = "rbLeaderboardBestLapTime";
            this.rbLeaderboardBestLapTime.Size = new System.Drawing.Size(135, 24);
            this.rbLeaderboardBestLapTime.TabIndex = 1;
            this.rbLeaderboardBestLapTime.Text = "Time in Seconds";
            this.rbLeaderboardBestLapTime.UseVisualStyleBackColor = true;
            // 
            // rbLeaderboardBestLapSpeed
            // 
            this.rbLeaderboardBestLapSpeed.AutoSize = true;
            this.rbLeaderboardBestLapSpeed.Checked = true;
            this.rbLeaderboardBestLapSpeed.Location = new System.Drawing.Point(6, 16);
            this.rbLeaderboardBestLapSpeed.Name = "rbLeaderboardBestLapSpeed";
            this.rbLeaderboardBestLapSpeed.Size = new System.Drawing.Size(130, 24);
            this.rbLeaderboardBestLapSpeed.TabIndex = 0;
            this.rbLeaderboardBestLapSpeed.TabStop = true;
            this.rbLeaderboardBestLapSpeed.Text = "Speed in M.P.H.";
            this.rbLeaderboardBestLapSpeed.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(19, 407);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(225, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "Leaderboard Best Lap displays:";
            // 
            // UserSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(677, 529);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.rbLeaderboardLastLapSpeed);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnLogDirectory);
            this.Controls.Add(this.btnBackupDirectory);
            this.Controls.Add(this.btnDataDirectory);
            this.Controls.Add(this.txtLogDirectory);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtBackupDirectory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDataDirectory);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "UserSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Settings";
            this.Load += new System.EventHandler(this.UserSettingsDialog_Load);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDataDirectory;
        private System.Windows.Forms.TextBox txtBackupDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLogDirectory;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDataDirectory;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnBackupDirectory;
        private System.Windows.Forms.Button btnLogDirectory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFastestLapsTime;
        private System.Windows.Forms.RadioButton rbFastestLapsSpeed;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbLastNLapsTime;
        private System.Windows.Forms.RadioButton rbLastNLapsSpeed;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbBestNLapsTime;
        private System.Windows.Forms.RadioButton rbBestNLapsSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbLeaderboardLastLapTime;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Label rbLeaderboardLastLapSpeed;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rbLeaderboardBestLapTime;
        private System.Windows.Forms.RadioButton rbLeaderboardBestLapSpeed;
        private System.Windows.Forms.Label label8;
    }
}