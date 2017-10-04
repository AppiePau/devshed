namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    public sealed class CsvLine : Dictionary<string, string>
    {
        public CsvLine(CsvSourceLine line)
        {
            this.Line = line;
        }

        public CsvSourceLine Line { get; set; }
    }
}