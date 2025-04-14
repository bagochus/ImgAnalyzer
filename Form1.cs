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
    public enum ClickMode {None, MeasurePixel, CornerCatching}
    public partial class Form1 : Form
    {
        ImageProcessor processor = new ImageProcessor();
        private ClickMode clickMode = ClickMode.None;
        int corner_index = 0;
        Point[] corners = new Point[4];

        public Form1()
        {
            InitializeComponent();
        }

        public void HookClick(Point point)
        {
            switch (clickMode)
            {
                case ClickMode.None: break;
                case ClickMode.MeasurePixel: 
                    MessageBox.Show(
                    processor.MeasurePixel(point.X, point.Y).ToString());
                    break;
                case ClickMode.CornerCatching:
                    CatchCorner(point);
                    break;

            }
        }

        private void CatchCorner(Point point)
        {
            corners[corner_index] = point;
            corner_index++;
            if (corner_index >= 4)
            {
                corner_index = 0;
                MessageBox.Show("Координаты углов считаны");
            
            }

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
            form2.ImageClicked += HookClick;
            /*form2.ImageClicked += (clickPoint) =>
            {
                MessageBox.Show($"Клик на координатах формы: X={clickPoint.X}, Y={clickPoint.Y}"
                    +"\n"+ "Интенсивность:"+
                    processor.MeasurePixel(clickPoint.X,clickPoint.Y).ToString());

            };*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickMode = ClickMode.MeasurePixel;
        }

        private void button_corners_Click(object sender, EventArgs e)
        {
            clickMode = ClickMode.CornerCatching;
        }
    }
}
