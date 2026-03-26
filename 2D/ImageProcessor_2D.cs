//#define Multithread

using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using NetTopologySuite.Algorithm;
using ScottPlot;
using ScottPlot.PlotStyles;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static int[,] Amplitude(ImageBatch batch, int step = 1)
        {
            int width = batch.Width;
            int height = batch.Height;
            int samplesPerPixel = batch.SamplesPerPixel;
            int[,] min_values = new int[width, height];
            int[,] max_values = new int[width, height];
            int[,] amplitude_values = new int[width, height];

            try
            {
                DataManager_2D.workToBeDone += batch.Count;
                for (int image_counter = 0; image_counter < batch.Count; image_counter += step)
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
            finally
            {
                Array.Clear(min_values, 0, min_values.Length);
                min_values = null;
                Array.Clear(max_values, 0, max_values.Length);
                max_values = null;
            }


            
        }

        public static double[,] FitData(int[,] data, CoordinateTransformation ct)
        {
            /* double[,] result = new double[ct.frame_width, ct.frame_height];
             if (ct == null) return result;


             for (int j = 0; j < ct.frame_width; j++)
                 for (int i = 0; i < ct.frame_height; i++)
                 {
                     var pw = ct.GeneratePWM_point(new PointF(j, i));
                     double tempvalue = 0;
                     for (int k = 0; k < pw.Count; k++)
                     {
                         tempvalue += pw.weights[k].weight * data[pw.weights[k].x, pw.weights[k].y];
                     }
                     tempvalue *= ct.k_area;
                     result[j, i] = tempvalue;

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
            //await proc_instance.FitImageAsync(hndl, ct);
            //while (!proc_instance.dataReady) { }
            //return proc_instance.dataF;
            //Task.Run(() => return FitImage2(hndl,ct))


            Task<double[,]> task = new Task<double[,]>(() => FitImage2(hndl, ct));
            return await task;
            



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

        public static double[,] FitImage2(TiffImgFileHandler hndl, CoordinateTransformation ct)
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
            {
                ushort[] buffer = hndl.GetLine(i); 
                for (int j = 0; j < buffer.Length; j++)
                    pixelData[j,i] = buffer[j];
            }
                
            float[,] interpolatedData = new float[x_steps, y_steps];



            
            
            Parallel.For(0, x_steps, (i) => 
            {
            double x = ct.point_TL.X + i * xt_x;
            double y = ct.point_TL.Y + i * xt_y;
                for (int j = 0; j < y_steps; j++)
                {
                    interpolatedData[i, j] = BilinearInterpolation(pixelData, x, y);
                    x += yt_x;
                    y += yt_y;
                }
            });
            /*
            double[,] test = new double[interpolatedData.GetLength(0), interpolatedData.GetLength(1)];
            for (int i = 0; i < test.GetLength(0); i++)
                for (int j = 0; j < test.GetLength(1); j++)
                    test[i, j] = interpolatedData[i, j];
            return test;
            */
            StretchAray(interpolatedData, result);

            pixelData = null;
            interpolatedData = null;
            GC.Collect();
            
            return result;
            
        }

        private static void StretchAray(float[,] src, double[,] dest)
        {
 
            // k_dest < 1 typically
            double x_k_dest = (double)dest.GetLength(0) / src.GetLength(0);
            double y_k_dest = (double)dest.GetLength(1) / src.GetLength(1);

            // x_k_src > 1 typically
            double x_k_src = (double)src.GetLength(0) / dest.GetLength(0);
            double y_k_src = (double)src.GetLength(1) / dest.GetLength(1);

            for (int j = 0; j < src.GetLength(1); j++)
            {
                Action<double, int> placeValue = (value, x_to) => { };
                int dest_y_index_start = (int)(j * y_k_dest );
                int dest_y_index_end = (int)((j + 1) * y_k_dest);
                if (dest_y_index_end > dest.GetLength(1) - 1) dest_y_index_end = dest.GetLength(0) - 1;
                if (dest_y_index_start == dest_y_index_end)
                {
                    // start and end point of source array belongs to same line in destination array, no need to split values
                    placeValue = (value, x_to) => { dest[x_to, dest_y_index_start] += value; };
                }
                else
                {
                    // start and end point of source array belongs to different j in destination array
                    // need to split values
                    double nearest_border = y_k_src * dest_y_index_end;
                    //double weight_y = nearest_border - Math.Floor(nearest_border);
                    double weight_y = nearest_border - j;
                    placeValue = (value, x_to) => {
                        dest[x_to, dest_y_index_start] += value * weight_y;
                        dest[x_to, dest_y_index_end] += value * (1-weight_y);
                    };
                }
                
                for (int i = 0; i < src.GetLength(0); i++)
                {
                    
                    int dest_x_index_start = (int)(i * x_k_dest);
                    int dest_x_index_end = (int)((i+1) * x_k_dest );
                    //if (dest_x_index_start == 253) Debugger.Break();

                    if (dest_x_index_end > dest.GetLength(0) - 1) dest_x_index_end = dest.GetLength(0) - 1;
                    if (dest_x_index_start == dest_x_index_end)
                    {
                        placeValue(src[i,j], dest_x_index_start);

                    }
                    else 
                    {
                        double nearest_border = x_k_src * dest_x_index_end;
                        //double weight_x = nearest_border - Math.Floor(nearest_border);
                        double weight_x = nearest_border - i;
                        placeValue(src[i, j] * weight_x, dest_x_index_start);
                        placeValue(src[i, j] * (1-weight_x), dest_x_index_end);

                    }
                }
            }
            double ks = (x_k_dest * y_k_dest);
            for (int i = 0; i < dest.GetLength(0); i++)
            {
                for (int j = 0; j < dest.GetLength(1); j++)
                    dest[i,j] *= ks;
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


        public static double CalculateAverage(IContainer_2D container, int grid_step = 1)
        {
            double sum = 0;
            long count = 0;

            for (int i = 0; i < container.Width; i += grid_step)
                for (int j = 0; j < container.Height; j += grid_step)
                {
                    sum += container.ddata(i,j);
                    count++;
                }

            return count == 0 ? 0 : sum / count;
        }



    }
}
