using ImgAnalyzer.MeasurmentTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public enum VariablesToMap : int { Minimum = 0, Maximum, Amplitude, Pseudophase, DeadAlive,
        A_values, B_values, NA_peaks, VA_peaks, NB_peaks, VB_peaks,
    }
    public partial class MainPresenter
    {











        #region MeasurmentListManagement
        public List<string> GetMeasurnetNames()
        {
            List<string> list = new List<string>();
            for (int i = 0; i < measurements.Count; i++)
            {
                list.Add(measurements[i].Name);
            }
            return list;
        }
        public void AddPointMeasurment(Point point)
        {
           // measurements.Add(new PointMeasurment(this, PointFrame));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void AddPolygonMeasurment(Point[] points)
        {
            if (points.Length < 3) return;
          //  measurements.Add(new PolygonMeasurment(this, ovelay_points));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }
        public void AddNewMatrixMeasurnment(Point[] corners, int nx, int ny)
        {
            if (corners.Length != 4) return;
          //  measurements.Add(new MatrixMeasurment(this, corners, nx, ny));
            ListUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void RenameItem(int index, string newName)
        {
            try
            {
                measurements[index].Name = newName;
                ListUpdated(this, EventArgs.Empty);
            }
            catch { }
        }

        public void DeleteItem(int index)
        {
            try
            {
                measurements.RemoveAt(index);
                ListUpdated(this, EventArgs.Empty);
            }
            catch { }
        }
        /*
        public void SaveMeasurmentList(string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true,
                Converters = { new JsonInterfaceConverter<IMeasurment>() }
            };
            string json = JsonSerializer.Serialize(measurements, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filename, json);
        }

        public void LoadMeasurmentList(string filename)
        {

            try
            {
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonInterfaceConverter<IMeasurment>() }
                };
                string jsonString = File.ReadAllText(filename);
                IMeasurment[] arr_measurements = JsonSerializer.Deserialize<IMeasurment[]>(jsonString, options);
                measurements.Clear();
                foreach (var measure in arr_measurements) measurements.Add(measure);
                ListUpdated(this, EventArgs.Empty);
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Файл не найден!");
            }
            catch (JsonException)
            {
                MessageBox.Show("Ошибка в формате JSON!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}");
            }


        } */
        #endregion
       


        /*
        public void SaveCSV_All(string filename)
        {
            if (measurements.Count == 0) return;
            int rows = measurements[0].DataCount;
            int cols = measurements.Count;

            var lines = new string[rows + 1];
            lines[0] = "";
            for (int i = 0; i < measurements.Count; i++)
            {
                lines[0] += measurements[i].Name;
                if (i < measurements.Count - 1) lines[0] += ",";

            }
            for (int i = 0; i < rows; i++)
            {
                // Преобразуем строку массива в строку CSV
                var rowValues = new string[cols];
                for (int j = 0; j < cols; j++)
                {
                   // List<double>[] data2d = measurements[j].RetrieveData();
                   // double[] data = data2d[0].ToArray();
                 //   rowValues[j] = data[i].ToString(System.Globalization.CultureInfo.InvariantCulture);
                }
                lines[i + 1] = string.Join(",", rowValues);
            }

            File.WriteAllLines(filename, lines);



        }
        */

        public void PlotValueMap(VariablesToMap variable)
        {
            switch (variable)
            {
                case VariablesToMap.Amplitude:
                    if (amplitude_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(amplitude_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "Amplitude";
                    }
                    else goto default;
                    break;
                case VariablesToMap.Maximum:
                    if (max_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(max_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "MAX";
                    }
                    else goto default;
                    break;
                case VariablesToMap.Minimum:
                    if (min_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(min_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "MIN";
                    }
                    else goto default;
                    break;
                case VariablesToMap.Pseudophase:
                    if (pseudo_phase_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(pseudo_phase_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "Pseudophase";
                    }
                    else goto default;
                    break;
                case VariablesToMap.DeadAlive:
                    if (good_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(good_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "Dead/Alive";
                    }
                    else goto default;
                    break;
                case VariablesToMap.A_values:
                    if (a_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(a_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "A";
                    }
                    else goto default;
                    break;
                case VariablesToMap.B_values:
                    if (b_values != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(b_values);
                        thermalMapForm.Show();
                        thermalMapForm.Text = "B";
                    }
                    else goto default;
                    break;
                case VariablesToMap.NA_peaks:
                    if (a_peaks != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(a_peaks);
                        thermalMapForm.Text = "NA_peaks";
                        thermalMapForm.Show();
                    }
                    else goto default;
                    break;
                case VariablesToMap.VA_peaks:
                    if (ax_positions?.Count != 0)
                    {
                        for (int i = 0; i < ax_positions.Count; i++)
                        { 
                        ThermalMapForm thermalMapForm = new ThermalMapForm(ax_positions[i]);
                        thermalMapForm.Text = "VA " + i.ToString();
                        thermalMapForm.Show();
                        }
                    }
                    else goto default;
                    break;
                case VariablesToMap.NB_peaks:
                    if (a_peaks != null)
                    {
                        ThermalMapForm thermalMapForm = new ThermalMapForm(b_peaks);
                        thermalMapForm.Text = "NB_peaks";
                        thermalMapForm.Show();
                    }
                    else goto default;
                    break;
                case VariablesToMap.VB_peaks:
                    if (ax_positions?.Count != 0)
                    {
                        for (int i = 0; i < ax_positions.Count; i++)
                        {
                            ThermalMapForm thermalMapForm = new ThermalMapForm(bx_positions[i]);
                            thermalMapForm.Text = "VB " + i.ToString();
                            thermalMapForm.Show();
                        }
                    }
                    else goto default;
                    break;
                default:
                    MessageBox.Show("Невозможно построить эту карту, возможно данные еще не готовы");
                    break;




            }


        }





    }
}
