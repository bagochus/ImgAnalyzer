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
    internal class PointMeasurment : IMeasurment
    {
        [JsonInclude]
        private Point point;
        private MeasurePointDelegate measurePoint;
        private CalculatePhaseDelegate calculatePhase;
        private List<double>[] data;
        private List<double>[] phaseData;
        public string Name { get; set; }
        public int DataCount { get; set; }
        public PointMeasurment(ImageProcessor imageProcessor, Point point)
        {
            this.point = point;
            this.measurePoint = imageProcessor.MeasurePoint;
            this.calculatePhase = imageProcessor.ConvertIntensityToPhase;
            this.data = new List<double>[1];
            data[0] = new List<double>();
            phaseData = new List<double>[1];
            phaseData[0] = new List<double>();
            Name = "Point "+ point.X.ToString() + ":" + point.Y.ToString();
            DataCount = 0;
        }
        public List<double>[] RetrieveData()
        { return this.data; }
        public void Measure()
        {
            data[0].Add(measurePoint(point));
            DataCount++;
        }
        public void ClearData()
        { 
            data[0].Clear();
            phaseData[0].Clear();
        }
        


        public void MeasurePhase(int n_frame)
        {
            int intensity = (int)measurePoint(point);
            phaseData[0].Add(calculatePhase(point.X,point.Y,n_frame,intensity));
        }

        public List<double>[] RetrievePhaseData()
        {
            return phaseData;
        }



    }
}
