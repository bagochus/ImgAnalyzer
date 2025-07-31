using ImgAnalyzer.DialogForms;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            column.Name = "operationName";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "operationName";
            dataGrid.Columns.Add(column);




            column = new DataGridViewTextBoxColumn();
            column.Name = "Image group";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "ImageGroup";
            dataGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "width";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "Width";
            dataGrid.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "Height";
            column.DefaultCellStyle.Format = "0.###";
            column.DataPropertyName = "Height";
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
            if (dataGrid.SelectedRows.Count > 0)
            {
                int index = dataGrid.SelectedRows[0].Index;
                string userInput = Interaction.InputBox("Введите новое имя:",
                   "Переименовать котейнер",
                   DataManager_2D.containers[index].Name);
                DataManager_2D.RenameContainer(index,userInput);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataManager_2D.CalculateFullCT();
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            bool error = false;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Настройки диалогового окна
                openFileDialog.Title = "Выберите .bin файлы";
                openFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.Multiselect = true; // Разрешаем выбор нескольких файлов
                openFileDialog.DefaultExt = "bin";
                openFileDialog.AddExtension = true;
                openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "containers");

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Проверяем, что файлы имеют правильное расширение
                    foreach (string fileName in openFileDialog.FileNames)
                    {
                        if (Path.GetExtension(fileName).Equals(".bin", StringComparison.OrdinalIgnoreCase))
                        {
                            try { DataManager_2D.LoadContainer(fileName); }
                            catch { error = true; }
                        }
                    }
                }
            }

            if (error) MessageBox.Show("Не удалось загрузить некоторые контейнеры");
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            for (int i = dataGrid.SelectedRows.Count - 1; i >=0;i--)
            {
                DataManager_2D.DeleteContainer(dataGrid.SelectedRows[i].Index);
            }
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            if (dataGrid.SelectedRows.Count == 0) return;

            int selectedIndex = dataGrid.SelectedRows[0].Index;
            string defaultFileName = DataManager_2D.containers[selectedIndex].Name;
            string filePath = string.Empty;

            // Создаем диалоговое окно сохранения файла
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Настраиваем параметры диалога
                saveFileDialog.Title = "Сохранить файл .bin";
                saveFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                saveFileDialog.DefaultExt = "bin";
                saveFileDialog.AddExtension = true;
                saveFileDialog.FileName = defaultFileName;


                // Показываем диалог и проверяем, нажал ли пользователь OK
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                    DataManager_2D.SaveContainer(selectedIndex, filePath);

                }
            }
        }

        private void button_group_new_Click(object sender, EventArgs e)
        {
            Form_GroupOperations form = new Form_GroupOperations();
            form.ShowDialog();
        }
    }
}
