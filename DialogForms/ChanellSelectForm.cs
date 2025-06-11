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
    public partial class ChanellSelectForm : Form
    {
        public int ItemSelected = -1;

        public ChanellSelectForm()
        {
            InitializeComponent();
        }

        private void ChanellSelectForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ItemSelected = 0;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ItemSelected = 1;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ItemSelected = 2;
            this.Close();
        }
    }
}
