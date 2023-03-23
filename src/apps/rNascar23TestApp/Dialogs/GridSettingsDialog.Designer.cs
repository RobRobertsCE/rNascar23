namespace rNascar23TestApp.Dialogs
{
    partial class GridSettingsDialog
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
            this.pnlDialogButtons = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnCancelClose = new System.Windows.Forms.Button();
            this.btnEditSave = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.pnlGridTitle = new System.Windows.Forms.Panel();
            this.txtGridTitle = new System.Windows.Forms.TextBox();
            this.btnTitleForeColor = new System.Windows.Forms.Button();
            this.btnTitleBackColor = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.lstFields = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDisplayIndex = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkColumnVisible = new System.Windows.Forms.CheckBox();
            this.txtColumnWidth = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDataPropertyName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtColumnHeader = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSelector = new System.Windows.Forms.Panel();
            this.cboGridSelection = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlGridSettings = new System.Windows.Forms.Panel();
            this.txtDisplayOrder = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboLocation = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtGridWidth = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkHideRowSelectors = new System.Windows.Forms.CheckBox();
            this.cboDataSource = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGridName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblIndex = new System.Windows.Forms.Label();
            this.cboSortBy = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSortNone = new System.Windows.Forms.RadioButton();
            this.rbSortDescending = new System.Windows.Forms.RadioButton();
            this.rbSortAscending = new System.Windows.Forms.RadioButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlDialogButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.pnlGridTitle.SuspendLayout();
            this.pnlFields.SuspendLayout();
            this.pnlSelector.SuspendLayout();
            this.pnlGridSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDialogButtons
            // 
            this.pnlDialogButtons.Controls.Add(this.btnDelete);
            this.pnlDialogButtons.Controls.Add(this.btnCopy);
            this.pnlDialogButtons.Controls.Add(this.btnNew);
            this.pnlDialogButtons.Controls.Add(this.btnCancelClose);
            this.pnlDialogButtons.Controls.Add(this.btnEditSave);
            this.pnlDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDialogButtons.Location = new System.Drawing.Point(0, 574);
            this.pnlDialogButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDialogButtons.Name = "pnlDialogButtons";
            this.pnlDialogButtons.Size = new System.Drawing.Size(1134, 37);
            this.pnlDialogButtons.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(255, 5);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 28);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnDelete, "Permanently deletes the selected grid.");
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(174, 5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 28);
            this.btnCopy.TabIndex = 3;
            this.btnCopy.Text = "Copy";
            this.toolTip1.SetToolTip(this.btnCopy, "Makes a copy of the selected grid and opens it to be edited.");
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnNew
            // 
            this.btnNew.Location = new System.Drawing.Point(93, 5);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 28);
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.toolTip1.SetToolTip(this.btnNew, "Creates a new grid and opens it to be edited.");
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnCancelClose
            // 
            this.btnCancelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelClose.Location = new System.Drawing.Point(1047, 6);
            this.btnCancelClose.Name = "btnCancelClose";
            this.btnCancelClose.Size = new System.Drawing.Size(75, 28);
            this.btnCancelClose.TabIndex = 1;
            this.btnCancelClose.Text = "Close";
            this.btnCancelClose.UseVisualStyleBackColor = true;
            this.btnCancelClose.Click += new System.EventHandler(this.btnCancelClose_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Location = new System.Drawing.Point(12, 6);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(75, 28);
            this.btnEditSave.TabIndex = 0;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.btnEditSave_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 244);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView);
            this.splitContainer1.Panel1.Controls.Add(this.pnlGridTitle);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.pnlFields);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblIndex);
            this.splitContainer1.Panel2.Controls.Add(this.label13);
            this.splitContainer1.Panel2.Controls.Add(this.txtDisplayIndex);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.chkColumnVisible);
            this.splitContainer1.Panel2.Controls.Add(this.txtColumnWidth);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.txtDataPropertyName);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.txtColumnHeader);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1134, 330);
            this.splitContainer1.SplitterDistance = 819;
            this.splitContainer1.TabIndex = 2;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(138, 25);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(679, 303);
            this.dataGridView.TabIndex = 1;
            this.toolTip1.SetToolTip(this.dataGridView, "The example of how the grid will look.\r\n\r\nClick a column header to edit the colum" +
        "n.\r\n");
            this.dataGridView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_ColumnWidthChanged);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // pnlGridTitle
            // 
            this.pnlGridTitle.Controls.Add(this.txtGridTitle);
            this.pnlGridTitle.Controls.Add(this.btnTitleForeColor);
            this.pnlGridTitle.Controls.Add(this.btnTitleBackColor);
            this.pnlGridTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGridTitle.Location = new System.Drawing.Point(138, 0);
            this.pnlGridTitle.Name = "pnlGridTitle";
            this.pnlGridTitle.Size = new System.Drawing.Size(679, 25);
            this.pnlGridTitle.TabIndex = 0;
            // 
            // txtGridTitle
            // 
            this.txtGridTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGridTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGridTitle.Location = new System.Drawing.Point(0, 0);
            this.txtGridTitle.Name = "txtGridTitle";
            this.txtGridTitle.Size = new System.Drawing.Size(631, 25);
            this.txtGridTitle.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtGridTitle, "The title of the grid. (Editable)");
            // 
            // btnTitleForeColor
            // 
            this.btnTitleForeColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTitleForeColor.Location = new System.Drawing.Point(631, 0);
            this.btnTitleForeColor.Name = "btnTitleForeColor";
            this.btnTitleForeColor.Size = new System.Drawing.Size(24, 25);
            this.btnTitleForeColor.TabIndex = 1;
            this.btnTitleForeColor.Text = "F";
            this.toolTip1.SetToolTip(this.btnTitleForeColor, "Sets the grid title text color");
            this.btnTitleForeColor.UseVisualStyleBackColor = true;
            this.btnTitleForeColor.Click += new System.EventHandler(this.btnTitleForeColor_Click);
            // 
            // btnTitleBackColor
            // 
            this.btnTitleBackColor.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTitleBackColor.Location = new System.Drawing.Point(655, 0);
            this.btnTitleBackColor.Name = "btnTitleBackColor";
            this.btnTitleBackColor.Size = new System.Drawing.Size(24, 25);
            this.btnTitleBackColor.TabIndex = 0;
            this.btnTitleBackColor.Text = "B";
            this.toolTip1.SetToolTip(this.btnTitleBackColor, "Sets the grid title background color");
            this.btnTitleBackColor.UseVisualStyleBackColor = true;
            this.btnTitleBackColor.Click += new System.EventHandler(this.btnTitleBackColor_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(135, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 328);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.lstFields);
            this.pnlFields.Controls.Add(this.label4);
            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFields.Location = new System.Drawing.Point(0, 0);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(135, 328);
            this.pnlFields.TabIndex = 2;
            // 
            // lstFields
            // 
            this.lstFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstFields.FormattingEnabled = true;
            this.lstFields.ItemHeight = 17;
            this.lstFields.Location = new System.Drawing.Point(0, 25);
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(135, 303);
            this.lstFields.TabIndex = 1;
            this.toolTip1.SetToolTip(this.lstFields, "List of all the available fields from the selected data source. Click to select.");
            this.lstFields.SelectedValueChanged += new System.EventHandler(this.lstFields_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Fields";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip1.SetToolTip(this.label4, "List of all the available fields from the selected data source");
            // 
            // txtDisplayIndex
            // 
            this.txtDisplayIndex.Location = new System.Drawing.Point(18, 176);
            this.txtDisplayIndex.Name = "txtDisplayIndex";
            this.txtDisplayIndex.Size = new System.Drawing.Size(45, 25);
            this.txtDisplayIndex.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtDisplayIndex, "The order that the column appears in the grid.");
            this.txtDisplayIndex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDisplayIndex_KeyUp);
            this.txtDisplayIndex.Leave += new System.EventHandler(this.txtDisplayIndex_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Column Display Index";
            // 
            // chkColumnVisible
            // 
            this.chkColumnVisible.AutoSize = true;
            this.chkColumnVisible.Location = new System.Drawing.Point(140, 129);
            this.chkColumnVisible.Name = "chkColumnVisible";
            this.chkColumnVisible.Size = new System.Drawing.Size(65, 21);
            this.chkColumnVisible.TabIndex = 6;
            this.chkColumnVisible.Text = "Visible";
            this.toolTip1.SetToolTip(this.chkColumnVisible, "Show or hide this column.");
            this.chkColumnVisible.UseVisualStyleBackColor = true;
            this.chkColumnVisible.CheckedChanged += new System.EventHandler(this.chkColumnVisible_CheckedChanged);
            // 
            // txtColumnWidth
            // 
            this.txtColumnWidth.Location = new System.Drawing.Point(18, 127);
            this.txtColumnWidth.Name = "txtColumnWidth";
            this.txtColumnWidth.Size = new System.Drawing.Size(87, 25);
            this.txtColumnWidth.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtColumnWidth, "The width of the column (if visible)");
            this.txtColumnWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtColumnWidth_KeyUp);
            this.txtColumnWidth.Leave += new System.EventHandler(this.txtColumnWidth_Leave);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Column Width";
            // 
            // txtDataPropertyName
            // 
            this.txtDataPropertyName.BackColor = System.Drawing.Color.Silver;
            this.txtDataPropertyName.Location = new System.Drawing.Point(18, 29);
            this.txtDataPropertyName.Name = "txtDataPropertyName";
            this.txtDataPropertyName.ReadOnly = true;
            this.txtDataPropertyName.Size = new System.Drawing.Size(187, 25);
            this.txtDataPropertyName.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtDataPropertyName, "The field name in the data source. (Not editable)");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Field Name";
            // 
            // txtColumnHeader
            // 
            this.txtColumnHeader.Location = new System.Drawing.Point(18, 78);
            this.txtColumnHeader.Name = "txtColumnHeader";
            this.txtColumnHeader.Size = new System.Drawing.Size(187, 25);
            this.txtColumnHeader.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtColumnHeader, "The text at the top of this column on the grid.");
            this.txtColumnHeader.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtColumnHeader_KeyUp);
            this.txtColumnHeader.Leave += new System.EventHandler(this.txtColumnHeader_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Column Header Text";
            // 
            // pnlSelector
            // 
            this.pnlSelector.Controls.Add(this.cboGridSelection);
            this.pnlSelector.Controls.Add(this.label6);
            this.pnlSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelector.Location = new System.Drawing.Point(0, 0);
            this.pnlSelector.Name = "pnlSelector";
            this.pnlSelector.Size = new System.Drawing.Size(1134, 71);
            this.pnlSelector.TabIndex = 3;
            // 
            // cboGridSelection
            // 
            this.cboGridSelection.FormattingEnabled = true;
            this.cboGridSelection.Location = new System.Drawing.Point(8, 29);
            this.cboGridSelection.Name = "cboGridSelection";
            this.cboGridSelection.Size = new System.Drawing.Size(324, 25);
            this.cboGridSelection.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cboGridSelection, "Select a grid to view or edit.");
            this.cboGridSelection.SelectedIndexChanged += new System.EventHandler(this.cboGridSelection_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Select a Grid";
            // 
            // pnlGridSettings
            // 
            this.pnlGridSettings.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGridSettings.Controls.Add(this.groupBox1);
            this.pnlGridSettings.Controls.Add(this.cboSortBy);
            this.pnlGridSettings.Controls.Add(this.label14);
            this.pnlGridSettings.Controls.Add(this.txtDisplayOrder);
            this.pnlGridSettings.Controls.Add(this.label12);
            this.pnlGridSettings.Controls.Add(this.cboLocation);
            this.pnlGridSettings.Controls.Add(this.label11);
            this.pnlGridSettings.Controls.Add(this.txtGridWidth);
            this.pnlGridSettings.Controls.Add(this.label10);
            this.pnlGridSettings.Controls.Add(this.chkHideRowSelectors);
            this.pnlGridSettings.Controls.Add(this.cboDataSource);
            this.pnlGridSettings.Controls.Add(this.label9);
            this.pnlGridSettings.Controls.Add(this.txtDescription);
            this.pnlGridSettings.Controls.Add(this.label8);
            this.pnlGridSettings.Controls.Add(this.txtGridName);
            this.pnlGridSettings.Controls.Add(this.label7);
            this.pnlGridSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGridSettings.Location = new System.Drawing.Point(0, 71);
            this.pnlGridSettings.Name = "pnlGridSettings";
            this.pnlGridSettings.Size = new System.Drawing.Size(1134, 173);
            this.pnlGridSettings.TabIndex = 4;
            // 
            // txtDisplayOrder
            // 
            this.txtDisplayOrder.Location = new System.Drawing.Point(518, 70);
            this.txtDisplayOrder.Name = "txtDisplayOrder";
            this.txtDisplayOrder.Size = new System.Drawing.Size(119, 25);
            this.txtDisplayOrder.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtDisplayOrder, "The order the grid is displayed in withiin the selected grid location.");
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(519, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 17);
            this.label12.TabIndex = 11;
            this.label12.Text = "Grid Display Index";
            // 
            // cboLocation
            // 
            this.cboLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLocation.FormattingEnabled = true;
            this.cboLocation.Items.AddRange(new object[] {
            "Main",
            "Right",
            "Bottom"});
            this.cboLocation.Location = new System.Drawing.Point(518, 22);
            this.cboLocation.Name = "cboLocation";
            this.cboLocation.Size = new System.Drawing.Size(175, 25);
            this.cboLocation.TabIndex = 10;
            this.toolTip1.SetToolTip(this.cboLocation, "The panel that the grid appears in on the form.");
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(519, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 17);
            this.label11.TabIndex = 9;
            this.label11.Text = "Location";
            // 
            // txtGridWidth
            // 
            this.txtGridWidth.Location = new System.Drawing.Point(337, 70);
            this.txtGridWidth.Name = "txtGridWidth";
            this.txtGridWidth.Size = new System.Drawing.Size(119, 25);
            this.txtGridWidth.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtGridWidth, "The width of the grid on the form.");
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(338, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 17);
            this.label10.TabIndex = 7;
            this.label10.Text = "Grid Width";
            // 
            // chkHideRowSelectors
            // 
            this.chkHideRowSelectors.AutoSize = true;
            this.chkHideRowSelectors.Location = new System.Drawing.Point(341, 130);
            this.chkHideRowSelectors.Name = "chkHideRowSelectors";
            this.chkHideRowSelectors.Size = new System.Drawing.Size(140, 21);
            this.chkHideRowSelectors.TabIndex = 6;
            this.chkHideRowSelectors.Text = "Hide Row Selectors";
            this.toolTip1.SetToolTip(this.chkHideRowSelectors, "Hide the first column in the grid, which is the row selector.");
            this.chkHideRowSelectors.UseVisualStyleBackColor = true;
            // 
            // cboDataSource
            // 
            this.cboDataSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataSource.FormattingEnabled = true;
            this.cboDataSource.Items.AddRange(new object[] {
            "Driver Statistics",
            "Flags",
            "Lap Times",
            "Live Feed",
            "Race Schedules"});
            this.cboDataSource.Location = new System.Drawing.Point(337, 22);
            this.cboDataSource.Name = "cboDataSource";
            this.cboDataSource.Size = new System.Drawing.Size(175, 25);
            this.cboDataSource.TabIndex = 5;
            this.toolTip1.SetToolTip(this.cboDataSource, "The data source used to populate the grid. ");
            this.cboDataSource.SelectedIndexChanged += new System.EventHandler(this.cboDataSource_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(338, 2);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 17);
            this.label9.TabIndex = 4;
            this.label9.Text = "Data Source";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(7, 70);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(324, 81);
            this.txtDescription.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtDescription, "A description of the grid.");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 17);
            this.label8.TabIndex = 2;
            this.label8.Text = "Description";
            // 
            // txtGridName
            // 
            this.txtGridName.Location = new System.Drawing.Point(7, 22);
            this.txtGridName.Name = "txtGridName";
            this.txtGridName.Size = new System.Drawing.Size(324, 25);
            this.txtGridName.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtGridName, "The name of the grid.");
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 2);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 17);
            this.label7.TabIndex = 0;
            this.label7.Text = "Grid Name";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(211, 8);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 17);
            this.label13.TabIndex = 9;
            this.label13.Text = "Index";
            // 
            // lblIndex
            // 
            this.lblIndex.AutoSize = true;
            this.lblIndex.Location = new System.Drawing.Point(211, 32);
            this.lblIndex.Name = "lblIndex";
            this.lblIndex.Size = new System.Drawing.Size(15, 17);
            this.lblIndex.TabIndex = 10;
            this.lblIndex.Text = "0";
            this.toolTip1.SetToolTip(this.lblIndex, "The oder that the field appears in the data source. (Not editable)");
            // 
            // cboSortBy
            // 
            this.cboSortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSortBy.FormattingEnabled = true;
            this.cboSortBy.Items.AddRange(new object[] {
            "Main",
            "Right",
            "Bottom"});
            this.cboSortBy.Location = new System.Drawing.Point(699, 22);
            this.cboSortBy.Name = "cboSortBy";
            this.cboSortBy.Size = new System.Drawing.Size(175, 25);
            this.cboSortBy.TabIndex = 14;
            this.toolTip1.SetToolTip(this.cboSortBy, "The column the data in the grid is sorted by.");
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(700, 2);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(49, 17);
            this.label14.TabIndex = 13;
            this.label14.Text = "Sort By";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSortAscending);
            this.groupBox1.Controls.Add(this.rbSortDescending);
            this.groupBox1.Controls.Add(this.rbSortNone);
            this.groupBox1.Location = new System.Drawing.Point(699, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(175, 98);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sort Direction";
            // 
            // rbSortNone
            // 
            this.rbSortNone.AutoSize = true;
            this.rbSortNone.Checked = true;
            this.rbSortNone.Location = new System.Drawing.Point(7, 24);
            this.rbSortNone.Name = "rbSortNone";
            this.rbSortNone.Size = new System.Drawing.Size(58, 21);
            this.rbSortNone.TabIndex = 0;
            this.rbSortNone.TabStop = true;
            this.rbSortNone.Text = "None";
            this.rbSortNone.UseVisualStyleBackColor = true;
            // 
            // rbSortDescending
            // 
            this.rbSortDescending.AutoSize = true;
            this.rbSortDescending.Location = new System.Drawing.Point(71, 51);
            this.rbSortDescending.Name = "rbSortDescending";
            this.rbSortDescending.Size = new System.Drawing.Size(94, 21);
            this.rbSortDescending.TabIndex = 1;
            this.rbSortDescending.Text = "Descending";
            this.toolTip1.SetToolTip(this.rbSortDescending, "Sorts the data in the grid by the selected sort column in descending order.");
            this.rbSortDescending.UseVisualStyleBackColor = true;
            // 
            // rbSortAscending
            // 
            this.rbSortAscending.AutoSize = true;
            this.rbSortAscending.Location = new System.Drawing.Point(71, 24);
            this.rbSortAscending.Name = "rbSortAscending";
            this.rbSortAscending.Size = new System.Drawing.Size(86, 21);
            this.rbSortAscending.TabIndex = 2;
            this.rbSortAscending.Text = "Ascending";
            this.toolTip1.SetToolTip(this.rbSortAscending, "Sorts the data in the grid by the selected sort column in ascending order.");
            this.rbSortAscending.UseVisualStyleBackColor = true;
            // 
            // GridSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 611);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.pnlGridSettings);
            this.Controls.Add(this.pnlSelector);
            this.Controls.Add(this.pnlDialogButtons);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GridSettingsDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Grid Settings";
            this.Load += new System.EventHandler(this.GridSettingsDialog_Load);
            this.pnlDialogButtons.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.pnlGridTitle.ResumeLayout(false);
            this.pnlGridTitle.PerformLayout();
            this.pnlFields.ResumeLayout(false);
            this.pnlSelector.ResumeLayout(false);
            this.pnlSelector.PerformLayout();
            this.pnlGridSettings.ResumeLayout(false);
            this.pnlGridSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlDialogButtons;
        private System.Windows.Forms.Button btnCancelClose;
        private System.Windows.Forms.Button btnEditSave;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlGridTitle;
        private System.Windows.Forms.TextBox txtGridTitle;
        private System.Windows.Forms.Button btnTitleForeColor;
        private System.Windows.Forms.Button btnTitleBackColor;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TextBox txtColumnHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDataPropertyName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtColumnWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkColumnVisible;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.ListBox lstFields;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDisplayIndex;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel pnlSelector;
        private System.Windows.Forms.ComboBox cboGridSelection;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlGridSettings;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGridName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboDataSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGridWidth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkHideRowSelectors;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ComboBox cboLocation;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtDisplayOrder;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lblIndex;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSortAscending;
        private System.Windows.Forms.RadioButton rbSortDescending;
        private System.Windows.Forms.RadioButton rbSortNone;
        private System.Windows.Forms.ComboBox cboSortBy;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}