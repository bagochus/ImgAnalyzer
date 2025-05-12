using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImgAnalyzer.MeasurmentTypes
{
    delegate double MeasurePointDelegate(Point point);
    delegate double MeasurePolygonDelegate(Point[] points);
    delegate int MeasureLineDelegate(int line, int start_x, int count);

    delegate double CalculatePhaseDelegate(int x, int y, int n_frame, int intesity);


    public interface IMeasurment
    {
        string Name { get; set; }

        int DataCount { get; set; }
        List<double>[] RetrieveData();
        void Measure();
        void ClearData();
        void MeasurePhase(int n_frame);
        List<double>[] RetrievePhaseData();


    }
}
