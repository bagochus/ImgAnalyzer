using System;
using System.Collections.Generic;
using System.Linq;

namespace ImgAnalyzer._2D
{

    public class Point2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public class LineResult
    {
        public double Slope { get; set; }        // k
        public double Intercept { get; set; }    // b
        public bool IsVertical { get; set; }
        public double VerticalX { get; set; }    // если вертикально

        public override string ToString()
        {
            if (IsVertical)
                return $"x = {VerticalX:F4}";
            return $"y = {Slope:F4}x + {Intercept:F4}";
        }
    }


    public class IntersectionResult
    {
        public bool HasIntersection { get; set; }
        public bool IsParallel { get; set; }     // Параллельны
        public bool IsCoincident { get; set; }   // Совпадают
        public Point2D IntersectionPoint { get; set; }

        public override string ToString()
        {
            if (!HasIntersection)
                return "Прямые не пересекаются";

            if (IsCoincident)
                return "Прямые совпадают (бесконечно много точек пересечения)";

            if (IsParallel)
                return "Прямые параллельны (нет точек пересечения)";

            return $"Точка пересечения: ({IntersectionPoint.X:F4}, {IntersectionPoint.Y:F4})";
        }
    }

}
