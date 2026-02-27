using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace ImgAnalyzer.DialogForms
{

    public partial class AddBatchToDB : Form
    {

        ContainerBatch _batch = null;


        private AddBatchToDB()
        {
            InitializeComponent();

            comboBox_sample.Items.Clear();
            comboBox_sample.Items.AddRange(SamplesDB.GetSamplesList().ToArray());
            if (comboBox_sample.Items.Count > 0) comboBox_sample.SelectedIndex = 0;

            comboBox_type.Items.Clear();    
            comboBox_type.Items.AddRange(BatchDatatypes.types.ToArray());
            comboBox_type.SelectedIndex = 0;
        }



        public static void AddNewBatch()
        {
            ContainerBatch batch = new ContainerBatch();
            SelectFiles(batch);

            //TODO: try read metadata from file

            if (batch.Count == 0) return;

            int intersecting_batch_count = 0;
            int intersecting_batch_id = SamplesDB.CheckFilenamesExist(batch.Filenames.ToArray(),out intersecting_batch_count);

            if (intersecting_batch_count > 0)
            { 
                string batch_name = SamplesDB.GetBatchName(intersecting_batch_id);
                DialogResult dialogResult = MessageBox.Show($"Один из указанных файлов найден в группе {batch_name}. Все равно добавить?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            DialogResult dialogResult_base = MessageBox.Show($"Добавить пакет данных в базу?",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult_base == DialogResult.No)
            {
                string userInput = Interaction.InputBox("Введите имя группы",
                   "Добавлено " + batch.Count.ToString() + " карт",
                   "New_Group");

                if (userInput != "")
                {
                    batch.Name = userInput;
                    ImageManager.containerBatches.Add(batch);
                }
                return;
            }

            AddBatchToDB form = new AddBatchToDB();
            form._batch = batch;
            form.ShowDialog();
           
        }

        private static void SelectFiles(ContainerBatch batch)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Настройки диалогового окна
                openFileDialog.Title = "Выберите .bin файлы";
                openFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                openFileDialog.Multiselect = true; // Разрешаем выбор нескольких файлов
                openFileDialog.DefaultExt = "bin";
                openFileDialog.AddExtension = true;
                openFileDialog.InitialDirectory = Path.Combine(Application.StartupPath, "containers");

                if (openFileDialog.ShowDialog() == DialogResult.OK & openFileDialog.FileNames.Length > 0)
                {
                    batch.Filenames.Clear();
                    batch.LocateImageBatch(openFileDialog.FileNames);
                }
                else return;

            }
            if (batch.Count == 0)
            {
                DialogResult dialogResult = MessageBox.Show($"Ни один из файлов не подходит. Выбрать заново?",
                        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                } else SelectFiles(batch);
            }
        }

        private void AddToDB()
        { 
            if (_batch.Filenames.Count == 0) 
            {
                MessageBox.Show("Empty Group!");
                return;
            }
            if (comboBox_sample.Text == "")
            {
                MessageBox.Show("Не указан образец!");
                return;
            }
            if (textBox_name.Text == "")
            {
                MessageBox.Show("Не указано имя пакета");
                return;
            }
            if (SamplesDB.ContainerBatchExists(textBox_name.Text,
                SamplesDB.GetSampleId(comboBox_sample.Text)))
            {
                MessageBox.Show("Для этого образца уже есть пакет данных с таким именем. " +
                    "Выберите другое имя.");
                return;
            }
            if (!SamplesDB.SampleExists(comboBox_sample.Text))
            SamplesDB.AddSample(comboBox_sample.Text);
            
            _batch.Name = textBox_name.Text;
            int sample_id = SamplesDB.GetSampleId(comboBox_sample.Text);
            int batch_id = SamplesDB.AddContainerBatch(_batch, comboBox_type.Text,sample_id,richTextBox_comment.Text);

            //TODO: write metadata to file

            Close();
        }


        private void button_add_Click(object sender, EventArgs e)
        {
            AddToDB();
        }

        private void button_reselect_Click(object sender, EventArgs e)
        {
            SelectFiles(_batch);
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
