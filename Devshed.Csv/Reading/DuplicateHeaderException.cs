namespace Devshed.Csv.Reading
{
    using System;

    /// <summary>An exception to indicate a duplicate header name has been found.</summary>
    public sealed class DuplicateHeaderException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="DuplicateHeaderException" /> class.</summary>
        /// <param name="message">The message.</param>
        public DuplicateHeaderException(string message)
              : base(message)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DuplicateHeaderException" /> class.</summary>
        /// <param name="header">The header.</param>
        /// <param name="lineNumber">The line number.</param>
        public DuplicateHeaderException(string header, int lineNumber)
            : base($"A duplicate header name was found '{header}' on line {lineNumber}.")
        {
            this.LineNumber = lineNumber;
            this.Header = header;
        }

        /// <summary>Gets the header.</summary>
        /// <value>The header.</value>
        public string Header { get; private set; }

        /// <summary>Gets the line number.</summary>
        /// <value>The line number.</value>
        public int LineNumber { get; private set; }
    }
}
