using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public class ThermalMapForm : Form
    {
        private int[,] dataArray;
        private Bitmap thermalImage;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel coordinatesLabel;
        private ToolStripStatusLabel valueLabel;
        private PictureBox pictureBox;

        public ThermalMapForm(int[,] data)
        {
            if (data == null || data.Length == 0)
                throw new ArgumentException("Data array cannot be null or empty");

            this.dataArray = data;
            InitializeComponents();
            GenerateThermalImage();
        }

        private void InitializeComponents()
        {
            // Настройка формы
            this.Text = "Thermal Map Viewer";
            this.ClientSize = new Size(800, 600);

            // PictureBox для отображения изображения
            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black
            };
            pictureBox.MouseMove += PictureBox_MouseMove;
            this.Controls.Add(pictureBox);

            // StatusBar
            statusStrip = new StatusStrip();
            coordinatesLabel = new ToolStripStatusLabel
            {
                Text = "X: -, Y: -",
                BorderSides = ToolStripStatusLabelBorderSides.Right,
                BorderStyle = Border3DStyle.Etched
            };
            valueLabel = new ToolStripStatusLabel
            {
                Text = "Value: -"
            };

            statusStrip.Items.AddRange(new ToolStripItem[] { coordinatesLabel, valueLabel });
            this.Controls.Add(statusStrip);
        }

        private void GenerateThermalImage()
        {
            int width = dataArray.GetLength(1);
            int height = dataArray.GetLength(0);

            thermalImage = new Bitmap(width, height);

            // Находим минимальное и максимальное значения для нормализации
            int min = int.MaxValue;
            int max = int.MinValue;

            foreach (int value in dataArray)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = dataArray[y, x];
                    Color pixelColor = GetThermalColor(value, min, max);
                    thermalImage.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox.Image = thermalImage;
        }

        private Color GetThermalColor(int value, int min, int max)
        {
            // Простая термическая карта (от синего к красному)
            float ratio = (float)(value - min) / (max - min);

            byte r = (byte)(255 * ratio);
            byte b = (byte)(255 * (1 - ratio));
            byte g = 0;

            return Color.FromArgb(r, g, b);

            // Можно использовать более сложные градиенты:
            // return Color.FromArgb(
            //     (byte)(255 * Math.Sqrt(ratio)),
            //     (byte)(255 * Math.Pow(ratio, 3)),
            //     (byte)(255 * Math.Sin(ratio * Math.PI))
            // );
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (thermalImage == null) return;

            // Получаем координаты относительно изображения (с учетом масштабирования)
            float xRatio = (float)thermalImage.Width / pictureBox.ClientSize.Width;
            float yRatio = (float)thermalImage.Height / pictureBox.ClientSize.Height;

            int imgX = (int)(e.X * xRatio);
            int imgY = (int)(e.Y * yRatio);

            // Проверяем, что координаты в пределах массива
            if (imgX >= 0 && imgX < dataArray.GetLength(1) &&
                imgY >= 0 && imgY < dataArray.GetLength(0))
            {
                coordinatesLabel.Text = $"X: {imgX}, Y: {imgY}";
                valueLabel.Text = $"Value: {dataArray[imgY, imgX]}";
            }
            else
            {
                coordinatesLabel.Text = "X: -, Y: -";
                valueLabel.Text = "Value: -";
            }
        }
    }

   
}
