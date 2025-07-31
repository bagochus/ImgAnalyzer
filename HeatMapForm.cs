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


        private Bitmap displayImage;
        private PointF imagePosition = new PointF(0, 0);
        private Point lastMousePos;
        private float zoomFactor = 1.0f;
        private bool isPanning;


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
                displayImage = new Bitmap(dataF.GetLength(0), dataF.GetLength(1));
            }
            else
            {
                {
                    data = (container as Container_2D_int).data;
                    displayImage = new Bitmap(data.GetLength(0), data.GetLength(1));
                }
            }
            

            InitializeComponent();
            FindLimits();
            if (UseDoubleData) GenerateThermalImageFloat();
            else GenerateThermalImage();

            GenerateScaleBarImage();
            SetFields();

            this.Text = "Heatmap: " + container.Name;


            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            pictureBox1.MouseClick += PictureBox_MouseClick;
            pictureBox1.MouseWheel += PictureBox_MouseWheel;
            pictureBox1.Paint += pictureBox1_Paint;
            this.DoubleBuffered = true;

            try
            {
              
                //displayImage = new Bitmap(originalImage);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


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
            pictureBox1.Invalidate();

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
            SetFields();
                zoomFactor = 1.0f;
            imagePosition = new PointF(0, 0);
            pictureBox1.Invalidate();
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

            HistogramForm form = new HistogramForm("Hystogram of " + containerName,xdata,ydata);
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

            HistogramForm form = new HistogramForm("Hystogram of " + containerName, xdata, ydata);
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

            //thermalImage = new Bitmap(width, height);


            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double value = dataF[x, y];
                    Color pixelColor = GetThermalColor(value, MinDouble, MaxDouble);
                    displayImage.SetPixel(x, y, pixelColor);
                }
            }

            //displayImage = thermalImage;
        }

        private void GenerateThermalImage()
        {
            int width = data.GetLength(0);
            int height = data.GetLength(1);

            //thermalImage = new Bitmap(width, height);


            // Создаем изображение
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int value = data[x, y];
                    Color pixelColor = GetThermalColor(value, MinInt, MaxInt);
                    displayImage.SetPixel(x, y, pixelColor);
                }
            }

            //displayImage = thermalImage;
            
        }

        private void GenerateScaleBarImage()
        {
            int width = pictureBox_scalebar.Width;
            int height = pictureBox_scalebar.Height;
            if (width < 1 || height < 1) return;

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
            if (ModifierKeys == Keys.Control)
            {
                float zoom = e.Delta > 0 ? 1.1f : 1 / 1.1f;
                zoomFactor *= zoom;
                pictureBox1.Invalidate();
            }
        }

        private void PictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isPanning = true;
                lastMousePos = e.Location;
                Cursor = Cursors.Hand;
            }
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                // Плавное перемещение с коэффициентом сглаживания
                float smoothFactor = 0.7f;
                imagePosition.X += (e.X - lastMousePos.X) * smoothFactor;
                imagePosition.Y += (e.Y - lastMousePos.Y) * smoothFactor;

                lastMousePos = e.Location;
                pictureBox1.Invalidate();
            }


        }

        private void PictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                isPanning = false;
                Cursor = Cursors.Default;
            }
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (displayImage == null ) return;
            if (e.Button == MouseButtons.Left)
            {
                // Calculate the actual image position considering zoom and pan
                float relativeX = (e.X - imagePosition.X) / zoomFactor;
                float relativeY = (e.Y - imagePosition.Y) / zoomFactor;

                // Ensure coordinates are within image bounds
                int imageX = (int)Math.Max(0, Math.Min(displayImage.Width - 1, relativeX));
                int imageY = (int)Math.Max(0, Math.Min(displayImage.Height - 1, relativeY));
                //ovelay_points.Add(new Point(imageX,imageY));
                ImageClick(imageX, imageY);

                pictureBox1.Invalidate();



                
            }

        }

        private void button_update_Click(object sender, EventArgs e)
        {
            UpdateLimits();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (displayImage == null) return;

            // Отрисовка с учетом позиции и масштаба
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(displayImage, imagePosition.X, imagePosition.Y,
                               displayImage.Width * zoomFactor,
                               displayImage.Height * zoomFactor);
            //DrawOverlays(e.Graphics);

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
