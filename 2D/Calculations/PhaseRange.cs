using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D

{
    internal class PhaseRange : Calculation2D
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

        public PhaseRange()
        {
            this.description =
                "Рассчитывает фазовый диапазон, реализуемый на p% \n" +
                "площади матрицы. min, max - карты фаз. Выводит карту участков\n" +
                "матрицы, реализующий данный фазовый диапазон";
            this.containerNames = new string[] { "Min", "Max" };
            this.singeValueNames = new string[] { "p" };
            this.pixbypix = false;
            //this.nonReturning = true;
            

        }

        public override double Measure(int x, int y) 
        {
            throw new NotImplementedException();
        }



        public override double[,] MeasureFull()
        {
            double[,] result = new double[width, height];
            percentile = SingleValueParameters[0];
            sortedArrayCount = (int)((1 - percentile / 100 ) * width * height);
            max_values = new double[sortedArrayCount];
            min_values = new double[sortedArrayCount];
            GetMinData = ContainerParameters[0].ddata;
            GetMaxData = ContainerParameters[1].ddata;


            for (int i = 0; i < sortedArrayCount; i++) 
            {
                max_values[i] = Double.MinValue;
                min_values[i] = Double.MaxValue;
            }
            for (int i = 0; i< width; i++) 
                for (int j = 0; j<height;j++)
                {
                     ArrangeMaxValue(GetMinData(i, j));
                    ArrangeMinValue(GetMaxData(i, j));
                }
             bottom = max_values[sortedArrayCount -1];
             top = min_values[sortedArrayCount -1];
             range = top- bottom;


            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    if (GetMinData(i, j) <= bottom && GetMaxData(i, j) >= top)
                        result[i, j] = 1; 
                    else result[i, j] = 0;
                }

            MessageBox.Show("Нижняя граница:\t" + bottom.ToString("F3") + " град. / " +
                (bottom / 180 ).ToString("F3") + "П\n" +
                "Верхняя граница:\t" + top.ToString("F3") + " град. / " +
                (top / 180 ).ToString("F3") + "П\n" +
                "Диапазон:\t" + range.ToString("F3") + " град. / " +
                (range / 180 ).ToString("F3") + "П");
            return result;
        }


        private void ArrangeMinValue(double value)
        {
            int insertIndex = -1;
            for (int i=0; i<sortedArrayCount; i++)
            {
                if (value < min_values[i]) { insertIndex = i; break; }
            }
            if (insertIndex == min_counter)
            {
                min_values[insertIndex] = value;
                min_counter++;
            }
            else if (insertIndex >= 0)
            {
                for (int i = sortedArrayCount - 1; i > insertIndex; i--)
                {
                    min_values[i] = min_values[i - 1];
                }
                min_values[insertIndex] = value;
                min_counter++;
            }
        }

        private void ArrangeMaxValue(double value)
        {
            int insertIndex = -1;
            for (int i = 0; i < sortedArrayCount; i++)
            {
                if (value > max_values[i]) { insertIndex = i; break; }
            }
            if (insertIndex == max_counter)
            {
                max_values[insertIndex] = value;
                max_counter++;
            }
            else if (insertIndex >= 0)
            {
                for (int i = sortedArrayCount - 1; i > insertIndex; i--)
                {
                    max_values[i] = max_values[i - 1];
                }
                max_values[insertIndex] = value;
                max_counter++;
            }
        }



        public override void Process()
        {
             


            int step = (int)SingleValueParameters[0];
            int xcount = width / step;
            int ycount = height / step;


            List<double> dx = new List<double>();
            List<double> dy = new List<double>();

            double[] datax = new double[xcount * ycount];
            double[] datay = new double[xcount * ycount];

            for (int i = 0; i < xcount; i++) 
                for (int j = 0; j < ycount; j++)
                {
                    //i,j - grid indicies
                    //x,y - container indicies
                    int x = i * step;
                    int y = j * step;
                    if (ContainerParameters[0].ddata(x, y) != 0 && ContainerParameters[1].ddata(x, y) != 0)
                    {
                        dx.Add(ContainerParameters[0].ddata(x, y));
                        dy.Add(ContainerParameters[1].ddata(x, y));
                    }



                }

            PlotForm form = new PlotForm(ContainerParameters[0].Name);
            form.Text ="Plot: "+ ContainerParameters[0].Name + " vs " + ContainerParameters[1].Name;
            form.AddDataMarkers(ContainerParameters[0].Name, dx.ToArray(), dy.ToArray());
            form.Show();



        }





    }
}
