using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ImgAnalyzer;

namespace ImgAnalyzer._2D
{
    public class StitchSpatially : Calculation2D
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
        
        private int PixelCountToFix = 5;

        public StitchSpatially()
        {
            pixbypix = false;
            this.description = "ѕроводит сшивку фазы по стыкам 0-360" +
                "thr - порог перехода через 0, x,y - начальна€ точка прохода";
            this.singeValueNames = new string[] { "thr", "x" , "y" };
            this.containerNames = new string[] { "Input" };

        }

        private int FindAddition(double x1, double x2)
        {
            if (x1 < 0 || x1 > top || x2 < 0 || x2 > top)
                throw new ArgumentException("Phase out of range");

            if (x1 < thr && x2 > (top - thr)) return -1;
            else if (x2 < thr && (x1 > (top - thr))) return 1;
            else return 0;
        }

        public override double[,] MeasureFull()
        {
            result = new double[width, height];
            shifts = new int[width, height];
            thr = SingleValueParameters[0];
            startx = (int)SingleValueParameters[1];
            starty = (int)SingleValueParameters[2];
            getData = (int x,int y) => ContainerParameters[0].ddata(x,y);


            if (starty < 0 || starty > height || startx < 0 || startx > width) 
            {
                throw new ArgumentException("Ќачальна€ точка вне диапазона");
            }

            Actions();

            return result;
        }

        protected virtual void Actions()
        {
            CalcCenterLine();
            CalculateSides();
            PinLowestZero();
            CalculateResult();

        }



        protected void CalcCenterLine()
        {
            shifts[startx, starty] = 0;
            previousValue = getData(startx, starty);
            for (int y = starty + 1; y < height; y++)
            {
                double currentValue = getData(startx, y);
                currentStep = shifts[startx, y] = currentStep + FindAddition(previousValue, currentValue);
                previousValue = currentValue;
            }
            previousValue = getData(startx, starty);
            for (int y = starty - 1; y >= 0; y--)
            {
                double currentValue = getData(startx, y);
                currentStep = shifts[startx, y] = currentStep + FindAddition(previousValue, currentValue);
                previousValue = currentValue;
            }
        }


        protected void CalculateSides()
        {
            for (int y = 0; y < height; y++)
            {
                currentStep = shifts[startx, y];
                previousValue = getData(startx, y);
                for (int x = startx + 1; x < width; x++)
                {
                    double currentValue = getData(x, y);
                    currentStep = shifts[x, y] = currentStep + FindAddition(previousValue, currentValue);
                    previousValue = currentValue;
                }
                currentStep = shifts[startx, y];
                previousValue = getData(startx, y);
                for (int x = startx - 1; x >= 0; x--)
                {
                    double currentValue = getData(x, y);
                    currentStep = shifts[x, y] = currentStep + FindAddition(previousValue, currentValue);
                    previousValue = currentValue;
                }
            }
        }

        protected void PinLowestZero()
        {
            int min_shift = int.MaxValue;
            foreach (int shift in shifts) if (shift<min_shift) min_shift = shift;

            for (int i = 0; i < shifts.GetLength(0); i++)
                for(int j = 0; j < shifts.GetLength(1); j++)
                    shifts[i, j] = shifts[i, j] - min_shift;
        }


        protected void CalculateResult()
        {
            for (int i = 0; i < shifts.GetLength(0); i++)
                for (int j = 0; j < shifts.GetLength(1); j++)
                {
                    result[i,j] = getData(i, j) + full_phase * shifts[i,j];
                }
        }

        protected void EliminateLines()
        {
            for (int y = 0; y < height - 1 ; y++)
            {
                bool[] errors = new bool[width];
                for (int x =0; x < width; x++)
                {
                    errors[x] = (FindAddition(getData(x, y), getData(x, y + 1)) != shifts[x, y] - shifts[x, y + 1]);
                }

                bool defectRegionFound = false;
                bool correctionApplied = false;
                int defectRegionLenght = 0;
                int defectRegionFirstIndex = -1;
                
                for (int x=0; x< width;x++)
                {
                    if (errors[x])
                    {
                        //обозначаем что нашли дефектный участок
                        defectRegionFound = true;
                        defectRegionLenght++;
                        if (defectRegionLenght == 1) defectRegionFirstIndex = x;
                    }
                    if (!errors[x] || x == width - 1) // если мы попали на бездефектный пиксель или дошли до конца
                    {
                        //если предыдущий пиксель был бездефектный то делать ничего не нужно
                        if (!defectRegionFound) continue;
                        //если мы соскочили с дефектной области, то надо проверить, кака€ у нее была длина
                        //если больше порога, то проводим корректировку
                        if (defectRegionLenght > PixelCountToFix)
                        {
                            correctionApplied = true;
                            for (int xx = defectRegionFirstIndex; xx <= x; xx++)
                            {
                                shifts[xx, y + 1] = shifts[xx, y] + FindAddition(getData(xx, y), getData(xx, y + 1));
                            }
                        }
                        defectRegionFound = false;
                        defectRegionLenght = 0;
                    }
                    //if (correctionApplied) y++;


                }





            }



        }




        public override double Measure(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}

