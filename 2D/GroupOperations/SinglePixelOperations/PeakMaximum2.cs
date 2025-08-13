using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class PeakMaximum2 : PeakAbstract
    {

        public PeakMaximum2() 
        {
            description = "Находит X соответстующий экстремуму максимуму.\n" +
        "Поиск ведется с начальной позиции Xmin\n" +
        "th - порог обнаружения экстремума\n";

        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {

            ProcessPixelMaximum(x, y, n, value);
        }

    }
}
