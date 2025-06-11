using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using ImgAnalyzer.MeasurmentTypes;
using ScottPlot.Plottables;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using ScottPlot.PlotStyles;
using ScottPlot.Interactivity.UserActions;
using System.Reflection;
using System.Diagnostics;

namespace ImgAnalyzer
{
    public enum State : byte { Rising, Falling, Unknown };

    public enum NearestExtremum:byte {A,B,Undef };
    public enum Vert : int { TopLeft = 0, TopRight, BottomRight, BottomLeft  };
    
    


    delegate void ListUpdatedDelegate();
    delegate void LoadFileListDeleggate(string[] filenames);
    public partial class MainPresenter
    {
        public Image image;
        public Tiff tiff_img;


        public string[] filenames;


        public Point[] corners;
        public int x_sectors;
        public int y_sectors;

        public List<IMeasurment> measurements = new List<IMeasurment>();
        public EventHandler ListUpdated;

        // usage of indexes - [y,x]
        private int[,] max_values;
        private int[,] min_values;
        private int[,] amplitude_values;
        private int[,] good_values;
        private int[,] pseudo_phase_values;
        private int[,] a_values;
        private int[,] b_values;
        private List<int[,]> ax_positions = new List<int[,]>();
        private List<int[,]> ay_positions = new List<int[,]>();
        private int[,] a_peaks;
        private List<int[,]> bx_positions = new List<int[,]>();
        private List<int[,]> by_positions = new List<int[,]>();
        private int[,] b_peaks;

        //test CT
        public CoordinateTransformation ct;


        public void LocateImageBatch(int batch_index, string[] filenames)
        {
            ImageManager.Batch(batch_index).LocateImageBatch(filenames);
        }








        public void LoadFileList(string[] filenames)
        {
            this.filenames = filenames;
        }

        public void LoadImage(string filename)
        {
            image = Image.FromFile(filename);
            tiff_img = Tiff.Open(filename,"r");
        }

        public double ConvertIntensityToPhase(int x, int y, int n_frame, int intesity)
        {
            if (a_values ==  null) return double.NaN;

            int a = a_values[y,x];
            int b = b_values[y,x];
            int n_period=-1;

            int nearestValueMinus = 0;
            NearestExtremum ne_minus = NearestExtremum.Undef;
            int nearestValuePlus = Int32.MaxValue;
            NearestExtremum ne_plus = NearestExtremum.Undef;
            int nma = -1;
            int npa = -1;
            int nmb = -1;
            int npb = -1;





            for (int i =0; i < ax_positions.Count;i++)
            {
                 //if (n_frame == 300) Debugger.Break();
                if (n_frame > ax_positions[i][y, x] && ax_positions[i][y, x] !=0)
                {
                    n_period = i;
                    //break;
                }
            }

            for (int i = 0; i < ax_positions.Count; i++)
            {
                if (ax_positions[i][y, x] > nearestValueMinus &&
                    ax_positions[i][y, x] < n_frame &&
                    ax_positions[i][y, x] != 0)
                {
                    nearestValueMinus = ax_positions[i][y, x];
                    ne_minus = NearestExtremum.A;
                    nma = i;
                }

                if (ax_positions[i][y, x] < nearestValuePlus &&
                    ax_positions[i][y, x] > n_frame &&
                    ax_positions[i][y, x] != 0)
                {
                    nearestValuePlus = ax_positions[i][y, x];
                    ne_plus = NearestExtremum.A;
                    npa = i;
                }
            }

            for (int i = 0; i < bx_positions.Count; i++)
            {
                if (bx_positions[i][y, x] > nearestValueMinus &&
                    bx_positions[i][y, x] < n_frame &&
                    bx_positions[i][y, x] != 0)
                {
                    nearestValueMinus = bx_positions[i][y, x];
                    ne_minus = NearestExtremum.B;
                    nmb = i;
                }

                if (bx_positions[i][y, x] < nearestValuePlus &&
                    bx_positions[i][y, x] > n_frame &&
                    bx_positions[i][y, x] != 0)
                {
                    nearestValuePlus = bx_positions[i][y, x];
                    ne_plus = NearestExtremum.B;
                    npb = i;
                }
            }




            //undefiened behaviour
            if (ne_minus == ne_plus && (ne_minus != NearestExtremum.Undef)) return double.NaN;
            bool ascending_region = (ne_minus == NearestExtremum.B || ne_plus == NearestExtremum.A);


            if (ne_plus == NearestExtremum.A) a = ay_positions[npa][y, x];
            if (ne_plus == NearestExtremum.B) b = by_positions[npb][y, x];
            if (ne_minus == NearestExtremum.A) a = ay_positions[nma][y, x];
            if (ne_minus == NearestExtremum.B) b = by_positions[nmb][y, x];


            double center_point = b + 0.5 * (a - b);
            double amplitude = 0.5 * (a - b);



            double acos_argument = (intesity - center_point) / amplitude;
            if (acos_argument > 1) acos_argument = 1;
            if (acos_argument < -1) acos_argument = -1;
            double phase_value;

            if (ascending_region) phase_value = 2 * Math.PI - Math.Acos(acos_argument);
            else phase_value = Math.Acos(acos_argument);

            return phase_value + 2 * Math.PI*n_period;    
        }


