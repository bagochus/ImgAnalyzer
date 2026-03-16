using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D
{
    public abstract class Container_2D : IContainer_2D
    {
        protected const string header_double = "Container_Double:";
        protected const string header_int = "Container_Int:";

        public string Name { get; set; }
        public string ImageGroup { get; set; }
        public int Width { get { return width; } }
        public int Height { get { return height; } }




        public string Filename { get { return filename; } set { filename = value; } }
        protected string filename = "";


        protected int width;
        protected int height;

        public double[,] DoubleData { get { return GetDoubleData(); } }

        protected abstract double[,] GetDoubleData();

        public abstract double Max();
        public abstract double Min();

        public abstract double ddata(int x, int y);

        public abstract int GetCount(double v1, double v2);

        public abstract void SaveToFile(string filename);

        public static IContainer_2D ReadFromFile(string filename)
        {
            IContainer_2D container;
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                string header = reader.ReadString();
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                if (header.Contains(header_double))
                {
                    container = new Container_2D_double(width, height);
                    container.Name = header.Replace(header_double, "");
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            (container as Container_2D_double).data[i, j] = reader.ReadDouble();
                        }
                    }
                    container.Filename = filename;
                    container.ImageGroup = "X";
                }
                else if (header.Contains(header_int))
                {
                    container = new Container_2D_int(width, height);
                    container.Name = header.Replace(header_int, "");
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            (container as Container_2D_int).data[i, j] = reader.ReadInt32();
                        }
                    }
                    container.Filename = filename;
                    container.ImageGroup = "X";
                }

                else throw new Exception("Error reading file");
            }
            return container;
        }

        public static bool GetParameters(string filename, out Type type, out int width, out int height)
        {
            bool result = true;
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = new BinaryReader(stream))
            {
                string header = reader.ReadString();
                width = reader.ReadInt32();
                height = reader.ReadInt32();
                if (header.Contains(header_double))
                {
                    type = typeof(Container_2D_double);
                }
                else if (header.Contains(header_int))
                {
                    type = typeof(Container_2D_int);
                }
                else

                {
                    result = false;
                    type = null;
                }

                return result;



            }





        }

        public double GetInterpolatedValue(double x, double y)
        {
            if (x < 0 || x >= width - 1 || y < 0 || y >= height - 1)
                throw new ArgumentOutOfRangeException();

            int x1 = (int)Math.Floor(x);
            int y1 = (int)Math.Floor(y);
            int x2 = x1 + 1;
            int y2 = y1 + 1;

            double dx = x - x1;
            double dy = y - y1;
            double w1 = (1 - dx) * (1 - dy);
            double w2 = dx * (1 - dy);
            double w3 = (1 - dx) * dy;
            double w4 = dx * dy;

            return ddata(x1, y1) * w1 + ddata(x2, y1) * w2 + ddata(x1, y2) * w3 + ddata(x2, y2) * w4;
        }


    }
}
