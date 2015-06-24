namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    public sealed class CsvLine : Dictionary<string, string>
    {
        public CsvLine(int line)
        {
            this.LineNumber = line;
        }

        public int LineNumber { get; set; }
    }
}