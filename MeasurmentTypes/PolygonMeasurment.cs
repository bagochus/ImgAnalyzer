using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    internal class AnalyzeLine
    {
        public AnalyzeLine() { count = 1; }
        public int line;
        public int strart_x;
        public int count;
    }

    internal class PolygonMeasurment : IMeasurment
    {
        private bool initialized = false;
        public string Name { get; set; }
        public int DataCount { get; set; }
        [JsonInclude]
        private Point[] points { get; set; }
        private MeasurePolygonDelegate measurePolygon;
        private MeasureLineDelegate measureLine;
        private List<double>[] data;
        private List<AnalyzeLine> analyzeLines = new List<AnalyzeLine>();
        public PolygonMeasurment(ImageProcessor imageProcessor, Point[] points)
        {
            this.points = points;
            if (points.Length < 3) throw new ArgumentException("Полигон должен имень минимум 3 точки");
            this.data = new List<double>[1];
            data[0] = new List<double>();
            Name = "Poly " + points[0].ToString()+"...";
            this.measurePolygon = imageProcessor.MeasurePolygon;
            this.measureLine = imageProcessor.MeasureLine;
            DataCount = 0;
        }
        public List<double>[] RetrieveData()
        { return this.data; }
        public void Measure()
        {
            if (!initialized) Initialize();
            //data[0].Add(measurePolygon(points));
            data[0].Add(MeasuseNew());
            DataCount++;
        }
        public void ClearData()
        {
            initialized = false;
            data[0].Clear();
        }

        private void Initialize()
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
                if (points[i].X > max_x) max_x = points[i].X;
                if (points[i].Y > max_y) max_y = points[i].Y;
                if (points[i].X < min_x) min_x = points[i].X;
                if (points[i].Y < min_y) min_y = points[i].Y;

            }

            
            for (int _row = min_y; _row <= max_y; _row++)
            {
                bool line_continous = false;    
                for (int _collumn = min_x; _collumn < max_x; _collumn++)
                {
                    if (ImageProcessor.IsPointInPolygon(new PointF(_collumn, _row), pointsF))
                    {
                        if (line_continous)
                        {
                            analyzeLines.Last().count++;    
                        } 
                        else
                        {
                            line_continous = true;
                            analyzeLines.Add(new AnalyzeLine { line = _row, strart_x = _collumn });
                        }
                    }
                    else { line_continous = false;}
                }
                line_continous = false ;
            }
            initialized = true;
        }

        private double MeasuseNew()
        {
            double int_result = 0;
            int pointsCount = 0;

            for (int i = 0; i < analyzeLines.Count; i++)
            {
                int_result += measureLine(analyzeLines[i].line,
                    analyzeLines[i].strart_x,
                    analyzeLines[i].count);
                pointsCount += analyzeLines[i].count;

            }
            return (double)int_result / pointsCount;
        }


        public void MeasurePhase(int n_frame)
        {
            throw new NotImplementedException();
        }

        public List<double>[] RetrievePhaseData()
        {
            throw new NotImplementedException();


        }




    }






}
