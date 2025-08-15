using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class PhaseMeasurment2_acos : Calculation2D
    {
        public PhaseMeasurment2_acos()
        {
            this.description = "Измеряет фазу по 3 каналам, вариант 2. A=sin|B=cos";
            this.singeValueNames = new string[] { "acos threshold" };
            this.containerNames = new string[] { "A_min", "A_max", "B_min", "B_max", "A", "B" };
        }
        public override double Measure(int x, int y)
        {


            double a_min = ContainerParameters[0].ddata(x, y);
            double a_max = ContainerParameters[1].ddata(x, y);
            double b_min = ContainerParameters[2].ddata(x, y);
            double b_max = ContainerParameters[3].ddata(x, y);

            double a = ContainerParameters[4].ddata(x, y);
            double b = ContainerParameters[5].ddata(x, y);

            double acos_th = SingleValueParameters[0];

            double a_value = 2 * ((a - a_min) / (a_max - a_min)) - 1;
            if (a_value > 1) a_value = 1;
            if (a_value < -1) a_value = -1;

            double b_value = 2 * ((b - b_min) / (b_max - b_min)) - 1;
            if (b_value > 1) b_value = 1;
            if (b_value < -1) b_value = -1;

            double modif_b = 1;
            if (Math.Abs(b_value) > (1 - acos_th))
            {
                modif_b = ((Math.Abs(b_value) - 1) / acos_th) + 1;
            }

            double modif_a = 1 - modif_b;


            double phase_a = Math.Asin(a_value);
            if (b_value < 0) phase_a = Math.PI - phase_a;
            if (phase_a < 0) phase_a = 2 * Math.PI + phase_a;
            double phase_b = Math.Acos(b_value);
            if (a_value < 0) phase_b = 2 * Math.PI - phase_b;

            return phase_b;


            
        }

        public new bool Check()
        {
            bool result = base.Check();
            result &= (SingleValueParameters[0] < 1 && SingleValueParameters[0] > 0);


            return result;

        }


    }
}
