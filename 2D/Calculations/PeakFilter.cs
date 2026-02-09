using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class PeakFilter : Calculation2D
    {
        private int r = 1;
        private double d = 0;

        public int normal_value = 0;
        public int except_value = 1;



        public PeakFilter() 
        {
            this.description = "Отчечает пиксели, отличающиеся от среднего\n" +
                " значения в своем окружении более чем в (1+d) раз" +
                "r - полуширина квадратной области сравнения в пикселях";
            this.singeValueNames = new string[] { "d", "r" };
            this.containerNames = new string[] { "x" };
        }

        public override void Init()
        {
            base.Init();
            d = SingleValueParameters[0];
            r = (int)SingleValueParameters[1];

        }

        private double AvgValue(int x, int y)
        {
            int x_start, x_end, y_start, y_end;
            if (x < r)
            {
                x_start = 0;
                x_end = 2 * r;
            }
            else if (x >= width - r)
            {
                x_end = width - 1;
                x_start = x_end - 2 * r;
            }
            else
            {
                x_start = x - r;
                x_end = x + r;
            }

            if (y < r)
            {
                y_start = 0;
                y_end = 2 * r;
            }
            else if (y >= width - r)
            {
                y_end = width - 1;
                y_start = y_end - 2 * r;
            }
            else
            {
                y_start = y - r;
                y_end = y + r;
            }

            double sum = 0;
            for (int i = x_start; i <= x_end; i++)
                for (int j = y_start; j <= y_end; j++)
                    sum += ContainerParameters[0].ddata(i, j);
            sum /= (2*r + 1) * (2*r + 1);
            return sum;

        }

        public override double Measure(int x, int y)
        {
            double point = ContainerParameters[0].ddata(x, y);
            double area = AvgValue(x, y);
            double diff = Math.Abs((point - area) / area);
            return diff < d ? normal_value : except_value;
        }
                                

    }
}
