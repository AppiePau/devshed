namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Writing;

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

        /// <summary>
        /// Writes the CSV data into a new stream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns> A MemoryStream object containing the rendered CSV data.</returns>
        public static MemoryStream CreateStream<T>(CsvDefinition<T> definition, T[] rows)
        {
            return definition.WriteAsCsv<T>(rows);
        }

        /// <summary>
        /// Writes the CSV data into the stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        public static void Write<T>(Stream stream, CsvDefinition<T> definition, T[] rows)
        {
            var builder = new CsvStreamWriter();
            builder.Write<T>(stream, rows, definition);
        }
    }
}