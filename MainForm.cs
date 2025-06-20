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
using ImgAnalyzer._2D;
using ImgAnalyzer.DialogForms;

namespace ImgAnalyzer
{
    public partial class MainForm : Form
    {
        MainPresenter presenter = new MainPresenter();
        DataManager_1D manager_1D = new DataManager_1D();



        public MainForm()
        {
            InitializeComponent();
        }

        private void button_1d_Click(object sender, EventArgs e)
        {
            manager_1D.ShowForm();
        }

        private void button_selctfiles_Click(object sender, EventArgs e)
        {
            ChanellSelectForm chanellSelectForm = new ChanellSelectForm();
            chanellSelectForm.ShowDialog();
            int batch_index = chanellSelectForm.ItemSelected;
            if (batch_index == -1) return;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Настраиваем параметры диалога
                openFileDialog.Filter = "TIFF файлы (*.tif;*.tiff)|*.tif;*.tiff|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true; // Разрешаем выбор нескольких файлов
                openFileDialog.Title = "Выберите TIFF файлы";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    presenter.LocateImageBatch(batch_index, openFileDialog.FileNames);
                }
            }




        }

        private void button_imgview_Click(object sender, EventArgs e)
        {
            ChanellSelectForm chanellSelectForm = new ChanellSelectForm();
            chanellSelectForm.ShowDialog();
            int batch_index = chanellSelectForm.ItemSelected;
            if (batch_index == -1) return;
            if (ImageManager.Batch(batch_index).Count == 0)
            {
                MessageBox.Show("Эта группа изображений пока пуста");
                return;
            }


            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Настраиваем параметры диалога
                openFileDialog.Filter = "TIFF файлы (*.tif;*.tiff)|*.tif;*.tiff|Все файлы (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Multiselect = false;
                string initialDirectory = Path.GetDirectoryName(ImageManager.Batch(batch_index).filenames[0]);
                openFileDialog.InitialDirectory = initialDirectory;
                openFileDialog.Title = "Выберите TIFF файл";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileDialog.FileName;
                    if (!ImageManager.Batch(batch_index).filenames.Contains(filename))
                    {
                        MessageBox.Show("Этого файла нет в выбранной группе");
                        return;
                    }

                    ImageViewForm imageViewForm = new ImageViewForm(ImageManager.Batch(batch_index));
                    imageViewForm.LoadImage(filename);
                    imageViewForm.Show();

                    //test2006
                    //SmoothImageViewer form = new SmoothImageViewer(ImageManager.Batch(batch_index).filenames[0]);
                    //form.Show();

                }
            }

        }

        private void button_2d_Click(object sender, EventArgs e)
        {
            DataManager_2D.ShowMainForm();
        }
    }
}
