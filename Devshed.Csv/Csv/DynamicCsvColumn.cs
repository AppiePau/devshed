namespace Devshed.Csv
{
    using Devshed.Csv.Reading;
    using Devshed.Csv.Writing;
    using Shared;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class DynamicCsvColumn<TSource, TValue> : ColumnDefinition<TSource, IEnumerable<TValue>>
    {
        private HeaderCollection headers;

        private readonly Func<TValue, IColumDefinition<TValue>> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="headers">The headers.</param>
        public DynamicCsvColumn(
            Expression<Func<TSource, IEnumerable<TValue>>> selector,
            Func<TValue, IColumDefinition<TValue>> converter,
            HeaderCollection headers)
            : base(selector, headers)
        {
            this.headers = new HeaderCollection(headers);
            this.converter = converter;
            this.AllowUndefinedColumnsInCollection = false;
        }
        public DynamicCsvColumn(
            Expression<Func<TSource, IEnumerable<TValue>>> selector,
           Func<TValue, IColumDefinition<TValue>> converter,
           params string[] headers)
           : this(selector, converter, new HeaderCollection(headers))
        {
        }
        /// <summary>
        /// If false an exception will be thrown when a header is not found the collection.
        /// </summary>
        public bool AllowUndefinedColumnsInCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCsvColumn{TSource, TValue}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="rows">The rows.</param>
        /// <param name="converter">The value converter.</param>
        public DynamicCsvColumn(Expression<Func<TSource, IEnumerable<TValue>>> selector,
            IEnumerable<TValue> rows,
            Func<TValue, IColumDefinition<TValue>> converter)
            : this(selector, converter, GetHeaderNames(rows, converter))
        {
            //selector = selector;
        }

        /// <summary>
        /// The data type of the column.
        /// </summary>
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
        public override HeaderCollection GetWritingHeaderNames(TSource[] rows)
        {
            if (this.headers.Length == 0)
            {
                var headers =
                    (from row in rows
                     from colrow in Selector(row)
                     let sub = converter(colrow)
                     from header in sub.GetReadingHeaderNames()
                     select header.Name);

                var collection = new HashSet<string>();

                collection.AddRange(headers);

                return new HeaderCollection(collection.ToArray());
            }

            return this.headers;
        }

        /// <summary>
        /// Renders the specified element.
        /// </summary>
        /// <param name="defintion">The CSV defintion.</param>
        /// <param name="element">The rendered element.</param>
        /// <param name="culture">The culture. </param>
        /// <param name="formatter">The formatter. </param>
        /// <returns></returns>
        public override string[] Render(ICsvDefinition defintion, TSource element, CultureInfo culture, IStringFormatter formatter)
        {
            var collection = this.Selector(element);

            return (from item in collection
                    let column = converter(item)
                    select column.Render(defintion, item, culture, formatter))
                    .SelectMany(elements => elements.Select(e => e.ToString()))
                    .ToArray();
        }

        private static HeaderCollection GetHeaderNames(IEnumerable<TValue> rows, Func<TValue, IColumDefinition<TValue>> converter)
        {
            return new HeaderCollection(
                (from row in rows
                 let sub = converter(row)
                 from header in sub.GetReadingHeaderNames()
                 group header by header.Name into name
                 orderby name.Key
                 select name.Key).ToArray());
        }
    }
}
