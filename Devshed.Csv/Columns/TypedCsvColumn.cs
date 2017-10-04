namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public class TypedCsvColumn<TSource, TProperty> : CsvColumn<TSource, TProperty>
    {
        public TypedCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public TypedCsvColumn(Expression<Func<TSource, TProperty>> selector)
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
                return ColumnDataType.StrongTyped;
            }
        }


        public Func<TProperty, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, TProperty value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
