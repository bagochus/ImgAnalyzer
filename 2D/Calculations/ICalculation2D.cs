using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public delegate double CalculateValue(int x, int y);


    public interface ICalculation2D
    {
        int Width { get; }
        int Height { get; }
        

        string Description { get; } 
        double[] SingleValueParameters { get; set; }
        string[] SingleValueNames { get; }
        IContainer_2D[] ContainerParameters { get; set; }
        string[] ContainerNames { get;  }

        List<IContainer_2D> SeriesParameters { get; set; }
        string SeriessName { get; }

        string ErrorMessage { get; }

        double Measure(int x, int y);
        bool Check();
        



    }
}
