namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>Represents a boolean based CSV column.</summary>
    /// <typeparam name="TSource"></typeparam>
    public sealed class BooleanCsvColumn<TSource> : CsvColumn<TSource, bool>
    {
        /// <summary>Initializes a new instance of the <see cref="BooleanCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public BooleanCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="BooleanCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public BooleanCsvColumn(Expression<Func<TSource, bool>> selector)
            : base(selector)
        {
            this.Format = (value, culture) => value.ToString(culture);
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Boolean;
            }
        }

        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<bool, CultureInfo, string> Format { get; set; }

        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        protected override string OnRender(ICsvDefinition defintion, bool value, CultureInfo culture, IStringFormatter formatter)
        {
            return this.Format(value, culture);
        }
    }
}
