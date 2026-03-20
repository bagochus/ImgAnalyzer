using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public string UserComment { get; set; }

        public int SampleId { get; set; }

        //-------------local----------------------------------------------

        string error_message = "";
        int width, height;

        private double thr;
        private double top = 360;
        private double[,] phase;
        private double[,] phase_prev;

        Func<int, int, double> getDataPrev;
        Func<int, int, double> getDataNext;

        private int lowerPeriod = 0;

        //---------------output variables for internal call---------------

        public int total_containers = 0;
        public int processed_containers = 0;
        public event Action containerPorcessed = () => { };
        public CancellationToken _cancellationToken;

        public ContainerBatch batch;

        public int id;

        public bool internal_call = false;



        public async Task Execute()
        {
            if (!Check())
            {
                MessageBox.Show(error_message); return;
            }

            thr = SingleValueParameters[0];
            I2DFileHandler hndlNext = null;


            batch = new ContainerBatch();
            batch.Name = ImageManager.GetUniqueSourceName("PhaseStitch");
            batch.BatchType = BatchDatatypes.PhaseUnwrapped;

            batch.comment += "Развернутая (сшитая) фаза\n";
            batch.comment += $"Cоздано {DateTime.Now}\n";
            if (SampleId > 0) batch.comment += $"Образец: {SamplesDB.GetSampleName(SampleId)}\n";
            if (UserComment?.Length > 0) batch.comment += UserComment + Environment.NewLine;

            if (imageSources[0] is ContainerBatch)
                if ((imageSources[0] as ContainerBatch).comment?.Length > 0)
                    batch.comment += "Phase measurment data: " + ((imageSources[0] as ContainerBatch).comment) + $"\n";

            id = SamplesDB.AddContainerBatch(batch);

            //ImageManager.containerBatches.Add(batch);


            DataManager_2D.workToBeDone += imageSources[0].Count;

            batch.AddContainer(ContainerParameters[0], true);

            total_containers = imageSources[0].Count;

            //начинаем с 1, так как первый кадр - исходная фаза
            for (int i = 1; i < imageSources[0].Count; i++)
            {

                if (_cancellationToken.IsCancellationRequested)
                {                    
                    SamplesDB.AppendBatchCommentNewLine(id, 
                        "Расчет был прерван до завершения!!!\n"+
                        "Данные могут быть некорректны!!!");
                    _cancellationToken.ThrowIfCancellationRequested();
                }

                if (i == 1)
                {
                    getDataPrev = (int x, int y) => ContainerParameters[0].ddata(x, y);
                }
                else
                {
                    getDataPrev = (int x, int y) => phase_prev[x, y];
                }
                hndlNext = imageSources[0].Get2DFileHandler(i);
                getDataNext = hndlNext.GetPixelValue;

                await Task.Run(() =>
                {
                    GeneratePhaseImage();
                    Container_2D_double c = new Container_2D_double(phase);
                    DataManager_2D.progress.Report(1);
                    batch.AddContainer(c);

                });

                SamplesDB.UpdateContainerBatch(id, batch);

                hndlNext.Dispose();
                processed_containers++;
                containerPorcessed();
            }
            CorrectPhaseShift(batch);
        }

        private int FindAddition(double x1, double x2)
        {
            if (x1 < 0 || x1 > top || x2 < 0 || x2 > top)
            {
                Debugger.Break();
                throw new ArgumentException("Phase out of range");
            }    


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
                    double phaseMeasuredPrev = 0;

                    if (phaseCalculatedPrev < 0)
                    {
                        phaseMeasuredPrev += (int)Math.Ceiling(-phaseCalculatedPrev / top) * top;
                    }
                    else 
                    {
                        phaseMeasuredPrev = phaseCalculatedPrev % top;
                    }
                    int periodPrev = (int)Math.Floor(phaseCalculatedPrev / top);
                    if (periodPrev < lowerPeriod) lowerPeriod = periodPrev;

                    double phaseMeasuredNext = getDataNext(i, j);

                    int addition = FindAddition(phaseMeasuredPrev, phaseMeasuredNext);

                    phase_prev[i, j] = phase[i, j] = phaseMeasuredNext + top * (addition + periodPrev);

                    
                }
        }


        private void CorrectPhaseShift(ContainerBatch batch)
        {
            processed_containers = 0;
            for (int n = 0; n<batch.Count;n++)
            {

                if (_cancellationToken.IsCancellationRequested)
                {
                    //some comment actions
                    _cancellationToken.ThrowIfCancellationRequested();
                }

                ContainerFileHandler hndl = batch.Get2DFileHandler(n) as  ContainerFileHandler;
                for (int i = 0; i< hndl.Width; i++) 
                    for (int j = 0;j< hndl.Height; j++)
                    {
                        hndl.SetPixel(i,j, hndl.GetPixelValue(i,j) - top * lowerPeriod);
                    }
                hndl.Save();
                hndl.Dispose();
                processed_containers++;
                containerPorcessed();
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
