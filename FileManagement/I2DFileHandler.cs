using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public interface I2DFileHandler : IDisposable
    {
        string Name { get; }    
        void LoadFile(IImageSource imageSource, string fileName);
        void LoadFile(IImageSource imageSource, int index);

        double[] GetLineDouble(int index);
        void SelectLine(int line);
        double GetPixelValue(int pixel);
        double GetPixelValue(int pixel, int line);

        int Width { get; }
        int Height { get; }

        double Max();
        double Min();
        int GetCount(double v1, double v2);
        

    }
}
