namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;

    /// <summary>Representa a line from a CSV file for processing</summary>
    public sealed class CsvLine : Dictionary<Header, string>
    {
        /// <summary>Initializes a new instance of the <see cref="CsvLine" /> class.</summary>
        /// <param name="line">The line.</param>
        public CsvLine(CsvSourceLine line)
        {
            this.SourceLine = line;
        }

        /// <summary>Gets or sets the source line.</summary>
        /// <value>The source line.</value>
        public CsvSourceLine SourceLine { get; set; }
    }
}