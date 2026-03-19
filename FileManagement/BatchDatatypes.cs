using System.Collections.Generic;

namespace ImgAnalyzer
{
    public static class BatchDatatypes
    {
        public static readonly List<string> types = new List<string> { "Unknown", "Phase-Wrapped", "Phase-Uwnrapped", "LUT" };
        public static string Unknown { get => types[0]; }
        public static string PhaseWrapped { get => types[1]; }
        public static string PhaseUnwrapped { get => types[2]; }
        public static string LUT { get => types[3]; }
    }
}
