namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    /// <summary>
    ///   <br />
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class DecimalCsvColumn<TSource> : CsvColumn<TSource, decimal?>
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
        public override Func<decimal?, CultureInfo, string> Format { get; set; }


        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        protected override object OnRender(ICsvDefinition defintion, decimal? value)
        {
            return value;
        }
    }
}
