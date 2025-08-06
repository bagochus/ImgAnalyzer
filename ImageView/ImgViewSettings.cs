using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.ImageView
{
    internal enum ColorMode {Simple, BW, Color };
    internal class ImgViewSettings
    {
        public ColorMode colorMode;
        public int schemeIndex = 0;
        public double min;
        public double max;



    }
}
