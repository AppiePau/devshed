namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public class TypedCsvColumn<TSource, TProperty> : CsvColumn<TSource, TProperty>
    {
        public TypedCsvColumn(Expression<Func<TSource, TProperty>> selector)
            : base(selector)
        {
            this.Format = value => value != null
                ? value.ToString()
                : string.Empty;
        }

        public Func<TProperty, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, TProperty value)
        {
            return this.Format(value);
        }
    }
}
