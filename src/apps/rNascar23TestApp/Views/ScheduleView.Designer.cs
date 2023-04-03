namespace rNascar23TestApp.Views
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
            this.lblEventSchedule = new System.Windows.Forms.Label();
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
            this.pnlEventSchedule.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.pnlEventWinnerAndComments.SuspendLayout();
            this.tlpCompletedEventDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
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
            this.flpScheduledEvents.Location = new System.Drawing.Point(0, 23);
            this.flpScheduledEvents.Name = "flpScheduledEvents";
            this.flpScheduledEvents.Size = new System.Drawing.Size(876, 1083);
            this.flpScheduledEvents.TabIndex = 1;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(876, 23);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 1083);
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
            this.lvSchedule.Location = new System.Drawing.Point(0, 23);
            this.lvSchedule.Name = "lvSchedule";
            this.lvSchedule.Size = new System.Drawing.Size(607, 573);
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
            this.pnlEventSchedule.Controls.Add(this.lvSchedule);
            this.pnlEventSchedule.Controls.Add(this.lblEventSchedule);
            this.pnlEventSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEventSchedule.Location = new System.Drawing.Point(0, 0);
            this.pnlEventSchedule.Name = "pnlEventSchedule";
            this.pnlEventSchedule.Size = new System.Drawing.Size(607, 596);
            this.pnlEventSchedule.TabIndex = 4;
            // 
            // lblEventSchedule
            // 
            this.lblEventSchedule.BackColor = System.Drawing.Color.SteelBlue;
            this.lblEventSchedule.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblEventSchedule.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblEventSchedule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventSchedule.ForeColor = System.Drawing.Color.Silver;
            this.lblEventSchedule.Location = new System.Drawing.Point(0, 0);
            this.lblEventSchedule.Name = "lblEventSchedule";
            this.lblEventSchedule.Size = new System.Drawing.Size(607, 23);
            this.lblEventSchedule.TabIndex = 4;
            this.lblEventSchedule.Text = "Event Schedule";
            this.lblEventSchedule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.pnlEventWinnerAndComments);
            this.pnlRight.Controls.Add(this.pnlEventSchedule);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(879, 23);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(607, 1083);
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
            // ScheduleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.flpScheduledEvents);
            this.Controls.Add(this.lblTitle);
            this.Name = "ScheduleView";
            this.Size = new System.Drawing.Size(1486, 1106);
            this.Load += new System.EventHandler(this.ScheduleView_Load);
            this.pnlEventSchedule.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            this.pnlEventWinnerAndComments.ResumeLayout(false);
            this.tlpCompletedEventDetails.ResumeLayout(false);
            this.tlpCompletedEventDetails.PerformLayout();
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
        private System.Windows.Forms.Label lblEventSchedule;
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
    }
}
