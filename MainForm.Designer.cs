namespace ImgAnalyzer
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button_selctfiles = new System.Windows.Forms.Button();
            this.button_imgview = new System.Windows.Forms.Button();
            this.button_2d = new System.Windows.Forms.Button();
            this.button_1d = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.автодобавлениеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.профильToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьПрофильToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьПрофильToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.настройкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button_showContainers = new System.Windows.Forms.Button();
            this.button_auto_phase = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_selctfiles
            // 
            this.button_selctfiles.Location = new System.Drawing.Point(12, 27);
            this.button_selctfiles.Name = "button_selctfiles";
            this.button_selctfiles.Size = new System.Drawing.Size(145, 45);
            this.button_selctfiles.TabIndex = 0;
            this.button_selctfiles.Text = "Выбрать папку с файлами";
            this.button_selctfiles.UseVisualStyleBackColor = true;
            this.button_selctfiles.Click += new System.EventHandler(this.button_selctfiles_Click);
            // 
            // button_imgview
            // 
            this.button_imgview.Location = new System.Drawing.Point(12, 77);
            this.button_imgview.Name = "button_imgview";
            this.button_imgview.Size = new System.Drawing.Size(145, 45);
            this.button_imgview.TabIndex = 1;
            this.button_imgview.Text = "Открыть изображение для прицеливания";
            this.button_imgview.UseVisualStyleBackColor = true;
            this.button_imgview.Click += new System.EventHandler(this.button_imgview_Click);
            // 
            // button_2d
            // 
            this.button_2d.Image = ((System.Drawing.Image)(resources.GetObject("button_2d.Image")));
            this.button_2d.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_2d.Location = new System.Drawing.Point(354, 27);
            this.button_2d.Name = "button_2d";
            this.button_2d.Size = new System.Drawing.Size(157, 95);
            this.button_2d.TabIndex = 2;
            this.button_2d.Text = "2D Данные";
            this.button_2d.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_2d.UseVisualStyleBackColor = true;
            this.button_2d.Click += new System.EventHandler(this.button_2d_Click);
            // 
            // button_1d
            // 
            this.button_1d.Image = ((System.Drawing.Image)(resources.GetObject("button_1d.Image")));
            this.button_1d.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button_1d.Location = new System.Drawing.Point(178, 27);
            this.button_1d.Name = "button_1d";
            this.button_1d.Size = new System.Drawing.Size(157, 95);
            this.button_1d.TabIndex = 3;
            this.button_1d.Text = "1D Данные";
            this.button_1d.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_1d.UseVisualStyleBackColor = true;
            this.button_1d.Click += new System.EventHandler(this.button_1d_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.профильToolStripMenuItem,
            this.настройкиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.автодобавлениеToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // автодобавлениеToolStripMenuItem
            // 
            this.автодобавлениеToolStripMenuItem.Name = "автодобавлениеToolStripMenuItem";
            this.автодобавлениеToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.автодобавлениеToolStripMenuItem.Text = "Автодобавление";
            // 
            // профильToolStripMenuItem
            // 
            this.профильToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.сохранитьПрофильToolStripMenuItem,
            this.загрузитьПрофильToolStripMenuItem,
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem});
            this.профильToolStripMenuItem.Name = "профильToolStripMenuItem";
            this.профильToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.профильToolStripMenuItem.Text = "Профиль";
            // 
            // сохранитьПрофильToolStripMenuItem
            // 
            this.сохранитьПрофильToolStripMenuItem.Name = "сохранитьПрофильToolStripMenuItem";
            this.сохранитьПрофильToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.сохранитьПрофильToolStripMenuItem.Text = "Сохранить профиль";
            this.сохранитьПрофильToolStripMenuItem.Click += new System.EventHandler(this.сохранитьПрофильToolStripMenuItem_Click);
            // 
            // загрузитьПрофильToolStripMenuItem
            // 
            this.загрузитьПрофильToolStripMenuItem.Name = "загрузитьПрофильToolStripMenuItem";
            this.загрузитьПрофильToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.загрузитьПрофильToolStripMenuItem.Text = "Загрузить профиль";
            this.загрузитьПрофильToolStripMenuItem.Click += new System.EventHandler(this.загрузитьПрофильToolStripMenuItem_Click);
            // 
            // загрузитьТолькоАктивныеОбластиToolStripMenuItem
            // 
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem.Name = "загрузитьТолькоАктивныеОбластиToolStripMenuItem";
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem.Text = "Загрузить только активные области";
            this.загрузитьТолькоАктивныеОбластиToolStripMenuItem.Click += new System.EventHandler(this.загрузитьТолькоАктивныеОбластиToolStripMenuItem_Click);
            // 
            // настройкиToolStripMenuItem
            // 
            this.настройкиToolStripMenuItem.Name = "настройкиToolStripMenuItem";
            this.настройкиToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.настройкиToolStripMenuItem.Text = "Настройки";
            this.настройкиToolStripMenuItem.Click += new System.EventHandler(this.настройкиToolStripMenuItem_Click);
            // 
            // button_showContainers
            // 
            this.button_showContainers.Location = new System.Drawing.Point(12, 128);
            this.button_showContainers.Name = "button_showContainers";
            this.button_showContainers.Size = new System.Drawing.Size(145, 43);
            this.button_showContainers.TabIndex = 5;
            this.button_showContainers.Text = "Массивы 2D карт";
            this.button_showContainers.UseVisualStyleBackColor = true;
            this.button_showContainers.Click += new System.EventHandler(this.button_showContainers_Click);
            // 
            // button_auto_phase
            // 
            this.button_auto_phase.Location = new System.Drawing.Point(12, 177);
            this.button_auto_phase.Name = "button_auto_phase";
            this.button_auto_phase.Size = new System.Drawing.Size(145, 43);
            this.button_auto_phase.TabIndex = 6;
            this.button_auto_phase.Text = "Авто - обработка";
            this.button_auto_phase.UseVisualStyleBackColor = true;
            this.button_auto_phase.Click += new System.EventHandler(this.button_auto_phase_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_auto_phase);
            this.Controls.Add(this.button_showContainers);
            this.Controls.Add(this.button_1d);
            this.Controls.Add(this.button_2d);
            this.Controls.Add(this.button_imgview);
            this.Controls.Add(this.button_selctfiles);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_selctfiles;
        private System.Windows.Forms.Button button_imgview;
        private System.Windows.Forms.Button button_2d;
        private System.Windows.Forms.Button button_1d;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem профильToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьПрофильToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьПрофильToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загрузитьТолькоАктивныеОбластиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem настройкиToolStripMenuItem;
        private System.Windows.Forms.Button button_showContainers;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem автодобавлениеToolStripMenuItem;
        private System.Windows.Forms.Button button_auto_phase;
    }
}