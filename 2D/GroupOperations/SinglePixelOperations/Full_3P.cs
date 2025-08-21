using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class Full_3P : ThreePeak
    {

        public Full_3P() 
        {
            operationName = "Full3p";
            description = "Выводит все промежуточные резутьтаты метода 3 экстремумов.\n" +
            "th - порог обнаружения экстремума\n";

        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            ProcessPixelExtremum(x, y, n, value);
        }

        protected override void Finish()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    result[i,j] = peak_count[i,j];
                }
            AddIntermediateResult("Count");

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = Apeak_values[i, j];
                }
            AddIntermediateResult("1st peak");
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = Bpeak_values[i, j];
                }
            AddIntermediateResult("2nd peak");
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    result[i, j] = Cpeak_values[i, j];
                }
            AddIntermediateResult("3rd peak");




            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
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
            base.Finish();
        }
    }
}
