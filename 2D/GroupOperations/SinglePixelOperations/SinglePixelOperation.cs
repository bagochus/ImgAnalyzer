using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;

namespace ImgAnalyzer._2D.GroupOperations
{
    public abstract class SinglePixelOperation : IGroupOperation
    {
        public string Description { get { return description; } }
        protected string description = "";
        public double[] SingleValueParameters { get; set; }
        public string[] SingleValueNames { get { return singeValueNames; } }
        protected string[] singeValueNames = new string[0];
        public IContainer_2D[] ContainerParameters { get; set; }
        public string[] ContainerNames { get { return containerNames; } }
        protected string[] containerNames = new string[0];

        
        public IImageSource[] imageSources {  get; set; }
        public string[] imageSourceNames { get { return _imageSourceNames; } }

        public bool UseTransformation {  get; set; }

        protected string[] _imageSourceNames;

        protected double[,] result;
        protected int width;
        protected int height;
        protected string operationName = "";


        public void Execute()
        {
            width = imageSources[0].Width;
            height = imageSources[0].Height;
            result = new double[width, height];

            Prepare();
            for (int image_counter = 0; image_counter < imageSources[0].Count; image_counter++)
            {
                var hndl = imageSources[0].Get2DFileHandler(image_counter);
                for (int y = 0; y < height; y++)
                {
                    hndl.SelectLine(y);
                    for (int x = 0; x < width; x++)
                        ProcessPixel(x, y, image_counter, hndl.GetPixelValue(x));

                }
                hndl.Dispose();

            }
            Finish();

            IContainer_2D cont = new Container_2D_double(result);
            cont.Name = imageSources[0].Name + operationName;
            DataManager_2D.containers.Add(cont);

        }

        protected abstract void ProcessPixel(int x,  int y, int n, double value);

        protected abstract void Prepare();
        protected abstract void Finish();


    }
}
