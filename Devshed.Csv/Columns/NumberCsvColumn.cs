namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class NumberCsvColumn<TSource> : CsvColumn<TSource, int?>
    {
        public NumberCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public NumberCsvColumn(Expression<Func<TSource, int?>> selector)
            : base(selector)
        {
            this.Format = (number, culture) => number != null 
                ? number.Value.ToString(culture)
                : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Number;
            }
        }


        public Func<int?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, int? value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
