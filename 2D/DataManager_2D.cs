
using ImgAnalyzer.DialogForms;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace ImgAnalyzer._2D
{
    public class DataManager_2D
    {

        public static BindingList<IContainer_2D> containers = new BindingList<IContainer_2D>();
        public static int workToBeDone = 0;
        public static int workDone = 0;
        public static IProgress<int> progress = new Progress<int>();
        public static List<Task> tasks = new List<Task>();
        public static void ShowMainForm()
        {
            Form form_2D = Application.OpenForms["Form_2D"];

            if (form_2D == null)
            {
                form_2D = new Form_2D();
                form_2D.Show();
            }
            else
            {
                form_2D.BringToFront();
                if (form_2D.WindowState == FormWindowState.Minimized)
                    form_2D.WindowState = FormWindowState.Normal;
                form_2D.Focus();
            }

        }

        public static void ShowCalcForm()
        {
            FormCalculations2D form = new FormCalculations2D();
            form.ShowDialog();

        }



        public static async void AddMeasurment()
        {
            AddMeasurment_2D addForm = new AddMeasurment_2D();
            addForm.ShowDialog();
            if (addForm.measurments.Count == 0) return;
            foreach (var m in addForm.measurments) 
                await m.ProcessMeasurment();
        }

        public static void PlotMap(int[] indicies)
        {
            foreach (var ind in indicies)
            {
                HeatMapForm form = new HeatMapForm(containers[ind]);
                form.Show();

            }
        }

        public static void PerformCalculation (ICalculation2D calculation)
        {
            if (calculation == null) return;
            if (!calculation.Check())
            {
                MessageBox.Show(calculation.ErrorMessage);
                return;
            }

            if (calculation.NonReturning)
            {
                calculation.Process();
            }
            else if (calculation.PixByPixCalculation)
            {
                double[,] datafloat = ImageProcessor_2D.PerformCalculation(calculation);
                var dc = new Container_2D_double(datafloat);
                dc.Name = calculation.ToString();
                dc.ImageGroup = "X";
                DataManager_2D.containers.Add(dc);

            }
            else 
            {
                double[,] datafloat = calculation.MeasureFull();
                var dc = new Container_2D_double(datafloat);
                dc.Name = calculation.ToString();
                dc.ImageGroup = "X";
                DataManager_2D.containers.Add(dc);


            }



        }

        public static void CalculateFullCT()
        {
            foreach (var ib in ImageManager.Stacks)
            {
                ib?.coordinateTransformation?.CalculateFullField();
            }


        }

        public static void SaveContainer(int containerIndex, string filePath)

        {
            if (containerIndex > containers.Count -1 ) return;
            if (containers[containerIndex] == null) return;
            containers[containerIndex].SaveToFile(filePath);


        }

        public static void DeleteContainer (int containerIndex)
        {
            if (containerIndex > containers.Count - 1) return;
            containers.RemoveAt(containerIndex);
        }

        public static void LoadContainer(string filename)
        {
            IContainer_2D container = Container_2D.ReadFromFile(filename);
            if (container != null) containers.Add(container);

        }

        public static void ClearContainers()
        { containers.Clear(); }


        public static void RenameContainer(int index, string newName)
        {
            containers[index].Name = newName;
            containers[index].Filename = "";
        }

    }
}
