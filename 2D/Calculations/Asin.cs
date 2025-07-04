using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class Asin : Calculation2D
    {
        public Asin() 
        {
            this.description = "Вычисляет арксинус поля X. Значение больше +-1 приводятся к единице";
            this.containerNames = new string[] { "X" };


        }

        public override double Measure(int x, int y)
        {
            double a = ContainerParameters[0].ddata[x, y];
            if (a < -1) a = -1;
            if (a > 1) a = 1;
            return Math.Asin(a) * (180 / Math.PI) ;

        }
    }








}
