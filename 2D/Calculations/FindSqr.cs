using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D

{
    internal class FindSqr : Calculation2D
    {

        int sortedArrayCount = 0;
        double percentile;
        double[] max_values;
        double[] min_values;
        int max_counter = 0;
        int min_counter = 0;
        Func<int, int, double> GetMaxData;
        Func<int, int, double> GetMinData;


        public FindSqr()
        {
            this.description =
                "Находит прямоугольную область с максимальной амплитудой";
            this.containerNames = new string[] { "Input" };
            this.pixbypix = false;
            this.nonReturning = true;
            

        }

        public override double Measure(int x, int y) 
        {
            throw new NotImplementedException();
        }



        

        public override void Process()
        {
            SqareFitter sf = new SqareFitter(ContainerParameters[0]);

            try { sf.FindRange(); }
            catch (Exception ex) { }


        }





    }
}
