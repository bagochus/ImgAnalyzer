using ScottPlot.Colormaps;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.ImageView
{
    internal partial class ImgViewSettingsForm : Form
    {
        ImgViewSettings settings;
        public bool changed = false;
        public ImgViewSettingsForm(ImgViewSettings settings)
        {
   


            InitializeComponent();
            foreach (var s in ColorMaps.Schemes) listBox_schemes.Items.Add(s.Name);


            this.settings = settings;
            if (settings.colorMode == ColorMode.Simple
                || settings.colorMode == ColorMode.BW)
                radioButton_bw.Checked = true;
            else radioButton_scheme.Checked = true;
       
            listBox_schemes.SelectedIndex = settings.schemeIndex;
            textBox_min.Text = settings.min.ToString();
            textBox_max.Text = settings.max.ToString();
            


        }


        private void GenerateSamplePicture(ColorScheme colorScheme)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            for (int x = 0; x < bmp.Width; x++)
            {
                Color color = colorScheme.CalculateColorDouble(0, bmp.Width, x);
                for (int y = 0; y < bmp.Height; y++)
                    bmp.SetPixel(x, y, color);
            }
            pictureBox1.Image = bmp;
             
        }

        private void listBox_schemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateSamplePicture(ColorMaps.Schemes[listBox_schemes.SelectedIndex]);
            

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (radioButton_bw.Checked) settings.colorMode = ColorMode.BW;
            else settings.colorMode = ColorMode.Color;
            settings.schemeIndex = listBox_schemes.SelectedIndex;
            try
            {
                settings.min = Double.Parse(textBox_min.Text);
                settings.max = Double.Parse(textBox_max.Text);
            }
            catch { }
            changed = true;
            Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
