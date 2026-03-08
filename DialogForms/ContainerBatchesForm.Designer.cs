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
            this.dataGridView1.Size = new System.Drawing.Size(625, 426);
            this.dataGridView1.TabIndex = 0;
            // 
            // button_show
            // 
            this.button_show.Location = new System.Drawing.Point(643, 12);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(145, 23);
            this.button_show.TabIndex = 1;
            this.button_show.Text = "Открыть карту";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button_show_Click);
            // 
            // button_delete
            // 
            this.button_delete.Location = new System.Drawing.Point(643, 41);
            this.button_delete.Name = "button_delete";
            this.button_delete.Size = new System.Drawing.Size(145, 23);
            this.button_delete.TabIndex = 2;
            this.button_delete.Text = "Удалить";
            this.button_delete.UseVisualStyleBackColor = true;
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(643, 70);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(145, 23);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "Добавить массив";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_extract
            // 
            this.button_extract.Location = new System.Drawing.Point(643, 99);
            this.button_extract.Name = "button_extract";
            this.button_extract.Size = new System.Drawing.Size(145, 23);
            this.button_extract.TabIndex = 4;
            this.button_extract.Text = "Извлечь карту";
            this.button_extract.UseVisualStyleBackColor = true;
            this.button_extract.Click += new System.EventHandler(this.button_extract_Click);
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(643, 335);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(145, 23);
            this.button_select.TabIndex = 5;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // groupBox_showmode
            // 
            this.groupBox_showmode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBox_showmode.Controls.Add(this.radioButton_showRelevant);
            this.groupBox_showmode.Controls.Add(this.radioButton_shoall);
            this.groupBox_showmode.Location = new System.Drawing.Point(643, 364);
            this.groupBox_showmode.Name = "groupBox_showmode";
            this.groupBox_showmode.Size = new System.Drawing.Size(145, 74);
            this.groupBox_showmode.TabIndex = 6;
            this.groupBox_showmode.TabStop = false;
            this.groupBox_showmode.Text = "Отображение";
            // 
            // radioButton_showRelevant
            // 
            this.radioButton_showRelevant.AutoSize = true;
            this.radioButton_showRelevant.Location = new System.Drawing.Point(6, 39);
            this.radioButton_showRelevant.Name = "radioButton_showRelevant";
            this.radioButton_showRelevant.Size = new System.Drawing.Size(89, 17);
            this.radioButton_showRelevant.TabIndex = 1;
            this.radioButton_showRelevant.TabStop = true;
            this.radioButton_showRelevant.Text = "Подходящие";
            this.radioButton_showRelevant.UseVisualStyleBackColor = true;
            // 
            // radioButton_shoall
            // 
            this.radioButton_shoall.AutoSize = true;
            this.radioButton_shoall.Location = new System.Drawing.Point(6, 16);
            this.radioButton_shoall.Name = "radioButton_shoall";
            this.radioButton_shoall.Size = new System.Drawing.Size(95, 17);
            this.radioButton_shoall.TabIndex = 0;
            this.radioButton_shoall.TabStop = true;
            this.radioButton_shoall.Text = "Показать все";
            this.radioButton_shoall.UseVisualStyleBackColor = true;
            // 
            // ContainerBatchesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}