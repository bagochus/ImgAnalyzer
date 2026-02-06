using ImgAnalyzer;
using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using NetTopologySuite.Geometries;
using SkiaSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Point = System.Drawing.Point;

namespace ImgAnalyzer._2D
{

    internal enum CellMark { Mid = 0, Lo, Hi };
    
    public class StitchSpatially3 : Calculation2D
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
        private int[,] tempData;
        private int[] region_shifts;
        private int biggest_region_index = -1;
        private int biggest_region_count = 0;
        private bool[] border_cells_cleared = null;
        List<Point> lu_points= new List<Point>();
        List<Point> rl_points= new List<Point>();   

        


        private int PixelCountToFix = 5;
        private CellMark[,] cellMarks;
        private CellState[,] cellStates;
        private CellState[,] cs_phase;
        private int[,] cellIndicies;
        private int currentRegion = 0;


        public StitchSpatially3()
        {
            pixbypix = false;
            this.description = "Проводит сшивку фазы по стыкам 0-360.\n" +
                "Еще один Альтернативный алгоритм. " +
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


        public override double[,] MeasureFull()
        {
            result = new double[width, height];
            shifts = new int[width, height];
            tempData = new int[width, height];
            thr = SingleValueParameters[0];
            //startx = (int)SingleValueParameters[1];
            //starty = (int)SingleValueParameters[2];
            getData = (int x,int y) => ContainerParameters[0].ddata(x,y);

            cellMarks = new CellMark[width, height];
            cellStates = new CellState[width, height];
            cellIndicies = new int[width, height];  


            if (starty < 0 || starty > height || startx < 0 || startx > width) 
            {
                throw new ArgumentException("Начальная точка вне диапазона");
            }

            Actions2();



            double[,] res2 = new double[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    res2[i, j] = (int)cellMarks[i, j];
            DataManager_2D.containers.Add(new Container_2D_double(res2) {Name = "marks" });

            double[,] res3 = new double[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    res3[i, j] = (int)tempData[i, j];
            DataManager_2D.containers.Add(new Container_2D_double(res3) { Name = "errors" });

            int[,] res4 = new int[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    res4[i, j] = cellIndicies[i, j];
            DataManager_2D.containers.Add(new Container_2D_int(res4) { Name = "inds" });


            return result;
        }


        protected virtual void Actions2()
        {
            CalcBorders();
            Point point = new Point(0, 0);

            while (FindNewStartPoint(out point))
            {
                MarkArea(point);
                ProcessRegion();

            }
            ArrangeShifths();
            ApplyShifts();
        }


        // эти функции необходимы для разбивки на области

        private void CalcBorders()
        {
            for (int i = 0; i < width ; i++)
                for (int j = 0; j < height ; j++)
                {
                    if (j < height - 1) 
                    {
                        if (FindAddition(getData(i, j), getData(i, j + 1)) == 1)
                        {
                            cellMarks[i, j] = CellMark.Hi;
                            cellMarks[i, j+1] = CellMark.Lo;
                        }
                        if (FindAddition(getData(i, j), getData(i, j + 1)) == -1)
                        {
                            cellMarks[i, j] = CellMark.Lo;
                            cellMarks[i, j+1] = CellMark.Hi;
                        } 
                    }
                    if (i < width - 1) 
                    {
                        if (FindAddition(getData(i, j), getData(i + 1, j)) == 1)
                        {
                            cellMarks[i, j] = CellMark.Hi;
                            cellMarks[i+1, j] = CellMark.Lo;
                        }

                        if (FindAddition(getData(i, j), getData(i + 1, j)) == -1)
                        {
                            cellMarks[i, j] = CellMark.Lo;
                            cellMarks[i+1, j] = CellMark.Hi;
                        } 
                    } 
                }
        }

        private void MarkArea(Point startPoint)
        {
            //"закрашивает" замкнутую область начиная с заданной точки
            //отмечает область тэгом "CurrentPass"
            
            Stack<Point> stack = new Stack<Point>();
            stack.Push(startPoint);
            cellStates[startPoint.X,startPoint.Y] = CellState.CurrentPass;

            Action<int, int, int, int> check_jump = (x1, y1, x2, y2) =>
            {
                if (cellStates[x2, y2] == CellState.Unmarked)
                    if (cellMarks[x1, y1] == CellMark.Mid || cellMarks[x1, y1] == cellMarks[x2, y2])
                    {
                        cellStates[x2, y2] = CellState.CurrentPass;
                        stack.Push(new Point(x2, y2));  
                    }
            };

            while (stack.Count > 0)
            {
                Point point = stack.Pop();

                int x = point.X;
                int y = point.Y;

                //up
                if (y > 0) check_jump(x, y, x, y - 1);
                //left
                if (x > 0) check_jump(x, y, x - 1, y);
                //down
                if (y < height - 1) check_jump(x, y, x, y + 1);
                //right
                if (x < width - 1) check_jump(x, y, x + 1, y);
            }
        }

        private void ProcessRegion()
        {
            // выставляет индекс на области, отмеченной как "СurrentPass"
            // находит левую верхнюю и нижнюю правую точки 
            Point lu = new Point(width, height);
            Point rl = new Point(0, 0);    


            int count=0;
            FindDisconnectedBorders();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) 
                {
                    if (cellStates[i, j] == CellState.CurrentPass)
                    {
                        if (i < lu.X) lu.X = i;
                        if (j < lu.Y) lu.Y = j;
                        if (i > rl.X) rl.X = i;
                        if (j > rl.Y) rl.Y = j;
                        cellStates[i,j] = CellState.Marked;
                        cellIndicies[i, j] = currentRegion;
                        count++;
                    }
                }
            }
            if (count > biggest_region_count)
            {
                biggest_region_count = count;
                biggest_region_index = currentRegion;
            }
            lu_points.Add(lu);
            rl_points.Add(rl);
            currentRegion++;
        }
       
        private void FindDisconnectedBorders()
        {
            // отмечает ошибочные участки - области разрыва 0-360 попавшие в одну область
            Action<int, int, int, int> check_jump = (x1, y1, x2, y2) =>
            {
                if (cellStates[x1, y1] != CellState.CurrentPass || cellStates[x2, y2] != CellState.CurrentPass) return;
                if (cellMarks[x1, y1] == CellMark.Mid || cellMarks[x2, y2] == CellMark.Mid) return;

                if (cellMarks[x1, y1] != cellMarks[x2, y2])
                {
                    tempData[x1, y1] = (int)cellMarks[x1,y1];
                    tempData[x2, y2] = (int)cellMarks[x2, y2];
                }
            };


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    if (i != width - 1) check_jump(i, j, i + 1, j);
                    if (j != height - 1) check_jump(i, j, i, j + 1);
                }
            }

        }
        private bool FindNewStartPoint(out Point point)
        {

            //находит стартовую точку для разметки области, не отмеченную тегом Marked
            //приоритет отдается не-граничным пикселям

            point = new Point(-1, -1);
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (cellStates[i, j] == CellState.Unmarked && cellMarks[i, j] == CellMark.Mid)
                    {
                        point = new Point(i, j);
                        return true;
                    }
                }
            }

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

        //эти функции необходимы для определения фазовых смещений



        private void ArrangeShifths()
        {
            
            border_cells_cleared = new bool[currentRegion];
            region_shifts = new int[currentRegion];
            for (int i = 0; i < region_shifts.Length; i++)
            {
                region_shifts[i] = int.MinValue;
                border_cells_cleared[i] = false;
            } 
            
            

            int index_in_work = biggest_region_index;
            region_shifts[biggest_region_index] = 0;
            int lowest_shift = int.MaxValue;


            bool all_regions_processed = false;
            while (!all_regions_processed)
            {
                FindShiftInSurroundingRegions(index_in_work);

                all_regions_processed=true;
                for (int i = 0;i < region_shifts.Length;i++)
                {
                    if (!border_cells_cleared[i] && region_shifts[i] != int.MinValue)
                    { 
                        all_regions_processed = false;
                        index_in_work = i;
                        break;
                    }
                }
            }

            for (int i = 0; i<region_shifts.Length;i++) 
                if (region_shifts[i] < lowest_shift) lowest_shift = region_shifts[i];   
            for (int i = 0; i < region_shifts.Length; i++)
                region_shifts[i] -= lowest_shift;



        }

        private void FindShiftInSurroundingRegions(int index)
        {
            //проходит по регону с индексом index, ищет соседние с ним регионы,
            //если там еще не выставлена фаза, выставляет

            Action<int, int, int, int> check_cells = (x1, y1, x2, y2) =>
            {
                if (cellIndicies[x1, y1] == index && cellIndicies[x2, y2] != index)
                    if (region_shifts[cellIndicies[x2, y2]] == int.MinValue)
                    { 
                        region_shifts[cellIndicies[x2, y2]] = region_shifts[index] + DetermineBorderState (index, cellIndicies[x2,y2]);
                    }
            };

            for (int i = lu_points[index].X; i <= rl_points[index].X; i++)
                for (int j = lu_points[index].Y; j <= rl_points[index].Y; j++)
                {
                    if (i != width - 1) check_cells(i, j, i + 1, j);
                    if (j != height - 1) check_cells(i, j, i, j + 1);
                    if (i != 0) check_cells(i, j, i - 1, j);
                    if (j != 0) check_cells(i, j, i, j - 1);
                }
            border_cells_cleared[index] = true;
        }
        

        private int DetermineBorderState(int index1, int index2)
        {
            // определяет характер границы между областями с индексами index1 и index2
            // возвращает 1 если область index1 имеет на границе фазу, близую к 360
            // возвращает -1 если область index1 имеет на границе фазу, близую к 0



            //количество границ, на которых index1 на верхнем пороге, index2 - на нижнем
            int count_plus =0;
            //количество границ, на которых index1 на нижнем пороге, index2 - на верхнем
            int count_minus=0;
            //if (index1 == 4 || index2 == 4) Debugger.Break();

            Action<int, int, int, int> check_cells = (x1, y1, x2, y2) => 
            {
                if (cellIndicies[x1, y1] == index1 && cellIndicies[x2, y2] == index2)
                {
                    if (cellMarks[x1, y1] == CellMark.Lo && cellMarks[x2, y2] == CellMark.Hi)
                    {
                        count_minus++;
                        return;
                    }
                    if (cellMarks[x1, y1] == CellMark.Hi && cellMarks[x2, y2] == CellMark.Lo)
                    {
                        count_plus++;
                        return;
                    }
                }
                else if (cellIndicies[x1, y1] == index2 && cellIndicies[x2, y2] == index1)
                {
                    if (cellMarks[x1, y1] == CellMark.Lo && cellMarks[x2, y2] == CellMark.Hi)
                    {
                        count_plus++;
                        return;
                    }
                    if (cellMarks[x1, y1] == CellMark.Hi && cellMarks[x2, y2] == CellMark.Lo)
                    {
                        count_minus++;
                        return;
                    }
                }
            };


            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (i < width - 1) check_cells(i, j, i + 1, j);
                    if (j< height -1) check_cells(i, j, i, j + 1);
                }
            }

            if (count_minus == count_plus && count_plus == 0) return 0;
            if (count_plus > count_minus) return 1;
            else return -1;
        }

        private void ApplyShifts()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    result[i, j] = getData(i,j) + top*region_shifts[cellIndicies[i, j]];

        }





        public override double Measure(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}

