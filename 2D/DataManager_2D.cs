
using System.ComponentModel;
using System.Windows.Forms;
using ImgAnalyzer.DialogForms;

namespace ImgAnalyzer._2D
{
    public class DataManager_2D
    {

        public static BindingList<IContainer_2D> containers = new BindingList<IContainer_2D>();

        public static void ShowMainForm()
        {
            Form form_2D = Application.OpenForms["Form_2D"];

            if (form_2D == null)
            {
                form_2D = new Form_2D();
                form_2D.Show();
            }
            else
            {
                form_2D.BringToFront();
                if (form_2D.WindowState == FormWindowState.Minimized)
                    form_2D.WindowState = FormWindowState.Normal;
                form_2D.Focus();
            }

        }

        public static void AddMeasurment()
        {
            AddMeasurment_2D addForm = new AddMeasurment_2D();
            addForm.ShowDialog();
            if (addForm.measurments.Count == 0) return;
            foreach (var m in addForm.measurments) m.ProcessMeasurment();
        }

        public static void PlotMap(int[] indicies)
        {
            foreach (var ind in indicies)
            {
                HeatMapForm form = new HeatMapForm(containers[ind]);
                form.Show();

            }



        }






    }
}
