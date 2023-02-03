namespace Devshed.Csv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;

    /// <summary>
    /// Represents a composite based CSV column. Having multiple columns.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="decimal">The type of the value.</typeparam>
    public class CompositeNumberCsvColumn<TSource> : CsvColumn<TSource, IEnumerable<CompositeColumnValue<decimal>>>
    {
        private HeaderCollection headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeNumberCsvColumn{TSource, decimal}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public CompositeNumberCsvColumn(
            Expression<Func<TSource, IEnumerable<CompositeColumnValue<decimal>>>> selector,
            HeaderCollection headers)
            : base(selector, new HeaderCollection(headers))
        {
            this.headers = new HeaderCollection(headers);
            this.AllowUndefinedColumnsInCollection = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeNumberCsvColumn{TSource, decimal}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="rows">The rows.</param>
        public CompositeNumberCsvColumn(
            Expression<Func<TSource, IEnumerable<CompositeColumnValue<decimal>>>> selector,
            IEnumerable<CompositeColumnValue<decimal>> rows)
            : this(selector, GetHeaderNames(rows))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector">The selector for values. </param>
        /// <param name="headers"> The headers corresponing to the value within the column. </param>
        public CompositeNumberCsvColumn(Expression<Func<TSource, IEnumerable<CompositeColumnValue<decimal>>>> selector, params string[] headers)
        : this(selector, new HeaderCollection(headers))
        {
        }

        /// <summary>
        /// If false an exception will be thrown when a header is not found the collection.
        /// </summary>
        public bool AllowUndefinedColumnsInCollection { get; set; }

        /// <summary>
        /// The data type of the column.
        /// </summary>
        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Composite;
            }
        }

        /// <summary>
        /// Gets the header names.
        /// </summary>
        /// <returns></returns>
        public override HeaderCollection GetWritingHeaderNames(TSource[] rows)
        {
            if (this.headers.Length == 0)
            {

                this.headers = new HeaderCollection(
                    (from row in rows
                     from col in Selector(row)
                     group col by col.HeaderName into name
                     orderby name.Key
                     select name.Key).ToArray());

            }

            return this.headers;
        }

        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"> The CSV definition. </param>
        /// <param name="value"> The value to render. </param>
        /// <param name="culture"> the culture to render in. </param>
        /// <param name="formatter"> The formatter to use for rendering the value into the cell. </param>
        /// <returns>A string that can be directly written into the CSV file. </returns>
        public override object[] Render(ICsvDefinition defintion, TSource value, CultureInfo culture, IStringFormatter formatter)
        {
            var collection = this.Selector(value);

            return this.ProcessElementsByHeaderNames(collection, culture, formatter).ToArray();
        }

        private static HeaderCollection GetHeaderNames(IEnumerable<CompositeColumnValue<decimal>> rows)
        {
            return new HeaderCollection(
                (from row in rows
                 group row by row.HeaderName into headers
                 select headers.Key).ToArray());
        }

        private IEnumerable<object> ProcessElementsByHeaderNames(IEnumerable<CompositeColumnValue<decimal>> collection, CultureInfo culture, IStringFormatter formatter)
        {
            if (this.headers.Length == 0)
            {
                this.headers = GetHeaderNames(collection);
            }

            foreach (var header in this.headers)
            {
                var column = collection.SingleOrDefault(c => c.HeaderName == header.Name);
                if (column == null && !this.AllowUndefinedColumnsInCollection)
                {
                    throw new KeyNotFoundException("The header key '" + header + "' was not found in the composite collection.");
                }
                else if (column == null && this.AllowUndefinedColumnsInCollection)
                {
                    yield return (decimal?)null;
                }
                else
                {
                    yield return column.Value;
                }
            }
        }
    }

}
