using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer;
using ScottPlot;

namespace ImgAnalyzer.DialogForms
{
    public delegate void MarkDeadPixelDelegate(int threshold);
    public partial class MarkDeadPixelsForm : Form
    {
        MarkDeadPixelDelegate amp;
        MarkDeadPixelDelegate phase;

        public MarkDeadPixelsForm(MainPresenter imageProcessor)
        {
            amp = imageProcessor.MarkDeadPixelByAmplitude;
            phase = imageProcessor.MarkDeadPixelByPseudophase;
            InitializeComponent();
        }

        private void button_calc_Click(object sender, EventArgs e)
        {
            int value = 0;
            if (Int32.TryParse(textBox_thr.Text, out value)) 
            {
                if (radioButton_amp.Checked) amp(value);
                else phase(value);
                this.Close();
            }
        }
    }
}
