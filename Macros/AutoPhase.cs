using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SD = ImgAnalyzer.SettingDefinition;
using System.Drawing;
using ImgAnalyzer._2D;
using ImgAnalyzer._2D.GroupOperations;
using ImgAnalyzer._2D.GroupOperations.SinglePixelOperations;
using System.ComponentModel;
using System.Windows.Forms;
using ImgAnalyzer.DialogForms;

namespace ImgAnalyzer.Macros
{
    public class AutoPhase
    {
        private string folder1, folder2, folder3;
        private static AutoPhase instance = null;
        private Action<string> writeLog = null;
        private AutoPhaseForm form = null;
        


        private int img_step = 1;

        private int active_area_res = 512;
        private double thrContrast = 0.5;
        private double profileContrast = 45;
        private double widthMismath = 0.02;
        private int gridStep = 50;

        double k1, k2, k3, m1, m2, m3;

        double stitch_thr = 70;

        double range_percentile = 99.5;

        public static void Run()
        {
            if (instance == null)
            {
                instance = new AutoPhase();

                try
                {
                    instance._run();
                }
                finally { instance = null; }
            }


        }

        private void _run()
        {
            writeLog("Запрос параметров от пользователя...");
            RequestParams();
            int count_a=0, count_b=0, count_c=0;
            try
            {
                ImageManager.Batch_A().SweepDirectory(folder1, true);
                 count_a = ImageManager.Batch_A().Count;
                writeLog($"Проверка папки А, найдено {count_a} файлов");

                ImageManager.Batch_B().SweepDirectory(folder2, true);
                 count_b = ImageManager.Batch_B().Count;
                writeLog($"Проверка папки B, найдено {count_b} файлов");

                ImageManager.Batch_C().SweepDirectory(folder3, true);
                 count_c = ImageManager.Batch_B().Count;
                writeLog($"Проверка папки C, найдено {count_c} файлов");
            }
            catch (Exception ex)
            {
                form.textColor = Color.Red;
                writeLog("Не удалось выполнить поиск: "+ex.Message);
                return;

            }

            if (count_a != count_b || count_a != count_c)
            {
                form.textColor = Color.Red;
                writeLog("Не совпадает количество файлов!");
                return;
            }
            if (count_a == 0 )
            {
                form.textColor = Color.Red;
                writeLog("Не найдено ни одного файла изображения!");
                return;
            }

            CoordinateTransformation ct1 = null, ct2 = null, ct3 = null;
            try 
            {
                writeLog("Расчет амплитуды для группы изображений А...");
                Container_2D_int ampl_a = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_A(), img_step));
                writeLog("Поиск активной области для группы изображений А");
                var sf1 = new SqareFitter(ampl_a);
                FillFitterField(sf1);
                ct1 = sf1.FindRange();

