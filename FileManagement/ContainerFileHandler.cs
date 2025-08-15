using ImgAnalyzer._2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    public class ContainerFileHandler : I2DFileHandler
    {
        private IContainer_2D container_2D;
        private bool _disposed = false;
        public int Width { get { return container_2D.Width; } }
        public int Height { get  { return container_2D.Height; } }

        public double Max() { return container_2D.Max(); }
        public double Min() { return container_2D.Min(); }
        public int GetCount(double v1, double v2) {return container_2D.GetCount(v1, v2);}

        int line = 0;

        public string Name {get { return container_2D.Name; } }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Отменяет вызов финализатора, если он есть
        }

        public ContainerFileHandler()
        {
            
        }

        public ContainerFileHandler(IContainer_2D container_2D)
        {
            this.container_2D = container_2D;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Освобождаем управляемые ресурсы
                    container_2D = null;
                }

                // Здесь можно освободить неуправляемые ресурсы (если есть)
                _disposed = true;
            }
        }

        // Финализатор (на случай, если Dispose не вызвали)
        ~ContainerFileHandler()
        {
            Dispose(false);
        }

        public double GetPixelValue(int pixel)
        {
            if (container_2D is Container_2D_double) return (container_2D as Container_2D_double).data[pixel, line];
            if (container_2D is Container_2D_int) return (container_2D as Container_2D_int).data[pixel, line];
            return 0;
        }

        public double GetPixelValue(int pixel, int line)
        {
            if (container_2D is Container_2D_double) return (container_2D as Container_2D_double).data[pixel,line];
            if (container_2D is Container_2D_int) return (container_2D as Container_2D_int).data[pixel, line];
            return 0;
        }

        public void LoadFile(IImageSource imageSource, string fileName)
        {
            container_2D = Container_2D.ReadFromFile(fileName);
        }

        public void LoadFile(IImageSource imageSource, int index)
        {
            var source = imageSource as ContainerBatch;

            container_2D = Container_2D.ReadFromFile(source.Filenames[index]);
        }

        public void SelectLine(int line)
        {
            this.line = line;   
        }

        public ushort[] GetLine(int index)
        {
            throw new NotImplementedException();
        }

        public double[] GetLineDouble(int index)
        {
            if (container_2D is Container_2D_double)
            {
                var c = container_2D as Container_2D_double;
                double[] result = new double[c.Width];
                Buffer.BlockCopy(c.data, index * c.Height * sizeof(double), result, 0, c.Width * sizeof(double));
                return result;
            }
            else {
                double[] result = new double[container_2D.Width];
                for (int x = 0; x < container_2D.Width; x++) result[x] = container_2D.ddata(x,index); 
                return result;
            }
        }
    }
}
