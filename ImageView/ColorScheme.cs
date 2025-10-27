using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.ImageView
{
    internal class ColorScheme
    {



        public ColorScheme Clone()
        { 
            ColorScheme result = new ColorScheme();
            result.Name = Name;

            for (int i = 0; i < colorList.Count;i++)
            {
                result.colorList.Add(new KeyValuePair<Color, double>(colorList[i].Key, colorList[i].Value));
            }
            result.min = min;
            result.max = max;
            return result;
        
        }

        public string Name { get; set; }

        protected List<KeyValuePair<Color, double>> colorList = new List<KeyValuePair<Color, double>>();
        protected double min = 0;
        protected double max = 1;

        public double Max { get { return max; } set { RescaleMax(value); max = value; } }
        public double Min { get { return min; } set { RescaleMin(value); min = value; } }


        private void RescaleMax(double new_max)
        {
            double k = (new_max - min) / (max - min);
            for (int i = 0; i < colorList.Count; i++)
            {
                double old_value = colorList[i].Value;
                Color color = colorList[i].Key;
                double new_value = min + k * (colorList[i].Value - min);
                colorList[i] = new KeyValuePair<Color, double>(color, new_value);

            }
        }
        private void RescaleMin(double new_min)
        {
            double k = (max - new_min) / (max - min);
            for (int i = 0; i < colorList.Count; i++)
            {
                double old_value = colorList[i].Value;
                Color color = colorList[i].Key;
                double new_value = max - k * (max - colorList[i].Value);
                colorList[i] = new KeyValuePair<Color, double>(color, new_value);

            }
        }


        public void AddColor(Color color, double value)
        {
            if (value < min || value > max) return;

            var record = new KeyValuePair<Color, double>(color, value);
            colorList.Add(record);
            colorList.Sort((x, y) => x.Value.CompareTo(y.Value));
        }

        public Color CalculateColorDouble(double Min, double Max, int value)
        {


            if (value >= Max) return colorList.Last().Key;
            if (value <= Min) return colorList.First().Key;

            double value_cor = min + ((value - Min) * (max - min) / (Max - Min));

            

            return CalculateColor(value_cor);

        }

        public static Color CalculateBW(double Min, double Max, double value)
        {

            byte rgb = (byte)((value - Min) * (256) / (Max - Min));

            return Color.FromArgb(rgb,rgb,rgb);

        }


        public Color CalculateColor(double value)
        {
            if (value >= max) return colorList.Last().Key;
            if (value <= min) return colorList.First().Key;

            Color colorA = Color.Black;
            Color colorB = Color.Black;
            double valueA = 0;
            double valueB = 1;

            for (int i = 0; i < colorList.Count - 1; i++)
            {
                if (colorList[i].Value <= value && colorList[i + 1].Value > value)
                {
                    colorA = colorList[i].Key;
                    colorB = colorList[i + 1].Key;
                    valueA = colorList[i].Value;
                    valueB = colorList[i + 1].Value;
                    break;
                }
            }
            return GetMediumColor(valueA, valueB, value, colorA, colorB);



        }


        private Color GetMediumColor(double v1, double v2, double v, Color c1, Color c2)
        {
            byte r = (byte)(c1.R + ((v - v1) * (c2.R - c1.R)) / (v2 - v1));
            byte g = (byte)(c1.G + ((v - v1) * (c2.G - c1.G)) / (v2 - v1));
            byte b = (byte)(c1.B + ((v - v1) * (c2.B - c1.B)) / (v2 - v1));
            return Color.FromArgb(r, g, b);
        }






    }
}
