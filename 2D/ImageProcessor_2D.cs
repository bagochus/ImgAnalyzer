using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using ScottPlot;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{

    public class ImageProcessor_2D
    {
        protected double[,] dataF = new double[0, 0];
        public bool DataReady { get { return dataReady; } }
        private bool dataReady = false;


        public static int[,] MinInt(ImageBatch batch)
        {
            int width = batch.Width;
            int height = batch.Height;
            int samplesPerPixel = batch.SamplesPerPixel;


            int[,] min_values = new int[width, height];
            DataManager_2D.workToBeDone += batch.Count;
            for (int image_counter = 0; image_counter < batch.Count; image_counter++)
            {
                var tiff_img = Tiff.Open(batch.filenames[image_counter], "r");
                for (int line = 0; line < height; line++)
                {

                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    if (batch.BitsPerSample == 16)
                        System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    else
                    {
                        for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
                    }
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (image_counter == 0)
                        {
                            min_values[pixel, line] = pixel_value;
                        }
                        else
                        {
                            if (pixel_value < min_values[pixel, line]) min_values[pixel, line] = pixel_value;
                        }
                    }
                }
                DataManager_2D.progress.Report(1);
            }
            return min_values;
        }

        public static int[,] MaxInt(ImageBatch batch)
        {
            int width = batch.Width;
            int height = batch.Height;
            int samplesPerPixel = batch.SamplesPerPixel;


            int[,] max_values = new int[width, height];
            DataManager_2D.workToBeDone += batch.Count;
            for (int image_counter = 0; image_counter < batch.Count; image_counter++)
            {
                var tiff_img = Tiff.Open(batch.filenames[image_counter], "r");
                for (int line = 0; line < height; line++)
                {



                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    if (batch.BitsPerSample == 16)
                        System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    else
                    {
                        for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
                    }
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (image_counter == 16)
                        {
                            max_values[pixel, line] = pixel_value;
                        }
                        else
                        {
                            if (pixel_value > max_values[pixel, line]) max_values[pixel, line] = pixel_value;
                        }
                    }
                }
                DataManager_2D.progress.Report(1);
            }
            return max_values;
        }

        public static int[,] Amplitude(ImageBatch batch)
        {
            int width = batch.Width;
            int height = batch.Height;
            int samplesPerPixel = batch.SamplesPerPixel;
            int[,] min_values = new int[width, height];
            int[,] max_values = new int[width, height];
            int[,] amplitude_values = new int[width, height];

            DataManager_2D.workToBeDone += batch.Count;
            for (int image_counter = 0; image_counter < batch.Count; image_counter++)
            {
                var tiff_img = Tiff.Open(batch.filenames[image_counter], "r");
                for (int line = 0; line < height; line++)
                {



                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    if (batch.BitsPerSample == 16)
                        System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    else
                    {
                        for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
                    }
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (image_counter == 0)
                        {
                            min_values[pixel, line] = pixel_value;
                            max_values[pixel, line] = pixel_value;
                        }
                        else
                        {
                            if (pixel_value < min_values[pixel, line]) min_values[pixel, line] = pixel_value;
                            if (pixel_value > max_values[pixel, line]) max_values[pixel, line] = pixel_value;
                        }
                    }
                }
                DataManager_2D.progress.Report(1);
            }

            for (int i = 0; i < min_values.GetLength(0); i++)
                for (int j = 0; j < min_values.GetLength(1); j++)
                {
                    amplitude_values[i, j] = (ushort)(max_values[i, j] - min_values[i, j]);
                }
            return amplitude_values;
        }

        public static double[,] FitData(int[,] data, CoordinateTransformation ct)
        {
            /* double[,] result = new double[ct.frame_width, ct.frame_height];
             if (ct == null) return result;


             for (int i = 0; i < ct.frame_width; i++)
                 for (int j = 0; j < ct.frame_height; j++)
                 {
                     var pw = ct.GeneratePWM_point(new PointF(i, j));
                     double tempvalue = 0;
                     for (int k = 0; k < pw.Count; k++)
                     {
                         tempvalue += pw.weights[k].weight * data[pw.weights[k].x, pw.weights[k].y];
                     }
                     tempvalue *= ct.k_area;
                     result[i, j] = tempvalue;

                 }*/

            ImageProcessor_2D proc_instance = new ImageProcessor_2D();
            proc_instance.FitDataAsync(data, ct);
            while (!proc_instance.dataReady) { }
            return proc_instance.dataF;

        }

        public static double[,] FitDataDouble(double[,] data, CoordinateTransformation ct)
        {

            ImageProcessor_2D proc_instance = new ImageProcessor_2D();
            proc_instance.FitDataDoubleAsync(data, ct);
            while (!proc_instance.dataReady) { }
            return proc_instance.dataF;

        }



        protected async void FitDataAsync(int[,] data, CoordinateTransformation ct)
        {
            dataF = new double[ct.frame_width, ct.frame_height];
            ct.CalculateFullField();

            await Task.Run(() =>
            {
                DataManager_2D.workToBeDone += ct.frame_width;
                lock (dataF)
                {
                    while (!ct.FullFieldCalculated) { }
                    Parallel.For(0, ct.frame_width,
                        (int i) =>
                        {

                            for (int j = 0; j < ct.frame_height; j++)
                            {
                                var pw = ct.FullFiedTransformation[i, j];
                                double tempvalue = 0;
                                for (int k = 0; k < pw.Count; k++)
                                {
                                    tempvalue += pw.weights[k].weight * data[pw.weights[k].x, pw.weights[k].y];
                                }
                                tempvalue *= ct.k_area;
                                dataF[i, j] = tempvalue;

                            }
                            DataManager_2D.progress.Report(1);
                        }
                        );
                }
                dataReady = true;
            });
        }

        protected async void FitDataDoubleAsync(double[,] data, CoordinateTransformation ct)
        {
            dataF = new double[ct.frame_width, ct.frame_height];
            ct.CalculateFullField();

            await Task.Run(() =>
            {
                DataManager_2D.workToBeDone += ct.frame_width;
                lock (dataF)
                {
                    while (!ct.FullFieldCalculated) { }
                    Parallel.For(0, ct.frame_width,
                        (int i) =>
                        {

                            for (int j = 0; j < ct.frame_height; j++)
                            {
                                var pw = ct.FullFiedTransformation[i, j];
                                double tempvalue = 0;
                                for (int k = 0; k < pw.Count; k++)
                                {
                                    tempvalue += pw.weights[k].weight * data[pw.weights[k].x, pw.weights[k].y];
                                }
                                tempvalue *= ct.k_area;
                                dataF[i, j] = tempvalue;

                            }
                            DataManager_2D.progress.Report(1);
                        }
                        );
                }
                dataReady = true;
            });
        }


        public static int[,] Index(ImageBatch batch, int index)
        {
            int width = batch.Width;
            int height = batch.Height;
            int samplesPerPixel = batch.SamplesPerPixel;


            int[,] values = new int[width, height];
            DataManager_2D.workToBeDone += batch.Count;

            var tiff_img = Tiff.Open(batch.filenames[index], "r");
            for (int line = 0; line < height; line++)
            {
                byte[] buffer = new byte[tiff_img.ScanlineSize()];
                tiff_img.ReadScanline(buffer, line);
                ushort[] pixelData = new ushort[width * samplesPerPixel];
                if (batch.BitsPerSample == 16)
                    System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                else
                {
                    for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
                }
                for (int pixel = 0; pixel < width; pixel++)
                {
                    ushort pixel_value = pixelData[pixel];
                    values[pixel, line] = pixel_value;
                }
            }
            DataManager_2D.progress.Report(1);

            return values;



        }



        public static double[,] PerformCalculation(ICalculation2D calculation)
        {

            ImageProcessor_2D proc_instance = new ImageProcessor_2D();
            proc_instance.PerformCalculationAsync(calculation);
            return proc_instance.dataF;

        }


        protected async void PerformCalculationAsync(ICalculation2D calculation)
        {
            dataF = new double[calculation.Width, calculation.Height];

            await Task.Run(() =>
            {
                lock (dataF)
                {
                    Parallel.For(0, calculation.Width,
                        (int i) =>
                        {
                            for (int j = 0; j < calculation.Height; j++)
                            {
                                dataF[i, j] = calculation.Measure(i, j);
                            }
                        }
                        );
                }
            });




        }


    }
}
