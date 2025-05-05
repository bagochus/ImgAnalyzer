using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using ImgAnalyzer.MeasurmentTypes;
using ScottPlot.Plottables;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public enum Vert : int { TopLeft = 0, TopRight, BottomRight, BottomLeft  };
    delegate void ListUpdatedDelegate();
    public class ImageProcessor
    {
        public Image image;
        public Tiff tiff_img;

       



        public Point[] corners;
        public int x_sectors;
        public int y_sectors;
        private int TopLeft = 0;
        private int TopRight = 1;
        private int BottomRight = 2;
        private int BottomLeft = 3;
        public List<IMeasurment> measurements = new List<IMeasurment>();
        public EventHandler ListUpdated;

        private ushort[,] max_values;
        private ushort[,] min_values;
        private ushort[,] amplitude_values;
        private ushort[,] good_values;
        private int[,] pseudo_phase_values;




        #region MeasurmentListManagement
        public List<string> GetMeasurnetNames()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < measurements.Count; i++)
            {
                list.Add(measurements[i].Name);
            }
            return list;
        }
        public void AddPointMeasurment(Point point)
        {
            measurements.Add(new PointMeasurment(this,point));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void AddPolygonMeasurment(Point[] points)
        {
            if (points.Length < 3) return;
            measurements.Add(new PolygonMeasurment(this, points));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void AddNewMatrixMeasurnment(Point[] corners,int nx,int ny)
        {
            if (corners.Length != 4) return;
            measurements.Add(new MatrixMeasurment(this, corners, nx, ny));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void RenameItem(int index, string newName)
        {
            try
            {
                measurements[index].Name = newName;
                ListUpdated(this, EventArgs.Empty);
            }
            catch { }
        }

        public void DeleteItem(int index)
        {
            try
            {
                measurements.RemoveAt(index);
                ListUpdated(this, EventArgs.Empty);
            }
            catch { }
        }

        public void SaveMeasurmentList(string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Converters = { new JsonInterfaceConverter<IMeasurment>() }
            };
            string json = JsonSerializer.Serialize(measurements, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filename, json);
        }

        public void LoadMeasurmentList(string filename)
        {

            try
            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonInterfaceConverter<IMeasurment>() }
                };
                string jsonString = File.ReadAllText(filename);
                IMeasurment[] arr_measurements = JsonSerializer.Deserialize<IMeasurment[]>(jsonString, options);
                measurements.Clear();
                foreach (var measure in arr_measurements) measurements.Add(measure);
                ListUpdated(this, EventArgs.Empty);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл не найден!");
            }
            catch (JsonException)
            {
                MessageBox.Show("Ошибка в формате JSON!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
            }


        }
        #endregion




        public void LoadImage(string filename)
        {
            image = Image.FromFile(filename);
            tiff_img = Tiff.Open(filename,"r");
        }

        private byte ConvertIntensity(double intesity)
        { return (byte)intesity; }




        public static bool IsPointInPolygonOptimized(PointF point, PointF[] polygon, RectangleF bounds)
        {
            if (!bounds.Contains(point))
                return false;

            return IsPointInPolygon(point, polygon);
        }

        public static bool IsPointInPolygon(PointF point, PointF[] polygon)
        {
            if (polygon == null || polygon.Length < 3)
                throw new ArgumentException("Polygon must have at least 3 points");

            bool inside = false;
            int n = polygon.Length;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X)
                    * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public double MeasurePolygon(Point[] points)
        {
            return MeanValueStrictBorders(points);
        }

        private double MeanValueStrictBorders(Point[] points)
        {
            PointF[] pointsF = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointsF[i] = new PointF(points[i].X, points[i].Y);
            }
            int max_x = 0;
            int min_x = int.MaxValue;
            int max_y = 0;
            int min_y = int.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X > max_x ) max_x = points[i].X;
                if (points[i].Y > max_y) max_y = points[i].Y;
                if (points[i].X < min_x) min_x = points[i].X;
                if (points[i].Y < min_y) min_y = points[i].Y;

            }
            //int size_x = max_x - min_x;
            //int size_y = max_y - min_y;
            //RectangleF borders = new RectangleF(min_x,min_y,size_x,size_y);
            int pixels_counted = 0;
            Int64 value_counted = 0;

            for (int i = min_x; i < max_x; i++)
            {
                for (int j = min_y; j < max_y; j++)
                {
                    if (IsPointInPolygon(new PointF(i, j), pointsF))
                    {     
                        pixels_counted++;
                        value_counted += MeasurePixel(i, j);
                    }
                }
            }
            return value_counted/pixels_counted;
        }

        public double MeasurePoint(Point point)
        {
            return MeasurePixel(point.X, point.Y);
        }

        public int MeasurePixel(int x,int y)
        {
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, y);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            return pixelData[x];
        }

        public int MeasureLine(int line, int start_index, int count)
        {
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, line);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            int result = 0;
            for (int i = start_index; i < start_index+count; i++)
            {
                result += pixelData[i];
            }

            return result;
        }

        public void BatchMeasurment(string[] filenames)
        {

            for (int i =0; i< measurements.Count;i++) measurements[i].ClearData();

            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                for (int j = 0; j < measurements.Count; j++)
                {
                    measurements[j].Measure();
                }
            }
            return;
        }

        public void FindMinMax(string[] filenames)
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

                if (i==0)
                {
                    min_values = new ushort[width, height];
                    max_values = new ushort[width, height];
                    amplitude_values = new ushort[width, height];
                }
                for (int line = 0; line<height; line++)
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = buffer[pixel];
                        if (i == 0)
                        {
                            min_values[line, pixel] = pixel_value;
                            max_values[line, pixel] = pixel_value;
                        } else
                        {
                            if (pixel_value < min_values[line,pixel]) min_values[line,pixel] = pixel_value;
                            if (pixel_value > max_values[line,pixel]) max_values[line,pixel] = pixel_value;
                        }
                    }
                }
            }
            
            for (int i = 0; i < min_values.GetLength(0);i++)
                for (int j = 0; j < min_values.GetLength(1); j++)
                {
                    amplitude_values[i,j] = (ushort)(max_values[i,j] - min_values[i,j]);
                }

        }

        public void CalculatePseudePhase(string[] filenames,int upper_threshold, int lower_threshold, int max_peak, int min_peak)
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
                bool[,] raising_state;


                if (i == 0)
                {
                    //min_values = new ushort[width, height];
                    //max_values = new ushort[width, height];
                    //amplitude_values = new ushort[width, height];
                }
                for (int line = 0; line < height; line++)
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = buffer[pixel];
                        if (i == 0)
                        {
                            min_values[line, pixel] = pixel_value;
                            max_values[line, pixel] = pixel_value;
                        }
                        else
                        {
                            if (pixel_value < min_values[line, pixel]) min_values[line, pixel] = pixel_value;
                            if (pixel_value > max_values[line, pixel]) max_values[line, pixel] = pixel_value;
                        }
                    }
                }
            }

            for (int i = 0; i < min_values.GetLength(0); i++)
                for (int j = 0; j < min_values.GetLength(1); j++)
                {
                    amplitude_values[i, j] = (ushort)(max_values[i, j] - min_values[i, j]);
                }

        }

        public void MarkDeadPixelByThreshold(int threshold)
        {
            if (amplitude_values == null) return;
            if (amplitude_values.Length == 0) return;
            good_values = new ushort[amplitude_values.GetLength(0),amplitude_values.GetLength(1)];


            for (int i = 0; i < amplitude_values.GetLength(0); i++)
                for (int j = 0; j < amplitude_values.GetLength(1); j++)
                {
                    if (amplitude_values[i, j] > threshold) good_values[i, j] = 1;
                    else good_values[i, j] = 0;
                }
        }



        public void SaveCSV_All(string filename)
        {
            if (measurements.Count == 0) return;    
            int rows = measurements[0].DataCount;
            int cols = measurements.Count;

            var lines = new string[rows+1];
            lines[0] = "";
            for (int i = 0; i < measurements.Count; i++) 
            {
                lines[0] += measurements[i].Name;
                if (i < measurements.Count - 1) lines[0] += ",";

            }
            for (int i = 0; i < rows; i++)
            {
                // Преобразуем строку массива в строку CSV
                var rowValues = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                    List<double>[] data2d = measurements[j].RetrieveData();
                    double[] data = data2d[0].ToArray();
                    rowValues[j] = data[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                lines[i+1] = string.Join(",", rowValues);
            }

            File.WriteAllLines(filename, lines);



        }





    }
}
