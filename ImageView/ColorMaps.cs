using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImgAnalyzer.ImageView
{
    internal class ColorMaps
    {
        public static List<ColorScheme> Schemes = new List<ColorScheme>();

        private static readonly Lazy<ColorMaps> _lazyInstance =
             new Lazy<ColorMaps>(() => new ColorMaps());

        private ColorMaps()
        {
            ColorScheme blu_red = new ColorScheme();
            blu_red.Name = "Blue-Red";
            blu_red.AddColor(Color.Blue, 0);
            blu_red.AddColor(Color.Red, 1);
            Schemes.Add(blu_red);

        
        
        
        
        
        }

        












    }
}
