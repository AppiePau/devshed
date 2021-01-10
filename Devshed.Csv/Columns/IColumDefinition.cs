using Devshed.Csv.Writing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Devshed.Csv
{
    public interface IColumDefinition<TSource>
    {
        string PropertyName { get; }

        Type ConversionResultType { get; }

        ColumnDataType DataType { get; }

        string[] Render(ICsvDefinition defintion, TSource element, CultureInfo formatCulture, IStringFormatter formatter);

        HeaderCollection GetWritingHeaderNames(TSource[] rows);

        HeaderCollection GetReadingHeaderNames();
    }

    public interface IColumnValueProvider
    {
        object GetValue();
    }

    public sealed class CsvColumnRawValue : IColumnValueProvider
    {
        private readonly object value;

        public CsvColumnRawValue(Func<object> value, Func<string> formatted)
        {
            this.value = value;
        }

        public object GetValue()
        {
            return this.value;
        }
    }

    public sealed class CsvColumnTextValue : CsvColumnValue, IColumnValueProvider
    {
        private readonly string value;
        private readonly bool removeEnters;

        public CsvColumnTextValue(string value, bool removeEnters = false)
        {
            this.value = value;
            this.removeEnters = removeEnters;
        }

        public object GetValue()
        {
            return base.FormatTextCell(this.value, this.removeEnters);
        }
    }


    public sealed class CsvColumnForcedTextValue : CsvColumnValue, IColumnValueProvider
    {
        private readonly string value;
        private readonly bool removeEnters;

        public CsvColumnForcedTextValue(string value, bool removeEnters = false)
        {
            this.value = value;
            this.removeEnters = removeEnters;
        }

        public object GetValue()
        {
            return base.FormatForcedTextCell(this.value, this.removeEnters);
        }
    }

    public abstract class CsvColumnValue
    {
        protected string FormatForcedTextCell(string value, bool removeEnters)
        {
            return string.Format("={0}", FormatTextCell(value, removeEnters));
        }

        protected string FormatTextCell(string value, bool removeEnters)
        {
            var sb = new StringBuilder(value);
            sb.Replace("\"", "\"\"");

            if (removeEnters)
            {
                sb.Replace("\r\n", "");
                sb.Replace("\n", "");
            }

            sb.Insert(0, "\"");
            sb.Append("\"");

            return sb.ToString();
        }
    }
}