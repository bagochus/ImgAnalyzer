using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class Threshold : Calculation2D
    {
        public Threshold() 
        {
            this.description = "Устанавливает значение А=>(x<min), B=>(min<x<max), C=>(x>max)";
            this.singeValueNames = new string[] { "A", "B","C", "min", "max" };
            this.containerNames = new string[] { "x" };
        }


        public override double Measure(int x, int y)
        {
            double a = SingleValueParameters[0];
            double b = SingleValueParameters[1];
            double c = SingleValueParameters[2];
            double min = SingleValueParameters[3];
            double max = SingleValueParameters[4];
            double value = Double.NaN;
            double input = 0;

            if (ContainerParameters[0] is Container_2D_double)
               input = (ContainerParameters[0] as Container_2D_double).data[x, y];
            if (ContainerParameters[0] is Container_2D_int)
                input = (ContainerParameters[0] as Container_2D_int).data[x, y];
            if (input < min) value = a;
            if (input >= min && input <= max) value = b;
            if (input > max) value = c;
            return value;

        }





    }
}
