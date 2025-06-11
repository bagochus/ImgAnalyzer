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

    internal class PolygonMeasurment : Measurment
    {
        private bool initialized = false;

        public Point[] points { get; }
       
        private List<AnalyzeLine> analyzeLines = new List<AnalyzeLine>();
        public PolygonMeasurment(Point[] points)
        {
            this.points = points;
            if (points.Length < 3) throw new ArgumentException("Полигон должен имень минимум 3 точки");
            Name = "Poly " + points[0].ToString()+"...";
        }

        public override void Init()
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
                    if (ImageProcessor_1D.IsPointInPolygon(new PointF(_collumn, _row), pointsF))
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
        public override double Measure(ImageProcessor_1D imageProcessor)
        {
            if (!initialized) Init();
            double int_result = 0;
            int pointsCount = 0;

            for (int i = 0; i < analyzeLines.Count; i++)
            {
                int_result += imageProcessor.MeasureLine(analyzeLines[i].line,
                    analyzeLines[i].strart_x,
                    analyzeLines[i].count);
                pointsCount += analyzeLines[i].count;

            }
            return (double)int_result / pointsCount;
        }

        public override IMeasurment Clone()
        {
            return new PolygonMeasurment(points);
        }




    }






}
