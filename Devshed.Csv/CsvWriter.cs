namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Writing;

    /// <summary>Writes CSV files.</summary>
    public static class CsvWriter
    {
        /// <summary>
        /// Writes the CSV data into a new stream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static MemoryStream WriteAsCsv<T>(this CsvDefinition<T> definition, T[] rows)
        {
            var stream = new MemoryStream();
            definition.WriteAsCsv(rows, stream);
            return stream;
        }

        /// <summary>
        /// Writes the CSV data into the stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <param name="stream"></param>
        /// <param name="rows"></param>
        public static void WriteAsCsv<T>(this CsvDefinition<T> definition, T[] rows, Stream stream)
        {
            var builder = new CsvStreamWriter();
            builder.Write<T>(stream, rows, definition);
        }
    }
}