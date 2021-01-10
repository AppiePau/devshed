namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class NumberCsvColumn<TSource> : ColumnDefinition<TSource, int?>
    {
        public NumberCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public NumberCsvColumn(Expression<Func<TSource, int?>> selector)
            : base(selector)
        {
            this.Format = (number, culture) => number != null 
                ? number.Value.ToString(culture)
                : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Number;
            }
        }


        public Func<int?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(ICsvDefinition defintion, int? value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatCell(this.Format(value, culture));
        }
    }
}
