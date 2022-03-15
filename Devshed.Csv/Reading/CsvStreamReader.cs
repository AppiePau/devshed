namespace Devshed.Csv.Reading
{
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.IO;
    using System.Text;

    /// <summary> Reads s stream as CSV content. </summary>
    public sealed class CsvStreamReader : IStreamReader, IDisposable
    {
        private readonly TextFieldParser parser;

        private int lineCount;

        /// <summary>
        /// Reads a stream in the specified enconding.
        /// </summary>
        /// <param name="stream"> The stream to read. </param>
        /// <param name="encoding"> The fallback encoding. </param>
        public CsvStreamReader(Stream stream, Encoding encoding)
            : this(stream, encoding, false)
        {
        }

        /// <summary>
        /// Reads a stream and detects (if specified) the encoding, but falls back to the specified enconding if not detection failed.
        /// </summary>
        /// <param name="stream"> The stream to read. </param>
        /// <param name="encoding"> The fallback encoding. </param>
        /// <param name="detectEncoding"> Wether to detect the encoding. </param>
        public CsvStreamReader(Stream stream, Encoding encoding, bool detectEncoding)
        {
            this.parser = new TextFieldParser(stream, encoding, detectEncoding);
            parser.TextFieldType = FieldType.Delimited;

            this.ElementDelimiter = ";";
            this.HasFieldsEnclosedInQuotes = true;
        }

        /// <summary>Gets or sets the element delimiter.</summary>
        /// <value>The element delimiter.</value>
        public string ElementDelimiter { get; set; }
        /// <summary>Gets or sets a value indicating whether this instance has fields enclosed in quotes.</summary>
        /// <value>
        ///   <c>true</c> if this instance has fields enclosed in quotes; otherwise, <c>false</c>.</value>
        public bool HasFieldsEnclosedInQuotes { get; set; }

        /// <summary>Gets a value indicating whether the stream has ended.</summary>
        /// <value>
        ///   <c>true</c> if [end of stream]; otherwise, <c>false</c>.</value>
        public bool EndOfStream
        {
            get
            {
                return this.parser.EndOfData;
            }
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        public void Dispose()
        {
            this.parser.Close();
        }


        /// <summary>Reads the CSV line.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public CsvSourceLine ReadLine()
        {
            this.parser.SetDelimiters(this.ElementDelimiter);
            this.parser.HasFieldsEnclosedInQuotes = this.HasFieldsEnclosedInQuotes;

            var elements = this.parser.ReadFields();

            this.lineCount += 1;
            return new CsvSourceLine(this.lineCount, elements);
        }
    }
}