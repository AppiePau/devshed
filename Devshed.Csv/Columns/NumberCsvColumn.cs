namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>Represents a number based CSV column.</summary>
    /// <typeparam name="TSource">The type of the source property.</typeparam>
    public sealed class NumberCsvColumn<TSource> : CsvColumn<TSource, int?>
    {

        /// <summary>Initializes a new instance of the <see cref="NumberCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName">The property name of the model being bound to.</param>
        public NumberCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="NumberCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector of the binding column.</param>
        public NumberCsvColumn(Expression<Func<TSource, int?>> selector)
            : base(selector)
        {
            this.Format = (number, culture) => number != null 
                ? number.Value.ToString(culture)
                : string.Empty;
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Number;
            }
        }


        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<int?, CultureInfo, string> Format { get; set; }

        /// <summary>Executed each time the cell/value is written to a file.</summary>
        /// <param name="defintion">The CSV definition.</param>
        /// <param name="value">The value to render.</param>
        /// <param name="culture">the culture to render in.</param>
        /// <param name="formatter">The formatter to use for rendering the value into the cell.</param>
        /// <returns>A string that can be directly written into the CSV file.</returns>
        protected override string OnRender(ICsvDefinition defintion, int? value, CultureInfo culture, IStringFormatter formatter)
        {
            return this.Format(value, culture);
        }
    }
}
