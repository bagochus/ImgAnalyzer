using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer.DialogForms;
using System.Globalization;


namespace ImgAnalyzer
{
    public enum ClickMode {None, MeasurePixel, MeasurePoly, MeasureMatrix}
    public partial class MainForm_old : Form
    {
        IFormatProvider frmt = new NumberFormatInfo { NumberDecimalSeparator = "." };
        MainPresenter mainPresenter = new MainPresenter();
        private ClickMode clickMode = ClickMode.None;
        int corner_index = 0;
        Point[] corners = new Point[4];

        private List<Point> CatchedPoints = new List<Point>();
        private int point_index = 0;
        private int nx;
        private int ny;


        private string[] batch_filanames;

        public double x_start = 0;
        public double x_step = 1;
        public string x_axis_unit = "";
        public string x_axis_variable = "";
        public int[] selected_items;



        public MainForm_old()
        {
            InitializeComponent();
            mainPresenter.ListUpdated += UpdateMeasurmentList;
        }

        private void UpdateMeasurmentList(object sender, EventArgs e) 
        {
            listBox_measurments.Items.Clear();
            listBox_measurments.Items.AddRange(mainPresenter.GetMeasurnetNames().ToArray());
        }

        public void HookClick(Point point)
        {
            switch (clickMode)
            {
                case ClickMode.None: break;
                case ClickMode.MeasurePixel: 
                    mainPresenter.AddPointMeasurment(point);
                    break;
                case ClickMode.MeasurePoly:
                    CatchedPoints.Add(point);
                    point_index++;
                    break;
                case ClickMode.MeasureMatrix:
                    CatchedPoints.Add(point);
                    point_index++;
                    if (point_index == 4) {
                        mainPresenter.AddNewMatrixMeasurnment(CatchedPoints.ToArray(), nx, ny);
                        CatchedPoints.Clear();
                        point_index = 0;
                        clickMode = ClickMode.None;
                    }
                    break;
            }
            UpdateButtonState();
        }



        private void button_openfile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Устанавливаем фильтр для файлов .tiff
            openFileDialog.Filter = "TIFF Files (*.tiff)|*.tiff|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1; // Устанавливаем фильтр по умолчанию
            openFileDialog.RestoreDirectory = true; // Восстанавливаем предыдущую директорию

            // Показываем диалоговое окно
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Получаем выбранный файл
                string selectedFilePath = openFileDialog.FileName;
                mainPresenter.LoadImage(selectedFilePath);

                // Здесь можно добавить код для работы с выбранным файлом
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (mainPresenter.image == null && mainPresenter.filenames == null) return;
            if (mainPresenter.image == null) mainPresenter.LoadImage(mainPresenter.filenames[0]);
            ImageForm form2 = new ImageForm(mainPresenter.image);
            form2.Show();
            form2.ImageClicked += HookClick;
            /*form2.ImageClicked += (clickPoint) =>
            {
                MessageBox.Show($"Клик на координатах формы: X={clickPoint.X}, Y={clickPoint.Y}"
                    +"\n"+ "Интенсивность:"+
                    mainPresenter.MeasurePixel(clickPoint.X,clickPoint.Y).ToString());

            };*/
        }



        private void textBox_varname_TextChanged(object sender, EventArgs e)
        {
            label_var1.Text = textBox_unitname.Text;
            label_var2.Text = textBox_unitname.Text;
        }

        private void UpdateButtonState()
        {
            button_addpoints.BackColor = SystemColors.Control;
            button_addmatrix.BackColor = SystemColors.Control;
            button_addmatrix.Text = "Выбрать полигон";
            button_addpolygon.BackColor = SystemColors.Control;
            button_addmatrix.Text = "Выбрать матрицу";

            switch (clickMode) 
            {
                case ClickMode.None: break;
                case ClickMode.MeasurePixel:
                    button_addpoints.BackColor = Color.LightGreen;
                    break;
                case ClickMode.MeasurePoly:
                    button_addpolygon.BackColor = Color.LightGreen;
                    button_addpolygon.Text = "Выбрать полигон: " +
                        (point_index + 1).ToString() + " точек";
                    break;
                case ClickMode.MeasureMatrix:
                    button_addmatrix.BackColor= Color.LightGreen;
                    button_addmatrix.Text = "Выбрать матрицу: точка" +
                        (point_index + 1).ToString() + "/4";
                    break;
            }
        }



