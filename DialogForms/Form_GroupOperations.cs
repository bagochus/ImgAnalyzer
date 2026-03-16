using ImgAnalyzer._2D;
using ImgAnalyzer._2D.GroupOperations;
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
    public partial class Form_GroupOperations : Form
    {

        private List<Type> types = new List<Type>();

        private List<Label> labels_single = new List<Label>();
        private List<TextBox> values_single = new List<TextBox>();

        private List<Label> labels_containers = new List<Label>();
        private List<ComboBox> comboBox_containers = new List<ComboBox>();

        private List<Label> labels_sources = new List<Label>();
        private List<ComboBox> comboBox_sources = new List<ComboBox>();



        private List<string> avaiable_containers = new List<string>();
        private List<string> avaiable_imgSources = new List<string>();

        private Label singleValuesHeader;
        private Label containerValuesHeader;
        private Label imageSourcesHeader;

        IGroupOperation operation;



        //----------------Positions---------------
        private int column1_x = 230;
        private int column2_x = 330;

        private int y_start = 50;
        private int y_current = 50;
        private int y_step = 30;
        private int default_height;

        public Form_GroupOperations()
        {
            InitializeComponent();
            default_height = this.Height;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Перебираем все типы в сборках и находим те, которые реализуют IMyInterface
            var implementingTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IGroupOperation).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

            // Выводим имена классов
            foreach (var type in implementingTypes)
            {
                listBox_operations.Items.Add(type.Name);
                types.Add(type);
            }

            foreach (var c in DataManager_2D.containers) { avaiable_containers.Add(c.Name); }

            foreach (var c in ImageManager.imageSources) { avaiable_imgSources.Add(c.Name); }


        }

        private void UpdateFormHeight()
        {
            if (Height - y_current < 50) Height += 50;

        }

        private void ConstructForm(IGroupOperation op)
        {
            
            

            label_description.Text = op.Description;
            y_current += label_description.Height;

            if (op.SingleValueNames.Length > 0)
            {
                singleValuesHeader = new Label();
                singleValuesHeader.Text = "Константы";
                singleValuesHeader.Location = new Point(column1_x, y_current);
                y_current += y_step;
                this.Controls.Add(singleValuesHeader);

                for (int i = 0; i < op.SingleValueNames.Length; i++)
                {
                    Label l = new Label();
                    l.Text = op.SingleValueNames[i];
                    l.Location = new Point(column1_x, y_current);
                    this.Controls.Add(l);
                    labels_single.Add(l);
                    TextBox t = new TextBox();
                    t.Location = new Point(column2_x, y_current);
                    this.Controls.Add(t);
                    values_single.Add(t);
                    y_current += y_step;
                }

            }

            if (op.ContainerNames.Length > 0)
            {
                containerValuesHeader = new Label();
                containerValuesHeader.Text = "2D - Массивы";
                containerValuesHeader.Location = new Point(column1_x, y_current);
                y_current += y_step;
                this.Controls.Add(containerValuesHeader);

                for (int i = 0; i < op.ContainerNames.Length; i++)
                {
                    Label l = new Label();
                    l.Text = op.ContainerNames[i];
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

            if (op.imageSourceNames.Length > 0)
            {
                imageSourcesHeader = new Label();
                imageSourcesHeader.Text = "Массивы 2D карт";
                imageSourcesHeader.Location = new Point(column1_x, y_current);
                y_current += y_step;
                this.Controls.Add(imageSourcesHeader);

                for (int i = 0; i < op.imageSourceNames.Length; i++)
                {
                    Label l = new Label();
                    l.Text = op.imageSourceNames[i];
                    l.Location = new Point(column1_x, y_current);
                    this.Controls.Add(l);
                    labels_sources.Add(l);
                    ComboBox cb = new ComboBox();
                    cb.Location = new Point(column2_x, y_current);
                    cb.Items.AddRange(avaiable_imgSources.ToArray());
                    this.Controls.Add(cb);
                    comboBox_sources.Add(cb);
                    y_current += y_step;
                }
            }

            UpdateFormHeight();


        }


        private void ClearForm()
        {

            RemoveControl(singleValuesHeader);
            RemoveControl(containerValuesHeader);
            RemoveControl(imageSourcesHeader);

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

            foreach (ComboBox cb in comboBox_sources)
            {
                RemoveControl(cb);
            }
            comboBox_sources.Clear();

            foreach (Label l in labels_single) RemoveControl(l);
            labels_single.Clear();
            foreach (Label l in labels_containers) RemoveControl(l);
            labels_containers.Clear();
            foreach (Label l in labels_sources) RemoveControl(l);
            labels_sources.Clear();



            y_current = y_start;
            Height = default_height;
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
            int index = listBox_operations.SelectedIndex;
            if (index == -1) return;
            var opr = Activator.CreateInstance(types[index]) as IGroupOperation;
            operation = opr;
            ClearForm();
            ConstructForm(opr);
        }

        private async void Execute()
        {
            if (ReadForm(operation))
            {
                await operation.Execute();
                this.Close();

            }


        }




        private bool ReadForm(IGroupOperation op)
        {
            bool input_valid = true;
            try
            {
                if (op.SingleValueNames.Length > 0)
                {
                    double[] SingleValueParameters = new double[op.SingleValueNames.Length];
                    for (int i = 0; i < op.SingleValueNames.Length; i++)
                    {
                        SingleValueParameters[i] = Double.Parse(this.values_single[i].Text);
                    }
                    op.SingleValueParameters = SingleValueParameters;
                }

                if (op.ContainerNames.Length > 0)
                {
                    IContainer_2D[] containers = new IContainer_2D[op.ContainerNames.Length];
                    for (int i = 0; i < containers.Length; i++)
                    {
                        int selected = comboBox_containers[i].SelectedIndex;
                        if (selected == -1) throw new Exception("Не выбран массив " + op.ContainerNames[i]);
                        containers[i] = DataManager_2D.containers[selected];
                    }
                    op.ContainerParameters = containers;
                }

                if (op.imageSourceNames.Length > 0)
                {
                    IImageSource[] sources = new IImageSource[op.imageSourceNames.Length];
                    for (int i = 0; i < sources.Length; i++)
                    {
                        int selected = comboBox_sources[i].SelectedIndex;
                        if (selected == -1) throw new Exception("Не выбран массив " + op.imageSourceNames[i]);
                        sources[i] = ImageManager.imageSources[selected];
                    }
                    op.imageSources = sources;
                }

                op.UseTransformation = checkBox_use_ct.Checked;
                if (checkBox_use_ct.Checked)
                {

                    foreach (var s in op.imageSources) input_valid &= s.coordinateTransformation != null;
                }
                if (!input_valid) throw new Exception("Не укзанана система координат");

                input_valid = true;
            }

            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return input_valid;



        }

        private void listBox_operations_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectItem();
        }

        private void button_exec_Click(object sender, EventArgs e)
        {
            Execute();
        }
    }
}
