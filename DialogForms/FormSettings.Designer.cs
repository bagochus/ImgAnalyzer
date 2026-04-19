namespace ImgAnalyzer.DialogForms
{
    partial class FormSettings
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button_search = new System.Windows.Forms.Button();
            this.textBox_search = new System.Windows.Forms.TextBox();
            this.button_import = new System.Windows.Forms.Button();
            this.checkBox_sf = new System.Windows.Forms.CheckBox();
            this.checkBox_sfofr = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(864, 361);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // button_search
            // 
            this.button_search.Location = new System.Drawing.Point(719, 12);
            this.button_search.Name = "button_search";
            this.button_search.Size = new System.Drawing.Size(75, 23);
            this.button_search.TabIndex = 1;
            this.button_search.Text = "Поиск";
            this.button_search.UseVisualStyleBackColor = true;
            this.button_search.Click += new System.EventHandler(this.button_search_Click);
            // 
            // textBox_search
            // 
            this.textBox_search.Location = new System.Drawing.Point(571, 12);
            this.textBox_search.Name = "textBox_search";
            this.textBox_search.Size = new System.Drawing.Size(142, 20);
            this.textBox_search.TabIndex = 2;
            // 
            // button_import
            // 
            this.button_import.Location = new System.Drawing.Point(800, 12);
            this.button_import.Name = "button_import";
            this.button_import.Size = new System.Drawing.Size(75, 23);
            this.button_import.TabIndex = 3;
            this.button_import.Text = "Импорт";
            this.button_import.UseVisualStyleBackColor = true;
            // 
            // checkBox_sf
            // 
            this.checkBox_sf.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_sf.AutoSize = true;
            this.checkBox_sf.Location = new System.Drawing.Point(12, 411);
            this.checkBox_sf.Name = "checkBox_sf";
            this.checkBox_sf.Size = new System.Drawing.Size(259, 17);
            this.checkBox_sf.TabIndex = 4;
            this.checkBox_sf.Text = "Показывать параметры при запросе из базы";
            this.checkBox_sf.UseVisualStyleBackColor = true;
            this.checkBox_sf.CheckedChanged += new System.EventHandler(this.checkBox_sf_CheckedChanged);
            // 
            // checkBox_sfofr
            // 
            this.checkBox_sfofr.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox_sfofr.AutoSize = true;
            this.checkBox_sfofr.Location = new System.Drawing.Point(277, 411);
            this.checkBox_sfofr.Name = "checkBox_sfofr";
            this.checkBox_sfofr.Size = new System.Drawing.Size(256, 17);
            this.checkBox_sfofr.TabIndex = 5;
            this.checkBox_sfofr.Text = "Показывать параметры при первом запросе";
            this.checkBox_sfofr.UseVisualStyleBackColor = true;
            this.checkBox_sfofr.CheckedChanged += new System.EventHandler(this.checkBox_sfofr_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 440);
            this.Controls.Add(this.checkBox_sfofr);
            this.Controls.Add(this.checkBox_sf);
            this.Controls.Add(this.button_import);
            this.Controls.Add(this.textBox_search);
            this.Controls.Add(this.button_search);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormSettings";
            this.Text = "Настройки";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormSettings_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_search;
        private System.Windows.Forms.TextBox textBox_search;
        private System.Windows.Forms.Button button_import;
        private System.Windows.Forms.CheckBox checkBox_sf;
        private System.Windows.Forms.CheckBox checkBox_sfofr;
    }
}