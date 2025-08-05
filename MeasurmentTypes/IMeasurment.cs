using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot;

namespace ImgAnalyzer.MeasurmentTypes
{
    delegate double MeasurePointDelegate(Point point);
    delegate double MeasurePolygonDelegate(Point[] points);
    delegate int MeasureLineDelegate(int line, int start_x, int count);

    delegate double CalculatePhaseDelegate(int x, int y, int n_frame, int intesity);


    public interface IMeasurment
    {

        IImageSource Source { get; }
        void BindImageStack(IImageSource source);

        string Name { get; set; }
        double Measure(ImageProcessor_1D processor);
        void Init();

        IMeasurment Clone();

    }
}
