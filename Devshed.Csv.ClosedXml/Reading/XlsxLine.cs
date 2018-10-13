namespace Devshed.Csv.ClosedXml.Reading
{
    using ClosedXML.Excel;
    using System.Collections.Generic;

    public sealed class XlsxLine<TRow>
    {
        private readonly IXLRow line;

        public XlsxLine(IXLRow line, TRow row)
        {
            this.line = line;
            this.Row = row;
        }

        public int LineNumber
        {
            get
            {
                return this.line.RowNumber();
            }
        }

        public HashSet<string> ErrorMessages { get; private set; } = new HashSet<string>();

        public TRow Row { get; private set; }
    }
}
