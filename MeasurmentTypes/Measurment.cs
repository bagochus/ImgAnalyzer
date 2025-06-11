using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.MeasurmentTypes
{
    public abstract class Measurment : IMeasurment
    {
        public ImageBatch Batch { get; set; }

        public string Name { get; set; }
        private ImageProcessor_1D _imageProcessor;

        public void BindIMGProcessorInstance(ImageProcessor_1D imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }
        public void BindImageStack(ImageBatch stack)
        {
            Batch = stack;
        }
        public abstract void Init();
        public abstract double Measure(ImageProcessor_1D processor);

        public abstract IMeasurment Clone();


    }
}
