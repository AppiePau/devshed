namespace Devshed.Csv.Reading
{
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class CsvStreamLineReaderExtensions
    {
        /// <summary>
        /// Reads the rows from a byte array.
        /// </summary>
        /// <param name="csv">The CSV line reader.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromBytes(this CsvStreamLineReader csv, byte[] bytes, Encoding encoding)
        {
            return FromStream(csv, new MemoryStream(bytes), encoding, false);
        }
        

        /// <summary>
        /// Reads the rows from a byte array.
        /// </summary>
        /// <param name="csv">The CSV line reader.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> the encoding will be detected automaticly.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromBytes(this CsvStreamLineReader csv, byte[] bytes, Encoding encoding, bool detectEncoding)
        {
            return FromStream(csv, new MemoryStream(bytes), encoding, detectEncoding);
        }

        /// <summary>
        /// Reads the rows from a byte array.
        /// </summary>
        /// <param name="csv">The CSV line reader.</param>
        /// <param name="bytes">The bytes.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> the encoding will be detected automaticly.</param>
        /// <param name="elementSeparator">The element separator characters.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromBytes(this CsvStreamLineReader csv, byte[] bytes, Encoding encoding, bool detectEncoding, string elementSeparator)
        {
            return FromStream(csv, new MemoryStream(bytes), encoding, true, elementSeparator);
        }

        /// <summary>
        /// Reads the rows from a stream.
        /// </summary>
        /// <param name="csv">The CSV.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromStream(this CsvStreamLineReader csv, Stream stream, Encoding encoding)
        {
            return FromStream(csv, stream, encoding, false);
        }


        /// <summary>
        /// Reads the rows from a stream.
        /// </summary>
        /// <param name="lineReader">The CSV line reader.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> the encoding will be detected automaticly.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromStream(this CsvStreamLineReader lineReader, Stream stream, Encoding encoding, bool detectEncoding)
        {
            using (var reader = new CsvStreamReader(stream, encoding, detectEncoding))
            {
                return lineReader.GetRows(reader).ToArray();
            }
        }

        /// <summary>
        /// Reads the rows from a stream.
        /// </summary>
        /// <param name="lineReader">The CSV line reader.</param>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="detectEncoding">if set to <c>true</c> the encoding will be detected automaticly.</param>
        /// <param name="elementSeparator">The element separator characters.</param>
        /// <returns>
        /// Returns the rows as an array.
        /// </returns>
        public static CsvLine[] FromStream(this CsvStreamLineReader lineReader, Stream stream, Encoding encoding, bool detectEncoding, string elementSeparator)
        {
            using (var reader = new CsvStreamReader(stream, encoding, detectEncoding))
            {
                reader.ElementDelimiter = elementSeparator;
                return lineReader.GetRows(reader).ToArray();
            }
        }
    }
}
