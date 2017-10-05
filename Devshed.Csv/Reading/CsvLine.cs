namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    public sealed class CsvLine : Dictionary<string, string>
    {
        public CsvLine(CsvSourceLine line)
        {
            this.SourceLine = line;
        }

        public CsvSourceLine SourceLine { get; set; }
    }
}