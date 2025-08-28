using ScottPlot.Colormaps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer._2D.GroupOperations
{
    public class LU_Table_mask : LU_Table
    {
        //-------------interface properties-----------------------------
        public override string Description
        {
            get
            {
                return 
                    "Создает LUТ таблицу 64*64*256 на основе пакета данных c маскированием битых пикселей\n" +
                    "OUT0-256 целевые выходные значения\n" +
                    "Vstart, Vstep - начальные значения и шаг АЦП соотв. кадрам\n" +
                    "Mask - битовое поле с дефектными пикселями";
            }
        }

        public LU_Table_mask() { useMask = true; }



    }
}
