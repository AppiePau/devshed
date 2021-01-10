namespace Devshed.Csv
{
    using Devshed.Csv.Writing;
    using System;
    using System.Globalization;
    using System.Linq.Expressions;

    public sealed class TimeCsvColumn<TSource> : ColumnDefinition<TSource, TimeSpan?>
    {
        public TimeCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public TimeCsvColumn(Expression<Func<TSource, TimeSpan?>> selector)
            : base(selector)
        {
            this.Format = (time, culture) => time != null
            ? string.Format(culture, "{0:00}:{1:00}", time.Value.Hours, time.Value.Minutes)
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

        protected override string OnRender(ICsvDefinition defintion, TimeSpan? value, CultureInfo culture, IStringFormatter formatter)
        {
            return this.Format(value, culture);
        }
    }
}