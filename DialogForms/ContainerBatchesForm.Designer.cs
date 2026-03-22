namespace ImgAnalyzer.DialogForms
{
    partial class ContainerBatchesForm
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
            this.button_show = new System.Windows.Forms.Button();
            this.button_delete = new System.Windows.Forms.Button();
            this.button_add = new System.Windows.Forms.Button();
            this.button_extract = new System.Windows.Forms.Button();
            this.button_select = new System.Windows.Forms.Button();
            this.groupBox_showmode = new System.Windows.Forms.GroupBox();
            this.radioButton_showRelevant = new System.Windows.Forms.RadioButton();
            this.radioButton_shoall = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button_deletefiles = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox_showmode.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(497, 444);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // button_show
            // 
            this.button_show.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_show.Location = new System.Drawing.Point(515, 12);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(143, 23);
            this.button_show.TabIndex = 1;
            this.button_show.Text = "Открыть карту";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button_show_Click);
            // 
            // button_delete
            // 
            this.button_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_delete.Location = new System.Drawing.Point(666, 12);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(143, 23);
            this.button_delete.TabIndex = 2;
            this.button_delete.Text = "Удалить";
            this.button_delete.UseVisualStyleBackColor = true;
            this.button_delete.Click += new System.EventHandler(this.button_delete_Click);
            // 
            // button_add
            // 
            this.button_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_add.Location = new System.Drawing.Point(515, 41);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(143, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "Добавить массив";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_extract
            // 
            this.button_extract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_extract.Location = new System.Drawing.Point(515, 70);
            this.button_extract.Name = "button_extract";
            this.button_extract.Size = new System.Drawing.Size(143, 23);
            this.button_extract.TabIndex = 4;
            this.button_extract.Text = "Извлечь карту";
            this.button_extract.UseVisualStyleBackColor = true;
            this.button_extract.Click += new System.EventHandler(this.button_extract_Click);
            // 
            // button_select
            // 
            this.button_select.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_select.Location = new System.Drawing.Point(675, 382);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(145, 73);
            this.button_select.TabIndex = 5;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // groupBox_showmode
            // 
            this.groupBox_showmode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_showmode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox_showmode.Controls.Add(this.radioButton_showRelevant);
            this.groupBox_showmode.Controls.Add(this.radioButton_shoall);
            this.groupBox_showmode.Location = new System.Drawing.Point(524, 382);
            this.groupBox_showmode.Name = "groupBox_showmode";
            this.groupBox_showmode.Size = new System.Drawing.Size(145, 74);
            this.groupBox_showmode.TabIndex = 6;
            this.groupBox_showmode.TabStop = false;
            this.groupBox_showmode.Text = "Отображение";
            // 
            // radioButton_showRelevant
            // 
            this.radioButton_showRelevant.AutoSize = true;
            this.radioButton_showRelevant.Checked = true;
            this.radioButton_showRelevant.Location = new System.Drawing.Point(6, 39);
            this.radioButton_showRelevant.Name = "radioButton_showRelevant";
            this.radioButton_showRelevant.Size = new System.Drawing.Size(89, 17);
            this.radioButton_showRelevant.TabIndex = 1;
            this.radioButton_showRelevant.TabStop = true;
            this.radioButton_showRelevant.Text = "Подходящие";
            this.radioButton_showRelevant.UseVisualStyleBackColor = true;
            this.radioButton_showRelevant.CheckedChanged += new System.EventHandler(this.radioButton_showRelevant_CheckedChanged);
            // 
            // radioButton_shoall
            // 
            this.radioButton_shoall.AutoSize = true;
            this.radioButton_shoall.Location = new System.Drawing.Point(6, 17);
            this.radioButton_shoall.Name = "radioButton_shoall";
            this.radioButton_shoall.Size = new System.Drawing.Size(95, 17);
            this.radioButton_shoall.TabIndex = 0;
            this.radioButton_shoall.Text = "Показать все";
            this.radioButton_shoall.UseVisualStyleBackColor = true;
            this.radioButton_shoall.CheckedChanged += new System.EventHandler(this.radioButton_shoall_CheckedChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.richTextBox1.Location = new System.Drawing.Point(515, 99);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(312, 277);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // button_deletefiles
            // 
            this.button_deletefiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_deletefiles.Location = new System.Drawing.Point(666, 41);
            this.button_deletefiles.Name = "button_deletefiles";
            this.button_deletefiles.Size = new System.Drawing.Size(143, 23);
            this.button_deletefiles.TabIndex = 8;
            this.button_deletefiles.Text = "Удалить с файлами";
            this.button_deletefiles.UseVisualStyleBackColor = true;
            this.button_deletefiles.Click += new System.EventHandler(this.button_deletefiles_Click);
            // 
            // ContainerBatchesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 468);
            this.Controls.Add(this.button_deletefiles);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox_showmode);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.button_extract);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.button_delete);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ContainerBatchesForm";
            this.Text = "2D Container Batches";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox_showmode.ResumeLayout(false);
            this.groupBox_showmode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button_show;
        private System.Windows.Forms.Button button_delete;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_extract;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.GroupBox groupBox_showmode;
        private System.Windows.Forms.RadioButton radioButton_showRelevant;
        private System.Windows.Forms.RadioButton radioButton_shoall;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button_deletefiles;
    }
}