namespace rNascar23TestApp.Dialogs
{
    partial class ImportExportDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportExportDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAccept = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lstSource = new System.Windows.Forms.ListBox();
            this.lstTarget = new System.Windows.Forms.ListBox();
            this.splLeft = new System.Windows.Forms.Splitter();
            this.splRight = new System.Windows.Forms.Splitter();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.btnImportExportSelected = new System.Windows.Forms.Button();
            this.grpOverwriteMethod = new System.Windows.Forms.GroupBox();
            this.rbOverwrite = new System.Windows.Forms.RadioButton();
            this.rbAddnew = new System.Windows.Forms.RadioButton();
            this.btnRemoveSelected = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.grpOverwriteMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAccept);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 221);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(563, 44);
            this.panel1.TabIndex = 1;
            // 
            // btnAccept
            // 
            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAccept.Image = ((System.Drawing.Image)(resources.GetObject("btnAccept.Image")));
            this.btnAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAccept.Location = new System.Drawing.Point(471, 7);
            this.btnAccept.Margin = new System.Windows.Forms.Padding(5);
            this.btnAccept.Name = "btnAccept";
            this.btnAccept.Size = new System.Drawing.Size(78, 29);
            this.btnAccept.TabIndex = 6;
            this.btnAccept.Text = "Accept";
            this.btnAccept.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAccept.UseVisualStyleBackColor = true;
            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(17, 7);
            this.button1.Margin = new System.Windows.Forms.Padding(5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 29);
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
            this.button2.Location = new System.Drawing.Point(1747, 7);
            this.button2.Margin = new System.Windows.Forms.Padding(5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(191, 41);
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
            this.btnCancel.Location = new System.Drawing.Point(-684, 12);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(209, 41);
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
            this.btnClose.Location = new System.Drawing.Point(1058, 12);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(191, 41);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Save and Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lstSource
            // 
            this.lstSource.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstSource.FormattingEnabled = true;
            this.lstSource.Location = new System.Drawing.Point(0, 0);
            this.lstSource.Name = "lstSource";
            this.lstSource.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSource.Size = new System.Drawing.Size(150, 221);
            this.lstSource.TabIndex = 2;
            this.lstSource.SelectedIndexChanged += new System.EventHandler(this.lstSource_SelectedIndexChanged);
            // 
            // lstTarget
            // 
            this.lstTarget.Dock = System.Windows.Forms.DockStyle.Right;
            this.lstTarget.Enabled = false;
            this.lstTarget.FormattingEnabled = true;
            this.lstTarget.Location = new System.Drawing.Point(413, 0);
            this.lstTarget.Name = "lstTarget";
            this.lstTarget.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstTarget.Size = new System.Drawing.Size(150, 221);
            this.lstTarget.TabIndex = 3;
            // 
            // splLeft
            // 
            this.splLeft.Location = new System.Drawing.Point(150, 0);
            this.splLeft.Name = "splLeft";
            this.splLeft.Size = new System.Drawing.Size(3, 221);
            this.splLeft.TabIndex = 4;
            this.splLeft.TabStop = false;
            // 
            // splRight
            // 
            this.splRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.splRight.Location = new System.Drawing.Point(410, 0);
            this.splRight.Name = "splRight";
            this.splRight.Size = new System.Drawing.Size(3, 221);
            this.splRight.TabIndex = 5;
            this.splRight.TabStop = false;
            // 
            // pnlCenter
            // 
            this.pnlCenter.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlCenter.Controls.Add(this.btnRemoveSelected);
            this.pnlCenter.Controls.Add(this.grpOverwriteMethod);
            this.pnlCenter.Controls.Add(this.btnImportExportSelected);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(153, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(257, 221);
            this.pnlCenter.TabIndex = 6;
            // 
            // btnImportExportSelected
            // 
            this.btnImportExportSelected.Enabled = false;
            this.btnImportExportSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnImportExportSelected.Image")));
            this.btnImportExportSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImportExportSelected.Location = new System.Drawing.Point(70, 69);
            this.btnImportExportSelected.Name = "btnImportExportSelected";
            this.btnImportExportSelected.Size = new System.Drawing.Size(105, 30);
            this.btnImportExportSelected.TabIndex = 0;
            this.btnImportExportSelected.Text = "Import Selected";
            this.btnImportExportSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportExportSelected.UseVisualStyleBackColor = true;
            this.btnImportExportSelected.Click += new System.EventHandler(this.btnImportSelected_Click);
            // 
            // grpOverwriteMethod
            // 
            this.grpOverwriteMethod.Controls.Add(this.rbAddnew);
            this.grpOverwriteMethod.Controls.Add(this.rbOverwrite);
            this.grpOverwriteMethod.Location = new System.Drawing.Point(6, 12);
            this.grpOverwriteMethod.Name = "grpOverwriteMethod";
            this.grpOverwriteMethod.Size = new System.Drawing.Size(245, 51);
            this.grpOverwriteMethod.TabIndex = 1;
            this.grpOverwriteMethod.TabStop = false;
            this.grpOverwriteMethod.Text = "Import Method";
            // 
            // rbOverwrite
            // 
            this.rbOverwrite.AutoSize = true;
            this.rbOverwrite.Checked = true;
            this.rbOverwrite.Location = new System.Drawing.Point(7, 20);
            this.rbOverwrite.Name = "rbOverwrite";
            this.rbOverwrite.Size = new System.Drawing.Size(109, 17);
            this.rbOverwrite.TabIndex = 0;
            this.rbOverwrite.TabStop = true;
            this.rbOverwrite.Text = "Overwrite Existing";
            this.rbOverwrite.UseVisualStyleBackColor = true;
            // 
            // rbAddnew
            // 
            this.rbAddnew.AutoSize = true;
            this.rbAddnew.Location = new System.Drawing.Point(122, 20);
            this.rbAddnew.Name = "rbAddnew";
            this.rbAddnew.Size = new System.Drawing.Size(117, 17);
            this.rbAddnew.TabIndex = 1;
            this.rbAddnew.Text = "Add as New Item(s)";
            this.rbAddnew.UseVisualStyleBackColor = true;
            // 
            // btnRemoveSelected
            // 
            this.btnRemoveSelected.Enabled = false;
            this.btnRemoveSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveSelected.Image")));
            this.btnRemoveSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveSelected.Location = new System.Drawing.Point(70, 126);
            this.btnRemoveSelected.Name = "btnRemoveSelected";
            this.btnRemoveSelected.Size = new System.Drawing.Size(105, 30);
            this.btnRemoveSelected.TabIndex = 2;
            this.btnRemoveSelected.Text = "Remove";
            this.btnRemoveSelected.UseVisualStyleBackColor = true;
            this.btnRemoveSelected.Click += new System.EventHandler(this.btnRemoveSelected_Click);
            // 
            // ImportExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 265);
            this.Controls.Add(this.pnlCenter);
            this.Controls.Add(this.splRight);
            this.Controls.Add(this.splLeft);
            this.Controls.Add(this.lstTarget);
            this.Controls.Add(this.lstSource);
            this.Controls.Add(this.panel1);
            this.Name = "ImportExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ImportExportDialog";
            this.Load += new System.EventHandler(this.ImportExportDialog_Load);
            this.panel1.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.grpOverwriteMethod.ResumeLayout(false);
            this.grpOverwriteMethod.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAccept;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ListBox lstSource;
        private System.Windows.Forms.ListBox lstTarget;
        private System.Windows.Forms.Splitter splLeft;
        private System.Windows.Forms.Splitter splRight;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Button btnImportExportSelected;
        private System.Windows.Forms.GroupBox grpOverwriteMethod;
        private System.Windows.Forms.RadioButton rbAddnew;
        private System.Windows.Forms.RadioButton rbOverwrite;
        private System.Windows.Forms.Button btnRemoveSelected;
    }
}