namespace ImgAnalyzer.DialogForms
{
    partial class Form_GroupOperations
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
            this.listBox_operations = new System.Windows.Forms.ListBox();
            this.button_exec = new System.Windows.Forms.Button();
            this.label_description = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBox_operations
            // 
            this.listBox_operations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBox_operations.FormattingEnabled = true;
            this.listBox_operations.Location = new System.Drawing.Point(12, 12);
            this.listBox_operations.Name = "listBox_operations";
            this.listBox_operations.Size = new System.Drawing.Size(198, 407);
            this.listBox_operations.TabIndex = 0;
            this.listBox_operations.SelectedIndexChanged += new System.EventHandler(this.listBox_operations_SelectedIndexChanged);
            // 
            // button_exec
            // 
            this.button_exec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_exec.Location = new System.Drawing.Point(705, 12);
            this.button_exec.Name = "button_exec";
            this.button_exec.Size = new System.Drawing.Size(75, 23);
            this.button_exec.TabIndex = 2;
            this.button_exec.Text = "Выполнить";
            this.button_exec.UseVisualStyleBackColor = true;
            this.button_exec.Click += new System.EventHandler(this.button_exec_Click);
            // 
            // label_description
            // 
            this.label_description.AutoSize = true;
            this.label_description.Location = new System.Drawing.Point(231, 22);
            this.label_description.Name = "label_description";
            this.label_description.Size = new System.Drawing.Size(19, 13);
            this.label_description.TabIndex = 3;
            this.label_description.Text = "----";
            // 
            // Form_GroupOperations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 444);
            this.Controls.Add(this.label_description);
            this.Controls.Add(this.button_exec);
            this.Controls.Add(this.listBox_operations);
            this.Name = "Form_GroupOperations";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_operations;
        private System.Windows.Forms.Button button_exec;
        private System.Windows.Forms.Label label_description;
    }
}