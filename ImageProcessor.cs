using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;

namespace ImgAnalyzer
{
    internal class ImageProcessor
    {
        public Image image;
        public Tiff tiff_img;
        public void LoadImage(string filename)
        {
            image = Image.FromFile(filename);
            tiff_img = Tiff.Open(filename,"r");


        }

        public void MeasurePixel(int x,int y)
        {
            int width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            int bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            byte[] buffer = new byte[tiff_img.ScanlineSize()];
            tiff_img.ReadScanline(buffer, y);
            ushort[] pixelData = new ushort[width * samplesPerPixel];
            Buffer.BlockCopy(buffer, 0, pixelData, 0, buffer.Length);

        }



    }
}
