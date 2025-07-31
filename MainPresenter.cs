using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using ImgAnalyzer.DialogForms;
using ImgAnalyzer.MeasurmentTypes;
using Microsoft.VisualBasic;
using OpenTK.Graphics.OpenGL;
using ScottPlot.Interactivity.UserActions;
using ScottPlot.PlotStyles;
using ScottPlot.Plottables;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public enum State : byte { Rising, Falling, Unknown };

    public enum NearestExtremum : byte { A, B, Undef };
    public enum Vert : int { TopLeft = 0, TopRight, BottomRight, BottomLeft };




    delegate void ListUpdatedDelegate();
    delegate void LoadFileListDeleggate(string[] filenames);
    public partial class MainPresenter
    {
        public Image image;
        public Tiff tiff_img;


        public string[] filenames;


        public Point[] corners;
        public int x_sectors;
        public int y_sectors;

        public List<IMeasurment> measurements = new List<IMeasurment>();
        public EventHandler ListUpdated;

        // usage of indexes - [y,x]
        /*
        private int[,] max_values;
        private int[,] min_values;
        private int[,] amplitude_values;
        private int[,] good_values;
        private int[,] pseudo_phase_values;
        private int[,] a_values;
        private int[,] peak_values;
        private List<int[,]> ax_positions = new List<int[,]>();
        private List<int[,]> ay_positions = new List<int[,]>();
        private int[,] a_peaks;
        private List<int[,]> bx_positions = new List<int[,]>();
        private List<int[,]> by_positions = new List<int[,]>();
        private int[,] b_peaks;
        */
        //test CT
        public CoordinateTransformation ct;


        public void LocateImageBatch(int batch_index, string[] filenames)
        {
            ImageManager.Batch(batch_index).LocateImageBatch(filenames);
        }

        public void LoadFileList(string[] filenames)
        {
            this.filenames = filenames;
        }

        public void SaveProfile(string profilename)
        {
            try
            {
                if (DB_Manager.GetProfileNames().Contains(profilename))
                {
                    MessageBox.Show("Такой профиль уже существует!");
                    return;
                }
                DB_Manager.SaveProfile(profilename);
            }
            catch (Exception ex)
            { MessageBox.Show("Не удалось загрузить профиль: \n" + ex.Message); }

        }

        public void LoadProfile()
        {
            try
            {
                ProfilesForm form = new ProfilesForm(DB_Manager.GetProfileNames().ToArray());
                form.ShowDialog();
                if (form.selected != -1)
                    DB_Manager.LoadProfile(form.selected_name);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public void LoadCT()
        {
            try
            {
                ProfilesForm form = new ProfilesForm(DB_Manager.GetProfileNames().ToArray());
                form.ShowDialog();
                if (form.selected != -1)
                    DB_Manager.LoadCT(form.selected_name);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        public async void MeasurePhase()
        {
            int n;
            string userInput = Interaction.InputBox("Введите намер кадра: 0-" + ImageManager.MaxCount().ToString(),
               "Расчет фазового профиля",
               "0");
            Int32.TryParse(userInput, out n);
            await Task.Run(() => {
                PhaseMeasurer.GeneratePhaseImage(n);
            });


        }

        public void OpenContainerBatchesForm()
        {
            Form form_batches = Application.OpenForms["2D Container Batches"];

            if (form_batches == null)
            {
                form_batches = new ContainerBatchesForm();
                form_batches.Show();
            }
            else
            {
                form_batches.BringToFront();
                if (form_batches.WindowState == FormWindowState.Minimized)
                    form_batches.WindowState = FormWindowState.Normal;
                form_batches.Focus();
            }



        }







    }
}
