using ImgAnalyzer._2D;
using ImgAnalyzer._2D.Calculations;
using ImgAnalyzer._2D.GroupOperations;
using ImgAnalyzer.DialogForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SD = ImgAnalyzer.SettingDefinition;

namespace ImgAnalyzer.Macros
{
    internal enum StitchMode { None, Calculate, UseBatch }
    internal enum PhaseCalculationMode { None, UseImages, UseBatch }

    public class AutoPhase
    {

        internal StitchMode stitchMode;
        internal PhaseCalculationMode calculationMode;

        public bool generateLUT = true;
        public bool useAutoSquare = true;
        public bool requestParams = true;

        internal ContainerBatch phaseBatch = null;
        internal ContainerBatch stitchedPhaseBatch = null;
        internal string comment = "";

        internal int sample_id = -1;


        List<SettingDefinition> settings = new List<SettingDefinition>();


        private string folder1, folder2, folder3;
        private Action<string> writeLog = null;
        private Action<string> writeErrorLog = null;
        private Action<string> replaceLastLine = null;
        //private AutoPhaseForm form = null;

        public CancellationTokenSource cts;

        private int img_step = 1;

        private int active_area_res = 512;
        private double thrContrast = 0.3;
        private double profileContrast = 20;
        private double widthMismath = 0.02;
        private int gridStep = 50;
        private int autorange_sa_window = 3;

        double k1, k2, k3, m1, m2, m3;

        double stitch_thr = 70;

        double range_percentile = 99.99;

        int lut_max_code = 4095;
        int lut_min_code = 0;
        double phase_range_thr = 30;
        double phase_diff_thr = 10;
        int median_area_r = 3;

        double phase_lo;
        double phase_hi;
        Container_2D mask;
        string defect_report = "";

        bool savemaskDebugData = true;
        
        


