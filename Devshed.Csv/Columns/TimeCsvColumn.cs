namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class TimeCsvColumn<TSource> : CsvColumn<TSource, TimeSpan?>
    {
        public TimeCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public TimeCsvColumn(Expression<Func<TSource, TimeSpan?>> selector)
            : base(selector)
        {
            this.Format = (time, culture) => time != null
            ? string.Format("{0:00}:{1:00}", culture, time.Value.Hours, time.Value.Minutes)
            : string.Empty;
        }
        
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Time;
            }
        }

        public Func<TimeSpan?, CultureInfo, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, TimeSpan? value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}