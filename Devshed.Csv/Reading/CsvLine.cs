namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    public sealed class CsvLine<TRow>
    {
        private readonly CsvLine line;

        public CsvLine(CsvLine line, TRow row)
        {
            this.line = line;
            this.Row = row;
        }

        public int LineNumber
        {
            get
            {
                return this.line.SourceLine.LineNumber;
            }
        }

        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return this.line.SourceLine.ErrorMessages;
            }
        }

        public TRow Row { get; private set; }
    }
}
