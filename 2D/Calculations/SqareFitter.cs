using ScottPlot.PathStrategies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer;
using System.Drawing;

namespace ImgAnalyzer._2D

{

    internal class ProfileScanResult
    {
        public double Coordinate = -1;
        public double Left = -1;
        public double Right = -1;
        public double Width => Right-Left;

        public double Contrast = 0;


    
    }

    public class SqareFitter
    {
        private IContainer_2D container;
        private double x_angle_start, x_angle_stop, y_angle_start, y_angle_stop;
        private double inial_step, final_step;
        private double thrContrast = 0.5;
        private double profileContrast = 45;
        private double widthMismath = 0.02;
        private int gridStep = 50;

        public SqareFitter(IContainer_2D container)
        {
            this.container = container;
            SettingsDB.GetDouble("AutoAA_thr_Contrast",out thrContrast);
            
        }



        public double[] GetProjectionProfieX(double angle, out int firstIndex)
        {
            // angle in radians
            double xstep = Math.Tan(angle);
            int blindZone = (int)(xstep * container.Height);
            int startIndex = 0;
            int endIndex = container.Width - 1;
            if (angle > 0) endIndex -= blindZone;
            else startIndex += blindZone;
            firstIndex = startIndex;
            double[] result = new double[endIndex - startIndex + 1];
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < container.Height; j++)
                {
                    result[i] += container.GetInterpolatedValue(i + startIndex + j * xstep, j);
                }
            }

