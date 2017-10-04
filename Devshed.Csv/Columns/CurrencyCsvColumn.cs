namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    public sealed class CurrencyCsvColumn<TSource> : CsvColumn<TSource, decimal?>
    {
        public CurrencyCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public CurrencyCsvColumn(Expression<Func<TSource, decimal?>> selector)
            : base(selector)
        {
            this.Format = (value, culture) => value != null
                ? string.Format(culture, "{0:c2}", value)
                : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Currency;
            }
        }

        public Func<decimal?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, decimal? value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
