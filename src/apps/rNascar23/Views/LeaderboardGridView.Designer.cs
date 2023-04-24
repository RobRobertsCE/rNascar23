namespace rNascar23.Views
{
    partial class LeaderboardGridView
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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.AllowUserToDeleteRows = false;
            this.Grid.AllowUserToResizeRows = false;
            this.Grid.BackgroundColor = System.Drawing.Color.Black;
            this.Grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Left;
            this.Grid.GridColor = System.Drawing.Color.Black;
            this.Grid.Location = new System.Drawing.Point(0, 25);
            this.Grid.Name = "Grid";
            this.Grid.RowTemplate.Height = 24;
            this.Grid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Grid.Size = new System.Drawing.Size(825, 392);
            this.Grid.TabIndex = 3;
            this.Grid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.Grid_DataBindingComplete);
            this.Grid.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid_RowsAdded);
            // 
            // TitleLabel
            // 
            this.TitleLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.ForeColor = System.Drawing.Color.LightGray;
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(825, 25);
            this.TitleLabel.TabIndex = 2;
            this.TitleLabel.Text = "Fastest Laps";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.TitleLabel.Visible = false;
            // 
            // LeaderboardGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.TitleLabel);
            this.Name = "LeaderboardGridView";
            this.Size = new System.Drawing.Size(825, 417);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DataGridView Grid;
        public System.Windows.Forms.Label TitleLabel;
        public System.Windows.Forms.BindingSource GridBindingSource;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
