namespace ImgAnalyzer
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_openfile = new System.Windows.Forms.Button();
            this.button_show = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button_corners = new System.Windows.Forms.Button();
            this.button_openfiles = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_start = new System.Windows.Forms.TextBox();
            this.textBox_step = new System.Windows.Forms.TextBox();
            this.label_var1 = new System.Windows.Forms.Label();
            this.label_var2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_addpoints = new System.Windows.Forms.Button();
            this.button_addpolygon = new System.Windows.Forms.Button();
            this.button_addmatrix = new System.Windows.Forms.Button();
            this.listBox_measurments = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_unitname = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_ny = new System.Windows.Forms.TextBox();
            this.textBox_nx = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button_start = new System.Windows.Forms.Button();
            this.button_plot = new System.Windows.Forms.Button();
            this.label_status = new System.Windows.Forms.Label();
            this.textBox_varname = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button_rename = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.button_save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.button_calcminmax = new System.Windows.Forms.Button();
            this.button_pseudoph = new System.Windows.Forms.Button();
            this.button_deadpix = new System.Windows.Forms.Button();
            this.button_map = new System.Windows.Forms.Button();
            this.button_ab = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_openfile
            // 
            this.button_openfile.Location = new System.Drawing.Point(12, 12);
            this.button_openfile.Name = "button_openfile";
            this.button_openfile.Size = new System.Drawing.Size(190, 23);
            this.button_openfile.TabIndex = 0;
            this.button_openfile.Text = "Выбрать файл для  прицеливания";
            this.button_openfile.UseVisualStyleBackColor = true;
            this.button_openfile.Click += new System.EventHandler(this.button_openfile_Click);
            // 
            // button_show
            // 
            this.button_show.Location = new System.Drawing.Point(12, 41);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(190, 23);
            this.button_show.TabIndex = 1;
            this.button_show.Text = "Показать изображение";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 247);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(190, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Измерить пиксель";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button_corners
            // 
            this.button_corners.Location = new System.Drawing.Point(12, 276);
            this.button_corners.Name = "button_corners";
            this.button_corners.Size = new System.Drawing.Size(190, 23);
            this.button_corners.TabIndex = 3;
            this.button_corners.Text = "Задать углы";
            this.button_corners.UseVisualStyleBackColor = true;
            this.button_corners.Click += new System.EventHandler(this.button_corners_Click);
            // 
            // button_openfiles
            // 
            this.button_openfiles.Location = new System.Drawing.Point(12, 70);
            this.button_openfiles.Name = "button_openfiles";
            this.button_openfiles.Size = new System.Drawing.Size(190, 23);
            this.button_openfiles.TabIndex = 4;
            this.button_openfiles.Text = "Выбрать массив файлов";
            this.button_openfiles.UseVisualStyleBackColor = true;
            this.button_openfiles.Click += new System.EventHandler(this.button_openfiles_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 26);
            this.label1.TabIndex = 5;
            this.label1.Text = "Изображения соответсвуют\r\n напряжениям:";
            // 
            // textBox_start
            // 
            this.textBox_start.Location = new System.Drawing.Point(71, 137);
            this.textBox_start.Name = "textBox_start";
            this.textBox_start.Size = new System.Drawing.Size(100, 20);
            this.textBox_start.TabIndex = 6;
            // 
            // textBox_step
            // 
            this.textBox_step.Location = new System.Drawing.Point(71, 163);
            this.textBox_step.Name = "textBox_step";
            this.textBox_step.Size = new System.Drawing.Size(100, 20);
            this.textBox_step.TabIndex = 7;
            // 
            // label_var1
            // 
            this.label_var1.AutoSize = true;
            this.label_var1.Location = new System.Drawing.Point(177, 141);
            this.label_var1.Name = "label_var1";
            this.label_var1.Size = new System.Drawing.Size(14, 13);
            this.label_var1.TabIndex = 8;
            this.label_var1.Text = "В";
            // 
            // label_var2
            // 
            this.label_var2.AutoSize = true;
            this.label_var2.Location = new System.Drawing.Point(177, 167);
            this.label_var2.Name = "label_var2";
            this.label_var2.Size = new System.Drawing.Size(14, 13);
            this.label_var2.TabIndex = 9;
            this.label_var2.Text = "В";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(45, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "От";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "С шагом";
            // 
            // button_addpoints
            // 
            this.button_addpoints.Location = new System.Drawing.Point(220, 41);
            this.button_addpoints.Name = "button_addpoints";
            this.button_addpoints.Size = new System.Drawing.Size(190, 23);
            this.button_addpoints.TabIndex = 12;
            this.button_addpoints.Text = "Выбрать точки";
            this.button_addpoints.UseVisualStyleBackColor = true;
            this.button_addpoints.Click += new System.EventHandler(this.button_addpoints_Click);
            // 
            // button_addpolygon
            // 
            this.button_addpolygon.Location = new System.Drawing.Point(220, 70);
            this.button_addpolygon.Name = "button_addpolygon";
            this.button_addpolygon.Size = new System.Drawing.Size(190, 23);
            this.button_addpolygon.TabIndex = 13;
            this.button_addpolygon.Text = "Выбрать полигон";
            this.button_addpolygon.UseVisualStyleBackColor = true;
            this.button_addpolygon.Click += new System.EventHandler(this.button_addpolygon_Click);
            // 
            // button_addmatrix
            // 
            this.button_addmatrix.Enabled = false;
            this.button_addmatrix.Location = new System.Drawing.Point(220, 99);
            this.button_addmatrix.Name = "button_addmatrix";
            this.button_addmatrix.Size = new System.Drawing.Size(190, 23);
            this.button_addmatrix.TabIndex = 14;
            this.button_addmatrix.Text = "Выбрать массив";
            this.button_addmatrix.UseVisualStyleBackColor = true;
            this.button_addmatrix.Click += new System.EventHandler(this.button_addmatrix_Click);
            // 
            // listBox_measurments
            // 
            this.listBox_measurments.FormattingEnabled = true;
            this.listBox_measurments.Location = new System.Drawing.Point(433, 12);
            this.listBox_measurments.Name = "listBox_measurments";
            this.listBox_measurments.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_measurments.Size = new System.Drawing.Size(146, 303);
            this.listBox_measurments.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(226, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Измерения";
            // 
            // textBox_unitname
            // 
            this.textBox_unitname.Location = new System.Drawing.Point(91, 218);
            this.textBox_unitname.Name = "textBox_unitname";
            this.textBox_unitname.Size = new System.Drawing.Size(100, 20);
            this.textBox_unitname.TabIndex = 17;
            this.textBox_unitname.TextChanged += new System.EventHandler(this.textBox_varname_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 192);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Переменная";
            // 
            // textBox_ny
            // 
            this.textBox_ny.Location = new System.Drawing.Point(310, 160);
            this.textBox_ny.Name = "textBox_ny";
            this.textBox_ny.Size = new System.Drawing.Size(100, 20);
            this.textBox_ny.TabIndex = 19;
            // 
            // textBox_nx
            // 
            this.textBox_nx.Location = new System.Drawing.Point(310, 134);
            this.textBox_nx.Name = "textBox_nx";
            this.textBox_nx.Size = new System.Drawing.Size(100, 20);
            this.textBox_nx.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(237, 166);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Блоков по y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(237, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Блоков по x";
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(220, 196);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(190, 74);
            this.button_start.TabIndex = 23;
            this.button_start.Text = "Запустить измерения";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // button_plot
            // 
            this.button_plot.Location = new System.Drawing.Point(594, 12);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(146, 23);
            this.button_plot.TabIndex = 24;
            this.button_plot.Text = "Построить график";
            this.button_plot.UseMnemonic = false;
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.Location = new System.Drawing.Point(217, 276);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(0, 13);
            this.label_status.TabIndex = 25;
            // 
            // textBox_varname
            // 
            this.textBox_varname.Location = new System.Drawing.Point(91, 192);
            this.textBox_varname.Name = "textBox_varname";
            this.textBox_varname.Size = new System.Drawing.Size(100, 20);
            this.textBox_varname.TabIndex = 26;
            this.textBox_varname.TextChanged += new System.EventHandler(this.textBox_varname_TextChanged_1);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 221);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 27;
            this.label8.Text = "Ед.изм.";
            // 
            // button_rename
            // 
            this.button_rename.Location = new System.Drawing.Point(594, 41);
            this.button_rename.Name = "button_rename";
            this.button_rename.Size = new System.Drawing.Size(146, 23);
            this.button_rename.TabIndex = 28;
            this.button_rename.Text = "Переименовать";
            this.button_rename.UseVisualStyleBackColor = true;
            this.button_rename.Click += new System.EventHandler(this.button_rename_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(594, 70);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(146, 23);
            this.button_clear.TabIndex = 29;
            this.button_clear.Text = "Удалить";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // button_save
            // 
            this.button_save.Enabled = false;
            this.button_save.Location = new System.Drawing.Point(594, 99);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(146, 23);
            this.button_save.TabIndex = 30;
            this.button_save.Text = "Сохранить";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_load
            // 
            this.button_load.Enabled = false;
            this.button_load.Location = new System.Drawing.Point(594, 128);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(146, 23);
            this.button_load.TabIndex = 31;
            this.button_load.Text = "Загрузить";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_calcminmax
            // 
            this.button_calcminmax.Location = new System.Drawing.Point(594, 187);
            this.button_calcminmax.Name = "button_calcminmax";
            this.button_calcminmax.Size = new System.Drawing.Size(146, 23);
            this.button_calcminmax.TabIndex = 32;
            this.button_calcminmax.Text = "Расчитать амплитуды";
            this.button_calcminmax.UseVisualStyleBackColor = true;
            this.button_calcminmax.Click += new System.EventHandler(this.button_calcminmax_Click);
            // 
            // button_pseudoph
            // 
            this.button_pseudoph.Location = new System.Drawing.Point(594, 215);
            this.button_pseudoph.Name = "button_pseudoph";
            this.button_pseudoph.Size = new System.Drawing.Size(146, 23);
            this.button_pseudoph.TabIndex = 33;
            this.button_pseudoph.Text = "Расчитать псевдо-фазы";
            this.button_pseudoph.UseVisualStyleBackColor = true;
            this.button_pseudoph.Click += new System.EventHandler(this.button_pseudoph_Click);
            // 
            // button_deadpix
            // 
            this.button_deadpix.Location = new System.Drawing.Point(594, 244);
            this.button_deadpix.Name = "button_deadpix";
            this.button_deadpix.Size = new System.Drawing.Size(146, 23);
            this.button_deadpix.TabIndex = 34;
            this.button_deadpix.Text = "Отметить битые пиксели";
            this.button_deadpix.UseVisualStyleBackColor = true;
            this.button_deadpix.Click += new System.EventHandler(this.button_deadpix_Click);
            // 
            // button_map
            // 
            this.button_map.Location = new System.Drawing.Point(594, 273);
            this.button_map.Name = "button_map";
            this.button_map.Size = new System.Drawing.Size(146, 23);
            this.button_map.TabIndex = 35;
            this.button_map.Text = "Построить карту";
            this.button_map.UseVisualStyleBackColor = true;
            this.button_map.Click += new System.EventHandler(this.button_map_Click);
            // 
            // button_ab
            // 
            this.button_ab.Location = new System.Drawing.Point(594, 302);
            this.button_ab.Name = "button_ab";
            this.button_ab.Size = new System.Drawing.Size(146, 23);
            this.button_ab.TabIndex = 36;
            this.button_ab.Text = "Рассчитать A-B";
            this.button_ab.UseVisualStyleBackColor = true;
            this.button_ab.Click += new System.EventHandler(this.button_ab_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_ab);
            this.Controls.Add(this.button_map);
            this.Controls.Add(this.button_deadpix);
            this.Controls.Add(this.button_pseudoph);
            this.Controls.Add(this.button_calcminmax);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_rename);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox_varname);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_nx);
            this.Controls.Add(this.textBox_ny);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_unitname);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBox_measurments);
            this.Controls.Add(this.button_addmatrix);
            this.Controls.Add(this.button_addpolygon);
            this.Controls.Add(this.button_addpoints);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label_var2);
            this.Controls.Add(this.label_var1);
            this.Controls.Add(this.textBox_step);
            this.Controls.Add(this.textBox_start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_openfiles);
            this.Controls.Add(this.button_corners);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.button_openfile);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_openfile;
        private System.Windows.Forms.Button button_show;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button_corners;
        private System.Windows.Forms.Button button_openfiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_start;
        private System.Windows.Forms.TextBox textBox_step;
        private System.Windows.Forms.Label label_var1;
        private System.Windows.Forms.Label label_var2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_addpoints;
        private System.Windows.Forms.Button button_addpolygon;
        private System.Windows.Forms.Button button_addmatrix;
        private System.Windows.Forms.ListBox listBox_measurments;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_unitname;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_ny;
        private System.Windows.Forms.TextBox textBox_nx;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_plot;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.TextBox textBox_varname;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_rename;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_calcminmax;
        private System.Windows.Forms.Button button_pseudoph;
        private System.Windows.Forms.Button button_deadpix;
        private System.Windows.Forms.Button button_map;
        private System.Windows.Forms.Button button_ab;
    }
}

