namespace rNascar23TestApp.Views
{
    partial class MoversFallersGridView
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
            this.Grid = new System.Windows.Forms.DataGridView();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.GridBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.splBottom = new System.Windows.Forms.Splitter();
            this.FallersGrid = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.FallersGridBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FallersGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FallersGridBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.BackgroundColor = System.Drawing.Color.Black;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.GridColor = System.Drawing.SystemColors.Control;
            this.Grid.Location = new System.Drawing.Point(0, 25);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(275, 148);
            this.Grid.TabIndex = 7;
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.DarkGreen;
            this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.White;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(275, 25);
            this.TitleLabel.TabIndex = 6;
            this.TitleLabel.Text = "Biggest Movers";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splBottom
            // 
            this.splBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splBottom.Location = new System.Drawing.Point(0, 173);
            this.splBottom.Name = "splBottom";
            this.splBottom.Size = new System.Drawing.Size(275, 3);
            this.splBottom.TabIndex = 8;
            this.splBottom.TabStop = false;
            // 
            // FallersGrid
            // 
            this.FallersGrid.BackgroundColor = System.Drawing.Color.Black;
            this.FallersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.FallersGrid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FallersGrid.GridColor = System.Drawing.SystemColors.Control;
            this.FallersGrid.Location = new System.Drawing.Point(0, 201);
            this.FallersGrid.Name = "FallersGrid";
            this.FallersGrid.Size = new System.Drawing.Size(275, 148);
            this.FallersGrid.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.DarkRed;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Biggest Fallers";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MoversFallersGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.splBottom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FallersGrid);
            this.Controls.Add(this.TitleLabel);
            this.Name = "MoversFallersGridView";
            this.Size = new System.Drawing.Size(275, 349);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FallersGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FallersGridBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView Grid;
        public System.Windows.Forms.Label TitleLabel;
        public System.Windows.Forms.BindingSource GridBindingSource;
        private System.Windows.Forms.Splitter splBottom;
        internal System.Windows.Forms.DataGridView FallersGrid;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.BindingSource FallersGridBindingSource;
    }
}
