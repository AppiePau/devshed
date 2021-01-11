namespace Devshed.Csv
{
    using System.Text;

    /// <summary>Default configuration for processing CSV data.</summary>
    public static class CsvConfiguration
    {
        /// <summary>
        /// Specifies the app domain default encoding for all created definitions. The default is UTF-8.
        /// </summary>
        public static Encoding DefaultEncoding = Encoding.Default;
    }
}