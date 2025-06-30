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
    public partial class ProfilesForm : Form
    {
        public int selected;
        public string selected_name;
        public ProfilesForm(string[] names)
        {
            InitializeComponent();
            listBox_profiles.Items.Clear();
            listBox_profiles.Items.AddRange(names);
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            selected = listBox_profiles.SelectedIndex;
            selected_name = listBox_profiles.SelectedItem.ToString();
            this.Close();
        }
    }
}
