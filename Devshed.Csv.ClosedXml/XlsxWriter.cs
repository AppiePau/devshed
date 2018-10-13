namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.ClosedXml;
    using Devshed.Csv.Writing;

    public static class XlsxWriter
    {
        /// <summary>
        /// Writes the CSV data into a new stream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public static MemoryStream WriteAsXlsx<T>(this CsvDefinition<T> definition, T[] rows, string name = "Document")
        {
            var stream = new MemoryStream();
            definition.WriteAsXlsx(rows, stream, name);
            return stream;
        }

        /// <summary>
        /// Writes the CSV data into the stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <param name="stream"></param>
        /// <param name="rows"></param>
        public static void WriteAsXlsx<T>(this CsvDefinition<T> definition, T[] rows, Stream stream, string name = "Document")
        {
            var builder = new XlsxStreamWriter(name);
            builder.Write<T>(stream, rows, definition);
        }
    }
}