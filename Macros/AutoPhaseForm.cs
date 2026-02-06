using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer.Macros
{
    public partial class AutoPhaseForm : Form
    {
        public Color textColor = Color.Black;

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



    }
}
