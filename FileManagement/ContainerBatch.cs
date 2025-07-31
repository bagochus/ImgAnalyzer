using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public class ContainerBatch : IImageSource
    {
        private Type containerType;
        private List<string> containers = new List<string>();

        public string Name { get; set; }

        private int width;
        public int Width { get { return width; } }

        private int height;
        public int Height { get { return height; } }

        public int Count {  get { return containers.Count; } }

        public EventHandler DataChanged;


        public List<string> Filenames { get { return filenames; } }
        private List<string> filenames;

        public void LocateImageBatch(string[] filenames)
        {
            this.filenames = new List<string>(filenames);
            GetParameters(0);
            CheckAllFiles();

            DataChanged?.Invoke(this, new EventArgs());

        }



        private void GetParameters(int index) 
        {
         
            IContainer_2D container = Container_2D.ReadFromFile(filenames[index]);
            this.width = container.Width;
            this.height = container.Height; 
            this.containerType = container.GetType();
        
        
        
        }
        private void CheckAllFiles()
        {

            for (int i = 0; i < filenames.Count; i++)
            {
                if (!CheckParamenters(i))
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

        private bool CheckParamenters(int index)
        {

            IContainer_2D container = Container_2D.ReadFromFile(filenames[index]);
            int _width = container.Width;
            int _height = container.Height;
            Type _containerType = container.GetType();

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





    }





        
}
