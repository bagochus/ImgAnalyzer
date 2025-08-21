using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public class TiffImgFileHandler : I2DFileHandler
    {
        private int line = 0;
        Tiff tiff_img = null;
        ushort[] pixelData;
        byte[] buffer;

        int width;
        int height;
        int samplesPerPixel;
        double min;
        double max;
        bool min_calculated = false;
        bool max_calculated = false;

        private string filename;
        public string Name { get { return filename; } }

        ImageBatch source;
        private bool _disposed = false;

        public int Width {  get { return width; } }

        public int Height {  get { return height; } }

        public double GetPixelValue(int pixel)
        {
            if (pixel >= width || pixel < 0) throw new ArgumentException("Wrong x");
            if (pixelData == null) SelectLine(0);
            return pixelData[pixel];
        }

        public double GetPixelValue(int pixel, int line)
        {
            if (pixel >= width || pixel < 0) throw new ArgumentException("Wrong x");
            if (line != this.line) SelectLine(line);
                return this.pixelData[pixel];

        }

        public void LoadFile(IImageSource imageSource, string fileName)
        {
            if (! (imageSource is ImageBatch))
            {  throw new TypeAccessException("Wrong source type"); }
            source = imageSource as ImageBatch;
            tiff_img = Tiff.Open(fileName, "r");
            filename = fileName;

             width = source.Width;
             height = source.Height;
             samplesPerPixel = source.SamplesPerPixel;

            SelectLine(0);


        }

        public void LoadFile(IImageSource imageSource, int index)
        {
            if (!(imageSource is ImageBatch))
            { throw new TypeAccessException("Wrong source type"); }
            source = imageSource as ImageBatch;
            tiff_img = Tiff.Open(source.filenames[index], "r");

            filename = source.filenames[index];
            width = source.Width;
            height = source.Height;
            samplesPerPixel = source.SamplesPerPixel;
            SelectLine(0);


        }

        public void SelectLine(int line)
        {
            this.line = line;
            buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, line);
            pixelData = new ushort[width * samplesPerPixel];

            if (source.BitsPerSample == 16)
                System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            else
            {
                for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
            }

        }

        public ushort[] GetLine(int index)
        {
            SelectLine(index);
            return pixelData;
        }

        public double[] GetLineDouble(int index)
        {
            SelectLine(index);
            double[] result = new double[pixelData.Length];
            for (int i = 0; i < pixelData.Length; i++) result[i] = pixelData[i];
            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Отменяет вызов финализатора, если он есть
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                    tiff_img.Dispose();
                    pixelData = null;
                    buffer = null;
                }

                // Здесь можно освободить неуправляемые ресурсы (если есть)
                _disposed = true;
            }
        }

        public double Max()
        {
            if (max_calculated) return max;
            double result = double.MinValue;
            lock (tiff_img) 
            {
                for (int y = 0; y <height; y++)
                {
                    SelectLine(y);
                    for (int x = 0; x <width; x++)
                    if (pixelData[x] > result) result = pixelData[x];
                }
            }
            max = result; max_calculated = true;
            return result;
        }

        public double Min()
        {
            if (min_calculated) return min;
            double result = double.MaxValue;
            lock (tiff_img)
            {
                for (int y = 0; y < height; y++)
                {
                    SelectLine(y);
                    for (int x = 0; x < width; x++)
                        if (pixelData[x] < result) result = pixelData[x];
                }
            }
            min_calculated = true; min = result;    
            return result;
        }
        

        public int GetCount(double v1, double v2)
        {
            int result = 0;
            lock (tiff_img)
            {
                for (int y = 0; y < height; y++)
                {
                    SelectLine(y);
                    for (int x = 0; x < width; x++)
                        if (pixelData[x] >= v1 && pixelData[x] <= v2) result++;
                }
            }
            return result;
        }





        // Финализатор (на случай, если Dispose не вызвали)
        ~TiffImgFileHandler()
        {
            Dispose(false);
        }



    }
}
