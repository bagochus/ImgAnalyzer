using BitMiracle.LibTiff.Classic;
using ImgAnalyzer._2D;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public class ContainerBatch : IImageSource
    {
        private Type containerType = null;
        //private List<string> containers = new List<string>();
        private string workFolder = "";


        public string Name { get; set; }

        private int width;
        public int Width { get { return width; } }

        private int height;
        public int Height { get { return height; } }
        public int Count {  get { return Filenames.Count; } }

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
            
            if (filenames.Count == 0)
            {
                if (container.Filename == "" || copy_container)
                {
                    if (workFolder == "") workFolder = CreateFolder(Name);
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
            // Получаем путь к папке с программой
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            // Создаем полный путь к целевой папке
            string targetPath = Path.Combine(appPath, name);

            // Если папка не существует, создаем ее
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
                return targetPath;
            }

            // Если папка существует, ищем доступное имя с суффиксом
            int counter = 0;
            string newPath;
            do
            {
                newPath = Path.Combine(appPath, $"{name}_{counter}");
                counter++;
            } while (Directory.Exists(newPath));

            // Создаем папку с новым именем
            Directory.CreateDirectory(newPath);
            return newPath;
        }


    }





        
}
