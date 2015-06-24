namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public sealed class NumberCsvColumn<TSource> : CsvColumn<TSource, int?>
    {
        public NumberCsvColumn(Expression<Func<TSource, int?>> selector)
            : base(selector)
        {
            this.Format = number => number != null 
                ? number.Value.ToString()
                : string.Empty;
        }

        public Func<int?, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, int? value)
        {
            return value.ToString();
        }
    }
}
