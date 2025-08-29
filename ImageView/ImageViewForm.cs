using ImgAnalyzer._2D;
using ImgAnalyzer.DialogForms;
using ImgAnalyzer.ImageView;
using ImgAnalyzer.MeasurmentTypes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace ImgAnalyzer
{
    public enum ClickModeV2 {None, Point, PointCT, Poly, PolyCT, Sqare, Frame }

    public partial class ImageViewForm : Form
    {
        //--------------Pointers to global objects------
        public MainPresenter _presenter;




        //------------Constants-------------------------
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };


        //----------Image management--------------------

        private Bitmap originalImage;
        private Bitmap displayImage;
        private PointF imagePosition = new PointF(0, 0);
        private Point lastMousePos;
        private float zoomFactor = 1.0f;
        private bool isPanning;


        //-------------Local settings--------------------
        
        private ClickModeV2 clickMode = ClickModeV2.None;
        private int PointCounter = 0;
        private ImgViewSettings settings = new ImgViewSettings();
        private I2DFileHandler hndl;
        private ColorScheme colorScheme;
        private bool slice_x = false;
        private bool slice_y = false;


        //-------------Image Source Management-----------

        // источник данных, из которого открыта картинка
        private IImageSource imageSource;

        //источник данных, на основе которого рисуются метки
        private IImageSource ovarlaySource;

        //источники данных, в которые будут добавляться измерения
        private List<IImageSource> sources = new List<IImageSource>();

        private bool addForAllBatches = false;

        //----------Overlays from 1D DataManager---------

        //simple points - in photo coordinate system
        private List<Point> ovelay_points = new List<Point>();
        private Color ptColor = Color.Yellow;

        //PointFrame in frame system
        private List<Point> ovelay_points_ct = new List<Point>();
        public List<string> points_ct_coords = new List<string>();
        private Color ptct_color = Color.Lime;


        //polygon for active area (frame)
        private Point[] frame_poly = null;
        private List<Point> points_for_frame = new List<Point>();
        private Color frColor = Color.Magenta;

        // polygons in photo system
        private List<Point[]> polygons = new List<Point[]>();
        private List<string> poly_names = new List<string>();   
        private Color polyColor = Color.Yellow;

        // polygons in frame system
        private List<Point[]> polygons_ct = new List<Point[]>();
        private List<string> poly_ct_names = new List<string>();
        private Color polyctColor = Color.Lime;

        // temporary points
        private List<Point> temp_points = new List<Point>();
        private Color tempColor = Color.Cyan;


        //-----------------------------------------------


        private void Initialize()
        {
            InitializeComponent();

            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            pictureBox1.MouseClick += PictureBox_MouseClick;
            pictureBox1.MouseWheel += PictureBox_MouseWheel;
            pictureBox1.Paint += pictureBox1_Paint;
            this.DoubleBuffered = true;
        }

        public ImageViewForm(IContainer_2D container)
        {
            hndl = new ContainerFileHandler(container);
            Initialize();
            settings.colorMode = ColorMode.Color;
            settings.schemeIndex = 0;
            colorScheme = ColorMaps.Schemes[0].Clone();
            settings.min = hndl.Min();
            settings.max = hndl.Max();
            colorScheme.Min = settings.min;
            colorScheme.Max = settings.max;
            ConstructImage();
        }

        public ImageViewForm(IImageSource imageSource, int index)
        {
            Initialize();
            if (index > imageSource.Count)
            {
                MessageBox.Show("Index out od range");
                originalImage = CreateErrorImage();
                pictureBox1.Image = originalImage;
                return;
            }
            hndl = imageSource.Get2DFileHandler(index);
            this.imageSource = imageSource;
            if (imageSource is ImageBatch)
            {
                settings.colorMode = ColorMode.Simple;
                LoadImage((imageSource as ImageBatch).filenames[index]);
            }
            else
            {
                settings.colorMode= ColorMode.Color;
                settings.schemeIndex = 0;
                colorScheme = ColorMaps.Schemes[0].Clone();
                settings.min = hndl.Min();
                settings.max = hndl.Max();
                colorScheme.Min = settings.min;
                colorScheme.Max = settings.max;

                ConstructImage();
            }



        }

        public void BindImageSource(IImageSource source)
        {
            imageSource = source;
            sources.Add(source);
        }



        public ImageViewForm(ImageBatch batch)
        {
            Initialize();
            

            imageSource = batch;
            sources.Add(imageSource);    
            UpdateOverlays();
        }


        public void LoadImage(string imagePath)
        {
            try
            {
                hndl = new TiffImgFileHandler();
                hndl.LoadFile(imageSource,imagePath);

                settings.min = hndl.Min();
                settings.max = hndl.Max();
                settings.colorMode = ColorMode.Simple;
                settings.schemeIndex = -1;
                originalImage = new Bitmap(imagePath);
                displayImage = new Bitmap(originalImage);
                

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                originalImage = CreateErrorImage();
                pictureBox1.Image = originalImage;


            }
        }

        public void ConstructImage()
        {
            if(originalImage != null) originalImage.Dispose();
            if(displayImage !=null)  displayImage.Dispose();


            originalImage = new Bitmap(hndl.Width, hndl.Height);
            for (int y = 0; y < hndl.Height; y++) {
                {
                    for (int x = 0; x < hndl.Width; x++)
                    {
                        if (settings.colorMode == ColorMode.Color)
                        originalImage.SetPixel(x, y, colorScheme.CalculateColor(hndl.GetPixelValue(x, y)));
                        if (settings.colorMode == ColorMode.BW)
                        originalImage.SetPixel(x, y, ColorScheme.CalculateBW(settings.min, settings.max,hndl.GetPixelValue(x, y)));

                    }
                       
                    int a = 0;
                }
                
            }

            displayImage = new Bitmap(originalImage);
        }


        private async void OpenSettings()
        {
            ImgViewSettingsForm form = new ImgViewSettingsForm(settings);
            form.ShowDialog();

            if (!form.changed) return;
            Cursor = Cursors.WaitCursor;
            await Task.Run(() =>
            {
                colorScheme = ColorMaps.Schemes[settings.schemeIndex].Clone();
                colorScheme.Min = settings.min;
                colorScheme.Max = settings.max;
                ConstructImage();
                pictureBox1.Invalidate();

            });
            Cursor = Cursors.Default;


        }

        private async void PlotHystogramDouble()
        {
            int barCount = 50;
            double[] xdata = new double[barCount];
            int[] ydata = new int[barCount];
            double xstep = (hndl.Max() - hndl.Min()) / barCount;

            for (int i = 0; i < barCount; i++)
            {
                xdata[i] = hndl.Min() + xstep * i;
                ydata[i] = 0;
            }
            Cursor = Cursors.WaitCursor;
            await Task.Run(() =>
            {
                for (int i = 0; i < barCount; i++)
                {
                    ydata[i] = hndl.GetCount(xdata[i], xdata[i] + xstep);
                }
            });
            Cursor = Cursors.Default;
            HistogramForm form = new HistogramForm("Hystogram of " + hndl.Name, xdata, ydata,(float)xstep);
            form.Show();
        }

        private async void PlotHystogram(double min, double max,int barCount)
        {
            
            double[] xdata = new double[barCount];
            int[] ydata = new int[barCount];
            double xstep = (max - min) / (barCount+1);

            for (int i = 0; i < barCount; i++)
            {
                xdata[i] = min + xstep * i;
                ydata[i] = 0;
            }
            Cursor = Cursors.WaitCursor;
            await Task.Run(() =>
            {
                for (int i = 0; i < barCount; i++)
                {
                    ydata[i] = hndl.GetCount(xdata[i], xdata[i] + xstep);
                }
            });
            Cursor = Cursors.Default;
            HistogramForm form = new HistogramForm("Hystogram of " + hndl.Name, xdata, ydata, (float)xstep);
            form.Show();
        }


        private void SliceX(int y)
        {
            double[] xdata = new double[hndl.Width];
            double[] ydata = new double[hndl.Width];


            for (int x = 0; x < xdata.Length; x++)
            {
                xdata[x] = x;
                ydata[x] = hndl.GetPixelValue(x,y);
            }

            PlotForm plotForm = new PlotForm("X, pixels");
            plotForm.Text = "X slice of " + hndl.Name;
            plotForm.AddData($"y = {y}", xdata, ydata);

            plotForm.Show();
        }

        private void SliceY(int x)
        {
            double[] xdata = new double[hndl.Height];
            double[] ydata = new double[hndl.Height];


            for (int y = 0; y < xdata.Length; y++)
            {
                xdata[y] = y;
                ydata[y] = hndl.GetPixelValue(x, y);
            }

            PlotForm plotForm = new PlotForm("Y, pixels");
            plotForm.Text = "Y slice of " + hndl.Name;
            plotForm.AddData($"X = {x}", xdata, ydata);

            plotForm.Show();
        }


        private Bitmap CreateErrorImage()
        {
            Bitmap bmp = new Bitmap(400, 300);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
                using (Font font = new Font("Arial", 12))
                {
                    g.DrawString("Image not available", font, Brushes.Red, 10, 10);
                }
            }
            return bmp;
        }



        #region Orerlays
        private void DrawOverlays(Graphics g)
        {
            foreach (Point pt in temp_points)
                DrawPoint(pt, g, tempColor, "");



            if (!checkBox_overlays.Checked) return;
            // draw points
            foreach (Point pt in ovelay_points)
                DrawPoint(pt, g, ptColor, pt.X.ToString() + ":" + pt.Y.ToString());
            //draw points-ct
            for (int i = 0; i < ovelay_points_ct.Count; i++)
            {
                Point pt = ovelay_points_ct[i];
                DrawPoint(pt, g, ptct_color, points_ct_coords[i]);
            }
            //dram polygons - photo
            for (int i = 0; i<polygons.Count; i++)
            {
                if (polygons.Count != poly_names.Count) break;
                DrawPoly(polygons[i], g, polyColor, poly_names[i]);
            }
            //dram polygons - frame
            for (int i = 0; i < polygons_ct.Count; i++)
            {
                if (polygons_ct.Count != poly_ct_names.Count) break;
                DrawPoly(polygons_ct[i], g, polyctColor, poly_ct_names[i]);
            }


            if (frame_poly != null) DrawPoly(frame_poly, g, frColor,"Active Area");

        }
        public void UpdateOverlays()
        {
            if (imageSource == null) return;
            ovelay_points = DataManager_1D.Instance.GetPoints(imageSource);
            ovelay_points_ct = DataManager_1D.Instance.GetPointsCT(imageSource);
            points_ct_coords = DataManager_1D.Instance.GetPoinsCT_Coords(imageSource);
            polygons = DataManager_1D.Instance.GetPolys(imageSource);
            polygons_ct = DataManager_1D.Instance.GetPolysCT(imageSource);
            poly_names = DataManager_1D.Instance.GetPolyNames(imageSource);
            poly_ct_names = DataManager_1D.Instance.GetPolyCTNames(imageSource);
            frame_poly = imageSource?.coordinateTransformation?.Polygon;

            pictureBox1.Invalidate();
        }
        private void DrawPoint(Point point, Graphics g, Color color,string text)
        {
            Point pt = new Point(0, 0);
            pt.X = (int)(point.X * zoomFactor + imagePosition.X);
            pt.Y = (int)(point.Y * zoomFactor + imagePosition.Y);

            int d = 2;
            int r = 5;
            
            Font font = new Font("Courier New", 15, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(color);
            g.DrawLine(new Pen(color),
                new Point(pt.X + d, pt.Y),
                new Point(pt.X + d + r, pt.Y));
            g.DrawLine(new Pen(color),
                new Point(pt.X - d, pt.Y),
                new Point(pt.X - d - r, pt.Y));
            g.DrawLine(new Pen(color),
                new Point(pt.X, pt.Y+d),
                new Point(pt.X , pt.Y+ d + r));
            g.DrawLine(new Pen(color),
                new Point(pt.X , pt.Y - d),
                new Point(pt.X , pt.Y - d - r));

            g.DrawString(text, font, brush, new Point(pt.X + d,pt.Y +d));
        }
        private void DrawPoly(Point[] points, Graphics g, Color color, string text)
        {


            Point[] pts = new Point[points.Length];
            for (int i = 0; i < pts.Length; i++) 
            
            {
                pts[i] = new Point(0,0);
                pts[i].X = (int)(points[i].X * zoomFactor + imagePosition.X);
                pts[i].Y = (int)(points[i].Y * zoomFactor + imagePosition.Y);
            }

            int d = 10;

            Font font = new Font("Courier New", 15, GraphicsUnit.Pixel);
            Brush brush = new SolidBrush(color);
            Pen pen = new Pen(color);

            g.DrawString(text, font, brush, new Point(pts[0].X, pts[0].Y + d));

            g.DrawPolygon(pen, pts);
            

        }

        #endregion
       
        #region Image click handlers
        private void ClickPoint(int x,int y)
        {
            PointMeasurment pm = new PointMeasurment(new Point(x,y));
            if (addForAllBatches) 
            DataManager_1D.Instance.AddMeasurment(pm);
            else DataManager_1D.Instance.AddMeasurment(pm,sources);
            UpdateOverlays();
        }
        private void ClickPointCT(int x,int y)
        {
            if (!ImageManager.IsCTDefined(imageSource)) return;
            if (addForAllBatches && !ImageManager.AllCTDefined())
            {
                MessageBox.Show("Для одной из групп изображений не определена активная область");
                return;
            }
            DataManager_1D.Instance.AddPointCTMeasurment(new Point(x,y),imageSource,addForAllBatches);
            UpdateOverlays();
        }
        private void ImageClick(int imageX, int imageY)
        {
            switch (clickMode)
            {
                case ClickModeV2.None: break;
                case ClickModeV2.Point: ClickPoint(imageX, imageY); break;
                case ClickModeV2.PointCT: ClickPointCT(imageX, imageY); break;
                case ClickModeV2.Poly: 
                case ClickModeV2.PolyCT: ManagePoly(imageX, imageY); break;
                case ClickModeV2.Sqare:ManageSquare(imageX, imageY); break;
                case ClickModeV2.Frame: ManageFrame(imageX, imageY); break;

                default:break;
            }

        }
        private void ManagePoly (int x,int y)
        {
            temp_points.Add(new Point(x, y));
            PointCounter++;
        }
        private void ClosePoly(bool in_ct)
        {
            if (temp_points.Count < 3)
            {
                temp_points.Clear();
                return;
            }
            if (in_ct)
            {
                PolygonMeasurmentCT pmct = new PolygonMeasurmentCT(temp_points.ToArray());
                DataManager_1D.Instance.AddMeasurment(pmct,sources);
            }
            else
            {
                PolygonMeasurment pm = new PolygonMeasurment(temp_points.ToArray());
                DataManager_1D.Instance.AddMeasurment(pm,sources);
            }
            temp_points.Clear ();
            UpdateOverlays();

        }
        private void ManageSquare(int x,int y)
        {
            temp_points.Add(new Point(x, y));
            if (temp_points.Count > 1)
            {
                Point a = temp_points[0];
                Point c = temp_points[1];
                Point b = new Point(a.X,c.Y);
                Point d = new Point(c.X,a.Y);
                Point[] points = {a,b,c,d};

                PolygonMeasurment ptm = new PolygonMeasurment(points);
                if (addForAllBatches) DataManager_1D.Instance.AddMeasurment(ptm);
                else DataManager_1D.Instance.AddMeasurment(ptm,sources);

                temp_points.Clear();
               
            }
            UpdateOverlays();
        }
        private void ManageFrame(int x, int y)
        {
            temp_points.Add(new Point(x, y));
            points_for_frame.Add(new Point(x,y));
            PointCounter++;
            if (PointCounter == 4)
            {
                int width = 0;
                int height = 0;
                bool valid_input = true;
                string userInput = Interaction.InputBox("Введите ширину активной области в пикселях:",
                   "Определение активной области",
                   "1024");
                valid_input &= (Int32.TryParse(userInput, NumberStyles.Any, frmt, out width));
                userInput = Interaction.InputBox("Введите высоту активной области в пикселях:",
                   "Определение активной области",
                   "512");
                valid_input &= (Int32.TryParse(userInput, NumberStyles.Any, frmt, out height));

                if (valid_input && imageSource != null )
                {
                    CoordinateTransformation ct = new CoordinateTransformation(points_for_frame.ToArray());
                    ct.frame_width = width;
                    ct.frame_height = height;
                    imageSource.coordinateTransformation = ct;
                }
                else MessageBox.Show("Ошибка ввода!");

                points_for_frame.Clear();
                temp_points.Clear();
                PointCounter = 0;
                UpdateOverlays();
                clickMode = ClickModeV2.None;

            }
            UpdateButtons();
        }

        #endregion
       
        #region Events
        private void ResetView(object sender, EventArgs e)
        {
            zoomFactor = 1.0f;
            imagePosition = new PointF(0, 0);
            pictureBox1.Invalidate();
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
            if (originalImage == null ) return;
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

                pictureBox1.Invalidate();



                CoordinateLabel.Text = $"Coordinates: ({imageX}, {imageY}) Value = " + hndl.GetPixelValue(imageX,imageY).ToString() ;
                if (slice_x) SliceX(imageY);
                if (slice_y) SliceY(imageX);

            }

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (displayImage == null) return;

            // Отрисовка с учетом позиции и масштаба
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(displayImage, imagePosition.X, imagePosition.Y,
                               displayImage.Width * zoomFactor,
                               displayImage.Height * zoomFactor);
            DrawOverlays(e.Graphics);

        }


        private void ImageViewForm_Load(object sender, EventArgs e)
        {

        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            ResetView(this, EventArgs.Empty);
        }


        #endregion

        #region Interface
        private void UpdateButtons()
        {
            button_frame.Text = "Определить активную \nобласть";
            button_addpoint.BackColor = SystemColors.Control;
            button_addpointct.BackColor = SystemColors.Control;
            button_addpoly.BackColor = SystemColors.Control;
            button_addpolyct.BackColor = SystemColors.Control;
            button_frame.BackColor = SystemColors.Control;


            switch (clickMode)
            {
                case ClickModeV2.None: break;
                case ClickModeV2.Point: button_addpoint.BackColor = Color.LightGreen; break;
                case ClickModeV2.Frame:
                    {
                        button_frame.BackColor = Color.LightGreen;
                        button_frame.Text = "Определить активную \nобласть: точек " +
                            PointCounter.ToString() + "/4";
                    }
                    break;
                case ClickModeV2.PointCT:
                    button_addpointct.BackColor = Color.LightGreen; break;
                case ClickModeV2.Poly: 
                    button_addpoly.BackColor= Color.LightGreen; break;
                case ClickModeV2.PolyCT:
                    button_addpolyct.BackColor = Color.LightGreen; break;    


            }



        }
        private void button_frame_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.Frame)
            {
                clickMode = ClickModeV2.Frame;


            }
            else clickMode = ClickModeV2.None;

            points_for_frame.Clear();
            PointCounter = 0;
            UpdateButtons();

        }

        private void button_addpoint_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.Point)
            {
                clickMode = ClickModeV2.Point;
            }
            else clickMode = ClickModeV2.None;
            UpdateButtons();   
        }
        private void checkBox_overlays_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void button_pointframe_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.PointCT)
            {
                clickMode = ClickModeV2.PointCT;
            }
            else clickMode = ClickModeV2.None;
            UpdateButtons();
        }
        private void checkBox_allch_CheckedChanged(object sender, EventArgs e)
        {
            addForAllBatches = checkBox_allch.Checked;
        }
        private void button_addsqare_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.Sqare)
            {
                clickMode = ClickModeV2.Sqare;
                temp_points.Clear();
                PointCounter = 0;
            }
            else clickMode = ClickModeV2.None;
            UpdateButtons();
        }

        private void button_addpoly_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.Poly)
            {
                clickMode = ClickModeV2.Poly;

                temp_points.Clear();
                PointCounter = 0;
            }
            else
            {
                clickMode = ClickModeV2.None;
                ClosePoly(false);
            }

            UpdateButtons();
        }

        private void button_polyframe_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickModeV2.PolyCT)
            {
                clickMode = ClickModeV2.PolyCT;

                temp_points.Clear();
                PointCounter = 0;
            }
            else
            {
                clickMode = ClickModeV2.None;
                ClosePoly(true);
            }
                
            UpdateButtons();
        }
        #endregion

        private void checkBox_allch_act_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button_sourceSelect_Click(object sender, EventArgs e)
        {
            sources.Clear();
            SourceSelectForm form = new SourceSelectForm();
            form.ShowDialog();
            this.sources = form.Sources;
        }

        private void настройкиОтображенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSettings();


        }

        private void ImageViewForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            originalImage.Dispose();
            displayImage.Dispose();

        }

        private void измеренияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void срезПоXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slice_x = !slice_x;
            срезПоXToolStripMenuItem.Checked = slice_x;
            
        }

        private void срезПоYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slice_y = !slice_y;
            срезПоYToolStripMenuItem.Checked = slice_y;
        }

        private void гистограммаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void nСтолбцовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ParameterRequestForm();
            form.Text = "Введите параметры";
            form.AddDoubleRequest("Минимум");
            form.AddDoubleRequest("Максимум");
            form.AddIntRequest("Столбцов");

            // Показываем форму как диалоговое окно
            if (form.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    double min = form.RequestDouble("Минимум");
                    double max = form.RequestDouble("Максимум");
                    int count = form.RequestInt("Столбцов");

                    PlotHystogram(min, max, count);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void построитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlotHystogramDouble();
        }

        private void замерВТочкеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int x = 0;  
            int y = 0;
            ParameterRequestForm form = new ParameterRequestForm();
            form.AddIntRequest("X");
            form.AddIntRequest("Y");
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
            {
                x = form.RequestInt("X");
                y = form.RequestInt("Y");

            }
            else return;
            CoordinateLabel.Text = $"Coordinates: ({x}, {y}) Value = " + hndl.GetPixelValue(x, y).ToString();
            if (slice_x) SliceX(y);
            if (slice_y) SliceY(x);
        }
    }
}
