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
    public partial class SourceSelectForm : Form
    {
        public List<IImageSource> Sources = new List<IImageSource>();
        public SourceSelectForm()
        {
            InitializeComponent();
            listBox1.SelectionMode = SelectionMode.MultiExtended;
            foreach (IImageSource s in ImageManager.imageSources) listBox1.Items.Add(s.Name);

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            foreach (int i in listBox1.SelectedIndices) Sources.Add(ImageManager.imageSources[i]);  
            this.Close();
        }
    }
}
