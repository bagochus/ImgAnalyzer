using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    internal class MatrixMeasurment : IMeasurment
    {
        public string Name { get; set; }
        public int DataCount { get; set; }
        [JsonInclude]
        private Point[] corners;
        [JsonInclude]
        private int x_sectors;
        [JsonInclude]
        private int y_sectors;
        private int x_current = 0;
        private int y_current = 0;
        private List<double>[] data;
        private MeasurePolygonDelegate measurePolygon;

        private int TopLeft = 0;
        private int TopRight = 1;
        private int BottomRight = 2;
        private int BottomLeft = 3;

        public MatrixMeasurment(ImageProcessor imageProcessor, Point[] corners, int x_sectors,int y_sectors)
        {
            this.corners = corners;
            if (corners.Length != 4 ) throw new AccessViolationException("Матрица должна иметь 4 угла");
            this.x_sectors = x_sectors;
            this.y_sectors = y_sectors;
            Name = "Matrix";
            data = new List<double>[x_sectors * y_sectors];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new List<double>();
            }
            this.measurePolygon = imageProcessor.MeasurePolygon;
            DataCount = 0;
        }
        public List<double>[] RetrieveData()
        { return this.data; }

        public void Measure()
        {
            double kxi = (corners[TopLeft].X - corners[TopRight].X) / x_sectors;
            double kyi = (corners[TopLeft].Y - corners[TopRight].Y) / x_sectors;
            double kxj = (corners[TopLeft].X - corners[BottomLeft].X) / y_sectors;
            double kyj = (corners[TopLeft].Y - corners[BottomLeft].Y) / y_sectors;
            int x0 = corners[TopLeft].X;
            int y0 = corners[TopLeft].Y;

            for (int i = 0; i < x_sectors; i++)
            {
                for (int j = 0; j < y_sectors; j++)
                {
                    Point[] points = new Point[4];
                    points[TopLeft] = new Point(x0 + (int)(kxi * i) + (int)(kxj * j), y0 + (int)(kyi * i) + (int)(kyj * j));
                    points[TopRight] = new Point(x0 + (int)(kxi * (i + 1)) + (int)(kxj * j), y0 + (int)(kyi * (i + 1)) + (int)(kyj * j));
                    points[BottomLeft] = new Point(x0 + (int)(kxi * i) + (int)(kxj * (j + 1)), y0 + (int)(kyi * i) + (int)(kyj * (j + 1)));
                    points[BottomRight] = new Point(x0 + (int)(kxi * (i + 1)) + (int)(kxj * (j + 1)), y0 + (int)(kyi * (i + 1)) + (int)(kyj * (j + 1)));
                    //table.WriteValue(i, j, ConvertIntensity(MeanValueStrictBorders(points)), value);
                    data[x_sectors * i + j].Add(measurePolygon(points));
                }
            }
            DataCount++;

        }

        public void ClearData()
        {
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = new List<double>();
            }
            DataCount = 0;
        }






    }
}
