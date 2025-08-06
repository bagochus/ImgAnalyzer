using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff.Classic;

namespace ImgAnalyzer
{
    internal class TiffImgFileHandler : I2DFileHandler
    {
        private int line = 0;
        Tiff tiff_img = null;
        ushort[] pixelData;
        byte[] buffer;

        int width;
        int height;
        int samplesPerPixel;

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

        public double GetPixelValue(int line, int pixel)
        {
            if (pixel >= width || pixel < 0) throw new ArgumentException("Wrong x");
            if (line != this.line) SelectLine(line);
                return this.pixelData[pixel];



            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            lock(tiff_img)
            {
                tiff_img.ReadScanline(buffer, line);
            }


            ushort[] pixelData = new ushort[width * samplesPerPixel];

            if (source.BitsPerSample == 16)
                System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            else
            {
                for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
            }
            return pixelData[pixel];

        }

        public void LoadFile(IImageSource imageSource, string fileName)
        {
            if (! (imageSource is ImageBatch))
            {  throw new TypeAccessException("Wrong source type"); }
            source = imageSource as ImageBatch;
            tiff_img = Tiff.Open(fileName, "r");

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

            width = source.Width;
            height = source.Height;
            samplesPerPixel = source.SamplesPerPixel;



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
            return result;
        }

        public double Min()
        {
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
