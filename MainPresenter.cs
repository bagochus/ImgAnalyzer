using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff;
using BitMiracle.LibTiff.Classic;
using ImgAnalyzer.MeasurmentTypes;
using ScottPlot.Plottables;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Windows.Forms;
using ScottPlot.PlotStyles;
using ScottPlot.Interactivity.UserActions;
using System.Reflection;
using System.Diagnostics;
using ImgAnalyzer.DialogForms;

namespace ImgAnalyzer
{
    public enum State : byte { Rising, Falling, Unknown };

    public enum NearestExtremum:byte {A,B,Undef };
    public enum Vert : int { TopLeft = 0, TopRight, BottomRight, BottomLeft  };
    
    


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
        private int[,] max_values;
        private int[,] min_values;
        private int[,] amplitude_values;
        private int[,] good_values;
        private int[,] pseudo_phase_values;
        private int[,] a_values;
        private int[,] b_values;
        private List<int[,]> ax_positions = new List<int[,]>();
        private List<int[,]> ay_positions = new List<int[,]>();
        private int[,] a_peaks;
        private List<int[,]> bx_positions = new List<int[,]>();
        private List<int[,]> by_positions = new List<int[,]>();
        private int[,] b_peaks;

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
            { MessageBox.Show("Не удалось загрузить профиль: \n" +ex.Message); }

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



   
    }
}
