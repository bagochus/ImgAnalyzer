using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer._2D;
using ImgAnalyzer.MeasurmentTypes;

namespace ImgAnalyzer
{
    public class PhaseMeasurer
    {
        public static int a_max_id = 0;
        public static int a_min_id = 0;
        public static int b_max_id = 0;
        public static int b_min_id = 0;
        public static bool CreateContainer = false;
        public static bool UseRadian = false;

        public static void GeneratePhaseImage(int n)
        {
            if(!Check(n))
            {
                MessageBox.Show("Не удалось выполнить преобразование");
                return;

            }
            int[,] dataA = ImageProcessor_2D.Index(ImageManager.Batch_A(), n);
            int[,] dataB = ImageProcessor_2D.Index(ImageManager.Batch_B(), n);
            double[,] ddataA = ImageProcessor_2D.FitData(dataA, ImageManager.Batch_A().coordinateTransformation);
            double[,] ddataB = ImageProcessor_2D.FitData(dataB, ImageManager.Batch_B().coordinateTransformation);
            double[,] phase = new double[ddataA.GetLength(0), ddataA.GetLength(1)];
            
            for (int i = 0; i < ddataA.GetLength(0); i++)
                for (int j = 0; j < ddataA.GetLength(1); j++)
                {
                    double a_min = DataManager_2D.containers[a_min_id].ddata[i, j];
                    double a_max = DataManager_2D.containers[a_max_id].ddata[i, j];
                    double b_min = DataManager_2D.containers[b_min_id].ddata[i, j];
                    double b_max = DataManager_2D.containers[b_max_id].ddata[i, j];

                    double a = ddataA[i, j];
                    double b = ddataB[i, j];


                    double a_value = 2 * ((a - a_min) / (a_max - a_min)) - 1;
                    if (a_value > 1) a_value = 1;
                    if (a_value < -1) a_value = -1;

                    double b_value = 2 * ((b - b_min) / (b_max - b_min)) - 1;
                    if (b_value > 1) b_value = 1;
                    if (b_value < -1) b_value = -1;

                    bool a_pos = (a_value > 0);
                    bool b_pos = (b_value > 0);

                    double phase_shift = 0;
                    if (a_pos && !b_pos) phase_shift = Math.PI;
                    if (!a_pos && !b_pos) phase_shift = Math.PI;
                    if (!a_pos && b_pos) phase_shift = 2 * Math.PI;

                    phase[i, j] = Math.Atan(a_value / b_value) + phase_shift;// / Math.PI * 180;
                    if (!UseRadian) phase[i, j] *= (180.0 / Math.PI);

                }


            Container_2D_double cont = new Container_2D_double(phase.GetLength(0), phase.GetLength(1));
            cont.data = phase;
            cont.Name = "Phase_" + n.ToString();
            cont.ImageGroup = "F";
            if (CreateContainer) DataManager_2D.containers.Add(cont);
                


            HeatMapForm form = new HeatMapForm(cont);



        }

        private static bool Check(int n)
        {
            bool result = true;
            result &= (a_min_id != a_max_id);
            result &= (b_min_id != b_max_id);


            result &= (ImageManager.Batch_A().Count >= n - 1);
            result &= (ImageManager.Batch_B().Count >= n - 1);
            result &= (ImageManager.IsCTDefined(ImageManager.Batch_A()));
            result &= (ImageManager.IsCTDefined(ImageManager.Batch_B()));
            CoordinateTransformation cta = ImageManager.Batch_A().coordinateTransformation;
            CoordinateTransformation ctb = ImageManager.Batch_B().coordinateTransformation;
            result &= (cta.frame_width == ctb.frame_width);
            result &= (cta.frame_height == ctb.frame_height);

            return result;
        }







    }
}
