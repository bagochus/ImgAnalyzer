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

        public Progress<int> progress;




        public Form_2D()
        {
            InitializeComponent();
            BuildTable();
            progress = new Progress<int>();
            DataManager_2D.progress = progress;

            progress.ProgressChanged += (s, percent) =>
            {
                DataManager_2D.workDone += percent;
                if (DataManager_2D.workDone >= DataManager_2D.workToBeDone)
                {
                    DataManager_2D.workToBeDone = 0;
                    DataManager_2D.workDone = 0;
                    progressBar1.Maximum = 1;
                    progressBar1.Value = 0;
                }
                else
                {
                    progressBar1.Maximum = DataManager_2D.workToBeDone;
                    progressBar1.Value = DataManager_2D.workDone;
                }
            };

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
            dataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;


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
            try
            {
                List<int> selectedIndices = new List<int>();
                foreach (DataGridViewRow row in dataGrid.SelectedRows)
                {
                    selectedIndices.Add(row.Index);
                }
                DataManager_2D.PlotMap(selectedIndices.ToArray());
            }
            catch (Exception ex ){ MessageBox.Show(ex.Message); }

        }

        private void button_math_Click(object sender, EventArgs e)
        {
            DataManager_2D.ShowCalcForm();
        }

        private void button_rename_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataManager_2D.CalculateFullCT();
        }
    }
}
