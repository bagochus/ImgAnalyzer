using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{
    public enum Measurment2DTypes {Minimum, Maximum, Amplitude, Average}
    public class Measurment2D
    {
        public Measurment2DTypes Type { get; set; }
        public ImageBatch Batch { get; set; }
        public bool CalculateInFrame { get; set; }

        private int[,] dataint = new int[0,0];
        string name = string.Empty;

        private void ProcessIntValues()
        {

            if (Type == Measurment2DTypes.Minimum) dataint = ImageProcessor_2D.MinInt(Batch);
            if (Type == Measurment2DTypes.Maximum) dataint = ImageProcessor_2D.MaxInt(Batch);
            if (Type == Measurment2DTypes.Amplitude) dataint = ImageProcessor_2D.Amplitude(Batch);

            var dc = new Container_2D_int(dataint);
            dc.Name = ImageManager.GetIndexLabel(Batch) +"_"+ Type.ToString();
            dc.ImageGroup = ImageManager.GetIndexLabel(Batch) + "_" + Type.ToString();

        }

        public void ProcessMeasurment()
        {
            if (Type == Measurment2DTypes.Minimum ||
                Type == Measurment2DTypes.Maximum ||
                Type == Measurment2DTypes.Amplitude )
                ProcessIntValues();

            if (CalculateInFrame) name += "*";
            name += ImageManager.GetIndexLabel(Batch);

            if (CalculateInFrame)
            {
                double[,] datafloat = ImageProcessor_2D.FitData(dataint,Batch.coordinateTransformation);
                var dc = new Container_2D_double(datafloat);
                dc.Name = name + "_" + Type.ToString();
                dc.ImageGroup = ImageManager.GetIndexLabel(Batch) ;
                DataManager_2D.containers.Add(dc);
            }
            else
            {
                var dc = new Container_2D_int(dataint);
                dc.Name = name + "_" + Type.ToString();
                dc.ImageGroup = ImageManager.GetIndexLabel(Batch);
                DataManager_2D.containers.Add(dc);
            }




        }

    }
}
