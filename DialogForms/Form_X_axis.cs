using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.DialogForms
{
    public partial class Form_X_axis : Form
    {
        public Form_X_axis()
        {
            InitializeComponent();
            FillForm();
        }


        private void FillForm()
        {
            textBox_label.Text = DataManager_1D.Instance.VariableName;
            textBox_units.Text = DataManager_1D.Instance.VariableUnit;
            textBox_xstart.Text = DataManager_1D.Instance.x_start.ToString();
            textBox_xstep.Text = DataManager_1D.Instance.x_step.ToString();
        }

        private void ReadForm()
        {
            double xstart = 0;
            double xstep = 0;
            try
            {
                xstart = Double.Parse(textBox_xstart.Text);
                xstep = Double.Parse(textBox_xstep.Text);
                DataManager_1D.Instance.VariableName = textBox_label.Text;
                DataManager_1D.Instance.VariableUnit = textBox_units.Text;
                DataManager_1D.Instance.x_start = xstart;
                DataManager_1D.Instance.x_step = xstep;
                Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadForm();
        }
    }
}
