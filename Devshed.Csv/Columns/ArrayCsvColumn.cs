namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;
    using Devshed.Shared;
    using System.Globalization;

    /// <summary>
    /// Represents an array based CSV column.
    /// </summary>
    /// <typeparam name="TSource"> The source of the mapping. </typeparam>
    /// <typeparam name="TArray"> The type of elements in the array. </typeparam>
    public class ArrayCsvColumn<TSource, TArray> : CsvColumn<TSource, IEnumerable<TArray>>
    {
        private string elementDelimiter;

        /// <summary>
        /// Initialize the array column definition.
        /// </summary>
        /// <param name="propertyName"> The name of the property bound to. </param>
        public ArrayCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        /// <summary>
        /// Initialize the array column definition.
        /// </summary>
        /// <param name="selector"> The expression mapping the property. </param>
        public ArrayCsvColumn(Expression<Func<TSource, IEnumerable<TArray>>> selector)
            : base(selector)
        {
            this.ElementDelimiter = ",";
            this.Format = value => value.ToString();
        }

        /// <summary>
        /// The data type of the column.
        /// </summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Currency;
            }
        }

        /// <summary> Specifies the delimiter between the array element, a comma by default.
        /// Do not change unless needed, a comma is always needed for reading. </summary>
        public string ElementDelimiter
        {
            get
            {
                return this.elementDelimiter;
            }
            set
            {
                Requires.IsNotNull("ElementDelimiter", value);
                this.elementDelimiter = value;
            }
        }

        /// <summary>
        /// The formatting function for rendering the value.
        /// </summary>
        public Func<TArray, string> Format { get; set; }

        /// <summary>
        /// Renders the value of the column.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        protected override string OnRender(ICsvDefinition defintion, IEnumerable<TArray> value, CultureInfo culture, IStringFormatter formatter)
        {
            var values = value.Select(e => CleanAndFormatValue(defintion, e)).ToArray();

            var element = string.Join(this.ElementDelimiter, values);

            return formatter.FormatStringCell(element, defintion.RemoveNewLineCharacters);
        }

        private string CleanAndFormatValue(ICsvDefinition defintion, TArray e)
        {
            return this.Format(e).Replace(this.ElementDelimiter, "_");
        }
    }
}
