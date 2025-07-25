namespace ImgAnalyzer.DialogForms
{
    partial class Form_PhaseMeasurment
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
            this.comboBox_amin = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_amax = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_bmin = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_bmax = new System.Windows.Forms.ComboBox();
            this.checkBox_rad = new System.Windows.Forms.CheckBox();
            this.checkBox_add = new System.Windows.Forms.CheckBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox_amin
            // 
            this.comboBox_amin.FormattingEnabled = true;
            this.comboBox_amin.Location = new System.Drawing.Point(24, 32);
            this.comboBox_amin.Name = "comboBox_amin";
            this.comboBox_amin.Size = new System.Drawing.Size(121, 21);
            this.comboBox_amin.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "A_min";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "A_max";
            // 
            // comboBox_amax
            // 
            this.comboBox_amax.FormattingEnabled = true;
            this.comboBox_amax.Location = new System.Drawing.Point(24, 75);
            this.comboBox_amax.Name = "comboBox_amax";
            this.comboBox_amax.Size = new System.Drawing.Size(121, 21);
            this.comboBox_amax.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "B_min";
            // 
            // comboBox_bmin
            // 
            this.comboBox_bmin.FormattingEnabled = true;
            this.comboBox_bmin.Location = new System.Drawing.Point(165, 32);
            this.comboBox_bmin.Name = "comboBox_bmin";
            this.comboBox_bmin.Size = new System.Drawing.Size(121, 21);
            this.comboBox_bmin.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(162, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "B_max";
            // 
            // comboBox_bmax
            // 
            this.comboBox_bmax.FormattingEnabled = true;
            this.comboBox_bmax.Location = new System.Drawing.Point(165, 75);
            this.comboBox_bmax.Name = "comboBox_bmax";
            this.comboBox_bmax.Size = new System.Drawing.Size(121, 21);
            this.comboBox_bmax.TabIndex = 6;
            // 
            // checkBox_rad
            // 
            this.checkBox_rad.AutoSize = true;
            this.checkBox_rad.Location = new System.Drawing.Point(24, 119);
            this.checkBox_rad.Name = "checkBox_rad";
            this.checkBox_rad.Size = new System.Drawing.Size(137, 17);
            this.checkBox_rad.TabIndex = 8;
            this.checkBox_rad.Text = "Результат в радианах";
            this.checkBox_rad.UseVisualStyleBackColor = true;
            // 
            // checkBox_add
            // 
            this.checkBox_add.AutoSize = true;
            this.checkBox_add.Location = new System.Drawing.Point(24, 153);
            this.checkBox_add.Name = "checkBox_add";
            this.checkBox_add.Size = new System.Drawing.Size(138, 17);
            this.checkBox_add.TabIndex = 9;
            this.checkBox_add.Text = "Добавлять контейнер";
            this.checkBox_add.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(315, 32);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(75, 64);
            this.button_ok.TabIndex = 10;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // Form_PhaseMeasurment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.checkBox_add);
            this.Controls.Add(this.checkBox_rad);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox_bmax);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox_bmin);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox_amax);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox_amin);
            this.Name = "Form_PhaseMeasurment";
            this.Text = "Form_PhaseMeasurment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_amin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_amax;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_bmin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_bmax;
        private System.Windows.Forms.CheckBox checkBox_rad;
        private System.Windows.Forms.CheckBox checkBox_add;
        private System.Windows.Forms.Button button_ok;
    }
}