using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D
{





    public abstract class Calculation2D : ICalculation2D
    {
        public int Width { get { return width; } }
        private int width = 0;
        public int Height { get { return height; } }
        private int height = 0;

        public string Description { get { return description; } }
        protected string description = "";
        public double[] SingleValueParameters { get; set; }
        public string[] SingleValueNames { get { return singeValueNames; } }
        protected string[] singeValueNames = new string[0];
        public IContainer_2D[] ContainerParameters { get; set; }
        public string[] ContainerNames { get { return containerNames; } }
        protected string[] containerNames = new string[0];

        public List<IContainer_2D> SeriesParameters { get; set; }
        public string SeriessName { get { return seriesName; } }
        protected string seriesName = "";

        public string ErrorMessage { get; }
        protected string errorMessage = "";

        public bool PixByPixCalculation { get { return pixbypix; } }
        private bool pixbypix = true;


        public abstract double Measure(int x, int y);

        public bool Check()
        {
            bool data_valid = true;
            errorMessage = "Недостаточно входных данных";

            if (singeValueNames.Length > 0) 
            {

                if (SingleValueParameters == null)
                {
                    errorMessage = "Не определены входные данные";
                    return false;
                }
                data_valid &= (singeValueNames.Length == SingleValueParameters.Length);
            } 

            if (seriesName != "")
            {
                if (SeriesParameters == null)
                {
                    errorMessage = "Не определены входные данные";
                    return false;
                }
            }

            if (containerNames.Length > 0)
            {
                if (ContainerParameters == null)
                {
                    errorMessage = "Не определены входные данные";
                    return false;
                }
                data_valid &= (containerNames.Length == ContainerParameters.Length);
            }

            if (!data_valid)
            {
                errorMessage = "";
                return data_valid;
            }

            if (containerNames.Length == 0 && seriesName == "") return true;

            if (containerNames.Length > 0)
            { 
                width = this.ContainerParameters[0].Width;
                height = this.ContainerParameters[0].Height;
            } else
            {
                width = this.SeriesParameters[0].Width;
                height = this.SeriesParameters[0].Height;
            }
            if (containerNames.Length !=0 )
            foreach (IContainer_2D container in this.ContainerParameters) 
            {
                if (container.Width != width || container.Height != height)
                {
                    errorMessage = "Выбранные массивы имеют разную размерность";
                    return false;
                }
            
            }
            if (SeriesParameters == null) return data_valid;
            foreach (IContainer_2D container in this.SeriesParameters)
            {
                if (container.Width != width || container.Height != height)
                {
                    errorMessage = "Выбранные массивы имеют разную размерность";
                    return false;
                }

            }
            
            return data_valid;




        }

        public void Test()
        {


        }


    }
}
