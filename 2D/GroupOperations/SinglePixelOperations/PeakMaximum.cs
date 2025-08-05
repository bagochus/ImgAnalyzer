using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class PeakMaximumAbs : SinglePixelOperation
    {
        private int xmin;
        private int xmax;
        private double[,] max_values;

        
        public PeakMaximumAbs()
        {
            operationName = "X максимум абс.";
            description = "Находит X соответстующий максимальной интенсивности";
            this.singeValueNames = new string[] { "X min", "X_max" }; // пределы для поиска

        }
        

        protected override void Finish()
        {
        }

        protected override void Prepare()
        {
            xmin = (int)SingleValueParameters[0];
            xmax = (int)SingleValueParameters[1];
            max_values = new double[width, height];

            for (int i = 0; i < result.GetLength(0); i++)
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = 0;
                    max_values[i, j] = double.MinValue;
                }

        }

        protected override void ProcessLine(int y, int n, ushort[] line)
        {
            if (n<xmin || n>xmax) return;

            for (int x = 0; x < width; x++)
            {
                if (line[x] > max_values[x, y])
                {
                    max_values[x, y] = line[x];
                    result[x, y] = n;
                }
            }
               
        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            if (value > max_values[x, y])
            {
                max_values[x, y] = value;
                result[x, y] = n;
            }
        }
    }
}
