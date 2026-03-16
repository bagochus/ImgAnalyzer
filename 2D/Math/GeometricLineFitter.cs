using System;
using System.Collections.Generic;
using System.Linq;

namespace ImgAnalyzer._2D
{

    public class GeometricLineFitter
    {
        /// <summary>
        /// Находит прямую, минимизирующую перпендикулярные расстояния от точек до прямой
        /// </summary>
        public static LineResult FitLine(List<Point2D> points)
        {
            if (points == null || points.Count < 2)
                throw new ArgumentException("Нужно как минимум 2 точки");

            int n = points.Count;

            // 1. Вычисляем средние значения
            double meanX = points.Average(p => p.X);
            double meanY = points.Average(p => p.Y);

            // 2. Центрируем данные и вычисляем ковариационную матрицу
            double covXX = 0, covXY = 0, covYY = 0;

            foreach (var p in points)
            {
                double dx = p.X - meanX;
                double dy = p.Y - meanY;

                covXX += dx * dx;
                covXY += dx * dy;
                covYY += dy * dy;
            }

            // 3. Строим ковариационную матрицу
            double[,] covMatrix = {
            { covXX, covXY },
            { covXY, covYY }
        };

            // 4. Находим собственные значения и векторы
            var (eigenvalue1, eigenvector1, eigenvalue2, eigenvector2) =
                ComputeEigenvectors2x2(covMatrix);

            // 5. Берем собственный вектор для наибольшего собственного значения
            double vx, vy;
            if (eigenvalue1 >= eigenvalue2)
            {
                vx = eigenvector1[0];
                vy = eigenvector1[1];
            }
            else
            {
                vx = eigenvector2[0];
                vy = eigenvector2[1];
            }

            // 6. Формируем результат
            var result = new LineResult();

            // Проверка на вертикальную прямую (маленький vx)
            const double eps = 1e-10;
            if (Math.Abs(vx) < eps)
            {
                result.IsVertical = true;
                result.VerticalX = meanX;
            }
            else
            {
                result.IsVertical = false;
                result.Slope = vy / vx;
                result.Intercept = meanY - result.Slope * meanX;
            }

            return result;
        }

        /// <summary>
        /// Вычисляет собственные значения и векторы для симметричной матрицы 2x2
        /// </summary>
        private static (double eval1, double[] evec1, double eval2, double[] evec2)
            ComputeEigenvectors2x2(double[,] matrix)
        {
            double a = matrix[0, 0];
            double b = matrix[0, 1];
            double c = matrix[1, 1];

            // 1. Вычисляем собственные значения
            double trace = a + c;
            double determinant = a * c - b * b;
            double discriminant = trace * trace - 4 * determinant;

            if (discriminant < 0) discriminant = 0;

            double sqrtDisc = Math.Sqrt(discriminant);
            double eigenvalue1 = (trace + sqrtDisc) / 2;
            double eigenvalue2 = (trace - sqrtDisc) / 2;

            // 2. Вычисляем собственные векторы
            double[] eigenvector1 = ComputeEigenvector(matrix, eigenvalue1);
            double[] eigenvector2 = ComputeEigenvector(matrix, eigenvalue2);

            return (eigenvalue1, eigenvector1, eigenvalue2, eigenvector2);
        }

        private static double[] ComputeEigenvector(double[,] matrix, double eigenvalue)
        {
            double a = matrix[0, 0] - eigenvalue;
            double b = matrix[0, 1];

            // Решаем (A - λI)v = 0
            // Для матрицы 2x2 можно взять вектор [-b, a], если он не нулевой
            double[] vector;

            // Проверяем, не равен ли первый столбец нулевому вектору
            if (Math.Abs(a) > 1e-10 || Math.Abs(b) > 1e-10)
            {
                // Берем v = [-b, a]
                vector = new double[] { -b, a };
            }
            else
            {
                // Иначе берем из второго уравнения
                double c = matrix[1, 0];
                double d = matrix[1, 1] - eigenvalue;
                vector = new double[] { -d, c };
            }

            // Нормализуем вектор
            double norm = Math.Sqrt(vector[0] * vector[0] + vector[1] * vector[1]);
            if (norm > 1e-10)
            {
                vector[0] /= norm;
                vector[1] /= norm;
            }

            return vector;
        }

