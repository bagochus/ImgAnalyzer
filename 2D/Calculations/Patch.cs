using ScottPlot.SubplotPositions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class Patch : Calculation2D
    {

        public Patch()
        {
            this.description = "Заменяет дефектные участки комбинацией из окружающих значений.\n" +
                "Input - исходная карта. Mask - карта годных участков, где 0 - дефект.";
            this.containerNames = new string[] { "Input", "Mask" };
            

        }

        public override double Measure(int x, int y)
        { 
            if (x== 378 && y==327)Debugger.Break();

            Func<int, int, double> inp = (xx, yy) => ContainerParameters[0].ddata(xx, yy); 
            Func<int, int, double> mask = (xx, yy) => ContainerParameters[1].ddata(xx, yy);

            if (mask(x, y) !=0) return inp(x, y);
            
            double sum = 0;
            double inverse_d = 0;

            //up
            for (int yt = y-1; yt >= 0; yt--)
            {
                if (mask(x, yt) != 0)
                {
                    inverse_d += 1.0f/Math.Abs(y - yt);
                    sum += inp(x, yt) / (double)(Math.Abs(y - yt));
                    break;
                }
            }
            //down
            for (int yt = y + 1; yt < ContainerParameters[0].Height; yt++)
            {
                if (mask(x, yt) != 0)
                {
                    inverse_d += 1.0f/Math.Abs(y - yt);
                    sum += inp(x, yt) / (double)(Math.Abs(y - yt));
                    break;
                }
            }
            //left
            for (int xt = x - 1; xt >= 0; xt--)
            {
                if (mask(xt, y) != 0)
                {
                    inverse_d += 1.0f/Math.Abs(x - xt);
                    sum += inp(xt, y) / (double)(Math.Abs(x - xt));
                    break;
                }
            }
            //right
            for (int xt = x + 1; xt < ContainerParameters[0].Width; xt++)
            {
                if (mask(xt, y) != 0)
                {
                    inverse_d += 1.0f / Math.Abs(x - xt);
                    sum += inp(xt, y) / (double)(Math.Abs(x - xt));
                    break;
                }
            }


            if (inverse_d == 0) return 0;
            return sum / inverse_d;

        }








    }
}
