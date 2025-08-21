using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class PlotXY : Calculation2D
    {
        public PlotXY()
        {
            this.description = "Строит завсимость точек друг от друга из разных наборов\n" +
                "a - шаг сетки";
            this.containerNames = new string[] { "X", "Y" };
            this.singeValueNames = new string[] { "a" };
            this.pixbypix = false;
            

        }

        public override double Measure(int x, int y) 
        {
            throw new NotImplementedException();
        }


        public new void Process()
        {

        }





    }
}
