namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    /// <summary>Represents a strong typed based CSV column.</summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    public class TypedCsvColumn<TSource, TProperty> : ColumnDefinition<TSource, TProperty>
    {
        /// <summary>Initializes a new instance of the <see cref="TypedCsvColumn{TSource, TProperty}" /> class.</summary>
        /// <param name="propertyName"></param>
        public TypedCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TypedCsvColumn{TSource, TProperty}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public TypedCsvColumn(Expression<Func<TSource, TProperty>> selector)
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
                return ColumnDataType.StrongTyped;
            }
        }

        /// <summary>Gets or sets the formatting method.</summary>
        /// <value>The format.</value>
        public Func<TProperty, CultureInfo, string> Format { get; set; }

        /// <summary>Executed each time the cell/value is written to a file.</summary>
        /// <param name="defintion">The CSV definition.</param>
        /// <param name="value">The value to render.</param>
        /// <param name="culture">the culture to render in.</param>
        /// <param name="formatter">The formatter to use for rendering the value into the cell.</param>
        /// <returns>A string that can be directly written into the CSV file.</returns>
        protected override string OnRender(ICsvDefinition defintion, TProperty value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatStringCell(this.Format(value, culture));
        }
    }
}
