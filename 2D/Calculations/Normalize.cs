using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class Normalize : Calculation2D
    {
        public Normalize()
        {
            this.description = "Нормализует значения поля Х относительно минимума(FieldMin)\n" +
                                    "и максимума(FieldMin) и приводит к базису (-1)-(+1)";
           // this.singeValueNames = new string[] { "min", "max" };
            this.containerNames = new string[] { "Field_min", "Field_max", "X" };
        }
        public override double Measure(int x, int y)
        {


            double a_min = ContainerParameters[0].ddata[x, y];
            double a_max = ContainerParameters[1].ddata[x, y];
            double a = ContainerParameters[2].ddata[x, y];




            double a_value = 2 * ((a - a_min) / (a_max - a_min)) - 1;
            if (a_value > 1) a_value = 1;
            if (a_value < -1) a_value = -1;

            return a_value;



        }




    }
}
