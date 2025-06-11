namespace ImgAnalyzer.DialogForms
{
    partial class AddMeasurment_2D
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
            this.button_calculate = new System.Windows.Forms.Button();
            this.checkBox_A = new System.Windows.Forms.CheckBox();
            this.checkBox_B = new System.Windows.Forms.CheckBox();
            this.checkBox_C = new System.Windows.Forms.CheckBox();
            this.checkBox_ct = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // listBox_names
            // 
            this.listBox_names.FormattingEnabled = true;
            this.listBox_names.Location = new System.Drawing.Point(12, 12);
            this.listBox_names.Name = "listBox_names";
            this.listBox_names.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox_names.Size = new System.Drawing.Size(158, 186);
            this.listBox_names.TabIndex = 0;
            // 
            // button_calculate
            // 
            this.button_calculate.Location = new System.Drawing.Point(191, 12);
            this.button_calculate.Name = "button_calculate";
            this.button_calculate.Size = new System.Drawing.Size(143, 44);
            this.button_calculate.TabIndex = 1;
            this.button_calculate.Text = "Рассчитать";
            this.button_calculate.UseVisualStyleBackColor = true;
            this.button_calculate.Click += new System.EventHandler(this.button_calculate_Click);
            // 
            // checkBox_A
            // 
            this.checkBox_A.AutoSize = true;
            this.checkBox_A.Location = new System.Drawing.Point(191, 75);
            this.checkBox_A.Name = "checkBox_A";
            this.checkBox_A.Size = new System.Drawing.Size(65, 17);
            this.checkBox_A.TabIndex = 2;
            this.checkBox_A.Text = "Group A";
            this.checkBox_A.UseVisualStyleBackColor = true;
            // 
            // checkBox_B
            // 
            this.checkBox_B.AutoSize = true;
            this.checkBox_B.Location = new System.Drawing.Point(191, 106);
            this.checkBox_B.Name = "checkBox_B";
            this.checkBox_B.Size = new System.Drawing.Size(65, 17);
            this.checkBox_B.TabIndex = 3;
            this.checkBox_B.Text = "Group B";
            this.checkBox_B.UseVisualStyleBackColor = true;
            // 
            // checkBox_C
            // 
            this.checkBox_C.AutoSize = true;
            this.checkBox_C.Location = new System.Drawing.Point(191, 137);
            this.checkBox_C.Name = "checkBox_C";
            this.checkBox_C.Size = new System.Drawing.Size(65, 17);
            this.checkBox_C.TabIndex = 4;
            this.checkBox_C.Text = "Group C";
            this.checkBox_C.UseVisualStyleBackColor = true;
            // 
            // checkBox_ct
            // 
            this.checkBox_ct.AutoSize = true;
            this.checkBox_ct.Location = new System.Drawing.Point(191, 168);
            this.checkBox_ct.Name = "checkBox_ct";
            this.checkBox_ct.Size = new System.Drawing.Size(155, 30);
            this.checkBox_ct.TabIndex = 5;
            this.checkBox_ct.Text = "Преобразовать с учетом \r\nактивной области";
            this.checkBox_ct.UseVisualStyleBackColor = true;
            // 
            // AddMeasurment_2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 250);
            this.Controls.Add(this.checkBox_ct);
            this.Controls.Add(this.checkBox_C);
            this.Controls.Add(this.checkBox_B);
            this.Controls.Add(this.checkBox_A);
            this.Controls.Add(this.button_calculate);
            this.Controls.Add(this.listBox_names);
            this.Name = "AddMeasurment_2D";
            this.Text = "AddMeasurment_2D";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_names;
        private System.Windows.Forms.Button button_calculate;
        private System.Windows.Forms.CheckBox checkBox_A;
        private System.Windows.Forms.CheckBox checkBox_B;
        private System.Windows.Forms.CheckBox checkBox_C;
        private System.Windows.Forms.CheckBox checkBox_ct;
    }
}