using ImgAnalyzer.MeasurmentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.CodeDom;
using System.Xml.Serialization;

namespace ImgAnalyzer
{
    public class DataManager_1D
    {
        
        // --------------X axis management----------------
        public double x_start = 0;
        public double x_step = 1;

        public string VariableName = "N кадра";
        public string VariableUnit = "";

        //---------------Global links---------------------
        
        public BindingList<DataContainer> dataContainers = new BindingList<DataContainer>();

        public static DataManager_1D Instance; 
        public DataManager_1D() { Instance = this; }

        // ---------------Local parameters---------------
        private int poly_counter = 0;
        private int poly_counter_ct = 0;
        public bool InWork
            { get { return in_work; } }
        private bool in_work = false;

        // ---------------------------------------------------



        #region Measurment List Management
        public void AddMeasurment(IMeasurment measurment, ImageBatch batch)
        {
            bool ct_error = true;
            ct_error &= (batch.coordinateTransformation == null);
            ct_error &= ((measurment is PointMeasurmentCT) || 
                        (measurment is PolygonMeasurmentCT));
            if (ct_error)
            {
                MessageBox.Show("Для выбранной группы изображений не определена активная область");
                return;
            }
            DataContainer dc = new DataContainer(measurment, batch);
            if (measurment is PolygonMeasurment)
            {
                poly_counter++;
                dc.Name = "Poly_"+poly_counter.ToString();
            }
            if (measurment is PolygonMeasurmentCT)
            {
                poly_counter_ct++;
                dc.Name = "PolyCT_" + poly_counter_ct.ToString();
            }

            dataContainers.Add(dc);
        }
        public void AddMeasurment(IMeasurment measurment)
        {
            try
            {
            AddMeasurment(measurment.Clone(), ImageManager.Batch(0));
            AddMeasurment(measurment.Clone(), ImageManager.Batch(1));
            AddMeasurment(measurment.Clone(), ImageManager.Batch(2));
            } catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        public void AddPointCTMeasurment(Point point, ImageBatch batch, bool addForAll)
        {
            PointMeasurmentCT pt_ct = new PointMeasurmentCT(point,batch);
            dataContainers.Add(new DataContainer(pt_ct, batch));
            if (!addForAll) return;
            PointF point_frame = pt_ct.PointFrame;
            foreach (ImageBatch b in ImageManager.Stacks) 
            {
                if (b == batch) continue;
                PointMeasurmentCT pt_ct_copy = new PointMeasurmentCT();
                pt_ct_copy.BindImageStack(batch);
                pt_ct_copy.PointFrame = point_frame;
                dataContainers.Add(new DataContainer(pt_ct_copy, b));
            }
        }

        





        public void DeleteItem(int n)
            { dataContainers.RemoveAt(n); }

        public void RenameItem(int n, string newName)
        {
            dataContainers[n].Name = newName;   
        }

        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            for (int i = 0; i < dataContainers.Count; i++) { names.Add(dataContainers[i].Name); }
            return names;
        }

