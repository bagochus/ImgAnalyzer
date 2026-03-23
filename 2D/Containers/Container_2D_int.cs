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
            this.filename = filename;
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

        public override double ddata(int x, int y)
        {
            return data[x,y];
        }

        public override double Max()
        {
            int result = int.MinValue;
            foreach (var item in data) if (item > result) result = item;
            return result;
        }

        public override double Min()
        {
            int result = int.MaxValue;
            foreach (var item in data) if (item < result) result = item;
            return result;
        }

        public override int GetCount(double v1, double v2)
        {
            int count = 0;
            foreach (var item in data) if (item >= v1 && item <= v2) count++;
            return count;
        }

        protected override double[,] GetDoubleData()
        {
            double[,] d_data = new double[width,height];
            for (int i = 0; i< width; i++)
                for(int j = 0; j< height;j++)
                    d_data[i,j] = data[i,j];
            return d_data;
        }


        public override double SumWhere(Func<double, bool> criteria)
        {
            double result = 0;
            foreach (var v in data) if (criteria(v)) result += v;
            return result;
        }

        public override int CountWhere(Func<double, bool> criteria)
        {
            int result = 0;
            foreach (var v in data) if (criteria(v)) result++;
            return result;
        }

    }
}
