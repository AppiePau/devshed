namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents a date type column.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class DateCsvColumn<TSource> : ColumnDefinition<TSource, DateTime?>
    {
        /// <summary>Initializes a new instance of the <see cref="DateCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public DateCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="DateCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public DateCsvColumn(Expression<Func<TSource, DateTime?>> selector)
            : base(selector)
        {
            this.Format = (date, culture) => date != null
                    ? date.Value.ToShortDateString()
                    : string.Empty;
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.DateTime;
            }
        }


        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<DateTime?, CultureInfo, string> Format { get; set; }

        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        protected override string OnRender(ICsvDefinition defintion, DateTime? value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatCell(this.Format(value, culture));
        }
    }

}
