namespace Devshed.Csv
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Writing;
    using System;
    public static class CsvWriter
    {
        private static Encoding defaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Gets or sets the default encoding of the CSV framework. UTF-8 is the default encoding.
        /// </summary>
        public static Encoding DefaultEncoding
        {
            get
            {
                return defaultEncoding;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("defaultEncoding");
                }

                DefaultEncoding = value;
            }
        }

        /// <summary>
        /// Writes the CSV data into a new MemoryStream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        /// <returns>
        /// A MemoryStream object containing the rendered CSV data.
        /// </returns>
        public static MemoryStream CreateStream<T>(CsvDefinition<T> definition, T[] rows)
        {
            return CreateStream(definition, rows, CsvWriter.DefaultEncoding);
        }

        /// <summary>
        /// Writes the CSV data using the specified encoding into a new MemoryStream and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns> A MemoryStream object containing the rendered CSV data.</returns>
        public static MemoryStream CreateStream<T>(CsvDefinition<T> definition, T[] rows, Encoding encoding)
        {
            var stream = new MemoryStream();
            CsvWriter.Write(stream, definition, rows, encoding);
            return stream;
        }

        /// <summary>
        /// Writes the CSV data to the specified stream.
        /// </summary>
        /// <typeparam name="T"> Type of collection to write. </typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        public static void Write<T>(Stream stream, CsvDefinition<T> definition, T[] rows)
        {
            Write(stream, definition, rows, CsvWriter.DefaultEncoding);
        }

        /// <summary>
        /// Writes the CSV data into the stream using the specified encoding.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <param name="definition">The definition.</param>
        /// <param name="rows">The rows.</param>
        public static void Write<T>(Stream stream, CsvDefinition<T> definition, T[] rows, Encoding encoding)
        {
            var builder = new CsvStreamWriter(stream, encoding);
            builder.Write<T>(rows, definition);
        }
    }
}