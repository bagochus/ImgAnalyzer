using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScottPlot;
using ScottPlot.WinForms;


namespace ImgAnalyzer
{
    delegate void SaveFileDelegate(string filename);
    public partial class PlotForm : Form
    {
        readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        int n_points;
        int margin_right;
        int margin_down;

        SaveFileDelegate saveFile;

        private void GetSizes()
        {
            margin_right = this.Width - panel1.Width ;   
            margin_down = this.Height - panel1.Height ;
        }


        public PlotForm(string x_text)
        {
            InitializeComponent();
            GetSizes();
            panel1.Controls.Add(FormsPlot1);
            FormsPlot1.Plot.ShowLegend();
            ScottPlot.AxisPanels.BottomAxis bottomAxis = new ScottPlot.AxisPanels.BottomAxis()
            {
                LabelText = x_text,
            };
            FormsPlot1.Plot.Axes.Remove(FormsPlot1.Plot.Axes.Bottom);
            FormsPlot1.Plot.Axes.AddBottomAxis(bottomAxis);
            FormsPlot1.Refresh();

        }

        public void AddData(string name, double[] xdata, double[] ydata)
        {
            var sp = FormsPlot1.Plot.Add.Scatter(xdata, ydata);
            sp.LegendText = name;
            FormsPlot1.Refresh();

        }


        private void button_save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image (*.png)|*.png|All Files (*.*)|*.*";
                saveDialog.Title = "Сохранить как PNG";
                saveDialog.DefaultExt = "png"; // Автоматическое добавление .png, если не указано

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    FormsPlot1.Plot.SavePng(saveDialog.FileName, FormsPlot1.Width, FormsPlot1.Height);
                  
                }
            }

        }

        private void PlotForm_Resize(object sender, EventArgs e)
        {
            panel1.Width = this.Width - margin_right;
            panel1.Height = this.Height - margin_down;
        }

        private void button_csv_all_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV File (*.csv)|*.csv|All Files (*.*)|*.*";
                saveDialog.Title = "Сохранить как CSV";
                saveDialog.DefaultExt = "csv"; 

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    saveFile(saveDialog.FileName);

                }
            }
        }
    }
}
