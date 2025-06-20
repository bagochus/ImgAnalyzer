using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public class Scaling : Calculation2D
    {
        public Scaling()
        {
            this.description = "Проводит линейное преобразование по формуле kx+b";
            this.singeValueNames = new string[] { "k", "b" };
            this.containerNames = new string[] {"x"};

        }



        public override double Measure(int x, int y)
        {
            double k = SingleValueParameters[0];
            double b = SingleValueParameters[1];
            if (ContainerParameters[0] is Container_2D_double)
                return (ContainerParameters[0] as Container_2D_double).data[x, y] * k + b;
            if (ContainerParameters[0] is Container_2D_int)
                return (ContainerParameters[0] as Container_2D_int).data[x, y] * k + b;
            return double.NaN;
        }
    }
}
