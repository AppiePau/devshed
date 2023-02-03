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
    /// <typeparam name="TResult"></typeparam>
    public interface ICsvColumn<TSource, TResult>
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
        /// Formats the value for string (plain) text files.
        /// </summary>
        Func<TResult, CultureInfo, string> Format { get; }


        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        object[] Render(ICsvDefinition defintion, TSource value);

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