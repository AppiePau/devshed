namespace Devshed.Csv.Reading
{
    using System;
    using System.IO;
    using System.Text;

    public sealed class CsvTextReader : IDisposable
    {
        private readonly static char[] NewLineCharacters = { '\r', '\n' };

        private readonly string newLineString;

        private readonly StreamReader reader;

        public CsvTextReader(Stream stream)
        {
            this.reader = new StreamReader(stream);
            this.newLineString = new String(NewLineCharacters);
        }

        public bool EndOfStream
        {
            get
            {
                return this.reader.EndOfStream;
            }
        }

        public Stream BaseStream
        {
            get
            {
                return this.reader.BaseStream;
            }
        }

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
