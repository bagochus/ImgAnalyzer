using System;
using System.Collections.Generic;
using System.IO;
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

        public override void SaveToFile(string filename)
        {
            this.filename = filename;
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(header_double + Name);
                writer.Write(width);
                writer.Write(height);
                for (int i = 0; i < data.GetLength(0); i++)
                    for (int j = 0; j < data.GetLength(1); j++)
                        writer.Write(data[i, j]);
            }


        }
        public override double[,] GetDData()
        {
            return data;
        }


    }
}
