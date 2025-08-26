using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImgAnalyzer;

namespace ImgAnalyzer._2D
{
    public class StitchSpatiallyFilter : StitchSpatially
    {
        private int startx, starty;
        //private int width, height;
        private double thr;
        private double top = 360;
        private double full_phase = 360;

        private int currentStep = 0;
        private double previousValue;
        private Func<int, int, double> getData;
        private double[,] result;
        private int[,] shifts;
        


        public StitchSpatiallyFilter()
        {
            pixbypix = false;
            this.description = "ѕроводит сшивку фазы по стыкам 0-360\n" +
                "thr - порог перехода через 0, x,y - начальна€ точка прохода\n" +
                "ѕосле вычислени€ набега фазы проводитс€ устранение битых линий";
            this.singeValueNames = new string[] { "thr", "x" , "y" };
            this.containerNames = new string[] { "Input" };

        }

        
        protected override void Actions()
        {

            CalcCenterLine();
            CalculateSides();
            PinLowestZero();
            EliminateLines();
            CalculateResult();
            
        }

       

        

       
    }
}

