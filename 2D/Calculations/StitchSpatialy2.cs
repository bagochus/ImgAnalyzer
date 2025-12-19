using ImgAnalyzer;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    internal enum  BorderState {None, In, Out , Wall}
    internal enum CellState { Unmarked,  CurrentPass, Marked}
    
    public class StitchSpatially2 : Calculation2D
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
        private BorderState[,] h_borders;
        private BorderState[,] v_borders;
        private CellState[,] cellStates;
        private int[,] cellIndicies;
        private int currentRegion = 0;


        public StitchSpatially2()
        {
            pixbypix = false;
            this.description = "Проводит сшивку фазы по стыкам 0-360.\n" +
                " Альтернативный алгоритм. " +
                "thr - порог перехода через 0.";
            this.singeValueNames = new string[] { "thr"};
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

        private BorderState FindAdditionBS(double x1, double x2)
        {
            if (x1 < 0 || x1 > top || x2 < 0 || x2 > top)
                throw new ArgumentException("Phase out of range");

            if (x1 < thr && x2 > (top - thr)) return BorderState.Out;
            else if (x2 < thr && (x1 > (top - thr))) return BorderState.In;
            else return BorderState.None;
        }

        public override double[,] MeasureFull()
        {
            result = new double[width, height];
            shifts = new int[width, height];
            thr = SingleValueParameters[0];
            startx = (int)SingleValueParameters[1];
            starty = (int)SingleValueParameters[2];
            getData = (int x,int y) => ContainerParameters[0].ddata(x,y);

            h_borders = new BorderState[width - 1, height - 1];
            v_borders = new BorderState[width - 1, height - 1];
            cellStates = new CellState[width, height];
            cellIndicies = new int[width, height];  


            if (starty < 0 || starty > height || startx < 0 || startx > width) 
            {
                throw new ArgumentException("Начальная точка вне диапазона");
            }

            Actions();

            return result;
        }


        private void CalcBorders()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    h_borders[i, j] = FindAdditionBS(getData(i, j), getData(i, j + 1));
                    v_borders[i, j] = FindAdditionBS(getData(i, j), getData(i + 1, j));
                }
        }

        private BorderState BorderUp(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1) throw new ArgumentException();
            if (y == 1) return BorderState.Wall;
            return h_borders[x, y - 1];
        }

        private BorderState BorderDown(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1) throw new ArgumentException();
            if (y == height - 1) return BorderState.Wall;
            return h_borders[x, y + 1];
        }

        private BorderState BorderLeft(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1) throw new ArgumentException();
            if (x == 1) return BorderState.Wall;
            return h_borders[x - 1, y];
        }

        private BorderState BorderRight(int x, int y)
        {
            if (x < 0 || x > width - 1 || y < 0 || y > height - 1) throw new ArgumentException();
            if (x == width - 1) return BorderState.Wall;
            return h_borders[x + 1,y];
        }

        


        private void MarkArea(Point startPoint)
        {
            Stack<Point> stack = new Stack<Point>();
            stack.Push(startPoint);

            while (stack.Count > 0)
            {
                Point point = stack.Pop();
                int x = point.X;
                int y = point.Y;

                // Находим левую границу линии
                while (x > 0 && BorderLeft(x,y)==BorderState.None )
                {
                    x--;
                }
                //x++;

                bool spanAbove = false;
                bool spanBelow = false;

                // Сканируем линию
                while (x < width && BorderLeft(x, y) == BorderState.None && cellStates[x, y] == CellState.Unmarked)
                {
                    // Заменяем цвет
                    cellStates[x, y] = CellState.CurrentPass;

                    // Проверяем пиксель выше
                    if (y > 0)
                    {
                        if (!spanAbove && BorderUp(x,y) == BorderState.None && cellStates[x,y-1] == CellState.Unmarked)
                        {
                            stack.Push(new Point(x, y - 1));
                            spanAbove = true;
                        }
                        else if (spanAbove && (BorderUp(x,y) != BorderState.None || cellStates[x,y-1] != CellState.Unmarked))
                        {
                            spanAbove = false;
                        }
                    }

                    // Проверяем пиксель ниже
                    if (y < height -1)
                    {
                        if (!spanBelow && BorderDown(x, y) == BorderState.None && cellStates[x, y + 1] == CellState.Unmarked)
                        {
                            stack.Push(new Point(x, y + 1));
                            spanBelow = true;
                        }
                        else if (spanBelow && (BorderDown(x, y) != BorderState.None || cellStates[x, y - 1] != CellState.Unmarked))
                        {
                            spanBelow = false;
                        }
                    }

                    x++;
                }

                

            }


        }


        private void ProcessRegion()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                {
                    if (cellStates[i, j] == CellState.CurrentPass)
                    {
                        cellStates[i,j] = CellState.Marked;
                        cellIndicies[i, j] = currentRegion;
                    
                    }

                }
            }
            currentRegion++;
        }

        private bool FindNewStartPoint(out Point point)
        {
            point = new Point(-1, -1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (cellStates[i, j] == CellState.Unmarked)
                    {
                        point = new Point(i, j);
                        return true;

                    }

                }
            }
            return false;
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
                        //если мы соскочили с дефектной области, то надо проверить, какая у нее была длина
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

