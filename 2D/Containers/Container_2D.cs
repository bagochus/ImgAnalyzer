using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public class Container_2D : IContainer_2D
    {
        public string Name { get; set ; }
        public string ImageGroup { get; set; }
        public int Width {get { return width; } }
        public int Height { get { return height; } }

        protected int width;
        protected int height;






    }
}
