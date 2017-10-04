namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public sealed class CsvSourceLine
    {
        public CsvSourceLine(int lineNumer, string[] elements)
        {
            this.LineNumber = lineNumer;
            this.Elements = elements;

            Trace.WriteLine("Line " + lineNumer + ", IsEmpty: " + this.IsEmpty + ", Elements: " + this.Count);
        }

        public int LineNumber { get; private set; }

        public string[] Elements { get; private set; }

        public int Count { get { return this.Elements.Count(); } }

        public List<string> ErrorMessages { get; } = new List<string>();

        public bool IsEmpty
        {
            get
            {
                return (this.Count == 1 && string.IsNullOrEmpty(this.Elements[0].Trim()))
                || this.Count == 0;
            }
        }
    }
}