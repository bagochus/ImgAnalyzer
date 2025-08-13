namespace ImgAnalyzer._2D
{
    partial class Form_2D
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
            this.button_measure = new System.Windows.Forms.Button();
            this.button_rename = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_math = new System.Windows.Forms.Button();
            this.button_plot = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.button_group_new = new System.Windows.Forms.Button();
            this.button_newview = new System.Windows.Forms.Button();
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
            this.dataGrid.Size = new System.Drawing.Size(480, 432);
            this.dataGrid.TabIndex = 0;
            // 
            // button_measure
            // 
            this.button_measure.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_measure.Location = new System.Drawing.Point(498, 12);
            this.button_measure.Name = "button_measure";
            this.button_measure.Size = new System.Drawing.Size(153, 23);
            this.button_measure.TabIndex = 1;
            this.button_measure.Text = "Групповые операции";
            this.button_measure.UseVisualStyleBackColor = true;
            this.button_measure.Click += new System.EventHandler(this.button_measure_Click);
            // 
            // button_rename
            // 
            this.button_rename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_rename.Location = new System.Drawing.Point(498, 99);
            this.button_rename.Name = "button_rename";
            this.button_rename.Size = new System.Drawing.Size(153, 23);
            this.button_rename.TabIndex = 2;
            this.button_rename.Text = "Переименовать";
            this.button_rename.UseVisualStyleBackColor = true;
            this.button_rename.Click += new System.EventHandler(this.button_rename_Click);
            // 
            // button_delete
            // 
            this.button_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_delete.Location = new System.Drawing.Point(498, 128);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(153, 23);
            this.button_delete.TabIndex = 3;
            this.button_delete.Text = "Удалить";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_math
            // 
            this.button_math.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_math.Location = new System.Drawing.Point(498, 41);
            this.button_math.Name = "button_math";
            this.button_math.Size = new System.Drawing.Size(153, 23);
            this.button_math.TabIndex = 4;
            this.button_math.Text = "Калькулятор изображений";
            this.button_math.UseVisualStyleBackColor = true;
            this.button_math.Click += new System.EventHandler(this.button_math_Click);
            // 
            // button_plot
            // 
            this.button_plot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_plot.Location = new System.Drawing.Point(498, 70);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(153, 23);
            this.button_plot.TabIndex = 5;
            this.button_plot.Text = "Отобразить";
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(498, 421);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(153, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(498, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Рассчитать матрицы";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(498, 186);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(153, 23);
            this.button_save.TabIndex = 8;
            this.button_save.Text = "Сохранить поля";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_load
            // 
            this.button_load.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load.Location = new System.Drawing.Point(498, 215);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(153, 23);
            this.button_load.TabIndex = 9;
            this.button_load.Text = "Загрузить поля";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_group_new
            // 
            this.button_group_new.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_group_new.Location = new System.Drawing.Point(498, 244);
            this.button_group_new.Name = "button_group_new";
            this.button_group_new.Size = new System.Drawing.Size(153, 23);
            this.button_group_new.TabIndex = 10;
            this.button_group_new.Text = "New GroupOP";
            this.button_group_new.UseVisualStyleBackColor = true;
            this.button_group_new.Click += new System.EventHandler(this.button_group_new_Click);
            // 
            // button_newview
            // 
            this.button_newview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_newview.Location = new System.Drawing.Point(498, 273);
            this.button_newview.Name = "button_newview";
            this.button_newview.Size = new System.Drawing.Size(153, 23);
            this.button_newview.TabIndex = 11;
            this.button_newview.Text = "Отобразить new";
            this.button_newview.UseVisualStyleBackColor = true;
            this.button_newview.Click += new System.EventHandler(this.button_newview_Click);
            // 
            // Form_2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(663, 456);
            this.Controls.Add(this.button_newview);
            this.Controls.Add(this.button_group_new);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.button_math);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_rename);
            this.Controls.Add(this.button_measure);
            this.Controls.Add(this.dataGrid);
            this.Name = "Form_2D";
            this.Text = "Form_2D";
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button button_measure;
        private System.Windows.Forms.Button button_rename;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_math;
        private System.Windows.Forms.Button button_plot;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_group_new;
        private System.Windows.Forms.Button button_newview;
    }
}