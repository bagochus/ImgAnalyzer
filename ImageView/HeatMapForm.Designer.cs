namespace ImgAnalyzer
{
    partial class HeatMapForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_max = new System.Windows.Forms.TextBox();
            this.textBox_min = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.измеренияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.срезПоXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.срезПоYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.гистограммаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.двоичныйФайлbinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изображениеbmpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox_scalebar = new System.Windows.Forms.PictureBox();
            this.button_update = new System.Windows.Forms.Button();
            this.CoordinateLabel = new System.Windows.Forms.Label();
            this.сброситьИзображениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_scalebar)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(121, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Max";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Min";
            // 
            // textBox_max
            // 
            this.textBox_max.Location = new System.Drawing.Point(154, 30);
            this.textBox_max.Name = "textBox_max";
            this.textBox_max.Size = new System.Drawing.Size(68, 20);
            this.textBox_max.TabIndex = 4;
            // 
            // textBox_min
            // 
            this.textBox_min.Location = new System.Drawing.Point(45, 30);
            this.textBox_min.Name = "textBox_min";
            this.textBox_min.Size = new System.Drawing.Size(68, 20);
            this.textBox_min.TabIndex = 5;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGreen;
            this.pictureBox1.Location = new System.Drawing.Point(12, 81);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(625, 484);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.измеренияToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.сброситьИзображениеToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 24);
            this.menuStrip1.TabIndex = 7;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // измеренияToolStripMenuItem
            // 
            this.измеренияToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.срезПоXToolStripMenuItem,
            this.срезПоYToolStripMenuItem,
            this.гистограммаToolStripMenuItem});
            this.измеренияToolStripMenuItem.Name = "измеренияToolStripMenuItem";
            this.измеренияToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
            this.измеренияToolStripMenuItem.Text = "Измерения";
            // 
            // срезПоXToolStripMenuItem
            // 
            this.срезПоXToolStripMenuItem.Name = "срезПоXToolStripMenuItem";
            this.срезПоXToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.срезПоXToolStripMenuItem.Text = "Срез по X";
            this.срезПоXToolStripMenuItem.Click += new System.EventHandler(this.срезПоXToolStripMenuItem_Click);
            // 
            // срезПоYToolStripMenuItem
            // 
            this.срезПоYToolStripMenuItem.Name = "срезПоYToolStripMenuItem";
            this.срезПоYToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.срезПоYToolStripMenuItem.Text = "Срез по Y";
            this.срезПоYToolStripMenuItem.Click += new System.EventHandler(this.срезПоYToolStripMenuItem_Click);
            // 
            // гистограммаToolStripMenuItem
            // 
            this.гистограммаToolStripMenuItem.Name = "гистограммаToolStripMenuItem";
            this.гистограммаToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.гистограммаToolStripMenuItem.Text = "Гистограмма";
            this.гистограммаToolStripMenuItem.Click += new System.EventHandler(this.гистограммаToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.двоичныйФайлbinToolStripMenuItem,
            this.изображениеbmpToolStripMenuItem});
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            // 
            // двоичныйФайлbinToolStripMenuItem
            // 
            this.двоичныйФайлbinToolStripMenuItem.Name = "двоичныйФайлbinToolStripMenuItem";
            this.двоичныйФайлbinToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.двоичныйФайлbinToolStripMenuItem.Text = "Двоичный файл (.bin)";
            // 
            // изображениеbmpToolStripMenuItem
            // 
            this.изображениеbmpToolStripMenuItem.Name = "изображениеbmpToolStripMenuItem";
            this.изображениеbmpToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.изображениеbmpToolStripMenuItem.Text = "Изображение (.bmp)";
            // 
            // pictureBox_scalebar
            // 
            this.pictureBox_scalebar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_scalebar.Location = new System.Drawing.Point(12, 56);
            this.pictureBox_scalebar.Name = "pictureBox_scalebar";
            this.pictureBox_scalebar.Size = new System.Drawing.Size(625, 19);
            this.pictureBox_scalebar.TabIndex = 8;
            this.pictureBox_scalebar.TabStop = false;
            // 
            // button_update
            // 
            this.button_update.Location = new System.Drawing.Point(228, 28);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(75, 23);
            this.button_update.TabIndex = 9;
            this.button_update.Text = "Обновить";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // CoordinateLabel
            // 
            this.CoordinateLabel.AutoSize = true;
            this.CoordinateLabel.Location = new System.Drawing.Point(309, 33);
            this.CoordinateLabel.Name = "CoordinateLabel";
            this.CoordinateLabel.Size = new System.Drawing.Size(0, 13);
            this.CoordinateLabel.TabIndex = 10;
            // 
            // сброситьИзображениеToolStripMenuItem
            // 
            this.сброситьИзображениеToolStripMenuItem.Name = "сброситьИзображениеToolStripMenuItem";
            this.сброситьИзображениеToolStripMenuItem.Size = new System.Drawing.Size(149, 20);
            this.сброситьИзображениеToolStripMenuItem.Text = "Сбросить изображение";
            this.сброситьИзображениеToolStripMenuItem.Click += new System.EventHandler(this.сброситьИзображениеToolStripMenuItem_Click);
            // 
            // HeatMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 577);
            this.Controls.Add(this.CoordinateLabel);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.pictureBox_scalebar);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.textBox_min);
            this.Controls.Add(this.textBox_max);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HeatMapForm";
            this.Text = "HeatMapForm";
            this.Resize += new System.EventHandler(this.HeatMapForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_scalebar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_max;
        private System.Windows.Forms.TextBox textBox_min;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem измеренияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem срезПоXToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem срезПоYToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem гистограммаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem двоичныйФайлbinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem изображениеbmpToolStripMenuItem;
        private System.Windows.Forms.PictureBox pictureBox_scalebar;
        private System.Windows.Forms.Button button_update;
        private System.Windows.Forms.Label CoordinateLabel;
        private System.Windows.Forms.ToolStripMenuItem сброситьИзображениеToolStripMenuItem;
    }
}