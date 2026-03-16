namespace ImgAnalyzer
{
    partial class NewMainForm
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
            this.components = new System.ComponentModel.Container();
            this.comboBox_sample = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_newSample = new System.Windows.Forms.Button();
            this.checkBox_auto_sq = new System.Windows.Forms.CheckBox();
            this.button_infoAutoSquare = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button_hintphase = new System.Windows.Forms.Button();
            this.button_hint_stitch = new System.Windows.Forms.Button();
            this.button_hint_loot = new System.Windows.Forms.Button();
            this.groupBox_phase = new System.Windows.Forms.GroupBox();
            this.label_selected_phase = new System.Windows.Forms.Label();
            this.button_select_phase = new System.Windows.Forms.Button();
            this.radioButton_batch_phase = new System.Windows.Forms.RadioButton();
            this.radioButton_calc = new System.Windows.Forms.RadioButton();
            this.richTextBox_sample = new System.Windows.Forms.RichTextBox();
            this.richTextBox_batch = new System.Windows.Forms.RichTextBox();
            this.groupBox_stitch = new System.Windows.Forms.GroupBox();
            this.label_selected_stitch = new System.Windows.Forms.Label();
            this.button_select_stitch = new System.Windows.Forms.Button();
            this.radioButton_batch_stitch = new System.Windows.Forms.RadioButton();
            this.radioButton_stitch = new System.Windows.Forms.RadioButton();
            this.radioButton_nostitich = new System.Windows.Forms.RadioButton();
            this.groupBox_lut = new System.Windows.Forms.GroupBox();
            this.radioButton_lut = new System.Windows.Forms.RadioButton();
            this.radioButton_no_lut = new System.Windows.Forms.RadioButton();
            this.button_start = new System.Windows.Forms.Button();
            this.checkBox_req_params = new System.Windows.Forms.CheckBox();
            this.groupBox_phase.SuspendLayout();
            this.groupBox_stitch.SuspendLayout();
            this.groupBox_lut.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox_sample
            // 
            this.comboBox_sample.FormattingEnabled = true;
            this.comboBox_sample.Location = new System.Drawing.Point(26, 36);
            this.comboBox_sample.Name = "comboBox_sample";
            this.comboBox_sample.Size = new System.Drawing.Size(291, 21);
            this.comboBox_sample.TabIndex = 0;
            this.comboBox_sample.SelectedIndexChanged += new System.EventHandler(this.comboBox_sample_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Образец";
            // 
            // button_newSample
            // 
            this.button_newSample.Location = new System.Drawing.Point(26, 63);
            this.button_newSample.Name = "button_newSample";
            this.button_newSample.Size = new System.Drawing.Size(291, 23);
            this.button_newSample.TabIndex = 2;
            this.button_newSample.Text = "Новый образец";
            this.button_newSample.UseVisualStyleBackColor = true;
            this.button_newSample.Click += new System.EventHandler(this.button_newSample_Click);
            // 
            // checkBox_auto_sq
            // 
            this.checkBox_auto_sq.AutoSize = true;
            this.checkBox_auto_sq.Location = new System.Drawing.Point(340, 130);
            this.checkBox_auto_sq.Name = "checkBox_auto_sq";
            this.checkBox_auto_sq.Size = new System.Drawing.Size(177, 17);
            this.checkBox_auto_sq.TabIndex = 5;
            this.checkBox_auto_sq.Text = "Авто поиск активной области";
            this.checkBox_auto_sq.UseVisualStyleBackColor = true;
            this.checkBox_auto_sq.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button_infoAutoSquare
            // 
            this.button_infoAutoSquare.Location = new System.Drawing.Point(523, 126);
            this.button_infoAutoSquare.Name = "button_infoAutoSquare";
            this.button_infoAutoSquare.Size = new System.Drawing.Size(23, 23);
            this.button_infoAutoSquare.TabIndex = 7;
            this.button_infoAutoSquare.Text = "?";
            this.toolTip1.SetToolTip(this.button_infoAutoSquare, "Подробнее об алгоритме");
            this.button_infoAutoSquare.UseVisualStyleBackColor = true;
            // 
            // button_hintphase
            // 
            this.button_hintphase.Location = new System.Drawing.Point(245, 16);
            this.button_hintphase.Name = "button_hintphase";
            this.button_hintphase.Size = new System.Drawing.Size(23, 23);
            this.button_hintphase.TabIndex = 8;
            this.button_hintphase.Text = "?";
            this.toolTip1.SetToolTip(this.button_hintphase, "Подробнее об алгоритме");
            this.button_hintphase.UseVisualStyleBackColor = true;
            // 
            // button_hint_stitch
            // 
            this.button_hint_stitch.Location = new System.Drawing.Point(245, 35);
            this.button_hint_stitch.Name = "button_hint_stitch";
            this.button_hint_stitch.Size = new System.Drawing.Size(23, 23);
            this.button_hint_stitch.TabIndex = 14;
            this.button_hint_stitch.Text = "?";
            this.toolTip1.SetToolTip(this.button_hint_stitch, "Подробнее об алгоритме");
            this.button_hint_stitch.UseVisualStyleBackColor = true;
            // 
            // button_hint_loot
            // 
            this.button_hint_loot.Location = new System.Drawing.Point(245, 42);
            this.button_hint_loot.Name = "button_hint_loot";
            this.button_hint_loot.Size = new System.Drawing.Size(23, 23);
            this.button_hint_loot.TabIndex = 15;
            this.button_hint_loot.Text = "?";
            this.toolTip1.SetToolTip(this.button_hint_loot, "Подробнее об алгоритме");
            this.button_hint_loot.UseVisualStyleBackColor = true;
            // 
            // groupBox_phase
            // 
            this.groupBox_phase.Controls.Add(this.label_selected_phase);
            this.groupBox_phase.Controls.Add(this.button_select_phase);
            this.groupBox_phase.Controls.Add(this.button_hintphase);
            this.groupBox_phase.Controls.Add(this.radioButton_batch_phase);
            this.groupBox_phase.Controls.Add(this.radioButton_calc);
            this.groupBox_phase.Location = new System.Drawing.Point(26, 130);
            this.groupBox_phase.Name = "groupBox_phase";
            this.groupBox_phase.Size = new System.Drawing.Size(306, 106);
            this.groupBox_phase.TabIndex = 8;
            this.groupBox_phase.TabStop = false;
            this.groupBox_phase.Text = "Расчет фазы";
            // 
            // label_selected_phase
            // 
            this.label_selected_phase.AutoSize = true;
            this.label_selected_phase.Location = new System.Drawing.Point(6, 74);
            this.label_selected_phase.Name = "label_selected_phase";
            this.label_selected_phase.Size = new System.Drawing.Size(68, 13);
            this.label_selected_phase.TabIndex = 16;
            this.label_selected_phase.Text = "Не выбрано";
            this.label_selected_phase.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label_selected_phase.Visible = false;
            // 
            // button_select_phase
            // 
            this.button_select_phase.Location = new System.Drawing.Point(231, 45);
            this.button_select_phase.Name = "button_select_phase";
            this.button_select_phase.Size = new System.Drawing.Size(69, 23);
            this.button_select_phase.TabIndex = 16;
            this.button_select_phase.Text = "Выбрать";
            this.button_select_phase.UseVisualStyleBackColor = true;
            this.button_select_phase.Visible = false;
            this.button_select_phase.Click += new System.EventHandler(this.button6_Click);
            // 
            // radioButton_batch_phase
            // 
            this.radioButton_batch_phase.AutoSize = true;
            this.radioButton_batch_phase.Location = new System.Drawing.Point(6, 45);
            this.radioButton_batch_phase.Name = "radioButton_batch_phase";
            this.radioButton_batch_phase.Size = new System.Drawing.Size(215, 17);
            this.radioButton_batch_phase.TabIndex = 1;
            this.radioButton_batch_phase.TabStop = true;
            this.radioButton_batch_phase.Text = "Использовать готовый пакет данных";
            this.radioButton_batch_phase.UseVisualStyleBackColor = true;
            // 
            // radioButton_calc
            // 
            this.radioButton_calc.AutoSize = true;
            this.radioButton_calc.Checked = true;
            this.radioButton_calc.Location = new System.Drawing.Point(6, 22);
            this.radioButton_calc.Name = "radioButton_calc";
            this.radioButton_calc.Size = new System.Drawing.Size(205, 17);
            this.radioButton_calc.TabIndex = 0;
            this.radioButton_calc.TabStop = true;
            this.radioButton_calc.Text = "Рассчитать фазу по изображениям";
            this.radioButton_calc.UseVisualStyleBackColor = true;
            // 
            // richTextBox_sample
            // 
            this.richTextBox_sample.Location = new System.Drawing.Point(338, 36);
            this.richTextBox_sample.Name = "richTextBox_sample";
            this.richTextBox_sample.Size = new System.Drawing.Size(420, 78);
            this.richTextBox_sample.TabIndex = 11;
            this.richTextBox_sample.Text = "";
            // 
            // richTextBox_batch
            // 
            this.richTextBox_batch.Location = new System.Drawing.Point(340, 161);
            this.richTextBox_batch.Name = "richTextBox_batch";
            this.richTextBox_batch.Size = new System.Drawing.Size(418, 263);
            this.richTextBox_batch.TabIndex = 9;
            this.richTextBox_batch.Text = "";
            // 
            // groupBox_stitch
            // 
            this.groupBox_stitch.Controls.Add(this.label_selected_stitch);
            this.groupBox_stitch.Controls.Add(this.button_select_stitch);
            this.groupBox_stitch.Controls.Add(this.button_hint_stitch);
            this.groupBox_stitch.Controls.Add(this.radioButton_batch_stitch);
            this.groupBox_stitch.Controls.Add(this.radioButton_stitch);
            this.groupBox_stitch.Controls.Add(this.radioButton_nostitich);
            this.groupBox_stitch.Location = new System.Drawing.Point(26, 242);
            this.groupBox_stitch.Name = "groupBox_stitch";
            this.groupBox_stitch.Size = new System.Drawing.Size(306, 123);
            this.groupBox_stitch.TabIndex = 13;
            this.groupBox_stitch.TabStop = false;
            this.groupBox_stitch.Text = "Сшивка фазы";
            // 
            // label_selected_stitch
            // 
            this.label_selected_stitch.AutoSize = true;
            this.label_selected_stitch.Location = new System.Drawing.Point(6, 95);
            this.label_selected_stitch.Name = "label_selected_stitch";
            this.label_selected_stitch.Size = new System.Drawing.Size(68, 13);
            this.label_selected_stitch.TabIndex = 16;
            this.label_selected_stitch.Text = "Не выбрано";
            this.label_selected_stitch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label_selected_stitch.Visible = false;
            this.label_selected_stitch.Click += new System.EventHandler(this.label2_Click);
            // 
            // button_select_stitch
            // 
            this.button_select_stitch.Location = new System.Drawing.Point(231, 66);
            this.button_select_stitch.Name = "button_select_stitch";
            this.button_select_stitch.Size = new System.Drawing.Size(69, 23);
            this.button_select_stitch.TabIndex = 17;
            this.button_select_stitch.Text = "Выбрать";
            this.button_select_stitch.UseVisualStyleBackColor = true;
            this.button_select_stitch.Visible = false;
            this.button_select_stitch.Click += new System.EventHandler(this.button_select_stitch_Click);
            // 
            // radioButton_batch_stitch
            // 
            this.radioButton_batch_stitch.AutoSize = true;
            this.radioButton_batch_stitch.Location = new System.Drawing.Point(6, 66);
            this.radioButton_batch_stitch.Name = "radioButton_batch_stitch";
            this.radioButton_batch_stitch.Size = new System.Drawing.Size(215, 17);
            this.radioButton_batch_stitch.TabIndex = 2;
            this.radioButton_batch_stitch.Text = "Использовать готовый пакет данных";
            this.radioButton_batch_stitch.UseVisualStyleBackColor = true;
            // 
            // radioButton_stitch
            // 
            this.radioButton_stitch.AutoSize = true;
            this.radioButton_stitch.Checked = true;
            this.radioButton_stitch.Location = new System.Drawing.Point(6, 41);
            this.radioButton_stitch.Name = "radioButton_stitch";
            this.radioButton_stitch.Size = new System.Drawing.Size(167, 17);
            this.radioButton_stitch.TabIndex = 1;
            this.radioButton_stitch.TabStop = true;
            this.radioButton_stitch.Text = "Сшить полученные профили";
            this.radioButton_stitch.UseVisualStyleBackColor = true;
            // 
            // radioButton_nostitich
            // 
            this.radioButton_nostitich.AutoSize = true;
            this.radioButton_nostitich.Location = new System.Drawing.Point(6, 16);
            this.radioButton_nostitich.Name = "radioButton_nostitich";
            this.radioButton_nostitich.Size = new System.Drawing.Size(85, 17);
            this.radioButton_nostitich.TabIndex = 0;
            this.radioButton_nostitich.Text = "Без сшивки";
            this.radioButton_nostitich.UseVisualStyleBackColor = true;
            // 
            // groupBox_lut
            // 
            this.groupBox_lut.Controls.Add(this.button_hint_loot);
            this.groupBox_lut.Controls.Add(this.radioButton_lut);
            this.groupBox_lut.Controls.Add(this.radioButton_no_lut);
            this.groupBox_lut.Location = new System.Drawing.Point(26, 382);
            this.groupBox_lut.Name = "groupBox_lut";
            this.groupBox_lut.Size = new System.Drawing.Size(306, 100);
            this.groupBox_lut.TabIndex = 14;
            this.groupBox_lut.TabStop = false;
            this.groupBox_lut.Text = "LUT";
            // 
            // radioButton_lut
            // 
            this.radioButton_lut.AutoSize = true;
            this.radioButton_lut.Checked = true;
            this.radioButton_lut.Location = new System.Drawing.Point(6, 48);
            this.radioButton_lut.Name = "radioButton_lut";
            this.radioButton_lut.Size = new System.Drawing.Size(221, 17);
            this.radioButton_lut.TabIndex = 1;
            this.radioButton_lut.TabStop = true;
            this.radioButton_lut.Text = "Сгенерировать с запросом диапазона";
            this.radioButton_lut.UseVisualStyleBackColor = true;
            // 
            // radioButton_no_lut
            // 
            this.radioButton_no_lut.AutoSize = true;
            this.radioButton_no_lut.Location = new System.Drawing.Point(6, 25);
            this.radioButton_no_lut.Name = "radioButton_no_lut";
            this.radioButton_no_lut.Size = new System.Drawing.Size(112, 17);
            this.radioButton_no_lut.TabIndex = 0;
            this.radioButton_no_lut.TabStop = true;
            this.radioButton_no_lut.Text = "Не генерировать";
            this.radioButton_no_lut.UseVisualStyleBackColor = true;
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(340, 430);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(418, 52);
            this.button_start.TabIndex = 15;
            this.button_start.Text = "Старт";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button_start_Click);
            // 
            // checkBox_req_params
            // 
            this.checkBox_req_params.AutoSize = true;
            this.checkBox_req_params.Checked = true;
            this.checkBox_req_params.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_req_params.Location = new System.Drawing.Point(26, 488);
            this.checkBox_req_params.Name = "checkBox_req_params";
            this.checkBox_req_params.Size = new System.Drawing.Size(213, 17);
            this.checkBox_req_params.TabIndex = 16;
            this.checkBox_req_params.Text = "Показать параметры перед стартом";
            this.checkBox_req_params.UseVisualStyleBackColor = true;
            // 
            // NewMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 582);
            this.Controls.Add(this.checkBox_req_params);
            this.Controls.Add(this.richTextBox_batch);
            this.Controls.Add(this.checkBox_auto_sq);
            this.Controls.Add(this.button_infoAutoSquare);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.groupBox_lut);
            this.Controls.Add(this.groupBox_stitch);
            this.Controls.Add(this.richTextBox_sample);
            this.Controls.Add(this.groupBox_phase);
            this.Controls.Add(this.button_newSample);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_sample);
            this.Name = "NewMainForm";
            this.Text = "NewMainForm";
            this.groupBox_phase.ResumeLayout(false);
            this.groupBox_phase.PerformLayout();
            this.groupBox_stitch.ResumeLayout(false);
            this.groupBox_stitch.PerformLayout();
            this.groupBox_lut.ResumeLayout(false);
            this.groupBox_lut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_sample;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_newSample;
        private System.Windows.Forms.CheckBox checkBox_auto_sq;
        private System.Windows.Forms.Button button_infoAutoSquare;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox_phase;
        private System.Windows.Forms.RadioButton radioButton_batch_phase;
        private System.Windows.Forms.RadioButton radioButton_calc;
        private System.Windows.Forms.Button button_hintphase;
        private System.Windows.Forms.RichTextBox richTextBox_sample;
        private System.Windows.Forms.RichTextBox richTextBox_batch;
        private System.Windows.Forms.GroupBox groupBox_stitch;
        private System.Windows.Forms.RadioButton radioButton_batch_stitch;
        private System.Windows.Forms.RadioButton radioButton_stitch;
        private System.Windows.Forms.RadioButton radioButton_nostitich;
        private System.Windows.Forms.Button button_hint_stitch;
        private System.Windows.Forms.GroupBox groupBox_lut;
        private System.Windows.Forms.RadioButton radioButton_lut;
        private System.Windows.Forms.RadioButton radioButton_no_lut;
        private System.Windows.Forms.Button button_hint_loot;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Button button_select_phase;
        private System.Windows.Forms.Button button_select_stitch;
        private System.Windows.Forms.Label label_selected_phase;
        private System.Windows.Forms.Label label_selected_stitch;
        private System.Windows.Forms.CheckBox checkBox_req_params;
    }
}