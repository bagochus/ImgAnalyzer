namespace ImgAnalyzer
{
    partial class ImageViewForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_addpoint = new System.Windows.Forms.Button();
            this.CoordinateLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button_addpoly = new System.Windows.Forms.Button();
            this.button_addsqare = new System.Windows.Forms.Button();
            this.checkBox_allch = new System.Windows.Forms.CheckBox();
            this.button_frame = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button_addpointct = new System.Windows.Forms.Button();
            this.button_addpolyct = new System.Windows.Forms.Button();
            this.checkBox_allch_act = new System.Windows.Forms.CheckBox();
            this.checkBox_overlays = new System.Windows.Forms.CheckBox();
            this.button_reset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.DarkGreen;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(852, 620);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button_addpoint
            // 
            this.button_addpoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addpoint.Location = new System.Drawing.Point(873, 68);
            this.button_addpoint.Name = "button_addpoint";
            this.button_addpoint.Size = new System.Drawing.Size(140, 23);
            this.button_addpoint.TabIndex = 1;
            this.button_addpoint.Text = "Точка";
            this.button_addpoint.UseVisualStyleBackColor = true;
            this.button_addpoint.Click += new System.EventHandler(this.button_addpoint_Click);
            // 
            // CoordinateLabel
            // 
            this.CoordinateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CoordinateLabel.AutoSize = true;
            this.CoordinateLabel.Location = new System.Drawing.Point(12, 635);
            this.CoordinateLabel.Name = "CoordinateLabel";
            this.CoordinateLabel.Size = new System.Drawing.Size(35, 13);
            this.CoordinateLabel.TabIndex = 2;
            this.CoordinateLabel.Text = "label1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(870, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 39);
            this.label1.TabIndex = 3;
            this.label1.Text = "Добавить измерения без \r\nпривзяки к активной \r\nобласти";
            // 
            // button_addpoly
            // 
            this.button_addpoly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addpoly.Location = new System.Drawing.Point(873, 126);
            this.button_addpoly.Name = "button_addpoly";
            this.button_addpoly.Size = new System.Drawing.Size(140, 23);
            this.button_addpoly.TabIndex = 4;
            this.button_addpoly.Text = "Полигон";
            this.button_addpoly.UseVisualStyleBackColor = true;
            this.button_addpoly.Click += new System.EventHandler(this.button_addpoly_Click);
            // 
            // button_addsqare
            // 
            this.button_addsqare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addsqare.Location = new System.Drawing.Point(873, 97);
            this.button_addsqare.Name = "button_addsqare";
            this.button_addsqare.Size = new System.Drawing.Size(140, 23);
            this.button_addsqare.TabIndex = 5;
            this.button_addsqare.Text = "Прямоугольник";
            this.button_addsqare.UseVisualStyleBackColor = true;
            this.button_addsqare.Click += new System.EventHandler(this.button_addsqare_Click);
            // 
            // checkBox_allch
            // 
            this.checkBox_allch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_allch.AutoSize = true;
            this.checkBox_allch.Location = new System.Drawing.Point(873, 155);
            this.checkBox_allch.Name = "checkBox_allch";
            this.checkBox_allch.Size = new System.Drawing.Size(97, 30);
            this.checkBox_allch.TabIndex = 6;
            this.checkBox_allch.Text = "Добавить для\r\n всех каналов";
            this.checkBox_allch.UseVisualStyleBackColor = true;
            this.checkBox_allch.CheckedChanged += new System.EventHandler(this.checkBox_allch_CheckedChanged);
            // 
            // button_frame
            // 
            this.button_frame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_frame.Location = new System.Drawing.Point(873, 202);
            this.button_frame.Name = "button_frame";
            this.button_frame.Size = new System.Drawing.Size(140, 39);
            this.button_frame.TabIndex = 7;
            this.button_frame.Text = "Определить активную область";
            this.button_frame.UseVisualStyleBackColor = true;
            this.button_frame.Click += new System.EventHandler(this.button_frame_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(870, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 39);
            this.label2.TabIndex = 8;
            this.label2.Text = "Добавить измерения \r\nс привязкой к \r\nактивной области";
            // 
            // button_addpointct
            // 
            this.button_addpointct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addpointct.Location = new System.Drawing.Point(873, 306);
            this.button_addpointct.Name = "button_addpointct";
            this.button_addpointct.Size = new System.Drawing.Size(140, 23);
            this.button_addpointct.TabIndex = 9;
            this.button_addpointct.Text = "Точка";
            this.button_addpointct.UseVisualStyleBackColor = true;
            this.button_addpointct.Click += new System.EventHandler(this.button_pointframe_Click);
            // 
            // button_addpolyct
            // 
            this.button_addpolyct.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_addpolyct.Location = new System.Drawing.Point(873, 335);
            this.button_addpolyct.Name = "button_addpolyct";
            this.button_addpolyct.Size = new System.Drawing.Size(140, 23);
            this.button_addpolyct.TabIndex = 10;
            this.button_addpolyct.Text = "Полигон";
            this.button_addpolyct.UseVisualStyleBackColor = true;
            this.button_addpolyct.Click += new System.EventHandler(this.button_polyframe_Click);
            // 
            // checkBox_allch_act
            // 
            this.checkBox_allch_act.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_allch_act.AutoSize = true;
            this.checkBox_allch_act.Location = new System.Drawing.Point(873, 364);
            this.checkBox_allch_act.Name = "checkBox_allch_act";
            this.checkBox_allch_act.Size = new System.Drawing.Size(97, 30);
            this.checkBox_allch_act.TabIndex = 11;
            this.checkBox_allch_act.Text = "Добавить для\r\n всех каналов";
            this.checkBox_allch_act.UseVisualStyleBackColor = true;
            // 
            // checkBox_overlays
            // 
            this.checkBox_overlays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_overlays.AutoSize = true;
            this.checkBox_overlays.Checked = true;
            this.checkBox_overlays.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_overlays.Location = new System.Drawing.Point(870, 602);
            this.checkBox_overlays.Name = "checkBox_overlays";
            this.checkBox_overlays.Size = new System.Drawing.Size(109, 30);
            this.checkBox_overlays.TabIndex = 12;
            this.checkBox_overlays.Text = "Показать места\r\nизмерений";
            this.checkBox_overlays.UseVisualStyleBackColor = true;
            this.checkBox_overlays.CheckedChanged += new System.EventHandler(this.checkBox_overlays_CheckedChanged);
            // 
            // button_reset
            // 
            this.button_reset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_reset.Location = new System.Drawing.Point(873, 573);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(140, 23);
            this.button_reset.TabIndex = 13;
            this.button_reset.Text = "Сбросить картинку";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button_reset_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(324, 635);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "label3";
            // 
            // ImageViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 657);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.checkBox_overlays);
            this.Controls.Add(this.checkBox_allch_act);
            this.Controls.Add(this.button_addpolyct);
            this.Controls.Add(this.button_addpointct);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_frame);
            this.Controls.Add(this.checkBox_allch);
            this.Controls.Add(this.button_addsqare);
            this.Controls.Add(this.button_addpoly);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CoordinateLabel);
            this.Controls.Add(this.button_addpoint);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(228, 502);
            this.Name = "ImageViewForm";
            this.Text = "ImageViewForm";
            this.Load += new System.EventHandler(this.ImageViewForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_addpoint;
        private System.Windows.Forms.Label CoordinateLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_addpoly;
        private System.Windows.Forms.Button button_addsqare;
        private System.Windows.Forms.CheckBox checkBox_allch;
        private System.Windows.Forms.Button button_frame;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_addpointct;
        private System.Windows.Forms.Button button_addpolyct;
        private System.Windows.Forms.CheckBox checkBox_allch_act;
        private System.Windows.Forms.CheckBox checkBox_overlays;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Label label3;
    }
}