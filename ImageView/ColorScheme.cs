using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.ImageView
{
    internal class ColorScheme
    {
        
        public string Name { get; set; }


        private List<KeyValuePair<Color, double>> colorList = new List<KeyValuePair<Color, double>>();
        private double min = 0;
        private double max = 1;


        public void AddColor(Color color, double value)
        {
            if (value > min || value > max) return;

            var record = new KeyValuePair<Color, double>(color, value);
            colorList.Add(record);
            colorList.Sort();
        }

        private Color CalculateColorDouble(double Min, double Max, int value)
        {
            if (value > Max) return colorList.Last().Key;
            if (value < Min) return colorList.First().Key;

            double value_cor = min + ((value - Min) * (max - min)) / (Max - Min);

            Color colorA = Color.Black;
            Color colorB = Color.Black;
            double valueA = 0;
            double valueB = 1;
            
            for (int i = 0; i < colorList.Count - 1; i++)
            {
                if (colorList[i].Value < value)
                {
                    colorA = colorList[i].Key;
                    colorB = colorList[i+1].Key;
                    valueA = colorList[i].Value;
                    valueB = colorList[i+1].Value;  
                }
            }
            return GetMediumColor(valueA, valueB, value_cor, colorA, colorB);

        }

        private Color CalculateColor(double value)
        {
            if (value > max) return colorList.Last().Key;
            if (value < min) return colorList.First().Key;

            Color colorA = Color.Black;
            Color colorB = Color.Black;
            double valueA = 0;
            double valueB = 1;

            for (int i = 0; i < colorList.Count - 1; i++)
            {
                if (colorList[i].Value < value)
                {
                    colorA = colorList[i].Key;
                    colorB = colorList[i + 1].Key;
                    valueA = colorList[i].Value;
                    valueB = colorList[i + 1].Value;
                }
            }
            return GetMediumColor(valueA, valueB, value, colorA, colorB);



        }


        private Color GetMediumColor(double v1,double v2, double v, Color c1, Color c2)
        {
            byte r = (byte)(c1.R + ((v - v1) * (c1.R - c2.R)) / (v2 - v1));
            byte g = (byte)(c1.R + ((v - v1) * (c1.R - c2.R)) / (v2 - v1));
            byte b = (byte)(c1.R + ((v - v1) * (c1.R - c2.R)) / (v2 - v1));
            return Color.FromArgb(r, g, b);
        }



        


    }
}
