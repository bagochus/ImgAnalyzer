using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;


namespace ImgAnalyzer
{
    public class ContainerBatch : IImageSource
    {
        private Type containerType = null;
        //private List<string> containers = new List<string>();
        private string workFolder = "";
        public string WorkFolder { get { return workFolder; } }

        public string Name { get; set; }

        private int width;
        public int Width { get { return width; } }

        private int height;
        public int Height { get { return height; } }
        public int Count {  get { return Filenames.Count; } }

        private string sample = "";
        public string Sample { get { return sample; } } 
        public string BatchType = "";
        public string comment = "";
       
        public int id = -1;

        private int sampleId = -1;
        public int SampleId
        {
            get { return sampleId; }
            set
            {
                this.sampleId = value;
                sample = SamplesDB.GetSampleName(sampleId);
            }
        }


        public EventHandler DataChanged;

        public CoordinateTransformation coordinateTransformation { get; set; }
        public List<string> Filenames { get { return filenames; } }
        private List<string> filenames = new List<string>();

        public void LocateImageBatch(string[] filenames)
        {
            if (filenames.Length == 0) return;
            if (this.filenames.Count == 0) GetParameters(filenames[0]);

            foreach (string filename in filenames) 
            {
                if (CheckParamenters(filename)) this.filenames.Add(filename);
            }
            DataChanged?.Invoke(this, new EventArgs());
        }

        private void GetParameters(string filename) 
        {
        
            Container_2D.GetParameters(filename,out containerType,out width, out height);    
        }
        private void CheckAllFiles()
        {

            for (int i = 0; i < filenames.Count; i++)
            {
                if (!CheckParamenters(filenames[i]))
                {
                    DialogResult dialogResult = MessageBox.Show("Файл " + filenames[i] +
                        " не соответсвтует остальным файлам. Исключить его из набора?"
                        , "Some Title", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        filenames.Remove(filenames[i]);
                    }
                }
            }
        }

        private bool CheckParamenters(string filename)
        {

            int _width;
            int _height;
            Type _containerType;
            Container_2D.GetParameters(filename, out _containerType, out _width, out _height);


            return (width == _width) &&
                (height == _height) &&
                (containerType == _containerType);

        }

        public I2DFileHandler Get2DFileHandler(int index)
        {
            if (index >= Count || index < 0) throw new ArgumentOutOfRangeException("index");
            var hndl = new ContainerFileHandler();
            hndl.LoadFile(this, index);
            return hndl;

        }

        public I2DFileHandler Get2DFileHandler(string filename)
        {
            if (!filenames.Contains(filename))
                throw new Exception("No such image in batch");

            var hndl = new ContainerFileHandler();
            hndl.LoadFile(this, filename);
            return hndl;

        }

        public void AddContainer(IContainer_2D container, bool copy_container = false)
        {
            //copy_container - сохранить контейнер в новый файл, даже если он уже куда то сохранен
            if (filenames.Count == 0)
            {
                if (container.Filename == "" || copy_container)
                {
                    if (workFolder == "") workFolder = CreateFolder(Name+"_"+sample);
                    container.SaveToFile(Path.Combine(workFolder, "cont_"+ filenames.Count.ToString() +".bin"));
                }
                filenames.Add(container.Filename);
                containerType = container.GetType();
                width = container.Width;
                height = container.Height;

            }
            else
            {
                if(!CheckConsistency(container)) return;
                if (container.Filename == "" || copy_container)
                {
                    container.SaveToFile(Path.Combine(workFolder, "cont_" + filenames.Count.ToString() + ".bin"));
                }
                filenames.Add(container.Filename);

            }
        }

        private bool CheckConsistency(IContainer_2D container)
        {
            
            if (container.GetType() != containerType) return false;
            if (container.Width != width) return false;
            if (container.Height != height) return false;

            return true;
        }


        public static string CreateFolder(string name)
        {

            var _containerFolder = SettingDefinition.CreateGlobal("_containerFolder", "D:\\containers", "Папка для сохранения данных");
            SettingsManager.GetSettingsFromDatabase(new List<SettingDefinition> { _containerFolder });
            string containerFolder = _containerFolder.GetValue<string>();
            return FileManagement.CreateUniqueFolder(containerFolder+"\\"+name);

            // Получаем путь к папке с программой
        }

        public void UpdateComment(string comment)
        {
            this.comment = comment;
            if (id >= 0) SamplesDB.UpdateBatchComment(id, comment);
        }


        public BatchHeader GetHeader()
        { 
            BatchHeader result = new BatchHeader(); 
            result.Name = Name;
            result.Type = BatchType;
            result.Sample = sample;
            result.Width = Width;
            result.Height = Height;
            result.Count = Count;

            return result;
        }

    }





        
}
