namespace Devshed.Csv.Writing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public sealed class CsvStreamWriter
    {
        private readonly Stream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamWriter"/> class.
        /// </summary>
        /// <param name="stream"> The stream to write to. </param>
        /// <param name="encoding"> The encoding to use. Unicode by default. </param>
        public CsvStreamWriter(Stream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Writes the array according to the specified definition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows">The rows.</param>
        /// <param name="definition">The definition.</param>
        public void Write<T>(T[] rows, CsvDefinition<T> definition)
        {
            var writer = new StreamWriter(this.stream, definition.Encoding);


            WriteBitOrderMarker<T>(definition);
            
            if (definition.FirstRowContainsHeaders)
            {
                this.AddHeader<T>(writer, definition, rows);
            }

            foreach (var row in rows)
            {
                this.AddLine<T>(writer, definition, row);
            }

            writer.Flush();

            this.stream.Flush();
        }

        private void AddLine<T>(StreamWriter writer, CsvDefinition<T> definition, T item)
        {  
            var values = definition.Columns.SelectMany(e => e.Render(definition, item, definition.FormattingCulture)).ToArray();
            writer.WriteLine(string.Join(definition.ElementDelimiter, values));
        }

        private void AddHeader<T>(StreamWriter writer, CsvDefinition<T> definition, T[] rows)
        {             
            var headers = definition.Columns.SelectMany(column => GetHeaderNames<T>(definition, column, rows)).ToArray();
            writer.WriteLine(string.Join(definition.ElementDelimiter, headers));
        }

        private static IEnumerable<string> GetHeaderNames<T>(CsvDefinition<T> definition, ICsvColumn<T> column, T[] rows)
        {
            return column.GetWritingHeaderNames(rows).Select(header => GetColumnHeaderWithoutEnters(header));
        }

        private static string GetColumnHeaderWithoutEnters(string header)
        {
            return CsvString.FormatStringCell(header, true);
        }

        private void WriteBitOrderMarker<T>(CsvDefinition<T> definition)
        {
            if (this.stream.Position == 0 && definition.WriteBitOrderMarker)
            {
                var bom = definition.Encoding.GetPreamble();
                this.stream.Write(bom, 0, bom.Length);
            }
        }
    }
}
