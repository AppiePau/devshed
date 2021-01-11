using Devshed.Csv.Writing;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Devshed.Csv
{
    /// <summary>
    /// Defines an abstract representation of a CSV column.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface ICsvColumn<TSource>
    {
        /// <summary>
        /// The property name that the column is bound to on the model.
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// The type that is being converted from and to.
        /// </summary>
        Type ConversionResultType { get; }

        /// <summary>
        /// The CSV data type converted from and to.
        /// </summary>
        ColumnDataType DataType { get; }

        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        string[] Render(ICsvDefinition defintion, TSource value, CultureInfo culture, IStringFormatter formatter);

        /// <summary>
        /// Gets the header names (multiple in the case of a composite column).
        /// </summary>
        /// <param name="rows"></param>
        /// <returns></returns>
        HeaderCollection GetWritingHeaderNames(TSource[] rows);

        /// <summary>
        /// Gets the header names (multiple in the case of a composite column).
        /// </summary>
        /// <returns></returns>
        HeaderCollection GetReadingHeaderNames();
    }
}