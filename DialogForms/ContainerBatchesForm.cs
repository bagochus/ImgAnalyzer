using ImgAnalyzer._2D;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.DialogForms
{
    public partial class ContainerBatchesForm : Form
    {
        int localBatchCount = 0;
        int databaseBatchCount = 0;

        ContainerBatch _selectedBatch = null;

        int separator1_index = -1; //    loaded 
        int separator2_index = -1; //   library

        private List<BatchHeader> localHeaders = new List<BatchHeader>();
        private List<BatchHeader> databaseHeaders = new List<BatchHeader>();


        private string sample_name = "";
        private string type = "";


        public ContainerBatchesForm(bool selectMode = false)
        {
            InitializeComponent();
            if(!selectMode) HideSelectModeControls();

            InitTable();

            FillTable();
        }



        private void HideSelectModeControls()
        {
            groupBox_showmode.Visible = false;
            button_select.Visible = false;
        }





        private void AddBatchHeaderToGrid(IEnumerable<BatchHeader> headers)
        {
            foreach (var h in headers)
            {
                int rowIndex = dataGridView1.Rows.Add();
                dataGridView1.Rows[rowIndex].Cells["Name"].Value = h.Name;
                dataGridView1.Rows[rowIndex].Cells["Type"].Value = h.Type;
                dataGridView1.Rows[rowIndex].Cells["Sample"].Value = h.Sample;
                dataGridView1.Rows[rowIndex].Cells["Width"].Value =  h.Width;
                dataGridView1.Rows[rowIndex].Cells["Height"].Value = h.Height;
                dataGridView1.Rows[rowIndex].Cells["Count"].Value = h.Count;
            }
        }


        private void UpdateLocalHeaders()
        {
            localHeaders.Clear();
            for (int i = 0; i < ImageManager.containerBatches.Count; i++)
            {
                localHeaders.Add(ImageManager.containerBatches[i].GetHeader());
                localHeaders[i].id = i;
            }
        }

        private void UpdatateDBHeaders()
        {
            databaseHeaders.Clear();
            databaseHeaders.AddRange(SamplesDB.GetBatchHeaders().ToArray());
        }

        private void InitTable()
        {

            // dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            // dataGridView1.DataSource = ImageManager.containerBatches;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Columns.Add("Name", "Название");
            dataGridView1.Columns.Add("Type", "Тип");
            dataGridView1.Columns.Add("Sample", "Образец");
            dataGridView1.Columns.Add("Width", "Width");
            dataGridView1.Columns.Add("Height", "Height");
            dataGridView1.Columns.Add("Count", "Count");
        }



        private void FillTable(bool showOnlyRelevant = false)
        {
           
            UpdatateDBHeaders();
            UpdateLocalHeaders();

            dataGridView1.Rows.Clear();

            IEnumerable<BatchHeader> local, db;

            if (showOnlyRelevant)
            {
                local = localHeaders.Where(x => x.Sample == sample_name && x.Type == type);
                db = databaseHeaders.Where(x => x.Sample == sample_name && x.Type == type);
            }
            else
            {
                local = localHeaders;
                db = databaseHeaders;   
            }

            if (local.Count() > 0)
            {
                AddStyledSeparatorRow("Загружено:");
                separator1_index = 0;
                AddBatchHeaderToGrid(local);
            }

            if (db.Count()> 0)
            {
                AddStyledSeparatorRow("Библиотека:");
                separator2_index = local.Count() + 1;
                AddBatchHeaderToGrid(db);
            }
        }

        private void AddStyledSeparatorRow(string separatorText)
        {
            int rowIndex = dataGridView1.Rows.Add();
            DataGridViewRow row = dataGridView1.Rows[rowIndex];

            // Заполняем все ячейки
            //foreach (DataGridViewCell cell in row.Cells)
            //{
            row.Cells[0].Value = separatorText;
            //}

            // Настраиваем стиль строки
            row.DefaultCellStyle.BackColor = Color.FromArgb(230, 240, 255); // Светло-голубой
            row.DefaultCellStyle.ForeColor = Color.Black;
            row.DefaultCellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            row.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 220, 255);
            //row.DefaultCellStyle.A

            // Устанавливаем высоту строки
            row.Height = 25;

            // Блокируем редактирование для строки-разделителя
            row.ReadOnly = true;

            // Добавляем небольшую границу сверху и снизу
            row.DividerHeight = 1;
            if (rowIndex > 0)
            {
                dataGridView1.Rows[rowIndex - 1].DividerHeight = 1;
            }
        }



        private void OpenImage(int BatchIndex)
        {
            int n=-1;
            string str = "Номер кадра";
            ParameterRequestForm form = new ParameterRequestForm();
            form.AddIntRequest(str);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                n = form.RequestInt(str);

            } else return;
            if (n >= ImageManager.containerBatches[BatchIndex].Count && n<0)
            {
                MessageBox.Show("Index out of range");

                return;
            }
            ImageViewForm ivForm = new ImageViewForm(ImageManager.containerBatches[BatchIndex], n);
            ivForm.Show();
        }

        private void CreateContainerBatch()
        {
            AddBatchToDB.AddNewBatch();
            return;
            ContainerBatch batch = new ContainerBatch();
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
                    batch.LocateImageBatch(openFileDialog.FileNames);

                }
            }
            if (batch.Count <= 0)
            {
                MessageBox.Show("Ни один из файлов не подходит");
                return;
            }

            string userInput = Interaction.InputBox("Введите имя группы",
                               "Добавлено "+ batch.Count.ToString() + " карт",
                               "New_Group");

            if (userInput != "")
            {
                batch.Name = userInput;
                ImageManager.containerBatches.Add(batch);
            
            
            }


        }

        private void ExtractContainer(ContainerBatch batch, int index)
        {
            IContainer_2D container = Container_2D.ReadFromFile(batch.Filenames[index]);
            DataManager_2D.containers.Add(container);
        }





        private void button_show_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <=0) return;
            OpenImage(dataGridView1.SelectedRows[0].Index);
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            CreateContainerBatch();
        }

        private void button_extract_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <= 0) return;
            int BatchIndex = dataGridView1.SelectedRows[0].Index;   
            int n = -1;
            string str = "Номер кадра";
            ParameterRequestForm form = new ParameterRequestForm();
            form.AddIntRequest(str);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                n = form.RequestInt(str);

            }
            else return;
            if (n >= ImageManager.containerBatches[BatchIndex].Count && n < 0)
            {
                MessageBox.Show("Index out of range");

                return;
            }
            ExtractContainer(ImageManager.containerBatches[BatchIndex],n);
        }


        public static ContainerBatch GetBatch(string sample, string type)
        { 
            ContainerBatchesForm form = new ContainerBatchesForm(true);

            form.type = type;
            form.sample_name = sample;
            form.ShowDialog();

            return form._selectedBatch;
        
        }

        private void button_select_Click(object sender, EventArgs e)
        {

        }
    }
}