        #region parameters
        private void AddPhaseSettings()
        {
            settings.Add(SD.CreateGlobal("k1", 2.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("k2", -1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("k3", -1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m1", 0.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m2", 1.0, "Коэффициент для измерения фазы"));
            settings.Add(SD.CreateGlobal("m3", -1.0, "Коэффициент для измерения фазы"));

        }
        private void RetrievePhaseSettings()
        {
            k1 = settings.Single(x => x.Name == "k1").GetValue<double>();
            k2 = settings.Single(x => x.Name == "k2").GetValue<double>();
            k3 = settings.Single(x => x.Name == "k3").GetValue<double>();
            m1 = settings.Single(x => x.Name == "m1").GetValue<double>();
            m2 = settings.Single(x => x.Name == "m2").GetValue<double>();
            m3 = settings.Single(x => x.Name == "m3").GetValue<double>();
        }
        private void AddAutoSquareSettings()
        {
            settings.Add(SD.CreateLocal("img_step", 5, this, "Шаг по изображениям при расчете амплитуды"));
            settings.Add(SD.CreateLocal("active_area_size", 512, this, "Разрешение получаемых фазовых карт"));
            settings.Add(SD.CreateLocal("profile_contrast", 10.0, this, "Контраст профиля для алгоритма поиска акт.области"));
            settings.Add(SD.CreateLocal("autorange_sa_window", (int)3, this, "Контраст профиля для алгоритма поиска акт.области"));
        }
        private void RetrieveAutoSqareSettings()
        {
            img_step = settings.Single(x => x.Name == "img_step").GetValue<int>();
            active_area_res = settings.Single(x => x.Name == "active_area_size").GetValue<int>();
            profileContrast = settings.Single(x => x.Name == "profile_contrast").GetValue<double>();
            autorange_sa_window = settings.Single(x => x.Name == "autorange_sa_window").GetValue<int>();
        }
        private void AddImageSettings()
        {
            settings.Add(SD.CreateLocal("FolderA", "C:\\Camera\\13", this,
        "Папка с изображениями с sin камеры"));
            settings.Add(SD.CreateLocal("FolderB", "C:\\Camera\\14", this,
    "Папка с изображениями с cos камеры"));
            settings.Add(SD.CreateLocal("FolderC", "C:\\Camera\\15", this,
    "Папка с изображениями с -cos камеры"));

        }
        private void RetrieveImageSettings()
        {
            folder1 = settings.Single(x => x.Name == "FolderA").GetValue<string>();
            folder2 = settings.Single(x => x.Name == "FolderB").GetValue<string>();
            folder3 = settings.Single(x => x.Name == "FolderC").GetValue<string>();
        }
        private void AddStitchSettings()
        {
            settings.Add(SD.CreateLocal("stitch_thr", 70.0, this, "Порог границы фазы для сшивки"));
        }
        private void RetrieveStitchSettings()
        {
            stitch_thr = settings.Single(x => x.Name == "stitch_thr").GetValue<double>();
        }
        private void AddLutSettings()
        {
            settings.Add(SD.CreateLocal("lut_min_code", (int)0, this, "Минимально разрешенный код для LUT"));
            settings.Add(SD.CreateLocal("lut_max_code", (int)4095, this, "Максимально разрешенный код для LUT"));
            //settings.Add(SD.CreateLocal("phase_range_thr", 30.0, this, "Порог фазового диапазона для того чтобы считать пиксель дефектным"));
            settings.Add(SD.CreateLocal("phase_diff_thr", 10.0, this,
                "Порог выброса фазы для того чтобы считать пиксель дефектным"));
            settings.Add(SD.CreateLocal("median_area_r", 10.0, this, "Радиус окна медианного фильтра"));




        }
        private void RetrieveLutSettings()
        {
            lut_min_code = settings.Single(x => x.Name == "lut_min_code").GetValue<int>();
            lut_max_code = settings.Single(x => x.Name == "lut_max_code").GetValue<int>();
           // phase_range_thr = settings.Single(x => x.Name == "phase_range_thr").GetValue<double>();
            phase_diff_thr = settings.Single(x => x.Name == "phase_diff_thr").GetValue<double>();
            median_area_r = settings.Single(x => x.Name == "median_area_r").GetValue<int>();
        }

        private void RequestParams()
        {

            if (calculationMode == PhaseCalculationMode.UseImages)
            {
                AddImageSettings();
                if (useAutoSquare) AddAutoSquareSettings();
            }
            if (calculationMode == PhaseCalculationMode.UseImages) AddPhaseSettings();
            if (stitchMode == StitchMode.Calculate) AddStitchSettings();
            if (generateLUT) AddLutSettings();

            if (settings.Count == 0) return;
            SettingsManager.RequestSettingList(settings,requestParams);

            if (calculationMode == PhaseCalculationMode.UseImages)
            {
                RetrieveImageSettings();
                if (useAutoSquare) RetrieveAutoSqareSettings();
            }
            if (calculationMode == PhaseCalculationMode.UseImages) RetrievePhaseSettings();
            if (stitchMode == StitchMode.Calculate) RetrieveStitchSettings();
            if (generateLUT) RetrieveLutSettings();




        }

        #endregion
        public async Task Run(LogForm form)
        {
            if (form == null) return;
            writeLog = form.AppendLog;
            //TODO: добавить запись в глобальный лог
            writeErrorLog = form.AppendErrorLog;
            replaceLastLine = form.ReplaceLastLine;
            form.stopButtonClick += () => { cts.Cancel(); };
            cts = new CancellationTokenSource();
            cts.Token.ThrowIfCancellationRequested();
            FileStream fs = null;
            StreamWriter wr = null;
            Directory.CreateDirectory("logs");
            try 
            {
                fs = new FileStream($"logs\\auto_phase_log_{DateTime.Now:MM-dd_hh-mm-ss}.txt", FileMode.CreateNew);
                wr = new StreamWriter(fs);
                writeLog += (s) => wr.Write($"[{DateTime.Now:HH:mm:ss}]"+s+ Environment.NewLine);
                writeErrorLog += (s) => wr.Write($"[{DateTime.Now:HH:mm:ss}]: err:" + s + Environment.NewLine);

            }
            catch (Exception ex) 
            {
                writeLog("Не удалось записать лог в файл: " + ex.Message);
            }
            try
            {
                await Task.Run(_run);
            }
            catch (Exception ex) { writeLog(ex.Message); }
            finally 
            {
                wr?.Dispose();
                fs?.Dispose();
            }
        }

        private async Task _run()
        {
            if (requestParams)
            writeLog("Запрос параметров от пользователя...");
            else writeLog("Запрос параметров из БД...");
            RequestParams();

            if (calculationMode == PhaseCalculationMode.UseImages)
            {
                try
                {
                    AddImages();
                }
                catch (Exception ex)
                {
                    writeErrorLog("Не удалось обработать исходные изображения: " + ex.Message);
                    return;
                }

                writeLog("Рачет фазовых профилей...");
                try
                {
                    await CalculatePhaseProfiles();
                }
                catch (Exception ex)
                {
                    writeErrorLog("Не удалось выполнить расчет фазовых профилей");
                    writeErrorLog(ex.Message);
                    return;
                }

            }
            else if (calculationMode == PhaseCalculationMode.UseBatch)
            {
                if (phaseBatch == null)
                {
                    writeLog("Не указан пакет фазовых профилей");
                    return;
                }
            }


            if (stitchMode == StitchMode.Calculate)
            {
                try { await StitchPhaseBatch(); }
                catch (Exception ex)
                {
                    writeErrorLog("Ошибка при сшивке фазы:");
                    writeErrorLog(ex.Message);
                    return;
                }

            }
            else if (stitchMode == StitchMode.UseBatch)
            {
                if (stitchedPhaseBatch == null)
                {
                    writeLog("Не указан пакет фазовых профилей");
                    return;
                }

            }
            else return;

            FindDefectPixels();

            if (generateLUT)
            {
                try { await GeneratuLUT(); }
                catch (Exception ex)
                {
                    writeErrorLog("Ошибка при построении LUT:");
                    writeErrorLog(ex.Message);
                    return;
                }
            }
            else
            {
                writeLog("Обработка завершена");
            }

        }


        private void AddImages()
        {
            int count_a = 0, count_b = 0, count_c = 0;

            cts.Token.ThrowIfCancellationRequested();
            ImageManager.Batch_A().SweepDirectory(folder1, true);
            count_a = ImageManager.Batch_A().Count;
            writeLog($"Проверка папки А, найдено {count_a} файлов");

            cts.Token.ThrowIfCancellationRequested();
            ImageManager.Batch_B().SweepDirectory(folder2, true);
            count_b = ImageManager.Batch_B().Count;
            writeLog($"Проверка папки B, найдено {count_b} файлов");

            cts.Token.ThrowIfCancellationRequested();
            ImageManager.Batch_C().SweepDirectory(folder3, true);
            count_c = ImageManager.Batch_B().Count;
            writeLog($"Проверка папки C, найдено {count_c} файлов");

            if (count_a != count_b || count_a != count_c)
            {
                throw new Exception("Не совпадает количество файлов!");
            }
            if (count_a == 0)
            {
                //defult ct needed
                throw new Exception("Не найдено ни одного файла изображения!");
            }

            if (useAutoSquare)
            {
                try
                {
                    FindActiveAresa();
                }

                catch (Exception ex)
                {
                    writeErrorLog("Не удалось выполнить поиск активных областей");
                    writeErrorLog(ex.Message);
                    throw new Exception("Не удалось выполнить поиск активных областей");
                }
            }
            else
            {
                string text = "";
                writeLog("Задайте активные области для груп изображений: ");
                writeLog("Главная форма->Открыть изображение->Определить активную область");
                writeLog("Выберите углы изображения: Нижний левый > Верхний левый > Верхний правый");
                writeLog("Ожидание ввода пользователя...");

                while (!CheckActiveAreas(out text) )//|| cts.IsCancellationRequested)
                { 
                    replaceLastLine(text);
                    if (cts.IsCancellationRequested) throw new Exception("Операция отменена");
                    Thread.Sleep(500);
                }
                writeLog("Активные области заданы, продолжаем");
            }


        }

        private bool CheckActiveAreas(out string text)
        {
            text = "__";
            bool result = (ImageManager.Batch_A().coordinateTransformation != null);
            result &= (ImageManager.Batch_B().coordinateTransformation != null);
            result &= (ImageManager.Batch_C().coordinateTransformation != null);
            if (!result) { text = "Пока не определены все активные области"; return false; }

            CoordinateTransformation ct_a = ImageManager.Batch_A().coordinateTransformation;
            CoordinateTransformation ct_b = ImageManager.Batch_B().coordinateTransformation;
            CoordinateTransformation ct_c = ImageManager.Batch_C().coordinateTransformation;

            result &= ct_a.frame_width == ct_b.frame_width;
            result &= ct_a.frame_height == ct_b.frame_height;

            result &= ct_a.frame_width == ct_c.frame_width;
            result &= ct_a.frame_height == ct_c.frame_height;

            if (!result) 
                { text = $"Не совпадают размеры! A{ct_a.frame_width}:{ct_a.frame_height} " +
                    $"B{ct_b.frame_width}:{ct_b.frame_height} " +
                    $"C{ct_c.frame_width}:{ct_c.frame_height} " ; return false; }

            return result;

        }

        private void FindActiveAresa()
        {


            CoordinateTransformation ct1 = null, ct2 = null, ct3 = null;
            cts.Token.ThrowIfCancellationRequested();
            writeLog("Расчет амплитуды для группы изображений А...");
            Container_2D_int ampl_a = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_A(), img_step));
            DataManager_2D.containers.Add(ampl_a);
            writeLog("Поиск активной области для группы изображений А");
            var sf1 = new SqareFitter(ampl_a);
            FillFitterField(sf1);
            ct1 = sf1.FindRange();

            cts.Token.ThrowIfCancellationRequested();
            writeLog("Расчет амплитуды для группы изображений B...");
            Container_2D_int ampl_b = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_B(), img_step));
            DataManager_2D.containers.Add(ampl_b);
            writeLog("Поиск активной области для группы изображений B");
            var sf2 = new SqareFitter(ampl_b);
            FillFitterField(sf2);
            ct2 = sf2.FindRange();

