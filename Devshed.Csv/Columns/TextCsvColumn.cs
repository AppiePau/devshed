namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;

    /// <summary>Represents a text based CSV column.</summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    public sealed class TextCsvColumn<TSource> : CsvColumn<TSource, string>
    {
        /// <summary>Initializes a new instance of the <see cref="TextCsvColumn{TSource}" /> class.</summary>
        /// <param name="propertyName"></param>
        public TextCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="TextCsvColumn{TSource}" /> class.</summary>
        /// <param name="selector">The selector.</param>
        public TextCsvColumn(Expression<Func<TSource, string>> selector)
            : base(selector)
        {
            this.ForceNumberToTextFormatting = false;
            this.Format = (value, culture) => value.ToString(culture);
        }

        /// <summary>The CSV column data type. Example: date, number, text.</summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Text;
            }
        }


        /// <summary>Gets or sets a value indicating whether to force number to text formatting.</summary>
        /// <value>
        ///   <c>true</c> if [force number to text formatting]; otherwise, <c>false</c>.</value>
        public bool ForceNumberToTextFormatting { get; set; }

        /// <summary>Gets or sets the formatting method.</summary>
        /// <value>The format.</value>
        public override  Func<string, CultureInfo, string> Format { get; set; }

        /// <summary>Executed each time the cell/value is written to a file.</summary>
        /// <param name="defintion">The CSV definition.</param>
        /// <param name="value">The value to render.</param>
        /// <param name="culture">the culture to render in.</param>
        /// <param name="formatter">The formatter to use for rendering the value into the cell.</param>
        /// <returns>A string that can be directly written into the CSV file.</returns>
        protected override object OnRender(ICsvDefinition defintion, string value, CultureInfo culture, IStringFormatter formatter)
        {
            var text = this.Format(value ?? string.Empty, culture);

            if (this.ForceNumberToTextFormatting)
            {
                return formatter.FormatForcedExcelStringCell(text, defintion.RemoveNewLineCharacters);
            }

            return formatter.FormatStringCell(text, defintion.RemoveNewLineCharacters);
        }
    }
}
