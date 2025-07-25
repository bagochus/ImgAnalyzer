using ImgAnalyzer._2D.Measurments;
using Microsoft.VisualBasic;
using OpenTK.Graphics.ES11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D
{
    public enum Measurment2DTypes {Minimum, Maximum, Amplitude, Index, PeakMinimum, PeakMaximum}
    public class Measurment2D
    {
        public Measurment2DTypes Type { get; set; }
        public ImageBatch Batch { get; set; }
        public bool CalculateInFrame { get; set; }

        public int index;

        private bool error = false;




        private int[,] dataint = new int[0,0];
        string name = string.Empty;

        private void ProcessIntValues()
        {

            if (Type == Measurment2DTypes.Minimum) dataint = ImageProcessor_2D.MinInt(Batch);
            if (Type == Measurment2DTypes.Maximum) dataint = ImageProcessor_2D.MaxInt(Batch);
            if (Type == Measurment2DTypes.Amplitude) dataint = ImageProcessor_2D.Amplitude(Batch);
            if (Type == Measurment2DTypes.Index) dataint = ImageProcessor_2D.Index(Batch,index);

            var dc = new Container_2D_int(dataint);
            dc.Name = ImageManager.GetIndexLabel(Batch) +"_"+ Type.ToString();
            dc.ImageGroup = ImageManager.GetIndexLabel(Batch) + "_" + Type.ToString();

        }


        private void ProcessExtremum()
        {




            var extSearch = new ExtremumSearch();
            if (Type == Measurment2DTypes.PeakMaximum)
            {
                int n_peak = 0;
                double threshold = 1000;
                int startpos = 0;
                error &= !AskIntValue(out startpos, "Введите начальный индекс", "Поиск экстремумов", "0");
                error &= !AskIntValue(out n_peak, "Введите номер пика (0 - первый)", "Поиск экстремумов", "0");
                error &= !AskDoubleValue(out threshold, "Введите порог различения экстремума", "Поиск экстремумов", "1000");
                if (error)
                {
                    MessageBox.Show("Ошибка ввода");
                    return;
                }

                dataint = extSearch.SearchPeak(Batch, n_peak, false, threshold,startpos);
            }

            if (Type == Measurment2DTypes.PeakMinimum)
            {
                int n_peak = 0;
                double threshold = 1000;
                int startpos = 0;
                error &= !AskIntValue(out startpos, "Введите начальный индекс", "Поиск экстремумов", "0");
                error &= !AskIntValue(out n_peak, "Введите номер пика (0 - первый)", "Поиск экстремумов", "0");
                error &= !AskDoubleValue(out threshold, "Введите порог различения экстремума", "Поиск экстремумов", "1000");
                if (error)
                {
                    MessageBox.Show("Ошибка ввода");
                    return;
                }

                dataint = extSearch.SearchPeak(Batch, n_peak, true, threshold, startpos);
            }

        }



        public async Task  ProcessMeasurment()
        {
            if (Type == Measurment2DTypes.Minimum ||
                Type == Measurment2DTypes.Maximum ||
                Type == Measurment2DTypes.Amplitude ||
                Type == Measurment2DTypes.Index)
                await Task.Run(()=> ProcessIntValues());

            if (Type == Measurment2DTypes.PeakMinimum ||
                    Type == Measurment2DTypes.PeakMaximum)
                    await Task.Run(()=> ProcessExtremum());
            if (error) return;


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


        public bool AskIntValue(out int value,string promt, string title, string default_value)
        {
            //int value = -1;
            string userInput = Interaction.InputBox(promt,
                   title,
                   default_value);
            
            return Int32.TryParse(userInput, out value);;

        }
        public bool AskDoubleValue(out double value, string promt, string title, string default_value)
        {
            //int value = -1;
            string userInput = Interaction.InputBox(promt,
                   title,
                   default_value);

            return Double.TryParse(userInput, out value); ;

        }




    }
}
