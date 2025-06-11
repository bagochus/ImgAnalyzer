using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ImgAnalyzer.MeasurmentTypes;
using Microsoft.VisualBasic;

namespace ImgAnalyzer
{
    public partial class Form_1D : Form
    {
        DataManager_1D _manager;

        public Form_1D(DataManager_1D manager)
        {
            InitializeComponent();
            ConstructTable();
            _manager = manager;
        }

        public void ConstructTable()
        {
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.MultiSelect = true; // если нужно разрешить множественное выделение
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.ReadOnly = true;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.DataSource = DataManager_1D.Instance.dataContainers;


            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Name";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "Name";
            dataGrid.Columns.Add(column);

            

            column = new DataGridViewTextBoxColumn();
            column.Name = "Type";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "Type";
            dataGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Image group";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "ImageGroup";
            dataGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Status";
            column.DataPropertyName = "Status";
            column.DefaultCellStyle.Format = "0.###";
            dataGrid.Columns.Add(column);

        }



        //-----------------Controls events----------------------------

        private void button_calculate_Click(object sender, EventArgs e)
        {
            if (DataManager_1D.Instance.InWork)
            {
                MessageBox.Show("В настоящий момент процесс занят");
                return;
            }
            List<int> selectedIndices = new List<int>();
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                selectedIndices.Add(row.Index);
            }
            DataManager_1D.Instance.ProcessAllImages(selectedIndices.ToArray());

        }

        private void button_calcall_Click(object sender, EventArgs e)
        {
            if (DataManager_1D.Instance.InWork)
            {
                MessageBox.Show("В настоящий момент процесс занят");
                return;
            }
            DataManager_1D.Instance.ProcessAllImages();

        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            List<int> selectedIndices = new List<int>();
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                selectedIndices.Add(row.Index);
            }
            DataManager_1D.Instance.PlotSelectedItems(selectedIndices.ToArray());
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            List<int> selectedIndices = new List<int>();
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                selectedIndices.Add(row.Index);
            }
            selectedIndices.Reverse();
            foreach (int  index in selectedIndices)
            {
                DataManager_1D.Instance.DeleteItem(index);
            }
        }

        private void button_rename_Click(object sender, EventArgs e)
        {
            List<int> selectedIndices = new List<int>();
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                selectedIndices.Add(row.Index);
            }
            if (selectedIndices.Count == 0) return;
            int index = selectedIndices[0];


            string userInput = Interaction.InputBox("Введите новое имя:",
            "Переименовать поле",
            DataManager_1D.Instance.GetNames()[index]);
            if (!string.IsNullOrEmpty(userInput))
            {
                DataManager_1D.Instance.RenameItem(index, userInput);
            }

        }

        private void button_deleteall_Click(object sender, EventArgs e)
        {
            DataManager_1D.Instance.ClearMeasurment();
        }


    }
}
