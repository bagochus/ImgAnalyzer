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
        public ContainerBatchesForm()
        {
            InitializeComponent();

           // dataGridView1.AutoGenerateColumns = false;
           dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = ImageManager.containerBatches;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
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


        private void button_show_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count <=0) return;
            OpenImage(dataGridView1.SelectedRows[0].Index);
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            CreateContainerBatch();
        }
    }
}
