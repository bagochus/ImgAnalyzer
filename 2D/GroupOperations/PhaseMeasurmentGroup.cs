using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class PhaseMeasurmentGroup : IGroupOperation
    {

        //-------------interface properties-----------------------------
        public string Description { get { return "Вычисляет фазу на основе данных с 3 камер, без использования\n" +
                " нормировочных карт и матриц преобразования по формуле atan(k1I1+k2I2+k3I3/m1I1+m2I2+m3I3)\n"; } }

        public double[] SingleValueParameters {  get; set; }

        string[] singeValueNames = new string[] { "k1", "k2", "k3", "m1", "m2", "m3" };
        public string[] SingleValueNames { get { return singeValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        public string[] ContainerNames { get { return new string[0]; } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "A(sin)","B(cos)","C(-cos)" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int min_count = Int32.MaxValue;
        double[,] phase = null;

        double k1, k2, k3, m1, m2, m3;

        //---------------output variables for internal call---------------

        public ContainerBatch batch;

        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }


            k1 = SingleValueParameters[0];
            k2 = SingleValueParameters[1];
            k3 = SingleValueParameters[2];
            m1 = SingleValueParameters[3];
            m2 = SingleValueParameters[4];
            m3 = SingleValueParameters[5];


            batch = new ContainerBatch();
            batch.Name = "PhaseNormless";
            ImageManager.containerBatches.Add(batch);
            
            DataManager_2D.workToBeDone += min_count;

            string foldername = FileManagement.CreateUniqueFolder("D:\\containers\\" + batch.Name);




            for (int i = 0; i < min_count; i++)
            {
                

                await Task.Run( () =>
                {
                    
                    GeneratePhaseImage(i);
                    Container_2D_double c = new Container_2D_double(phase);
                    c.Name = batch.Name+"_"+i.ToString();
                    DataManager_2D.progress.Report(1);

                    string filename = Path.Combine(foldername, i.ToString() + ".bin");
                    c.SaveToFile(filename);
                    batch.AddContainer(c);
                    //batch.Filenames.Add(filename);

                });
            }
           
        }


        private void GeneratePhaseImage(int n)
        {

            //int[,] dataA = ImageProcessor_2D.Index(imageSources[0] as ImageBatch, n);
            //int[,] dataB = ImageProcessor_2D.Index(imageSources[1] as ImageBatch, n);
            //int[,] dataC = ImageProcessor_2D.Index(imageSources[2] as ImageBatch, n);
            //var ct1 = (imageSources[0] as ImageBatch).coordinateTransformation;
            //var ct2 = (imageSources[1] as ImageBatch).coordinateTransformation;
            //var ct3 = (imageSources[2] as ImageBatch).coordinateTransformation;
            //double[,] ddataA = ImageProcessor_2D.FitData(dataA, ct1);
            //double[,] ddataB = ImageProcessor_2D.FitData(dataB, ct2);
            //double[,] ddataC = ImageProcessor_2D.FitData(dataC, ct3);


            var ib0 = (imageSources[0] as ImageBatch);
            var ib1 = (imageSources[1] as ImageBatch);
            var ib2 = (imageSources[2] as ImageBatch);

            var hndl0 = (imageSources[0].Get2DFileHandler(n) as TiffImgFileHandler);
            var hndl1 = (imageSources[1].Get2DFileHandler(n) as TiffImgFileHandler);
            var hndl2 = (imageSources[2].Get2DFileHandler(n) as TiffImgFileHandler);

            double[,] ddataA =  ImageProcessor_2D.FitImage2(hndl0, ib0.coordinateTransformation);
            double[,] ddataB =  ImageProcessor_2D.FitImage2(hndl1, ib1.coordinateTransformation);
            double[,] ddataC =  ImageProcessor_2D.FitImage2(hndl2, ib2.coordinateTransformation);

            for (int i = 0; i < ddataA.GetLength(0); i++)
            //Parallel.For(0, ddataA.GetLength(0), (i) =>
            {
                for (int j = 0; j < ddataA.GetLength(1); j++)
                {

                    double a = ddataA[i, j];
                    double b = ddataB[i, j];
                    double c = ddataC[i, j];

                    double sin_value = k1 * a + k2 * b + k3 * c;
                    double cos_value = m1 * a + m2 * b + m3 * c;

                    phase[i, j] = Math.Atan2(sin_value, cos_value) / Math.PI * 180 + 180;

                }
            }//);

            

        }



        private bool Check()
        {
            if (!UseTransformation)
            {
                error_message = "Необходимо преорбразование координат";
                return false;

            }

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
            result &= (ct1.frame_height == ct2.frame_height);
            result &= (ct1.frame_height == ct3.frame_height);

            phase = new double[ct1.frame_width, ct1.frame_height];


            if (!result)
            {
                error_message = "Размеры активных областей не совпадают";
                return false;

            }
           

            return result;





        }





    }
}
