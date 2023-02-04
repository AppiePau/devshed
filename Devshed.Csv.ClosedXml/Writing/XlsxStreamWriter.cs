using ClosedXML.Excel;
using Devshed.Csv.Writing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Devshed.Csv.ClosedXml
{
    /// <summary>
    /// Writes an object model to a XLSX file.
    /// </summary>
    public class XlsxStreamWriter //: ICsvStreamWriter
    {
        private readonly IXLWorksheet worksheet;

        private readonly IStringFormatter formatter;

        /// <summary>
        /// Inititize the writer.
        /// </summary>
        /// <param name="sheet"> The sheet. </param>
        /// <param name="options"> Saving options. </param>
        public XlsxStreamWriter(IXLWorksheet sheet, SaveOptions options = null)
        {
            this.worksheet = sheet;
            this.formatter = new XlsxStringFormatter();
        }

        /// <summary>
        /// Write the object model / collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows"></param>
        /// <param name="definition"></param>
        public void Write<T>(T[] rows, CsvDefinition<T> definition)
        {
            var rowid = 1;
            if (definition.FirstRowContainsHeaders)
            {
                this.AddHeader<T>(worksheet, definition, rows, rowid);
                rowid++;
            }

            foreach (var row in rows)
            {
                this.AddLine<T>(worksheet, definition, row, rowid);
                rowid++;
            }
        }

        private void AddLine<T>(IXLWorksheet worksheet, CsvDefinition<T> definition, T item, int rowid)
        {
            var colid = 1;
            foreach (var column in definition.Columns)
            {
                foreach (var value in column.Render(definition, item, definition.FormattingCulture, formatter))
                {
                    var valueType = value?.GetType() ?? typeof(string);
                    var cell = worksheet.Row(rowid).Cell(colid);
                    
                    cell.DataType = GetDataType(column, valueType);
                    cell.Value = value;
                    cell.Style.Alignment.WrapText = false;
                    
                    SetNumberValueFormatting(cell, valueType);

                    colid++;
                }
            }
        }

        private static void SetNumberValueFormatting(IXLCell cell, Type valueType)
        {
            if (valueType == typeof(decimal)
               || valueType == typeof(double)
               || valueType == typeof(float))
            {
                cell.Style.NumberFormat.Format = "#,##0.0";
            }
            else if (valueType == typeof(short)
               || valueType == typeof(int)
               || valueType == typeof(long))
            {
                cell.Style.NumberFormat.Format = "#,##0";
            }

            //// Default is no formatting
        }

        private static XLDataType GetDataType<T>(ICsvColumn<T> column, Type valueType)
        {
            switch (column.DataType)
            {
                case ColumnDataType.Number:
                case ColumnDataType.Decimal:
                    return XLDataType.Number;

                case ColumnDataType.DateTime:
                    return XLDataType.DateTime;

                case ColumnDataType.Boolean:
                    return XLDataType.Boolean;

                case ColumnDataType.Time:
                    //return XLDataType.Text;
                    return XLDataType.TimeSpan;

                case ColumnDataType.Currency:
                    return XLDataType.Number;

                case ColumnDataType.Composite:
                    if (valueType == typeof(decimal)
                        || valueType == typeof(double)
                        || valueType == typeof(float)
                        || valueType == typeof(short)
                        || valueType == typeof(int)
                        || valueType == typeof(long))

                    {
                        return XLDataType.Number;
                    }
                    break;
                case ColumnDataType.Text:
                case ColumnDataType.StrongTyped:
                case ColumnDataType.Object:
                case ColumnDataType.Dynamic:
                    return XLDataType.Text;

                default:
                    return XLDataType.Text;
            }

            return XLDataType.Text;
        }

        private void AddHeader<T>(IXLWorksheet worksheet, CsvDefinition<T> definition, T[] rows, int rowid)
        {
            var headers = definition.Columns.SelectMany(column => GetHeaderNames<T>(definition, column, rows)).ToArray();
            var row = worksheet.Row(rowid);
            var colid = 1;

            foreach (var header in headers)
            {
                row.Cell(colid).Value = header;
                colid++;
            }
        }

        private static HeaderCollection GetHeaderNames<T>(CsvDefinition<T> definition, ICsvColumn<T> column, T[] rows)
        {
            return column.GetWritingHeaderNames(rows);
        }
    }
}
