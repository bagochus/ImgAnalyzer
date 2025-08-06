namespace ImgAnalyzer.ImageView
{
    partial class ImgViewSettingsForm
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
            this.radioButton_bw = new System.Windows.Forms.RadioButton();
            this.radioButton_scheme = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_schemes = new System.Windows.Forms.ListBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_max = new System.Windows.Forms.TextBox();
            this.textBox_min = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton_bw
            // 
            this.radioButton_bw.AutoSize = true;
            this.radioButton_bw.Location = new System.Drawing.Point(6, 19);
            this.radioButton_bw.Name = "radioButton_bw";
            this.radioButton_bw.Size = new System.Drawing.Size(90, 17);
            this.radioButton_bw.TabIndex = 1;
            this.radioButton_bw.TabStop = true;
            this.radioButton_bw.Text = "Черно-белая";
            this.radioButton_bw.UseVisualStyleBackColor = true;
            // 
            // radioButton_scheme
            // 
            this.radioButton_scheme.AutoSize = true;
            this.radioButton_scheme.Location = new System.Drawing.Point(6, 42);
            this.radioButton_scheme.Name = "radioButton_scheme";
            this.radioButton_scheme.Size = new System.Drawing.Size(106, 17);
            this.radioButton_scheme.TabIndex = 2;
            this.radioButton_scheme.TabStop = true;
            this.radioButton_scheme.Text = "Тепловая карта";
            this.radioButton_scheme.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_scheme);
            this.groupBox1.Controls.Add(this.radioButton_bw);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 73);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Цветовая схема";
            // 
            // listBox_schemes
            // 
            this.listBox_schemes.FormattingEnabled = true;
            this.listBox_schemes.Location = new System.Drawing.Point(12, 91);
            this.listBox_schemes.Name = "listBox_schemes";
            this.listBox_schemes.Size = new System.Drawing.Size(178, 121);
            this.listBox_schemes.TabIndex = 2;
            this.listBox_schemes.SelectedIndexChanged += new System.EventHandler(this.listBox_schemes_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 218);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(178, 30);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(207, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(495, 288);
            this.panel1.TabIndex = 4;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(708, 12);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(93, 23);
            this.button_ok.TabIndex = 5;
            this.button_ok.Text = "ОК";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(708, 41);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(93, 23);
            this.button_cancel.TabIndex = 6;
            this.button_cancel.Text = "Отмена";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_max
            // 
            this.textBox_max.Location = new System.Drawing.Point(90, 280);
            this.textBox_max.Name = "textBox_max";
            this.textBox_max.Size = new System.Drawing.Size(100, 20);
            this.textBox_max.TabIndex = 7;
            // 
            // textBox_min
            // 
            this.textBox_min.Location = new System.Drawing.Point(90, 254);
            this.textBox_min.Name = "textBox_min";
            this.textBox_min.Size = new System.Drawing.Size(100, 20);
            this.textBox_min.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "MIN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 283);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "MAX";
            // 
            // ImgViewSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 451);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_min);
            this.Controls.Add(this.textBox_max);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listBox_schemes);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImgViewSettingsForm";
            this.Text = "ImgViewSettingsForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_bw;
        private System.Windows.Forms.RadioButton radioButton_scheme;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_schemes;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_max;
        private System.Windows.Forms.TextBox textBox_min;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}