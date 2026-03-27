using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D

{
    internal class PhaseRange2 : Calculation2D
    {

        int sortedArrayCount = 0;
        double percentile;
        double[] max_values;
        double[] min_values;
        int max_counter = 0;
        int min_counter = 0;
        Func<int, int, double> GetMaxData;
        Func<int, int, double> GetMinData;

        public double top, bottom, range;

        public bool internal_call= false;
        public Action<string> DisplayMessage = (x) => { };

        public PhaseRange2()
        {
            this.description =
                "Рассчитывает фазовый диапазон, реализуемый на 100% \n" +
                "площади матрицы. min, max - карты фаз. ";
            this.containerNames = new string[] { "Min", "Max" };
            this.singeValueNames = new string[] { "p" };
            this.pixbypix = false;
            this.nonReturning = true;
            

        }

        public override double Measure(int x, int y) 
        {
            throw new NotImplementedException();
        }



        public override double[,] MeasureFull()
        {
            throw new NotImplementedException();

        }

        public override void Process()
        {
            if (!internal_call) DisplayMessage = (s) => MessageBox.Show(s);

            bottom = ContainerParameters[0].Max();
            top = ContainerParameters[1].Min();
            range = top - bottom;



            DisplayMessage("Диапазон:\nНижняя граница:\t" + bottom.ToString("F3") + " град. / " +
                (bottom / 180).ToString("F3") + "П\n" +
                "Верхняя граница:\t" + top.ToString("F3") + " град. / " +
                (top / 180).ToString("F3") + "П\n" +
                "Диапазон:\t" + range.ToString("F3") + " град. / " +
                (range / 180).ToString("F3") + "П");
        }


      




    }
}
