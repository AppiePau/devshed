namespace Devshed.Csv
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary> Defines the CSV document layout. </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public class TableDefinition<TSource>
    {
        private readonly ICollection<ICsvColumn<TSource>> columns;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDefinition{TSource}"/> class.
        /// </summary>
        /// <param name="columns">The elements.</param>
        public TableDefinition(params ICsvColumn<TSource>[] columns)
        {
            this.columns = columns;
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



        /// <summary>
        /// Writes a the Byte Order Marker specifying the encoding.
        /// </summary>
        public ElementProcessing ElementProcessing { get; set; }

        /// <summary>
        /// Instructs the materializer to ignore readonly properties without throwing an exception.
        /// </summary>
        public bool IgnoreReadonlyProperties { get; set; }

        public bool ThrowExceptionOnError { get; set; } = true;

        public CultureInfo FormattingCulture { get; set; } = CultureInfo.CurrentCulture;
    }
}