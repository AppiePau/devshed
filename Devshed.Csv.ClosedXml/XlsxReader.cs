namespace Devshed.Csv
{
    using System.IO;
    using System.Linq;
    using Devshed.Csv.Reading;

    /// <summary>
    /// Reads XLSX files and converts them into models.
    /// </summary>
    public static class XlsxReader
    {
        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <returns></returns>
        public static TRow[] ReadXlsx<TRow>(this CsvDefinition<TRow> definition, Stream stream) where TRow : new()
        {
            var mapper = new TableDataMapper<TRow>(definition);
           
            return mapper.ReadXlsx(stream).ToArray();
        }

        /// <summary>
        /// Read a row from the file.
        /// </summary>
        /// <typeparam name="TRow"> Type of row to map to. </typeparam>
        /// <param name="mapper"> The mapper that provides value to object translation. </param>
        /// <param name="stream"> The stream to the file to read. </param>
        /// <returns></returns>
        public static TRow[] ReadXlsx<TRow>(this TableDataMapper<TRow> mapper, Stream stream) where TRow : new()
        {
            return ReadXlsxVerbose(mapper, stream).Select(e => e.Row).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRow"> Type of row to map to. </typeparam>
        /// <param name="mapper"> The mapper that provides value to object translation. </param>
        /// <param name="stream"> The stream to the file to read. </param>
        /// <returns></returns>
        public static CsvLine<TRow>[] ReadXlsxVerbose<TRow>(this TableDataMapper<TRow> mapper, Stream stream) where TRow : new()
        {
            var reader = new XlsxStreamReader(stream);

            return mapper.GetRows(reader).ToArray();
        }
    }
}
