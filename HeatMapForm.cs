using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer._2D;

namespace ImgAnalyzer
{
    public partial class HeatMapForm : Form
    {
        private bool UseDoubleData = false;
        private double[,] dataF = new double[0,0];
        private int[,] data = new int[0, 0];

        private Bitmap thermalImage;

        public HeatMapForm(IContainer_2D container)
        {
            if (container is Container_2D_double)
            {
                UseDoubleData = true;
                dataF = (container as Container_2D_double).data;
            }
            else
            {
                {
                    data = (container as Container_2D_int).data;
                }
            }
            Text = "Heatmap: " + container.Name;

            InitializeComponent();
            if (UseDoubleData) GenerateThermalImageFloat();
            else GenerateThermalImage();

            pictureBox1.Image = thermalImage;
        }

        private void GenerateThermalImageFloat()
        {
            int width = dataF.GetLength(0);
            int height = dataF.GetLength(1);

            thermalImage = new Bitmap(width, height);

            // Находим минимальное и максимальное значения для нормализации
            double min = Double.MaxValue;
            double max = Double.MinValue;

            foreach (double value in dataF)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double value = dataF[x, y];
                    Color pixelColor = GetThermalColor(value, min, max);
                    thermalImage.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox1.Image = thermalImage;
        }

        private void GenerateThermalImage()
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            thermalImage = new Bitmap(width, height);

            // Находим минимальное и максимальное значения для нормализации
            double min = Double.MaxValue;
            double max = Double.MinValue;

            foreach (int value in data)
            {
                if (value < min) min = value;
                if (value > max) max = value;
            }

            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = data[x, y];
                    Color pixelColor = GetThermalColor(value, min, max);
                    thermalImage.SetPixel(x, y, pixelColor);
                }
            }

            pictureBox1.Image = thermalImage;
        }




        private Color GetThermalColor(double value, double min, double max)
        {
            // Простая термическая карта (от синего к красному)
            double ratio = (value - min) / (max - min);

            byte r = 0;
            byte g = 0;
            byte b = 0;


            if (ratio < 0.5)
            {
                byte r_max = 200;
                byte r_min = 60;
                byte g_max = 70;
                byte g_min = 0;
                byte b_max = 0;
                byte b_min = 60;

                r = (byte)(r_min + ratio * 2 * (r_max - r_min));
                g = (byte)(g_min + ratio * 2 * (g_max - g_min));
                b = (byte)(b_min + ratio * 2 * (b_max - b_min));
            }
            else 
            {
                byte r_max = 255;
                byte r_min = 200;
                byte g_max = 247;
                byte g_min = 70;
                byte b_max = 220;
                byte b_min = 0;

                r = (byte)(r_min + (ratio-0.5) * 2 * (r_max - r_min));
                g = (byte)(g_min + (ratio - 0.5) * 2 * (g_max - g_min));
                b = (byte)(b_min + (ratio - 0.5) * 2 * (b_max - b_min));



            }



            //byte r = (byte)(255 * ratio);
            //byte b = (byte)(255 * (1 - ratio));
            //byte g = 0;

            return Color.FromArgb(r, g, b);

            // Можно использовать более сложные градиенты:
            // return Color.FromArgb(
            //     (byte)(255 * Math.Sqrt(ratio)),
            //     (byte)(255 * Math.Pow(ratio, 3)),
            //     (byte)(255 * Math.Sin(ratio * Math.PI))
            // );
        }

    }
}
