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
        public PolygonMeasurmentCT(Point[] point)
        { }
        public override double Measure(ImageProcessor_1D processor)
        { throw new NotImplementedException();  }
        public override void Init()
        {
            throw new NotImplementedException();    
        }

        public override IMeasurment Clone()
        {
            throw new NotImplementedException();
        }



    }
}
