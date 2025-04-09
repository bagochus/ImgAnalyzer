using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public partial class Form1 : Form
    {
        ImageProcessor processor = new ImageProcessor();
        public Form1()
        {
            InitializeComponent();
        }

        private void button_openfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Устанавливаем фильтр для файлов .tiff
            openFileDialog.Filter = "TIFF Files (*.tiff)|*.tiff|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1; // Устанавливаем фильтр по умолчанию
            openFileDialog.RestoreDirectory = true; // Восстанавливаем предыдущую директорию

            // Показываем диалоговое окно
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Получаем выбранный файл
                string selectedFilePath = openFileDialog.FileName;
                processor.LoadImage(selectedFilePath);

                // Здесь можно добавить код для работы с выбранным файлом
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ImageForm form2 = new ImageForm(processor.image);
            form2.Show();
            form2.ImageClicked += (clickPoint) =>
            {
                MessageBox.Show($"Клик на координатах формы: X={clickPoint.X}, Y={clickPoint.Y}");
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            processor.MeasurePixel(100, 100);
        }
    }
}
