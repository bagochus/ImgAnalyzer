using ImgAnalyzer._2D;
//using ClipperLib;
using NetTopologySuite.Geometries;
using OpenTK;
using ScottPlot.Plottables;
using ScottPlot.Rendering.RenderActions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
//using System.Numerics;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Point = System.Drawing.Point;
using Polygon = NetTopologySuite.Geometries.Polygon;
using Vector2 = OpenTK.Vector2;


namespace ImgAnalyzer
{
    

    public class PixelWeight
    {
        public int x { get; set; }
        public int y { get; set; }
        public double weight { get; set; }
        public PixelWeight(int x, int y, double weight)
        {
            this.x = x;
            this.y = y;
            this.weight = weight;
        }
    }

    public class PixelWeightMatrix
    {
        public List<PixelWeight> weights = new List<PixelWeight>();
        public int left { get { return Left(); } }
        public int top { get; }
        public int right { get { return Right(); } }
        public int bottom { get; }
        public int Count { get { return weights.Count; } }
        private int _left = int.MaxValue;
        private int _top = int.MinValue;
        private int _right = int.MinValue;
        private int _bottom = int.MaxValue;

        public int pix_checked = 0;
        public int nx = 0;
        public int ny = 0;

        private void ResetBorders()
        {
            _left = int.MaxValue;
            _top = int.MinValue;
            _right = int.MinValue;
            _bottom = int.MaxValue;
        }

        public void AddWeight(PixelWeight weight)
        {
            this.weights.Add(weight);
            ResetBorders();
        }
        private int Left()
        {
            if (_left != int.MaxValue) return _left;

            for (int i = 0; i < weights.Count; i++)
            {
                if (weights[i].x < _left) _left = weights[i].x;
            }
            return _left;

        }
        private int Right()
        {
            if (_right != int.MinValue) return _right;

            for (int i = 0; i < weights.Count; i++)
            {
                if (weights[i].x > _right) _right = weights[i].x;
            }
            return _right;
        }




    }

    public class PolygonBorders
    {
        private int xmin = int.MaxValue;
        private int xmax = int.MinValue;
        private int ymin = int.MaxValue;
        private int ymax = int.MinValue;
        
        public int x_min { get => xmin;}
        public int x_max { get => xmax;}
        public int y_min { get => ymin;}
        public int y_max { get => ymax;}

        public int Count()
        {
            return (xmax - xmin)*(ymax - ymin);
        }
        public int nx()
        { return xmax-xmin; }
        public int ny() { return ymax-ymin; }


        public PolygonBorders(PointF[] points)
        {
            for (int i = 0; i< points.Length; i++)
            {
                if (Math.Floor(points[i].X) < xmin) xmin = (int)Math.Floor(points[i].X);
                if (Math.Floor(points[i].X) + 1 > xmax) xmax = (int)Math.Floor(points[i].X) + 1;
                if (Math.Floor(points[i].Y) < ymin) ymin = (int)Math.Floor(points[i].Y);
                if (Math.Floor(points[i].Y) + 1 > ymax) ymax = (int)Math.Floor(points[i].Y) + 1;
            }
        }
    }

    public class CoordinateTransformation
    {
        /*
          Система координат соответсвует экранной - 0:0 - верхний левый угол
          Y направлена вниз 
          X направлена вправо
        */
        public const string ct_header = "FullFieldMatrix";

        public string FulCTFilename { 
            get { return ct_filename; }
            set { ct_filename = value; }}
        private string ct_filename= "";

        public PointF point_BL {  get; set; }
        public PointF point_TL { get; set; }
        public PointF point_TR { get; set; }
        public PointF point_BR {  get; set; }
        private int width = 1024;
        public int frame_width {
            get { return width; }
            set 
            {
                width = value;
                CalcTranslationVectors();
            } }
        private int height = 512;
        public int frame_height
        {
            get { return height; }
            set
            {
                height = value;
                CalcTranslationVectors();
            }
        }
        public double k_area { get { return ka; } }

        public PixelWeightMatrix[,] FullFiedTransformation { 
            get 
            {   if (!fullFieldCalculated) calculateFullField();
                return fullField;
            }
        }
        private PixelWeightMatrix[,] fullField;
        public bool FullFieldCalculated { get { return fullFieldCalculated; } }
        private bool fullFieldCalculated = false;



        public Point[] Polygon { get { return polygon(); } }

        private OpenTK.Vector2 xt;
        private OpenTK.Vector2 yt;
        private OpenTK.Vector2 xb;
        private OpenTK.Vector2 yb;
        private double ka;
        private PointF ZeroB;

        private OpenTK.Matrix2 BackMatrix;

