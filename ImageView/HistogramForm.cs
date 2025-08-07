using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public partial class HistogramForm : Form
    {
        readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        public HistogramForm(string Header, double[] xdata, int[] ydata)
        {
            InitializeComponent();
            panel1.Controls.Add(FormsPlot1);
            var bars1 = FormsPlot1.Plot.Add.Bars(xdata, ydata);
            bars1.LegendText = Header;




        }


        public HistogramForm(string Header, double[] xdata, int[] ydata,float width)
        {
            InitializeComponent();
            panel1.Controls.Add(FormsPlot1);
            var bars1 = FormsPlot1.Plot.Add.Bars(xdata, ydata);
            foreach (var bar in bars1.Bars) bar.Size = width;
            bars1.LegendText = Header;



        }
    }
}
