using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    public class PolygonMeasurmentCT : Measurment
    {
        public PointF[] points_t;
        PixelWeightMatrix _weightMatrix;

        public PointF[] PointsFrame {
            get { return pointsFrame; }
            set 
            {
                pointsFrame = value;
                FrameToPhoto();
            }
        }

        public Point[] PointsFrameInt { get { return ConvertToInt(pointsFrame); } }

        private PointF[] pointsFrame = new PointF[0];
        
        public PointF[] PointsPhoto { get { return pointsPhoto; } }
        public Point[] PointsPhotoInt 
        {
            get { return ConvertToInt(pointsPhoto); }
        }
        private PointF[] pointsPhoto= new PointF[0];

        public void PhotoToFrame()
        {
            if (Source == null) return;
            if (Source.coordinateTransformation == null) return;
            if (_weightMatrix == null) _weightMatrix = Source.coordinateTransformation.GeneratePWM_poly(pointsPhoto);
            pointsFrame = Source.coordinateTransformation.BackTransformPolygon(pointsPhoto);
        }
        public void FrameToPhoto()
        {
            if (Source == null) return;
            if (Source.coordinateTransformation == null) return;
            if (_weightMatrix == null) _weightMatrix = Source.coordinateTransformation.GeneratePWM_poly(pointsPhoto);
            pointsPhoto = Source.coordinateTransformation.TransformPolygon(pointsFrame);
        }


        private Point[] ConvertToInt(PointF[] points)
        {
            Point[] result = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                result[i] = new Point((int)points[i].X, (int)points[i].Y);
            }
            return result;
        }

        public PolygonMeasurmentCT(Point[] points)
        {
            pointsPhoto = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointsPhoto[i] = points[i];
                PhotoToFrame();
            }
        }

        public PolygonMeasurmentCT()
        {

        }


        public override double Measure(ImageProcessor_1D processor)
        {
            double result = Double.NaN;
            if (processor == null) { return result; }
            if (Source == null) { return result; }
            if (Source.coordinateTransformation == null) { return result; }
            if (_weightMatrix == null)
            {
                _weightMatrix = Source.coordinateTransformation.GeneratePWM_poly(pointsFrame);
            }
            result = processor.Measure_PWM(_weightMatrix);
            result *= Source.coordinateTransformation.k_area;

            return result;
        }



        public override void Init()
        {
            bool error = false;
            error |= (Source == null);
            error |= (Source.coordinateTransformation == null);
            error |= (pointsFrame == null && pointsPhoto == null);
            if (error) throw new Exception("Неправильно определено измерение");

            if (pointsFrame == null) FrameToPhoto();
            else PhotoToFrame();

            _weightMatrix = Source.coordinateTransformation.GeneratePWM_poly(pointsFrame);    
        }

        public override IMeasurment Clone()
        {
            if (Source?.coordinateTransformation == null) throw (new Exception("Невозможно копировать измерение, система координат не определена"));
            PolygonMeasurmentCT clone_pt = new PolygonMeasurmentCT();
            clone_pt.PointsFrame = this.pointsFrame;
            return clone_pt;
        }



    }
}
