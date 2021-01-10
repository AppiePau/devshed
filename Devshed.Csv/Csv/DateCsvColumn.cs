namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class DateCsvColumn<TSource> : ColumnDefinition<TSource, DateTime?>
    {
        public DateCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public DateCsvColumn(Expression<Func<TSource, DateTime?>> selector)
            : base(selector)
        {
            this.Format = (date, culture) => date != null
                    ? date.Value.ToShortDateString()
                    : string.Empty;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.DateTime;
            }
        }

        public Func<DateTime?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(ICsvDefinition defintion, DateTime? value, CultureInfo culture, IStringFormatter formatter)
        {
            return formatter.FormatCell(this.Format(value, culture));
        }
    }

}
