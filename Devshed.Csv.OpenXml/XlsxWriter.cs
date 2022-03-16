namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.ClosedXml;
    using Devshed.Csv.Writing;

    /// <summary>
    /// 
    /// </summary>
    public static class XlsxWriter
    {
        /// <summary>
        /// Writes the CSV data into a new stream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"> The definition to write. </param>
        /// <param name="rows"> The rows to write. </param>
        /// <param name="name"> Name of the document, 'Document' by default. </param>
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
        /// <param name="name"> Name of the document, 'Document' by default. </param>
        public static void WriteAsXlsx<T>(this CsvDefinition<T> definition, T[] rows, Stream stream, string name = "Document")
        {
            var builder = new XlsxStreamWriter(name);
            builder.Write<T>(stream, rows, definition);
        }
    }
}