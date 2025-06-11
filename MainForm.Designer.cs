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
            this.SuspendLayout();
            // 
            // button_selctfiles
            // 
            this.button_selctfiles.Location = new System.Drawing.Point(12, 12);
            this.button_selctfiles.Name = "button_selctfiles";
            this.button_selctfiles.Size = new System.Drawing.Size(145, 45);
            this.button_selctfiles.TabIndex = 0;
            this.button_selctfiles.Text = "Выбрать папку с файлами";
            this.button_selctfiles.UseVisualStyleBackColor = true;
            this.button_selctfiles.Click += new System.EventHandler(this.button_selctfiles_Click);
            // 
            // button_imgview
            // 
            this.button_imgview.Location = new System.Drawing.Point(12, 62);
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
            this.button_2d.Location = new System.Drawing.Point(354, 12);
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
            this.button_1d.Location = new System.Drawing.Point(178, 12);
            this.button_1d.Name = "button_1d";
            this.button_1d.Size = new System.Drawing.Size(157, 95);
            this.button_1d.TabIndex = 3;
            this.button_1d.Text = "1D Данные";
            this.button_1d.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.button_1d.UseVisualStyleBackColor = true;
            this.button_1d.Click += new System.EventHandler(this.button_1d_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_1d);
            this.Controls.Add(this.button_2d);
            this.Controls.Add(this.button_imgview);
            this.Controls.Add(this.button_selctfiles);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_selctfiles;
        private System.Windows.Forms.Button button_imgview;
        private System.Windows.Forms.Button button_2d;
        private System.Windows.Forms.Button button_1d;
    }
}