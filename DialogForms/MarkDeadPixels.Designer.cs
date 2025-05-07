namespace ImgAnalyzer.DialogForms
{
    partial class MarkDeadPixelsForm
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
            this.radioButton_amp = new System.Windows.Forms.RadioButton();
            this.radioButton_psph = new System.Windows.Forms.RadioButton();
            this.textBox_thr = new System.Windows.Forms.TextBox();
            this.button_calc = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // radioButton_amp
            // 
            this.radioButton_amp.AutoSize = true;
            this.radioButton_amp.Location = new System.Drawing.Point(12, 12);
            this.radioButton_amp.Name = "radioButton_amp";
            this.radioButton_amp.Size = new System.Drawing.Size(137, 17);
            this.radioButton_amp.TabIndex = 0;
            this.radioButton_amp.TabStop = true;
            this.radioButton_amp.Text = "На основе амплитуды";
            this.radioButton_amp.UseVisualStyleBackColor = true;
            // 
            // radioButton_psph
            // 
            this.radioButton_psph.AutoSize = true;
            this.radioButton_psph.Location = new System.Drawing.Point(12, 35);
            this.radioButton_psph.Name = "radioButton_psph";
            this.radioButton_psph.Size = new System.Drawing.Size(148, 17);
            this.radioButton_psph.TabIndex = 1;
            this.radioButton_psph.TabStop = true;
            this.radioButton_psph.Text = "На основе псевдо-фазы";
            this.radioButton_psph.UseVisualStyleBackColor = true;
            // 
            // textBox_thr
            // 
            this.textBox_thr.Location = new System.Drawing.Point(12, 78);
            this.textBox_thr.Name = "textBox_thr";
            this.textBox_thr.Size = new System.Drawing.Size(100, 20);
            this.textBox_thr.TabIndex = 2;
            // 
            // button_calc
            // 
            this.button_calc.Location = new System.Drawing.Point(186, 10);
            this.button_calc.Name = "button_calc";
            this.button_calc.Size = new System.Drawing.Size(184, 88);
            this.button_calc.TabIndex = 3;
            this.button_calc.Text = "Рассчитать";
            this.button_calc.UseVisualStyleBackColor = true;
            this.button_calc.Click += new System.EventHandler(this.button_calc_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Пороговое значение";
            // 
            // MarkDeadPixelsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 161);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_calc);
            this.Controls.Add(this.textBox_thr);
            this.Controls.Add(this.radioButton_psph);
            this.Controls.Add(this.radioButton_amp);
            this.Name = "MarkDeadPixelsForm";
            this.Text = "MarkDeadPixelsForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_amp;
        private System.Windows.Forms.RadioButton radioButton_psph;
        private System.Windows.Forms.TextBox textBox_thr;
        private System.Windows.Forms.Button button_calc;
        private System.Windows.Forms.Label label1;
    }
}