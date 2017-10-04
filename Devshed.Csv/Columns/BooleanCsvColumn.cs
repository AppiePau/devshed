﻿namespace Devshed.Csv
{
    using System;
    using System.Globalization;
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

        protected override string OnRender(CsvDefinition<TSource> defintion, bool value, CultureInfo culture)
        {
            return this.Format(value, culture);
        }
    }
}
