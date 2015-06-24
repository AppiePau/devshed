namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Reading;

    public static class CsvReader
    {
        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        public static TRow[] Read<TRow>(Stream stream, CsvDefinition<TRow> definition) where TRow : new()
        {
            return Read(stream, definition, CsvWriter.DefaultEncoding, true);
        }

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="encoding">The byte encoding.</param>
        /// <returns></returns>
        public static TRow[] Read<TRow>(Stream stream, CsvDefinition<TRow> definition, Encoding encoding) where TRow : new()
        {
            return Read(stream, definition, encoding, false);
        }

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncodig">if set to <c>true</c> detects the byte encodig.</param>
        /// <returns></returns>
        public static TRow[] Read<TRow>(Stream stream, CsvDefinition<TRow> definition, Encoding encoding, bool detectEncodig) where TRow : new()
        {
            var mapper = new CsvStreamMapper<TRow>(definition);
            return mapper.FromStream(stream, encoding, detectEncodig);
        }
    }
}
