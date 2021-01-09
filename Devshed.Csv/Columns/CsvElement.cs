namespace Devshed.Csv
{
    using System;
    using System.Linq;
    using System.Diagnostics;
    using System.Linq.Expressions;
    using Devshed.Shared;
    using System.Globalization;
    using Devshed.Csv.Writing;

    /// <summary> Provides basic column behavior. </summary>
    /// <typeparam name="TSource">The selected source row type.</typeparam>
    /// <typeparam name="TResult">The selected result proprty type to be rendered.</typeparam>
    [DebuggerDisplay("{HeaderName}")]
    public abstract class CsvColumn<TSource, TResult> : ICsvColumn<TSource>
    {
        /// <summary>
        /// The property selector for columns.
        /// </summary>
        protected readonly Expression<Func<TSource, TResult>> propertySelector;

        private HeaderCollection headers;

        private Func<TSource, TResult> _selector;

        /// <summary>
        /// Returns the result type of the data type that is being mapped.
        /// </summary>
        /// <example><![CDATA[
        ///    var column = new TextCsvColumn<T>(m => m.Username);
        ///    column.ConversionResultType == typeof(string); // returns true
        /// ]]></example>
        public Type ConversionResultType
        {
            get
            {
                return typeof(TResult);
            }
        }

        /// <summary>
        /// Gets or sets the name of the header. The reflected property name by default.
        /// </summary>
        /// <value>
        /// The name of the header.
        /// </value>
        public string HeaderName
        {
            get
            {
                return this.headers.Single().Name;
            }
            set
            {
                this.headers = new HeaderCollection(new[] { value });
            }
        }

        /// <summary>
        /// Inititate a new column with a headername.
        /// </summary>
        /// <param name="propertyName"></param>
        public CsvColumn(string propertyName)
            : this(propertyName, propertyName)
        {
        }

        /// <summary>
        /// Inititate a new column with multiple headernames and one property to bind on. For example array based values.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="headers"></param>
        public CsvColumn(string propertyName, params string[] headers)
        {
            ParameterExpression argParam = Expression.Parameter(typeof(TSource), "s");
            var selectedField = Expression.PropertyOrField(argParam, propertyName);
            this.propertySelector = Expression.Lambda<Func<TSource, TResult>>(selectedField, argParam);

            this.PropertyName = propertyName;
            this.headers = new HeaderCollection(headers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector)
            : this(selector, selector.GetMemberName())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="header">The header.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector, string header)
            : this(selector, new HeaderCollection(new[] { header }))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumn{TSource, TResult}"/> class.
        /// </summary>
        /// <param name="selector">The property selector.</param>
        /// <param name="headers">The property headers.</param>
        public CsvColumn(Expression<Func<TSource, TResult>> selector, HeaderCollection headers)
        {
            Requires.IsNotNull(selector, "selector");
            Requires.IsNotNull(headers, "headers");

            this.propertySelector = selector;
            this.headers = new HeaderCollection(headers);
            this.PropertyName = selector.GetMemberName();
        }

        /// <summary>
        /// Gets the reflected name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; private set; }

        /// <summary>
        /// The CSV column data type. Example: date, number, text.
        /// </summary>
        public abstract ColumnDataType DataType { get; }

        /// <summary>
        /// The column value selector.
        /// </summary>
        public Func<TSource, TResult> Selector
        {
            get
            {
                if (this._selector == null)
                {
                    this._selector = this.propertySelector.Compile();
                }

                return this._selector;
            }
        }

        /// <summary>
        /// Gets the header names for this column.
        /// </summary>
        /// <param name="rows"> Contains the whole set of data. </param>
        /// <remarks>
        /// Multiple header names are posible for composite columns.
        /// </remarks>
        /// <returns></returns>
        public virtual HeaderCollection GetWritingHeaderNames(TSource[] rows)
        {
            return new HeaderCollection(new[] { this.HeaderName });
        }

        /// <summary>
        /// Returns the headernames required for rendering the column.
        /// </summary>
        /// <returns></returns>
        public virtual HeaderCollection GetReadingHeaderNames()
        {
            return new HeaderCollection(new[] { this.HeaderName });
        }

        /// <summary>
        /// Renders the specified element contents.
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="element">The element.</param>
        /// <param name="formattingCulture"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public virtual string[] Render(ICsvDefinition definition, TSource element, CultureInfo formattingCulture, IStringFormatter formatter)
        {
            var value = this.Selector(element);

            return new[] { this.OnRender(definition, value, formattingCulture.Parent, formatter) };
        }

        /// <summary>
        /// Executed each time the cell/value is written to a file.
        /// </summary>
        /// <param name="defintion"></param>
        /// <param name="value"></param>
        /// <param name="formattingCulture"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        protected virtual string OnRender(ICsvDefinition defintion, TResult value, CultureInfo formattingCulture, IStringFormatter formatter)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.ToString();
        }
    }
}
