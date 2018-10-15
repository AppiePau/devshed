namespace Devshed.Csv.Reading
{
    using System;

    public sealed class DuplicateHeaderException : Exception
    {
        public DuplicateHeaderException(string message)
              : base(message)
        {
        }

        public DuplicateHeaderException(string header, int lineNumber)
            : base($"A duplicate header name was found '{header}' on line {lineNumber}.")
        {
            this.LineNumber = lineNumber;
            this.Header = header;
        }

        public string Header { get; private set; }

        public int LineNumber { get; private set; }
    }
}
