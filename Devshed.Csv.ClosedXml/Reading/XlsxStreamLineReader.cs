namespace Devshed.Csv.Reading
{
    using ClosedXML.Excel;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary> Reads s stream as CSV content. </summary>
    public sealed class XlsxStreamReader : IStreamReader
    {
        private readonly IXLWorkbook workbook;
        private readonly IXLWorksheet sheet;
        private readonly IEnumerator<IXLRow> rows;
      
        /// <summary>
        /// Reads a stream and detects (if specified) the encoding, but falls back to the specified enconding if not detection failed.
        /// </summary>
        /// <param name="stream"> The stream to read. </param>
        public XlsxStreamReader(Stream stream)
        {
            this.workbook = new XLWorkbook(stream);
            this.sheet = workbook.Worksheet(1);
            this.rows = sheet.RowsUsed().GetEnumerator();
            this.EndOfStream = !this.rows.MoveNext();
        }

        /// <summary>
        /// Has the end of the stream  been reached.
        /// </summary>
        public bool EndOfStream
        {
            get; private set;
        }

        /// <summary>
        /// Dispose the reader and close the workbook.
        /// </summary>
        public void Dispose()
        {
            this.workbook.Dispose();
        }
      
        /// <summary>
        /// Read a line from the file.
        /// </summary>
        /// <returns></returns>
        public CsvSourceLine ReadLine()
        {
            var elements = this.rows.Current.Cells().Select(e => e.Value?.ToString() ?? string.Empty).ToArray();
            var rowNumber = this.rows.Current.RowNumber();

            this.EndOfStream = !this.rows.MoveNext();

            return new CsvSourceLine(rowNumber, elements);
        }
    }
}