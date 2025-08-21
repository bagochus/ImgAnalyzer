using BitMiracle.LibTiff.Classic;
using ImgAnalyzer;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{

    public  class ExtremumSearch
    {
        int start_position = 0;


        private int peak_to_search = 0;

        private double threshold = 0;

        private int Width;
        private int Height;
        

        private double[,] peak_values = null;       // найденные z-значения пиков

        private double[,] min_values = null;        // текущие минимальные и максимальные найденные z-значения
        private double[,] max_values = null;

        private int[,] n_min_values = null;         // текущие номера кадров, соответсвующие минимума и максимумам
        private int[,] n_max_values = null;

        private bool[,] peak_found = null;          // найден пик, прекращаем поиск для данного XY
        private bool[,] threshold_found = null;     // найдено превышение порога, ищем превышение с обратным знаком

        private int[,] npeak_values = null;         //текущие номера кадров, соответсвующие пикам

        private int[,] peak_counter = null;




        private void ProcessPixelMinimum(double data, int x, int y, int n)
        {
            //if (x == 1327 && y == 1596) Debugger.Break();
            if (peak_found[x,y]) return;

            if (!threshold_found[x,y])
            {
                // До того как найдено превышение порогового значения 
                // идет поиск минимума
                if (data > max_values[x, y])
                {
                    max_values[x, y] = data ;
                }
                if (data < min_values[x, y])
                {
                    min_values[x, y] = data;
                    n_min_values[x, y] = n;
                    // Чтобы исключить срабатывание поиска порога на экстремум-максимум
                    // превышение порога регистрируется только при нахождении нового минимума
                    if (max_values[x, y] > min_values[x, y] + threshold)
                    {
                        threshold_found[x, y] = true;
                        //ищем новый максимум
                        max_values[x, y] = Double.MinValue;

                    }
                }
            }
            else
            {
                if (data > max_values[x, y])
                {
                    max_values[x, y] = data;
                }
                //все равно обновляем минимум - вдруг найдется значение получше
                if (data < min_values[x, y])
                {
                    min_values[x, y] = data;
                    n_min_values[x, y] = n;
                    //если нашли, максимум надо искать заново, т.к. он должен быть справа от минимума
                    max_values[x, y] = Double.MinValue;
                }
                // мы "понимаем" что обнаружили пик только после того, как второй раз прошли порог
                // поэтому в счетчик пиков передаем значение ранее найденного минимального значения
                if (max_values[x, y] > min_values[x, y] + threshold) CountPeak(data,x, y, n_min_values[x,y]);

            }

        }

        private void ProcessPixelMaximum(double data,int x, int y, int n)
        {
            if (peak_found[x, y]) return;

            if (!threshold_found[x, y])
            {
                // До того как найдено превышение порогового значения 
                // идет поиск максимума
                if (data < min_values[x, y])
                {
                    min_values[x, y] = data;
                }
                // Чтобы исключить срабатывание поиска порога на экстремум-минимум
                // превышение порога регистрируется только при нахождении нового максимум
                if (data > max_values[x, y])
                {
                    max_values[x, y] = data;
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
                if (data < min_values[x, y])
                {
                    min_values[x, y] = data;
                }
                //все равно обновляем максимум - вдруг найдется значение получше
                if (data > max_values[x, y])
                {
                    max_values[x, y] = data;
                    n_max_values[x, y] = n;
                    //если нашли, минимум надо искать заново, т.к. он должен быть справа от максимума
                    min_values[x, y] = Double.MaxValue;
                }
                // мы "понимаем" что обнаружили пик только после того, как второй раз прошли порог
                // поэтому в счетчик пиков передаем значение ранее найденного максимального значения
                if (max_values[x, y] > min_values[x, y] + threshold) CountPeak(data,x, y, n_max_values[x,y]);
            }
        }


        //счетчик пиков - так как нам может быть нужен не первый обнаруженный пик
        //при необходимости - регистируем пик и начинаем процесс заново
        private void CountPeak(double data, int x, int y, int n)
        {
            peak_counter[x,y]++;
            if (peak_counter[x, y] >= peak_to_search)
            {
                peak_found[x, y] = true;
                peak_values[x, y] = data;
                npeak_values[x, y] = n;
            }
            else
            {
                min_values[x, y] = Double.MaxValue;
                max_values[x, y] = Double.MinValue;
                threshold_found[x, y] = false;
            }

        }


        public void InitFields()
        {
            peak_values = new double[Width, Height];

            min_values = new double[Width, Height];
            max_values = new double[Width, Height];


            n_min_values = new int[Width, Height];
            n_max_values = new int[Width, Height];

            peak_found = new bool[Width, Height];
            threshold_found = new bool[Width,Height];

            npeak_values = new int[Width,Height];

            peak_counter = new int [Width,Height];  

            for (int x = 0; x < Width; x++) 
                for(int y = 0; y < Height; y++)
                {
                    min_values[x, y] = Double.MaxValue;
                    max_values[x, y] = Double.MinValue;
                    threshold_found[x,y] = false;
                    peak_found[x, y] = false;
                    peak_counter[x, y] = -1;
                }

            

        }





        public int[,] SearchPeak(ImageBatch batch, int n_peak, bool FindMinimum, double threshold, int startPosition)
        {
            
            this.threshold = threshold;
            start_position = startPosition;
            Width = batch.Width; Height = batch.Height; 
            InitFields();
            if (start_position >= batch.Count)
            {
                MessageBox.Show("Wrong index");
                return npeak_values;
            }
                int samplesPerPixel = batch.SamplesPerPixel;


            DataManager_2D.workToBeDone += batch.Height * (batch.Count - startPosition);

            for (int index = start_position; index < batch.Count; index++)
            {
                var tiff_img = Tiff.Open(batch.filenames[index], "r");
                int buffer_size = tiff_img.ScanlineSize();

                Parallel.For(0, Height, (int line) =>
                {
                    byte[] buffer = new byte[buffer_size];
                    lock (tiff_img)
                    {
                        tiff_img.ReadScanline(buffer, line);
                    }
                    
                    ushort[] pixelData = new ushort[Width * samplesPerPixel];
                    if (batch.BitsPerSample == 16)
                        System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    else
                    {
                        for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
                    }

                    for (int pixel = 0; pixel < Width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (FindMinimum) ProcessPixelMinimum(pixel_value, pixel, line, index);
                        else ProcessPixelMaximum(pixel_value, pixel, line, index);

                    }
                    DataManager_2D.progress.Report(1);

                });
                

            }

            return npeak_values;

        }


    }
}
