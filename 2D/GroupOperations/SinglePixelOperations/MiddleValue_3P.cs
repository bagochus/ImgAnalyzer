using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class MiddleValue_3P : ThreePeak
    {

        public MiddleValue_3P() 
        {
            operationName = "Center";
            description = "Находит центральное значение колебаний из 3 экстремумов.\n" +
            "th - порог обнаружения экстремума\n";

        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            ProcessPixelExtremum(x, y, n, value);
        }

        protected override void Finish()
        {
            for (int i=0; i<width; i++)
                for (int j=0; j<height; j++)
                {
                    if (peaks_found[i,j])
                    {
                        double a = Apeak_values[i, j];
                        double b = Bpeak_values[i, j];
                        double c = Cpeak_values[i, j];
                        result[i,j] = Math.Abs ((a+c)/4 + b/2);

                    }
                    else result[i,j] = 0;

                }
        }
    }
}
