using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            this.description = "Находит различие между заданным пикселем и медианной величиной" +
                "в области радиусом r" +
                "r - полуширина квадратной области сравнения в пикселях";
            this.singeValueNames = new string[] { "r" };
            this.containerNames = new string[] { "x" };
        }

        public override void Init()
        {
            base.Init();
           // d = SingleValueParameters[0];
            r = (int)SingleValueParameters[0];

        }

        private double MedianValue(int x, int y)
        {
            double[] values = new double[2*r*r+2*r+1];


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

            for (int i = 0; i < values.Length; i++) values[i] = double.MaxValue;


            for (int i = x_start; i <= x_end; i++)
                for (int j = y_start; j <= y_end; j++)
                    InsertElement(ref values, ContainerParameters[0].ddata(i, j));
            return values[values.Length - 1];

        }

        private void InsertElement(ref double[] array, double value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (value < array[i])
                {
                    double insert_value = array[i];
                    array[i] = value;
                    for (int j = i + 1; j < array.Length; j++)
                    { 
                        double temp = array[j];
                        array[j] = insert_value;
                        insert_value = temp;
                        if (temp == double.MaxValue) return;
                        
                    }
                    return;
                }  
            }
        }




        public override double Measure(int x, int y)
        {
            double point = ContainerParameters[0].ddata(x, y);
            double area = MedianValue(x, y);
            double diff = Math.Abs((point - area));
            return diff;//< d ? normal_value : except_value;
        }
                                

    }
}
