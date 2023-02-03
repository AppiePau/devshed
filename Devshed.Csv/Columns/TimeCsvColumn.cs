namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>Represents a time based CSV column.</summary>
    /// <typeparam name="TSource">The type of the mapping source.</typeparam>
    public sealed class TimeCsvColumn<TSource> : CsvColumn<TSource, TimeSpan?>
    {
        /// <summary>Initializes a new instance of the <see cref="TimeCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public TimeCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TimeCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public TimeCsvColumn(Expression<Func<TSource, TimeSpan?>> selector)
            : base(selector)
        {
            this.Format = (time, culture) => time != null
            ? string.Format(culture, "{0:00}:{1:00}", time.Value.Hours, time.Value.Minutes)
            : string.Empty;
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Time;
            }
        }

        /// <summary>Gets or sets the formatting method.</summary>
        /// <value>The format.</value>
        public Func<TimeSpan?, CultureInfo, string> Format { get; set; }

        /// <summary>Executed each time the cell/value is written to a file.</summary>
        /// <param name="defintion">The CSV definition.</param>
        /// <param name="value">The value to render.</param>
        /// <param name="culture">the culture to render in.</param>
        /// <param name="formatter">The formatter to use for rendering the value into the cell.</param>
        /// <returns>A string that can be directly written into the CSV file.</returns>
        protected override object OnRender(ICsvDefinition defintion, TimeSpan? value, CultureInfo culture, IStringFormatter formatter)
        {
            return this.Format(value, culture);
        }
    }
}