using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class LU_Table : IGroupOperation
    {
        //-------------interface properties-----------------------------
        public virtual string Description
        {
            get
            {
                return
                    "Создает LUТ таблицу 64*64*256 на основе пакета данных\n" +
                    "OUT0-256 целевые выходные значения\n" +
                    "Vstart, Vstep - начальные значения и шаг АЦП соотв. кадрам\n";
            }
        }

        public double[] SingleValueParameters { get; set; }

        string[] singeValueNames = new string[] { "OUT0", "OUT255", "Vstart", "Vstep" };
        public string[] SingleValueNames { get { return singeValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }



        public string[] ContainerNames { get {
                if (useMask)
                    return new string[] { "Mask" };
                else return new string[0]; 
            } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "Input (phase)" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int width, height;

        double[,] avgField, avgFieldPrev;
        int[,,] lut;

        double out0, out255, vstart, vstep;
        Func<int, int, double> GetData;
        Func<int, int, double> GetMaskData;


        int lut_width = 64;
        int lut_height = 64;
        int lut_depth = 256;
        int max_code = 4096;

        int block_width;
        int block_height;

        protected bool useMask = false;
        protected bool useGradientTails = false;

        //--------------------report data----------------

        protected int patched_cells = 0;
        protected int patched_pilars = 0;
        protected int trimmed_values = 0;


        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }


            out0 = SingleValueParameters[0];
            out255 = SingleValueParameters[1];
            vstart = SingleValueParameters[2];
            vstep = SingleValueParameters[3];
            width = imageSources[0].Width;
            height = imageSources[0].Height;

            block_width = (int)Math.Ceiling((double)width / lut_width);
            block_height = (int)Math.Ceiling((double)height / lut_height);

            avgField = new double[lut_width, lut_height];
            avgFieldPrev = new double[lut_width, lut_height];
            lut = new int[lut_width, lut_height, lut_depth];

            for (int i = 0; i < lut_width; i++)
                for (int j = 0; j < lut_height; j++)
                    for (int k = 0; k < lut_depth; k++)
                        lut[i, j, k] = -1;


            if (useMask)
            {
                GetMaskData = ContainerParameters[0].ddata;
            }
            else
            {
                GetMaskData = (x, y) => 1;
            }



            ContainerBatch batch = new ContainerBatch();
            batch.Name = ImageManager.GetUniqueSourceName("LUT");
            //batch.Width = width;
            //block_height = height;
            ImageManager.containerBatches.Add(batch);

            DataManager_2D.workToBeDone += imageSources[0].Count;

            string foldername = FileManagement.CreateUniqueFolder("D:\\containers\\" + batch.Name);



            await Task.Run(() =>
            {
                SweepInputData();
                PatchTable();
                double[,] lut_data = new double[lut_width, lut_height];
                for (int lut_layer = 0; lut_layer < lut_depth;lut_layer++)
                {
                    for (int i = 0;i < lut_width;i++)
                        for (int j = 0;j < lut_height;j++)
                            lut_data[i,j] = lut[i,j,lut_layer];

                    Container_2D_double c = new Container_2D_double(lut_data);
                    
                    DataManager_2D.progress.Report(1);

                    string filename = Path.Combine(foldername, lut_layer.ToString() + ".bin");
                    c.SaveToFile(filename);
                    batch.Filenames.Add(filename);
                }

                WriteLUTFile(Path.Combine(foldername, "LUT.txt"));

            });

            MessageBox.Show("LUT таблица сгенерирована\n" +
                $"{patched_pilars} битых секторов скопировано\n" +
                $"{patched_cells} ячеек интерполировано\n" +
                $"{trimmed_values} значений было вне допустимого диапазона");

            

        }

        public bool Check()
        {
            bool result = true;
            if (useMask)
            {
                result &= (ContainerParameters[0].Width == imageSources[0].Width);
                result &= (ContainerParameters[0].Height == imageSources[0].Height);
            }

            if(!result)
            {
                error_message = "Размеры полей не совпадают";
                return false;

            }

            return true;
        }


        
        private void SweepInputData()
        {
            I2DFileHandler hndl_first = imageSources[0].Get2DFileHandler(0);
            GetData = hndl_first.GetPixelValue;
            GenerateAverageField();
            avgFieldPrev = avgField;
            hndl_first.Dispose();

            for (int i = 1; i < imageSources[0].Count; i++)
            {
                I2DFileHandler hndl = imageSources[0].Get2DFileHandler(i);
                GetData = hndl.GetPixelValue;
                GenerateAverageField();
                AnalyzeFields(i - 1);
                avgFieldPrev = avgField;
                hndl.Dispose();
            }
        }

        private void AnalyzeFields(int n)
        {
            //n - номер кадра, из которого получено avgFieldPrev
            //пробегаем по значениям двух усреденнных полей, ищем, можем ли мы что нибудь внести в ЛУТ
            for (int i = 0; i < lut_width; i++)
                for (int j = 0; j < lut_height; j++)
                {
                    FindCodes(avgFieldPrev[i, j], avgField[i, j],n,i,j);

                }
        }
        
        private void GenerateAverageField()
        {
            avgField = new double[lut_width, lut_height];
            for (int i = 0; i <lut_width;i++)
                for (int j = 0; j< lut_height; j++)
                {
                    avgField[i,j] = CalculateBlockValue(i, j);
                }
        }

        private double CalculateBlockValue(int i_block, int j_block)
        {
            double result = 0;
            int count = 0;
            for (int i = 0; i < block_width; i++)
            {
                int x = i_block * block_width + i;
                if (x > width - 1) continue;

                for (int j = 0; j < block_height; j++)
                {

                    int y = j_block * block_height + j;
                    if (y > height - 1) continue;
                    if (GetMaskData(x,y) == 0) continue;
                    result += GetData(x, y);
                    count++;
                }
            }
            if (count == 0) return 0;
            else return result/count;

        }

        private void FindCodes(double value1, double value2, int v_index,int lut_i,int lut_j)
        {
            //value1, value2 - измеренные значения "фазы" для двух соседних точек
            //v_index - номер кадра, при котором было получено значение value1
            double step = (out255 - out0) / (lut_depth - 1);
            //int lut_index = -1;
            //пробегаем по всему требуемому диапазону фаз
            for (int i = 0; i < lut_depth; i++)
            {
                //выбираем одно из целевых значений фазы
                double value = out0 + step * i;

                double low_value = Math.Min(value1, value2);
                double high_value = Math.Max(value1, value2);

                if (value >= low_value && value < high_value)
                {
                    //если целевое значение оказалось между нашими полученными
                    //то возвращаем взвешенный код АЦП который будет находится между v_index и v_index+1
                    double float_code = vstart + vstep * (v_index + (value - value1) / (value2 - value1));
                    //защита от дурака
                    if (float_code < 0) float_code = 0;
                    if (float_code > max_code) float_code = max_code;
                    //Записываем в ЛУТ наш код
                    
                    

                    lut[lut_i, lut_j,i] = (int)Math.Round(float_code);
                }
            }


        }
        
        private void PatchTable()
        {
            int[,] correctValues = new int[lut_width,lut_height];
            bool[,] goodPilars = new bool[lut_width,lut_height];

            for (int i = 0; i < lut_width; i++) 
                for (int j = 0; j < lut_height; j++)
                {
                    correctValues[i,j] = 0;
                    for (int k = 0; k < lut_depth; k++)
                        if (lut[i, j, k] != -1) correctValues[i, j]++;
                }

            for (int i = 0; i < lut_width; i++)
                for (int j = 0; j < lut_height; j++)
                {
                    goodPilars[i, j] = (correctValues[i, j] > 1 );
                }

            for (int i = 0; i < lut_width; i++)
                for (int j = 0; j < lut_height; j++)
                {
                    if (correctValues[i, j] == lut_depth) continue;
                    if (correctValues[i, j] == 1) PatchOneValuePilar(i, j);
                    if (correctValues[i, j] > 1 ) PatchFewValuesPilar(i, j);
                    if (correctValues[i,j] == 0) 
                    {
                        Point nearestPilar = FindNearestGoodPilar(goodPilars, i, j);
                        ClonePilar(nearestPilar.X, i,nearestPilar.Y,j);
                    
                    }
                }



        }

        private void PatchOneValuePilar(int i, int j)
        {
            int value = 0;
            for (int k = 0; k < lut_depth; k++)
            {
                if (lut[i,j,k] != -1)
                {
                    value = lut[i,j,k];
                    break;
                }
            }
            for (int k = 0; k < lut_depth; k++) lut[i,j,k] = value;

        }

        private void PatchFewValuesPilar(int i, int j)
        {
            int[] patched_array = new int[lut_depth];
            int good_index1 = -1;
            int good_index2 = -1;
            int good_value1 = 0;
            int good_value2 = 0;

            bool need_head_patch = false;
            bool need_tail_patch = false;

            for (int k = 0; k < lut_depth; k++)
            {
                if (lut[i, j, k] != -1)
                {
                    patched_array[k] = lut[i, j, k];
                    if (good_index1 == k - 1)
                    {
                        //это значит что мы залатали все ранее встреченные дыры
                        good_index1 = k;
                        good_value1 = lut[i, j, k];
                    }
                    else
                    {
                        //это значит мы соскочили с дефектного участка, необходимо восстанавливать
                        //пройденную дыру
                        good_index2 = k;
                        good_value2 = lut[i, j, k];

                        double step = (double)(good_value2 - good_value1) / (good_index2 - good_index1);

                        for (int k_temp = good_index1 + 1; k_temp < good_index2; k_temp++)
                        {
                            double patch_value = good_value1 + (k_temp - good_index1) * step;
                            if (patch_value < 0)
                            {
                                patch_value = 0;
                                trimmed_values++;
                            }
                            if (patch_value > max_code)
                            {
                                patch_value = max_code;
                                trimmed_values++;
                            }
                            patched_array[k_temp] = (int)Math.Round(patch_value);
                            patched_cells++;
                        }
                        good_index1 = k;
                        good_value1 = lut[i, j, k];
                    }
                }
                else
                {
                    if (k == 0)
                    {
                        //если дыра в начале массива, сейчас мы ничего не сделаем
                        //отметим и вернемся к ней потом
                        need_head_patch = true;
                        continue;
                    }
                    //если дыра в начале масиива продолжается, тоже самое
                    if (need_head_patch && good_index1 == -1) continue;

                    if (k == lut_depth -1) need_tail_patch = true;
                }
            }


            if (need_head_patch && useGradientTails)
            {
                //если нужно заделать дыру в начала массива 
                //ищем две первые два хороших значение и от них интерполируем
                good_index1 = -1;
                good_index2 = -1;

                for (int k = 0; k < lut_depth; k++)
                {
                    if (lut[i, j, k] != -1) 
                    {
                        if (good_index1 == -1)
                        {
                            good_index1 = k;
                            good_value1 = lut[i,j, k];
                            continue;
                        }
                        else if (good_index2 == -1) 
                        {
                            good_index2 = k;
                            good_value2 = lut[i, j, k];
                            break;
                        }
                    }
                }

                double step = (double)(good_value2 - good_value1) / (good_index2 - good_index1);

                for (int k_temp = good_index1 - 1; k_temp >= 0; k_temp--)
                {
                    double patch_value = good_value1 + (k_temp - good_index1) * step;
                    if (patch_value < 0)
                    {
                        patch_value = 0;
                        trimmed_values++;
                    }
                    if (patch_value > max_code)
                    {
                        patch_value = max_code;
                        trimmed_values++;
                    }
                    patched_array[k_temp] = (int)Math.Round(patch_value);
                    patched_cells++;
                }
            }


            if (need_head_patch && !useGradientTails)
            {
                //если нужно заделать дыру в начала массива 
                //ищем две первое хорошее значение и заполняем им все дыры
                good_index1 = -1;

                for (int k = 0; k < lut_depth; k++)
                {
                    if (lut[i, j, k] != -1)
                    {
                        good_index1 = k;
                        good_value1 = lut[i, j, k];
                        break;
                    }
                }

                for (int k_temp = good_index1 - 1; k_temp >= 0; k_temp--)
                {
                    patched_array[k_temp] = good_value1;
                }
            }




            if (need_tail_patch && useGradientTails)
            {
                //если нужно заделать дыру в конце массива 
                //ищем два последних хороших значения и от них интерполируем
                good_index1 = -1;
                good_index2 = -1;

                for (int k = 0; k < lut_depth; k++)
                {
                    if (lut[i, j, k] != -1)
                    {
                        if (good_index1 == -1)
                        {
                            good_index1 = k;
                            good_value1 = lut[i, j, k];
                            continue;
                        }
                        if (good_index2 == -1)
                        {
                            good_index2 = k;
                            good_value2 = lut[i, j, k];
                            continue;
                        }
                        good_index1 = good_index2;
                        good_value1 = good_value2;
                        good_index2 = k;
                        good_value2 = lut[i, j, k];
                    }
                }

                double step = (double)(good_value2 - good_value1) / (good_index2 - good_index1);

                for (int k_temp = good_index2 + 1; k_temp < lut_depth; k_temp++)
                {
                    double patch_value = good_value1 + (k_temp - good_index1) * step;
                    if (patch_value < 0)
                    {
                        patch_value = 0;
                        trimmed_values++;
                    }
                    if (patch_value > max_code)
                    {
                        patch_value = max_code;
                        trimmed_values++;
                    }
                    patched_array[k_temp] = (int)Math.Round(patch_value);
                    patched_cells++;
                }
            }

            if (need_tail_patch && !useGradientTails)
            {
                //если нужно заделать дыру в конце массива 
                //ищем последнее хорошее значение и заполняем им оставшуюся часть
                good_index1 = -1;
                good_index2 = -1;

                for (int k = 0; k < lut_depth; k++)
                {
                    if (lut[i, j, k] != -1)
                    {
                        good_index2 = k;
                        good_value2 = lut[i,j,k];
                    }
                }

                for (int k_temp = good_index2 + 1; k_temp < lut_depth; k_temp++)
                {
                    patched_array[k_temp] = good_value2;
                }
            }

            for (int k = 0; k < lut_depth; k++)
                lut[i, j, k] = patched_array[k];

        }

        private void ClonePilar(int i_from, int i_to, int j_from, int j_to)
        {
            for (int k = 0; k<lut_depth; k++)
                lut[i_to,j_to,k] = lut[i_from,j_from,k];
            patched_pilars++;
        }

        private Point FindNearestGoodPilar(bool[,] field, int i,int j)
        {
            Point result = new Point(-1, -1);
            double min_distance = Double.MaxValue;

            for (int ii =  0; ii < field.GetLength(0); ii++) 
                for (int jj = 0; jj < field.GetLength(1); jj++)
                {
                    if (!field[ii, jj]) continue;
                    double distance = Math.Sqrt(Math.Pow(i-ii,2)+Math.Pow(j-jj,2));
                    if (distance < min_distance)
                    {
                        min_distance = distance;
                        result.X = ii;
                        result.Y = jj;
                    }
                }
            return result;
        }

        private void WriteLUTFile(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                for (int i = 0; i < lut_width*lut_height; i++)
                {
                    int x = i % lut_width;
                    int y = i / lut_height;

                    string s = "";
                    for (int j = 0; j < lut_depth; j++)
                    {
                        int value = lut[x,y,j];
                        s += value.ToString();
                        if (j != lut_depth-1) s += ";";

                    }
                    writer.WriteLine(s);
                }
            }
        }



    }
}