        public static bool IsPointInPolygonOptimized(PointF point, PointF[] polygon, RectangleF bounds)
        {
            if (!bounds.Contains(point))
                return false;

            return IsPointInPolygon(point, polygon);
        }

        public static bool IsPointInPolygon(PointF point, PointF[] polygon)
        {
            if (polygon == null || polygon.Length < 3)
                throw new ArgumentException("Polygon must have at least 3 ovelay_points");

            bool inside = false;
            int n = polygon.Length;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X)
                    * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public double MeasurePolygon(Point[] points)
        {
            return MeanValueStrictBorders(points);
        }

        private double MeanValueStrictBorders(Point[] points)
        {
            PointF[] pointsF = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointsF[i] = new PointF(points[i].X, points[i].Y);
            }
            int max_x = 0;
            int min_x = int.MaxValue;
            int max_y = 0;
            int min_y = int.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                if (points[i].X > max_x ) max_x = points[i].X;
                if (points[i].Y > max_y) max_y = points[i].Y;
                if (points[i].X < min_x) min_x = points[i].X;
                if (points[i].Y < min_y) min_y = points[i].Y;

            }
            //int size_x = max_x - min_x;
            //int size_y = max_y - min_y;
            //RectangleF borders = new RectangleF(min_x,min_y,size_x,size_y);
            int pixels_counted = 0;
            Int64 value_counted = 0;

