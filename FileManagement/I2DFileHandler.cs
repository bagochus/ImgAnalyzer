using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public interface I2DFileHandler : IDisposable
    {
        void LoadFile(IImageSource imageSource, string fileName);
        void LoadFile(IImageSource imageSource, int index);

        void SelectLine(int line);
        double GetPixelValue(int pixel);
        double GetPixelValue(int line, int pixel);

        

    }
}
