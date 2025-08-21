using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class Divide : Calculation2D
    {
        public Divide()
        {
            this.description = "Попиксельное частное карт A/B";
            this.containerNames = new string[] { "A", "B" };
            

        }

        public override double Measure(int x, int y) 
        {
            if (ContainerParameters[1].ddata(x,y) !=0)
            return ContainerParameters[0].ddata(x, y) / ContainerParameters[1].ddata(x, y);
            else return 0;
        }








    }
}
