using ImgAnalyzer._2D;
using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.DialogForms
{

    public partial class FormCalculations2D : Form
    {
        private List<Type> types = new List<Type>();

        private List<Label> labels_single = new List<Label>();
        private List<TextBox> values_single = new List<TextBox>();

        private List<Label> labels_containers = new List<Label>();
        private List<ComboBox> comboBox_containers = new List<ComboBox>(); 

        private List<ComboBox> comboBoxes_series = new List<ComboBox>();  

        private List<string> avaiable_containers = new List<string>();

        private Label singleValuesHeader;
        private Label containerValuesHeader;
        private Label seriesHeader;

        ICalculation2D calculation;

        //----------------Positions---------------
        private int column1_x = 20;
        private int column2_x = 120;

        private int y_start = 50;
        private int y_current = 50;
        private int y_step = 30;





        public FormCalculations2D()
        {

            InitializeComponent();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Перебираем все типы в сборках и находим те, которые реализуют IMyInterface
            var implementingTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ICalculation2D).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            // Выводим имена классов
            foreach (var type in implementingTypes)
            {
                comboBox_type.Items.Add(type.Name);
                types.Add(type);
            }

            foreach (var c in DataManager_2D.containers) {avaiable_containers.Add(c.Name); }


        }

        private void ConstructForm (ICalculation2D calc)
        {
            label_decription.Text = calc.Description;

            if (calc.SingleValueNames.Length > 0)
            {
                singleValuesHeader = new Label();
                singleValuesHeader.Text = "Константы";
                singleValuesHeader.Location = new Point(column1_x,y_current);
                y_current += y_step;
                this.Controls.Add(singleValuesHeader);

                for (int i = 0; i < calc.SingleValueNames.Length; i++) 
                {
                    Label l = new Label();
                    l.Text = calc.SingleValueNames[i];
                    l.Location = new Point (column1_x,y_current);
                    this.Controls.Add(l);
                    labels_single.Add(l);
                    TextBox t = new TextBox();
                    t.Location = new Point(column2_x,y_current);
                    this.Controls.Add(t);
                    values_single.Add(t);
                    y_current += y_step;
                }
            }

            if (calc.ContainerNames.Length > 0)
            {
                containerValuesHeader = new Label();
                containerValuesHeader.Text = "2D - Массивы";
                containerValuesHeader.Location = new Point(column1_x, y_current);
                y_current += y_step;
                this.Controls.Add(containerValuesHeader);

                for (int i = 0; i < calc.ContainerNames.Length; i++)
                {
                    Label l = new Label();
                    l.Text = calc.ContainerNames[i];
                    l.Location = new Point(column1_x, y_current);
                    this.Controls.Add(l);
                    labels_containers.Add(l);
                    ComboBox cb = new ComboBox();
                    cb.Location = new Point(column2_x, y_current);
                    cb.Items.AddRange(avaiable_containers.ToArray());
                    this.Controls.Add(cb);
                    comboBox_containers.Add(cb);
                    y_current += y_step;
                }
            }

            if (calc.SeriessName != "")
            {
                seriesHeader = new Label();
                seriesHeader.Text = calc.SeriessName;
                seriesHeader.Location = new Point(column1_x, y_current);
                y_current += y_step;
                this.Controls.Add(seriesHeader);
                AddSeriesCombobox();

            }
        }

        private bool ReadForm(ICalculation2D calc)
        {
            bool input_valid = false;
            try 
            {
                if (calc.SingleValueNames.Length > 0)
                {
                    double[] SingleValueParameters = new double[calc.SingleValueNames.Length];
                    for (int i = 0; i < calc.SingleValueNames.Length; i++)
                    {
                        SingleValueParameters[i] = Double.Parse(this.values_single[i].Text);
                    }
                    calc.SingleValueParameters = SingleValueParameters;
                }

                if (calc.ContainerNames.Length > 0)
                {
                    IContainer_2D[] containers = new IContainer_2D[calc.ContainerNames.Length];
                    for (int i = 0; i < containers.Length; i++)
                    {
                        int selected = comboBox_containers[i].SelectedIndex;
                        if (selected == -1) throw new Exception("Не выбран массив " + calc.ContainerNames[i]);
                        containers[i] = DataManager_2D.containers[selected];
                    }
                    calc.ContainerParameters = containers;
                }

                if (calc.SeriessName != "")
                {
                    calc.SeriesParameters = new List<IContainer_2D>();
                    foreach (ComboBox cb in comboBoxes_series)
                    {
                        int index = cb.SelectedIndex;
                        if (index != -1) calc.SeriesParameters.Add(DataManager_2D.containers[index]);
                    }
                }
                input_valid = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return input_valid;

            

        }

        private void ClearForm()
        {
            
            RemoveControl(singleValuesHeader);
            RemoveControl(containerValuesHeader);
            RemoveControl(seriesHeader);

            foreach (TextBox tb in values_single)
            {
                RemoveControl(tb);
            }
            values_single.Clear();

            foreach (ComboBox cb in comboBox_containers)
            {
                RemoveControl(cb);
            }
            comboBox_containers.Clear();

            foreach (ComboBox cb in comboBoxes_series)
            {
                RemoveControl(cb);
            }
            comboBoxes_series.Clear();

            foreach (Label l  in labels_single) RemoveControl(l);
            labels_single.Clear();
            foreach (Label l in labels_containers) RemoveControl(l);
            labels_containers.Clear();
            


            
            y_current = y_start;

        }

        private void RemoveControl(Control c)
        {
            if (c == null) return;
            if (!this.Controls.Contains(c)) return; 
            this.Controls.Remove(c);
            c.Dispose();
        }

        private void SelectItem()
        {
            int index = comboBox_type.SelectedIndex;
            if (index < 0) return;
            var calc = Activator.CreateInstance(types[index]) as ICalculation2D;
            calculation = calc;
            ClearForm();
            ConstructForm(calc);
        }

        private void Execute()
        {
            if (ReadForm(calculation))
            { 
                DataManager_2D.PerformCalculation(calculation);
                this.Close();
            
            }


        }


        private void SeriesComboboxTextChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex != -1) AddSeriesCombobox();
        }

        private void AddSeriesCombobox()
        { 
            ComboBox cb = new ComboBox();
            cb.Location = new Point(column2_x, y_current);
            cb.Items.AddRange(avaiable_containers.ToArray());
            cb.SelectedIndexChanged += SeriesComboboxTextChanged;
            comboBoxes_series.Add(cb);
            this.Controls.Add(cb);
            y_current += y_step;
       
        }

        private void button_select_Click(object sender, EventArgs e)
        {
            SelectItem();
        }

        private void button_execute_Click(object sender, EventArgs e)
        {
            Execute();
        }
    }
}
