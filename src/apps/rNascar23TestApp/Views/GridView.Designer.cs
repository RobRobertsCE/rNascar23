namespace rNascar23TestApp.Views
{
    partial class GridView
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
            this.TitleLabel = new System.Windows.Forms.Label();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.GridBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TitleLabel
            // 
            this.TitleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(0, 0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(418, 25);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "{Grid Title}";
            this.TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Grid
            // 
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Grid.Location = new System.Drawing.Point(0, 25);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(418, 306);
            this.Grid.TabIndex = 1;
            // 
            // GridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.TitleLabel);
            this.Name = "GridView";
            this.Size = new System.Drawing.Size(418, 331);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label TitleLabel;
        public System.Windows.Forms.BindingSource GridBindingSource;
        internal System.Windows.Forms.DataGridView Grid;
    }
}
