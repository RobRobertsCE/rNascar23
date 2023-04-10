namespace rNascar23.PatchBuilder
{
    partial class PatchHelperDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatchHelperDialog));
            this.btnGetCurrentAssemblies = new System.Windows.Forms.Button();
            this.lvCurrent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTopButtons = new System.Windows.Forms.Panel();
            this.lblRegistryPath = new System.Windows.Forms.Label();
            this.lblRegistryVersion = new System.Windows.Forms.Label();
            this.btnGetPath = new System.Windows.Forms.Button();
            this.btnGetVersion = new System.Windows.Forms.Button();
            this.txtVersionNumber = new System.Windows.Forms.TextBox();
            this.btnSetVersion = new System.Windows.Forms.Button();
            this.btnBuildChangeSet = new System.Windows.Forms.Button();
            this.btnGetPatchFiles = new System.Windows.Forms.Button();
            this.cboReleaseType = new System.Windows.Forms.ComboBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.btnBuildPatchZip = new System.Windows.Forms.Button();
            this.btnGetAvailablePatches = new System.Windows.Forms.Button();
            this.pnlPatchSets = new System.Windows.Forms.Panel();
            this.lvPatchSets = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvAssets = new System.Windows.Forms.ListView();
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader19 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlPatchFiles = new System.Windows.Forms.Panel();
            this.lvPatchFiles = new System.Windows.Forms.ListView();
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader16 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader17 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader18 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlCurrentAssemblyFiles = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlChangeSet = new System.Windows.Forms.Panel();
            this.lvChangeSet = new System.Windows.Forms.ListView();
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader22 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader20 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader21 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.columnHeader23 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlTopButtons.SuspendLayout();
            this.pnlPatchSets.SuspendLayout();
            this.pnlPatchFiles.SuspendLayout();
            this.pnlCurrentAssemblyFiles.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlChangeSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGetCurrentAssemblies
            // 
            this.btnGetCurrentAssemblies.Location = new System.Drawing.Point(13, 12);
            this.btnGetCurrentAssemblies.Name = "btnGetCurrentAssemblies";
            this.btnGetCurrentAssemblies.Size = new System.Drawing.Size(124, 23);
            this.btnGetCurrentAssemblies.TabIndex = 0;
            this.btnGetCurrentAssemblies.Text = "Get Current Assemblies";
            this.btnGetCurrentAssemblies.UseVisualStyleBackColor = true;
            this.btnGetCurrentAssemblies.Click += new System.EventHandler(this.btnGetCurrentAssemblies_Click);
            // 
            // lvCurrent
            // 
            this.lvCurrent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCurrent.GridLines = true;
            this.lvCurrent.HideSelection = false;
            this.lvCurrent.Location = new System.Drawing.Point(8, 8);
            this.lvCurrent.Name = "lvCurrent";
            this.lvCurrent.Size = new System.Drawing.Size(503, 260);
            this.lvCurrent.TabIndex = 1;
            this.lvCurrent.UseCompatibleStateImageBehavior = false;
            this.lvCurrent.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Assenbly Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Path";
            this.columnHeader3.Width = 500;
            // 
            // pnlTopButtons
            // 
            this.pnlTopButtons.Controls.Add(this.lblRegistryPath);
            this.pnlTopButtons.Controls.Add(this.lblRegistryVersion);
            this.pnlTopButtons.Controls.Add(this.btnGetPath);
            this.pnlTopButtons.Controls.Add(this.btnGetVersion);
            this.pnlTopButtons.Controls.Add(this.txtVersionNumber);
            this.pnlTopButtons.Controls.Add(this.btnSetVersion);
            this.pnlTopButtons.Controls.Add(this.btnBuildChangeSet);
            this.pnlTopButtons.Controls.Add(this.btnGetPatchFiles);
            this.pnlTopButtons.Controls.Add(this.cboReleaseType);
            this.pnlTopButtons.Controls.Add(this.txtVersion);
            this.pnlTopButtons.Controls.Add(this.btnBuildPatchZip);
            this.pnlTopButtons.Controls.Add(this.btnGetAvailablePatches);
            this.pnlTopButtons.Controls.Add(this.btnGetCurrentAssemblies);
            this.pnlTopButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTopButtons.Location = new System.Drawing.Point(0, 0);
            this.pnlTopButtons.Name = "pnlTopButtons";
            this.pnlTopButtons.Size = new System.Drawing.Size(1335, 125);
            this.pnlTopButtons.TabIndex = 2;
            // 
            // lblRegistryPath
            // 
            this.lblRegistryPath.AutoSize = true;
            this.lblRegistryPath.Location = new System.Drawing.Point(274, 73);
            this.lblRegistryPath.Name = "lblRegistryPath";
            this.lblRegistryPath.Size = new System.Drawing.Size(10, 13);
            this.lblRegistryPath.TabIndex = 22;
            this.lblRegistryPath.Text = "-";
            // 
            // lblRegistryVersion
            // 
            this.lblRegistryVersion.AutoSize = true;
            this.lblRegistryVersion.Location = new System.Drawing.Point(274, 46);
            this.lblRegistryVersion.Name = "lblRegistryVersion";
            this.lblRegistryVersion.Size = new System.Drawing.Size(10, 13);
            this.lblRegistryVersion.TabIndex = 21;
            this.lblRegistryVersion.Text = "-";
            // 
            // btnGetPath
            // 
            this.btnGetPath.Location = new System.Drawing.Point(143, 68);
            this.btnGetPath.Name = "btnGetPath";
            this.btnGetPath.Size = new System.Drawing.Size(124, 23);
            this.btnGetPath.TabIndex = 20;
            this.btnGetPath.Text = "Get Path";
            this.btnGetPath.UseVisualStyleBackColor = true;
            this.btnGetPath.Click += new System.EventHandler(this.btnGetPath_Click);
            // 
            // btnGetVersion
            // 
            this.btnGetVersion.Location = new System.Drawing.Point(143, 41);
            this.btnGetVersion.Name = "btnGetVersion";
            this.btnGetVersion.Size = new System.Drawing.Size(124, 23);
            this.btnGetVersion.TabIndex = 19;
            this.btnGetVersion.Text = "Get Version";
            this.btnGetVersion.UseVisualStyleBackColor = true;
            this.btnGetVersion.Click += new System.EventHandler(this.btnGetVersion_Click);
            // 
            // txtVersionNumber
            // 
            this.txtVersionNumber.Location = new System.Drawing.Point(274, 13);
            this.txtVersionNumber.Name = "txtVersionNumber";
            this.txtVersionNumber.Size = new System.Drawing.Size(100, 20);
            this.txtVersionNumber.TabIndex = 18;
            // 
            // btnSetVersion
            // 
            this.btnSetVersion.Location = new System.Drawing.Point(143, 12);
            this.btnSetVersion.Name = "btnSetVersion";
            this.btnSetVersion.Size = new System.Drawing.Size(124, 23);
            this.btnSetVersion.TabIndex = 17;
            this.btnSetVersion.Text = "Set Version";
            this.btnSetVersion.UseVisualStyleBackColor = true;
            this.btnSetVersion.Click += new System.EventHandler(this.btnSetVersion_Click);
            // 
            // btnBuildChangeSet
            // 
            this.btnBuildChangeSet.Location = new System.Drawing.Point(13, 96);
            this.btnBuildChangeSet.Name = "btnBuildChangeSet";
            this.btnBuildChangeSet.Size = new System.Drawing.Size(124, 23);
            this.btnBuildChangeSet.TabIndex = 14;
            this.btnBuildChangeSet.Text = "Build Change Set";
            this.btnBuildChangeSet.UseVisualStyleBackColor = true;
            this.btnBuildChangeSet.Click += new System.EventHandler(this.btnBuildChangeSet_Click);
            // 
            // btnGetPatchFiles
            // 
            this.btnGetPatchFiles.Enabled = false;
            this.btnGetPatchFiles.Location = new System.Drawing.Point(13, 68);
            this.btnGetPatchFiles.Name = "btnGetPatchFiles";
            this.btnGetPatchFiles.Size = new System.Drawing.Size(124, 23);
            this.btnGetPatchFiles.TabIndex = 13;
            this.btnGetPatchFiles.Text = "Get Patch Files";
            this.btnGetPatchFiles.UseVisualStyleBackColor = true;
            this.btnGetPatchFiles.Click += new System.EventHandler(this.btnGetPatchFiles_Click);
            // 
            // cboReleaseType
            // 
            this.cboReleaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReleaseType.FormattingEnabled = true;
            this.cboReleaseType.Items.AddRange(new object[] {
            "Beta",
            "Patch",
            "Release"});
            this.cboReleaseType.Location = new System.Drawing.Point(274, 97);
            this.cboReleaseType.Name = "cboReleaseType";
            this.cboReleaseType.Size = new System.Drawing.Size(121, 21);
            this.cboReleaseType.TabIndex = 12;
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(401, 97);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(60, 20);
            this.txtVersion.TabIndex = 11;
            this.txtVersion.Text = "0.2.1.1";
            // 
            // btnBuildPatchZip
            // 
            this.btnBuildPatchZip.Location = new System.Drawing.Point(143, 96);
            this.btnBuildPatchZip.Name = "btnBuildPatchZip";
            this.btnBuildPatchZip.Size = new System.Drawing.Size(124, 23);
            this.btnBuildPatchZip.TabIndex = 10;
            this.btnBuildPatchZip.Text = "Build Patch Zip";
            this.btnBuildPatchZip.UseVisualStyleBackColor = true;
            this.btnBuildPatchZip.Click += new System.EventHandler(this.btnBuildRelease_Click);
            // 
            // btnGetAvailablePatches
            // 
            this.btnGetAvailablePatches.Location = new System.Drawing.Point(13, 41);
            this.btnGetAvailablePatches.Name = "btnGetAvailablePatches";
            this.btnGetAvailablePatches.Size = new System.Drawing.Size(124, 23);
            this.btnGetAvailablePatches.TabIndex = 9;
            this.btnGetAvailablePatches.Text = "Get Available Patches";
            this.btnGetAvailablePatches.UseVisualStyleBackColor = true;
            this.btnGetAvailablePatches.Click += new System.EventHandler(this.btnGetAvailablePatches_Click);
            // 
            // pnlPatchSets
            // 
            this.pnlPatchSets.Controls.Add(this.lvPatchSets);
            this.pnlPatchSets.Controls.Add(this.lvAssets);
            this.pnlPatchSets.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPatchSets.Location = new System.Drawing.Point(0, 125);
            this.pnlPatchSets.Name = "pnlPatchSets";
            this.pnlPatchSets.Padding = new System.Windows.Forms.Padding(8);
            this.pnlPatchSets.Size = new System.Drawing.Size(1335, 208);
            this.pnlPatchSets.TabIndex = 4;
            // 
            // lvPatchSets
            // 
            this.lvPatchSets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader7,
            this.columnHeader6,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.lvPatchSets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPatchSets.FullRowSelect = true;
            this.lvPatchSets.GridLines = true;
            this.lvPatchSets.HideSelection = false;
            this.lvPatchSets.Location = new System.Drawing.Point(8, 8);
            this.lvPatchSets.Name = "lvPatchSets";
            this.lvPatchSets.Size = new System.Drawing.Size(1319, 127);
            this.lvPatchSets.TabIndex = 0;
            this.lvPatchSets.UseCompatibleStateImageBehavior = false;
            this.lvPatchSets.View = System.Windows.Forms.View.Details;
            this.lvPatchSets.SelectedIndexChanged += new System.EventHandler(this.lvPatchSets_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 125;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Version";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Stage";
            this.columnHeader7.Width = 45;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Files";
            this.columnHeader6.Width = 35;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Created";
            this.columnHeader8.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Published";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "Author";
            this.columnHeader10.Width = 100;
            // 
            // lvAssets
            // 
            this.lvAssets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader11,
            this.columnHeader13,
            this.columnHeader23,
            this.columnHeader14,
            this.columnHeader19});
            this.lvAssets.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvAssets.FullRowSelect = true;
            this.lvAssets.GridLines = true;
            this.lvAssets.HideSelection = false;
            this.lvAssets.Location = new System.Drawing.Point(8, 135);
            this.lvAssets.Name = "lvAssets";
            this.lvAssets.Size = new System.Drawing.Size(1319, 65);
            this.lvAssets.TabIndex = 2;
            this.lvAssets.UseCompatibleStateImageBehavior = false;
            this.lvAssets.View = System.Windows.Forms.View.Details;
            this.lvAssets.SelectedIndexChanged += new System.EventHandler(this.lvAssets_SelectedIndexChanged);
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "Name";
            this.columnHeader11.Width = 125;
            // 
            // columnHeader13
            // 
            this.columnHeader13.Text = "Size";
            this.columnHeader13.Width = 70;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "Created";
            this.columnHeader14.Width = 200;
            // 
            // columnHeader19
            // 
            this.columnHeader19.Text = "Url";
            this.columnHeader19.Width = 600;
            // 
            // pnlPatchFiles
            // 
            this.pnlPatchFiles.Controls.Add(this.lvPatchFiles);
            this.pnlPatchFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPatchFiles.Location = new System.Drawing.Point(522, 0);
            this.pnlPatchFiles.Name = "pnlPatchFiles";
            this.pnlPatchFiles.Padding = new System.Windows.Forms.Padding(8);
            this.pnlPatchFiles.Size = new System.Drawing.Size(549, 276);
            this.pnlPatchFiles.TabIndex = 6;
            // 
            // lvPatchFiles
            // 
            this.lvPatchFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader15,
            this.columnHeader16,
            this.columnHeader17,
            this.columnHeader18});
            this.lvPatchFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPatchFiles.GridLines = true;
            this.lvPatchFiles.HideSelection = false;
            this.lvPatchFiles.Location = new System.Drawing.Point(8, 8);
            this.lvPatchFiles.Name = "lvPatchFiles";
            this.lvPatchFiles.Size = new System.Drawing.Size(533, 260);
            this.lvPatchFiles.TabIndex = 2;
            this.lvPatchFiles.UseCompatibleStateImageBehavior = false;
            this.lvPatchFiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "Name";
            this.columnHeader15.Width = 125;
            // 
            // columnHeader16
            // 
            this.columnHeader16.Text = "Version";
            this.columnHeader16.Width = 100;
            // 
            // columnHeader17
            // 
            this.columnHeader17.Text = "Size";
            this.columnHeader17.Width = 100;
            // 
            // columnHeader18
            // 
            this.columnHeader18.Text = "Created";
            this.columnHeader18.Width = 200;
            // 
            // pnlCurrentAssemblyFiles
            // 
            this.pnlCurrentAssemblyFiles.Controls.Add(this.lvCurrent);
            this.pnlCurrentAssemblyFiles.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlCurrentAssemblyFiles.Location = new System.Drawing.Point(0, 0);
            this.pnlCurrentAssemblyFiles.Name = "pnlCurrentAssemblyFiles";
            this.pnlCurrentAssemblyFiles.Padding = new System.Windows.Forms.Padding(8);
            this.pnlCurrentAssemblyFiles.Size = new System.Drawing.Size(519, 276);
            this.pnlCurrentAssemblyFiles.TabIndex = 7;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.pnlChangeSet);
            this.pnlBottom.Controls.Add(this.splitter2);
            this.pnlBottom.Controls.Add(this.pnlPatchFiles);
            this.pnlBottom.Controls.Add(this.splitter1);
            this.pnlBottom.Controls.Add(this.pnlCurrentAssemblyFiles);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 333);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1335, 276);
            this.pnlBottom.TabIndex = 8;
            // 
            // pnlChangeSet
            // 
            this.pnlChangeSet.Controls.Add(this.lvChangeSet);
            this.pnlChangeSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChangeSet.Location = new System.Drawing.Point(1074, 0);
            this.pnlChangeSet.Name = "pnlChangeSet";
            this.pnlChangeSet.Padding = new System.Windows.Forms.Padding(8);
            this.pnlChangeSet.Size = new System.Drawing.Size(261, 276);
            this.pnlChangeSet.TabIndex = 10;
            // 
            // lvChangeSet
            // 
            this.lvChangeSet.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader12,
            this.columnHeader22,
            this.columnHeader20,
            this.columnHeader21});
            this.lvChangeSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvChangeSet.FullRowSelect = true;
            this.lvChangeSet.GridLines = true;
            this.lvChangeSet.HideSelection = false;
            this.lvChangeSet.Location = new System.Drawing.Point(8, 8);
            this.lvChangeSet.Name = "lvChangeSet";
            this.lvChangeSet.Size = new System.Drawing.Size(245, 260);
            this.lvChangeSet.TabIndex = 0;
            this.lvChangeSet.UseCompatibleStateImageBehavior = false;
            this.lvChangeSet.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader12
            // 
            this.columnHeader12.Text = "Name";
            this.columnHeader12.Width = 300;
            // 
            // columnHeader22
            // 
            this.columnHeader22.Text = "Action";
            // 
            // columnHeader20
            // 
            this.columnHeader20.Text = "Old";
            // 
            // columnHeader21
            // 
            this.columnHeader21.Text = "New";
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(1071, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 276);
            this.splitter2.TabIndex = 9;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(519, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 276);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // columnHeader23
            // 
            this.columnHeader23.Text = "Downloads";
            this.columnHeader23.Width = 75;
            // 
            // PatchHelperDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 609);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlPatchSets);
            this.Controls.Add(this.pnlTopButtons);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PatchHelperDialog";
            this.Text = "rNascar23 Patcher";
            this.Load += new System.EventHandler(this.PatcherDialog_Load);
            this.pnlTopButtons.ResumeLayout(false);
            this.pnlTopButtons.PerformLayout();
            this.pnlPatchSets.ResumeLayout(false);
            this.pnlPatchFiles.ResumeLayout(false);
            this.pnlCurrentAssemblyFiles.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlChangeSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnGetCurrentAssemblies;
        private System.Windows.Forms.ListView lvCurrent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel pnlTopButtons;
        private System.Windows.Forms.Button btnGetAvailablePatches;
        private System.Windows.Forms.Panel pnlPatchSets;
        private System.Windows.Forms.ListView lvPatchSets;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Panel pnlPatchFiles;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.Button btnBuildPatchZip;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.ComboBox cboReleaseType;
        private System.Windows.Forms.Button btnGetPatchFiles;
        private System.Windows.Forms.ListView lvAssets;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.ColumnHeader columnHeader19;
        private System.Windows.Forms.ListView lvPatchFiles;
        private System.Windows.Forms.ColumnHeader columnHeader15;
        private System.Windows.Forms.ColumnHeader columnHeader16;
        private System.Windows.Forms.ColumnHeader columnHeader17;
        private System.Windows.Forms.ColumnHeader columnHeader18;
        private System.Windows.Forms.Panel pnlCurrentAssemblyFiles;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlChangeSet;
        private System.Windows.Forms.ListView lvChangeSet;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader20;
        private System.Windows.Forms.ColumnHeader columnHeader21;
        private System.Windows.Forms.Button btnBuildChangeSet;
        private System.Windows.Forms.ColumnHeader columnHeader22;
        private System.Windows.Forms.TextBox txtVersionNumber;
        private System.Windows.Forms.Button btnSetVersion;
        private System.Windows.Forms.Button btnGetPath;
        private System.Windows.Forms.Button btnGetVersion;
        private System.Windows.Forms.Label lblRegistryPath;
        private System.Windows.Forms.Label lblRegistryVersion;
        private System.Windows.Forms.ColumnHeader columnHeader23;
    }
}

