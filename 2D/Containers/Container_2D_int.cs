using System;
using System.Collections.Generic;
using System.IO;
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

        public override void SaveToFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(header_int + Name);
                writer.Write(width);
                writer.Write(height);   
                for (int i = 0; i < data.GetLength(0); i++)
                    for (int j = 0; j < data.GetLength(1); j++)
                        writer.Write(data[i, j]);
            }


        }


    }
}
