namespace rNascar23.Dialogs
{
    partial class StyleEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StyleEditor));
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.pnlDialogButtons = new System.Windows.Forms.Panel();
            this.btnDiscardAndClose = new System.Windows.Forms.Button();
            this.btnDeleteStyle = new System.Windows.Forms.Button();
            this.btnCopyStyle = new System.Windows.Forms.Button();
            this.btnNewStyle = new System.Windows.Forms.Button();
            this.btnCancelClose = new System.Windows.Forms.Button();
            this.btnEditSave = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlSelection = new System.Windows.Forms.Panel();
            this.cboStyles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlDialogButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.pnlSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.Location = new System.Drawing.Point(580, 66);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(431, 382);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // pnlDialogButtons
            // 
            this.pnlDialogButtons.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlDialogButtons.Controls.Add(this.btnDiscardAndClose);
            this.pnlDialogButtons.Controls.Add(this.btnDeleteStyle);
            this.pnlDialogButtons.Controls.Add(this.btnCopyStyle);
            this.pnlDialogButtons.Controls.Add(this.btnNewStyle);
            this.pnlDialogButtons.Controls.Add(this.btnCancelClose);
            this.pnlDialogButtons.Controls.Add(this.btnEditSave);
            this.pnlDialogButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDialogButtons.Location = new System.Drawing.Point(0, 448);
            this.pnlDialogButtons.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDialogButtons.Name = "pnlDialogButtons";
            this.pnlDialogButtons.Size = new System.Drawing.Size(1015, 43);
            this.pnlDialogButtons.TabIndex = 1;
            // 
            // btnDiscardAndClose
            // 
            this.btnDiscardAndClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDiscardAndClose.Location = new System.Drawing.Point(695, 4);
            this.btnDiscardAndClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnDiscardAndClose.Name = "btnDiscardAndClose";
            this.btnDiscardAndClose.Size = new System.Drawing.Size(172, 34);
            this.btnDiscardAndClose.TabIndex = 5;
            this.btnDiscardAndClose.Text = "Discard Changes && Close";
            this.btnDiscardAndClose.UseVisualStyleBackColor = true;
            this.btnDiscardAndClose.Click += new System.EventHandler(this.btnDiscardAndClose_Click);
            // 
            // btnDeleteStyle
            // 
            this.btnDeleteStyle.Location = new System.Drawing.Point(340, 5);
            this.btnDeleteStyle.Margin = new System.Windows.Forms.Padding(4);
            this.btnDeleteStyle.Name = "btnDeleteStyle";
            this.btnDeleteStyle.Size = new System.Drawing.Size(100, 34);
            this.btnDeleteStyle.TabIndex = 4;
            this.btnDeleteStyle.Text = "Delete";
            this.btnDeleteStyle.UseVisualStyleBackColor = true;
            this.btnDeleteStyle.Click += new System.EventHandler(this.btnDeleteStyle_Click);
            // 
            // btnCopyStyle
            // 
            this.btnCopyStyle.Location = new System.Drawing.Point(232, 4);
            this.btnCopyStyle.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopyStyle.Name = "btnCopyStyle";
            this.btnCopyStyle.Size = new System.Drawing.Size(100, 34);
            this.btnCopyStyle.TabIndex = 3;
            this.btnCopyStyle.Text = "Copy";
            this.btnCopyStyle.UseVisualStyleBackColor = true;
            this.btnCopyStyle.Click += new System.EventHandler(this.btnCopyStyle_Click);
            // 
            // btnNewStyle
            // 
            this.btnNewStyle.Location = new System.Drawing.Point(124, 4);
            this.btnNewStyle.Margin = new System.Windows.Forms.Padding(4);
            this.btnNewStyle.Name = "btnNewStyle";
            this.btnNewStyle.Size = new System.Drawing.Size(100, 34);
            this.btnNewStyle.TabIndex = 2;
            this.btnNewStyle.Text = "New";
            this.btnNewStyle.UseVisualStyleBackColor = true;
            this.btnNewStyle.Click += new System.EventHandler(this.btnNewStyle_Click);
            // 
            // btnCancelClose
            // 
            this.btnCancelClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelClose.Location = new System.Drawing.Point(875, 4);
            this.btnCancelClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancelClose.Name = "btnCancelClose";
            this.btnCancelClose.Size = new System.Drawing.Size(124, 34);
            this.btnCancelClose.TabIndex = 1;
            this.btnCancelClose.Text = "Save All && Close";
            this.btnCancelClose.UseVisualStyleBackColor = true;
            this.btnCancelClose.Click += new System.EventHandler(this.btnCancelClose_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Location = new System.Drawing.Point(16, 4);
            this.btnEditSave.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(100, 34);
            this.btnEditSave.TabIndex = 0;
            this.btnEditSave.Text = "Save";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.Location = new System.Drawing.Point(0, 66);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(580, 382);
            this.dataGridView1.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(1011, 0);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 448);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // pnlSelection
            // 
            this.pnlSelection.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlSelection.Controls.Add(this.cboStyles);
            this.pnlSelection.Controls.Add(this.label1);
            this.pnlSelection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSelection.Location = new System.Drawing.Point(0, 0);
            this.pnlSelection.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSelection.Name = "pnlSelection";
            this.pnlSelection.Size = new System.Drawing.Size(1011, 66);
            this.pnlSelection.TabIndex = 4;
            // 
            // cboStyles
            // 
            this.cboStyles.FormattingEnabled = true;
            this.cboStyles.Location = new System.Drawing.Point(16, 29);
            this.cboStyles.Margin = new System.Windows.Forms.Padding(4);
            this.cboStyles.Name = "cboStyles";
            this.cboStyles.Size = new System.Drawing.Size(371, 24);
            this.cboStyles.TabIndex = 1;
            this.cboStyles.SelectedIndexChanged += new System.EventHandler(this.cboStyles_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Styles";
            // 
            // StyleEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 491);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.pnlSelection);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlDialogButtons);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "StyleEditor";
            this.Text = "Style Editor";
            this.Load += new System.EventHandler(this.GridStyleDialog_Load);
            this.pnlDialogButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.pnlSelection.ResumeLayout(false);
            this.pnlSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel pnlDialogButtons;
        private System.Windows.Forms.Button btnCancelClose;
        private System.Windows.Forms.Button btnEditSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlSelection;
        private System.Windows.Forms.ComboBox cboStyles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteStyle;
        private System.Windows.Forms.Button btnCopyStyle;
        private System.Windows.Forms.Button btnNewStyle;
        private System.Windows.Forms.Button btnDiscardAndClose;
    }
}