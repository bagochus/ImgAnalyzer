using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    internal class StitchFramewise : IGroupOperation
    {
        //-------------interface properties-----------------------------
        public string Description
        {
            get
            {
                return "Проводит сшивку пакета фазовых изображений по границе 0-360.\n" +
                " на основе первого кадра, прошедшего сшивку по пространству\n" +
                "thr - порог определения пересечения нуля";
            }
        }

        public double[] SingleValueParameters { get; set; }

        string[] singeValueNames = new string[] { "thr" };
        public string[] SingleValueNames { get { return singeValueNames; } }

        public IContainer_2D[] ContainerParameters { get; set; }

        public string[] ContainerNames { get { return new string[] {"First frame" }; } }

        public IImageSource[] imageSources { get; set; }

        private string[] imgSourceNames = { "Input batch" };
        public string[] imageSourceNames { get { return imgSourceNames; } }

        public bool UseTransformation { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int width, height;

        private double thr;
        private double top = 360;
        private double[,] phase;
        private double[,] phase_prev;

        Func<int, int, double> getDataPrev;
        Func<int, int, double> getDataNext;

        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }

            thr = SingleValueParameters[0];
            I2DFileHandler hndlPrev = null, hndlNext = null;


            ContainerBatch batch = new ContainerBatch();
            batch.Name = ImageManager.GetUniqueSourceName("PhaseStitch");
            ImageManager.containerBatches.Add(batch);

            DataManager_2D.workToBeDone += imageSources[0].Count;

            string foldername = FileManagement.CreateUniqueFolder("D:\\containers\\" + batch.Name);


            string filename = Path.Combine(foldername, "0.bin");
            ContainerParameters[0].SaveToFile(filename);
            batch.Filenames.Add(filename);

            for (int i = 1; i < imageSources[0].Count; i++)
            {

                if (i == 1)
                {
                    getDataPrev = (int x, int y) => ContainerParameters[0].ddata(x, y);
                }
                else
                {
                    getDataPrev = (int x, int y) => phase_prev[x,y];
                }
                hndlNext = imageSources[0].Get2DFileHandler(i);
                getDataNext = hndlNext.GetPixelValue;

                //await Task.Run(() =>
                //{
                    GeneratePhaseImage();
                    Container_2D_double c = new Container_2D_double(phase);
                    DataManager_2D.progress.Report(1);

                    filename = Path.Combine(foldername, i.ToString() + ".bin");
                    c.SaveToFile(filename);
                    batch.Filenames.Add(filename);

                //});

                //hndlPrev = hndlNext;
                hndlNext.Dispose();


            }

        }

        private int FindAddition(double x1, double x2)
        {
            if (x1 < 0 || x1 > top || x2 < 0 || x2 > top)
                throw new ArgumentException("Phase out of range");

            if (x1 < thr && x2 > (top - thr)) return -1;
            else if (x2 < thr && (x1 > (top - thr))) return 1;
            else return 0;
        }

        private void GeneratePhaseImage()
        {

            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                {
                    double phaseCalculatedPrev = getDataPrev(i,j);
                    double phaseMeasuredPrev = phaseCalculatedPrev % top;
                    int periodsPrev = (int)(phaseCalculatedPrev / top);

                    double phaseMeasuredNext = getDataNext(i, j);

                    int addition = FindAddition(phaseMeasuredPrev, phaseMeasuredNext);

                    phase_prev[i, j] = phase[i, j] = phaseMeasuredNext + top * (addition + periodsPrev);
                    
                }
        }



        private bool Check()
        {
           

           

            

            
            width = imageSources[0].Width;
            height = imageSources[0].Height;


            bool result = ( imageSources[0].Width == ContainerParameters[0].Width );
            result &= (imageSources[0].Height == ContainerParameters[0].Height);
            if (!result)
            {
                error_message = "Размеры активных областей не совпадают";
                return false;

            }
            phase = new double[imageSources[0].Width, imageSources[0].Height];
            phase_prev = new double[imageSources[0].Width, imageSources[0].Height];

            return result;





        }






    }
}
