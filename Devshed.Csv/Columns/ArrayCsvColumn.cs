namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ArrayCsvColumn<TSource, TArray> : CsvColumn<TSource, IEnumerable<TArray>>
    {
        public ArrayCsvColumn(Expression<Func<TSource, IEnumerable<TArray>>> selector)
            : base(selector)
        {
            this.Format = value => value.ToString();
        }

        public Func<TArray, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, IEnumerable<TArray> value)
        {
            var values = value.Select(e => this.Format(e).Replace(',', '_')).ToArray();

            var element = string.Join(",", values);

            return element;
        }
    }
}
