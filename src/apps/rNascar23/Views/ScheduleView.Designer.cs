namespace rNascar23.Views
{
    partial class ScheduleView
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.flpScheduledEvents = new System.Windows.Forms.FlowLayoutPanel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.lvSchedule = new System.Windows.Forms.ListView();
            this.chSpacer = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStartTimeLocal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chEventName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chNotes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlEventSchedule = new System.Windows.Forms.Panel();
            this.tabEventScheduleResults = new System.Windows.Forms.TabControl();
            this.tpSchedule = new System.Windows.Forms.TabPage();
            this.tpResults = new System.Windows.Forms.TabPage();
            this.lvResults = new System.Windows.Forms.ListView();
            this.chPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCarNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDriver = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chHometown = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chVehicle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStart = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLapsCompleted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chLapsLed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPoints = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chPlayoff = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSponsor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chOwner = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCrewChief = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlEventWinnerAndComments = new System.Windows.Forms.Panel();
            this.lblComments = new System.Windows.Forms.Label();
            this.tlpCompletedEventDetails = new System.Windows.Forms.TableLayoutPanel();
            this.lblLeaders = new System.Windows.Forms.Label();
            this.lblCautions = new System.Windows.Forms.Label();
            this.lblPoleWinner = new System.Windows.Forms.Label();
            this.lblAverageSpeed = new System.Windows.Forms.Label();
            this.lblRaceTime = new System.Windows.Forms.Label();
            this.lblWinner = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLeadChanges = new System.Windows.Forms.Label();
            this.lblCautionLaps = new System.Windows.Forms.Label();
            this.lblPoleSpeed = new System.Windows.Forms.Label();
            this.lblMargin = new System.Windows.Forms.Label();
            this.lblCarsInField = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblCompletedEventDetails = new System.Windows.Forms.Label();
            this.pnlHistoricalDataSelection = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.chkCup = new System.Windows.Forms.CheckBox();
            this.chkXfinity = new System.Windows.Forms.CheckBox();
            this.chkTruck = new System.Windows.Forms.CheckBox();
            this.btnDisplayHistoricalData = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.pnlEventSchedule.SuspendLayout();
            this.tabEventScheduleResults.SuspendLayout();
            this.tpSchedule.SuspendLayout();
            this.tpResults.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlEventWinnerAndComments.SuspendLayout();
            this.tlpCompletedEventDetails.SuspendLayout();
            this.pnlHistoricalDataSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 41);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1486, 23);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Schedule";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpScheduledEvents
            // 
            this.flpScheduledEvents.AutoScroll = true;
            this.flpScheduledEvents.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpScheduledEvents.Dock = System.Windows.Forms.DockStyle.Left;
            this.flpScheduledEvents.Location = new System.Drawing.Point(0, 64);
            this.flpScheduledEvents.Name = "flpScheduledEvents";
            this.flpScheduledEvents.Size = new System.Drawing.Size(876, 1042);
            this.flpScheduledEvents.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(876, 64);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 1042);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // lvSchedule
            // 
            this.lvSchedule.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSpacer,
            this.chStartTimeLocal,
            this.chEventName,
            this.chNotes,
            this.chDescription});
            this.lvSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSchedule.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvSchedule.HideSelection = false;
            this.lvSchedule.Location = new System.Drawing.Point(3, 3);
            this.lvSchedule.Name = "lvSchedule";
            this.lvSchedule.Size = new System.Drawing.Size(593, 556);
            this.lvSchedule.TabIndex = 3;
            this.lvSchedule.UseCompatibleStateImageBehavior = false;
            this.lvSchedule.View = System.Windows.Forms.View.Details;
            // 
            // chSpacer
            // 
            this.chSpacer.Text = "";
            this.chSpacer.Width = 25;
            // 
            // chStartTimeLocal
            // 
            this.chStartTimeLocal.Text = "Time";
            this.chStartTimeLocal.Width = 100;
            // 
            // chEventName
            // 
            this.chEventName.Text = "Event";
            this.chEventName.Width = 500;
            // 
            // chNotes
            // 
            this.chNotes.Text = "Notes";
            this.chNotes.Width = 350;
            // 
            // chDescription
            // 
            this.chDescription.Text = "On Track";
            this.chDescription.Width = 100;
            // 
            // pnlEventSchedule
            // 
            this.pnlEventSchedule.Controls.Add(this.tabEventScheduleResults);
            this.pnlEventSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventSchedule.Location = new System.Drawing.Point(0, 0);
            this.pnlEventSchedule.Name = "pnlEventSchedule";
            this.pnlEventSchedule.Size = new System.Drawing.Size(607, 596);
            this.pnlEventSchedule.TabIndex = 4;
            // 
            // tabEventScheduleResults
            // 
            this.tabEventScheduleResults.Controls.Add(this.tpSchedule);
            this.tabEventScheduleResults.Controls.Add(this.tpResults);
            this.tabEventScheduleResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabEventScheduleResults.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabEventScheduleResults.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabEventScheduleResults.ItemSize = new System.Drawing.Size(150, 26);
            this.tabEventScheduleResults.Location = new System.Drawing.Point(0, 0);
            this.tabEventScheduleResults.Name = "tabEventScheduleResults";
            this.tabEventScheduleResults.SelectedIndex = 0;
            this.tabEventScheduleResults.Size = new System.Drawing.Size(607, 596);
            this.tabEventScheduleResults.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabEventScheduleResults.TabIndex = 5;
            this.tabEventScheduleResults.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabEventScheduleResults_DrawItem);
            // 
            // tpSchedule
            // 
            this.tpSchedule.BackColor = System.Drawing.Color.White;
            this.tpSchedule.Controls.Add(this.lvSchedule);
            this.tpSchedule.Location = new System.Drawing.Point(4, 30);
            this.tpSchedule.Name = "tpSchedule";
            this.tpSchedule.Padding = new System.Windows.Forms.Padding(3);
            this.tpSchedule.Size = new System.Drawing.Size(599, 562);
            this.tpSchedule.TabIndex = 0;
            this.tpSchedule.Text = "Event Schedule";
            // 
            // tpResults
            // 
            this.tpResults.Controls.Add(this.lvResults);
            this.tpResults.Location = new System.Drawing.Point(4, 30);
            this.tpResults.Name = "tpResults";
            this.tpResults.Padding = new System.Windows.Forms.Padding(3);
            this.tpResults.Size = new System.Drawing.Size(599, 562);
            this.tpResults.TabIndex = 1;
            this.tpResults.Text = "Results";
            this.tpResults.UseVisualStyleBackColor = true;
            // 
            // lvResults
            // 
            this.lvResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chPosition,
            this.chCarNumber,
            this.chDriver,
            this.chHometown,
            this.chVehicle,
            this.chStart,
            this.chLapsCompleted,
            this.chStatus,
            this.chLapsLed,
            this.chPoints,
            this.chPlayoff,
            this.chSponsor,
            this.chOwner,
            this.chCrewChief});
            this.lvResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvResults.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvResults.HideSelection = false;
            this.lvResults.Location = new System.Drawing.Point(3, 3);
            this.lvResults.Name = "lvResults";
            this.lvResults.Size = new System.Drawing.Size(593, 556);
            this.lvResults.TabIndex = 6;
            this.lvResults.UseCompatibleStateImageBehavior = false;
            this.lvResults.View = System.Windows.Forms.View.Details;
            // 
            // chPosition
            // 
            this.chPosition.Text = "";
            this.chPosition.Width = 35;
            // 
            // chCarNumber
            // 
            this.chCarNumber.Text = "#";
            this.chCarNumber.Width = 35;
            // 
            // chDriver
            // 
            this.chDriver.Text = "Driver";
            this.chDriver.Width = 150;
            // 
            // chHometown
            // 
            this.chHometown.Text = "Hometown";
            this.chHometown.Width = 100;
            // 
            // chVehicle
            // 
            this.chVehicle.Text = "Vehicle";
            this.chVehicle.Width = 80;
            // 
            // chStart
            // 
            this.chStart.Text = "Start";
            this.chStart.Width = 45;
            // 
            // chLapsCompleted
            // 
            this.chLapsCompleted.Text = "Laps";
            this.chLapsCompleted.Width = 45;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            this.chStatus.Width = 70;
            // 
            // chLapsLed
            // 
            this.chLapsLed.Text = "Led";
            this.chLapsLed.Width = 40;
            // 
            // chPoints
            // 
            this.chPoints.Text = "Pts";
            this.chPoints.Width = 35;
            // 
            // chPlayoff
            // 
            this.chPlayoff.Text = "Playoff";
            // 
            // chSponsor
            // 
            this.chSponsor.Text = "Sponsor";
            this.chSponsor.Width = 125;
            // 
            // chOwner
            // 
            this.chOwner.Text = "Owner";
            this.chOwner.Width = 125;
            // 
            // chCrewChief
            // 
            this.chCrewChief.Text = "CrewChief";
            this.chCrewChief.Width = 125;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlEventWinnerAndComments);
            this.pnlRight.Controls.Add(this.pnlEventSchedule);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(879, 64);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(607, 1042);
            this.pnlRight.TabIndex = 5;
            // 
            // pnlEventWinnerAndComments
            // 
            this.pnlEventWinnerAndComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEventWinnerAndComments.Controls.Add(this.lblComments);
            this.pnlEventWinnerAndComments.Controls.Add(this.tlpCompletedEventDetails);
            this.pnlEventWinnerAndComments.Controls.Add(this.lblCompletedEventDetails);
            this.pnlEventWinnerAndComments.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventWinnerAndComments.Location = new System.Drawing.Point(0, 596);
            this.pnlEventWinnerAndComments.Name = "pnlEventWinnerAndComments";
            this.pnlEventWinnerAndComments.Size = new System.Drawing.Size(607, 285);
            this.pnlEventWinnerAndComments.TabIndex = 8;
            this.pnlEventWinnerAndComments.Visible = false;
            // 
            // lblComments
            // 
            this.lblComments.BackColor = System.Drawing.Color.White;
            this.lblComments.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblComments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblComments.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComments.Location = new System.Drawing.Point(0, 165);
            this.lblComments.Name = "lblComments";
            this.lblComments.Size = new System.Drawing.Size(605, 118);
            this.lblComments.TabIndex = 7;
            // 
            // tlpCompletedEventDetails
            // 
            this.tlpCompletedEventDetails.ColumnCount = 4;
            this.tlpCompletedEventDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpCompletedEventDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpCompletedEventDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpCompletedEventDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpCompletedEventDetails.Controls.Add(this.lblLeaders, 1, 1);
            this.tlpCompletedEventDetails.Controls.Add(this.lblCautions, 1, 2);
            this.tlpCompletedEventDetails.Controls.Add(this.lblPoleWinner, 1, 3);
            this.tlpCompletedEventDetails.Controls.Add(this.lblAverageSpeed, 1, 4);
            this.tlpCompletedEventDetails.Controls.Add(this.lblRaceTime, 1, 5);
            this.tlpCompletedEventDetails.Controls.Add(this.lblWinner, 1, 0);
            this.tlpCompletedEventDetails.Controls.Add(this.label1, 0, 0);
            this.tlpCompletedEventDetails.Controls.Add(this.label2, 0, 1);
            this.tlpCompletedEventDetails.Controls.Add(this.label3, 0, 2);
            this.tlpCompletedEventDetails.Controls.Add(this.label4, 0, 3);
            this.tlpCompletedEventDetails.Controls.Add(this.label5, 0, 4);
            this.tlpCompletedEventDetails.Controls.Add(this.label6, 0, 5);
            this.tlpCompletedEventDetails.Controls.Add(this.lblLeadChanges, 3, 1);
            this.tlpCompletedEventDetails.Controls.Add(this.lblCautionLaps, 3, 2);
            this.tlpCompletedEventDetails.Controls.Add(this.lblPoleSpeed, 3, 3);
            this.tlpCompletedEventDetails.Controls.Add(this.lblMargin, 3, 4);
            this.tlpCompletedEventDetails.Controls.Add(this.lblCarsInField, 3, 5);
            this.tlpCompletedEventDetails.Controls.Add(this.label7, 2, 1);
            this.tlpCompletedEventDetails.Controls.Add(this.label8, 2, 2);
            this.tlpCompletedEventDetails.Controls.Add(this.label9, 2, 3);
            this.tlpCompletedEventDetails.Controls.Add(this.label10, 2, 4);
            this.tlpCompletedEventDetails.Controls.Add(this.label11, 2, 5);
            this.tlpCompletedEventDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpCompletedEventDetails.Location = new System.Drawing.Point(0, 23);
            this.tlpCompletedEventDetails.Name = "tlpCompletedEventDetails";
            this.tlpCompletedEventDetails.RowCount = 6;
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tlpCompletedEventDetails.Size = new System.Drawing.Size(605, 142);
            this.tlpCompletedEventDetails.TabIndex = 7;
            // 
            // lblLeaders
            // 
            this.lblLeaders.AutoSize = true;
            this.lblLeaders.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeaders.Location = new System.Drawing.Point(126, 22);
            this.lblLeaders.Name = "lblLeaders";
            this.lblLeaders.Size = new System.Drawing.Size(71, 21);
            this.lblLeaders.TabIndex = 8;
            this.lblLeaders.Text = "Leaders:";
            // 
            // lblCautions
            // 
            this.lblCautions.AutoSize = true;
            this.lblCautions.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCautions.Location = new System.Drawing.Point(126, 44);
            this.lblCautions.Name = "lblCautions";
            this.lblCautions.Size = new System.Drawing.Size(77, 21);
            this.lblCautions.TabIndex = 12;
            this.lblCautions.Text = "Cautions:";
            // 
            // lblPoleWinner
            // 
            this.lblPoleWinner.AutoSize = true;
            this.lblPoleWinner.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoleWinner.Location = new System.Drawing.Point(126, 66);
            this.lblPoleWinner.Name = "lblPoleWinner";
            this.lblPoleWinner.Size = new System.Drawing.Size(102, 21);
            this.lblPoleWinner.TabIndex = 10;
            this.lblPoleWinner.Text = "Pole Winner:";
            // 
            // lblAverageSpeed
            // 
            this.lblAverageSpeed.AutoSize = true;
            this.lblAverageSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAverageSpeed.Location = new System.Drawing.Point(126, 88);
            this.lblAverageSpeed.Name = "lblAverageSpeed";
            this.lblAverageSpeed.Size = new System.Drawing.Size(126, 21);
            this.lblAverageSpeed.TabIndex = 13;
            this.lblAverageSpeed.Text = "Average Speed:";
            // 
            // lblRaceTime
            // 
            this.lblRaceTime.AutoSize = true;
            this.lblRaceTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRaceTime.Location = new System.Drawing.Point(126, 110);
            this.lblRaceTime.Name = "lblRaceTime";
            this.lblRaceTime.Size = new System.Drawing.Size(89, 21);
            this.lblRaceTime.TabIndex = 15;
            this.lblRaceTime.Text = "Race Time:";
            // 
            // lblWinner
            // 
            this.lblWinner.AutoSize = true;
            this.tlpCompletedEventDetails.SetColumnSpan(this.lblWinner, 2);
            this.lblWinner.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWinner.Location = new System.Drawing.Point(126, 0);
            this.lblWinner.Name = "lblWinner";
            this.lblWinner.Size = new System.Drawing.Size(67, 21);
            this.lblWinner.TabIndex = 5;
            this.lblWinner.Text = "Winner:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 21);
            this.label1.TabIndex = 16;
            this.label1.Text = "Winner:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 21);
            this.label2.TabIndex = 17;
            this.label2.Text = "Leaders:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 21);
            this.label3.TabIndex = 18;
            this.label3.Text = "Cautions:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 21);
            this.label4.TabIndex = 19;
            this.label4.Text = "Pole Winner:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 21);
            this.label5.TabIndex = 20;
            this.label5.Text = "Average Speed:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 21);
            this.label6.TabIndex = 21;
            this.label6.Text = "Race Time:";
            // 
            // lblLeadChanges
            // 
            this.lblLeadChanges.AutoSize = true;
            this.lblLeadChanges.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLeadChanges.Location = new System.Drawing.Point(466, 22);
            this.lblLeadChanges.Name = "lblLeadChanges";
            this.lblLeadChanges.Size = new System.Drawing.Size(115, 21);
            this.lblLeadChanges.TabIndex = 14;
            this.lblLeadChanges.Text = "Lead Changes:";
            // 
            // lblCautionLaps
            // 
            this.lblCautionLaps.AutoSize = true;
            this.lblCautionLaps.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCautionLaps.Location = new System.Drawing.Point(466, 44);
            this.lblCautionLaps.Name = "lblCautionLaps";
            this.lblCautionLaps.Size = new System.Drawing.Size(107, 21);
            this.lblCautionLaps.TabIndex = 7;
            this.lblCautionLaps.Text = "Caution Laps:";
            // 
            // lblPoleSpeed
            // 
            this.lblPoleSpeed.AutoSize = true;
            this.lblPoleSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoleSpeed.Location = new System.Drawing.Point(466, 66);
            this.lblPoleSpeed.Name = "lblPoleSpeed";
            this.lblPoleSpeed.Size = new System.Drawing.Size(96, 21);
            this.lblPoleSpeed.TabIndex = 11;
            this.lblPoleSpeed.Text = "Pole Speed:";
            // 
            // lblMargin
            // 
            this.lblMargin.AutoSize = true;
            this.lblMargin.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMargin.Location = new System.Drawing.Point(466, 88);
            this.lblMargin.Name = "lblMargin";
            this.lblMargin.Size = new System.Drawing.Size(143, 21);
            this.lblMargin.TabIndex = 6;
            this.lblMargin.Text = "Margin of Victory:";
            // 
            // lblCarsInField
            // 
            this.lblCarsInField.AutoSize = true;
            this.lblCarsInField.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarsInField.Location = new System.Drawing.Point(466, 110);
            this.lblCarsInField.Name = "lblCarsInField";
            this.lblCarsInField.Size = new System.Drawing.Size(101, 21);
            this.lblCarsInField.TabIndex = 9;
            this.lblCarsInField.Text = "Cars in Field:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(326, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 21);
            this.label7.TabIndex = 22;
            this.label7.Text = "Lead Changes:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(326, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(103, 21);
            this.label8.TabIndex = 23;
            this.label8.Text = "Caution Laps:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(326, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 21);
            this.label9.TabIndex = 24;
            this.label9.Text = "Pole Speed:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(326, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(134, 21);
            this.label10.TabIndex = 25;
            this.label10.Text = "Margin of Victory:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(326, 110);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(98, 21);
            this.label11.TabIndex = 26;
            this.label11.Text = "Cars in Field:";
            // 
            // lblCompletedEventDetails
            // 
            this.lblCompletedEventDetails.BackColor = System.Drawing.Color.SteelBlue;
            this.lblCompletedEventDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCompletedEventDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCompletedEventDetails.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCompletedEventDetails.ForeColor = System.Drawing.Color.Silver;
            this.lblCompletedEventDetails.Location = new System.Drawing.Point(0, 0);
            this.lblCompletedEventDetails.Name = "lblCompletedEventDetails";
            this.lblCompletedEventDetails.Size = new System.Drawing.Size(605, 23);
            this.lblCompletedEventDetails.TabIndex = 9;
            this.lblCompletedEventDetails.Text = "Event Details";
            this.lblCompletedEventDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHistoricalDataSelection
            // 
            this.pnlHistoricalDataSelection.Controls.Add(this.label13);
            this.pnlHistoricalDataSelection.Controls.Add(this.btnDisplayHistoricalData);
            this.pnlHistoricalDataSelection.Controls.Add(this.chkTruck);
            this.pnlHistoricalDataSelection.Controls.Add(this.chkXfinity);
            this.pnlHistoricalDataSelection.Controls.Add(this.chkCup);
            this.pnlHistoricalDataSelection.Controls.Add(this.cboYear);
            this.pnlHistoricalDataSelection.Controls.Add(this.label12);
            this.pnlHistoricalDataSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHistoricalDataSelection.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlHistoricalDataSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlHistoricalDataSelection.Name = "pnlHistoricalDataSelection";
            this.pnlHistoricalDataSelection.Size = new System.Drawing.Size(1486, 41);
            this.pnlHistoricalDataSelection.TabIndex = 0;
            this.pnlHistoricalDataSelection.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(156, 20);
            this.label12.TabIndex = 0;
            this.label12.Text = "Select Year and Series:";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Location = new System.Drawing.Point(165, 8);
            this.cboYear.Name = "cboYear";
            this.cboYear.Size = new System.Drawing.Size(121, 28);
            this.cboYear.TabIndex = 1;
            // 
            // chkCup
            // 
            this.chkCup.AutoSize = true;
            this.chkCup.Location = new System.Drawing.Point(292, 10);
            this.chkCup.Name = "chkCup";
            this.chkCup.Size = new System.Drawing.Size(54, 24);
            this.chkCup.TabIndex = 2;
            this.chkCup.Text = "Cup";
            this.chkCup.UseVisualStyleBackColor = true;
            this.chkCup.CheckStateChanged += new System.EventHandler(this.seriesSelection_CheckChanged);
            // 
            // chkXfinity
            // 
            this.chkXfinity.AutoSize = true;
            this.chkXfinity.Location = new System.Drawing.Point(352, 10);
            this.chkXfinity.Name = "chkXfinity";
            this.chkXfinity.Size = new System.Drawing.Size(70, 24);
            this.chkXfinity.TabIndex = 3;
            this.chkXfinity.Text = "Xfinity";
            this.chkXfinity.UseVisualStyleBackColor = true;
            this.chkXfinity.CheckStateChanged += new System.EventHandler(this.seriesSelection_CheckChanged);
            // 
            // chkTruck
            // 
            this.chkTruck.AutoSize = true;
            this.chkTruck.Location = new System.Drawing.Point(428, 10);
            this.chkTruck.Name = "chkTruck";
            this.chkTruck.Size = new System.Drawing.Size(68, 24);
            this.chkTruck.TabIndex = 4;
            this.chkTruck.Text = "Trucks";
            this.chkTruck.UseVisualStyleBackColor = true;
            this.chkTruck.CheckStateChanged += new System.EventHandler(this.seriesSelection_CheckChanged);
            // 
            // btnDisplayHistoricalData
            // 
            this.btnDisplayHistoricalData.Enabled = false;
            this.btnDisplayHistoricalData.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayHistoricalData.Location = new System.Drawing.Point(502, 8);
            this.btnDisplayHistoricalData.Name = "btnDisplayHistoricalData";
            this.btnDisplayHistoricalData.Size = new System.Drawing.Size(75, 28);
            this.btnDisplayHistoricalData.TabIndex = 5;
            this.btnDisplayHistoricalData.Text = "Display";
            this.btnDisplayHistoricalData.UseVisualStyleBackColor = true;
            this.btnDisplayHistoricalData.Click += new System.EventHandler(this.btnDisplayHistoricalData_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(641, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(319, 17);
            this.label13.TabIndex = 6;
            this.label13.Text = "Not all races have data for event schedules or results";
            // 
            // ScheduleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.flpScheduledEvents);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlHistoricalDataSelection);
            this.DoubleBuffered = true;
            this.Name = "ScheduleView";
            this.Size = new System.Drawing.Size(1486, 1106);
            this.Load += new System.EventHandler(this.ScheduleView_Load);
            this.pnlEventSchedule.ResumeLayout(false);
            this.tabEventScheduleResults.ResumeLayout(false);
            this.tpSchedule.ResumeLayout(false);
            this.tpResults.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlEventWinnerAndComments.ResumeLayout(false);
            this.tlpCompletedEventDetails.ResumeLayout(false);
            this.tlpCompletedEventDetails.PerformLayout();
            this.pnlHistoricalDataSelection.ResumeLayout(false);
            this.pnlHistoricalDataSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel flpScheduledEvents;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ListView lvSchedule;
        private System.Windows.Forms.ColumnHeader chStartTimeLocal;
        private System.Windows.Forms.ColumnHeader chNotes;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ColumnHeader chEventName;
        private System.Windows.Forms.ColumnHeader chSpacer;
        private System.Windows.Forms.Panel pnlEventSchedule;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblComments;
        private System.Windows.Forms.Label lblMargin;
        private System.Windows.Forms.Label lblWinner;
        private System.Windows.Forms.Panel pnlEventWinnerAndComments;
        private System.Windows.Forms.TableLayoutPanel tlpCompletedEventDetails;
        private System.Windows.Forms.Label lblCautions;
        private System.Windows.Forms.Label lblLeadChanges;
        private System.Windows.Forms.Label lblPoleWinner;
        private System.Windows.Forms.Label lblAverageSpeed;
        private System.Windows.Forms.Label lblLeaders;
        private System.Windows.Forms.Label lblCautionLaps;
        private System.Windows.Forms.Label lblPoleSpeed;
        private System.Windows.Forms.Label lblCarsInField;
        private System.Windows.Forms.Label lblRaceTime;
        private System.Windows.Forms.Label lblCompletedEventDetails;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabControl tabEventScheduleResults;
        private System.Windows.Forms.TabPage tpSchedule;
        private System.Windows.Forms.TabPage tpResults;
        private System.Windows.Forms.ListView lvResults;
        private System.Windows.Forms.ColumnHeader chPosition;
        private System.Windows.Forms.ColumnHeader chDriver;
        private System.Windows.Forms.ColumnHeader chCarNumber;
        private System.Windows.Forms.ColumnHeader chHometown;
        private System.Windows.Forms.ColumnHeader chVehicle;
        private System.Windows.Forms.ColumnHeader chStart;
        private System.Windows.Forms.ColumnHeader chLapsCompleted;
        private System.Windows.Forms.ColumnHeader chPoints;
        private System.Windows.Forms.ColumnHeader chPlayoff;
        private System.Windows.Forms.ColumnHeader chOwner;
        private System.Windows.Forms.ColumnHeader chSponsor;
        private System.Windows.Forms.ColumnHeader chCrewChief;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chLapsLed;
        private System.Windows.Forms.Panel pnlHistoricalDataSelection;
        private System.Windows.Forms.Button btnDisplayHistoricalData;
        private System.Windows.Forms.CheckBox chkTruck;
        private System.Windows.Forms.CheckBox chkXfinity;
        private System.Windows.Forms.CheckBox chkCup;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
    }
}
