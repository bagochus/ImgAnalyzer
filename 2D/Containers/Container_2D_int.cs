using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    public class Container_2D_int : Container_2D
    {
        public int[,] data;
        public Container_2D_int(int width, int height )
        {
            this.width = width;
            this.height = height;
            data = new int[width, height];
        }

        public Container_2D_int(int[,] data)
        {
            this.data = data;
            this.width = data.GetLength(0);
            this.height = data.GetLength(1);
        }

    }
}
