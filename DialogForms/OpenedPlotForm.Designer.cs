namespace ImgAnalyzer.DialogForms
{
    partial class OpenedPlotForm
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
            this.listBox_forms = new System.Windows.Forms.ListBox();
            this.button_select = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_forms
            // 
            this.listBox_forms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_forms.FormattingEnabled = true;
            this.listBox_forms.Location = new System.Drawing.Point(12, 12);
            this.listBox_forms.Name = "listBox_forms";
            this.listBox_forms.Size = new System.Drawing.Size(338, 186);
            this.listBox_forms.TabIndex = 0;
            // 
            // button_select
            // 
            this.button_select.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_select.Location = new System.Drawing.Point(12, 214);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(338, 23);
            this.button_select.TabIndex = 1;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // OpenedPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 248);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.listBox_forms);
            this.Name = "OpenedPlotForm";
            this.Text = "Plot Forms";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_forms;
        private System.Windows.Forms.Button button_select;
    }
}