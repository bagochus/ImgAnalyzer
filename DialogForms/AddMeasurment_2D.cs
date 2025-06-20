using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer;
using ImgAnalyzer._2D;

namespace ImgAnalyzer.DialogForms
{
    public partial class AddMeasurment_2D : Form
    {
        public List<Measurment2D> measurments = new List<Measurment2D>();
        public AddMeasurment_2D()
        {
            InitializeComponent();

            listBox_names.Items.AddRange(Enum.GetNames(typeof(Measurment2DTypes)));

            checkBox_A.Enabled = (ImageManager.Batch_A().Count != 0);
            checkBox_B.Enabled = (ImageManager.Batch_B().Count != 0);
            checkBox_C.Enabled = (ImageManager.Batch_C().Count != 0);

            checkBox_A.Checked = checkBox_A.Enabled;
            checkBox_B.Checked = checkBox_B.Enabled;
            checkBox_C.Checked = checkBox_C.Enabled;

        }

        private void button_calculate_Click(object sender, EventArgs e)
        {
            if (CheckCTError())
            {
                MessageBox.Show("Для одной из групп изображений не указана активная область");
                return;
            
            }

            int[] selected_items = listBox_names.SelectedIndices.Cast<int>().ToArray();
            foreach (int index in selected_items) 
            {
                if (Enum.IsDefined(typeof(Measurment2DTypes), index))
                {
                    Measurment2DTypes type = (Measurment2DTypes)index;    
                    if (checkBox_A.Checked) AddMeasurment(type, ImageManager.Batch_A(),checkBox_ct.Checked);
                    if (checkBox_B.Checked) AddMeasurment(type, ImageManager.Batch_B(), checkBox_ct.Checked);
                    if (checkBox_C.Checked) AddMeasurment(type, ImageManager.Batch_C(), checkBox_ct.Checked);

                }
                else
                {
                    Console.WriteLine("Недопустимое значение enum");
                }

            }
            this.Close();


        }

        private bool CheckCTError()
        {
            bool error = false;
            if (!checkBox_ct.Checked) { return false; }
            error |= (checkBox_A.Checked && !ImageManager.IsCTDefined(ImageManager.Batch_A()));
            error |= (checkBox_B.Checked && !ImageManager.IsCTDefined(ImageManager.Batch_B()));
            error |= (checkBox_C.Checked && !ImageManager.IsCTDefined(ImageManager.Batch_C()));
            return error;
        }


        private void AddMeasurment(Measurment2DTypes type, ImageBatch batch, bool use_ct)
        {
            Measurment2D measurment2D = new Measurment2D();
            measurment2D.Type = type;
            measurment2D.CalculateInFrame = use_ct;
            measurment2D.Batch = batch;   
            measurments.Add(measurment2D);
        }






    }
}
