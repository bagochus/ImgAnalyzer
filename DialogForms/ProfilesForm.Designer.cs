namespace ImgAnalyzer.DialogForms
{
    partial class ProfilesForm
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
            this.listBox_profiles = new System.Windows.Forms.ListBox();
            this.button_load = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_profiles
            // 
            this.listBox_profiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_profiles.FormattingEnabled = true;
            this.listBox_profiles.Location = new System.Drawing.Point(12, 12);
            this.listBox_profiles.Name = "listBox_profiles";
            this.listBox_profiles.Size = new System.Drawing.Size(356, 160);
            this.listBox_profiles.TabIndex = 0;
            // 
            // button_load
            // 
            this.button_load.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_load.Location = new System.Drawing.Point(12, 186);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(356, 23);
            this.button_load.TabIndex = 1;
            this.button_load.Text = "Загрузить";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // Profiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 224);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.listBox_profiles);
            this.Name = "Profiles";
            this.Text = "Profiles";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_profiles;
        private System.Windows.Forms.Button button_load;
    }
}