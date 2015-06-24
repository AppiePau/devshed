namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public class ObjectCsvColumn<TSource> : CsvColumn<TSource, object>
    {
        public ObjectCsvColumn(Expression<Func<TSource, object>> selector)
            : base(selector)
        {
            this.Format = value => value != null
                ? value.ToString()
                : string.Empty;
        }

        public Func<object, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, object value)
        {
            return this.Format(value);
        }
    }
}
