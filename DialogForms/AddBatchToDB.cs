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
    
    
    
    public partial class AddBatchToDB : Form
    {

        public string[] BatchTypes = new string[] {"123","312" };

        public List<string> filenames = new List<string>();
        private List<string> SampleNames = new List<string>();


        public AddBatchToDB()
        {
            InitializeComponent();
        }









    }
}
