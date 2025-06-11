namespace ImgAnalyzer
{
    partial class Form_1D
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
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_rename = new System.Windows.Forms.Button();
            this.button_deleteall = new System.Windows.Forms.Button();
            this.button_calculate = new System.Windows.Forms.Button();
            this.button_calcall = new System.Windows.Forms.Button();
            this.button_plot = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(12, 12);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.Size = new System.Drawing.Size(642, 426);
            this.dataGrid.TabIndex = 0;
            // 
            // button_delete
            // 
            this.button_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_delete.Location = new System.Drawing.Point(660, 99);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(130, 23);
            this.button_delete.TabIndex = 1;
            this.button_delete.Text = "Удалить измерения";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_rename
            // 
            this.button_rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_rename.Location = new System.Drawing.Point(660, 128);
            this.button_rename.Name = "button_rename";
            this.button_rename.Size = new System.Drawing.Size(130, 23);
            this.button_rename.TabIndex = 2;
            this.button_rename.Text = "Переименовать";
            this.button_rename.UseVisualStyleBackColor = true;
            this.button_rename.Click += new System.EventHandler(this.button_rename_Click);
            // 
            // button_deleteall
            // 
            this.button_deleteall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_deleteall.Location = new System.Drawing.Point(660, 157);
            this.button_deleteall.Name = "button_deleteall";
            this.button_deleteall.Size = new System.Drawing.Size(130, 23);
            this.button_deleteall.TabIndex = 3;
            this.button_deleteall.Text = "Удалить все";
            this.button_deleteall.UseVisualStyleBackColor = true;
            this.button_deleteall.Click += new System.EventHandler(this.button_deleteall_Click);
            // 
            // button_calculate
            // 
            this.button_calculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_calculate.Location = new System.Drawing.Point(660, 12);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(130, 23);
            this.button_calculate.TabIndex = 4;
            this.button_calculate.Text = "Рассчитать";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // button_calcall
            // 
            this.button_calcall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_calcall.Location = new System.Drawing.Point(660, 41);
            this.button_calcall.Name = "button_calcall";
            this.button_calcall.Size = new System.Drawing.Size(130, 23);
            this.button_calcall.TabIndex = 5;
            this.button_calcall.Text = "Рассчитать все";
            this.button_calcall.UseVisualStyleBackColor = true;
            this.button_calcall.Click += new System.EventHandler(this.button_calcall_Click);
            // 
            // button_plot
            // 
            this.button_plot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_plot.Location = new System.Drawing.Point(660, 70);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(130, 23);
            this.button_plot.TabIndex = 6;
            this.button_plot.Text = "Построить график";
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // Form_1D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 450);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.button_calcall);
            this.Controls.Add(this.button_calculate);
            this.Controls.Add(this.button_deleteall);
            this.Controls.Add(this.button_rename);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.dataGrid);
            this.Name = "Form_1D";
            this.Text = "Form_1D";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_rename;
        private System.Windows.Forms.Button button_deleteall;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.Button button_calcall;
        private System.Windows.Forms.Button button_plot;
    }
}