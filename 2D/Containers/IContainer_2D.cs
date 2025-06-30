using System;
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

        void SaveToFile(string filname);
        


    }
}
