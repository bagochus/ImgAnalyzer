using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public interface IImageSource
    {
        int Width { get; }
        int Height { get; }
        int Count { get; }

        string Name { get; }    


        I2DFileHandler Get2DFileHandler(int index);

        I2DFileHandler Get2DFileHandler(string filename);






    }
}
