using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer
{
    public static class StaticFunctions
    {

        public static string GetUniqueName(string baseName, List<string> existingNames)
        {
            if (!existingNames.Contains(baseName))
            {
                return baseName;
            }

            int counter = 0;
            string newName;

            do
            {
                newName = $"{baseName}_{counter}";
                counter++;
            }
            while (existingNames.Contains(newName));

            return newName;
        }





    }
}
