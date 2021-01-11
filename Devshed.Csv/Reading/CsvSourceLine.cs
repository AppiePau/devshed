namespace Devshed.Csv.Reading
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>Representa a string base line from a CSV file for processing</summary>
    public sealed class CsvSourceLine
    {
        /// <summary>Initializes a new instance of the <see cref="CsvSourceLine" /> class.</summary>
        /// <param name="lineNumer">The line numer.</param>
        /// <param name="elements">The elements.</param>
        public CsvSourceLine(int lineNumer, string[] elements)
        {
            this.LineNumber = lineNumer;
            this.Elements = elements;

            Trace.WriteLine("Line " + lineNumer + ", IsEmpty: " + this.IsEmpty + ", Elements: " + this.Count);
        }

        /// <summary>Gets the line number.</summary>
        /// <value>The line number.</value>
        public int LineNumber { get; private set; }

        /// <summary>Gets the value elements.</summary>
        /// <value>The elements.</value>
        public string[] Elements { get; private set; }

        /// <summary>Gets the element/column  count.</summary>
        /// <value>The element/column count.</value>
        public int Count { get { return this.Elements.Count(); } }

        /// <summary>Gets the error messages ocurred during reading.</summary>
        /// <value>The error messages.</value>
        public List<string> ErrorMessages { get; } = new List<string>();

        /// <summary>Gets a value indicating whether this line is empty.</summary>
        /// <value>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                return (this.Count == 1 && string.IsNullOrEmpty(this.Elements[0].Trim()))
                || this.Count == 0;
            }
        }
    }
}