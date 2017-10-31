using System;
using System.Collections.Generic;
using System.Globalization;

namespace Devshed.Csv
{
    public interface ICsvColumn<TSource>
    {
        string PropertyName { get; }

        Type ConversionResultType { get; }

        ColumnDataType DataType { get; }

        string[] Render(ICsvDefinition defintion, TSource element, CultureInfo writingFormat);

        string[] GetWritingHeaderNames(TSource[] rows);

        string[] GetReadingHeaderNames();
    }
}