using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class Index : SinglePixelOperation
    {

        double x = 0;
        int[,] indicies = null;

        public Index()
        {
            operationName = "Index";
            description = "Присваивает каждому пикселю значение из кадра n" +
                "Если кадр n не найден, присваивается значение X";

            this.singeValueNames = new string[] { "X" }; // Значение для замены
            this.containerNames = new string[] { "n" };
            //this._imageSourceNames = new string[] { "ImgArray" };

        }
        

        protected override void Finish()
        {
        }

        protected override void Prepare()
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

        protected override void ProcessLine(int y, int n, ushort[] line)
        {
            for (int x = 0; x < width; x++)
                if (n == indicies[x,y]) result[x, y] = line[x];
        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            if (n == indicies[x, y]) result[x, y] = value;
        }
    }
}
