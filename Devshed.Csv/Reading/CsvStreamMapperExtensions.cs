namespace Devshed.Csv.Reading
{
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class CsvStreamMapperExtensions
    {
        /// <summary>
        /// Reads the CSV data from a byte array and maps it to the specified objecttype array. Detects encoding automaticly, but falls back to UTF8.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="bytes">The bytes.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromBytes<TRow>(this CsvStreamMapper<TRow> mapper, byte[] bytes) where TRow : new()
        {
            return FromBytes(mapper, bytes, CsvConfiguration.DefaultEncoding, true);
        }

        /// <summary>
        /// Reads the CSV data from a byte array and maps it to the specified objecttype array.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromBytes<TRow>(this CsvStreamMapper<TRow> mapper, byte[] bytes, Encoding encoding) where TRow : new()
        {
            return FromBytes(mapper, bytes, encoding, false);
        }

        /// <summary>
        /// Reads the CSV data from a byte array and maps it to the specified objecttype array.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> encoding will be automaticly detected.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromBytes<TRow>(this CsvStreamMapper<TRow> mapper, byte[] bytes, Encoding encoding, bool detectEncoding) where TRow : new()
        {
            using (var memory = new MemoryStream(bytes))
            {
                return FromStream(mapper, memory, encoding, detectEncoding);
            }
        }

        /// <summary>
        /// Reads the CSV data from a stream and maps it to the specified objecttype array. Detects encoding automaticly, but falls back to UTF8.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromStream<TRow>(this CsvStreamMapper<TRow> mapper, Stream stream) where TRow : new()
        {
            return FromStream<TRow>(mapper, stream, CsvConfiguration.DefaultEncoding, true);
        }

        /// <summary>
        /// Reads the CSV data from a stream and maps it to the specified objecttype array.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromStream<TRow>(this CsvStreamMapper<TRow> mapper, Stream stream, Encoding encoding) where TRow : new()
        {
            return FromStream<TRow>(mapper, stream, encoding, false);
        }

        /// <summary>
        /// Reads the CSV data from a stream and maps it to the specified objecttype array.
        /// </summary>
        /// <typeparam name="TRow">The type to materialize to.</typeparam>
        /// <param name="mapper">The CSV mapper.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> encoding will be automaticly detected.</param>
        /// <returns>
        /// An array of objects.
        /// </returns>
        public static TRow[] FromStream<TRow>(this CsvStreamMapper<TRow> mapper, Stream stream, Encoding encoding, bool detectEncoding) where TRow : new()
        {
            var reader = new CsvStreamReader(stream, encoding, detectEncoding)
            {
                ElementDelimiter = mapper.Definition.ElementDelimiter,
                HasFieldsEnclosedInQuotes = mapper.Definition.HasFieldsEnclosedInQuotes
            };

            return mapper.GetRows(reader).ToArray();
        }
    }
}
