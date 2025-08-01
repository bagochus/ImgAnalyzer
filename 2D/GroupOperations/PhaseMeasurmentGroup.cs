using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class PhaseMeasurmentGroup : IGroupOperation
    {

        //-------------interface properties-----------------------------
        public string Description { get { return "Вычисляет фазу на основании 3 каналов."; } }

        public double[] SingleValueParameters {  get; set; }

        public string[] SingleValueNames { get { return new string[0]; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        private string[] containerNames = { "A_min", "A_max", "B_min", "B_max", "C_min", "C_max" };
        public string[] ContainerNames { get { return containerNames; } }

        public IImageSource[] imageSources { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private string[] imgSourceNames = { "A(sin)","B(cos)","C(-cos)" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int min_count = Int32.MaxValue;



        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }

            ContainerBatch batch = new ContainerBatch();
            batch.Name = "Phase";
            ImageManager.containerBatches.Add(batch);
            DataManager_2D.workToBeDone += min_count;

            for (int i = 0; i < min_count; i++)
            {

               double[,] phaseData = GeneratePhaseImage(i);
               Container_2D_double c = new Container_2D_double(phaseData);
               batch.AddContainer(c);
                DataManager_2D.progress.Report(1);

            }
            










        }


        private double[,] GeneratePhaseImage(int n)
        {

            int[,] dataA = ImageProcessor_2D.Index(imageSources[0] as ImageBatch, n);
            int[,] dataB = ImageProcessor_2D.Index(imageSources[1] as ImageBatch, n);
            int[,] dataC = ImageProcessor_2D.Index(imageSources[2] as ImageBatch, n);
            var ct1 = (imageSources[0] as ImageBatch).coordinateTransformation;
            var ct2 = (imageSources[1] as ImageBatch).coordinateTransformation;
            var ct3 = (imageSources[2] as ImageBatch).coordinateTransformation;
            double[,] ddataA = ImageProcessor_2D.FitData(dataA, ct1);
            double[,] ddataB = ImageProcessor_2D.FitData(dataB, ct2);
            double[,] ddataC = ImageProcessor_2D.FitData(dataC, ct3);

            double[,] phase = new double[ddataA.GetLength(0), ddataA.GetLength(1)];

            for (int i = 0; i < ddataA.GetLength(0); i++)
                for (int j = 0; j < ddataA.GetLength(1); j++)
                {
                    double a_min = ContainerParameters[0].ddata[i, j];
                    double a_max = ContainerParameters[1].ddata[i, j];
                    double b_min = ContainerParameters[2].ddata[i, j];
                    double b_max = ContainerParameters[3].ddata[i, j];
                    double c_min = ContainerParameters[4].ddata[i, j];
                    double c_max = ContainerParameters[5].ddata[i, j];

                    double a = ddataA[i, j];
                    double b = ddataB[i, j];
                    double c = ddataB[i, j];

                    double a_value = 2 * ((a - a_min) / (a_max - a_min)) - 1;
                    if (a_value > 1) a_value = 1;
                    if (a_value < -1) a_value = -1;

                    double b_value = 2 * ((b - b_min) / (b_max - b_min)) - 1;
                    if (b_value > 1) b_value = 1;
                    if (b_value < -1) b_value = -1;

                    double c_value = 2 * ((c - c_min) / (c_max - c_min)) - 1;
                    if (c_value > 1) c_value = 1;
                    if (c_value < -1) c_value = -1;

                    phase[i, j] = Math.Atan2(a_value, (b_value-c_value)/2) / Math.PI * 180;


                }
            return phase;

            Container_2D_double cont = new Container_2D_double(phase.GetLength(0), phase.GetLength(1));
            cont.data = phase;
            cont.Name = "Phase_" + n.ToString();
            cont.ImageGroup = "F";
            //if (CreateContainer) DataManager_2D.containers.Add(cont);



            HeatMapForm form = new HeatMapForm(cont);



        }



        private bool Check()
        {
            bool result = true;
            result &= (imageSources[0] is ImageBatch);
            result &= (imageSources[1] is ImageBatch);
            result &= (imageSources[2] is ImageBatch);
            if (!result)
            {
                error_message = "Этот алгоритм использует изображения с камеры";
                return false;
            }

            result = (ImageManager.AllCTDefined());
            if (!result)
            {
                error_message = "На одной из групп изображений не указана активная область";
                return false;
            }
            if (imageSources[0].Count < min_count) min_count = imageSources[0].Count;
            if (imageSources[1].Count < min_count) min_count = imageSources[1].Count;
            if (imageSources[2].Count < min_count) min_count = imageSources[2].Count;

            if (min_count < 1)
            {
                error_message = "Одна из групп изображений пуста";
                return false;

            }


            var ct1 = (imageSources[0] as ImageBatch).coordinateTransformation;
            var ct2 = (imageSources[1] as ImageBatch).coordinateTransformation;
            var ct3 = (imageSources[2] as ImageBatch).coordinateTransformation;
            result = (ct1.frame_width == ct2.frame_width);
            result &= (ct1.frame_width == ct3.frame_width);
            result &= (ct1.frame_width == ContainerParameters[0].Width);
            result &= (ct1.frame_height == ct2.frame_height);
            result &= (ct1.frame_height == ct3.frame_height);
            



            if (!result)
            {
                error_message = "Размеры активных областей не совпадают";
                return false;

            }
            for (int i = 0; i< 6 ; i++)
            {
                result &= (ct1.frame_width == ContainerParameters[i].Width);
                result &= (ct1.frame_height == ContainerParameters[i].Height);
            }
            if (!result)
            {
                error_message = "Размер одной из карт не совпадает с размером активных областей ";
                return false;

            }


            return result;





        }







    }
}
