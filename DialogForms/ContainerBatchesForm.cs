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
    public partial class ContainerBatchesForm : Form
    {
        public ContainerBatchesForm()
        {
            InitializeComponent();

           // dataGridView1.AutoGenerateColumns = false;
           dataGridView1.AllowUserToAddRows = false;
            dataGridView1.DataSource = ImageManager.containerBatches;
        }






    }
}
