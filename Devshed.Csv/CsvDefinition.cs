namespace Devshed.Csv
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary> Defines the CSV document layout. </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class CsvDefinition<TSource>
    {
        private readonly ICollection<ICsvColumn<TSource>> columns;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDefinition{TSource}"/> class.
        /// </summary>
        /// <param name="columns">The elements.</param>
        public CsvDefinition(params ICsvColumn<TSource>[] columns)
        {
            this.columns = columns.ToList();
            this.ElementDelimiter = ";";
            this.RemoveNewLineCharacters = true;
            this.HasFieldsEnclosedInQuotes = true;
            this.WriteBitOrderMarker = false;
        }

        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <value>
        /// The elements.
        /// </value>
        public ICollection<ICsvColumn<TSource>> Columns
        {
            get { return this.columns; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [first row contains headers].
        /// </summary>
        /// <value>
        /// <c>true</c> if [first row contains headers]; otherwise, <c>false</c>.
        /// </value>
        public bool FirstRowContainsHeaders { get; set; }

        /// <summary> Indicates if new line characters should be removed when writing an text the element. </summary>
        public bool RemoveNewLineCharacters { get; set; }

        /// <summary>
        /// Gets or sets the element separator between CSV elements. By default a semicolon ';' is used.
        /// </summary>
        /// <value>
        /// The element separator.
        /// </value>
        public string ElementDelimiter { get; set; }

        /// <summary>
        /// Wether the fields are enclosed in quotes.
        /// </summary>
        public bool HasFieldsEnclosedInQuotes { get; set; }
        
        /// <summary>
        /// Writes a the Byte Order Marker specifying the encoding.
        /// </summary>
        public bool WriteBitOrderMarker { get; set; }
        
        /// <summary>
        /// Writes a the Byte Order Marker specifying the encoding.
        /// </summary>
        public ElementProcessing ElementProcessing { get; set; }
    }
}