using BitMiracle.LibTiff.Classic;
using ImgAnalyzer;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{

    public  class ExtremumSearchAbs
    {
        int start_position = 0;
        bool FindMinimum = false;

        private int peak_to_search = 0;

        private double threshold = 0;

        private int Width;
        private int Height;
        

        private double[,] peak_values = null;
        private int[,] npeak_values = null;





        private void ProcessPixelMinimum(double data, int x, int y, int n)
        {
            if (data < peak_values[x, y])
            {
                peak_values[x, y] = data;
                npeak_values[x, y] = n;
            }


        }

        private void ProcessPixelMaximum(double data,int x, int y, int n)
        {
            if (data > peak_values[x, y])
            {
                peak_values[x, y] = data;
                npeak_values[x, y] = n;
            }
        }






        public void InitFields()
        {
            peak_values = new double[Width, Height];
            npeak_values = new int[Width,Height];

            for (int x = 0; x < Width; x++) 
                for(int y = 0; y < Height; y++)
                {
                    if (FindMinimum) peak_values[x, y] = Double.MaxValue;
                    else peak_values[x, y] = Double.MinValue;
                    npeak_values[x,y] = 0;
                }

          
        }





        public int[,] SearchPeak(ImageBatch batch,  bool FindMinimum, int startPosition)
        {
            
            this.FindMinimum = FindMinimum;
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
