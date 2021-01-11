namespace Devshed.Csv.Reading
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>Reads and converts text to CSV</summary>
    public sealed class CsvTextReader : IDisposable
    {
        private readonly static char[] NewLineCharacters = { '\r', '\n' };

        private readonly string newLineString;

        private readonly StreamReader reader;

        /// <summary>Initializes a new instance of the <see cref="CsvTextReader" /> class.</summary>
        /// <param name="stream">The stream.</param>
        public CsvTextReader(Stream stream)
        {
            this.reader = new StreamReader(stream);
            this.newLineString = new String(NewLineCharacters);
        }

        /// <summary>Gets a value indicating whether the end of the stream has been reached.</summary>
        /// <value>
        ///   <c>true</c> if [end of stream]; otherwise, <c>false</c>.</value>
        public bool EndOfStream
        {
            get
            {
                return this.reader.EndOfStream;
            }
        }

        /// <summary>Gets the base stream being read from.</summary>
        /// <value>The base stream.</value>
        public Stream BaseStream
        {
            get
            {
                return this.reader.BaseStream;
            }
        }

        /// <summary>Reads the line of text to conver to.</summary>
        /// <returns>The line of the current position.<br /></returns>
        public string ReadLine()
        {
            var buffer = new StringBuilder();
            var inString = false;

            while (!this.reader.EndOfStream && !EndsWithEnter(buffer, inString))
            {
                var character = this.reader.Read();

                if (character == '"' && this.reader.Peek() == '"')
                {
                    //// Skip first quote
                    this.reader.Read();
                    buffer.Append((char)character);
                }
                else if (character == '"')
                {
                    inString = !inString;
                }
                else
                {
                    buffer.Append((char)character);
                }
            }

            return buffer.ToString().TrimEnd(NewLineCharacters);
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        public void Dispose()
        {
            this.reader.Dispose();
        }

        private bool EndsWithEnter(StringBuilder buffer, bool inString)
        {
            return buffer.ToString().EndsWith(this.newLineString) && !inString;
        }
    }
}
