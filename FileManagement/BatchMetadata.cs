using NetTopologySuite.Index.Strtree;
using ScottPlot.PlotStyles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ImgAnalyzer
{
    public class BatchMetadata
    {
        public const string SampleInfoSeparator = "[Sample info]";

        public string batch_type;
        public string comment;
        public int sample_id;

        public void SaveToFile(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine($"type={batch_type}");
                writer.WriteLine($"sampleId={sample_id}");
                writer.WriteLine($"sampleName={SamplesDB.GetSampleName(sample_id)}");
                writer.Write(comment);
            }
        }

        public static BatchMetadata LoadFromFile(string filename)
        {
            BatchMetadata metadata = new BatchMetadata();
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("sampleId="))
                    {
                        string numberPart = line.Substring(9); // Получаем всё после "Id="

                        // Если число может содержать только цифры до первого нецифрового символа
                        string number = new string(numberPart.TakeWhile(char.IsDigit).ToArray());

                        if (!string.IsNullOrEmpty(number))
                        {
                            metadata.sample_id = int.Parse(number);

                        }
                    }
                    else if (line.StartsWith("type="))
                    {
                        metadata.batch_type = line.Substring(5); // Получаем всё после "Id="
                    }
                    else if (line.StartsWith("#")) { }
                    else metadata.comment += line + "\n";
                }
            }
            return metadata;
        }




    }
}
