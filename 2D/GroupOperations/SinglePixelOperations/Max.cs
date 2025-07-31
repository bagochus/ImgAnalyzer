using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.GroupOperations.SinglePixelOperations
{
    internal class Max : SinglePixelOperation
    {
        
        public Max()
        {
            operationName = "Maximum";
            description = "Попиксельно вычисляет максимум";
        }
        

        protected override void Finish()
        {
        }

        protected override void Prepare()
        {
            for (int i = 0; i < result.GetLength(0); i++) 
                for (int j = 0; j < result.GetLength(1); j++) 
                    result[i,j]  = Double.MinValue;
        }

        protected override void ProcessPixel(int x, int y, int n, double value)
        {
            if (value > result[x,y])  result[x,y]  = value;
        }
    }
}
