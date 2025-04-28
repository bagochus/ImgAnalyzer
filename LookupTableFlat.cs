using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace ImgAnalyzer
{

    internal class LookupTableFlat
    {
        private int max_read_buffer_size = 4096;
        private int max_buffer_size = 2000000000;

        private String filename;
        private int x_pixels;
        private int y_pixels;
        private int x_sectors;
        private int y_sectors;
        private int x_block;
        private int y_block;
        private int page_size;

        private short[] table;

        private void LoadFromFile(String filename)
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





    }
}
