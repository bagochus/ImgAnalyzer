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
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 218);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(178, 30);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // ImgViewSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.listBox_schemes);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImgViewSettingsForm";
            this.Text = "ImgViewSettingsForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_bw;
        private System.Windows.Forms.RadioButton radioButton_scheme;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox listBox_schemes;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}