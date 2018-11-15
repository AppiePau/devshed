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
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class CompositeCsvColumn<TSource, TValue> : ColumnDefinition<TSource, IEnumerable<CompositeColumnValue<TValue>>>
    {
        private HeaderCollection headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public CompositeCsvColumn(
            Expression<Func<TSource, IEnumerable<CompositeColumnValue<TValue>>>> selector,
            HeaderCollection headers)
            : base(selector, new HeaderCollection(headers))
        {
            this.headers = new HeaderCollection(headers);
            this.Format = (value, culture) => value?.ToString() ?? string.Empty;
            this.AllowUndefinedColumnsInCollection = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public CompositeCsvColumn(
            Expression<Func<TSource, IEnumerable<CompositeColumnValue<TValue>>>> selector,
            IEnumerable<CompositeColumnValue<TValue>> rows)
            : this(selector, GetHeaderNames(rows))
        {
        }

        public CompositeCsvColumn(Expression<Func<TSource, IEnumerable<CompositeColumnValue<TValue>>>> selector, params string[] headers)
        : this(selector, new HeaderCollection(headers))
        {
        }

        /// <summary>
        /// If false an exception will be thrown when a header is not found the collection.
        /// </summary>
        public bool AllowUndefinedColumnsInCollection { get; set; }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Composite;
            }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public Func<TValue, CultureInfo, string> Format { get; set; }

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
        /// Renders the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public override string[] Render(ICsvDefinition defintion, TSource element, CultureInfo culture, IStringFormatter formatter)
        {
            var collection = this.Selector(element);

            return this.ProcessElementsByHeaderNames(collection, culture, formatter).ToArray();
        }

        private static HeaderCollection GetHeaderNames(IEnumerable<CompositeColumnValue<TValue>> rows)
        {
            return new HeaderCollection(
                (from row in rows
                 group row by row.HeaderName into headers
                 select headers.Key).ToArray());
        }

        private IEnumerable<string> ProcessElementsByHeaderNames(IEnumerable<CompositeColumnValue<TValue>> collection, CultureInfo culture, IStringFormatter formatter)
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
                    yield return formatter.FormatStringCell(this.Format(default(TValue), culture));
                }
                else
                {
                    yield return formatter.FormatStringCell(this.Format(column.Value, culture));
                }
            }
        }
    }

    /// <summary> Holds the values for the composite columns. </summary>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [DebuggerDisplay("{HeaderName}: {Value}")]
    public sealed class CompositeColumnValue<TValue>
    {
        public CompositeColumnValue(string header, TValue value)
        {
            // TODO: Complete member initialization
            this.HeaderName = header;
            this.Value = value;
        }
        public string HeaderName { get; private set; }

        public TValue Value { get; private set; }
    }
}
