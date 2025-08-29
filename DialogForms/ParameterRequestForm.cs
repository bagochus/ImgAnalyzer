using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.DialogForms
{
    public partial class ParameterRequestForm : Form
    {

        // Словарь для хранения значений параметров
        private Dictionary<string, TextBox> parameterControls = new Dictionary<string, TextBox>();
        private Dictionary<string, Type> parameterTypes = new Dictionary<string, Type>();

        private TextBox firstBox;

        public ParameterRequestForm()
        {
            InitializeComponent();
        }


        // Добавление запроса double параметра
        public void AddDoubleRequest(string parameterName)
        {
            AddParameterRequest(parameterName, typeof(double));
        }

        // Добавление запроса int параметра
        public void AddIntRequest(string parameterName)
        {
            AddParameterRequest(parameterName, typeof(int));
        }

        private void AddParameterRequest(string parameterName, Type parameterType)
        {
            // Проверяем, нет ли уже параметра с таким именем
            if (parameterControls.ContainsKey(parameterName))
            {
                throw new ArgumentException($"Parameter '{parameterName}' already exists.");
            }

            // Создаем Label
            Label label = new Label();
            label.Text = parameterName + ":";
            label.AutoSize = true;
            label.Top = 20 + parameterControls.Count * 30;
            label.Left = 20;

            // Создаем TextBox
            TextBox textBox = new TextBox();
            textBox.Name = "txt" + parameterName;
            textBox.Top = label.Top;
            textBox.Left = 150;
            textBox.Width = 150;
            if (parameterControls.Count == 0) firstBox = textBox;

            // Добавляем элементы на форму
            this.Controls.Add(label);
            this.Controls.Add(textBox);

            // Сохраняем ссылки
            parameterControls.Add(parameterName, textBox);
            parameterTypes.Add(parameterName, parameterType);

            // Увеличиваем высоту формы, если нужно
            if (this.Height < textBox.Bottom + 70)
            {
                this.Height = textBox.Bottom + 70;
            }

            // Перемещаем кнопку OK вниз
            //btnOk.Top = textBox.Bottom + 20;
        }

        // Получение double значения
        public double RequestDouble(string parameterName)
        {
            return Convert.ToDouble(GetParameterValue(parameterName));
        }

        // Получение int значения
        public int RequestInt(string parameterName)
        {
            return Convert.ToInt32(GetParameterValue(parameterName));
        }

        private object GetParameterValue(string parameterName)
        {
            if (!parameterControls.TryGetValue(parameterName, out TextBox textBox))
            {
                throw new ArgumentException($"Parameter '{parameterName}' not found.");
            }

            if (!parameterTypes.TryGetValue(parameterName, out Type parameterType))
            {
                throw new InvalidOperationException($"Parameter type for '{parameterName}' not found.");
            }

            try
            {
                if (parameterType == typeof(double))
                {
                    return double.Parse(textBox.Text, CultureInfo.InvariantCulture);
                }
                else if (parameterType == typeof(int))
                {
                    return int.Parse(textBox.Text, CultureInfo.InvariantCulture);
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported parameter type for '{parameterName}'.");
                }
            }
            catch (FormatException)
            {
                throw new FormatException($"Invalid format for parameter '{parameterName}'. Expected {parameterType.Name}.");
            }
        }

        private void btnOK_Click()
        {
            // Проверяем все параметры
            foreach (var param in parameterControls)
            {
                try
                {
                    // Просто пытаемся получить значение для проверки
                    GetParameterValue(param.Key);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    param.Value.Focus();
                    return;
                }
            }

            // Если все в порядке, закрываем форму с результатом OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnOk_Click_1(object sender, EventArgs e)
        {
            btnOK_Click();
        }

        private void ParameterRequestForm_Shown(object sender, EventArgs e)
        {
            firstBox?.Focus();
        }
    }
}
