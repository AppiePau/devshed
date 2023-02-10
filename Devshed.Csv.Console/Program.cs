
namespace Devshed.Csv.Console
{
    using System.Data;
    using Devshed.Csv;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Newtonsoft.Json;

    // using Devshed.Csv.OpenXml;

    public class RowModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; internal set; }
        public decimal Money { get; internal set; }
        public int Count { get; internal set; }
        public bool Bool { get; internal set; }
    }

    public static class Program
    {
        public static void Main()
        {
            var rows = new RowModel[]{
                new RowModel { Id = 1, Name = "Test", Date = new DateTime(2020,1,1), Money = 123.44M, Count = 543, Bool = true},
                new RowModel { Id = 1, Name = "Test", Date = new DateTime(2020,1,1), Money = 123.44M, Count = 543, Bool = false}
            };

            var definition = new CsvDefinition<RowModel>(
                new NumberCsvColumn<RowModel>(e => e.Id),
                new TextCsvColumn<RowModel>(e => e.Name),
                new DateCsvColumn<RowModel>(e => e.Date),
                new CurrencyCsvColumn<RowModel>(e => e.Money),
                new DecimalCsvColumn<RowModel>(e => e.Money),
                new NumberCsvColumn<RowModel>(e => e.Count),
                new BooleanCsvColumn<RowModel>(e => e.Bool))
            {
                FirstRowContainsHeaders = true
            };

            using (var file = new FileStream("Test.xlsx", FileMode.OpenOrCreate))
            {
                definition.WriteAsXlsx(rows, file);
            }

            using (var file = new FileStream("Test2.xlsx", FileMode.OpenOrCreate))
            {
                WriteXlsxStream(rows, file);
            }
        }


        public static void WriteXlsxStream<T>(IEnumerable<T> data, Stream stream)
        {
            DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(data), (typeof(DataTable)));

            //using (SpreadsheetDocument document = SpreadsheetDocument.Create("TestNewData.xlsx", SpreadsheetDocumentType.Workbook))
            //var stream = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };

                sheets.Append(sheet);

                Row headerRow = new Row();

                List<String> columns = new List<string>();
                foreach (System.Data.DataColumn column in table.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in table.Rows)
                {
                    Row newRow = new Row();
                    foreach (String col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString());
                        newRow.AppendChild(cell);
                    }

                    sheetData.AppendChild(newRow);
                }

                workbookPart.Workbook.Save();
            }

            //stream.Flush();
            // stream.Position = 0;
            // return stream;
        }
    }
}