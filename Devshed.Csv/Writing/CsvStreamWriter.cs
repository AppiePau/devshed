namespace Devshed.Csv.Writing
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public sealed class CsvStreamWriter
    {
        private readonly StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamWriter"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public CsvStreamWriter(Stream stream)
            : this(stream, CsvWriter.DefaultEncoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvStreamWriter"/> class.
        /// </summary>
        /// <param name="stream"> The stream to write to. </param>
        /// <param name="encoding"> The encoding to use. Unicode by default. </param>
        public CsvStreamWriter(Stream stream, System.Text.Encoding encoding)
        {
            this.writer = new StreamWriter(stream, encoding);
        }

        /// <summary>
        /// Writes the array according to the specified definition.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rows">The rows.</param>
        /// <param name="definition">The definition.</param>
        public void Write<T>(T[] rows, CsvDefinition<T> definition)
        {
            WriteBom<T>(definition);
            
            if (definition.FirstRowContainsHeaders)
            {
                this.AddHeader<T>(definition);
            }

            foreach (var row in rows)
            {
                this.AddLine<T>(definition, row);
            }

            this.writer.Flush();
        }

        private void AddLine<T>(CsvDefinition<T> definition, T item)
        {  
            var values = definition.Columns.SelectMany(e => e.Render(definition, item));
            this.writer.WriteLine(string.Join(definition.ElementDelimiter, values.ToArray()));
        }

        private void AddHeader<T>(CsvDefinition<T> definition)
        {             
            var headers = definition.Columns.SelectMany(column => GetHeaderNames<T>(definition, column));
            this.writer.WriteLine(string.Join(definition.ElementDelimiter, headers.ToArray()));
        }

        private static IEnumerable<string> GetHeaderNames<T>(CsvDefinition<T> definition, ICsvColumn<T> column)
        {
            return column.GetHeaderNames().Select(header => GetColumnHeaderWithoutEnters(header));
        }

        private static string GetColumnHeaderWithoutEnters(string header)
        {
            return CsvString.FormatStringCell(header, true);
        }

        private void WriteBom<T>(CsvDefinition<T> definition)
        {
            if (this.writer.BaseStream.Position == 0 && definition.WriteBitOrderMarker)
            {
                var bom = this.writer.Encoding.GetPreamble();
                this.writer.BaseStream.Write(bom, 0, bom.Length);
            }
        }
    }
}
