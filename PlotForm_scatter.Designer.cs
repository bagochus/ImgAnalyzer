namespace ImgAnalyzer
{
    partial class PlotForm_scatter
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_save = new System.Windows.Forms.Button();
            this.button_csvsel = new System.Windows.Forms.Button();
            this.button_send = new System.Windows.Forms.Button();
            this.button_savetxt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 426);
            this.panel1.TabIndex = 0;
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(687, 12);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(101, 40);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "Сохранить\r\nизображение";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_csvsel
            // 
            this.button_csvsel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_csvsel.Location = new System.Drawing.Point(687, 58);
            this.button_csvsel.Name = "button_csvsel";
            this.button_csvsel.Size = new System.Drawing.Size(101, 40);
            this.button_csvsel.TabIndex = 1;
            this.button_csvsel.Text = "Сохранить CSV";
            this.button_csvsel.UseVisualStyleBackColor = true;
            this.button_csvsel.Click += new System.EventHandler(this.button_csvsel_Click);
            // 
            // button_send
            // 
            this.button_send.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_send.Location = new System.Drawing.Point(687, 150);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(101, 40);
            this.button_send.TabIndex = 3;
            this.button_send.Text = "Отправить  в другое окно";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // button_savetxt
            // 
            this.button_savetxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_savetxt.Location = new System.Drawing.Point(687, 104);
            this.button_savetxt.Name = "button_savetxt";
            this.button_savetxt.Size = new System.Drawing.Size(101, 40);
            this.button_savetxt.TabIndex = 4;
            this.button_savetxt.Text = "Сохранить TXT";
            this.button_savetxt.UseVisualStyleBackColor = true;
            this.button_savetxt.Click += new System.EventHandler(this.button_savetxt_Click);
            // 
            // PlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_savetxt);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.button_csvsel);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.panel1);
            this.Name = "PlotForm";
            this.Text = "PlotForm";
            this.Resize += new System.EventHandler(this.PlotForm_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_csvsel;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.Button button_savetxt;
    }
}