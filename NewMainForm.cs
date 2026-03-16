using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ImgAnalyzer.DialogForms;
using ImgAnalyzer.Macros;
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

        //private enum PhaseCalculationMode {None, UseImages, UseBatch }
        private PhaseCalculationMode phaseCalculationMode;

        //private enum StitchMode { None, Calculate, UseBatch }
        private StitchMode stitchMode = StitchMode.Calculate;

        private bool useImages_prev_state = true;
        private bool useAutoSquare = true;
        private bool generateLUT = true;
        private bool generateLUT_prev_state = true;

        string input_text = "";

        private ContainerBatch phaseBatch, stitchBatch;

        private enum BatchCommentBinding { New, Phase, Stitch }
        private BatchCommentBinding commentBinding = BatchCommentBinding.New;



        public NewMainForm()
        {
            InitializeComponent();


            radioButton_calc.Click += Phase_click;
            radioButton_batch_phase.Click += Phase_click;

            radioButton_nostitich.Click += StitchClick;
            radioButton_stitch.Click += StitchClick;
            radioButton_batch_stitch.Click += StitchClick;

            radioButton_lut.Click += LutClick;
            radioButton_no_lut.Click += LutClick;


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

        private void Phase_click(object sender, EventArgs e)
        {
            if (radioButton_calc.Checked)
            {
                phaseCalculationMode = PhaseCalculationMode.UseImages;
                useImages_prev_state = true;
            }
            else
            {
                phaseCalculationMode = PhaseCalculationMode.UseBatch;
                useImages_prev_state = false;
            }
            ChangeCalculationMode();
        
        }


        private void StitchClick(object sender, EventArgs e)
        { 
            if (radioButton_nostitich.Checked) stitchMode = StitchMode.None;
            if (radioButton_stitch.Checked) stitchMode = StitchMode.Calculate;
            if (radioButton_batch_stitch.Checked) stitchMode = StitchMode.UseBatch;
            ChangeStitchMode();
        }

        private void LutClick(object sender, EventArgs e)
        { 
            generateLUT = radioButton_lut.Checked;
            ChangeLLUTMode();
        }


        private void ChangeCalculationMode()
        {

            switch (phaseCalculationMode)
            {
                case PhaseCalculationMode.None:
                    button_hintphase.Visible = false;
                    button_select_phase.Visible = false;
                    radioButton_batch_phase.Checked = false;
                    radioButton_calc.Checked = false;
                    label_selected_phase.Visible = false;
                    break;
                case PhaseCalculationMode.UseImages:
                    radioButton_calc.Checked = true;
                    button_hintphase.Visible = true;
                    button_select_phase.Visible = false;
                    label_selected_phase.Visible = false;
                    richTextBox_batch.Text = input_text;
                    commentBinding = BatchCommentBinding.New;
                    richTextBox_batch.Text = input_text;
                    richTextBox_batch_Leave(this, EventArgs.Empty);
                    if (stitchMode == StitchMode.UseBatch)
                    {
                        stitchMode = StitchMode.Calculate;
                        ChangeStitchMode();
                    }
                    break;
                case PhaseCalculationMode.UseBatch:
                    radioButton_batch_phase.Checked = true;
                    button_select_phase.Visible = true;
                    label_selected_phase.Visible = true;
                    button_hintphase.Visible = false;
                    richTextBox_batch.Text = phaseBatch?.comment ?? string.Empty;
                    richTextBox_batch_Leave(this, EventArgs.Empty);

                    if (stitchMode == StitchMode.UseBatch)
                    {
                        stitchMode = StitchMode.Calculate;
                        ChangeStitchMode();
                    }
                    break;
            }
            
        }

        private void ChangeStitchMode()
        {
            switch (stitchMode)
            {
                case StitchMode.None:
                    radioButton_nostitich.Checked = true;
                    radioButton_lut.Enabled = false; 
                    radioButton_no_lut.Enabled = false;
                    radioButton_lut.Checked = false;
                    radioButton_no_lut.Checked = true;
                    button_hint_loot.Visible = false;
                    if (phaseCalculationMode == PhaseCalculationMode.None)
                    { 
                        if (useImages_prev_state) phaseCalculationMode = PhaseCalculationMode.UseImages;
                        else phaseCalculationMode = PhaseCalculationMode.UseBatch;
                        ChangeCalculationMode();
                    }


                    break;
                    case StitchMode.Calculate:
                    //
                    radioButton_stitch.Checked = true;
                    //lut ok
                    radioButton_lut.Enabled = true;
                    radioButton_no_lut.Enabled = true;
                    radioButton_lut.Checked = true;
                    radioButton_no_lut.Checked = false;
                    button_hint_loot.Visible = false;
                    //select off
                    button_select_stitch.Visible = false;
                    label_selected_stitch.Visible = false;
                    //algo on
                    button_hint_stitch.Visible = true;
                    //phase calc restore
                    if (phaseCalculationMode == PhaseCalculationMode.None)
                    {
                        if (useImages_prev_state) phaseCalculationMode = PhaseCalculationMode.UseImages;
                        else phaseCalculationMode = PhaseCalculationMode.UseBatch;
                        ChangeCalculationMode();
                    }
                    break;
                    case StitchMode.UseBatch:
                    //
                    radioButton_batch_stitch.Checked = true;
                    //lut possible
                    radioButton_lut.Enabled = true;
                    radioButton_no_lut.Enabled = true;
                    radioButton_lut.Checked = true;
                    radioButton_no_lut.Checked = false;
                    button_hint_loot.Visible = false;
                    //alg off
                    button_hint_stitch.Visible = false;
                    //select on
                    button_select_stitch.Visible = true;
                    label_selected_stitch.Visible = true;
                    //phase calc off
                    phaseCalculationMode = PhaseCalculationMode.None;
                    ChangeCalculationMode();
                    commentBinding = BatchCommentBinding.Stitch;
                    richTextBox_batch.Text = stitchBatch?.comment ?? string.Empty;
                    richTextBox_batch_Leave(this, EventArgs.Empty);

                    radioButton_calc.Checked = false;
                    radioButton_batch_phase.Checked = false;
                    button_hintphase.Visible = false;
                    break;
            }
        }

        private void ChangeLLUTMode()
        {
            button_hint_loot.Visible = generateLUT;
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

        private void SelectPhaseBatch()
        {
            
            var batch = ContainerBatchesForm.GetBatch(SamplesDB.GetSampleName(selectedSampleId), BatchDatatypes.PhaseWrapped);
            if (batch != null)
            { 
                phaseBatch = batch;
                label_selected_phase.Text = "Выбрано: " + phaseBatch.Name;
                commentBinding = BatchCommentBinding.Phase;

                if (batch.comment.Length > 0)
                {

                    if (!isPlaceholderActive2) input_text = richTextBox_batch.Text;
                    richTextBox_batch.ForeColor = Color.Black;
                    richTextBox_batch.Text = batch.comment;
                    isPlaceholderActive2 = false;
                }
            }
        
        }

        private void SelectStitchBatch()
        {
            var batch = ContainerBatchesForm.GetBatch(SamplesDB.GetSampleName(selectedSampleId), BatchDatatypes.PhaseUnwrapped);
            if (batch != null)
            {
                stitchBatch = batch;
                label_selected_stitch.Text = "Выбрано: " + phaseBatch.Name;
                commentBinding = BatchCommentBinding.Stitch;

                if (batch.comment.Length > 0)
                {
                    
                    if (!isPlaceholderActive2) input_text = richTextBox_batch.Text;
                    richTextBox_batch.ForeColor = Color.Black;
                    richTextBox_batch.Text = batch.comment;
                    isPlaceholderActive2 = false;
                }
            }
        }

        private void Start()
        {
            if (phaseCalculationMode == PhaseCalculationMode.UseBatch && phaseBatch == null)
            {
                MessageBox.Show("Не выбран пакет данных с фазовыми картами!");
                return;
            }
            if (stitchMode == StitchMode.UseBatch && stitchBatch == null)
            {
                MessageBox.Show("Не выбран пакет данных с фазовыми картами!");
                return;
            }
            bool combination_fail = false;
            combination_fail |= (phaseCalculationMode == PhaseCalculationMode.UseBatch
                && stitchMode == StitchMode.UseBatch);
            combination_fail |= (generateLUT && stitchMode == StitchMode.None);
            combination_fail |= (phaseCalculationMode == PhaseCalculationMode.None
                && stitchMode == StitchMode.None);

            if (combination_fail)
            {
                MessageBox.Show("Недопустимая комбинация!");
                return;
            }

            AutoPhase autoPhase = new AutoPhase();
            autoPhase.stitchMode = stitchMode;
            autoPhase.calculationMode = phaseCalculationMode;
            autoPhase.useAutoSquare = useAutoSquare;
            autoPhase.generateLUT = generateLUT;
            autoPhase.requestParams = checkBox_req_params.Checked;
            autoPhase.sample_id = selectedSampleId;



            if (phaseCalculationMode == PhaseCalculationMode.UseBatch) autoPhase.phaseBatch = phaseBatch;   
            if(stitchMode == StitchMode.UseBatch) autoPhase.stitchedPhaseBatch = stitchBatch;

            //Task.Run(() => autoPhase.Run());

            var log_form = new LogForm();
            Thread thread = new Thread(() => autoPhase.Run(log_form));
            log_form.Show();

            
            thread.Start();

            Close();

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
            else if (commentBinding == BatchCommentBinding.Phase) phaseBatch?.UpdateComment(richTextBox_batch.Text);
            else if (commentBinding == BatchCommentBinding.Stitch) stitchBatch?.UpdateComment(richTextBox_batch.Text);
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            useAutoSquare = checkBox_auto_sq.Checked;
            button_infoAutoSquare.Enabled = checkBox_auto_sq.Checked;
        }

        private void button_select_stitch_Click(object sender, EventArgs e)
        {
            SelectStitchBatch();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button_start_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SelectPhaseBatch();
        }
    }
}
