using ImgAnalyzer.DialogForms;
using ScottPlot;
using ScottPlot.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;


namespace ImgAnalyzer
{
    delegate void SaveFileDelegate(string filename);
    public partial class PlotForm : Form
    {
        readonly FormsPlot FormsPlot1 = new FormsPlot() { Dock = DockStyle.Fill };

        int n_points;
        int margin_right;
        int margin_down;

        private List<double[]> xdatas = new List<double[]>();
        private List<double[]> ydatas = new List<double[]>();
        private List<string> names = new List<string>();

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
            names.Add(name);
            xdatas.Add(xdata);
            ydatas.Add(ydata);
            var sp = FormsPlot1.Plot.Add.Scatter(xdata, ydata);
            sp.LegendText = name;
            FormsPlot1.Refresh();

        }

        private void SendPlots()
        {
            OpenedPlotForm form = new OpenedPlotForm();
            form.ShowDialog();
            if (form.selectedForm == null) return;
            if (xdatas.Count != ydatas.Count || xdatas.Count != names.Count)
            {
                MessageBox.Show("Data arrays is not consistent");
                return;
            }
            for (int i = 0; i < xdatas.Count; i++)
            {
                form.selectedForm.AddData(this.Text+ ":" + names[i], xdatas[i], ydatas[i]);
            }
            



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

        private void button_send_Click(object sender, EventArgs e)
        {
            SendPlots();
        }

        private void button_csvsel_Click(object sender, EventArgs e)
        {
            SaveChartsToCsv();
        }



        private void SaveChartsToCsv()
        {
            // Проверка, что есть данные для сохранения
            if (xdatas.Count == 0 || ydatas.Count == 0 || names.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка согласованности данных
            if (xdatas.Count != ydatas.Count || xdatas.Count != names.Count)
            {
                MessageBox.Show("Количество наборов данных X, Y и имен не совпадает.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Диалог для выбора файла
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Сохранить графики как CSV файл";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            // Записываем заголовок с именами графиков
                            writer.WriteLine(string.Join(",", names.Select(name => $"{name}_X,{name}_Y")));

                            // Находим максимальную длину среди всех наборов данных
                            int maxLength = xdatas.Max(x => x.Length);

                            // Записываем данные построчно
                            for (int i = 0; i < maxLength; i++)
                            {
                                List<string> lineParts = new List<string>();

                                for (int j = 0; j < xdatas.Count; j++)
                                {
                                    // Добавляем X и Y значения для каждого графика
                                    string xValue = i < xdatas[j].Length ? xdatas[j][i].ToString(CultureInfo.InvariantCulture) : "";
                                    string yValue = i < ydatas[j].Length ? ydatas[j][i].ToString(CultureInfo.InvariantCulture) : "";

                                    lineParts.Add(xValue);
                                    lineParts.Add(yValue);
                                }

                                writer.WriteLine(string.Join(",", lineParts));
                            }
                        }

                        MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void SaveChartsToTxt()
        {
            // Проверка, что есть данные для сохранения
            if (xdatas.Count == 0 || ydatas.Count == 0 || names.Count == 0)
            {
                MessageBox.Show("Нет данных для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка согласованности данных
            if (xdatas.Count != ydatas.Count || xdatas.Count != names.Count)
            {
                MessageBox.Show("Количество наборов данных X, Y и имен не совпадает.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Диалог для выбора файла
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "TXT файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.Title = "Сохранить графики как TXT файл";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                        {
                            // Записываем заголовок с именами графиков
                            writer.WriteLine(string.Join("\t", names.Select(name => $"{name}_X,{name}_Y")));

                            // Находим максимальную длину среди всех наборов данных
                            int maxLength = xdatas.Max(x => x.Length);

                            // Записываем данные построчно
                            for (int i = 0; i < maxLength; i++)
                            {
                                List<string> lineParts = new List<string>();

                                for (int j = 0; j < xdatas.Count; j++)
                                {
                                    // Добавляем X и Y значения для каждого графика
                                    string xValue = i < xdatas[j].Length ? xdatas[j][i].ToString() : "";
                                    string yValue = i < ydatas[j].Length ? ydatas[j][i].ToString() : "";

                                    lineParts.Add(xValue);
                                    lineParts.Add(yValue);
                                }

                                writer.WriteLine(string.Join("\t", lineParts));
                            }
                        }

                        MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void button_savetxt_Click(object sender, EventArgs e)
        {
            SaveChartsToTxt();
        }
    }
}