            for (int i = min_x; i < max_x; i++)
            {
                for (int j = min_y; j < max_y; j++)
                {
                    if (IsPointInPolygon(new PointF(i, j), pointsF))
                    {     
                        pixels_counted++;
                        value_counted += MeasurePixel(i, j);
                    }
                }
            }
            return value_counted/pixels_counted;
        }

        public double MeasurePoint(Point point)
        {
            return MeasurePixel(point.X, point.Y);
        }

        public int MeasurePixel(int x,int y)
        {
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, y);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            return pixelData[x];
        }

        public int MeasureLine(int line, int start_index, int count)
        {
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, line);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            int result = 0;
            for (int i = start_index; i < start_index+count; i++)
            {
                result += pixelData[i];
            }

            return result;
        }

        public void BatchMeasurment()
        {

            //for (int i =0; i< measurements.Count;i++) measurements[i].ClearData();

            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                for (int j = 0; j < measurements.Count; j++)
                {
                   // measurements[j].Measure();
                }
            }
            return;
        }

        public void BatchPhaseMeasurment()
        {
            

           // for (int i = 0; i < measurements.Count; i++) measurements[i].ClearData();

            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                for (int j = 0; j < measurements.Count; j++)
                {
            //        measurements[j].MeasurePhase(i);
                }
            }
            return;
        }


        public void FindMinMax()
        {
            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

                if (i==0)
                {
                    min_values = new int[height,width];
                    max_values = new int[height, width];
                    amplitude_values = new int[height, width];
                }
                for (int line = 0; line<height; line++)
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (i == 0)
                        {
                            min_values[line, pixel] = pixel_value;
                            max_values[line, pixel] = pixel_value;
                        } else
                        {
                            if (pixel_value < min_values[line,pixel]) min_values[line,pixel] = pixel_value;
                            if (pixel_value > max_values[line,pixel]) max_values[line,pixel] = pixel_value;
                        }
                    }
                }
            }
            
            for (int i = 0; i < min_values.GetLength(0);i++)
                for (int j = 0; j < min_values.GetLength(1); j++)
                {
                    amplitude_values[i,j] = (ushort)(max_values[i,j] - min_values[i,j]);
                }

        }

        public void CalculatePseudoPhase(int upper_threshold, int lower_threshold, int max_peak, int min_peak)
        {
            
            State[,] previous_state = new State[0,0];
            
            int[,] initial_values = new int[0,0];
            //ushort[,] pseudeophase_values = new ushort[0,0]; 

            for (int i = 0; i < filenames.Length; i++)
            {
                tiff_img = Tiff.Open(filenames[i], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();



                if (i == 0)
                {
                    previous_state = new State[height, width];             
                    initial_values = new int[height, width]; 
                    pseudo_phase_values = new int[height, width];
                }
                for (int line = 0; line < height; line++)
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel]; 
                        if (i == 0)
                        {
                            initial_values[line, pixel] = pixel_value;
                            pseudo_phase_values[line, pixel] = 0;

                            if (pixel_value < lower_threshold)
                            {
                                previous_state[line, pixel] = State.Rising;
                            }
                            else if (pixel_value > upper_threshold)
                            {
                                previous_state[line, pixel] = State.Falling;
                            }
                            else
                            {
                                previous_state[line, pixel] = State.Unknown;
                            }
                        }
                        else
                        {
                            State state = State.Unknown;
                            if (pixel_value < lower_threshold) state = State.Rising;
                            if (pixel_value > upper_threshold) state = State.Falling;

                            if (state == State.Rising && previous_state[line,pixel] == State.Falling )
                            {
                                pseudo_phase_values[line, pixel] += 180;
                                previous_state[line,pixel ] = State.Rising; 
                            }
                            else if (state == State.Falling && previous_state[line,pixel] == State.Rising)
                            {
                                pseudo_phase_values[line, pixel] += 180;
                                previous_state[line, pixel] = State.Falling;
                            }
                            else if (state == State.Rising && previous_state[line,pixel] == State.Unknown)
                            {
                                pseudo_phase_values[line,pixel] += (initial_values[line, pixel] - min_peak) * 180/ (max_peak - min_peak);
                                previous_state[line,pixel] = State.Rising;
                            }
                            else if (state == State.Falling && previous_state[line,pixel] == State.Unknown)
                            {
                                pseudo_phase_values[line,pixel] += (max_peak - initial_values[line,pixel]) * 180 / (max_peak - min_peak);
                                previous_state[line, pixel] = State.Falling;
                            }

                            if (i == filenames.Length-1)
                            {

                                if (previous_state[line,pixel] == State.Unknown)
                                    pseudo_phase_values[line, pixel] = Math.Abs(pixel_value - initial_values[line,pixel] ) * 180 / (max_peak - min_peak);
                                if (previous_state[line, pixel] == State.Rising && state == State.Unknown)
                                    pseudo_phase_values[line, pixel] += (pixel_value - min_peak) * 180 / (max_peak - min_peak);
                                if (previous_state[line,pixel] == State.Falling && state == State.Unknown)
                                    pseudo_phase_values[line, pixel] += (pixel_value - min_peak) * 180 / (max_peak - min_peak);
                            }
                        }
                    }
                }
            }
        }

        public void MarkDeadPixelByAmplitude(int threshold)
        {
            if (amplitude_values == null) return;
            if (amplitude_values.Length == 0) return;
            good_values = new int[amplitude_values.GetLength(0),amplitude_values.GetLength(1)];


            for (int i = 0; i < amplitude_values.GetLength(0); i++)
                for (int j = 0; j < amplitude_values.GetLength(1); j++)
                {
                    if (amplitude_values[i, j] > threshold) good_values[i, j] = 1;
                    else good_values[i, j] = 0;
                }
        }

        public void MarkDeadPixelByPseudophase(int threshold)
        {
            if (pseudo_phase_values == null) return;
            if (pseudo_phase_values.Length == 0) return;
            good_values = new int[pseudo_phase_values.GetLength(0), pseudo_phase_values.GetLength(1)];


            for (int i = 0; i < pseudo_phase_values.GetLength(0); i++)
                for (int j = 0; j < pseudo_phase_values.GetLength(1); j++)
                {
                    if (pseudo_phase_values[i, j] > threshold) good_values[i, j] = 1;
                    else good_values[i, j] = 0;
                }
        }
        
        private int[,] AvgByBlockAvoidDeadPixels(int image_counter, int block_size)
        {
            Func<int, int> pix_to_block = x => x / block_size;
            Func<int, int> block_to_pix = x => x * block_size;

            int[,] result = new int[0,0];
            Tiff tiff_img = Tiff.Open(filenames[image_counter], "r");
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();
            int[,] block_sums = new int[0, 0];
            int[,] block_counters = new int[0, 0];

            result = new int[height / block_size, width / block_size];
            block_sums = new int[height / block_size, width / block_size];
            block_counters = new int[height / block_size, width / block_size];

            for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                {
                    block_counters[ii, jj] = 0;
                    block_sums[ii, jj] = 0;
                }

            for (int line = 0; line < height; line++)
            {
                byte[] buffer = new byte[tiff_img.ScanlineSize()];
                tiff_img.ReadScanline(buffer, line);
                ushort[] pixelData = new ushort[width * samplesPerPixel];
                System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                for (int pixel = 0; pixel < width; pixel++)
                {
                    ushort pixel_value = pixelData[pixel];
                    if (good_values[line, pixel] == 1)
                    {
                        block_sums[pix_to_block(line), pix_to_block(pixel)] += pixel_value;
                        block_counters[pix_to_block(line), pix_to_block(pixel)]++;
                    }
                }
            }

            for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                {
                    int block_value = 0;
                    if (block_counters[ii, jj] != 0)
                    { 
                        block_value = block_sums[ii, jj] / block_counters[ii, jj];
                    }
                    result[ii,jj] = block_value;
                }

            return result;
        }

        public void CalculateABByBlocks(int block_size)
        {
            if (good_values == null)
            {
                MessageBox.Show("Не отмечены битые пиксели");
                return;
            }
            //A - maximum
            //B - minimum

            Func<int, int> pix_to_block = x => x / block_size;
            Func<int, int> block_to_pix = x => x * block_size;

            int[,] block_sums = new int[0, 0];
            int[,] block_counters = new int[0, 0];
            int[,] a_block_values = new int[0, 0];
            int[,] b_block_values = new int[0, 0];


            for (int image_counter = 0; image_counter < filenames.Length; image_counter++)
            {
                tiff_img = Tiff.Open(filenames[image_counter], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

                if (image_counter == 0)
                {
                    a_values = new int[height, width];
                    b_values = new int[height, width];

                    block_sums = new int[height / block_size, width / block_size];
                    block_counters = new int[height / block_size, width / block_size];

                    a_block_values = new int[height / block_size, width / block_size];
                    b_block_values = new int[height / block_size, width / block_size];


                    for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                        for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                        {
                            a_block_values[ii, jj] = Int32.MinValue;
                            b_block_values[ii, jj] = Int32.MaxValue;
                        }

                }

                for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                    for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                    {
                        block_counters[ii, jj] = 0;
                        block_sums[ii, jj] = 0;
                    }


                for (int line = 0; line < height; line++)
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (good_values[line, pixel] == 1)
                        {
                            block_sums[pix_to_block(line), pix_to_block(pixel)] += pixel_value;
                            block_counters[pix_to_block(line), pix_to_block(pixel)]++;
                        }
                    }
                }

                

                for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                    for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                    {
                        if (block_counters[ii, jj] == 0) continue;
                        int block_value = block_sums[ii, jj] / block_counters[ii, jj];
                        

                        if (block_value > a_block_values[ii, jj]) a_block_values[ii, jj] = block_value;
                        if (block_value < b_block_values[ii, jj]) b_block_values[ii, jj] = block_value;
                    }



            }

            for (int i = 0; i < a_values.GetLength(0); i++)
                for (int j = 0; j < b_values.GetLength(1); j++)
                {
                    if (a_block_values[pix_to_block(i), pix_to_block(j)] == Int32.MinValue) a_values[i, j] = 0;
                    else a_values[i, j] = a_block_values[pix_to_block(i), pix_to_block(j)];

                    if (b_block_values[pix_to_block(i), pix_to_block(j)] == Int32.MaxValue) b_values[i, j] = 0;
                    else b_values[i, j] = b_block_values[pix_to_block(i), pix_to_block(j)];

                }

        }

        public void CalculateABPosByBlock(int tolerance, int block_size)
        {
            const int min_window_size = 5;
            if (a_values == null)
            {
                MessageBox.Show("Не рассчитаны значения A/B");
                return;
            }

            int currernt_a_peak = 0;
            int currernt_b_peak = 0;
            ax_positions.Clear();
            bx_positions.Clear();

            ay_positions.Clear();
            by_positions.Clear();


            Func<int, int> pix_to_block = x => x / block_size;
            Func<int, int> block_to_pix = x => x * block_size;

            int[,] block_sums = new int[0, 0];
            int[,] block_counters = new int[0, 0];


            bool[,] near_a_block = new bool[0, 0];
            int[,] current_vmax_block = new int[0, 0];
            int[,] a_peaks_block = new int[0, 0];
            int [,] a_window_size_block = new int[0, 0];
            List<int[,]> ax_pos_block = new List<int[,]>();
            List<int[,]> ay_pos_block = new List<int[,]>();

            bool[,] near_b_block = new bool[0, 0];
            int[,] current_vmin_block = new int[0, 0];
            int[,] b_peaks_block = new int[0, 0];
            int[,] b_window_size_block = new int[0, 0];
            List<int[,]> bx_pos_block = new List<int[,]>();
            List<int[,]> by_pos_block = new List<int[,]>();




            for (int image_counter = 0; image_counter < filenames.Length; image_counter++)
            {
                tiff_img = Tiff.Open(filenames[image_counter], "r");
                int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
                int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
                int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

                // initializig local arrays
                if (image_counter == 0) 
                {

                    block_sums = new int[height / block_size, width / block_size];
                    block_counters = new int[height / block_size, width / block_size];

                    near_a_block = new bool[height / block_size, width / block_size];
                    current_vmax_block = new int[height / block_size, width / block_size];
                    ax_pos_block.Add(new int[height / block_size, width / block_size]);
                    ay_pos_block.Add(new int[height / block_size, width / block_size]);
                    a_peaks_block = new int[height / block_size, width / block_size];
                    a_window_size_block = new int[height / block_size, width / block_size];
                    a_peaks = new int[height, width];

                    near_b_block = new bool[height / block_size, width / block_size];
                    current_vmin_block = new int[height / block_size, width / block_size];
                    bx_pos_block.Add(new int[height / block_size, width / block_size]);
                    by_pos_block.Add(new int[height / block_size, width / block_size]);
                    b_peaks_block = new int[height / block_size, width / block_size];
                    b_window_size_block = new int[height / block_size, width / block_size];
                    b_peaks = new int[height, width];


                    for (int ii = 0; ii < block_sums.GetLength(0); ii++)            //cleaning and filling function-related arrays
                        for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                        {
                            current_vmax_block[ii, jj] = Int32.MinValue;
                            current_vmin_block[ii, jj] = Int32.MaxValue;

                            near_a_block[ii, jj] = false;
                            ax_pos_block[0][ii, jj] = 0;
                            ay_pos_block[0][ii, jj] = 0;
                            a_peaks_block[ii,jj] = 0;
                            a_window_size_block[ii,jj] = 0;

                            near_b_block[ii, jj] = false;
                            bx_pos_block[0][ii, jj] = 0;
                            by_pos_block[0][ii, jj] = 0;
                            b_peaks_block[ii, jj] = 0;
                            b_window_size_block[ii, jj] = 0;
                        }


                }
                //cleaning and filling frame-related arrays
                for (int ii = 0; ii < block_sums.GetLength(0); ii++)        
                    for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                    {
                        block_counters[ii, jj] = 0;
                        block_sums[ii, jj] = 0;
                    }

                //calculating block's mean-values
                for (int line = 0; line < height; line++)           
                {
                    byte[] buffer = new byte[tiff_img.ScanlineSize()];
                    tiff_img.ReadScanline(buffer, line);
                    ushort[] pixelData = new ushort[width * samplesPerPixel];
                    System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
                    for (int pixel = 0; pixel < width; pixel++)
                    {
                        ushort pixel_value = pixelData[pixel];
                        if (good_values[line, pixel] == 1)
                        {
                            block_sums[pix_to_block(line), pix_to_block(pixel)] += pixel_value;
                            block_counters[pix_to_block(line), pix_to_block(pixel)]++;
                        }
                    }
                }

                //analyzing blocks in current frame - finding A positions
                for (int ii = 0; ii < block_sums.GetLength(0); ii++)            
                    for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                    {
                        if (block_counters[ii, jj] == 0) continue;
                        int block_value = block_sums[ii, jj] / block_counters[ii, jj];

                        bool in_tolerance = (block_value > (a_values[block_to_pix(ii), block_to_pix(jj)] - tolerance));

                        if (in_tolerance && !near_a_block[ii, jj])          //entering range
                        {
                            near_a_block[ii, jj] = true;
                            a_window_size_block[ii, jj] = 1;
                            current_vmax_block[ii, jj] = block_value;
                        } else if (in_tolerance && near_a_block[ii, jj])   // continuing range
                        {
                            a_window_size_block[ii, jj]++;
                            if(block_value > current_vmax_block[ii,jj])
                            {
                                current_vmax_block[ii, jj] = block_value;
                            }
                        } else if ( !in_tolerance && near_a_block[ii, jj] && a_window_size_block[ii, jj] > min_window_size) //leaving range with peak registration
                        {
                            
                            ax_pos_block[a_peaks_block[ii, jj]][ii,jj] = image_counter;
                            ay_pos_block[a_peaks_block[ii, jj]][ii, jj] = current_vmax_block[ii, jj];
                            a_peaks_block[ii, jj]++;
                            current_vmax_block[ii, jj] = Int32.MinValue;
                            a_window_size_block[ii, jj] = 0;
                            near_a_block[ii, jj] = false;
                            if (a_peaks_block[ii,jj] > currernt_a_peak)
                            {
                                currernt_a_peak++;
                                ax_pos_block.Add(new int[height / block_size, width / block_size]);
                                ay_pos_block.Add(new int[height / block_size, width / block_size]);
                                for (int iii= 0;  iii < a_peaks_block.GetLength(0); iii++)
                                    for (int jjj = 0; jjj< a_peaks_block.GetLength (1); jjj++)
                                    {
                                        ax_pos_block[currernt_a_peak][iii, jjj] = 0;
                                        ay_pos_block[currernt_a_peak][iii, jjj] = 0;
                                    }
                            }
                        } else if (!in_tolerance && near_a_block[ii, jj])   //leaving range w/o peak registration
                        {
                            near_a_block[ii, jj] = false;
                            current_vmax_block[ii, jj] = Int32.MinValue;
                            a_window_size_block[ii, jj] = 0;
                        }
                    }


                //analyzing blocks in current frame - finding B positions
                for (int ii = 0; ii < block_sums.GetLength(0); ii++)
                    for (int jj = 0; jj < block_sums.GetLength(1); jj++)
                    {
                        if (block_counters[ii, jj] == 0) continue;
                        int block_value = block_sums[ii, jj] / block_counters[ii, jj];

                        bool in_tolerance = (block_value < (b_values[block_to_pix(ii), block_to_pix(jj)] + tolerance));

                        if (in_tolerance && !near_b_block[ii, jj])          //entering range
                        {
                            near_b_block[ii, jj] = true;
                            b_window_size_block[ii, jj] = 1;
                            current_vmin_block[ii, jj] = block_value;
                        }
                        else if (in_tolerance && near_b_block[ii, jj])   // continuing range
                        {
                            b_window_size_block[ii, jj]++;
                            if (block_value < current_vmin_block[ii, jj])
                            {
                                current_vmin_block[ii, jj] = block_value;
                            }
                        }
                        else if (!in_tolerance && near_b_block[ii, jj] && b_window_size_block[ii, jj] > min_window_size) //leaving range with peak registration
                        {

                            bx_pos_block[b_peaks_block[ii, jj]][ii, jj] = image_counter;
                            by_pos_block[b_peaks_block[ii, jj]][ii, jj] = current_vmin_block[ii, jj];
                            b_peaks_block[ii, jj]++;
                            current_vmin_block[ii, jj] = Int32.MaxValue;
                            b_window_size_block[ii, jj] = 0;
                            near_b_block[ii, jj] = false;
                            if (b_peaks_block[ii, jj] > currernt_b_peak)
                            {
                                currernt_b_peak++;
                                bx_pos_block.Add(new int[height / block_size, width / block_size]);
                                by_pos_block.Add(new int[height / block_size, width / block_size]);
                                for (int iii = 0; iii < b_peaks_block.GetLength(0); iii++)
                                    for (int jjj = 0; jjj < b_peaks_block.GetLength(1); jjj++)
                                    {
                                        bx_pos_block[currernt_b_peak][iii, jjj] = 0;
                                        by_pos_block[currernt_b_peak][iii, jjj] = 0;
                                    }
                            }
                        }
                        else if (!in_tolerance && near_b_block[ii, jj])   //leaving range w/o peak registration
                        {
                            current_vmin_block[ii, jj] = Int32.MaxValue;
                            b_window_size_block[ii, jj] = 0;
                        }
                    }


            } // all images browsed

            // laying out block-variables to pixel-variables

            for (int n = 0; n < ax_pos_block.Count; n++)
            {
                ax_positions.Add(new int[a_values.GetLength(0), a_values.GetLength(1)]);
                ay_positions.Add(new int[a_values.GetLength(0), a_values.GetLength(1)]);
                for (int i = 0; i < a_values.GetLength(0);i++)
                    for (int j = 0; j < a_values.GetLength(1);j++)
                    {
                        ax_positions[n][i, j] =
                            ax_pos_block[n][pix_to_block(i),pix_to_block(j)];
                        ay_positions[n][i, j] =
                            ay_pos_block[n][pix_to_block(i), pix_to_block(j)];
                    }

            }

            for (int i = 0; i < a_values.GetLength(0); i++)
                for (int j = 0; j < a_values.GetLength(1); j++)
                {
                    a_peaks[i,j] = a_peaks_block[pix_to_block(i),pix_to_block(j)];
                }

            for (int n = 0; n < bx_pos_block.Count; n++)
            {
                bx_positions.Add(new int[b_values.GetLength(0), b_values.GetLength(1)]);
                by_positions.Add(new int[b_values.GetLength(0), b_values.GetLength(1)]);
                for (int i = 0; i < b_values.GetLength(0); i++)
                    for (int j = 0; j < b_values.GetLength(1); j++)
                    {
                        bx_positions[n][i, j] =
                            bx_pos_block[n][pix_to_block(i), pix_to_block(j)];
                        by_positions[n][i, j] =
                            by_pos_block[n][pix_to_block(i), pix_to_block(j)];
                    }

            }

            for (int i = 0; i < b_values.GetLength(0); i++)
                for (int j = 0; j < b_values.GetLength(1); j++)
                {
                    b_peaks[i, j] = b_peaks_block[pix_to_block(i), pix_to_block(j)];
                }



        } // function end

        public double[,] StretchData(int[,] data)
        {
            double[,] result = new double[ct.frame_width, ct.frame_height];
            npixels = new int[ct.frame_width, ct.frame_height];
            pix_checked = new int[ct.frame_width, ct.frame_height];
            nx = new int[ct.frame_width, ct.frame_height];
            ny = new int[ct.frame_width, ct.frame_height];
            weightsum = new double[ct.frame_width, ct.frame_height];
            if (ct == null) return result;

            for (int i = 0; i < ct.frame_width; i++)
                for(int j = 0; j < ct.frame_height; j++)
                {
                    var pw = ct.GeneratePWM_point(new PointF(i, j));
                    npixels[i,j] = pw.Count;
                    nx[i,j] = pw.nx;
                    ny[i,j] = pw.ny;
                    pix_checked[i,j] = pw.pix_checked;
                    double tempvalue = 0;
                    for (int k = 0; k < pw.Count; k++)
                    {
                        tempvalue += pw.weights[k].weight * data[pw.weights[k].x, pw.weights[k].y];
                        weightsum[i, j] += pw.weights[k].weight * 100000;
                    }
                    tempvalue *= ct.k_area;
                    result[i, j] = tempvalue;

                }



            return result;
        }

        private int[,] npixels;
        private double[,] weightsum;
        private int[,] pix_checked;
        private int[,] nx;
        private int[,] ny;

        public void StretchData_test()
        {
            double[,] data = StretchData(max_values);
            ThermalMapForm form = new ThermalMapForm(data);
            form.Text = "data";
            form.Show();
            ThermalMapForm form2 = new ThermalMapForm(npixels);
            form2.Text = "npix";
            form2.Show();
            ThermalMapForm form3 = new ThermalMapForm(weightsum);
            form3.Text = "sum";
            form3.Show();
            ThermalMapForm form4 = new ThermalMapForm(pix_checked);
            form4.Text = "checkd";
            form4.Show();
            ThermalMapForm form5 = new ThermalMapForm(nx);
            form5.Text = "nx";
            form5.Show();
            ThermalMapForm form6 = new ThermalMapForm(ny);
            form6.Text = "ny";
            form6.Show();

        }


    }
}
