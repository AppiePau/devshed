namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;
    using Devshed.Shared;
    using System.Globalization;

    public class ArrayCsvColumn<TSource, TArray> : ColumnDefinition<TSource, IEnumerable<TArray>>
    {
        private string elementDelimiter;

        public ArrayCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public ArrayCsvColumn(Expression<Func<TSource, IEnumerable<TArray>>> selector)
            : base(selector)
        {
            this.ElementDelimiter = ",";
            this.Format = value => value.ToString();
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Text;
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

        public Func<TArray, string> Format { get; set; }

        protected override IColumnValueProvider OnRender(ICsvDefinition defintion, IEnumerable<TArray> value, CultureInfo culture, IStringFormatter formatter)
        {
            var values = value.Select(e => CleanAndFormatValue(defintion, e)).ToArray();

            var element = string.Join(this.ElementDelimiter, values);

            return new CsvColumnTextValue(element, defintion.RemoveNewLineCharacters);
        }

        private string CleanAndFormatValue(ICsvDefinition defintion, TArray e)
        {
            return this.Format(e).Replace(this.ElementDelimiter, "_");
        }
    }
}
