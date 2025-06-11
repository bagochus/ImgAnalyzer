using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImgAnalyzer;

namespace ImgAnalyzer.DialogForms
{
    public partial class EditRangeForm : Form
    {
        int nmax;
        int from;
        int to;
        LoadFileListDeleggate loadFilenames;
        string[] filenames;

        public EditRangeForm(MainPresenter imageProcessor)
        {
            InitializeComponent();
            if (imageProcessor == null) return;
            if (imageProcessor.filenames.Length == 0) return;
            nmax = imageProcessor.filenames.Length - 1 ;
            textBox_from.Text = "0";
            textBox_to.Text = nmax.ToString();
            this.loadFilenames = imageProcessor.LoadFileList;
            this.filenames = imageProcessor.filenames;
        }

        private void button_edit_Click(object sender, EventArgs e)
        {
            bool read_ok = false;
            try
            {
                from = Int32.Parse(textBox_from.Text);
                to = Int32.Parse(textBox_to.Text);
                read_ok = true;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            if (read_ok) 
            {
                if (from > to || from < 0 || to > nmax)
                {
                    MessageBox.Show("Неверный диапазон!");
                    return;
                }
                else
                {
                    string[] new_filenames = new string[to-from];

                    for (int i = 0; i < new_filenames.Length; i++) 
                        new_filenames[i] = filenames[i+from];
                    loadFilenames (new_filenames);
                    this.Close();
                }
                
            
            
            
            }





        }


    }
}
