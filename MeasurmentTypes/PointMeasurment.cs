using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ImgAnalyzer.MeasurmentTypes
{
    internal class PointMeasurment : Measurment
    {

        public Point point;
        ImageProcessor_1D _imageProcessor;
        public ImageBatch _stack;



        public int DataCount { get; set; }

        public PointMeasurment(Point point)
        {
            this.point = point;
            Name = "Point "+ point.X.ToString() + ":" + point.Y.ToString();
        }

        public override void Init()
        { }

        public override double Measure(ImageProcessor_1D processor)
        {
            double result = Double.NaN;
            result = processor.MeasurePixel(point.X, point.Y);
            return result;
        }

        public override IMeasurment Clone()
        {
            return new PointMeasurment(point);  
        }


    }
}
