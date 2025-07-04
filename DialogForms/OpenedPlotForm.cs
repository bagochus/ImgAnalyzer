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
    public partial class OpenedPlotForm : Form
    {
        public PlotForm selectedForm = null;
        private List<PlotForm> foundForms;

        public OpenedPlotForm()
        {
            InitializeComponent();
            foundForms = Application.OpenForms.OfType<PlotForm>().ToList();

            foreach (var secondaryForm in foundForms)
            {
                listBox_forms.Items.Add(secondaryForm.Text);
            }



        }

        private void button_select_Click(object sender, EventArgs e)
        {
            if (listBox_forms.SelectedIndex < 0) return;
            selectedForm = foundForms[listBox_forms.SelectedIndex];
            this.Close();
        }
    }
}
