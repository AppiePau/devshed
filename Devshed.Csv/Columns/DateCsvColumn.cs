namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public sealed class DateCsvColumn<TSource> : CsvColumn<TSource, DateTime?>
    {
        public DateCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public DateCsvColumn(Expression<Func<TSource, DateTime?>> selector)
            : base(selector)
        {
            this.Format = date => date != null
                    ? date.Value.ToShortDateString()
                    : string.Empty;
        }

        public Func<DateTime?, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, DateTime? value)
        {
            return this.Format(value);
        }
    }

}
