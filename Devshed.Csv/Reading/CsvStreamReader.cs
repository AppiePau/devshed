namespace Devshed.Csv.Reading
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualBasic.FileIO;

    /// <summary> Reads s stream as CSV content. </summary>
    public sealed class CsvStreamReader : IDisposable
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

        public string ElementDelimiter { get; set; }
        public bool HasFieldsEnclosedInQuotes { get; set; }

        public bool EndOfStream
        {
            get
            {
                return this.parser.EndOfData;
            }
        }

        public void Dispose()
        {
            this.parser.Dispose();
        }


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