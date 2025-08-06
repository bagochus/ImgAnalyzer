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

        //private static readonly Lazy<ColorMaps> _lazyInstance = new Lazy<ColorMaps>(() => new ColorMaps());

        static ColorMaps()
        {
            var old = new ColorScheme();
            old.Name = "Old";
            old.AddColor(Color.FromArgb(60, 0, 60), 0);
            old.AddColor(Color.FromArgb(200, 70, 0), 0.5);
            old.AddColor(Color.FromArgb(255, 247, 220), 1);
            Schemes.Add(old);

            ColorScheme blu_red = new ColorScheme();
            blu_red.Name = "Blue-Red";
            blu_red.AddColor(Color.Blue, 0);
            blu_red.AddColor(Color.Red, 1);
            Schemes.Add(blu_red);

            ColorScheme Heat = new ColorScheme();
            Heat.Name = "Heat";
            Heat.AddColor(Color.Black, 0);
            Heat.AddColor(Color.Red, 0.33);
            Heat.AddColor(Color.Orange, 0.66);
            Heat.AddColor(Color.White, 1);
            Schemes.Add(Heat);





        }














    }
}
