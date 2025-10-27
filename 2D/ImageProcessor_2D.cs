#define Multithread

using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using NetTopologySuite.Algorithm;
using ScottPlot;
using ScottPlot.PlotStyles;
using System;
using System.Buffers;
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


        protected void PerformCalculationAsync(ICalculation2D calculation)
        {
            dataF = new double[calculation.Width, calculation.Height];



#if Multithread

            Parallel.For(0, calculation.Width,
                (int i) =>
                {
                    for (int j = 0; j < calculation.Height; j++)
                    {
                        dataF[i, j] = calculation.Measure(i, j);
                    }
                }
                );

#else
            for (int i = 0; i < calculation.Width; i++)
                    for (int j = 0; j < calculation.Height; j++)
                    {
                        dataF[i, j] = calculation.Measure(i, j);

                    }
#endif




        }


        public static async Task<double[,]> FitImage(TiffImgFileHandler hndl, CoordinateTransformation ct)
        {

            ImageProcessor_2D proc_instance = new ImageProcessor_2D();
            await proc_instance.FitImageAsync(hndl, ct);
            while (!proc_instance.dataReady) { }
            return proc_instance.dataF;

        }

        protected async Task FitImageAsync(TiffImgFileHandler hndl, CoordinateTransformation ct)
        {
            dataF = new double[ct.frame_width, ct.frame_height];

            await Task.Run(() =>
            {
                DataManager_2D.workToBeDone += ct.frame_width;

                ct.CalculateFullField();

                ushort[,] imgData = new ushort[hndl.Width, hndl.Height];

                for (int j = 0; j < hndl.Height; j++)
                {
                    ushort[] line = hndl.GetLine(j);
                    for (int i = 0; i < hndl.Width; i++)
                    {
                        imgData[i, j] = line[i];
                    }
                }


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
                                        tempvalue += pw.weights[k].weight * imgData[pw.weights[k].x, pw.weights[k].y];
                                    }
                                    tempvalue *= ct.k_area;
                                    dataF[i, j] = tempvalue;

                                }
                                DataManager_2D.progress.Report(1);
                            }
                            );
                }

                imgData = null; 
                GC.Collect();

                dataReady = true;
            });
        }

        protected async Task<double[,]> FitImageAsync2(TiffImgFileHandler hndl, CoordinateTransformation ct)
        {
            double[,] result = new double[ct.frame_width, ct.frame_height];

            if (!ct.Contains(hndl)) throw new Exception("Часть рамки выходит за границы изображения");
           
            double xt_x = (ct.point_TR.X - ct.point_TL.X);
            double xt_y = (ct.point_TR.Y - ct.point_TL.Y);
            double yt_x = (ct.point_BL.X - ct.point_TL.X);
            double yt_y = (ct.point_BL.Y - ct.point_TL.Y);

            double xt_length = Math.Sqrt(Math.Pow(xt_x, 2) + Math.Pow(xt_y, 2));
            double yt_length = Math.Sqrt(Math.Pow(yt_x, 2) + Math.Pow(yt_y, 2));

            xt_x /= xt_length;
            xt_y /= xt_length;
            yt_x /= yt_length;
            yt_y /= yt_length;

            int x_steps = (int)Math.Floor(xt_length);
            int y_steps = (int)Math.Floor(yt_length);

            ushort[,] pixelData = new ushort[hndl.Width,hndl.Height];
            for (int i =0; i<hndl.Height;i++)
                Buffer.BlockCopy(hndl.GetLine(i), 0, pixelData, i * hndl.Width, hndl.Height);

            float[,] shiftedData = new float[x_steps, y_steps];

            
            
            for (int i = 0; i < xt_length; i++) //Parallel this
            {
                double x = ct.point_TL.X + i * yt_x;
                double y = ct.point_TL.Y + i * yt_y;
                for (int j = 0; j < yt_length; j++)
                {
                    shiftedData[i, j] = BilinearInterpolation(pixelData, x, y);
                    x += xt_x;
                    y += xt_y;
                }
            }

            double x_array = 0;
            double y_array = 0;
            double x_array_step = ct.frame_width / x_steps;
            double y_array_step = ct.frame_height / y_steps;

            for (int i = 0; i < pixelData.GetLength(0); i++)
                for (int j = 0; j < pixelData.GetLength(1); j++)
                {
                    double x_array_next = x_array + 1;
                    double y_array_next = y_array + 1;
                    int x_index = (int)x_array/x_steps;
                    int x_index_next = (int)x_array_next/x_steps;
                    int y_index = (int)y_array / y_steps;
                    int y_index_next = (int)y_array_next / y_steps;
                    bool splited_x = x_index == x_index_next;
                    bool splited_y = y_index == y_index_next;   

                    if (!splited_x && !splited_y)
                    {
                        result[x_index,y_index] += pixelData[i,j];
                    }
                    else if (splited_x && !splited_y)
                    {
                        double nearest_border = x_array_step * x_array;
                        double weight_x = nearest_border - Math.Floor(nearest_border);
                        result[x_index, y_index] += pixelData[i, j] * weight_x;
                        result[x_index_next, y_index] += pixelData[i, j] * (1-weight_x);
                    }
                    else if (!splited_x && splited_y)
                    {
                        double nearest_border = y_array_step * y_array;
                        double weight_y = nearest_border - Math.Floor(nearest_border);
                        result[x_index, y_index] += pixelData[i, j] * weight_y;
                        result[x_index, y_index_next] += pixelData[i, j] * (1 - weight_y);
                    }






                }











        }

        private static float BilinearInterpolation(ushort[,] source, double x, double y)
        { 
            int x1 = (int)Math.Floor(x);
            int y1 = (int)Math.Floor(y);
            int x2 = x1 + 1;
            int y2 = y1 + 1;

            double dx = x - x1;
            double dy = y - y1;
            double w1 = (1 - dx) * (1 - dy);
            double w2 = dx * (1 - dy);
            double w3 = (1 - dx) * dy;
            double w4 = dx * dy;


            return (float)(source[x1,y1]*w1 + source[x2,y1]*w2 + source[x2,y1]*w3+source[x2,y2]*w4);
        }

         




    }
}
