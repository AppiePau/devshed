namespace Devshed.Csv
{
    using Devshed.Shared;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary> Defines the CSV document layout. </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class CsvDefinition<TSource> : TableDefinition<TSource>, ICsvDefinition
    {
        private readonly ICollection<IColumDefinition<TSource>> columns;

        private Encoding encoding;

        /// <summary>Initializes a new instance of the <see cref="CsvDefinition{TSource}" /> class.</summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="columns">The columns.</param>
        public CsvDefinition(Encoding encoding, params IColumDefinition<TSource>[] columns) : base(columns)
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
        public CsvDefinition(params IColumDefinition<TSource>[] columns)
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
    }
}