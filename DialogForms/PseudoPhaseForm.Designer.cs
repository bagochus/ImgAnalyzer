namespace ImgAnalyzer.DialogForms
{
    partial class PseudoPhaseForm
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
            this.textBox_upth = new System.Windows.Forms.TextBox();
            this.textBox_lowth = new System.Windows.Forms.TextBox();
            this.textBox_min = new System.Windows.Forms.TextBox();
            this.textBox_max = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button_calc = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_upth
            // 
            this.textBox_upth.Location = new System.Drawing.Point(35, 40);
            this.textBox_upth.Name = "textBox_upth";
            this.textBox_upth.Size = new System.Drawing.Size(100, 20);
            this.textBox_upth.TabIndex = 0;
            // 
            // textBox_lowth
            // 
            this.textBox_lowth.Location = new System.Drawing.Point(35, 85);
            this.textBox_lowth.Name = "textBox_lowth";
            this.textBox_lowth.Size = new System.Drawing.Size(100, 20);
            this.textBox_lowth.TabIndex = 1;
            // 
            // textBox_min
            // 
            this.textBox_min.Location = new System.Drawing.Point(35, 127);
            this.textBox_min.Name = "textBox_min";
            this.textBox_min.Size = new System.Drawing.Size(100, 20);
            this.textBox_min.TabIndex = 2;
            // 
            // textBox_max
            // 
            this.textBox_max.Location = new System.Drawing.Point(35, 169);
            this.textBox_max.Name = "textBox_max";
            this.textBox_max.Size = new System.Drawing.Size(100, 20);
            this.textBox_max.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Верхний порог";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Нижний порог";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Минимум";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 153);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Максимум";
            // 
            // button_calc
            // 
            this.button_calc.Location = new System.Drawing.Point(175, 37);
            this.button_calc.Name = "button_calc";
            this.button_calc.Size = new System.Drawing.Size(97, 152);
            this.button_calc.TabIndex = 8;
            this.button_calc.Text = "Рассчитать";
            this.button_calc.UseVisualStyleBackColor = true;
            this.button_calc.Click += new System.EventHandler(this.button_calc_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 232);
            this.Controls.Add(this.button_calc);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_max);
            this.Controls.Add(this.textBox_min);
            this.Controls.Add(this.textBox_lowth);
            this.Controls.Add(this.textBox_upth);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_upth;
        private System.Windows.Forms.TextBox textBox_lowth;
        private System.Windows.Forms.TextBox textBox_min;
        private System.Windows.Forms.TextBox textBox_max;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_calc;
    }
}