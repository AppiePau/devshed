namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>Represents a object/anonymous type based CSV column.</summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public class ObjectCsvColumn<TSource> : CsvColumn<TSource, object>
    {
        /// <summary>Initializes a new instance of the <see cref="ObjectCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public ObjectCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="ObjectCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public ObjectCsvColumn(Expression<Func<TSource, object>> selector)
            : base(selector)
        {
            this.Format = (value, culture) => value != null
                ? value.ToString()
                : string.Empty;
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Object;
            }
        }

        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<object, CultureInfo, string> Format { get; set; }

        /// <summary>Executed each time the cell/value is written to a file.</summary>
        /// <param name="defintion">The CSV definition.</param>
        /// <param name="value">The value to render.</param>
        /// <param name="culture">the culture to render in.</param>
        /// <param name="formatter">The formatter to use for rendering the value into the cell.</param>
        /// <returns>A string that can be directly written into the CSV file.</returns>
        protected override object OnRender(ICsvDefinition defintion, object value, CultureInfo culture, IStringFormatter formatter)
        {
            return this.Format(value, culture);
        }
    }
}
