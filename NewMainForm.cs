using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace ImgAnalyzer
{
    public partial class NewMainForm : Form
    {
        private string placeholderText1 = "Комментарии относительно образца - тип смеси, особенности сборки итд...";
        private bool isPlaceholderActive1 = true;
        private string placeholderText2 = "Комментарии про особенности измерения, режимы ITO/BIAS, \n"+
                                           "параметры калибровки (стартовое значенеи и шаг ЦАП)";
        private bool isPlaceholderActive2 = true;

        private int selectedSampleId = -1;
        private List<string> sampleList = new List<string>();




        public NewMainForm()
        {
            InitializeComponent();

            richTextBox_sample.Text = placeholderText1;
            richTextBox_sample.ForeColor = Color.Gray;
            richTextBox_batch.Text = placeholderText2;
            richTextBox_batch.ForeColor = Color.Gray;
            richTextBox_sample.Enter += richTextBox_sample_Enter;

            /*richTextBox_sample.Leave += richTextBox_sample_L0.e
                130000002
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
          .000000
                .............................................................................                               
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
                
               ve;*/
            richTextBox_sample.Leave += richTextBox_sample_Leave;
            richTextBox_batch.Enter += richTextBox_batch_Enter;
            richTextBox_batch.Leave += richTextBox_batch_Leave;
            richTextBox_sample.KeyDown += richTextBox_sample_KeyDown;
            richTextBox_batch.KeyDown += richTextBox_batch_KeyDown;


            UpdateSamplesList();

        }

        private void SelectSample(string Name)
        { 
            selectedSampleId = SamplesDB.GetSampleId(Name);
            string comment = SamplesDB.GetSampleComment(selectedSampleId);
            if (comment.Length > 0)
            {
                richTextBox_sample.ForeColor = Color.Black;
                richTextBox_sample.Text = comment;
                isPlaceholderActive1 = false;
            }
            
            else
            {
                richTextBox_sample.Clear(); 
                richTextBox_sample_Leave(this,EventArgs.Empty);
            }
        
        }

        private void UpdateComment()
        {
            SamplesDB.UpdateSampleComment(selectedSampleId,richTextBox_sample.Text);
        }

        private void UpdateSamplesList()
        {
            sampleList = SamplesDB.GetSamplesList();
            comboBox_sample.Items.Clear();
            comboBox_sample.Items.AddRange(sampleList.ToArray());

        }

        private void AddSample()
        {
            string newSampleName = Interaction.InputBox("Введите ия образца");
            if (newSampleName == "") return;
            selectedSampleId = SamplesDB.AddSample(newSampleName);
            UpdateSamplesList();
            SelectSample(newSampleName);
        }




        #region placehorder_text
        private void richTextBox_sample_Enter(object sender, EventArgs e)
        {
            if (isPlaceholderActive1)
            {
                // Очищаем поле и меняем цвет обратно на черный
                richTextBox_sample.Text = "";
                richTextBox_sample.ForeColor = Color.Black;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Regular);
                isPlaceholderActive1 = false;
            }
        }

        private void richTextBox_sample_Leave(object sender, EventArgs e)
        {
            // Если поле пустое, возвращаем плейсхолдер
            if (string.IsNullOrWhiteSpace(richTextBox_sample.Text))
            {
                richTextBox_sample.Text = placeholderText1;
                richTextBox_sample.ForeColor = Color.Gray;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Italic);
                isPlaceholderActive1 = true;
            }
            else UpdateComment();
            
        }

        private void richTextBox_sample_KeyDown(object sender, KeyEventArgs e)
        {
            // Если плейсхолдер активен, но пользователь нажал клавишу (букву, цифру),
            // нужно очистить поле и показать, что он печатает.
            // Это сработает быстрее, чем Enter.
            if (isPlaceholderActive1 && e.KeyCode != Keys.Shift && e.KeyCode != Keys.Control && e.KeyCode != Keys.Alt)
            {
                richTextBox_sample.Text = "";
                richTextBox_sample.ForeColor = Color.Black;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Regular);
                isPlaceholderActive1 = false;

                // Эмулируем нажатие клавиши, которую хотел ввести пользователь
                // (Просто отправляем символ, который был нажат).
                // Если этого не сделать, первая нажатая буква пропадет.
                if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Enter)
                {
                    char key = (char)e.KeyValue;
                    if (char.IsLetterOrDigit(key) || char.IsPunctuation(key) || char.IsWhiteSpace(key))
                    {
                        // SendKeys не самый надежный метод, но для простых случаев подходит.
                        // Лучше просто добавить символ вручную.
                        richTextBox_sample.SelectedText = key.ToString();
                    }
                }
                e.SuppressKeyPress = true; // Предотвращаем стандартную обработку, чтобы не дублировать символ
            }
        }

        private void richTextBox_batch_Enter(object sender, EventArgs e)
        {
            if (isPlaceholderActive2)
            {
                // Очищаем поле и меняем цвет обратно на черный
                richTextBox_batch.Text = "";
                richTextBox_batch.ForeColor = Color.Black;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Regular);
                isPlaceholderActive2 = false;
            }
        }

        private void richTextBox_batch_Leave(object sender, EventArgs e)
        {
            // Если поле пустое, возвращаем плейсхолдер
            if (string.IsNullOrWhiteSpace(richTextBox_batch.Text))
            {
                richTextBox_batch.Text = placeholderText2;
                richTextBox_batch.ForeColor = Color.Gray;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Italic);
                isPlaceholderActive2 = true;
            }
        }

        private void richTextBox_batch_KeyDown(object sender, KeyEventArgs e)
        {
            // Если плейсхолдер активен, но пользователь нажал клавишу (букву, цифру),
            // нужно очистить поле и показать, что он печатает.
            // Это сработает быстрее, чем Enter.
            if (isPlaceholderActive2 && e.KeyCode != Keys.Shift && e.KeyCode != Keys.Control && e.KeyCode != Keys.Alt)
            {
                richTextBox_batch.Text = "";
                richTextBox_batch.ForeColor = Color.Black;
                //richTextBox_sample.Font = new Font(richTextBox_sample.Font, FontStyle.Regular);
                isPlaceholderActive2 = false;

                // Эмулируем нажатие клавиши, которую хотел ввести пользователь
                // (Просто отправляем символ, который был нажат).
                // Если этого не сделать, первая нажатая буква пропадет.
                if (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete && e.KeyCode != Keys.Enter)
                {
                    char key = (char)e.KeyValue;
                    if (char.IsLetterOrDigit(key) || char.IsPunctuation(key) || char.IsWhiteSpace(key))
                    {
                        // SendKeys не самый надежный метод, но для простых случаев подходит.
                        // Лучше просто добавить символ вручную.
                        richTextBox_batch.SelectedText = key.ToString();
                    }
                }
                e.SuppressKeyPress = true; // Предотвращаем стандартную обработку, чтобы не дублировать символ
            }
        }






        #endregion

        private void button_newSample_Click(object sender, EventArgs e)
        {
            AddSample();
        }

        private void comboBox_sample_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectSample(comboBox_sample.Text);
        }
    }
}
