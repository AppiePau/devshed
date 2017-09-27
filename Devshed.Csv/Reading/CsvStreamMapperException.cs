using System;
using System.Runtime.Serialization;

namespace Devshed.Csv.Reading
{
    [Serializable]
    public sealed class CsvStreamMapperException : Exception
    {
        public CsvLine Line
        {
            get;
            private set;
        }

        public CsvStreamMapperException(string message, CsvLine line) : base(message)
        {
            this.Line = line;
        }

        public CsvStreamMapperException(string message, Exception innerException, CsvLine line) : base(message, innerException)
        {
            this.Line = line;
        }
    }
}