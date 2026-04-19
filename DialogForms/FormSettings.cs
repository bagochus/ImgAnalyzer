using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer.Settings;

namespace ImgAnalyzer.DialogForms
{
    public partial class FormSettings : Form
    {
        BindingList<SettingRecord> recordList;

        public bool ShowForm { get=>SettingsManager.ShowForm; set { SettingsManager.ShowForm = value; } }
        public bool ShowFormOnFirstRequest { get => SettingsManager.ShowFormOnFirstRequest; set { SettingsManager.ShowFormOnFirstRequest = value; } }

        public FormSettings()
        {
            InitializeComponent();
            checkBox_sf.Checked = ShowForm;
            checkBox_sfofr.Checked = ShowFormOnFirstRequest;



            recordList = SettingsManager.GetAllSettings();

            dataGridView1.DataSource = recordList;
            dataGridView1.AutoResizeColumns();

        }

        private void SearchSettings()
        {
            string searchStr = textBox_search.Text;
            //if (searchStr.Length == 0) return;
            recordList = SettingsManager.GetAllSettings("", searchStr);
            dataGridView1.DataSource = recordList;
            dataGridView1.Invalidate();
        }

        private void EditSetting()
        {
            if (dataGridView1.SelectedRows.Count != 1) return;
            int id = (dataGridView1.SelectedRows[0].DataBoundItem as SettingRecord).Id;
            


        
        }




        private void checkBox_sf_CheckedChanged(object sender, EventArgs e)
        {
            ShowForm = checkBox_sf.Checked;
        }

        private void checkBox_sfofr_CheckedChanged(object sender, EventArgs e)
        {
            ShowFormOnFirstRequest = checkBox_sfofr.Checked;
        }

        private void FormSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            var sd_ShowForm = SettingDefinition.CreateGlobal(SettingsManager._showForm, false, "");
            sd_ShowForm.Value = ShowForm;
            var sd_SFOFR = SettingDefinition.CreateGlobal(SettingsManager._showFormOnFirstRequest, false, "");
            sd_SFOFR.Value = ShowFormOnFirstRequest;
            SettingsManager.SaveSettingsToDatabase(new List<SettingDefinition> { sd_ShowForm, sd_SFOFR });
        }

        private void button_search_Click(object sender, EventArgs e)
        {
            SearchSettings();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditSetting();
        }
    }
}
