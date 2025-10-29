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
    public class NewCT_test : IGroupOperation
    {

        //-------------interface properties-----------------------------
        public string Description { get => "Test";  }

        public double[] SingleValueParameters {  get; set; }

        string[] singeValueNames = new string[] { "n" };
        public string[] SingleValueNames { get { return singeValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        public string[] ContainerNames { get { return new string[0]; } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "A"};
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int min_count = Int32.MaxValue;
        double[,] phase = null;

        double k1, k2, k3, m1, m2, m3;


        public async Task Execute()
        {



            int n = (int)SingleValueParameters[0];



            var ib0 = (imageSources[0] as ImageBatch);


            var hndl0 = (imageSources[0].Get2DFileHandler(n) as TiffImgFileHandler);


            double[,] ddataA = ImageProcessor_2D.FitImage2(hndl0, ib0.coordinateTransformation);

            DataManager_2D.containers.Add(new Container_2D_double(ddataA) {Name = "Test" });
            
           
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
