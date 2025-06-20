namespace ImgAnalyzer.DialogForms
{
    partial class FormCalculations2D
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
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.button_select = new System.Windows.Forms.Button();
            this.label_decription = new System.Windows.Forms.Label();
            this.button_execute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(25, 12);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(121, 21);
            this.comboBox_type.TabIndex = 0;
            // 
            // button_select
            // 
            this.button_select.Location = new System.Drawing.Point(164, 12);
            this.button_select.Name = "button_select";
            this.button_select.Size = new System.Drawing.Size(75, 23);
            this.button_select.TabIndex = 1;
            this.button_select.Text = "Выбрать";
            this.button_select.UseVisualStyleBackColor = true;
            this.button_select.Click += new System.EventHandler(this.button_select_Click);
            // 
            // label_decription
            // 
            this.label_decription.AutoSize = true;
            this.label_decription.Location = new System.Drawing.Point(263, 17);
            this.label_decription.Name = "label_decription";
            this.label_decription.Size = new System.Drawing.Size(60, 13);
            this.label_decription.TabIndex = 4;
            this.label_decription.Text = "Description";
            // 
            // button_execute
            // 
            this.button_execute.Location = new System.Drawing.Point(704, 12);
            this.button_execute.Name = "button_execute";
            this.button_execute.Size = new System.Drawing.Size(75, 23);
            this.button_execute.TabIndex = 5;
            this.button_execute.Text = "Выполнить";
            this.button_execute.UseVisualStyleBackColor = true;
            this.button_execute.Click += new System.EventHandler(this.button_execute_Click);
            // 
            // FormCalculations2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button_execute);
            this.Controls.Add(this.label_decription);
            this.Controls.Add(this.button_select);
            this.Controls.Add(this.comboBox_type);
            this.Name = "FormCalculations2D";
            this.Text = "Произвести вычисления";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Button button_select;
        private System.Windows.Forms.Label label_decription;
        private System.Windows.Forms.Button button_execute;
    }
}