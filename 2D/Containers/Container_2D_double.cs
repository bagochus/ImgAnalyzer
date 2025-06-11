using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public class Container_2D_double : Container_2D
    {
        public double[,] data;
        public Container_2D_double(int width, int height)
        {
            this.width = width;
            this.height = height;
            data = new double[width, height];
        }

        public Container_2D_double(double[,] data)
        {
            this.data = data;
            this.width = data.GetLength(0);
            this.height = data.GetLength(1);
        }


    }
}