        public List<Point> GetPoints(ImageBatch imageBatch)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != imageBatch) continue; 
                if (dataContainers[i].measurment is PointMeasurment)
                {
                    IMeasurment ms = dataContainers[i].measurment;
                    Point pt = (ms as PointMeasurment).point;
                    points.Add(pt);
                }
            }
            return points;
        }
        
        public List<Point> GetPointsCT(ImageBatch imageBatch)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != imageBatch) continue;
                if (dataContainers[i].measurment is PointMeasurmentCT)
                {
                    IMeasurment ms = dataContainers[i].measurment;
                    Point pt = (ms as PointMeasurmentCT).PointPhotoInt;
                    points.Add(pt);
                }
            }
            return points;
        }

        public List<string> GetPoinsCT_Coords(ImageBatch imageBatch)
        {
            List<string> coords = new List<string>();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != imageBatch) continue;
                if (dataContainers[i].measurment is PointMeasurmentCT)
                {
                    IMeasurment ms = dataContainers[i].measurment;
                    string x_str = (ms as PointMeasurmentCT).PointFrameInt.X.ToString();
                    string y_str = (ms as PointMeasurmentCT).PointFrameInt.Y.ToString();
                    coords.Add("*"+x_str+":"+y_str);    
                }
            }
            return coords;
        }

        public List<Point[]> GetPolys(ImageBatch imageBatch)
        {
            List<Point[]> polys = new List<Point[]>();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != imageBatch) continue;
                if (dataContainers[i].measurment is PolygonMeasurment)
                {
                    IMeasurment ms = dataContainers[i].measurment;
                    Point[] pts = (ms as PolygonMeasurment).points;
                    polys.Add(pts);
                }
            }
            return polys;
        }

        public List<string> GetPolyNames(ImageBatch imageBatch)
        {
            List<string> names = new List<string>();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != imageBatch) continue;
                if (dataContainers[i].measurment is PolygonMeasurment)
                {
                    IMeasurment ms = dataContainers[i].measurment;
                    Point[] pts = (ms as PolygonMeasurment).points;
                    names.Add(dataContainers[i].Name);
                }
            }
            return names;


        }

        public void PlotSelectedItems(int[] items)
        {
            List<int> selected = new List<int>(items);
            for (int i = 0; i < selected.Count; i++) 
            {
                if (!(dataContainers[i].containerStatus == ContainerStatus.Done ||
                    dataContainers[i].containerStatus == ContainerStatus.Obsolete))
                { MessageBox.Show("Не все данные готовы"); return; }
            }
            PlotForm plotForm = new PlotForm(VariableName+", " + VariableUnit);

            double[] xdata = new double[ImageManager.MaxCount()];
            for (int j = 0; j < ImageManager.MaxCount(); j++) xdata[j] = x_start + x_step * j;

            foreach (int i in  selected) plotForm.AddData(dataContainers[i].Name, xdata, dataContainers[i].data);


            plotForm.Show();
        }

        public void ClearMeasurment()
        {
            dataContainers.Clear();
        }


        #endregion

        #region 1D Form management

        public void ShowForm()
        {
            Form form_1D = Application.OpenForms["Form1D"];

            if (form_1D == null) CreateForm();
            else
            {
                form_1D.BringToFront();
                if (form_1D.WindowState == FormWindowState.Minimized)
                    form_1D.WindowState = FormWindowState.Normal;
                form_1D.Focus();
            }

        }
       
        private void CreateForm()
        {
            Form_1D form_1D = new Form_1D(this);
            form_1D.Show();
        }






        #endregion




        private void ProcessImage(ImageBatch batch, int n)
        {
            ImageProcessor_1D processor = new ImageProcessor_1D(batch.filenames[n]);

            for (int i = 0; i < dataContainers.Count; i++) 
            {
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != batch) continue;
                if (dataContainers[i].containerStatus == ContainerStatus.Done) continue;

                dataContainers[i].data[n] = dataContainers[i].measurment.Measure(processor);
                dataContainers[i].imageStatus[n] = ImageStatus.Done;
            
            }
        }

        private void ProcessImage(ImageBatch batch, int n, int[] selected)
        {
            ImageProcessor_1D processor = new ImageProcessor_1D(batch.filenames[n]);

            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (!selected.Contains(i)) continue;
                if (dataContainers[i].measurment == null) continue;
                if (dataContainers[i].measurment.Batch != batch) continue;
                if (dataContainers[i].containerStatus == ContainerStatus.Done) continue;

                dataContainers[i].data[n] = dataContainers[i].measurment.Measure(processor);
                dataContainers[i].imageStatus[n] = ImageStatus.Done;

            }
        }

        public void InitContainers()
        {
            foreach (var container in dataContainers)
                container.measurment?.Init();
        }


        public async void ProcessAllImages()
        {
            if (in_work) return;
            in_work = true;
            InitContainers();
            foreach (DataContainer c in dataContainers) c.containerStatus = ContainerStatus.InWork;
            for (int i = 0; i < 3; i++) 
            {
                Action<int> work = (int n) => ProcessImage(ImageManager.Batch(i), n);
                int count = ImageManager.Batch(i).Count;

                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount // Оптимальное кол-во потоков
                };
                await Task.Run(() =>
                {
                    Parallel.For(0, count, work);
                });
                
            }
            foreach (DataContainer c in dataContainers) c.containerStatus = ContainerStatus.Done;

            in_work = false;
        }

        public async void ProcessAllImages(int[] selected)
        {
            if (in_work) return;
            in_work = true;
            InitContainers();
            for (int i = 0; i < dataContainers.Count; i++)
            {
                if (selected.Contains(i))
                    dataContainers[i].containerStatus = ContainerStatus.InWork;
            }

            for (int i = 0; i < 3; i++)
            {
                Action<int> work = (int n) => ProcessImage(ImageManager.Batch(i), n, selected);
                int count = ImageManager.Batch(i).Count;

                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount // Оптимальное кол-во потоков
                };
                await Task.Run(() =>
                {
                    Parallel.For(0, count, work);
                });
            }
            for (int i = 0; i<dataContainers.Count; i++)
            {
                if(selected.Contains(i))
                    dataContainers[i].containerStatus = ContainerStatus.Done;
            }
            in_work = false;
        }








    }
}
