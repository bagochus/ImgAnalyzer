using System;
using System.Collections.Generic;
using System.ComponentModel;


namespace ImgAnalyzer
{
    public class ImageManager
    {
        //private static readonly Lazy<ImageBatch[]> _stacks = new Lazy<ImageBatch[]>(()=> {return new ImageBatch[0];});
        public static ImageBatch[] Stacks;
        public static BindingList<ContainerBatch> containerBatches = new BindingList<ContainerBatch>();
        public static BindingList<IImageSource> imageSources
        {

            get
            {
                BindingList<IImageSource> _imageSources = new BindingList<IImageSource>();
                _imageSources.Add(Batch_A());
                _imageSources.Add(Batch_B());
                _imageSources.Add(Batch_C());
                foreach (var s in containerBatches) _imageSources.Add(s);
                return _imageSources;
            }
        }



        static ImageManager()
        {
            string[] names = new string[] {"A","B","C" };
            Stacks = new ImageBatch[3];

            //containerBatches = new BindingList<ContainerBatch>();
            for (int i = 0; i < Stacks.Length; i++)
            {
                Stacks[i] = new ImageBatch();
                Stacks[i].Name = names[i];  
                //imageSources.Add(Stacks[i]);
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

        public static bool IsCTDefined(IImageSource batch)
        { return batch.coordinateTransformation != null; }


        public static string GetUniqueSourceName(string baseName)
        {
            List<string> existingNames = new List<string>();
            foreach (var s in imageSources) existingNames.Add(s.Name);

            if (!existingNames.Contains(baseName))
            {
                return baseName;
            }

            int counter = 0;
            string newName;

            do
            {
                newName = $"{baseName}_{counter}";
                counter++;
            }
            while (existingNames.Contains(newName));

            return newName;
        }


    }
}
