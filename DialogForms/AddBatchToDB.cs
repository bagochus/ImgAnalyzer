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
using static SkiaSharp.HarfBuzz.SKShaper;

namespace ImgAnalyzer.DialogForms
{
    
    
    
    public partial class AddBatchToDB : Form
    {

        public string[] BatchTypes = new string[] {"123","312" };

        public List<string> filenames = new List<string>();
        private List<string> SampleNames = new List<string>();


        public AddBatchToDB()
        {
            InitializeComponent();
        }



        public static void AddNewBatch()
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

            int intersecting_batch_count = 0;
            int intersecting_batch_id = SamplesDB.CheckFilenamesExist(batch.Filenames.ToArray(),out intersecting_batch_count);

            if (intersecting_batch_count > 0)
            { 
                string batch_name = SamplesDB.GetSampleName(intersecting_batch_id);
                DialogResult dialogResult = MessageBox.Show($"Один из указанных файлов найден в группе {batch_name}. Все равно добавить?",
                    "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }



            string userInput = Interaction.InputBox("Введите имя группы",
                               "Добавлено " + batch.Count.ToString() + " карт",
                               "New_Group");

            if (userInput != "")
            {
                batch.Name = userInput;
                ImageManager.containerBatches.Add(batch);


            }




        }








    }
}
