namespace Devshed.Csv
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class CompositeCsvColumn<TSource, TValue> : CsvColumn<TSource, IEnumerable<CompositeColumnValue<TValue>>>
    {
        private readonly string[] headers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public CompositeCsvColumn(Expression<Func<TSource, IEnumerable<CompositeColumnValue<TValue>>>> selector,
            params string[] headers)
            : base(selector, headers)
        {
            this.headers = headers;
            this.Format = value => value.ToString();
            this.SetDefaultValueForUnknowHeaders = false;
        }

        public bool SetDefaultValueForUnknowHeaders { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public CompositeCsvColumn(Expression<Func<TSource, IEnumerable<CompositeColumnValue<TValue>>>> selector,
            IEnumerable<CompositeColumnValue<TValue>> rows)
            : this(selector, GetHeaderNames(rows))
        {
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        public Func<TValue, string> Format { get; set; }

        /// <summary>
        /// Gets the header names.
        /// </summary>
        /// <returns></returns>
        public override string[] GetHeaderNames()
        {
            return this.headers;
        }

        /// <summary>
        /// Renders the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public override string[] Render(CsvDefinition<TSource> defintion, TSource element)
        {
            var collection = this.Selector.Compile()(element);

            return this.ProcessElementsByHeaderNames(collection).ToArray();
        }

        private static string[] GetHeaderNames(IEnumerable<CompositeColumnValue<TValue>> rows)
        {
            return (from row in rows
                    group row by row.HeaderName into headers
                    select headers.Key).ToArray();
        }

        private IEnumerable<string> ProcessElementsByHeaderNames(IEnumerable<CompositeColumnValue<TValue>> collection)
        {
            foreach (var header in this.headers)
            {
                var column = collection.SingleOrDefault(c => c.HeaderName == header);
                if (column == null && !this.SetDefaultValueForUnknowHeaders)
                {
                    throw new KeyNotFoundException("The header key '" + header + "' was not found in the composite collection.");
                }
                else if (column == null && this.SetDefaultValueForUnknowHeaders)
                {
                    yield return CsvString.FormatStringCell(this.Format(default(TValue)));
                }
                else
                {
                    yield return CsvString.FormatStringCell(this.Format(column.Value));
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
