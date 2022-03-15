namespace Devshed.Csv.Reading
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    /// <summary> Reads s stream as CSV content. </summary>
    public sealed class XlsxStreamReader : IStreamReader
    {
        //private readonly WorksheetPart workbook;
        private readonly SpreadsheetDocument document;
        private readonly WorksheetPart worksheetPart;
        private readonly SheetData sheetData;
        private readonly IEnumerator<Row> rows;
      
        /// <summary>
        /// Reads a stream and detects (if specified) the encoding, but falls back to the specified enconding if not detection failed.
        /// </summary>
        /// <param name="stream"> The stream to read. </param>
        public XlsxStreamReader(Stream stream)
        {
            this.document = SpreadsheetDocument.Open(stream, false);
            this.worksheetPart = document.WorkbookPart.WorksheetParts.First();
            this.sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            this.rows = sheetData.Elements<Row>().GetEnumerator();
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
            this.document.Dispose();
        }
      
        /// <summary>
        /// Read a line from the file.
        /// </summary>
        /// <returns></returns>
        public CsvSourceLine ReadLine()
        {
            var elements = this.rows.Current.Elements<Cell>().Select(e => e.InnerText?.ToString() ?? string.Empty).ToArray();
            var rowNumber = this.rows.Current.RowIndex != null ? checked((int)this.rows.Current.RowIndex.Value) : -1;

            this.EndOfStream = !this.rows.MoveNext();

            return new CsvSourceLine(rowNumber, elements);
        }
    }
}