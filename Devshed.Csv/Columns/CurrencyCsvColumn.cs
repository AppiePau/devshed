namespace Devshed.Csv.Columns
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    public sealed class CurrencyCsvColumn<TSource> : CsvColumn<TSource, decimal?>
    {
        private readonly CultureInfo culture;

        public CurrencyCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public CurrencyCsvColumn(Expression<Func<TSource, decimal?>> selector, CultureInfo culture)
            : base(selector)
        {
            this.culture = culture;
        }

        public CurrencyCsvColumn(Expression<Func<TSource, decimal?>> selector)
            : this(selector, Thread.CurrentThread.CurrentUICulture)
        {
            this.Format = value => value != null
                ? string.Format(this.culture, "{0:c2}", value)
                : string.Empty;
        }

        public Func<decimal?, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, decimal? value)
        {
            return this.Format(value);
        }
    }
}
