using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D.Calculations
{
    internal class Reascale1D : Calculation2D
    {
        public Reascale1D()
        {
            this.description = "Преобразует значения поля согласно базису графиков 1D";
            this.singeValueNames = new string[] { "Repalce" };
            this.containerNames = new string[] { "Field" };
        }
        public override double Measure(int x, int y)
        {

            double result = SingleValueParameters[0];
            int a = (int)ContainerParameters[0].ddata[x, y];
            try
            {
                result = DataManager_1D.Instance.x_start + DataManager_1D.Instance.x_step * a;
            }
            catch { }
            return result;
        }




    }
}
