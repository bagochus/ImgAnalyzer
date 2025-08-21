using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ImgAnalyzer;
using FileManagement = ImgAnalyzer.FileManagement;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class ScalingGroup : IGroupOperation
    {

        //-------------interface properties-----------------------------
        public string Description { get { return "Складывает попиксельно k1*A1+k2*A2 и создает массив контейнеров."; } }

        public double[] SingleValueParameters {  get; set; }

        public string[] SingleValueNames { get { return new string[] {"k1","k2" }; } }

        public IContainer_2D[] ContainerParameters { get; set; }

       // private string[] containerNames = { "A1", "A2", "B_min", "B_max", "C_min", "C_max" };
        public string[] ContainerNames { get { return new string[0]; } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "A1","A2"};
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int min_count = Int32.MaxValue;

        int width;
        int height;
        double k1;
        double k2;
        Container_2D_double container;

        double[,] dataAin = null;
        double[,] dataBin = null;
        double[,] dataAout = null;
        double[,] dataBout = null;


        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }
            k1 = SingleValueParameters[0];
            k2 = SingleValueParameters[1];
            container = new Container_2D_double(width, height);

            ContainerBatch batch = new ContainerBatch();
            batch.Name = "Scaling";
            ImageManager.containerBatches.Add(batch);
            DataManager_2D.workToBeDone += min_count;
            
            string foldername = FileManagement.CreateUniqueFolder("D:\\containers\\" + batch.Name);




            if (UseTransformation)
            {
                dataAin = new double[imageSources[0].Width, imageSources[0].Height];
                dataBin = new double[imageSources[1].Width, imageSources[1].Height];
            }



            for (int i = 0; i < min_count; i++)
            {
                
                    


                await Task.Run(() =>
                {
                    if (UseTransformation)
                    {
                        PrepareData(i);
                        GenerateSumFromData();

                    }
                    else
                    {
                        GenerateSum(i);
                    }

                    string filename = Path.Combine(foldername, i.ToString() + ".bin");
                    container.SaveToFile(filename);
                    batch.Filenames.Add(filename);


                });


            }
            










        }

        private void PrepareData(int n)
        {
            I2DFileHandler handler1 = imageSources[0].Get2DFileHandler(n);
            I2DFileHandler handler2 = imageSources[1].Get2DFileHandler(n);


            for (int j = 0; j < dataAin.GetLength(1); j++)
                for (int i = 0; i < dataAin.GetLength(0);i++) 
                {
                    dataAin[i,j] = handler1.GetPixelValue(i,j);
                }
            dataAout = null;
            dataAout = ImageProcessor_2D.FitDataDouble(dataAin, imageSources[0].coordinateTransformation);

            for (int j = 0; j < dataBin.GetLength(1); j++)
                for (int i = 0; i < dataBin.GetLength(0); i++)
                {
                    dataBin[i, j] = handler2.GetPixelValue(i, j);
                }
            dataBout = null;
            dataBout = ImageProcessor_2D.FitDataDouble(dataBin, imageSources[1].coordinateTransformation);

            handler1?.Dispose();
            handler2?.Dispose();

        }


        private void GenerateSum(int n)
        {
            //double[,] result = new double[width, height];
            I2DFileHandler hndl1 = imageSources[0].Get2DFileHandler(n);
            I2DFileHandler hndl2 = imageSources[1].Get2DFileHandler(n);

            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    container.data[i, j] = k1 * hndl1.GetPixelValue(i, j) + k2 * hndl2.GetPixelValue(i, j);
                }
            hndl1?.Dispose();
            hndl2?.Dispose();
        }

        private void GenerateSumFromData()
        {
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                {
                    container.data[i, j] = k1 * dataAout[i, j] + k2 * dataBout[i, j];
                }

        }
        



        private bool Check()
        {
            bool result = true;


            if (imageSources[0].Count < min_count) min_count = imageSources[0].Count;
            if (imageSources[1].Count < min_count) min_count = imageSources[1].Count;

            if (min_count < 1)
            {
                error_message = "Одна из групп изображений пуста";
                return false;

            }

            if (UseTransformation)
            {
                var ct1 = (imageSources[0] as ImageBatch).coordinateTransformation;
                var ct2 = (imageSources[1] as ImageBatch).coordinateTransformation;
                result = (ct1.frame_width == ct2.frame_width);
                result &= (ct1.frame_height == ct2.frame_height);
                width = ct1.frame_width;    
                height = ct1.frame_height;

            }
            else
            {
                width = imageSources[0].Width;
                height = imageSources[0].Height;

                result &= imageSources[1].Width == width;
                result &= imageSources[1].Height == height;



            }




            if (!result)
            {
                error_message = "Размеры активных областей не совпадают";
                return false;

            }
            

            return result;





        }







    }
}
