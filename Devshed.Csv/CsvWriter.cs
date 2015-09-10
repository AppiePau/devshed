namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Writing;
  
    public static class CsvWriter
    {
        /// <summary>
        /// Writes the CSV data using the specified encoding into a new MemoryStream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns> A MemoryStream object containing the rendered CSV data.</returns>
        public static MemoryStream CreateStream<T>(CsvDefinition<T> definition, T[] rows)
        {
            var stream = new MemoryStream();
            CsvWriter.Write(stream, definition, rows);
            return stream;
        }

        /// <summary>
        /// Writes the CSV data into the stream using the specified encoding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        public static void Write<T>(Stream stream, CsvDefinition<T> definition, T[] rows)
        {
            var builder = new CsvStreamWriter(stream);
            builder.Write<T>(rows, definition);
        }
    }
}