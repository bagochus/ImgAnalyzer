using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class PhaseMeasurmentGroup2 : IGroupOperation
    {

        //-------------interface properties-----------------------------
        public string Description { get { return "Вычисляет фазу на основании 2 каналов.\n Минимум интенсивности - const, k1= А1/А2 = const"; } }

        public double[] SingleValueParameters {  get; set; }

        private string[] singleValueNames = { "k1" ,"A_min", "B_min" };
        public string[] SingleValueNames { get { return singleValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        //private string[] containerNames = { "A_min", "A_max", "B_min", "B_max", "C_min", "C_max" };
        public string[] ContainerNames { get { return new string[0]; } }

        public IImageSource[] imageSources {  get; set; }

        private string[] imgSourceNames = { "A(sin)","B(cos)","C(-cos)" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int min_count = Int32.MaxValue;

        private double a_min;
        private double b_min;
        private double k1;




        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }
            k1 = SingleValueParameters[0];

            a_min= SingleValueParameters[1];
            b_min= SingleValueParameters[2];




            ContainerBatch batch = new ContainerBatch();
            batch.Name = "Phase";
            ImageManager.containerBatches.Add(batch);
            DataManager_2D.workToBeDone += min_count;

            for (int i = 0; i < min_count; i++)
            {

                await Task.Run(() =>
                {
                    double[,] phaseData = GeneratePhaseImage(i);
                    Container_2D_double c = new Container_2D_double(phaseData);
                    batch.AddContainer(c);
                    DataManager_2D.progress.Report(1);

                });


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


                    double a = ddataA[i, j];
                    double b = ddataB[i, j];
                    double c = ddataB[i, j];



                    phase[i, j] = Math.Atan(((a-a_min)/(b_min))*k1) / Math.PI * 180;


                }
            return phase;




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
            


            return result;





        }







    }
}
