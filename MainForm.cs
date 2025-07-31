using ImgAnalyzer._2D;
using ImgAnalyzer.DialogForms;
using Microsoft.VisualBasic;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public partial class MainForm : Form
    {
        MainPresenter presenter = new MainPresenter();




        public MainForm()
        {
            InitializeComponent();
        }

        private void button_1d_Click(object sender, EventArgs e)
        {
            DataManager_1D.Instance.ShowForm();
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


                }
            }

        }

        private void button_2d_Click(object sender, EventArgs e)
        {
            DataManager_2D.ShowMainForm();
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            int a;
            try
            {
                a = DB_Manager.SaveImageBatch(ImageManager.Batch_A());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }


        }

        private void button_test2_Click(object sender, EventArgs e)
        {
            ImageManager.Stacks[0] = DB_Manager.LoadImageBatch(4);
        }

        private void сохранитьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userInput = Interaction.InputBox("Введите имя профиля:",
               "Сохрание профиля работы",
               "profile");
            if (userInput != "")
            presenter.SaveProfile(userInput);

        }

        private async void загрузитьПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            await Task.Run(() =>
            {
                presenter.LoadProfile();
            });
            this.Cursor = Cursors.Default;
        }

        private void загрузитьТолькоАктивныеОбластиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            presenter.LoadCT();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_PhaseMeasurment form = new Form_PhaseMeasurment();
            form.ShowDialog();
        }

        private void измеритьФазовыйПрофильToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            presenter.MeasurePhase();
            this.Cursor = Cursors.Default;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Вы действительно хотите закрыть приложение?",
                "Подтверждение закрытия",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            // Если пользователь выбрал "Нет", отменяем закрытие формы
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void button_showContainers_Click(object sender, EventArgs e)
        {
            presenter.OpenContainerBatchesForm();
        }
    }
}