        //максимальная игнорируемая площадь пересечения пикселей
        private double s_min = 1e-9;
        public double MinimuaArea { get { return s_min; } set { s_min = value; } }

        //размер оригинального изображения
        private int x_max = 1280;
        private int y_max = 1024;

        public int XMax { get { return x_max; } set { x_max = value; } }
        public int YMax { get { return y_max; } set { y_max = value; } }

        public bool RangeExceprion = false;

        public CoordinateTransformation(Point[] points)
        {
            if (points.Length != 4) throw new ArgumentException();
            point_BL = points[0];
            point_TL = points[1];
            point_TR = points[2];
            point_BR = points[3];
            CalcTranslationVectors();
        }

        public CoordinateTransformation(PointF[] points)
        {
            if (points.Length < 3) throw new ArgumentException();
            point_BL = points[0];
            point_TL = points[1];
            point_TR = points[2];
            CalcTranslationVectors();
        }


        private void CalcTranslationVectors()
        {
            fullFieldCalculated = false;

            float Xxt = point_TR.X - point_TL.X;
            float Xyt = point_TR.Y - point_TL.Y;

            float Yyt = point_BL.Y - point_TL.Y;
            float Yxt = point_BL.X - point_TL.X;

            point_BR = new Point((int)(point_TL.X + Xxt + Yxt), (int)(point_TL.Y + Xyt + Yyt));
                

            Xxt /= width;
            Xyt /= width;

            Yxt /= height;
            Yyt /= height;

            xt = new OpenTK.Vector2(Xxt, Xyt);
            yt = new OpenTK.Vector2(Yxt, Yyt);

            // 1 / area of frame pixel in photo system
            ka = 1 / Math.Abs(Xxt * Yyt - Xyt * Yxt);

            BackMatrix = new Matrix2(Xxt, Xyt, Yxt, Yyt);
            BackMatrix.Invert();
            xb = new OpenTK.Vector2(BackMatrix.M11, BackMatrix.M12);
            yb = new OpenTK.Vector2(BackMatrix.M21, BackMatrix.M22);


        }

        private Point ConvertToPoint(PointF point)
        {
            return new Point((int)point.X, (int)point.Y);
        }

        private Point[] polygon()
        {
            Point[] points = new Point[4];
            points[0] = ConvertToPoint(point_TL);
            points[1] = ConvertToPoint(point_TR);
            points[2] = ConvertToPoint(point_BR);
            points[3] = ConvertToPoint(point_BL);
            return points;

        }
        


        //from frame system to photo system
        public PointF TransformPoint(PointF point)
        {
            double x = point_TL.X + (point.X * xt).X + (point.Y * yt).X ;
            double y = point_TL.Y + (point.X * xt).Y + (point.Y * yt).Y;
            return new PointF((float)x, (float)y);

        }

        public PointF[] TransformPolygon(PointF[] points)
        {
            PointF[] result = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++) result[i] = TransformPoint(points[i]);
            return result;
        }
        // from photo system to frame system

        public PointF BackTransformPoint(PointF point)
        {
            OpenTK.Vector2 rv_b = (point.X - point_TL.X) * xb + (point.Y - point_TL.Y) * yb;
            if(rv_b.X > x_max)
            {
                rv_b.X = x_max;
                if (RangeExceprion) throw new Exception("Point out of photo range");
            }
            if (rv_b.Y > y_max)
            {
                rv_b.Y = y_max;
                if (RangeExceprion) throw new Exception("Point out of photo range");
            }

            return new PointF(rv_b.X, rv_b.Y);
        }

