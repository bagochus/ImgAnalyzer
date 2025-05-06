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

namespace ImgAnalyzer.DialogForms
{

    public delegate void CalculatePseudoPhaseDelegate(string[] filenames, int upper_threshold, int lower_threshold, int max_peak, int min_peak);
    public partial class PseudoPhaseForm : Form
    {
        CalculatePseudoPhaseDelegate calculate;
        private int upper_threshold;
        private int lower_threshold;
        private int max_peak;
        private int min_peak;
        string[] filenames;
        private bool read_ok = false;
        public PseudoPhaseForm(ImageProcessor processor, string[] filenames)
        {
            if (processor == null) return;
            InitializeComponent();
            calculate = processor.CalculatePseudoPhase;
            this.filenames = filenames; 
        }
        private void ReadInterface()
        {
            try
            {
                upper_threshold = Int32.Parse(textBox_upth.Text);
                lower_threshold = Int32.Parse(textBox_lowth.Text);
                max_peak = Int32.Parse(textBox_max.Text);
                min_peak = Int32.Parse(textBox_min.Text);
                read_ok = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }



        }
        


        private void button_calc_Click(object sender, EventArgs e)
        {
            ReadInterface();
            if (read_ok)
            {
                calculate(filenames, upper_threshold, lower_threshold, max_peak, min_peak);
                this.Close();
            }
        }
    }
}
