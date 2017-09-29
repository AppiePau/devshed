using System;

namespace Devshed.Csv
{
    public interface ICsvColumn<TSource>
    {
        string PropertyName { get; }

        Type ConversionResultType { get; }

        ColumnDataType DataType { get; }

        string[] Render(CsvDefinition<TSource> defintion, TSource element);

        string[] GetHeaderNames();
    }
}