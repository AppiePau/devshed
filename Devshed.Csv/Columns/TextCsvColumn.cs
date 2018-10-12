namespace Devshed.Csv
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;

    public sealed class TextCsvColumn<TSource> : CsvColumn<TSource, string>
    {
        public TextCsvColumn(string propertyName)
            : base(propertyName)
        {
        }

        public TextCsvColumn(Expression<Func<TSource, string>> selector)
            : base(selector)
        {
            this.ForceNumberToTextFormatting = false;
            this.Format = (value, culture) => value.ToString(culture);
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Text;
            }
        }


        public bool ForceNumberToTextFormatting { get; set; }

        public Func<string, CultureInfo, string> Format { get; set; }

        protected override string OnRender(ICsvDefinition defintion, string value, CultureInfo culture, IStringFormatter formatter)
        {
            var text = this.Format(value ?? string.Empty, culture);

            if (this.ForceNumberToTextFormatting)
            {
                return formatter.FormatForcedExcelStringCell(text, defintion.RemoveNewLineCharacters);
            }

            return formatter.FormatStringCell(text, defintion.RemoveNewLineCharacters);
        }
    }
}