        private void button_addpoints_Click(object sender, EventArgs e)
        {
            if (clickMode == ClickMode.MeasurePixel) clickMode = ClickMode.None;
            else
            {
                clickMode = ClickMode.MeasurePixel;
                CatchedPoints.Clear();
                point_index = 0;
            }
            UpdateButtonState();
        }

        private void button_addpolygon_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickMode.MeasurePoly)
            {
                clickMode = ClickMode.MeasurePoly;
                CatchedPoints.Clear();
                point_index = 0;
            }
            else 
            {
                if (point_index >= 3)
                {
                    mainPresenter.AddPolygonMeasurment(CatchedPoints.ToArray());

                }
                else 
                {
                    MessageBox.Show("Для полигона нужно минимум 3 точки");
                }
                CatchedPoints.Clear();
                point_index = 0;
                clickMode = ClickMode.None;
            }
            UpdateButtonState();
        }

        private void button_addmatrix_Click(object sender, EventArgs e)
        {
            if (clickMode != ClickMode.MeasureMatrix)
            {

                bool parse_ok = true;
                parse_ok &= Int32.TryParse(textBox_nx.Text, out nx);
                parse_ok &= Int32.TryParse(textBox_ny.Text, out ny);
                if (!parse_ok)
                {
                    MessageBox.Show("Ошибка ввода");
                    return;
                }
                clickMode = ClickMode.MeasureMatrix;
                CatchedPoints.Clear();
                point_index = 0;
            }
            else
            {
                clickMode = ClickMode.None;
                CatchedPoints.Clear();
                point_index = 0;
            }
            UpdateButtonState();
        }

        private void button_openfiles_Click(object sender, EventArgs e)
        {
            var filePaths = new List<string>();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "TIFF Files (*.tiff;*.tif)|*.tiff;*.tif|All Files (*.*)|*.*";
                openFileDialog.Multiselect = true; // Разрешаем выбор нескольких файлов
                openFileDialog.Title = "Выберите TIFF файлы";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePaths.AddRange(openFileDialog.FileNames);
                }
            }
            if (filePaths.Count > 0) 
                mainPresenter.filenames = filePaths.ToArray();
        }

        private void button_start_Click(object sender, EventArgs e)
        {
            if (mainPresenter.filenames == null) return;
            label_status.Text = "Обработка начата";
            mainPresenter.BatchMeasurment();
            label_status.Text = "Обработка закончена";



        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            selected_items = listBox_measurments.SelectedIndices.Cast<int>().ToArray();
            Double.TryParse(textBox_start.Text, out x_start);
            Double.TryParse(textBox_step.Text, out x_step);
            x_axis_unit = textBox_unitname.Text;

            //PlotForm plotForm = new PlotForm(this, mainPresenter,false);
            //plotForm.Show();
        }

        private void textBox_varname_TextChanged_1(object sender, EventArgs e)
        {
            x_axis_variable = textBox_varname.Text; 
        }

        private void button_rename_Click(object sender, EventArgs e)
        {
            if (listBox_measurments.SelectedItems.Count == 0) return;
            selected_items = listBox_measurments.SelectedIndices.Cast<int>().ToArray();
            int selectedIndex = selected_items[0];


            string userInput = Interaction.InputBox("Введите новое имя:",
                "Переименовать поле",
                mainPresenter.measurements[selectedIndex].Name);
            if (!string.IsNullOrEmpty(userInput))
            {
                mainPresenter.RenameItem(selectedIndex, userInput);
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            if (listBox_measurments.SelectedItems.Count == 0) return;
            selected_items = listBox_measurments.SelectedIndices.Cast<int>().ToArray();
            for (int i = selected_items.Length - 1 ; i >= 0; i--)
            {
                mainPresenter.DeleteItem(selected_items[i]);
            }

        }

        private void button_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            // Настраиваем параметры диалога
            saveFileDialog.Filter = "Файлы JSON (*.json)|*.json";
            saveFileDialog.FilterIndex = 1; // Устанавливаем фильтр по умолчанию
            saveFileDialog.Title = "Сохранить файл";
            saveFileDialog.DefaultExt = "json"; // Расширение по умолчанию
            saveFileDialog.AddExtension = true; // Автоматически добавлять расширение
            saveFileDialog.OverwritePrompt = true; // Предупреждать о перезаписи файла

            // Показать диалог и проверить, нажал ли пользователь OK
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Здесь код для сохранения файла
                    string filePath = saveFileDialog.FileName;
                    //mainPresenter.SaveMeasurmentList(filePath);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            try
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    // Настройки диалога
                    openFileDialog.Filter = "JSON files (*.json)|*.json";
                    openFileDialog.DefaultExt = "json";
                    openFileDialog.AddExtension = true;
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.CheckPathExists = true;

                    // Показываем диалог
                    if (openFileDialog.ShowDialog() != DialogResult.OK)
                        return;

                    // Дополнительная проверка расширения
                    //mainPresenter.LoadMeasurmentList(openFileDialog.FileName);

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выборе файла: {ex.Message}");
            }
        }

        private void button_calcminmax_Click(object sender, EventArgs e)
        {
            mainPresenter.FindMinMax();
        }

        private void button_pseudoph_Click(object sender, EventArgs e)
        {
            PseudoPhaseForm form = new PseudoPhaseForm(mainPresenter, batch_filanames);
            form.ShowDialog();
        }

        private void button_deadpix_Click(object sender, EventArgs e)
        {
            MarkDeadPixelsForm form = new MarkDeadPixelsForm(mainPresenter);
            form.Show();
        }

        private void button_map_Click(object sender, EventArgs e)
        {
            ValueMapForm valueMapForm = new ValueMapForm(mainPresenter);
            valueMapForm.Show();
        }

        private void button_ab_Click(object sender, EventArgs e)
        {
            int value = 0;
            string userInput = Interaction.InputBox("Введите размер блока устреднения:",
               "Поиск максимумов интенсивности",
               "4");
            if (Int32.TryParse(userInput, NumberStyles.Any, frmt, out value))
            {
                mainPresenter.CalculateABByBlocks(value);
            }
            
        }

        private void button_peaks_Click(object sender, EventArgs e)
        {
            int value1 = 0;
            int value2 = 0;
            bool good_input = false;
            string userInput = Interaction.InputBox("Ведите погрешность поиска пика:",
               "Поиск положения максимумов",
               "500");
            good_input = (Int32.TryParse(userInput, NumberStyles.Any, frmt, out value1));
            if (good_input) 
                userInput = Interaction.InputBox("Ведите размер блока:",
               "Поиск положения максимумов",
               "4");
            good_input = (Int32.TryParse(userInput, NumberStyles.Any, frmt, out value2));
            if (good_input) mainPresenter.CalculateABPosByBlock(value1,value2);


        }

        private void button_editrange_Click(object sender, EventArgs e)
        {
            EditRangeForm form = new EditRangeForm(mainPresenter);
            form.ShowDialog();
        }

        private void button_invrange_Click(object sender, EventArgs e)
        {
            string[] new_filenames = new string[mainPresenter.filenames.Length];
            int imax = mainPresenter.filenames.Length - 1;
            for (int i = 0; i < mainPresenter.filenames.Length; i++)
                new_filenames[i] = mainPresenter.filenames[imax-i];
            mainPresenter.filenames = new_filenames;
        }

        private void button_calc_phase_Click(object sender, EventArgs e)
        {
            if (mainPresenter.filenames == null) return;
            label_status.Text = "Обработка начата";
            mainPresenter.BatchPhaseMeasurment();
            label_status.Text = "Обработка закончена";



        }

        private void button_plotphase_Click(object sender, EventArgs e)
        {
            selected_items = listBox_measurments.SelectedIndices.Cast<int>().ToArray();
            Double.TryParse(textBox_start.Text, out x_start);
            Double.TryParse(textBox_step.Text, out x_step);
            x_axis_unit = textBox_unitname.Text;

            //PlotForm plotForm = new PlotForm(this, mainPresenter,true);
            //plotForm.Show();
        }

        private void button_test_Click(object sender, EventArgs e)
        {
            ImageViewForm form = new ImageViewForm(ImageManager.Batch_A());
            form._presenter = mainPresenter;
            form.LoadImage(mainPresenter.filenames[0]);
            form.Show();
        }

        private void button_stretch_Click(object sender, EventArgs e)
        {
            mainPresenter.StretchData_test();
        }
    }
}