                writeLog("Расчет амплитуды для группы изображений B...");
                Container_2D_int ampl_b = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_B(), img_step));
                writeLog("Поиск активной области для группы изображений B");
                var sf2 = new SqareFitter(ampl_b);
                FillFitterField(sf2);
                ct2 = sf2.FindRange();

                writeLog("Расчет амплитуды для группы изображений C...");
                Container_2D_int ampl_c = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_C(), img_step));
                writeLog("Поиск активной области для группы изображений C");
                var sf3 = new SqareFitter(ampl_c);
                FillFitterField(sf3);
                ct3 = sf3.FindRange();
            }
            catch (Exception ex) 
            {
                form.textColor = Color.Red;
                writeLog("Не удалось выполнить поиск активных областей");
                writeLog(ex.Message);
                return;
            }

            writeLog("Рачет фазовых профилей...");
            PhaseMeasurmentGroup pmg = new PhaseMeasurmentGroup();
            pmg.SingleValueParameters = new double[] { k1, k2, k3, m1, m2, m3 };
            pmg.imageSources = new IImageSource[]
            { ImageManager.Batch_A(),
                ImageManager.Batch_B(),
                ImageManager.Batch_C() };
            pmg.UseTransformation = true;

            pmg.Execute();

            var phase0 = Container_2D.ReadFromFile(pmg.batch.Filenames[0]);

            writeLog("Сшивка первого фазового профиля...");
            StitchSpatially3 stitch_calc = new StitchSpatially3();

            stitch_calc.SingleValueParameters = new double[] { stitch_thr };

            stitch_calc.ContainerParameters = new IContainer_2D[] { phase0 };

            Container_2D phase_stitched = new Container_2D_double(stitch_calc.MeasureFull());

            writeLog("Сшивка группы фазовых профилей...");
            StitchFramewise sfw = new StitchFramewise();

            sfw.SingleValueParameters = new double[] { stitch_thr };
            sfw.ContainerParameters = new IContainer_2D[] { phase_stitched };
            sfw.imageSources = new IImageSource[] { pmg.batch };
            sfw.Execute();

            var phase_start = Container_2D.ReadFromFile(sfw.batch.Filenames[0]);
            var phase_end = Container_2D.ReadFromFile(sfw.batch.Filenames[sfw.batch.Count-1]);

            double avg_start = ImageProcessor_2D.CalculateAverage(phase_start, 8);
            double avg_end = ImageProcessor_2D.CalculateAverage(phase_end, 8);
            


            writeLog("Расчет диапазона...");
            PhaseRange pr_calc = new PhaseRange();
            pr_calc.SingleValueParameters = new double[] { range_percentile };
            if (avg_start<avg_end)
                pr_calc.ContainerParameters = new IContainer_2D[] { phase_start, phase_end };
            else
                pr_calc.ContainerParameters = new IContainer_2D[] { phase_end, phase_start };
            var container_range = new Container_2D_double(pr_calc.MeasureFull());

            DialogResult result = MessageBox.Show("Рассчитать LU таблицу?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            LU_Table lut_calc = new LU_Table();

            ParameterRequestForm form = new ParameterRequestForm();
            form.AddDoubleRequest("PHASE_0");
            form.AddDoubleRequest("PHASE_255");
            form.AddDoubleRequest("DAC_0");
            form.AddDoubleRequest("DAC_STEP");

            form.ShowDialog();

            double ph0 = form.RequestDouble("PHASE_0");
            double ph255 = form.RequestDouble("PHASE_255");
            double dac_0 = form.RequestDouble("DAC_0");
            double dac_step = form.RequestDouble("DAC_STEP");

            lut_calc.SingleValueParameters = new double[] { ph0, ph255, dac_0, dac_step };
            lut_calc.imageSources = new IImageSource[] { sfw.batch };
            lut_calc.Execute();












        }


        private void FillFitterField(SqareFitter sf)
        {
            sf.aa_size = active_area_res;
            sf.thrContrast = thrContrast;
            sf.profileContrast = profileContrast;
            sf.widthMismath = widthMismath;
            sf.gridStep = gridStep;

        }


        private void RequestParams()
        { 
            List<SettingDefinition> settings = new List<SettingDefinition>();
            settings.Add(SD.CreateGlobal("k1", 2.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("k2", -1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("k3", -1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m1", 0.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m2", 1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m3", -1.0, "Коэффициент для измерения фазы"));

            settings.Add(SD.CreateLocal("FolderA", "C:\\Camera\\13", this,
                "Папка с изображениями с sin камеры"));
            settings.Add(SD.CreateLocal("FolderB", "C:\\Camera\\14", this,
    "Папка с изображениями с cos камеры"));
            settings.Add(SD.CreateLocal("FolderC", "C:\\Camera\\15", this,
    "Папка с изображениями с -cos камеры"));

            settings.Add(SD.CreateLocal("img_step", 5,this, "Шаг по изображениям при расчете амплитуды"));
            settings.Add(SD.CreateLocal("active_area_size", 512, this, "Высота и ширина активной области в псевдопикселях"));


            SettingsManager.RequestSettingList(settings);


            folder1 =   settings.Single(x => x.Name == "FolderA").GetValue<string>();
            folder2 =   settings.Single(x => x.Name == "FolderB").GetValue<string>();
            folder3 =   settings.Single(x => x.Name == "FolderC").GetValue<string>();
            img_step =  settings.Single(x => x.Name == "img_step").GetValue<int>();
            active_area_res = settings.Single(x => x.Name == "active_area_size").GetValue<int>();


        }




        private AutoPhase() 
        {
            form = new AutoPhaseForm();
            form.Show();
            writeLog = form.AppendLog;
        
        
        }
        
        public void Init()
        {






        }










    }
}
