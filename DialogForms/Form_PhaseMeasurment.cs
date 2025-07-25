using ImgAnalyzer._2D;
using ScottPlot.Finance;
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
    public partial class Form_PhaseMeasurment : Form
    {
        private List<string> avaiable_containers = new List<string>();

        public Form_PhaseMeasurment()
        {
            InitializeComponent();

            foreach (var c in DataManager_2D.containers) { avaiable_containers.Add(c.Name); }

            comboBox_amax.Items.AddRange(avaiable_containers.ToArray());
            comboBox_amin.Items.AddRange(avaiable_containers.ToArray());
            comboBox_bmax.Items.AddRange(avaiable_containers.ToArray());
            comboBox_bmin.Items.AddRange(avaiable_containers.ToArray());

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            bool select_correct = true;
            select_correct &= (comboBox_amax.SelectedIndex >= 0);
            select_correct &= (comboBox_amin.SelectedIndex >= 0);
            select_correct &= (comboBox_bmax.SelectedIndex >= 0);
            select_correct &= (comboBox_bmin.SelectedIndex >= 0);

            if (!select_correct) 
            {
                MessageBox.Show("Ошибка выбора");
                return;
                  
            }

            PhaseMeasurer.a_min_id = comboBox_amin.SelectedIndex;
            PhaseMeasurer.b_min_id= comboBox_bmin.SelectedIndex;
            PhaseMeasurer.a_max_id = comboBox_amax.SelectedIndex;
            PhaseMeasurer.b_max_id= comboBox_bmax.SelectedIndex;
            PhaseMeasurer.CreateContainer = checkBox_add.Checked; 
            PhaseMeasurer.UseRadian = checkBox_rad.Checked;
            this.Close();




        }
    }
}
