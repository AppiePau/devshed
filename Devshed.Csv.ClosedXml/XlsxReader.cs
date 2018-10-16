namespace Devshed.Csv
{
    using System.IO;
    using System.Linq;
    using Devshed.Csv.Reading;

    public static class XlsxReader
    {
        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TRow">The type of the row.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncodig">if set to <c>true</c> detects the byte encodig.</param>
        /// <returns></returns>
        public static TRow[] ReadXlsx<TRow>(this CsvDefinition<TRow> definition, Stream stream) where TRow : new()
        {
            var mapper = new TableDataMapper<TRow>(definition);
           
            return mapper.ReadXlsx(stream).ToArray();
        }

        public static TRow[] ReadXlsx<TRow>(this TableDataMapper<TRow> mapper, Stream stream) where TRow : new()
        {
            return ReadXlsxVerbose(mapper, stream).Select(e => e.Row).ToArray();
        }

        public static CsvLine<TRow>[] ReadXlsxVerbose<TRow>(this TableDataMapper<TRow> mapper, Stream stream) where TRow : new()
        {
            var reader = new XlsxStreamReader(stream);

            return mapper.GetRows(reader).ToArray();
        }
    }
}
