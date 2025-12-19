using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using NetTopologySuite.Triangulate.QuadEdge;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ImgAnalyzer
{
    public delegate void EventHandler(object sender, EventArgs e);


    public class ImageBatch : IImageSource
    {
        private int width;
        public int Width {get {return width;}}
        private int height;
        public int Height {get {return height;}}
        private int samplesPerPixel;
        public int SamplesPerPixel {get {return samplesPerPixel;}}
        private int bitsPerSample;
        public int BitsPerSample {get {return bitsPerSample;}}

        public int Count { get {  return filenames.Count; } }

        public List<string> filenames = new List<string>();
        public CoordinateTransformation coordinateTransformation {  get; set; }

        public string Name { get; set; }

        public EventHandler DataChanged;
        public void LocateImageBatch(string[] filenames)
        {
            this.filenames = new List<string>(filenames);
            GetParameters(0);
            CheckAllFiles();
            
            DataChanged?.Invoke(this, new EventArgs());
            if (bitsPerSample == 8) MessageBox.Show("Данные в 8-битном формате");
            
        }


        public bool FrameDefined()
        {
            return (coordinateTransformation != null);
        }

        private void GetParameters(int index)
        {
            var tiff_img = Tiff.Open(filenames[index], "r");
            width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            try
            { samplesPerPixel =  tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt(); }
            catch { samplesPerPixel = 1; }
            // tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

        }

        private bool CheckParamenters(int index)
        {

            var tiff_img = Tiff.Open(filenames[index], "r");
            int _width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            int _height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            int _samplesPerPixel = 1;
            try
            { _samplesPerPixel = tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt(); }
            catch { _samplesPerPixel = 1; }
            
            int _bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

            return (width == _width) &&
                (height == _height) &&
                (bitsPerSample == _bitsPerSample) &&
                (samplesPerPixel == _samplesPerPixel);

        }

        private void CheckAllFiles()
        {


            for (int i = 0; i < filenames.Count; i++)
            {
                if (!CheckParamenters(i))
                {
                    DialogResult dialogResult = MessageBox.Show("Файл " + filenames[i] + 
                        " не соответсвтует остальным файлам. Исключить его из набора?"
                        , "Some Title", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        filenames.Remove(filenames[i]);
                    }
                }



            }


        }

        public I2DFileHandler Get2DFileHandler(int index)
        {
            if (index >= Count || index <0) throw new ArgumentOutOfRangeException("index");
            var hndl = new TiffImgFileHandler();
            hndl.LoadFile(this, index);
            return hndl;

        }

        public I2DFileHandler Get2DFileHandler(string filename)
        {
            if (!filenames.Contains(filename))
                throw new Exception("No such image in batch");

            var hndl = new TiffImgFileHandler();
            hndl.LoadFile(this, filename);  
            return hndl;

        }








    }
}
