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
    public partial class EditRangeForm : Form
    {
        int nmax;

        public EditRangeForm(ImageProcessor imageProcessor)
        {
            InitializeComponent();
            if (imageProcessor != null) return;
            if (imageProcessor.filenames.Length == 0) return;
            nmax = imageProcessor.filenames.Length;
            textBox_from.Text = "0";
            textBox_to.Text = nmax.ToString();


        }
    }
}
