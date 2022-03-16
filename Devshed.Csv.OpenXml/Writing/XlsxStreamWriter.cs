using Devshed.Csv.Writing;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Devshed.Csv.ClosedXml
{
    /// <summary>
    /// Writes an object model to a XLSX file.
    /// </summary>
    public class XlsxStreamWriter : ICsvStreamWriter
    {
        private readonly string sheetName;

        private readonly IStringFormatter formatter;

        /// <summary>
        /// Inititize the writer.
        /// </summary>
        /// <param name="sheetName"> The sheet name. </param>
        public XlsxStreamWriter(string sheetName = "Document")
        {
            this.sheetName = sheetName;
            this.formatter = new XlsxStringFormatter();
        }

        /// <summary>
        /// Write the object model / collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <param name="rows"></param>
        /// <param name="definition"></param>
        public void Write<T>(Stream stream, T[] rows, CsvDefinition<T> definition)
        {
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            //var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                var rowid = 1;
                if (definition.FirstRowContainsHeaders)
                {
                    this.AddHeader<T>(sheetData, definition, rows, rowid);
                    rowid++;
                }

                foreach (var row in rows)
                {
                    this.AddLine<T>(sheetData, definition, row, rowid);
                    rowid++;
                }

                workbookPart.Workbook.Save();
            }

            stream.Flush();
            stream.Position = 0;
        }

        private void AddLine<T>(SheetData worksheet, CsvDefinition<T> definition, T item, int rowid)
        {
            //var colid = 1;

            Row headerRow = new Row();

            foreach (var column in definition.Columns)
            {
                foreach (var value in column.Render(definition, item, definition.FormattingCulture, formatter))
                {
                    //var cell = worksheet.Row(rowid).Cell(colid);
                    // cell.DataType = GetDataType(column);
                    // cell.Value = value;
                    // cell.Style.Alignment.WrapText = false;

                    Cell cell = new Cell();
                    cell.DataType = GetDataType(column);
                    cell.CellValue = new CellValue(value);
                    
                    headerRow.AppendChild(cell);
                }
            }

            worksheet.AppendChild(headerRow);
        }

        private static CellValues GetDataType<T>(ICsvColumn<T> column)
        {
            switch (column.DataType)
            {
                case ColumnDataType.Number:
                case ColumnDataType.Decimal:
                    return CellValues.Number;

                case ColumnDataType.DateTime:
                    return CellValues.Date;

                case ColumnDataType.Boolean:
                    return CellValues.Boolean;

                case ColumnDataType.Time:
                    //return XLDataType.Text;
                    return CellValues.Date;

                case ColumnDataType.Currency:
                    return CellValues.Number;

                case ColumnDataType.Text:
                case ColumnDataType.Composite:
                case ColumnDataType.StrongTyped:
                case ColumnDataType.Object:
                case ColumnDataType.Dynamic:
                    return CellValues.String;

                default:
                    return CellValues.String;
            }
        }

        private void AddHeader<T>(SheetData worksheet, CsvDefinition<T> definition, T[] rows, int rowid)
        {
            var headers = definition.Columns.SelectMany(column => GetHeaderNames<T>(definition, column, rows)).ToArray();
            Row headerRow = new Row();

            foreach (var header in headers)
            {
                Cell cell = new Cell();
                cell.DataType = CellValues.String;
                cell.CellValue = new CellValue(header.Name);
                headerRow.AppendChild(cell);
            }

            worksheet.AppendChild(headerRow);
        }

        private static HeaderCollection GetHeaderNames<T>(CsvDefinition<T> definition, ICsvColumn<T> column, T[] rows)
        {
            return column.GetWritingHeaderNames(rows);
        }
    }
}
