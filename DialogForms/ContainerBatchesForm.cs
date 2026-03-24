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

        private List<BatchHeader> localHeaders_disp = new List<BatchHeader>();
        private List<BatchHeader> databaseHeaders_disp = new List<BatchHeader>();



        private string sample_name = "";
        private string type = "";

        public static ContainerBatch GetBatch(string sample, string type)
        {
            ContainerBatchesForm form = new ContainerBatchesForm(true, sample, type);

            form.ShowDialog();

            return form._selectedBatch;
        }

        public ContainerBatchesForm(bool selectMode = false, string sample_name = "", string type = "")
        {
            this.sample_name = sample_name;
            this.type = type;
            InitializeComponent();
            if(!selectMode) HideSelectModeControls();

            InitTable();

            FillTable(selectMode);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
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


            if (showOnlyRelevant)
            {
                localHeaders_disp = localHeaders.Where(x => x.Sample == sample_name && x.Type == type).ToList();
                databaseHeaders_disp = databaseHeaders.Where(x => x.Sample == sample_name && x.Type == type).ToList();
            }
            else
            {
                localHeaders_disp = localHeaders;
                databaseHeaders_disp = databaseHeaders;   
            }

            if (localHeaders_disp.Count() > 0)
            {
                AddStyledSeparatorRow("Загружено:");
                separator1_index = 0;
                AddBatchHeaderToGrid(localHeaders_disp);
            }

            if (databaseHeaders_disp.Count()> 0)
            {
                AddStyledSeparatorRow("Библиотека:");
                separator2_index = localHeaders_disp.Count() == 0? 0 :  localHeaders_disp.Count() + 1;
                AddBatchHeaderToGrid(databaseHeaders_disp);
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



        private void OpenImage()
        {
            ContainerBatch batch = null;
            try
            {
                bool fromDB = false;
                var header = GetSelectedBatch(out fromDB);
                if (header == null) return;
                if (fromDB) batch = SamplesDB.GetBatch(header.id);
                else batch = ImageManager.containerBatches[header.id];
            }
            catch (Exception ex)
            {
                _selectedBatch = null;
                MessageBox.Show("Не удалось загрузить пакет: \n" + ex.Message);
                return;
            }
            if (batch.Count == 0) return;

            int n=-1;
            string str = "Номер кадра";
            ParameterRequestForm form = new ParameterRequestForm();
            form.AddIntRequest(str);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                n = form.RequestInt(str);

            } else return;
            if (n >= batch.Count || n<0)
            {
                MessageBox.Show("Index out of range");

                return;
            }
            ImageViewForm ivForm = new ImageViewForm(batch, n);
            ivForm.Show();
        }

        private void CreateContainerBatch()
        {
            AddBatchToDB.AddNewBatch();
            FillTable();
        }



        private void ExtractContainer()
        {
            ContainerBatch batch = null;
            try
            {
                bool fromDB = false;
                var header = GetSelectedBatch(out fromDB);
                if (header == null) return;
                if (fromDB) batch = SamplesDB.GetBatch(header.id);
                else batch = ImageManager.containerBatches[header.id];
            }
            catch (Exception ex)
            {
                _selectedBatch = null;
                MessageBox.Show("Не удалось загрузить пакет: \n" + ex.Message);
                return;
            }
            if (batch.Count == 0) return;


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
            if (n >= batch.Count || n < 0)
            {
                MessageBox.Show("Index out of range");

                return;
            }

            IContainer_2D container = Container_2D.ReadFromFile(batch.Filenames[n]);
            DataManager_2D.containers.Add(container);
        }

        private void SelectClick()
        {
            try
            {
                bool fromDB = false;
                var header = GetSelectedBatch(out fromDB);
                if (header == null) return;
                if (fromDB) _selectedBatch = SamplesDB.GetBatch(header.id);
                else _selectedBatch = ImageManager.containerBatches[header.id];
            }
            catch (Exception ex)
            {
                _selectedBatch = null;
                MessageBox.Show("Не удалось загрузить пакет: \n" + ex.Message);
                return;
            }

            if (_selectedBatch.Count == 0)
            {
                _selectedBatch = null;
                MessageBox.Show("Не удалось загрузить карты из выбранного пакета данных");
            }
            else Close();
        }

        private BatchHeader GetSelectedBatch(out bool fromDB)
        {
            fromDB = false;

            //ничего не выбрано
            if (dataGridView1.SelectedRows.Count <= 0) return null;
            int index = dataGridView1.SelectedRows[0].Index;
            //выбран разделитель
            if (index == separator1_index
                || index == separator2_index) return null;

            if (localHeaders_disp.Count > 0 && index <= localHeaders_disp.Count)
            {

                return localHeaders_disp[index - 1];
            }
            else if (databaseHeaders_disp.Count > 0 && index > separator2_index)
            {
                fromDB = true;
                int offset_index = index - separator2_index - 1;
                return databaseHeaders_disp[offset_index];
            }
            return null;
        }

        private void DeleteBatchClick()
        {
            try
            {
                bool fromDB = false;
                var header = GetSelectedBatch(out fromDB);
                if (header == null) return;
                if (fromDB) 
                {
                    if (!SamplesDB.DeleteContainerBatch(header.id))
                        throw new Exception("Ошибка БД");
                }
                else ImageManager.containerBatches.RemoveAt(header.id);
                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить пакет: \n" + ex.Message);
                return;
            }
        }


        private void DeleteWithFiles()
        {
            BatchHeader header;
            ContainerBatch batch;

            try
            {
                bool fromDB = false;
                header = GetSelectedBatch(out fromDB);
                if (header == null) return;
                if (fromDB)
                {
                    batch = SamplesDB.GetBatch(header.id);
                    if (batch == null) throw new Exception("Ошибка БД");
                }
                else batch = ImageManager.containerBatches[header.id];


                int int_count = 0;
                int id = SamplesDB.CheckFilenamesExist(batch.Filenames.ToArray(), out int_count);
                if (int_count > 1)
                {
                    DialogResult dr = MessageBox.Show($"Фaйла из данного пакета найдены еще в {int_count} пакетах. Все равно удалить?",
                        "Подтвержение",MessageBoxButtons.YesNo);
                    if (dr == DialogResult.No) return;
                }

                DialogResult dr2 = MessageBox.Show($"Уверены что хотите удалить пакет вместе сфайлами?",
                            "Подтвержение", MessageBoxButtons.YesNo);
                if (dr2 == DialogResult.No) return;

                if (batch.DeleteAllFiles())
                {
                    if (fromDB) SamplesDB.DeleteContainerBatch(header.id);
                    else ImageManager.containerBatches.RemoveAt(header.id);
                    FillTable();
                }
                else MessageBox.Show($"Не удалось удалить {batch.Count} файлов из пакета");
                         


            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось удалить пакет: \n" + ex.Message);
                return;
            }




        }

        private void ChangeBatchVisiblity()
        {
            UpdatateDBHeaders();
            UpdateLocalHeaders();

            FillTable(radioButton_showRelevant.Checked);
        }

        private void SelectionChanged()
        {
            richTextBox1.Clear();
            //ничего не выбрано
            if (dataGridView1.SelectedRows.Count <= 0) return;
            int index = dataGridView1.SelectedRows[0].Index;
            //выбран разделитель
            if (index == separator1_index
                || index == separator2_index) return;

            try
            {
                if (databaseHeaders_disp.Count > 0 && index > separator2_index)
                {
                    int offset_index = index - separator2_index - 1;
                    int id = databaseHeaders_disp[offset_index].id;
                    richTextBox1.Text = SamplesDB.GetBatchComment(id);
                }
            }
            catch 
            {

            }

        }


        private void button_show_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            CreateContainerBatch();
        }

        private void button_extract_Click(object sender, EventArgs e)
        {

            ExtractContainer();
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            SelectClick();
        }

        private void radioButton_shoall_CheckedChanged(object sender, EventArgs e)
        {
            ChangeBatchVisiblity();
        }

        private void radioButton_showRelevant_CheckedChanged(object sender, EventArgs e)
        {
            ChangeBatchVisiblity();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            SelectionChanged();
        }

        private void button_delete_Click(object sender, EventArgs e)
        {
            DeleteBatchClick();
        }

        private void button_deletefiles_Click(object sender, EventArgs e)
        {
            DeleteWithFiles();
        }
    }
}
