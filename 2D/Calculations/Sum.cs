using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class Sum : Calculation2D
    {
        public Sum()
        {
            this.description = "Попиксельная сумма карт";
            this.seriesName = "Cлагаемые";
            

        }

        public override double Measure(int x, int y) 
        {
            double sum = 0;
            foreach (Container_2D c in SeriesParameters) 
            {
                if (c is Container_2D_double) sum += (c as Container_2D_double).data[x, y];
                if (c is Container_2D_int) sum += (c as Container_2D_int).data[x, y];
            }
        
            return sum;
        }








    }
}
