namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    /// <summary>Representa a processed line from a CSV file.</summary>
    /// <typeparam name="TRow">The type of the row.</typeparam>
    public sealed class CsvLine<TRow>
    {
        private readonly CsvLine line;

        /// <summary>Initializes a new instance of the <see cref="CsvLine{TRow}" /> class.</summary>
        /// <param name="line">The line.</param>
        /// <param name="row">The row.</param>
        public CsvLine(CsvLine line, TRow row)
        {
            this.line = line;
            this.Row = row;
        }

        /// <summary>Gets the line number where the line has been read from.</summary>
        /// <value>The line number.</value>
        public int LineNumber
        {
            get
            {
                return this.line.SourceLine.LineNumber;
            }
        }

        /// <summary>Gets the error messages ocurred during reading.</summary>
        /// <value>The error messages.</value>
        public IEnumerable<string> ErrorMessages
        {
            get
            {
                return this.line.SourceLine.ErrorMessages;
            }
        }

        /// <summary>Gets the row as a model that has been generated from the line.</summary>
        /// <value>The row.</value>
        public TRow Row { get; private set; }
    }
}