            cts.Token.ThrowIfCancellationRequested();
            writeLog("Расчет амплитуды для группы изображений C...");
            Container_2D_int ampl_c = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_C(), img_step));
            DataManager_2D.containers.Add(ampl_b);
            writeLog("Поиск активной области для группы изображений C");
            var sf3 = new SqareFitter(ampl_c);
            FillFitterField(sf3);
            ct3 = sf3.FindRange();

            ImageManager.Batch_A().coordinateTransformation = ct1;
            ImageManager.Batch_B().coordinateTransformation = ct2;
            ImageManager.Batch_C().coordinateTransformation = ct3;


            ampl_a.Name = "A_Amplitude";
            ampl_b.Name = "B_Amplitude";
            ampl_c.Name = "C_Amplitude";
            DataManager_2D.containers.Add(ampl_a);
            DataManager_2D.containers.Add(ampl_b);
            DataManager_2D.containers.Add(ampl_c);


        }

        private async Task CalculatePhaseProfiles()
        {

            PhaseMeasurmentGroup pmg = new PhaseMeasurmentGroup();
            pmg.SingleValueParameters = new double[] { k1, k2, k3, m1, m2, m3 };
            pmg.imageSources = new IImageSource[]
            { ImageManager.Batch_A(),
                ImageManager.Batch_B(),
                ImageManager.Batch_C() };
            pmg.UseTransformation = true;

            writeLog("Расчет фазовых профилей...");
            pmg.containerPorcessed += () =>
            {
                replaceLastLine($"Расчет фазовых профилей...{pmg.processed_containers}/{pmg.total_containers}");
            };

            //  internal call settings
            pmg._cancellationToken = cts.Token;
            pmg.SampleId = sample_id;
            pmg.internal_call = true;
            pmg.AppendUserComment( comment);
            //-----------------------------
            await pmg.Execute();

            phaseBatch = pmg.batch;
        }

        private async Task StitchPhaseBatch()
        {
            var phase0 = Container_2D.ReadFromFile(phaseBatch.Filenames[0]);

            writeLog("Сшивка первого фазового профиля...");
            StitchSpatially3 stitch_calc = new StitchSpatially3();

            stitch_calc.SingleValueParameters = new double[] { stitch_thr };
            stitch_calc.ContainerParameters = new IContainer_2D[] { phase0 };
            if (!stitch_calc.Check())
            {
                throw new Exception("Ошибка при сшивке фазы - ошибка входных данных");
            }
            Container_2D phase_stitched = new Container_2D_double(stitch_calc.MeasureFull());
            writeLog($"Сшивка произведена, {stitch_calc.err_count} битых пикселей");
            writeLog("Сшивка группы фазовых профилей...");

            StitchFramewise sfw = new StitchFramewise();

            sfw.SingleValueParameters = new double[] { stitch_thr };
            sfw.ContainerParameters = new IContainer_2D[] { phase_stitched };
            sfw.imageSources = new IImageSource[] { phaseBatch };
            sfw._cancellationToken = cts.Token;
            sfw.AppendUserComment ( $"Сшивка первого кадра произведена по алгоритму заливки, порог фазы = {stitch_thr}");
            sfw.AppendUserComment( $"При сшивке проигнорировано {stitch_calc.err_count} битых пикселей");

            sfw.containerPorcessed += () =>
            {
                replaceLastLine($"Сшивка группы фазовых профилей...{sfw.processed_containers}/{sfw.total_containers}");
            };

            //  internal call settings
            sfw._cancellationToken = cts.Token;
            sfw.SampleId = sample_id;
            sfw.internal_call = true;
           // sfw.UserComment = comment;
            //-----------------------------


            await sfw.Execute();
            stitchedPhaseBatch = sfw.batch;

        }

        private void FindDefectPixels()
        {

            var phase_start = Container_2D.ReadFromFile(stitchedPhaseBatch.Filenames[0]);
            var phase_end = Container_2D.ReadFromFile(stitchedPhaseBatch.Filenames[stitchedPhaseBatch.Count - 1]);

            double avg_start = ImageProcessor_2D.CalculateAverage(phase_start, 8);
            double avg_end = ImageProcessor_2D.CalculateAverage(phase_end, 8);

            var phase_min = avg_start < avg_end ? phase_start : phase_end;
            var phase_max = avg_start < avg_end ? phase_end : phase_start;

            var pf_calc = new PeakFilter();
            pf_calc.SingleValueParameters = new double[] { median_area_r };

            pf_calc.ContainerParameters = new IContainer_2D[] { phase_min };
            var min_phase_diff = new Container_2D_double(pf_calc.MeasureFull());
            min_phase_diff.Name = "min_phase_diff";

            pf_calc.ContainerParameters = new IContainer_2D[] { phase_max };
            var max_phase_diff = new Container_2D_double(pf_calc.MeasureFull());
            max_phase_diff.Name = "max_phase_diff";

            var thr_calc = new Threshold();
            thr_calc.SingleValueParameters = new double[] { 1, 0, 0, phase_diff_thr, phase_diff_thr };

            thr_calc.ContainerParameters = new IContainer_2D[] { min_phase_diff };
            var min_mask = new Container_2D_double(thr_calc.MeasureFull());

            thr_calc.ContainerParameters = new IContainer_2D[] { max_phase_diff };
            var max_mask = new Container_2D_double(thr_calc.MeasureFull());

            var defect_mask = max_mask & min_mask;
            defect_mask.Name = "defect_mask";



            writeLog("Расчет диапазона...");
            PhaseRange pr_calc = new PhaseRange();
            pr_calc.internal_call = true;
            pr_calc.DisplayMessage = writeLog;
            pr_calc.SingleValueParameters = new double[] { range_percentile };
            pr_calc.ContainerParameters = new IContainer_2D[] { phase_end, phase_start };
            var container_range = new Container_2D_double(pr_calc.MeasureFull());

            mask = container_range & defect_mask;
            mask.Name = "mask";

            phase_lo = pr_calc.bottom;
             phase_hi = pr_calc.top; 


            double defect_count = mask.CountWhere((x) => x == 0);
            double defect_percent = defect_count / (mask.Width * mask.Width) * 100;



            defect_report = "Дефектные пиксели отмечены по алгоритму " +
                $"выброса относительно медианного значения r={median_area_r}, dmax = {phase_diff_thr}\n";
            if (range_percentile < 100) defect_report += $"Дополнительная обработка по персентилю p={range_percentile}\n";
            defect_report += $"Отмечено {defect_count} дефектных пикселей ({defect_percent:f2}%)";

            if (stitchMode == StitchMode.Calculate) SamplesDB.AppendBatchCommentNewLine(stitchedPhaseBatch.id,defect_report);

            writeLog(defect_report);

            if (savemaskDebugData)
            {
                DataManager_2D.containers.Add(min_phase_diff);
                DataManager_2D.containers.Add(max_phase_diff);
                DataManager_2D.containers.Add(defect_mask);
                DataManager_2D.containers.Add(mask);

            }


        }

        private async Task GeneratuLUT()
        {


            FindDefectPixels();

     

            ParameterRequestForm pr_form = new ParameterRequestForm();
            pr_form.AddHeader("Введите параметры для LU таблицы. PHASE_0 - значение фазы," +
                " которе выводится при подаче 0 по HDMI (черный цвет), PHASE_255 - значение фазы," +
                " которе выводится при подаче 255 по HDMI (белый цвет)." +
                $" Работа гарантируется в диапазоне >>>  {phase_lo:f2} - {phase_hi:f2} <<<" +
                ". DAC_0/DAC_STEP начальное значение и шаг по коду ЦАП, которое указывалось" +
                " при настройке калибровки в управляющей программе ПФМС" +
                "");
            pr_form.AddDoubleRequest("PHASE_0");
            pr_form.AddDoubleRequest("PHASE_255");
            pr_form.AddDoubleRequest("DAC_0");
            pr_form.AddDoubleRequest("DAC_STEP");

            pr_form.ShowDialog();

            double ph0 = pr_form.RequestDouble("PHASE_0");
            double ph255 = pr_form.RequestDouble("PHASE_255");
            double dac_0 = pr_form.RequestDouble("DAC_0");
            double dac_step = pr_form.RequestDouble("DAC_STEP");


            LU_Table lut_calc = new LU_Table();

            lut_calc.min_code = lut_min_code;
            lut_calc.max_code = lut_max_code;
            lut_calc.internalCall = true;
            lut_calc.DisplayMessage += writeLog;
            lut_calc.SampleId = sample_id;
            lut_calc._cancellationToken = cts.Token;

            lut_calc.SingleValueParameters = new double[] { ph0, ph255, dac_0, dac_step };
            lut_calc.imageSources = new IImageSource[] { stitchedPhaseBatch };
            lut_calc.useMask = true;
            lut_calc.ContainerParameters = new IContainer_2D[] {mask };


            writeLog("Построение LU таблицы...");
            lut_calc.containerPorcessed += () =>
            {
                replaceLastLine($"Построение LU таблицы...{lut_calc.processed_steps}/{lut_calc.total_steps}");
            };

            try
            {
                await lut_calc.Execute();
            }
            catch (Exception ex)
            {
                writeErrorLog("Ошибка при построении LUT: " + ex.Message);
                return;
            }
            writeLog("Обработка завершениа");




        }

        private void FillFitterField(SqareFitter sf)
        {
            sf.aa_size = active_area_res;
            sf.thrContrast = thrContrast;
            sf.profileContrast = profileContrast;
            sf.widthMismath = widthMismath;
            sf.gridStep = gridStep;
            sf.sa_window_size = autorange_sa_window;

        }


       
        public AutoPhase() 
        {


            
        
        
        }
        



    }
}
