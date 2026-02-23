using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImgAnalyzer
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]




        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string folderPath = Path.Combine(AppContext.BaseDirectory, "transformation");
            Directory.CreateDirectory(folderPath); // Создаст папку, только если её нет
            string folderPath2 = Path.Combine(AppContext.BaseDirectory, "containers");
            Directory.CreateDirectory(folderPath2); // Создаст папку, только если её нет
            //Directory.CreateDirectory(Path.GetDirectoryName(SettingsDB.databaseFile));


            DB_Manager.InitializeDatabase();
            SettingsManager.Initalize();
            SamplesDB.InitializeDatabase();

            Application.Run(new MainForm());
        }
    }
}
