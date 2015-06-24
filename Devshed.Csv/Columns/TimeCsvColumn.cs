namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public sealed class TimeCsvColumn<TSource> : CsvColumn<TSource, TimeSpan?>
    {
        public TimeCsvColumn(Expression<Func<TSource, TimeSpan?>> selector)
            : base(selector)
        {
            this.Format = time => time != null
            ? string.Format("{0:00}:{1:00}", time.Value.Hours, time.Value.Minutes)
            : string.Empty;
        }

        public Func<TimeSpan?, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, TimeSpan? value)
        {
            return this.Format(value);
        }
    }
}