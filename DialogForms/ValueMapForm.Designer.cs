namespace ImgAnalyzer.DialogForms
{
    partial class ValueMapForm
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
            this.listBox_names = new System.Windows.Forms.ListBox();
            this.button_plot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_names
            // 
            this.listBox_names.FormattingEnabled = true;
            this.listBox_names.Location = new System.Drawing.Point(31, 29);
            this.listBox_names.Name = "listBox_names";
            this.listBox_names.Size = new System.Drawing.Size(161, 95);
            this.listBox_names.TabIndex = 0;
            // 
            // button_plot
            // 
            this.button_plot.Location = new System.Drawing.Point(198, 29);
            this.button_plot.Name = "button_plot";
            this.button_plot.Size = new System.Drawing.Size(86, 95);
            this.button_plot.TabIndex = 1;
            this.button_plot.Text = "Построить";
            this.button_plot.UseVisualStyleBackColor = true;
            this.button_plot.Click += new System.EventHandler(this.button_plot_Click);
            // 
            // ValueMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 158);
            this.Controls.Add(this.button_plot);
            this.Controls.Add(this.listBox_names);
            this.Name = "ValueMapForm";
            this.Text = "ValueMapForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_names;
        private System.Windows.Forms.Button button_plot;
    }
}