        /// <summary>
        /// Вычисляет сумму квадратов перпендикулярных расстояний от точек до прямой
        /// </summary>
        public static double CalculateGeometricError(List<Point2D> points, LineResult line)
        {
            double error = 0;

            foreach (var p in points)
            {
                double distance;

                if (line.IsVertical)
                {
                    // Расстояние до вертикальной прямой x = c
                    distance = Math.Abs(p.X - line.VerticalX);
                }
                else
                {
                    // Расстояние от точки до прямой Ax + By + C = 0
                    // Для y = kx + b: kx - y + b = 0 => A = k, B = -1, C = b
                    double A = line.Slope;
                    double B = -1;
                    double C = line.Intercept;

                    distance = Math.Abs(A * p.X + B * p.Y + C) / Math.Sqrt(A * A + B * B);
                }

                error += distance * distance;
            }

            return error;
        }



        public static IntersectionResult FindIntersection(LineResult line1, LineResult line2)
        {
            var result = new IntersectionResult();

            // Случай 1: Обе прямые вертикальны
            if (line1.IsVertical && line2.IsVertical)
            {
                result.HasIntersection = false;
                result.IsParallel = true;

                if (Math.Abs(line1.VerticalX - line2.VerticalX) < 1e-10)
                {
                    result.IsCoincident = true;
                    result.HasIntersection = true;
                }

                return result;
            }

            // Случай 2: Первая прямая вертикальна
            if (line1.IsVertical)
            {
                result.HasIntersection = true;
                result.IsParallel = false;
                result.IsCoincident = false;

                
                double xx = line1.VerticalX;
                double yy = line2.Slope * xx + line2.Intercept;

                result.IntersectionPoint = new Point2D(xx, yy);
                return result;
            }

            // Случай 3: Вторая прямая вертикальна
            if (line2.IsVertical)
            {
                result.HasIntersection = true;
                result.IsParallel = false;
                result.IsCoincident = false;

                double xx = line2.VerticalX;
                double yy = line1.Slope * xx + line1.Intercept;

                result.IntersectionPoint = new Point2D(xx, yy);
                return result;
            }

            // Случай 4: Ни одна из прямых не вертикальна
            // Проверка на параллельность (угловые коэффициенты равны)
            if (Math.Abs(line1.Slope - line2.Slope) < 1e-10)
            {
                result.HasIntersection = false;
                result.IsParallel = true;

                // Проверка на совпадение (b1 = b2)
                if (Math.Abs(line1.Intercept - line2.Intercept) < 1e-10)
                {
                    result.IsCoincident = true;
                    result.HasIntersection = true;
                }

                return result;
            }

            // Случай 5: Прямые пересекаются
            // Решаем систему:
            // y = k1*x + b1
            // y = k2*x + b2
            // => k1*x + b1 = k2*x + b2

            double k1 = line1.Slope, b1 = line1.Intercept;
            double k2 = line2.Slope, b2 = line2.Intercept;

            // Вычисляем x координату пересечения
            double x = (b2 - b1) / (k1 - k2);

            // Вычисляем y координату (используем любую из формул)
            double y = k1 * x + b1;

            // Альтернативно: y = k2 * x + b2 (должно дать тот же результат)
            double y2 = k2 * x + b2;

            // Проверяем согласованность (с учетом погрешности вычислений)
            if (Math.Abs(y - y2) > 1e-8)
            {
                // Должно быть маловероятно, но на всякий случай берем среднее
                y = (y + y2) / 2;
            }

            result.HasIntersection = true;
            result.IsParallel = false;
            result.IsCoincident = false;
            result.IntersectionPoint = new Point2D(x, y);

            return result;
        }


    }
}
