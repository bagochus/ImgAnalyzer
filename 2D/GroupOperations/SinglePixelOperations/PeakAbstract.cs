using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal abstract class PeakAbstract : SinglePixelOperation
    {

        protected double[,] n_start_positions; // точни начала поиска, из данных пользователя

        protected double[,] peak_values = null; // найденные z-значения пиков

        protected double[,] min_values = null; // текущие минимальные и максимальные найденные значения
        protected double[,] max_values = null;

        protected int[,] n_min_values = null; // текущие номера кадров, соответсвующие минимума и максимумам
        protected int[,] n_max_values = null;

        protected bool[,] peak_found = null;          // найден пик, прекращаем поиск для данного XY
        protected bool[,] threshold_found = null;     // найдено превышение порога, ищем превышение с обратным знаком

        protected int[,] npeak_values = null;           //текущие номера кадров, соответсвующие пикам


        protected double threshold;


        public PeakAbstract()
        {
            operationName = "Экстремум max";
           // description = "Находит X соответстующий экстремуму максимуму.\n" +
             //   "Поиск ведется с начальной позиции Xmin\n" +
               // "th - порог обнаружения экстремума\n";
            this.containerNames = new string[] { "X min" }; // пределы для поиска
            this.singeValueNames = new string[] { "th" }; //  

        }
        



        protected override void Prepare()
        {
            min_values = new double[width, height];
            max_values = new double[width, height];

            n_max_values = new int[width, height];
            n_min_values = new int[width, height];  

            n_start_positions = new double[width, height];
            npeak_values = new int[width,height];


            peak_found = new bool[width, height];
            peak_values = new double[width, height];
            threshold_found = new bool[width, height];
            

            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    min_values[i, j] = Double.MaxValue;
                    max_values[i, j] = Double.MinValue;

                    n_max_values[i, j] = -1;
                    n_min_values[i, j] = -1;
                    npeak_values[i, j] = -1;
                    n_start_positions[i, j] = (int)ContainerParameters[0].ddata(i,j);
                    peak_found[i, j] = false;
                    threshold_found[i, j] = false;

                }
            }
            
            threshold = SingleValueParameters[0];
        }

        protected override void ProcessLine(int y, int n, ushort[] line)
        {


            for (int x = 0; x < width; x++)
            {
                ProcessPixel(x, y, n, line[x]);
            }
               
        }

        //protected abstract override void ProcessPixel(int x, int y, int n, double value);

        protected  void ProcessPixelMaximum(int x, int y, int n, double value)
        {
            if (peak_found[x, y]) return;
            if (n < n_start_positions[x, y]) return;  

            if (!threshold_found[x, y])
            {
                if (value < min_values[x, y])
                {
                    min_values[x, y] = value;
                }

                if (value > max_values[x, y])
                {
                    max_values[x, y] = value;
                    n_max_values[x, y] = n;
                    if (max_values[x, y] > min_values[x, y] + threshold)
                    {
                        threshold_found[x, y] = true;
                        min_values[x, y] = Double.MaxValue;
                    }

                }

            }
            else
            {
                if (value < min_values[x, y])
                {
                    min_values[x, y] = value;
                }

                if (value > max_values[x, y])
                {
                    max_values[x, y] = value;
                    n_max_values[x, y] = n;
                }

                if (max_values[x, y] > min_values[x, y] + threshold)
                {
                    peak_found[x, y] = true;

                }

            }
        }

        protected void ProcessPixelMinimum(int x, int y, int n, double value)
        {
            if (peak_found[x, y]) return;

            if (!threshold_found[x, y])
            {
                if (value < min_values[x, y])
                {
                    min_values[x, y] = value;
                }

                if (value > max_values[x, y])
                {
                    max_values[x, y] = value;
                    n_max_values[x, y] = n;
                    if (max_values[x, y] > min_values[x, y] + threshold)
                    {
                        threshold_found[x, y] = true;
                        min_values[x, y] = Double.MaxValue;
                    }

                }


            }
            else
            {
                if (value < min_values[x, y])
                {
                    min_values[x, y] = value;
                }

                if (value > max_values[x, y])
                {
                    max_values[x, y] = value;
                    n_max_values[x, y] = n;
                }

                if (max_values[x, y] > min_values[x, y] + threshold)
                {
                    peak_found[x, y] = true;
                    npeak_values[x, y] = n;
                    peak_values[x, y] = value;
                }
                    


            }




        }




    }
}
