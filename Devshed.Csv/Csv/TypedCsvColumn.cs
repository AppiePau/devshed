namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public class TypedCsvColumn<TSource, TProperty> : ColumnDefinition<TSource, TProperty>
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

        protected override string OnRender(ICsvDefinition defintion, TProperty value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatStringCell(this.Format(value, culture));
        }
    }
}
