using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ImgAnalyzer.LookupTables;
using ImgAnalyzer.MeasurmentTypes;
using System.Drawing;


namespace ImgAnalyzer.LookupTables
{
    public class LookupTableFlatHeader { }

    internal class LookupTableFlat : ILookupTable
    {
        private  LookupTableFlatHeader tableHeader;

        private int max_read_buffer_size = 4096;
        private int max_buffer_size = 2000000000;

        
        //размер обрабатываемого изображения - задается при инициализации
        private int x_pixels;
        private int y_pixels;

        //количество блоков - задается при инициализации
        private int x_sectors;
        private int y_sectors;

        //размер блока - расчитывается
        private int x_block;
        private int y_block;
        
        //количество блоков на изображении - расчитывается
        private int page_size;

        private short[] table;



        public Point[] corners;
        private int TopLeft = 0;
        private int TopRight = 1;
        private int BottomRight = 2;
        private int BottomLeft = 3;

        //делегаты 
        private MeasurePolygonDelegate measurePolygon;
        private MeasureLineDelegate measureLine;

        public LookupTableFlat(MainPresenter processor)
        {
           // measurePolygon = processor.MeasurePolygon;
            //measureLine = processor.MeasureLine;


        }

        private void LoadFromFile(String filename) // добавить обработку хидера
        {
            byte[] bytes = File.ReadAllBytes(filename);
            table = new short[bytes.Length / 2];
            Buffer.BlockCopy(bytes, 0, table, 0, bytes.Length);
        }

        private void CalculateBlockLength()
        {
            if (x_pixels % x_sectors == 0)  x_block = x_pixels / x_sectors; 
            else  x_block = (x_pixels / x_sectors) + 1;
            if (y_pixels % y_sectors == 0) y_block = y_pixels / y_sectors;
            else y_block = (y_pixels / y_sectors) + 1;
            page_size = x_sectors * y_sectors;
        }

        private void WriteToFile(String filename)
        {
            byte[] bytes = new byte[table.Length * sizeof(short)];
            Buffer.BlockCopy(table, 0, bytes, 0, bytes.Length);
            File.WriteAllBytes(filename, bytes);
        }

        private void AllocateTable()
        {
            table = new short[x_sectors * y_sectors * 256];
        }
       
        public void WriteValue(int x,int y, byte input, short value)
        {
            if (table == null) return;
            table[input*page_size+x+x_sectors*y] = value;
        }

        public short ReadValue(int x,int y,byte input)
        {
            int x_sector = x % x_block;
            int y_sector = y % y_block;
            if (x_sector >= x_pixels || y_sector >= y_pixels) return 0;
            return table[input * page_size + y_sector *x_sectors + x_sector];
        }

        public void ProcessImage(int n)
        {



        }




        private void ProcessImageFlatLUT(LookupTableFlat table, short value)
        {
            double kxi = (corners[TopLeft].X - corners[TopRight].X) / x_sectors;
            double kyi = (corners[TopLeft].Y - corners[TopRight].Y) / x_sectors;
            double kxj = (corners[TopLeft].X - corners[BottomLeft].X) / y_sectors;
            double kyj = (corners[TopLeft].Y - corners[BottomLeft].Y) / y_sectors;
            int x0 = corners[TopLeft].X;
            int y0 = corners[TopLeft].Y;

            for (int i = 0; i < x_sectors; i++)
            {
                for (int j = 0; j < y_sectors; j++)
                {
                    Point[] points = new Point[4];
                    points[TopLeft] = new Point(x0 + (int)(kxi * i) + (int)(kxj * j), y0 + (int)(kyi * i) + (int)(kyj * j));
                    points[TopRight] = new Point(x0 + (int)(kxi * (i + 1)) + (int)(kxj * j), y0 + (int)(kyi * (i + 1)) + (int)(kyj * j));
                    points[BottomLeft] = new Point(x0 + (int)(kxi * i) + (int)(kxj * (j + 1)), y0 + (int)(kyi * i) + (int)(kyj * (j + 1)));
                    points[BottomRight] = new Point(x0 + (int)(kxi * (i + 1)) + (int)(kxj * (j + 1)), y0 + (int)(kyi * (i + 1)) + (int)(kyj * (j + 1)));
                    //table.WriteValue(i, j, ConvertIntensity(MeanValueStrictBorders(ovelay_points)), value);
                }
            }
        }


    }
}
