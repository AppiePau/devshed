namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public sealed class BooleanCsvColumn<TSource> : CsvColumn<TSource, bool>
    {
        public BooleanCsvColumn(Expression<Func<TSource, bool>> selector)
            : base(selector)
        {
            this.Format = value => value.ToString();
        }

        public Func<bool, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, bool value)
        {
            return this.Format(value);
        }
    }
}
