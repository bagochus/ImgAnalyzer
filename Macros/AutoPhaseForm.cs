using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.Macros
{
    public partial class AutoPhaseForm : Form
    {
        public Color textColor = Color.Black;

        public Action stopButtonClick = () => { };

        public AutoPhaseForm()
        {
            InitializeComponent();
        }

        public void AppendLog(string message)
        {

            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new Action(() => AppendLog(message)));
                return;
            }

            logBox.SelectionColor = textColor;
            logBox.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            logBox.ScrollToCaret();
        }

        public void ReplaceLastLine(string newText)
        {

            if (logBox.InvokeRequired)
            {
                logBox.Invoke(new Action(() => ReplaceLastLine(newText)));
                return;
            }

            if (string.IsNullOrEmpty(logBox.Text))
            {
                logBox.Text = newText;
                return;
            }

            if (logBox.Lines.Length > 0)
            {
                var lines = logBox.Lines.ToList();
                lines[lines.Count - 1] = newText; // Заменяем последнюю строку
                logBox.Lines = lines.ToArray();

                // Возвращаем курсор в конец
                logBox.SelectionStart = logBox.Text.Length;
                logBox.ScrollToCaret();
            }
            else
            {
                // Если нет строк, просто добавляем новую
                logBox.Text = newText;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            stopButtonClick();
        }
    }
}
