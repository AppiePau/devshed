namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Threading;

    public sealed class DecimalCsvColumn<TSource> : CsvColumn<TSource, decimal?>
    {
        private readonly CultureInfo culture;

        public DecimalCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public DecimalCsvColumn(Expression<Func<TSource, decimal?>> selector, CultureInfo culture)
            : base(selector)
        {
            this.culture = culture;
        }

        public DecimalCsvColumn(Expression<Func<TSource, decimal?>> selector)
            : this(selector, Thread.CurrentThread.CurrentUICulture)
        {
            this.Format = (number, formatter) => number != null
                ? number.Value.ToString(formatter)
                : string.Empty;
        }

        public Func<decimal?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, decimal? value)
        {
            return this.Format(value, this.culture);
        }
    }
}
