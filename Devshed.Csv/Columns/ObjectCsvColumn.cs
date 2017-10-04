namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public class ObjectCsvColumn<TSource> : CsvColumn<TSource, object>
    {
        public ObjectCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public ObjectCsvColumn(Expression<Func<TSource, object>> selector)
            : base(selector)
        {
            this.Format = (value, culture) => value != null
                ? value.ToString()
                : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Object;
            }
        }

        public Func<object, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, object value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
