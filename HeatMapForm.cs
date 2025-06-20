using ImgAnalyzer._2D;
using NetTopologySuite.Operation.Overlay;
using OpenTK.Graphics.OpenGL;
using ScottPlot;
using ScottPlot.Finance;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace ImgAnalyzer
{
    public partial class HeatMapForm : Form
    {
        private bool UseDoubleData = false;
        private double[,] dataF = new double[0,0];
        private int[,] data = new int[0, 0];

        private int MaxInt = int.MinValue;
        private int MinInt = int.MaxValue;
        private int MaxInt_abs = int.MinValue;
        private int MinInt_abs = int.MaxValue;


        private double MaxDouble = double.MinValue;
        private double MinDouble = double.MaxValue;
        private double MaxDouble_abs = double.MinValue;
        private double MinDouble_abs = double.MaxValue;


        //----------Image management--------------------
        private Image originalImage;
        private Image scaledImage;
        private float zoomFactor = 1.0f;

        private PointF imagePosition = new PointF(0, 0);
        private bool imageRescaled = true;

        private Point dragStart;
        private bool isDragging = false;

        //--------------------------------------------------
        private bool slice_x= false;
        private bool slice_y= false;
        private string containerName;




        private Bitmap thermalImage;

        public HeatMapForm(IContainer_2D container)
        {


            containerName = container.Name;
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
            FindLimits();
            if (UseDoubleData) GenerateThermalImageFloat();
            else GenerateThermalImage();

            GenerateScaleBarImage();
            SetFields();
            UpdateImageDisplay();



            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            pictureBox1.MouseClick += PictureBox_MouseClick;
            pictureBox1.MouseWheel += PictureBox_MouseWheel;

        }

        private void ZoomImage(float factor)
        {
            zoomFactor *= factor;
            if (pictureBox1.Image.Width < pictureBox1.Width) imagePosition.X = 0;
            if (pictureBox1.Image.Height < pictureBox1.Height) imagePosition.Y = 0;
            imageRescaled = true;

            UpdateImageDisplay();
        }
        private void UpdateImageDisplay()
        {

            if (originalImage == null) return;

            // Calculate new size
            int newWidth = (int)(originalImage.Width * zoomFactor);
            int newHeight = (int)(originalImage.Height * zoomFactor);

            // Create a temporary bitmap for zoomed image

            scaledImage = new Bitmap(newWidth, newHeight);

            using (Graphics g = Graphics.FromImage(scaledImage))
            {

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(originalImage, imagePosition.X, imagePosition.Y, newWidth, newHeight);
                imageRescaled = false;
            }
            pictureBox1.Image = scaledImage;



        }



        private void FindLimits()
        {
            if (UseDoubleData)
            {
                foreach (double value in dataF)
                {
                    if (value < MinDouble_abs) MinDouble_abs = value;
                    if (value > MaxDouble_abs) MaxDouble_abs = value;
                }
                MaxDouble = MaxDouble_abs;
                MinDouble = MinDouble_abs;
            }
            else
            {
                foreach (int value in data)
                {
                    if (value < MinInt_abs) MinInt_abs = value;
                    if (value > MaxInt_abs) MaxInt_abs = value;
                }
                MaxInt = MaxInt_abs;
                MinInt = MinInt_abs;
            }
        }


        private void SetFields()
        {

            if (UseDoubleData)
            {
                textBox_max.Text = MaxDouble.ToString();
                textBox_min.Text = MinDouble.ToString();

            }
            else 
            {
                textBox_max.Text = MaxInt.ToString();
                textBox_min.Text = MinInt.ToString();

            }



        }
     



        private void UpdateLimits()
        {



            if (UseDoubleData)
            {
                Double.TryParse(textBox_max.Text, out MaxDouble);
                Double.TryParse(textBox_min.Text, out MinDouble);
                if (MaxDouble < MinDouble_abs + 1) MaxDouble = MinDouble_abs + 1;
                if (MinDouble > MaxDouble_abs - 1) MinDouble = MaxDouble_abs - 1;
                if (MaxDouble < MinDouble) MaxDouble = MinDouble_abs + 1;
                textBox_max.Text = MaxDouble.ToString();
                textBox_min.Text = MinDouble.ToString();



                GenerateThermalImageFloat();
            }
            else
            {
                Int32.TryParse(textBox_max.Text, out MaxInt);
                Int32.TryParse(textBox_min.Text, out MinInt);
                if (MaxInt < MinInt_abs + 1) MaxInt = MinInt_abs + 1;
                if (MinInt > MaxInt_abs - 1) MinInt = MaxInt_abs - 1;
                if (MaxInt < MinInt) MaxInt = MinInt_abs + 1;
                textBox_max.Text = MaxInt.ToString();
                textBox_min.Text = MinInt.ToString();
                GenerateThermalImage();
            }
            GenerateScaleBarImage();
            UpdateImageDisplay();

        }

        private void ImageClick(int x, int y)
        {
            string value;
            if (UseDoubleData) value = dataF[x, y].ToString();
            else value = data[x, y].ToString();   

            CoordinateLabel.Text = $"Coordinates: ({x}, {y}) Value = " + value;

            if (slice_x) SliceX(y);
            if (slice_y) SliceY(x);


        }

        public void ResetView()
        {
            if (UseDoubleData) 
            {
                MaxDouble = MaxDouble_abs;
                MinDouble = MinDouble_abs;
                GenerateThermalImageFloat();
            }
            else
            {
                MaxInt = MaxInt_abs;
                MinInt = MinInt_abs;
                GenerateThermalImage();
            }
                zoomFactor = 1.0f;
            imagePosition = new PointF(0, 0);
            UpdateImageDisplay();
            //pictureBox1.Location = Point.Empty;

        }

        private void PlotHystogramDouble ()
        {
            int barCount = (int)(MaxDouble_abs - MinDouble_abs);
            if (barCount > 200) barCount = 200;
            double[] xdata = new double[barCount];
            int[] ydata = new int[barCount];


            for (int i = 0; i < barCount; i++) 
            {
                xdata[i] = ((MaxDouble_abs - MinDouble_abs) / barCount * i);
                ydata[i] = 0;
            }

            foreach (double v  in dataF)
            {
                for (int i = 0; i < barCount - 1 ; i++) 
                {
                    if (v > xdata[i] && v < xdata[i + 1]) ydata[i]++;
                }
            }

            HystogrammForm form = new HystogrammForm("Hystogram of " + containerName,xdata,ydata);
            form.Show();
        }
        private void PlotHystogramInt()
        {
            int barCount = (int)(MaxInt_abs - MinInt_abs);
            if (barCount > 200) barCount = 200;
            double[] xdata = new double[barCount];
            int[] ydata = new int[barCount];


            for (int i = 0; i < barCount; i++)
            {
                xdata[i] = ((MaxInt_abs - MinInt_abs) / barCount * i);
                ydata[i] = 0;
            }

            foreach (double v in data)
            {
                for (int i = 0; i < barCount - 1; i++)
                {
                    if (v > xdata[i] && v < xdata[i + 1]) ydata[i]++;
                }
            }

            HystogrammForm form = new HystogrammForm("Hystogram of " + containerName, xdata, ydata);
            form.Show();    
        }




        private void SliceX(int y)
        {
            double[] xdata = new double[0];
            double[] ydata = new double[0];
            if (UseDoubleData)
            {
                xdata = new double[dataF.GetLength(0)];
                ydata = new double[dataF.GetLength(0)];
            }
            else
            {
                xdata = new double[data.GetLength(0)];
                ydata = new double[data.GetLength(0)];
            }


            for (int x = 0; x < xdata.Length; x++)
            {
                xdata[x] = x;
                if (UseDoubleData) 
                    ydata[x] = dataF[x, y];
                else ydata[x] = data[x, y];
            }

            PlotForm plotForm = new PlotForm("X, pixels");
            plotForm.Text = "X slice of " + containerName;
            plotForm.AddData($"y = {y}", xdata, ydata);

            plotForm.Show();
        }

        private void SliceY(int x)
        {
            double[] xdata = new double[0];
            double[] ydata = new double[0];
            if (UseDoubleData)
            {
                xdata = new double[dataF.GetLength(1)];
                ydata = new double[dataF.GetLength(1)];
            }
            else
            {
                xdata = new double[data.GetLength(1)];
                ydata = new double[data.GetLength(1)];
            }


            for (int y = 0; y < xdata.Length; y++)
            {
                xdata[y] = y;
                if (UseDoubleData)
                    ydata[y] = dataF[x, y];
                else ydata[y] = data[x, y];
            }

            PlotForm plotForm = new PlotForm("Y, pixels");
            plotForm.Text = "Y slice of " + containerName;
            plotForm.AddData($"x = {x}", xdata, ydata);

            plotForm.Show();
        }


        private void GenerateThermalImageFloat()
        {
            int width = dataF.GetLength(0);
            int height = dataF.GetLength(1);

            thermalImage = new Bitmap(width, height);


            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double value = dataF[x, y];
                    Color pixelColor = GetThermalColor(value, MinDouble, MaxDouble);
                    thermalImage.SetPixel(x, y, pixelColor);
                }
            }

            originalImage = thermalImage;
        }

        private void GenerateThermalImage()
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            thermalImage = new Bitmap(width, height);


            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = data[x, y];
                    Color pixelColor = GetThermalColor(value, MinInt, MaxInt);
                    thermalImage.SetPixel(x, y, pixelColor);
                }
            }

            originalImage = thermalImage;
        }

        private void GenerateScaleBarImage()
        {
            int width = pictureBox_scalebar.Width;
            int height = pictureBox_scalebar.Height;

            Bitmap img = new Bitmap(width, height);


            for (int x = 0; x < width; x++)
            {
                Color pixelColor;
                double value;
                if (UseDoubleData)
                    value = ((double)x / width) * (MaxDouble_abs - MinDouble_abs) + MinDouble_abs;
                else
                    value = ((double)x / width) * (MaxInt_abs - MinInt_abs) + MinInt_abs;

                if (UseDoubleData)
                    pixelColor = GetThermalColor(value, MinDouble, MaxDouble);
                else
                    pixelColor = GetThermalColor(value, MinInt, MaxInt);

                for (int y = 0; y < height; y++)
                img.SetPixel(x, y, pixelColor);
            }

            pictureBox_scalebar.Image = img;



        }

        private Color GetThermalColor(double value, double min, double max)
        {
            if (value < min) value = min;
            if (value > max) value = max;
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

        private void HeatMapForm_Resize(object sender, EventArgs e)
        {
            GenerateScaleBarImage();
        }

        private void PictureBox_MouseWheel(object sender, MouseEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                float zoom = e.Delta > 0 ? 1.1f : 1 / 1.1f;
                ZoomImage(zoom);
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                pictureBox1.Cursor = Cursors.Hand;
                dragStart = e.Location;
                isDragging = true;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                imagePosition.X = e.X - dragStart.X;
                imagePosition.Y = e.Y - dragStart.Y;
                UpdateImageDisplay();
            }


        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isDragging = false;
            }
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (originalImage == null || pictureBox1.Image == null) return;
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the actual image position considering zoom and pan
                float relativeX = (e.X - imagePosition.X) / zoomFactor;
                float relativeY = (e.Y - imagePosition.Y) / zoomFactor;

                // Ensure coordinates are within image bounds
                int imageX = (int)Math.Max(0, Math.Min(originalImage.Width - 1, relativeX));
                int imageY = (int)Math.Max(0, Math.Min(originalImage.Height - 1, relativeY));
                //ovelay_points.Add(new Point(imageX,imageY));
                ImageClick(imageX, imageY);

                UpdateImageDisplay();



                
            }

        }

        private void button_update_Click(object sender, EventArgs e)
        {
            UpdateLimits();
        }

        private void сброситьИзображениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetView();
        }

        private void срезПоXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            срезПоXToolStripMenuItem.Checked ^= true;
            slice_x = срезПоXToolStripMenuItem.Checked;
        }

        private void срезПоYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            срезПоYToolStripMenuItem.Checked ^= true;
            slice_y = срезПоYToolStripMenuItem.Checked;
        }

        private void гистограммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UseDoubleData) 
            PlotHystogramDouble();
            else PlotHystogramInt();
        }
    }
}
