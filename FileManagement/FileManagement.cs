using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public static class FileManagement
    {
        
        public static string CreateUniqueFolder(string baseFolderName)
        {
            // Получаем директорию исполняемого файла
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формируем начальный путь
            string folderPath = Path.Combine(baseDirectory, baseFolderName);
            string newFolderPath = folderPath;

            // Если папка уже существует, добавляем суффиксы
            int counter = 1;
            while (Directory.Exists(newFolderPath))
            {
                newFolderPath = $"{folderPath}_{counter}";
                counter++;

                // Защита от бесконечного цикла
                if (counter > 100000)
                    throw new InvalidOperationException("Слишком много попыток создания папки");
            }

            // Создаем папку
            Directory.CreateDirectory(newFolderPath);
            return newFolderPath;
        }





    }
}
