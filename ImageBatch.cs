using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NetTopologySuite.Triangulate.QuadEdge;
using System.Runtime.Remoting.Messaging;
using BitMiracle.LibTiff.Classic;
using BitMiracle.LibTiff;


namespace ImgAnalyzer
{
    public delegate void EventHandler(object sender, EventArgs e);

    public class ImageManager
    {
        //private static readonly Lazy<ImageBatch[]> _stacks = new Lazy<ImageBatch[]>(()=> {return new ImageBatch[0];});
        public static ImageBatch[] Stacks;
        static ImageManager()
        {
            Stacks = new ImageBatch[3];
            for (int i = 0; i < Stacks.Length; i++)
            {
                Stacks[i] = new ImageBatch();
            }
                
        }

        public static ImageBatch Batch_A() { return Stacks[0]; }
        public static ImageBatch Batch_B() { return Stacks[1]; }
        public static ImageBatch Batch_C() { return Stacks[2]; }

        public static ImageBatch Batch(int n)
        {  return Stacks[n]; }

        public static int GetIndex(ImageBatch batch)
        {
            return Array.IndexOf(Stacks, batch);

        }

        public static string GetIndexLabel (ImageBatch batch)
        {
            string[] labels = { "A", "B", "C" };
            int index = Array.IndexOf(Stacks, batch);
            if (index < 0) return "X"; else 
                return labels[index];

        }

        public static int MaxCount()
        {
            int result = 0;
            foreach (ImageBatch batch in Stacks)  
                if (batch.Count > result) result = batch.Count;
            return result;
        }

        public static bool AllCTDefined()
        {
            bool result = true;
            foreach (ImageBatch batch in Stacks)
                result &= (batch.coordinateTransformation != null);
            return result;
        }

        public static bool IsCTDefined(ImageBatch batch)
        { return batch.coordinateTransformation != null; }



        


    }

    public class ImageBatch
    {
        private int width;
        public int Width {get {return width;}}
        private int height;
        public int Height {get {return height;}}
        private int samplesPerPixel;
        public int SamplesPerPixel {get {return samplesPerPixel;}}
        private int bitsPerSample;
        public int BitsPerSample {get {return bitsPerSample;}}



        public EventHandler DataChanged;
        public void LocateImageBatch(string[] filenames)
        {
            this.filenames = new List<string>(filenames);
            GetParameters(0);
            DataChanged?.Invoke(this, new EventArgs());
            
        }

        public List<string> filenames = new List<string>();
        public CoordinateTransformation coordinateTransformation {  get; set; }

        public int Count { get {  return filenames.Count; } }

        public bool FrameDefined()
        {
            return (coordinateTransformation != null);
        }



        private void GetParameters(int index)
        {
            var tiff_img = Tiff.Open(filenames[index], "r");
            width = tiff_img.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
            height = tiff_img.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            samplesPerPixel = 1;// tiff_img.GetField(TiffTag.SAMPLESPERPIXEL)[0].ToInt();
            bitsPerSample = tiff_img.GetField(TiffTag.BITSPERSAMPLE)[0].ToInt();

        }




    }
}
