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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ImgAnalyzer.DialogForms
{
    public delegate void PlotValueMapDelegate(VariablesToMap variable);
    public partial class ValueMapForm : Form
    {
        PlotValueMapDelegate plotMap;
        public ValueMapForm(MainPresenter processor)
        {

            InitializeComponent();
            plotMap = processor.PlotValueMap;
            listBox_names.Items.AddRange(Enum.GetNames(typeof(VariablesToMap)));
        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            int index = listBox_names.SelectedIndex;
            if (Enum.IsDefined(typeof(VariablesToMap), index))
            {
                plotMap((VariablesToMap)index);
                this.Close();
            }
            else
            {
                Console.WriteLine("Недопустимое значение enum");
            }
            
        }
    }
}
