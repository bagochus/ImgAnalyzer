using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer._2D

{
    internal class PlotXY : Calculation2D
    {
        public PlotXY()
        {
            this.description = "Строит завсимость точек друг от друга из разных наборов\n" +
                "a - шаг сетки";
            this.containerNames = new string[] { "X", "Y" };
            this.singeValueNames = new string[] { "a" };
            this.pixbypix = false;
            this.nonReturning = true;
            

        }

        public override double Measure(int x, int y) 
        {
            throw new NotImplementedException();
        }


        public override void Process()
        {
            int step = (int)SingleValueParameters[0];
            int xcount = width / step;
            int ycount = height / step;


            List<double> dx = new List<double>();
            List<double> dy = new List<double>();

            double[] datax = new double[xcount * ycount];
            double[] datay = new double[xcount * ycount];

            for (int i = 0; i < xcount; i++) 
                for (int j = 0; j < ycount; j++)
                {
                    //i,j - grid indicies
                    //x,y - container indicies
                    int x = i * step;
                    int y = j * step;
                    if (ContainerParameters[0].ddata(x, y) != 0 && ContainerParameters[1].ddata(x, y) != 0)
                    {
                        dx.Add(ContainerParameters[0].ddata(x, y));
                        dy.Add(ContainerParameters[1].ddata(x, y));
                    }



                }

            PlotForm form = new PlotForm(ContainerParameters[0].Name);
            form.Text ="Plot: "+ ContainerParameters[0].Name + " vs " + ContainerParameters[1].Name;
            form.AddDataMarkers(ContainerParameters[0].Name, dx.ToArray(), dy.ToArray());
            form.Show();



        }





    }
}
