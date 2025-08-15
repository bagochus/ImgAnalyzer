using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class NormalizeMinMax : Calculation2D
    {
        public NormalizeMinMax()
        {
            this.description = "Нормализует значения поля Х относительно минимума(FieldMin)\n" +
                                    "и максимума(FieldMin) и приводит к базису min-max";
            this.singeValueNames = new string[] { "min", "max" };
            this.containerNames = new string[] { "Field_min", "Field_max", "X" };
        }
        public override double Measure(int x, int y)
        {


            double a_min = ContainerParameters[0].ddata(x, y);
            double a_max = ContainerParameters[1].ddata(x, y);
            double a = ContainerParameters[2].ddata(x, y);
            double min = SingleValueParameters[0];
            double max = SingleValueParameters[1];



            double a_value =  ((a - a_min) / (a_max - a_min));
            if (a_value > 1) a_value = 1;
            if (a_value < 0) a_value = 0;
            a_value = min + (max-min) * a_value;



            

            return a_value;



        }




    }
}
