namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    public sealed class DecimalCsvColumn<TSource> : CsvColumn<TSource, decimal?>
    {
        public DecimalCsvColumn(string propertyName)
            : base(propertyName)
        {
        }


        public DecimalCsvColumn(Expression<Func<TSource, decimal?>> selector)
            : base(selector)
        {
            this.Format = (number, formatter) => number != null
                ? number.Value.ToString(formatter)
                : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Decimal;
            }
        }


        public Func<decimal?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, decimal? value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
