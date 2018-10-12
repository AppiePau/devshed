namespace Devshed.Csv
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using Devshed.Csv.Writing;
    using Shared;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class DynamicCsvColumn<TSource, TValue> : CsvColumn<TSource, IEnumerable<TValue>>
    {
        private string[] headers;
        private readonly Func<TValue, ICsvColumn<TValue>> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public DynamicCsvColumn(Expression<Func<TSource, IEnumerable<TValue>>> selector,
            Func<TValue, ICsvColumn<TValue>> converter,
            params string[] headers)
            : base(selector, headers)
        {
            this.headers = headers;
            this.converter = converter;
            this.AllowUndefinedColumnsInCollection = false;
        }

        /// <summary>
        /// If false an exception will be thrown when a header is not found the collection.
        /// </summary>
        public bool AllowUndefinedColumnsInCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public DynamicCsvColumn(Expression<Func<TSource, IEnumerable<TValue>>> selector,
            IEnumerable<TValue> rows,
            Func<TValue, ICsvColumn<TValue>> converter)
            : this(selector, converter, GetHeaderNames(rows, converter))
        {
            //selector = selector;
        }

        public override ColumnDataType DataType
        {
            get
            {
                return ColumnDataType.Dynamic;
            }
        }


        /// <summary>
        /// Gets the header names.
        /// </summary>
        /// <returns></returns>
        public override string[] GetWritingHeaderNames(TSource[] rows)
        {
            if (this.headers.Length == 0)
            {
                var headers =
                    (from row in rows
                     from colrow in Selector(row)
                     let sub = converter(colrow)
                     from header in sub.GetReadingHeaderNames()
                     select header);

                var collection = new HashSet<string>();

                collection.AddRange(headers);

                return collection.ToArray();
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
            
            return (from item in collection
                    let column = converter(item)
                    select column.Render(defintion, item, culture, formatter))
                    .SelectMany(elements => elements)
                    .ToArray();
        }

        private static string[] GetHeaderNames(IEnumerable<TValue> rows, Func<TValue, ICsvColumn<TValue>> converter)
        {
            return (from row in rows
                    let sub = converter(row)
                    from header in sub.GetReadingHeaderNames()
                    group header by header into name
                    orderby name.Key
                    select name.Key).ToArray(); ;
        }
    }
}
