namespace Devshed.Csv.ClosedXml.Reading
{
    using System.Collections.Generic;
    using DocumentFormat.OpenXml.Spreadsheet;

    /// <summary>
    /// Represent a line in the Excel file.
    /// </summary>
    /// <typeparam name="TRow"></typeparam>
    public sealed class XlsxLine<TRow>
    {
        private readonly Row line;

        /// <summary>
        /// Initialize a line.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="row"></param>
        public XlsxLine(Row line, TRow row)
        {
            this.line = line;
            this.Row = row;
        }

        /// <summary>
        /// The line number in the file.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return checked((int)this.line.RowIndex.Value);
            }
        }

        /// <summary>
        /// Collection of error messages that have occurred.
        /// </summary>
        public HashSet<string> ErrorMessages { get; private set; } = new HashSet<string>();

        /// <summary>
        /// The type of row bein mapped.
        /// </summary>
        public TRow Row { get; private set; }
    }
}
