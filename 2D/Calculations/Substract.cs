using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class Substract : Calculation2D
    {
        public Substract()
        {
            this.description = "Попиксельная разность карт A-B";
            this.containerNames = new string[] { "A", "B" };
            

        }

        public override double Measure(int x, int y)
        {
            return ContainerParameters[0].ddata(x, y) - ContainerParameters[1].ddata(x, y);
        }








    }
}
