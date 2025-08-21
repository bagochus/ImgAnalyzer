using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    enum SearchMode  {Any, Min, Max }

    internal abstract class ThreePeak : SinglePixelOperation
    {

        //protected double[,] n_start_positions; // точни начала поиска, из данных пользователя

        protected double[,] Apeak_values = null; // найденные z-значения пиков
        protected double[,] Bpeak_values = null;
        protected double[,] Cpeak_values = null;


        protected double[,] min_values = null; // текущие минимальные и максимальные найденные значения
        protected double[,] max_values = null;

        protected int[,] n_min_values = null; // текущие номера кадров, соответсвующие минимума и максимумам
        protected int[,] n_max_values = null;

        protected bool[,] peaks_found = null;          // найдены все пики, прекращаем поиск для данного XY


        protected int[,] npeak_values = null;           //текущие номера кадров, соответсвующие пикам
        protected int[,] peak_count = null;         // сколько пиков уже найдено

        protected SearchMode[,] search_modes;



        protected double threshold;


        public ThreePeak()
        {
           // operationName = "3Peak";
           // description = "Находит X соответстующий экстремуму максимуму.\n" +
             //   "Поиск ведется с начальной позиции Xmin\n" +
               // "th - порог обнаружения экстремума\n";
            //this.containerNames = new string[] { "X min" }; // пределы для поиска
            this.singeValueNames = new string[] { "th" }; //  

        }
        



        protected override void Prepare()
        {
            min_values = new double[width, height];
            max_values = new double[width, height];

            n_max_values = new int[width, height];
            n_min_values = new int[width, height];  

            //n_start_positions = new double[width, height];
            npeak_values = new int[width,height];
            peak_count = new int[width,height]; 

            peaks_found = new bool[width, height];
            Apeak_values = new double[width, height];
            Bpeak_values = new double[width, height];
            Cpeak_values = new double[width, height];
            search_modes = new SearchMode[width, height];

            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    min_values[i, j] = Double.MaxValue;
                    max_values[i, j] = Double.MinValue;

                    Apeak_values[i, j] = 0;
                    Bpeak_values[i, j] = 0;
                    Cpeak_values[i, j] = 0;

                    n_max_values[i, j] = -1;
                    n_min_values[i, j] = -1;
                    npeak_values[i, j] = -1;
                    //n_start_positions[i, j] = (int)ContainerParameters[0].ddata(i,j);
                    peaks_found[i, j] = false;
                    peak_count[i, j] = 0;   
                    search_modes[i, j] = SearchMode.Any;



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


        protected override void Finish()
        {
            min_values = null;
            max_values = null;

            n_max_values = null;
            n_min_values =  

            //n_start_positions = new double[width, height];
            npeak_values = null;
            peak_count = null;

            peaks_found = null;
            Apeak_values = null;
            Bpeak_values = null;
            Cpeak_values = null;
            search_modes = null;
            GC.Collect();

        }









        protected void ProcessPixelExtremum(int x, int y, int n, double value)
        {

            if (peaks_found[x, y]) return;

            //вначале непонятно какой экстремум искать, максимум или минимум
            //поэтому просто регистрируем превышение порога
            if (search_modes[x, y] == SearchMode.Any)
            {
                if (value > max_values[x, y])
                {
                    max_values[x, y] = value;
                    n_max_values[x, y] = n;
                    if (max_values[x, y] > min_values[x, y] + threshold)
                    {
                        //мы "взобрались на выстоу", это значит
                        //что следующий пик будет максимумом
                        //или не будет вовсе
                        //теперь наша задача - найти точку которая будет ниже максимума на величину Threshold
                        //и будет правее максимума
                        search_modes[x, y] = SearchMode.Max;
                    }
                }
                if (value < min_values[x, y])
                {
                    min_values[x, y] = value;
                    n_min_values[x, y] = n;
                    if (max_values[x, y] > min_values[x, y] + threshold)
                    {
                        //мы "спустились с высоты", это значит
                        //что следующий пик будет мминимумом
                        //или не будет вовсе
                        //теперь наша задача - найти точку которая будет выше миниимума на величину Threshold
                        //и будет правее минимума
                        search_modes[x, y] = SearchMode.Min;
                    }

                }
            }
            else if (search_modes[x, y] == SearchMode.Min) ProcessPixelMinimum(x, y, n, value);
            else if (search_modes[x, y] == SearchMode.Max) ProcessPixelMaximum(x, y, n, value);
        }
        protected void ProcessPixelMaximum(int x, int y, int n, double value)
        {
            // мы перешли порог "снизу вверх"
            // это значит что мы ищем максимум
            // в этой процедуре мы обновляем максимальное значение 
            // и фиксируем пик, если встретили значение ниже максимума на величину th


            if (value > max_values[x, y])
            {
                max_values[x, y] = value;
                n_max_values[x, y] = n;
            }
            else if (max_values[x, y] > value + threshold)
            {
                //если перещши порог - значит прошлое зарешистрированное значение было локальным максимумом
                CountPeak(max_values[x, y], x, y, n_max_values[x, y]);
                //так как теперь искать будем минимум - начнем с текущего значения
                min_values[x,y] = value;
                n_min_values[x, y] = n;
            }
        }

        protected void ProcessPixelMinimum(int x, int y, int n, double value)
        {
            // мы перешли порог "сверху вниз"
            // это значит что мы ищем минимум
            // в этой процедуре мы обновляем минимальное значение 
            // и фиксируем пик, если встретили значение выше минимума на величину th

            if (value < min_values[x, y])
            {
                min_values[x, y] = value;
                n_min_values[x, y] = n;
            }
            else if (min_values[x, y] < value - threshold)
            {
                //если перещши порог - значит прошлое зарешистрированное значение было локальным минимумом
                CountPeak(min_values[x, y], x, y, n_min_values[x, y]);
                //так как теперь искать мы будем максимум - начнем с текущего значения
                max_values[x, y] = value;
                n_max_values[x, y] = n;
            }
        }

        // счетчик пиков 
        private void CountPeak(double data, int x, int y, int n)
        {
            peak_count[x, y]++;
            if (peak_count[x, y] == 1) Apeak_values[x, y] = data;
            if (peak_count[x, y] == 2) Bpeak_values[x, y] = data;
            if (peak_count[x, y] == 3)
            {
                Cpeak_values[x, y] = data;
                peaks_found[x, y] = true;
            }
            if (search_modes[x, y] == SearchMode.Min) search_modes[x, y] = SearchMode.Max;
            else search_modes[x, y] = SearchMode.Min;




        }

    }
}
