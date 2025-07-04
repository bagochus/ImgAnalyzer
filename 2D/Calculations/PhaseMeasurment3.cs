using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class PhaseMeasurment3 : Calculation2D
    {
        public PhaseMeasurment3()
        {
            this.description = "Измеряет фазу по 2 каналам, вариант 3 - арктангенс. A=sin|B=cos";
            this.singeValueNames = new string[] { "acos threshold" };
            this.containerNames = new string[] { "A_min", "A_max", "B_min", "B_max", "A", "B" };
        }
        public override double Measure(int x, int y)
        {


            double a_min = ContainerParameters[0].ddata[x, y];
            double a_max = ContainerParameters[1].ddata[x, y];
            double b_min = ContainerParameters[2].ddata[x, y];
            double b_max = ContainerParameters[3].ddata[x, y];

            double a = ContainerParameters[4].ddata[x, y];
            double b = ContainerParameters[5].ddata[x, y];

            double acos_th = SingleValueParameters[0];

            double a_value = 2 * ((a - a_min) / (a_max - a_min)) - 1;
            if (a_value > 1) a_value = 1;
            if (a_value < -1) a_value = -1;

            double b_value = 2 * ((b - b_min) / (b_max - b_min)) - 1;
            if (b_value > 1) b_value = 1;
            if (b_value < -1) b_value = -1;

            bool a_pos = (a_value > 0);
            bool b_pos = (b_value > 0);

            double phase_shift = 0;
            if (a_pos && !b_pos) phase_shift = Math.PI;
            if (!a_pos && !b_pos) phase_shift = Math.PI;
            if (!a_pos && b_pos) phase_shift = 2*Math.PI;




            return Math.Atan(a_value/b_value) + phase_shift;// / Math.PI * 180;

            
            
        }

        public new bool Check()
        {
            bool result = base.Check();
            result &= (SingleValueParameters[0] < 1 && SingleValueParameters[0] > 0);


            return result;

        }


    }
}
