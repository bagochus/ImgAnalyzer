using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D
{
    public partial class Form_2D : Form
    {
        public Form_2D()
        {
            InitializeComponent();
            BuildTable();



        }


        private void BuildTable()
        {
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.MultiSelect = true; // если нужно разрешить множественное выделение
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.ReadOnly = true;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.DataSource = DataManager_2D.containers;


            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.Name = "Name";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "Name";
            dataGrid.Columns.Add(column);




            column = new DataGridViewTextBoxColumn();
            column.Name = "Image group";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "ImageGroup";
            dataGrid.Columns.Add(column);




        }



        //-----------------Controls events------------------------
        private void button_measure_Click(object sender, EventArgs e)
        {
            DataManager_2D.AddMeasurment();
        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            List<int> selectedIndices = new List<int>();
            foreach (DataGridViewRow row in dataGrid.SelectedRows)
            {
                selectedIndices.Add(row.Index);
            }
            DataManager_2D.PlotMap(selectedIndices.ToArray());
        }

        private void button_math_Click(object sender, EventArgs e)
        {
            DataManager_2D.ShowCalcForm();
        }

        private void button_rename_Click(object sender, EventArgs e)
        {
            
        }
    }
}
