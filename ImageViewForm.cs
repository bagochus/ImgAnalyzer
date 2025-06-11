using ImgAnalyzer.MeasurmentTypes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        private ImageBatch _batch;

        //------------Constants-------------------------
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };


        //----------Image management--------------------
        private Image originalImage;
        private Image scaledImage;
        private float zoomFactor = 1.0f;

        private PointF imagePosition = new PointF(0, 0);
        private bool imageRescaled = true;

        private Point dragStart;
        private bool isDragging = false;





        //-------------Local settings--------------------
        private bool addForAllBatches = false;
        private ClickModeV2 clickMode = ClickModeV2.None;
        private int PointCounter = 0;



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






        public ImageViewForm(ImageBatch batch)
        {
            InitializeComponent();

            pictureBox1.MouseDown += PictureBox_MouseDown;
            pictureBox1.MouseMove += PictureBox_MouseMove;
            pictureBox1.MouseUp += PictureBox_MouseUp;
            pictureBox1.MouseClick += PictureBox_MouseClick;
            pictureBox1.MouseWheel += PictureBox_MouseWheel;
            pictureBox1.Paint += pictureBox1_Paint;

            _batch = batch;
            UpdateOverlays();
        }


        public void LoadImage(string imagePath)
        {
            try
            {
                originalImage = Image.FromFile(imagePath);
                pictureBox1.Image = originalImage;
                scaledImage = originalImage;
                ResetView(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                originalImage = CreateErrorImage();
                pictureBox1.Image = originalImage;


            }
        }

        private Image CreateErrorImage()
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
                DrawOverlays(g);
                
            }
            pictureBox1.Image = scaledImage;



        }
        
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
            for (int i = 0; i < polygons.Count; i++)
            {
                if (polygons.Count != poly_ct_names.Count) break;
                DrawPoly(polygons[i], g, polyColor, poly_names[i]);
            }


            if (frame_poly != null) DrawPoly(frame_poly, g, frColor,"Active Area");

        }

        public void UpdateOverlays()
        {
            ovelay_points = DataManager_1D.Instance.GetPoints(_batch);
            ovelay_points_ct = DataManager_1D.Instance.GetPointsCT(_batch);
            points_ct_coords = DataManager_1D.Instance.GetPoinsCT_Coords(_batch);
            polygons = DataManager_1D.Instance.GetPolys(_batch);
            poly_names = DataManager_1D.Instance.GetPolyNames(_batch);
            frame_poly = _batch.coordinateTransformation?.Polygon;

            UpdateImageDisplay();
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


        private void ClickPoint(int x,int y)
        {
            PointMeasurment pm = new PointMeasurment(new Point(x,y));
            if (addForAllBatches) 
            DataManager_1D.Instance.AddMeasurment(pm);
            else DataManager_1D.Instance.AddMeasurment(pm,_batch);
            UpdateOverlays();
        }
        private void ClickPointCT(int x,int y)
        {
            if (!ImageManager.IsCTDefined(_batch)) return;
            if (addForAllBatches && !ImageManager.AllCTDefined())
            {
                MessageBox.Show("Для одной из групп изображений не определена активная область");
                return;
            }
            DataManager_1D.Instance.AddPointCTMeasurment(new Point(x,y),_batch,addForAllBatches);
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
                DataManager_1D.Instance.AddMeasurment(pmct,_batch);
            }
            else
            {
                PolygonMeasurment pm = new PolygonMeasurment(temp_points.ToArray());
                DataManager_1D.Instance.AddMeasurment(pm,_batch);
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
                else DataManager_1D.Instance.AddMeasurment(ptm,_batch);

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

                if (valid_input && _batch != null )
                {
                    CoordinateTransformation ct = new CoordinateTransformation(points_for_frame.ToArray());
                    ct.frame_width = width;
                    ct.frame_height = height;
                    _batch.coordinateTransformation = ct;
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

        #region Events
        private void ResetView(object sender, EventArgs e)
        {
            zoomFactor = 1.0f;
            imagePosition = new PointF(0, 0);
            UpdateImageDisplay();
            pictureBox1.Location = Point.Empty;
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



                CoordinateLabel.Text = $"Coordinates: ({imageX}, {imageY})";
            }

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //  pictureBox1.Image = scaledImage;
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
            UpdateImageDisplay();
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


        #endregion

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
    }
}
