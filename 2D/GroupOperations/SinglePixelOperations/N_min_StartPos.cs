using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class N_min_StartPos : PeakAbstract
    {

        public N_min_StartPos() 
        {
            operationName = "N_min";
            description = "Находит N соответстующий экстремуму минимуму.\n" +
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
                    result[i,j] = npeak_values[i,j];
                }
        }
    }
}
