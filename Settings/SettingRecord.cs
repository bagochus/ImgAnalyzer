using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgAnalyzer.Settings
{
    public class SettingRecord
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public string Value { get; set; }
        public string Owner { get; set; }
        public string Datatype {  get; set; }
        public string Comment { get; set; }

        public DateTime modified { get; set; }


    }
}
