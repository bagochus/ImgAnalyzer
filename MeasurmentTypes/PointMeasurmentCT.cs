using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    internal class PointMeasurmentCT : Measurment
    {
        private readonly PointF invalidPoint = new PointF(float.NaN, float.NaN);

        PixelWeightMatrix _weightMatrix; 

        public PointF PointFrame {
            get { return pointFrame; }
            set 
            {
                pointFrame = value;
                FrameToPhoto();
            }
        }

        public Point PointFrameInt { get { return new Point((int)pointFrame.X, (int)pointFrame.Y); } }

        private PointF pointFrame = new PointF(float.NaN,float.NaN);
        
        public PointF PointPhoto { get { return pointPhoto; } }
        public Point PointPhotoInt 
        {
            get { return new Point((int)pointPhoto.X, (int)pointPhoto.Y); }
        }
        private PointF pointPhoto;
        


        public void PhotoToFrame()
        {
            if (Batch == null) return;
            if (Batch.coordinateTransformation == null) return;
            if (_weightMatrix == null) _weightMatrix = Batch.coordinateTransformation.GeneratePWM_point(pointPhoto);
            pointFrame = Batch.coordinateTransformation.BackTransformPoint(pointPhoto);
        }
        public void FrameToPhoto()
        {
            if (Batch == null) return;
            if (Batch.coordinateTransformation == null) return;
            if (_weightMatrix == null) _weightMatrix = Batch.coordinateTransformation.GeneratePWM_point(pointPhoto);
            pointPhoto = Batch.coordinateTransformation.TransformPoint(pointFrame);
        }



        public override void Init()
        {
            bool error = false;
            error |= (Batch == null);
            error |= (Batch.coordinateTransformation == null);
            error |= (pointFrame == invalidPoint && pointPhoto == invalidPoint);
            if (error) throw new Exception("Неправильно определено измерение");

            if (pointFrame == invalidPoint) FrameToPhoto();
            else PhotoToFrame();

            _weightMatrix = Batch.coordinateTransformation.GeneratePWM_point(pointFrame);
        }

        public PointMeasurmentCT(Point point,ImageBatch batch)
        {
            this.pointPhoto = point;
            Batch = batch;
            PhotoToFrame();
            Name = "PointCT " + point.X.ToString() + ":" + point.Y.ToString();
        }
        public PointMeasurmentCT() { }




        public override double Measure(ImageProcessor_1D _imageProcessor)
        {
            double result = Double.NaN;
            if (_imageProcessor == null) { return result; }
            if (Batch == null) { return result; }
            if (Batch.coordinateTransformation == null) { return result; }
            if (_weightMatrix == null) 
            {
                _weightMatrix = Batch.coordinateTransformation.GeneratePWM_point(PointFrame);
            }
            result = _imageProcessor.Measure_PWM(_weightMatrix);
            result *= Batch.coordinateTransformation.k_area;
           
            return result;
        }

        public override IMeasurment Clone()
        {
            if (Batch?.coordinateTransformation == null) throw (new Exception("Невозможно копировать измерение, система координат не определена"));
            PointMeasurmentCT clone_pt = new PointMeasurmentCT();
            clone_pt.pointFrame = pointFrame;
            return clone_pt;
        }







    }
}
