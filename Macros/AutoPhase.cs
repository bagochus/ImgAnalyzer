using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using SD = ImgAnalyzer.SettingDefinition;
using System.Drawing;
using ImgAnalyzer._2D;

namespace ImgAnalyzer.Macros
{
    public class AutoPhase
    {
        private string folder1, folder2, folder3;
        private static AutoPhase instance = null;
        private Action<string> writeLog = null;
        private AutoPhaseForm form;
        


        private int img_step = 1;

        private int active_area_res = 512;
        private double thrContrast = 0.5;
        private double profileContrast = 45;
        private double widthMismath = 0.02;
        private int gridStep = 50;


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

            writeLog("Расчет амплитуды для группы изображений А...");
            Container_2D_int ampl_a = new Container_2D_int(ImageProcessor_2D.Amplitude(ImageManager.Batch_A(), img_step));
            writeLog("Поиск активной области для группы изображений А");
            var sr = new SqareFitter(ampl_a);
            sr.








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