            return result;
        }

        public double[] GetProjectionProfieY(double angle)
        {
            // angle in radians
            double ystep = Math.Tan(angle);
            int blindZone = (int)(ystep * container.Width);
            int startIndex = 0;
            int endIndex = container.Height - 1;
            if (angle > 0) endIndex -= blindZone;
            else startIndex += blindZone;

            double[] result = new double[endIndex - startIndex + 1];
            for (int i = 0; i < result.Length; i++)
            {
                for (int j = 0; j < container.Width; j++)
                {
                    result[i] += container.GetInterpolatedValue(j, i + startIndex + j * ystep);
                }
            }

            return result;
        }

        private double[] GetXProfile(int y)
        {
            double[] result = new double[container.Width];
            for (int i = 0; i < container.Width; i++) result[i] = container.ddata(i, y);
            return result;
        }

        private double[] GetYProfile(int x)
        {
            double[] result = new double[container.Height];
            for (int i = 0; i < container.Height; i++) result[i] = container.ddata(x, i);
            return result;
        }



        private ProfileScanResult ScanContainer (int coord, bool X)
        {
            ProfileScanResult result = new ProfileScanResult();
            result.Coordinate = coord;
            double[] values = X? GetXProfile(coord) : GetYProfile(coord);
            double threshold = values.Min() + (values.Max()-values.Min())*thrContrast;
            result.Left = -1;
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i] <= threshold && values[i + 1] >= threshold)
                {
                    result.Left = i + (values[i + 1] - values[i]) / (threshold - values[i]);
                    break;
                }
                    
            }

            for (int i = values.Length - 1; i >= 1; i--)
            {
                if (values[i] <= threshold && values[i - 1] >= threshold)
                {
                    result.Right = i - (values[i - 1] - values[i]) / (threshold - values[i]);
                    break;
                }
            }

            if (result.Left > 0 && result.Right > 0 && result.Right > result.Left)
            {
                double out_values = 0;
                int out_count = 0;
                double in_values = 0;
                int in_count = 0;
                for (int i = 0; i <= values.Length - 1; i++)
                {
                    if (i >= result.Left && i <= result.Right)
                    { in_count++; in_values += values[i]; } 
                    else
                    { out_count++; out_values += values[i]; }
                }

                out_values /= out_count;
                in_values /= in_count;
                result.Contrast = in_values / out_values;
            }

            return result;

        }


        public double ArrayPercentile(double[] input, double p)
        {



            if (p <= 0 || p >= 1) return Double.NaN;

            double[] sortedData = new double[input.Length];
            Array.Copy(input, 0, sortedData, 0, input.Length);
            Array.Sort(sortedData);


            double index = p * (sortedData.Length - 1);
            return input[(int)p];
        }


        private double FindWidth(double[] input, double threshold, out double leftRange)
        {
            leftRange = -1;
            for (int i = 0; i < input.Length -1 ; i++)
            {
                if (input[i] >= threshold && input[i+1] <= threshold) 
                    leftRange = i + (input[i+1] - input[i] )/(threshold-input[i]);
            }
            if (leftRange < 0) return 0;
            double rightRange = 0;
            for (int i = input.Length - 1; i >= 1; i--)
            {
                if (input[i] >= threshold && input[i - 1] <= threshold)
                    rightRange = i - (input[i - 1] - input[i]) / (threshold - input[i]);
            }
            return rightRange - leftRange;
        }

        private double[] GetAngleDependency(double start, double step, int count, bool x)
        { 
            double[] result = new double[count];
            double angle = start;
            for (int i =0; i<count;i++)
            {
                double[] profile =  x ?  GetProjectionProfieX(angle,out _) : GetProjectionProfieY(angle);
                double thr = (profile.Max() - profile.Min())*0.1 + profile.Min();
                result[i] = FindWidth(profile, thr, out _);
            }

            return result;
        }




        public void FindRange()
        {
            List<ProfileScanResult> scansX = new List<ProfileScanResult>();
            List<ProfileScanResult> scansY = new List<ProfileScanResult>();

            for (int i = 0; i < container.Height; i += gridStep) scansX.Add(ScanContainer(i, true));
            for (int i = 0; i < container.Width; i += gridStep) scansY.Add(ScanContainer(i, false));

            double max_w_x = scansX.Where(x=> x.Contrast > profileContrast).Select(x=>x.Width).Max();
            double max_w_y = scansY.Where(x => x.Contrast > profileContrast).Select(x => x.Width).Max();

            List<Point2D> pointsL = new List<Point2D>();
            List<Point2D> pointsR = new List<Point2D>();
            List<Point2D> pointsU = new List<Point2D>();
            List<Point2D> pointsD = new List<Point2D>();

            foreach (var s in scansX)
            {
                if (Math.Abs(1 - s.Width / max_w_x) > widthMismath) continue;
                if (s.Contrast < profileContrast) continue;
                pointsL.Add(new Point2D(s.Left, s.Coordinate));
                pointsR.Add(new Point2D(s.Right, s.Coordinate));
            }

            foreach (var s in scansY)
            {
                if (Math.Abs(1 - s.Width / max_w_y) > widthMismath) continue;
                if (s.Contrast < profileContrast) continue;
                pointsU.Add(new Point2D(s.Coordinate, s.Left));
                pointsD.Add(new Point2D(s.Coordinate, s.Right));
            }

            LineResult line_l = GeometricLineFitter.FitLine(pointsL);
            LineResult line_r = GeometricLineFitter.FitLine(pointsR);
            LineResult line_u = GeometricLineFitter.FitLine(pointsU);
            LineResult line_d = GeometricLineFitter.FitLine(pointsD);

            var ir_bl = GeometricLineFitter.FindIntersection(line_d, line_l);
            var ir_br = GeometricLineFitter.FindIntersection(line_d, line_r);
            var ir_tl = GeometricLineFitter.FindIntersection(line_u, line_l);
            var ir_tr = GeometricLineFitter.FindIntersection(line_u, line_r);

            PointF bl = new PointF((float)ir_bl.IntersectionPoint.X, (float)ir_bl.IntersectionPoint.Y);
            PointF br = new PointF((float)ir_br.IntersectionPoint.X, (float)ir_br.IntersectionPoint.Y);
            PointF tl = new PointF((float)ir_tl.IntersectionPoint.X, (float)ir_tl.IntersectionPoint.Y);
            PointF tr = new PointF((float)ir_tr.IntersectionPoint.X, (float)ir_tr.IntersectionPoint.Y);

            PointF[] points = new PointF[] { bl, tl, tr, br };

            ImageManager.Batch_A().coordinateTransformation = new CoordinateTransformation(points);

        }





    }
}
