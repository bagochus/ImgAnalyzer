using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class Z_min_StartPos : PeakAbstract
    {

        public Z_min_StartPos() 
        {
            operationName = "Z_min";
            description = "Находит Z соответстующий первому обнаруженному минимуму.\n" +
            "Поиск ведется с начальных позиций из массива Nmin\n" +
            "th - порог обнаружения экстремума\n";

        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {

            ProcessPixelMinimum(x, y, n, value);
        }

        protected override void Finish()
        {
            for (int i=0; i<width; i++)
                for (int j=0; j<height; j++)
                {
                    result[i,j] = peak_values[i,j];
                }
        }
    }
}
