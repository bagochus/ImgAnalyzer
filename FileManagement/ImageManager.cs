using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace ImgAnalyzer
{
    public class ImageManager
    {
        //private static readonly Lazy<ImageBatch[]> _stacks = new Lazy<ImageBatch[]>(()=> {return new ImageBatch[0];});
        public static ImageBatch[] Stacks;
        public static BindingList<ContainerBatch> containerBatches;
        public static BindingList<IImageSource> imageSources;


        static ImageManager()
        {
            Stacks = new ImageBatch[3];
            containerBatches = new BindingList<ContainerBatch>();
            for (int i = 0; i < Stacks.Length; i++)
            {
                Stacks[i] = new ImageBatch();
                imageSources.Add(Stacks[i]);
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
}