        public PointF[] BackTransformPolygon(PointF[] points) 
        {
            PointF[] result = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++) result[i] = BackTransformPoint(points[i]);
            return result;
        }

        
        public PixelWeightMatrix GeneratePWM_point(PointF point)
        {
            PixelWeightMatrix result = new PixelWeightMatrix();
            // unity polygon - 1x1 polygon in frame system with BL PointFrame @ "PointFrame"
            PointF[] unity_polygon = new PointF[4];
            unity_polygon[0] = point;
            unity_polygon[1] = new PointF(point.X, point.Y+1);
            unity_polygon[2] = new PointF(point.X+1, point.Y+1);
            unity_polygon[3] = new PointF(point.X+1, point.Y);
            // UP_b - same polygon in photo system
            PointF[] UP_b = TransformPolygon(unity_polygon);
            PolygonBorders borders = new PolygonBorders(UP_b);
            result.nx = borders.nx();                                               //debug
            result.ny = borders.ny();                                               //debug
            var unity_poly_frame = BuildNTSPoly(UP_b);
            for (int x = borders.x_min; x <= borders.x_max - 1 ; x ++) 
                for (int y = borders.y_min; y <= borders.y_max - 1;y++)
                {
                    result.pix_checked++;
                    var unity_poly_photo = BuildNTSPoly_point(new PointF(x,y));
                    var intersection = unity_poly_frame.Intersection(unity_poly_photo);
                    double area = intersection.Area;
                    if (area > s_min) result.AddWeight(new PixelWeight(x,y,area));
                }
            return result;
        }

        public PixelWeightMatrix GeneratePWM_poly(PointF[] points)
        {
            PixelWeightMatrix result = new PixelWeightMatrix();
            // UP_b - polygon in photo system
            PointF[] UP_b = TransformPolygon(points);
            PolygonBorders borders = new PolygonBorders(UP_b);

            for (int x = borders.x_min; x < borders.x_max - 1; x++)
                for (int y = borders.y_min; y < borders.y_max - 1; y++)
                {
                    var unity_poly_frame = BuildNTSPoly(UP_b);
                    var unity_poly_photo = BuildNTSPoly_point(new PointF(x, y));
                    var intersection = unity_poly_frame.Intersection(unity_poly_photo);
                    double area = intersection.Area;
                    if (area > s_min) result.AddWeight(new PixelWeight(x, y, area));
                }
            return result;
        }


        private Polygon BuildNTSPoly(PointF[] points)
        {
            Coordinate[] coords = new Coordinate[points.Length+1];

            for (int i = 0; i < points.Length; i++)
            {
                coords[i] = new Coordinate(points[i].X, points[i].Y);   

            }
            coords[coords.Length -1 ] = new Coordinate(points[0].X, points[0].Y);
            LinearRing lr = new LinearRing(coords);
            Polygon poly = new Polygon(lr);
            return poly;
        }

        private Polygon BuildNTSPoly_point(PointF point)
        {
            Coordinate[] coords = new Coordinate[5];
            coords[0] = new Coordinate(point.X, point.Y);
            coords[1] = new Coordinate(point.X, point.Y + 1);
            coords[2] = new Coordinate(point.X + 1, point.Y + 1);
            coords[3] = new Coordinate(point.X + 1, point.Y);
            coords[4] = new Coordinate(point.X, point.Y);
            LinearRing lr = new LinearRing(coords);
            Polygon poly = new Polygon(lr);
            return poly;
        }

        private async void calculateFullField()
        {
            fullField = new PixelWeightMatrix[width, height];

            await Task.Run(() =>
            {
                lock (fullField)
                {
                    DataManager_2D.workToBeDone += width;
                    Parallel.For(0, width, (int i) =>

                    {
                        for (int j = 0; j < height; j++)
                        {
                            fullField[i, j] = GeneratePWM_point(new PointF(i, j));

                        }
                        DataManager_2D.progress.Report(1);
                    });

                }
                fullFieldCalculated = true;
            });
        }

        public void CalculateFullField()
        {
            if (fullFieldCalculated) return;
            if (ct_filename != "" && !fullFieldCalculated)
            {
                try
                {
                LoadFullField(ct_filename);
                }
                catch
                {
                    calculateFullField();
                }
            }

        }

        public void SaveFullField(string filename)
        {
            if (!fullFieldCalculated) return;
            if (filename == ct_filename) return;
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(ct_header);
                writer.Write(width);
                writer.Write(height);
                for (int i = 0; i < fullField.GetLength(0); i++)
                    for (int j = 0; j < fullField.GetLength(1); j++)
                    {
                        writer.Write(fullField[i, j].Count);
                        foreach (var pw in fullField[i,j].weights)
                        {
                            writer.Write(pw.x);
                            writer.Write(pw.y);
                            writer.Write(pw.weight);
                        }

                    }
            }
            ct_filename = filename;

        }

        public void LoadFullField(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                string header = reader.ReadString();
                if (header != ct_header)
                {
                    MessageBox.Show("Файл не содержит матрицу преобразований");
                    return;
                }
                int file_width = reader.ReadInt32();
                int file_height = reader.ReadInt32();
                if (file_width != width || file_height != height)
                {
                    MessageBox.Show("Размер матрицы некорректен");
                    return;
                }
                fullField = new PixelWeightMatrix[width,height];


                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        var pw = new PixelWeightMatrix();
                        int count = reader.ReadInt32();
                        for (int k = 0; k < count; k++)
                        {
                            int x = reader.ReadInt32();
                            int y = reader.ReadInt32();
                            double w = reader.ReadDouble();
                            pw.weights.Add(new PixelWeight(x, y, w));
                        }
                        fullField[i, j] = pw;
                    }
                }
                fullFieldCalculated = true;
                ct_filename = filename;
            }
        }


    }
}
