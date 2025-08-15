using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class InverseX : Calculation2D
    {

        public InverseX() 
        {
            this.description = "Инвертирует поле по X.";
            this.containerNames = new string[] { "X" };


        }

        public override double Measure(int x, int y)
        {
            int w = ContainerParameters[0].Width - 1;
            return ContainerParameters[0].ddata(w - x, y);
        }
    }








}
