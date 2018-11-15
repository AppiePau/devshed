namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class BooleanCsvColumn<TSource> : ColumnDefinition<TSource, bool>
    {
        public BooleanCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public BooleanCsvColumn(Expression<Func<TSource, bool>> selector)
            : base(selector)
        {
            this.Format = (value, culture) => value.ToString(culture);
        }
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Boolean;
            }
        }

        public Func<bool, CultureInfo, string> Format { get; set; }

        protected override IColumnValueProvider OnRender(ICsvDefinition defintion, bool value, CultureInfo culture, IStringFormatter formatter)
        {
            return new CsvColumnTextValue(this.Format(value, culture));
        }
    }
}
