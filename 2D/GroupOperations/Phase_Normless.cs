using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class PhaseNormless : IGroupOperation
    {
        public string Description { get { return "Вычисляет фазу на основании 3 каналов."; } }

        public double[] SingleValueParameters { get; set; }

        private string[] singleValueNames = { "k1", "k2", "k3", "m1", "m2", "m3" };
        public string[] SingleValueNames { get { return singleValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        public string[] ContainerNames { get { return new string[0]; } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "A(sin)", "B(cos)", "C(-cos)" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }
        double x = 0;
        int[,] indicies = null;

        public PhaseNormless()
        {
            operationName = "Phase_";
            description = "Вычисляет фазу на основе данных с 3 камер, без использования\n" +
                " нормировочных карт по формуле atan(k1I1+k2I2+k3I3/m1I1+m2I2+m3I3)\n";

            this.singeValueNames = new string[] { "k1", "k2", "k3", "m1", "m2", "m3" }; // Значение для замены
            //this.containerNames = new string[] { "n" };
            //this._imageSourceNames = new string[] { "ImgArray" };
            _imageSourceNames = new string[] { "Input data" }
            

        }
        

        protected  void Finish()
        {
        }

        protected  void Prepare()
        {

            x = SingleValueParameters[0];
            indicies = new int[width, height];

            for (int i = 0; i < result.GetLength(0); i++) 
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = x;
                    indicies[i, j] = (int)ContainerParameters[0].ddata(i, j);    
                }
                    
        }

        protected  void ProcessLine(int y, int n, ushort[] line)
        {
            for (int x = 0; x < width; x++)
                if (n == indicies[x,y]) result[x, y] = line[x];
        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            if (n == indicies[x, y]) result[x, y] = value;
        }

        public Task Execute()
        {
            throw new NotImplementedException();
        }
    }
}
