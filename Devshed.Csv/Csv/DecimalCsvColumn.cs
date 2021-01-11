namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    /// <summary>
    /// Represents a decimal type column.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class DecimalCsvColumn<TSource> : ColumnDefinition<TSource, decimal?>
    {
        /// <summary>Initializes a new instance of the <see cref="DecimalCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public DecimalCsvColumn(string propertyName)
            : base(propertyName)
        {
        }


        /// <summary>Initializes a new instance of the <see cref="DecimalCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public DecimalCsvColumn(Expression<Func<TSource, decimal?>> selector)
            : base(selector)
        {
            this.Format = (number, formatter) => number != null
                ? number.Value.ToString(formatter)
                : string.Empty;
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Decimal;
            }
        }


        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<decimal?, CultureInfo, string> Format { get; set; }


        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        protected override string OnRender(ICsvDefinition defintion, decimal? value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatCell(this.Format(value, culture));
        }
    }
}
