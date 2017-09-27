namespace Devshed.Csv
{
    using Devshed.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class CsvConfiguration
    {
        /// <summary>
        /// Specifies the app domain default encoding for all created definitions. The default is UTF-8.
        /// </summary>
        public static Encoding DefaultEncoding = Encoding.Default;
    }

    /// <summary> Defines the CSV document layout. </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class CsvDefinition<TSource>
    {
        private readonly ICollection<ICsvColumn<TSource>> columns;

        private Encoding encoding;

        public CsvDefinition(Encoding encoding, params ICsvColumn<TSource>[] columns)
        {
            Requires.IsNotNull(columns, "columns");
            Requires.IsNotNull(encoding, "encoding");

            this.columns = columns.ToList();
            this.ElementDelimiter = ";";
            this.RemoveNewLineCharacters = true;
            this.HasFieldsEnclosedInQuotes = true;
            this.WriteBitOrderMarker = true;
            this.Encoding = encoding;
        }

        
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDefinition{TSource}"/> class.
        /// </summary>
        /// <param name="columns">The elements.</param>
        public CsvDefinition(params ICsvColumn<TSource>[] columns)
            : this(CsvConfiguration.DefaultEncoding, columns)
        {
        }

        /// <summary>
        /// Gets or sets the encoding of the file.
        /// </summary>
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                Requires.IsNotNull(value, "encoding");

                this.encoding = value;
            }
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

        /// <summary>
        /// Instructs the materializer to ignore readonly properties without throwing an exception.
        /// </summary>
        public bool IgnoreReadonlyProperties { get;  set; }
    }
}