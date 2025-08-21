using BitMiracle.LibTiff.Classic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace ImgAnalyzer
{
    



    public class ImageProcessor_1D
    {
        public Tiff tiff_img;
        bool container_mode = false;

        private I2DFileHandler hndl;


        public ImageProcessor_1D (string filename)
        {
            tiff_img = Tiff.Open(filename, "r");


        }

        public ImageProcessor_1D(I2DFileHandler hndl)
        {
            this.hndl = hndl;
            container_mode = true;

        }



        public double MeasurePixel(int x, int y)
        {
            if (container_mode) return hndl.GetPixelValue(x, y);


            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = 1;
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, y);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            if (bitsPerSample == 16)
                System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            else
            {
                for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
            }
            return pixelData[x];
        }

        public double Measure_PWM (PixelWeightMatrix pw)
        {
            double tempvalue = 0;
            for (int k = 0; k < pw.Count; k++)
            {
                tempvalue += pw.weights[k].weight * MeasurePixel(pw.weights[k].x, pw.weights[k].y);
            }
            return tempvalue;
        }

        public double MeasureLine(int line, int start_index, int count)
        {
            if (container_mode) return MeasureLineContainer(line, start_index, count);

            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = 1;
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, line);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            if (bitsPerSample == 16)
                System.Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);
            else
            {
                for (int i = 0; i < buffer.Length; i++) pixelData[i] = buffer[i];
            }
            int result = 0;
            for (int i = start_index; i < start_index + count; i++)
            {
                result += pixelData[i];
            }

            return result;
        }

        private double MeasureLineContainer(int line, int start_index, int count)
        {
            double result = 0;
            for (int i = start_index; i < start_index + count; i++)
            {
                result += hndl.GetPixelValue(i,line);
            }


            return result;

        }

        public void Dispose()
        {
            tiff_img.Dispose();
            if(hndl!=null) hndl.Dispose();  

        }






        #region Static functions
        public static bool IsPointInPolygon(PointF point, PointF[] polygon)
        {
            if (polygon == null || polygon.Length < 3)
                throw new ArgumentException("Polygon must have at least 3 ovelay_points");

            bool inside = false;
            int n = polygon.Length;

            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) &&
                    (point.X < (polygon[j].X - polygon[i].X)
                    * (point.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        #endregion


    }
}
