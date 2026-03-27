using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public interface IContainer_2D
    {
        string Name { get; set; }
        string ImageGroup { get; set; }
        int Width { get; }
        int Height { get; }

        void SaveToFile(string filename);

        String Filename { get; set; }

        double ddata(int x, int y);
        double[,] DoubleData { get;  }

        double GetInterpolatedValue(double x, double y);

        double Max();
        double Min();
        int GetCount(double v1, double v2);

        double SumWhere(Func<double, bool> criteria);

        int CountWhere(Func<double, bool> criteria);

         void Heaviside();


    }
}
