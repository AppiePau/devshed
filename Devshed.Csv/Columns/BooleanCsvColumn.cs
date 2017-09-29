namespace Devshed.Csv
{
    using System;
    using System.Linq.Expressions;

    public sealed class BooleanCsvColumn<TSource> : CsvColumn<TSource, bool>
    {
        public BooleanCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public BooleanCsvColumn(Expression<Func<TSource, bool>> selector)
            : base(selector)
        {
            this.Format = value => value.ToString();
        }
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Boolean;
            }
        }

        public Func<bool, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, bool value)
        {
            return this.Format(value);
        }
    }
}
