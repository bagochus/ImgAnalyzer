using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class FitCoordinateSystem : IGroupOperation
    {
        public string Name = "";
        public string Description { get { return description; } }
        protected string description = "Преобразует 2D карту согласно системе координат";
        public double[] SingleValueParameters { get; set; }
        public string[] SingleValueNames { get { return singeValueNames; } }
        protected string[] singeValueNames = new string[0];
        public IContainer_2D[] ContainerParameters { get; set; }
        public string[] ContainerNames { get { return containerNames; } }
        protected string[] containerNames = new string[] {"Карта для преобразования" };


        public IImageSource[] imageSources { get; set; }
        public string[] imageSourceNames { get { return _imageSourceNames; } }
        private string[] _imageSourceNames = { "Группа изображений с системой координат" };
        public bool UseTransformation { get; set; }



        protected double[,] result;
        protected int width;
        protected int height;
        protected string operationName = "Fit";
        private string error_message = "";

        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message);
                return;
            }

            await Task.Run(() => 
            {
                 result = ImageProcessor_2D.FitDataDouble(ContainerParameters[0].ddata,
                    (imageSources[0] as ImageBatch).coordinateTransformation);

                



            });

            Container_2D_double c = new Container_2D_double(result);
            c.Name = operationName + '_' + ContainerParameters[0].Name;
            DataManager_2D.containers.Add(c);







        }



        private bool Check()
        {
            width = imageSources[0].Width;
            height = imageSources[0].Height;
            
            bool result = true;

            if (!(imageSources[0] is ImageBatch))
            {
                error_message = "Нужно выбрать группу изображений";
                return false;
            }
            if ((imageSources[0] as ImageBatch).coordinateTransformation == null)
            {
                error_message = "Не определена система координат";
                return false;
            }


            result &= (ContainerParameters[0].Width == width);
            result &= (ContainerParameters[0].Height == height);
            if (!result)
            {
                error_message = "Размер контейнера должен соответствовать размеру группы изображений";
                return false;
            }






            return true;
        }

    }
}
