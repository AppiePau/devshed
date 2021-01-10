namespace Devshed.Csv.Writing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Writes the CSV data through a stream.
    /// </summary>
    public sealed class CsvStreamWriter : ICsvStreamWriter
    {
        private IStringFormatter formatter;

        public CsvStreamWriter()
        {
            this.formatter = new CsvStringFormatter();
        }

        /// <summary>
        /// Writes the array according to the specified definition.
        /// </summary>
        /// <typeparam name="T"> The model type to write. </typeparam>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="definition">The definition.</param>
        public void Write<T>(Stream stream, T[] rows, CsvDefinition<T> definition)
        {
            var writer = new StreamWriter(stream, definition.Encoding);


            WriteBitOrderMarker<T>(stream, definition);

            if (definition.FirstRowContainsHeaders)
            {
                this.AddHeader<T>(writer, definition, rows);
            }

            foreach (var row in rows)
            {
                this.AddLine<T>(writer, definition, row);
            }

            writer.Flush();

            stream.Flush();
        }

        private void AddLine<T>(StreamWriter writer, CsvDefinition<T> definition, T item)
        {
            var values = definition.Columns.SelectMany(e => e.Render(definition, item, definition.FormattingCulture, formatter)).ToArray();
            writer.WriteLine(string.Join(definition.ElementDelimiter, values));
        }

        private void AddHeader<T>(StreamWriter writer, CsvDefinition<T> definition, T[] rows)
        {
            var headers = definition.Columns.SelectMany(column => GetHeaderNames<T>(definition, column, rows)).ToArray();
            writer.WriteLine(string.Join(definition.ElementDelimiter, headers));
        }

        private IEnumerable<string> GetHeaderNames<T>(CsvDefinition<T> definition, IColumDefinition<T> column, T[] rows)
        {
            return column.GetWritingHeaderNames(rows).Select(header => GetColumnHeaderWithoutEnters(header));
        }

        private string GetColumnHeaderWithoutEnters(Header header)
        {
            return formatter.FormatStringCell(header.Name, true);
        }

        private void WriteBitOrderMarker<T>(Stream stream, CsvDefinition<T> definition)
        {
            if (stream.Position == 0 && definition.WriteBitOrderMarker)
            {
                var bom = definition.Encoding.GetPreamble();
                stream.Write(bom, 0, bom.Length);
            }
        }
    }
}
