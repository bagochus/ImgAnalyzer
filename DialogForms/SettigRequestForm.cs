using ImgAnalyzer;
using Microsoft.VisualBasic;
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
    public partial class SettigRequestForm : Form
    {
        List<SettingDefinition> settings;
        public bool saveAfterUse { get { return checkBox_save.Checked; } }

        public SettigRequestForm(List<SettingDefinition> settings)
        {
            InitializeComponent();
            this.settings = settings;
            foreach (SettingDefinition setting in settings)
            {
                dataGridView1.Rows.Add(setting.Name, setting.Value,
                    setting.Global? "global":"local", setting.ValueType.ToString(),setting.Comment);
            }
        }

        private void EditParameter(int selected)
        {
            string userInput = Interaction.InputBox($"Введите новое значение для {settings[selected].Name}",
                 "Изменить значение",
                 settings[selected].Value.ToString());
            if (settings[selected].ConvertFromString(userInput))
            {
                dataGridView1.Rows[selected].Cells["Value"].Value = settings[selected].Value.ToString();
            }
            else MessageBox.Show("Не удалось распознать введенное значение");

        }


        private void button1_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.SelectedRows[0].Index;
            if (dataGridView1.SelectedRows[0].Index < settings.Count) EditParameter(index);
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
