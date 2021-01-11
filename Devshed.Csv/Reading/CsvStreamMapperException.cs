using System;
using System.Runtime.Serialization;

namespace Devshed.Csv.Reading
{
    /// <summary>An exception that ocurres when a mapping converions fails.</summary>
    [Serializable]
    public sealed class CsvStreamMapperException : Exception
    {
        /// <summary>Gets the line on which the error has ocurred.</summary>
        /// <value>The line.</value>
        public CsvLine Line
        {
            get;
            private set;
        }

        /// <summary>Initializes a new instance of the <see cref="CsvStreamMapperException" /> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="line">The line.</param>
        public CsvStreamMapperException(string message, CsvLine line) : base(message)
        {
            this.Line = line;
        }

        /// <summary>Initializes a new instance of the <see cref="CsvStreamMapperException" /> class.</summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="line">The line.</param>
        public CsvStreamMapperException(string message, Exception innerException, CsvLine line) : base(message, innerException)
        {
            this.Line = line;
        }
    }
}