namespace Devshed.Csv
{
    using System;
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
            this.Format = value => value;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Text;
            }
        }


        public bool ForceNumberToTextFormatting { get; set; }

        public Func<string, string> Format { get; set; }

        protected override string OnRender(CsvDefinition<TSource> defintion, string value)
        {
            var text = this.Format(value ?? string.Empty);

            if (this.ForceNumberToTextFormatting)
            {
                return CsvString.FormatForcedExcelStringCell(text, defintion.RemoveNewLineCharacters);
            }

            return CsvString.FormatStringCell(text, defintion.RemoveNewLineCharacters);
        }
    }
}
