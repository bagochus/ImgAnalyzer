using ScottPlot.Colormaps;
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
    public partial class ImgViewSettingsForm : Form
    {
        public ImgViewSettingsForm()
        {
            InitializeComponent();
            foreach (var s in ColorMaps.Schemes) listBox_schemes.Items.Add(s.Name);
        }


        public void GenerateSamplePicture(ColorScheme colorScheme)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                
            }



        }

    }
}
